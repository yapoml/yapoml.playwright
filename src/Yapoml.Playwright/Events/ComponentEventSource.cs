using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Element;

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

        public void RaiseOnFoundComponent(string by, IPage driver, ILocator element, ComponentMetadata componentMetadata)
        {
            OnFoundComponent?.Invoke(this, new FoundElementEventArgs(by, driver, element, componentMetadata));
        }

        public void RaiseOnFoundComponents(string by, IPage driver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata)
        {
            OnFoundComponents?.Invoke(this, new FoundElementsEventArgs(by, driver, elements, componentsListMetadata));
        }
    }
}
