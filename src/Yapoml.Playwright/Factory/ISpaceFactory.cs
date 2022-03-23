using Microsoft.Playwright;

namespace Yapoml.Playwright.Factory
{
    public interface ISpaceFactory
    {
        TSpace Create<TSpace>(IPage page, Options.ISpaceOptions spaceOptions);
    }
}
