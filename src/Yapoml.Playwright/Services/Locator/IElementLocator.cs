using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Services.Locator;

/// <summary>
/// Provides methods for locating elements within a search context using selector strings.
/// </summary>
public interface IElementLocator
{
    /// <summary>
    /// Finds a single element within the search context.
    /// </summary>
    /// <param name="searchContext">The parent locator to search within.</param>
    /// <param name="by">The selector string to use for finding the element.</param>
    /// <returns>A Playwright <see cref="ILocator"/> for the found element.</returns>
    ILocator FindElement(ILocator searchContext, string by);

    /// <summary>
    /// Finds all elements matching the selector within the search context.
    /// </summary>
    /// <param name="searchContext">The parent locator to search within.</param>
    /// <param name="by">The selector string to use for finding elements.</param>
    /// <returns>A read-only list of Playwright <see cref="ILocator"/> instances.</returns>
    Task<IReadOnlyList<ILocator>> FindElementsAsync(ILocator searchContext, string by);
}
