using Microsoft.Playwright;
using System;
using Yapoml.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Factory
{
    public class DefaultPageFactory : IPageFactory
    {
        public TPage Create<TPage>(IPage page, ISpaceOptions spaceOptions) where TPage : BasePage
        {
            var pageObj = (TPage)Activator.CreateInstance(typeof(TPage), page, spaceOptions);

            return pageObj;
        }
    }
}
