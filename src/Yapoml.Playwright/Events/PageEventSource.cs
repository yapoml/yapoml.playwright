using System;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Page;

namespace Yapoml.Playwright.Events
{
    public class PageEventSource : IPageEventSource
    {
        public event EventHandler<PageNavigatingEventArgs> OnPageNavigating;

        public void RaiseOnPageNavigating(BasePage page, Uri uri, PageMetadata metadata)
        {
            OnPageNavigating?.Invoke(this, new PageNavigatingEventArgs(page, uri, metadata));
        }
    }
}
