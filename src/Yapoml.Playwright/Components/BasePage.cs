using Microsoft.Playwright;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Factory;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components;

public abstract class BasePage
{
    public BasePage(IPage driver, IElementHandlerRepository elementHandlerRepository, PageMetadata metadata, ISpaceOptions spaceOptions)
    {
        Driver = driver;
        ElementHandlerRepository = elementHandlerRepository;
        Metadata = metadata;
        SpaceOptions = spaceOptions;

        EventSource = spaceOptions.Services.Get<IEventSource>();
        _logger = spaceOptions.Services.Get<ILogger>();
    }

    protected IPage Driver { get; }

    protected IElementHandlerRepository ElementHandlerRepository { get; }

    protected PageMetadata Metadata { get; }

    protected ISpaceOptions SpaceOptions { get; }

    protected IEventSource EventSource { get; }

    protected ILogger _logger;

    /// <summary>
    /// Resolves an element handler from the page's repository, creating and caching it if not found.
    /// </summary>
    protected IElementHandler ResolveElementHandler(string key, string by, ElementLocatorContext byFrom, string metadataName)
    {
        if (ElementHandlerRepository.TryGet(key, out var cachedElementHandler))
            return cachedElementHandler;

        var metadata = new ComponentMetadata { Name = metadataName };
        var elementLocator = SpaceOptions.Services.Get<IElementLocator>();
        var elementHandler = new ElementHandler(Driver, null, elementLocator, by, byFrom, metadata, ElementHandlerRepository.CreateNestedRepository(), EventSource);
        ElementHandlerRepository.Set(key, elementHandler);
        return elementHandler;
    }

    /// <summary>
    /// Creates an elements list handler for plural components on a page.
    /// </summary>
    protected IElementsListHandler CreateElementsListHandler(string by, ElementLocatorContext byFrom, string singularName, string pluralName)
    {
        var metadata = new ComponentMetadata { Name = singularName };
        var listMetadata = new ComponentsListMetadata { Name = pluralName, ComponentMetadata = metadata };
        var elementLocator = SpaceOptions.Services.Get<IElementLocator>();
        var factory = SpaceOptions.Services.Get<IElementsListHandlerFactory>();
        return factory.Create(Driver, null, elementLocator, by, byFrom, listMetadata, ElementHandlerRepository.CreateNestedRepository(), EventSource);
    }
}
