using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element
{
    public class FindingElementsEventArgs : EventArgs
    {
        public FindingElementsEventArgs(string by, ComponentsListMetadata componentsLIstMetadata)
        {
            By = by;
            ComponentsListMetadata = componentsLIstMetadata;
        }

        public string By { get; }

        public ComponentsListMetadata ComponentsListMetadata { get; }
    }
}
