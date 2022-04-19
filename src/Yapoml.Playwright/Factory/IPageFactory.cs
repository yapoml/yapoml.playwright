using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Factory
{
    public interface IPageFactory
    {
        TPage Create<TPage>(IPage page, ISpaceOptions spaceOptions) where TPage : BasePage;
    }
}
