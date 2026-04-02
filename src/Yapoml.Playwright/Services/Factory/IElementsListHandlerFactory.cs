using Microsoft.Playwright;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Factory for creating <see cref="IElementsListHandler"/> instances for managing lists of elements.
/// </summary>
public interface IElementsListHandlerFactory
{
    /// <summary>
    /// Creates a new elements list handler.
    /// </summary>
    /// <param name="driver">The Playwright page instance.</param>
    /// <param name="parentElementHandler">The parent element handler, or <c>null</c> for page-level lists.</param>
    /// <param name="elementLocator">The element locator service.</param>
    /// <param name="by">The locator selector string.</param>
    /// <param name="from">Specifies whether to search from parent or root context.</param>
    /// <param name="componentsListMetadata">Metadata describing the components list.</param>
    /// <param name="elementHandlerRepository">The repository for caching element handlers.</param>
    /// <param name="eventSource">The event source for raising lifecycle events.</param>
    /// <returns>A new <see cref="IElementsListHandler"/> instance.</returns>
    IElementsListHandler Create(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ElementLocatorContext from, ComponentsListMetadata componentsListMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource);
}
