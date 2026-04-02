using System;
using System.Collections.Generic;
using System.Linq;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;

namespace Yapoml.Playwright.Services;

/// <summary>
/// Builds navigation URIs by combining base URLs, path segments, and query parameters.
/// </summary>
public class NavigationService
{
    readonly ISpaceOptions _spaceOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="spaceOptions">The space options containing the base URL configuration.</param>
    public NavigationService(ISpaceOptions spaceOptions)
    {
        _spaceOptions = spaceOptions;
    }

    /// <summary>
    /// Builds a URI by resolving a URL against the base URL, substituting path segments, and appending query parameters.
    /// </summary>
    /// <param name="url">The URL, which may be relative to the registered base URL.</param>
    /// <param name="segments">Path segment placeholders and their replacement values.</param>
    /// <param name="queryParams">Query string parameters to append to the URL.</param>
    /// <returns>The fully constructed <see cref="Uri"/>.</returns>
    public Uri BuildUri(string url, IList<KeyValuePair<string, string>> segments, IList<KeyValuePair<string, string>> queryParams)
    {
        url = new SegmentService().Replace(url, segments);

        UriBuilder urlBuilder;

        if (Uri.IsWellFormedUriString(url, UriKind.Relative))
        {
            var baseUrl = _spaceOptions.Services.Get<BaseUrlOptions>();

            urlBuilder = new UriBuilder(new Uri(new Uri(baseUrl.Url), url));
        }
        else
        {
            urlBuilder = new UriBuilder(url);
        }

        if (queryParams != null && queryParams.Count > 0)
        {
            urlBuilder.Query = string.Join("&", queryParams.Where(qp => qp.Value is not null).Select(qp => $"{qp.Key}={qp.Value}"));
        }

        return urlBuilder.Uri;
    }
}
