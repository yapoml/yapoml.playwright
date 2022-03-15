using Microsoft.Playwright;
using System;

namespace Yapoml.Playwright.Events.Args.WebElement
{
    public class FoundElementEventArgs : EventArgs
    {
        public FoundElementEventArgs(string by, IPage page, ILocator locator)
        {
            By = by;
            Page = page;
            Locator = locator;
        }

        public string By { get; }
        public IPage Page { get; }
        public ILocator Locator { get; }
    }
}
