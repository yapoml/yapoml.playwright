using System;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Page;

namespace Yapoml.Playwright.Events;

/// <summary>
/// Default implementation of <see cref="IPageEventSource"/> that manages page lifecycle events.
/// </summary>
public class PageEventSource : IPageEventSource
{
    /// <inheritdoc />
    public event EventHandler<PageNavigatingEventArgs> OnPageNavigating;

    /// <inheritdoc />
    public void RaiseOnPageNavigating(BasePage page, Uri uri, PageMetadata metadata)
    {
        OnPageNavigating?.Invoke(this, new PageNavigatingEventArgs(page, uri, metadata));
    }
}
