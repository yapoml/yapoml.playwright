using Microsoft.Playwright;
using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element
{
    public class FoundElementEventArgs : EventArgs
    {
        public FoundElementEventArgs(string by, IPage driver, ILocator element, ComponentMetadata componentMetadata)
        {
            By = by;
            Driver = driver;
            Element = element;
            ComponentMetadata = componentMetadata;
        }

        public string By { get; }
        public IPage Driver { get; }
        public ILocator Element { get; }
        public ComponentMetadata ComponentMetadata { get; }
    }
}
