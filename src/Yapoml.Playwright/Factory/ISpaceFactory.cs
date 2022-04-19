using Microsoft.Playwright;
using Yapoml.Framework.Options;

namespace Yapoml.Playwright.Factory
{
    public interface ISpaceFactory
    {
        TSpace Create<TSpace>(IPage page, ISpaceOptions spaceOptions);
    }
}
