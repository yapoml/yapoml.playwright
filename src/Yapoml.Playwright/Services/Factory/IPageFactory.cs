using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory
{
    public interface IPageFactory
    {
        TPage Create<TPage>(IPage driver, IElementHandlerRepository elementHandlerRepository, PageMetadata metadata, ISpaceOptions spaceOptions) where TPage : BasePage;
    }
}
