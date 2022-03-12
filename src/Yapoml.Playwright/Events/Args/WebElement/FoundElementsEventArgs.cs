using Microsoft.Playwright;
using System;
using System.Collections.Generic;

namespace Yapoml.Playwright.Events.Args.WebElement
{
    public class FoundElementsEventArgs : EventArgs
    {
        public FoundElementsEventArgs(string by, IReadOnlyList<IElementHandle> elementHandles)
        {
            By = by;
            ElementHandles = elementHandles;
        }

        public string By { get; }
        public IReadOnlyList<IElementHandle> ElementHandles { get; }
    }
}
