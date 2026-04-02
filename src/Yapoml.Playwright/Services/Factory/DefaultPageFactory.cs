using Microsoft.Playwright;
using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Default implementation of <see cref="IPageFactory"/> that creates page objects via reflection.
/// </summary>
public class DefaultPageFactory : IPageFactory
{
    /// <inheritdoc />
    public TPage Create<TPage>(IPage driver, IElementHandlerRepository elementHandlerRepository, PageMetadata metadata, ISpaceOptions spaceOptions) where TPage : BasePage
    {
        var page = (TPage)Activator.CreateInstance(typeof(TPage), driver, elementHandlerRepository, metadata, spaceOptions);

        return page;
    }
}
