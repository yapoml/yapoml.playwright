using Microsoft.Playwright;
using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Services.Locator;

/// <summary>
/// Handles locating and caching a single element on a page or within a parent element.
/// </summary>
public interface IElementHandler
{
    /// <summary>
    /// Locates the element using default timeout settings.
    /// </summary>
    /// <returns>The Playwright <see cref="ILocator"/> for the element.</returns>
    ILocator Locate();

    /// <summary>
    /// Locates the element with the specified timeout and polling interval.
    /// </summary>
    /// <param name="timeout">The maximum duration to wait for the element.</param>
    /// <param name="pollingInterval">The interval between locate attempts.</param>
    /// <returns>The Playwright <see cref="ILocator"/> for the element.</returns>
    ILocator Locate(TimeSpan timeout, TimeSpan pollingInterval);

    /// <summary>
    /// Invalidates the cached element, forcing a re-locate on the next access.
    /// </summary>
    void Invalidate();

    /// <summary>
    /// Gets the locator selector string used to find the element.
    /// </summary>
    string By { get; }

    /// <summary>
    /// Gets the context (parent or root) from which the element is located.
    /// </summary>
    ElementLocatorContext From { get; }

    /// <summary>
    /// Gets the metadata describing the component.
    /// </summary>
    ComponentMetadata ComponentMetadata { get; }

    /// <summary>
    /// Gets the repository for caching nested element handlers.
    /// </summary>
    IElementHandlerRepository ElementHandlerRepository { get; }
}
