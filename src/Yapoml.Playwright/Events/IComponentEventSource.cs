using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.WebElement;

namespace Yapoml.Playwright.Events
{
    public interface IComponentEventSource
    {
        event EventHandler<FindingElementEventArgs> OnFindingComponent;

        event EventHandler<FoundElementEventArgs> OnFoundComponent;

        event EventHandler<FindingElementsEventArgs> OnFindingComponents;

        event EventHandler<FoundElementsEventArgs> OnFoundComponents;

        void RaiseOnFindingComponent(string by, ComponentMetadata componentMetadata);

        void RaiseOnFindingComponents(string by, ComponentsListMetadata componentsListMetadata);

        void RaiseOnFoundComponents(string by, IPage context, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata);

        void RaiseOnFoundComponent(string by, IPage context, ILocator webElement, ComponentMetadata componentMetadata);
    }
}
