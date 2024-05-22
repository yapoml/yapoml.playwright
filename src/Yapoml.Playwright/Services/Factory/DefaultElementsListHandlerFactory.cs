using Microsoft.Playwright;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory
{
    public class DefaultElementsListHandlerFactory : IElementsListHandlerFactory
    {
        public virtual IElementsListHandler Create(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ElementLocatorContext from, ComponentsListMetadata componentsListMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource)
        {
            return new ElementsListHandler(driver, parentElementHandler, elementLocator, by, from, componentsListMetadata, elementHandlerRepository, eventSource);
        }
    }
}
