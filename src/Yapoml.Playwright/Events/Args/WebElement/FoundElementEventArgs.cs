using Microsoft.Playwright;
using System;

namespace Yapoml.Playwright.Events.Args.WebElement
{
    public class FoundElementEventArgs : EventArgs
    {
        public FoundElementEventArgs(string by, IPage page, IElementHandle elementHandle)
        {
            By = by;
            Page = page;
            ElementHandle = elementHandle;
        }

        public string By { get; }
        public IPage Page { get; }
        public IElementHandle ElementHandle { get; }
    }
}
