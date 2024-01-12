using Microsoft.Playwright;
using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.WebElement
{
    public class FoundElementEventArgs : EventArgs
    {
        public FoundElementEventArgs(string by, IPage webDriver, ILocator webElement, ComponentMetadata componentMetadata)
        {
            By = by;
            WebDriver = webDriver;
            WebElement = webElement;
            ComponentMetadata = componentMetadata;
        }

        public string By { get; }
        public IPage WebDriver { get; }
        public ILocator WebElement { get; }
        public ComponentMetadata ComponentMetadata { get; }
    }
}
