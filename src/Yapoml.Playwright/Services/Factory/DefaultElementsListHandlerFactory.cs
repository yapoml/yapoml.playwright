using Microsoft.Playwright;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Default implementation of <see cref="IElementsListHandlerFactory"/> that creates <see cref="ElementsListHandler"/> instances.
/// </summary>
public class DefaultElementsListHandlerFactory : IElementsListHandlerFactory
{
    /// <inheritdoc />
    public virtual IElementsListHandler Create(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ElementLocatorContext from, ComponentsListMetadata componentsListMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource)
    {
        return new ElementsListHandler(driver, parentElementHandler, elementLocator, by, from, componentsListMetadata, elementHandlerRepository, eventSource);
    }
}
