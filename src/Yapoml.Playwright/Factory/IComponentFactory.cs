using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Factory
{
    public interface IComponentFactory
    {
        TComponent Create<TComponent>(IPage page, ILocator locator, ISpaceOptions spaceOptions) where TComponent : BaseComponent;
    }
}
