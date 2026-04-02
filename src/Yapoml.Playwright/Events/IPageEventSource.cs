using System;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Page;

namespace Yapoml.Playwright.Events;

/// <summary>
/// Event source for page lifecycle events such as navigation.
/// </summary>
public interface IPageEventSource
{
    /// <summary>
    /// Occurs when a page is about to navigate to a URL.
    /// </summary>
    event EventHandler<PageNavigatingEventArgs> OnPageNavigating;

    /// <summary>
    /// Raises the <see cref="OnPageNavigating"/> event.
    /// </summary>
    /// <param name="page">The page object performing navigation.</param>
    /// <param name="uri">The target URL.</param>
    /// <param name="metadata">Metadata describing the page.</param>
    void RaiseOnPageNavigating(BasePage page, Uri uri, PageMetadata metadata);
}
