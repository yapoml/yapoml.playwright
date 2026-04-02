using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Factory for creating component instances.
/// </summary>
public interface IComponentFactory
{
    /// <summary>
    /// Creates a new component instance of the specified type.
    /// </summary>
    /// <typeparam name="TComponent">The component type to create.</typeparam>
    /// <typeparam name="TConditions">The chainable conditions type for the component.</typeparam>
    /// <typeparam name="TCondition">The one-time conditions type for the component.</typeparam>
    /// <param name="page">The parent page.</param>
    /// <param name="parentComponent">The parent component, or <c>null</c> for top-level components.</param>
    /// <param name="driver">The Playwright page instance.</param>
    /// <param name="elementHandler">The element handler for locating the component.</param>
    /// <param name="componentMetadata">Metadata describing the component.</param>
    /// <param name="spaceOptions">The space options configuration.</param>
    /// <returns>A new instance of <typeparamref name="TComponent"/>.</returns>
    TComponent Create<TComponent, TConditions, TCondition>(BasePage page, BaseComponent parentComponent, IPage driver, IElementHandler elementHandler, ComponentMetadata componentMetadata, ISpaceOptions spaceOptions) where TComponent : BaseComponent;
}
