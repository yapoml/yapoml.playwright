using Microsoft.Playwright;
using System;
using Yapoml.Framework.Options;

namespace Yapoml.Playwright.Factory
{
    public class DefaultSpaceFactory : ISpaceFactory
    {
        public TSpace Create<TSpace>(IPage page, ISpaceOptions spaceOptions)
        {
            var space = (TSpace)Activator.CreateInstance(typeof(TSpace), page, spaceOptions);

            return space;
        }
    }
}
