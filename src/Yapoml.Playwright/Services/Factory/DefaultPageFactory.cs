using Microsoft.Playwright;
using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory
{
    public class DefaultPageFactory : IPageFactory
    {
        public TPage Create<TPage>(IPage context, IElementHandlerRepository elementHandlerRepository, PageMetadata metadata, ISpaceOptions spaceOptions) where TPage : BasePage
        {
            var page = (TPage)Activator.CreateInstance(typeof(TPage), context, elementHandlerRepository, metadata, spaceOptions);

            return page;
        }
    }
}
