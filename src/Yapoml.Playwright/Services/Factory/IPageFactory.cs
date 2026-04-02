using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Factory for creating page object instances.
/// </summary>
public interface IPageFactory
{
    /// <summary>
    /// Creates a new page object instance of the specified type.
    /// </summary>
    /// <typeparam name="TPage">The page type to create.</typeparam>
    /// <param name="driver">The Playwright page instance.</param>
    /// <param name="elementHandlerRepository">The repository for caching element handlers on the page.</param>
    /// <param name="metadata">Metadata describing the page.</param>
    /// <param name="spaceOptions">The space options configuration.</param>
    /// <returns>A new instance of <typeparamref name="TPage"/>.</returns>
    TPage Create<TPage>(IPage driver, IElementHandlerRepository elementHandlerRepository, PageMetadata metadata, ISpaceOptions spaceOptions) where TPage : BasePage;
}
