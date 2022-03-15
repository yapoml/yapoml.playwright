﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Events.Args.WebElement;

namespace Yapoml.Playwright.Events
{
    public class ComponentEventSource : IComponentEventSource
    {
        public event EventHandler<FindingElementEventArgs> OnFindingComponent;
        public event EventHandler<FindingElementEventArgs> OnFindingComponents;
        public event EventHandler<FoundElementsEventArgs> OnFoundComponents;
        public event EventHandler<FoundElementEventArgs> OnFoundComponent;

        public void RaiseOnFindingComponent(string componentName, string by)
        {
            OnFindingComponent?.Invoke(this, new FindingElementEventArgs(componentName, by));
        }

        public void RaiseOnFindingComponents(string componentName, string by)
        {
            OnFindingComponents?.Invoke(this, new FindingElementEventArgs(componentName, by));
        }

        public void RaiseOnFoundComponent(string by, IPage page, ILocator locator)
        {
            OnFoundComponent?.Invoke(this, new FoundElementEventArgs(by, page, locator));
        }

        public void RaiseOnFoundComponents(string by, IReadOnlyList<ILocator> locator)
        {
            OnFoundComponents?.Invoke(this, new FoundElementsEventArgs(by, locator));
        }
    }
}
