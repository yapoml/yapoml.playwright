using System;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Page;

namespace Yapoml.Playwright.Events
{
    public interface IPageEventSource
    {
        event EventHandler<PageNavigatingEventArgs> OnPageNavigating;

        void RaiseOnPageNavigating(BasePage page, Uri uri, PageMetadata metadata);
    }
}
