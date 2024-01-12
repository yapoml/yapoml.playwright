using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;
using Microsoft.Playwright;

namespace Yapoml.Playwright.Services.Factory
{
    public class DefaultComponentFactory : IComponentFactory
    {
        public TComponent Create<TComponent, TConditions, TCondition>(BasePage page, BaseComponent parentComponent, IPage context, IElementHandler elementHandler, ComponentMetadata componentMetadata, ISpaceOptions spaceOptions) where TComponent : BaseComponent
        {
            var component = (TComponent)Activator.CreateInstance(typeof(TComponent), page, parentComponent, context, elementHandler, componentMetadata, spaceOptions);

            return component;
        }
    }
}
