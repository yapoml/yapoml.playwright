using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;
using Microsoft.Playwright;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Default implementation of <see cref="IComponentFactory"/> that creates components via reflection.
/// </summary>
public class DefaultComponentFactory : IComponentFactory
{
    /// <inheritdoc />
    public TComponent Create<TComponent, TConditions, TCondition>(BasePage page, BaseComponent parentComponent, IPage driver, IElementHandler elementHandler, ComponentMetadata componentMetadata, ISpaceOptions spaceOptions) where TComponent : BaseComponent
    {
        var component = (TComponent)Activator.CreateInstance(typeof(TComponent), page, parentComponent, driver, elementHandler, componentMetadata, spaceOptions);

        return component;
    }
}
