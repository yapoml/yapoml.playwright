using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.WebElement;

namespace Yapoml.Playwright.Events
{
    public class ComponentEventSource : IComponentEventSource
    {
        public event EventHandler<FindingElementEventArgs> OnFindingComponent;
        public event EventHandler<FindingElementsEventArgs> OnFindingComponents;
        public event EventHandler<FoundElementsEventArgs> OnFoundComponents;
        public event EventHandler<FoundElementEventArgs> OnFoundComponent;

        public void RaiseOnFindingComponent(string by, ComponentMetadata componentMetadata)
        {
            OnFindingComponent?.Invoke(this, new FindingElementEventArgs(by, componentMetadata));
        }

        public void RaiseOnFindingComponents(string by, ComponentsListMetadata componentsListMetadata)
        {
            OnFindingComponents?.Invoke(this, new FindingElementsEventArgs(by, componentsListMetadata));
        }

        public void RaiseOnFoundComponent(string by, IPage webDriver, ILocator webElement, ComponentMetadata componentMetadata)
        {
            OnFoundComponent?.Invoke(this, new FoundElementEventArgs(by, webDriver, webElement, componentMetadata));
        }

        public void RaiseOnFoundComponents(string by, IPage webDriver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata)
        {
            OnFoundComponents?.Invoke(this, new FoundElementsEventArgs(by, webDriver, elements, componentsListMetadata));
        }
    }
}
