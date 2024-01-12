using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Services.Factory
{
    public interface ISpaceFactory
    {
        TSpace Create<TSpace>(BaseSpace parentSpace, IPage context, ISpaceOptions spaceOptions);
    }
}
