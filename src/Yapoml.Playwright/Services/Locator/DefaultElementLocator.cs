using Microsoft.Playwright;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Services.Locator
{
    public class DefaultElementLocator : IElementLocator
    {
        [DebuggerHidden]
        public ILocator FindElement(ILocator searchContext, string by)
        {
            return searchContext.Locator(by);
        }

        [DebuggerHidden]
        public IReadOnlyList<ILocator> FindElements(ILocator searchContext, string by)
        {
            return Task.Run(() => searchContext.Locator(by).AllAsync()).GetAwaiter().GetResult();
        }
    }
}
