﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Yapoml.Playwright.Services
{
    public class NavigationService
    {
        string _baseUrl;

        public NavigationService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public Uri BuildUri(string url, IList<KeyValuePair<string, string>> segments, IList<KeyValuePair<string, string>> queryParams)
        {
            url = new SegmentService().Replace(url, segments);

            var urlBuilder = new UriBuilder(new Uri(new Uri(_baseUrl), url));

            if (queryParams != null)
            {
                urlBuilder.Query = string.Join("&", queryParams.Select(qp => $"{qp.Key}={qp.Value}"));
            }

            return urlBuilder.Uri;
        }
    }
}
