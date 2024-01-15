using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element
{
    public class FindingElementEventArgs : EventArgs
    {
        public FindingElementEventArgs(string by, ComponentMetadata componentMetadata)
        {
            By = by;
            ComponentMetadata = componentMetadata;
        }

        public string By { get; }

        public ComponentMetadata ComponentMetadata { get; }
    }
}
