﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events.Args.Element;

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

        void RaiseOnFoundComponents(string by, IPage driver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata);

        void RaiseOnFoundComponent(string by, IPage driver, ILocator element, ComponentMetadata componentMetadata);
    }
}
