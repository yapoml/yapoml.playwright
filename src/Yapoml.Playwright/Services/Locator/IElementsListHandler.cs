using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Services.Locator;

/// <summary>
/// Handles locating and caching a list of elements on a page or within a parent element.
/// </summary>
public interface IElementsListHandler
{
    /// <summary>
    /// Invalidates the cached elements, forcing a re-locate on the next access.
    /// </summary>
    void Invalidate();

    /// <summary>
    /// Locates all matching elements.
    /// </summary>
    /// <returns>A read-only list of Playwright <see cref="ILocator"/> instances for the matched elements.</returns>
    Task<IReadOnlyList<ILocator>> LocateManyAsync();

    /// <summary>
    /// Gets the lazy locator matching all the elements, without resolving them.
    /// </summary>
    ILocator Locate();

    /// <summary>
    /// Gets the locator selector string used to find the elements.
    /// </summary>
    string By { get; }

    /// <summary>
    /// Gets the context (parent or root) from which the elements are located.
    /// </summary>
    ElementLocatorContext From { get; }

    /// <summary>
    /// Gets the metadata describing the components list.
    /// </summary>
    ComponentsListMetadata ComponentsListMetadata { get; }

    /// <summary>
    /// Gets the repository for caching nested element handlers.
    /// </summary>
    IElementHandlerRepository ElementHandlerRepository { get; }
}
