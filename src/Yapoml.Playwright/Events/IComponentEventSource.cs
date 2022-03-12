using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Events.Args.WebElement;

namespace Yapoml.Playwright.Events
{
    public interface IComponentEventSource
    {
        event EventHandler<FindingElementEventArgs> OnFindingComponent;

        event EventHandler<FoundElementEventArgs> OnFoundComponent;

        event EventHandler<FindingElementEventArgs> OnFindingComponents;

        event EventHandler<FoundElementsEventArgs> OnFoundComponents;

        void RaiseOnFindingComponent(string componentName, string by);

        void RaiseOnFindingComponents(string componentName, string by);

        void RaiseOnFoundComponents(string by, IReadOnlyList<IElementHandle> elementHandles);

        void RaiseOnFoundComponent(string by, IPage page, IElementHandle elementHandle);
    }
}
