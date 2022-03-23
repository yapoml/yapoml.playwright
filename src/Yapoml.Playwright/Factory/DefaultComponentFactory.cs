using Microsoft.Playwright;
using System;
using Yapoml.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Factory
{
    public class DefaultComponentFactory : IComponentFactory
    {
        public TComponent Create<TComponent>(IPage page, ILocator locator, ISpaceOptions spaceOptions) where TComponent : BaseComponent
        {
            var component = (TComponent)Activator.CreateInstance(typeof(TComponent), page, locator, spaceOptions);

            return component;
        }
    }
}
