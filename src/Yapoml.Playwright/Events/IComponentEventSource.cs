using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Element;

namespace Yapoml.Playwright.Events;

/// <summary>
/// Event source for component lifecycle events such as finding and locating elements.
/// </summary>
public interface IComponentEventSource
{
    /// <summary>
    /// Occurs when the framework begins searching for a single component.
    /// </summary>
    event EventHandler<FindingElementEventArgs> OnFindingComponent;

    /// <summary>
    /// Occurs when a single component has been found.
    /// </summary>
    event EventHandler<FoundElementEventArgs> OnFoundComponent;

    /// <summary>
    /// Occurs when the framework begins searching for a list of components.
    /// </summary>
    event EventHandler<FindingElementsEventArgs> OnFindingComponents;

    /// <summary>
    /// Occurs when a list of components has been found.
    /// </summary>
    event EventHandler<FoundElementsEventArgs> OnFoundComponents;

    /// <summary>
    /// Raises the <see cref="OnFindingComponent"/> event.
    /// </summary>
    /// <param name="by">The locator selector being used to find the component.</param>
    /// <param name="componentMetadata">Metadata describing the component being searched for.</param>
    void RaiseOnFindingComponent(string by, ComponentMetadata componentMetadata);

    /// <summary>
    /// Raises the <see cref="OnFindingComponents"/> event.
    /// </summary>
    /// <param name="by">The locator selector being used to find the components.</param>
    /// <param name="componentsListMetadata">Metadata describing the components list being searched for.</param>
    void RaiseOnFindingComponents(string by, ComponentsListMetadata componentsListMetadata);

    /// <summary>
    /// Raises the <see cref="OnFoundComponents"/> event.
    /// </summary>
    /// <param name="by">The locator selector that was used.</param>
    /// <param name="driver">The Playwright page instance.</param>
    /// <param name="elements">The located elements.</param>
    /// <param name="componentsListMetadata">Metadata describing the located components list.</param>
    void RaiseOnFoundComponents(string by, IPage driver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata);

    /// <summary>
    /// Raises the <see cref="OnFoundComponent"/> event.
    /// </summary>
    /// <param name="by">The locator selector that was used.</param>
    /// <param name="driver">The Playwright page instance.</param>
    /// <param name="element">The located element.</param>
    /// <param name="componentMetadata">Metadata describing the located component.</param>
    void RaiseOnFoundComponent(string by, IPage driver, ILocator element, ComponentMetadata componentMetadata);
}
