using Microsoft.Playwright;
using System;
using System.Collections.Generic;

namespace Yapoml.Playwright.Events.Args.WebElement
{
    public class FoundElementsEventArgs : EventArgs
    {
        public FoundElementsEventArgs(string by, IReadOnlyList<ILocator> locators)
        {
            By = by;
            Locators = locators;
        }

        public string By { get; }
        public IReadOnlyList<ILocator> Locators { get; }
    }
}
