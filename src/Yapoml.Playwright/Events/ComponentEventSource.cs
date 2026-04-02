using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Element;

namespace Yapoml.Playwright.Events;

/// <summary>
/// Default implementation of <see cref="IComponentEventSource"/> that manages component lifecycle events.
/// </summary>
public class ComponentEventSource : IComponentEventSource
{
    /// <inheritdoc />
    public event EventHandler<FindingElementEventArgs> OnFindingComponent;
    /// <inheritdoc />
    public event EventHandler<FindingElementsEventArgs> OnFindingComponents;
    /// <inheritdoc />
    public event EventHandler<FoundElementsEventArgs> OnFoundComponents;
    /// <inheritdoc />
    public event EventHandler<FoundElementEventArgs> OnFoundComponent;

    /// <inheritdoc />
    public void RaiseOnFindingComponent(string by, ComponentMetadata componentMetadata)
    {
        OnFindingComponent?.Invoke(this, new FindingElementEventArgs(by, componentMetadata));
    }

    /// <inheritdoc />
    public void RaiseOnFindingComponents(string by, ComponentsListMetadata componentsListMetadata)
    {
        OnFindingComponents?.Invoke(this, new FindingElementsEventArgs(by, componentsListMetadata));
    }

    /// <inheritdoc />
    public void RaiseOnFoundComponent(string by, IPage driver, ILocator element, ComponentMetadata componentMetadata)
    {
        OnFoundComponent?.Invoke(this, new FoundElementEventArgs(by, driver, element, componentMetadata));
    }

    /// <inheritdoc />
    public void RaiseOnFoundComponents(string by, IPage driver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata)
    {
        OnFoundComponents?.Invoke(this, new FoundElementsEventArgs(by, driver, elements, componentsListMetadata));
    }
}
