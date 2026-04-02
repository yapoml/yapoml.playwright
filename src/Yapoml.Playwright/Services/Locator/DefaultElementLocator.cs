using Microsoft.Playwright;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Services.Locator;

/// <summary>
/// Default implementation of <see cref="IElementLocator"/> that uses Playwright's built-in locator resolution.
/// </summary>
public class DefaultElementLocator : IElementLocator
{
    /// <inheritdoc />
    [DebuggerHidden]
    public ILocator FindElement(ILocator searchContext, string by)
    {
        return searchContext.Locator(by);
    }

    /// <inheritdoc />
    [DebuggerHidden]
    public IReadOnlyList<ILocator> FindElements(ILocator searchContext, string by)
    {
        return Task.Run(() => searchContext.Locator(by).AllAsync()).GetAwaiter().GetResult();
    }
}
