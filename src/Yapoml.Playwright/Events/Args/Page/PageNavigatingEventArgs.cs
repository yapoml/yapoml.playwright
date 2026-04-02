using System;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Page;

/// <summary>
/// Event arguments for the page navigating event, containing the page, target URI, and page metadata.
/// </summary>
public class PageNavigatingEventArgs : EventArgs
{
    /// <inheritdoc />
    public PageNavigatingEventArgs(BasePage page, Uri uri, PageMetadata metadata)
    {
        Page = page;
        Uri = uri;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the page object performing navigation.
    /// </summary>
    public BasePage Page { get; }

    /// <summary>
    /// Gets the target URI being navigated to.
    /// </summary>
    public Uri Uri { get; }

    /// <summary>
    /// Gets the metadata describing the page.
    /// </summary>
    public PageMetadata Metadata { get; }
}
