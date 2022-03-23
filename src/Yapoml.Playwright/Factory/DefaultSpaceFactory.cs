using Microsoft.Playwright;
using System;

namespace Yapoml.Playwright.Factory
{
    public class DefaultSpaceFactory : ISpaceFactory
    {
        public TSpace Create<TSpace>(IPage page, Options.ISpaceOptions spaceOptions)
        {
            var space = (TSpace)Activator.CreateInstance(typeof(TSpace), page, spaceOptions);

            return space;
        }
    }
}
