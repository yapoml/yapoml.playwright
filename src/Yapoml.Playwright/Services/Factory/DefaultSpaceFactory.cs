using Microsoft.Playwright;
using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Services.Factory
{
    public class DefaultSpaceFactory : ISpaceFactory
    {
        public TSpace Create<TSpace>(BaseSpace parentSpace, IPage driver, ISpaceOptions spaceOptions)
        {
            var space = (TSpace)Activator.CreateInstance(typeof(TSpace), parentSpace, driver, spaceOptions);

            return space;
        }
    }
}
