using Microsoft.Playwright;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Factory
{
    public interface IComponentFactory
    {
        TComponent Create<TComponent>(IPage page, ILocator locator, Options.ISpaceOptions spaceOptions) where TComponent : BaseComponent;
    }
}
