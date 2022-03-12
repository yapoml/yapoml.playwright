using System;

namespace Yapoml.Playwright.Events.Args.WebElement
{
    public class FindingElementEventArgs : EventArgs
    {
        public FindingElementEventArgs(string componentName, string by)
        {
            ComponentName = componentName;
            By = by;
        }

        public string ComponentName { get; }
        public string By { get; }
    }
}
