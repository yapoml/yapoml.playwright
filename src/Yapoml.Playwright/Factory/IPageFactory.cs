using Microsoft.Playwright;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Factory
{
    public interface IPageFactory
    {
        TPage Create<TPage>(IPage page, Options.ISpaceOptions spaceOptions) where TPage : BasePage;
    }
}
