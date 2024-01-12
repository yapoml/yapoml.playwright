using Microsoft.Playwright;
using System.Collections.Generic;

namespace Yapoml.Playwright.Services.Locator
{
    public interface IElementLocator
    {
        ILocator FindElement(ILocator searchContext, string by);

        IReadOnlyList<ILocator> FindElements(ILocator searchContext, string by);
    }
}
