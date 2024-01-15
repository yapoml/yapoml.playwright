using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory
{
    public interface IComponentFactory
    {
        TComponent Create<TComponent, TConditions, TCondition>(BasePage page, BaseComponent parentComponent, IPage driver, IElementHandler elementHandler, ComponentMetadata componentMetadata, ISpaceOptions spaceOptions) where TComponent : BaseComponent;
    }
}
