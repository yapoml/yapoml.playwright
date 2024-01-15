using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element
{
    public class FoundElementsEventArgs : EventArgs
    {
        public FoundElementsEventArgs(string by, IPage driver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata)
        {
            By = by;
            Driver = driver;
            Elements = elements;
            ComponentsListMetadata = componentsListMetadata;
        }

        public string By { get; }
        public IPage Driver { get; }
        public IReadOnlyList<ILocator> Elements { get; }
        public ComponentsListMetadata ComponentsListMetadata { get; }
    }
}
