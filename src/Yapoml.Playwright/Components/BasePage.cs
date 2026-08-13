using Microsoft.Playwright;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Factory;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Base class for all page objects, providing navigation and element resolution capabilities.
/// </summary>
public abstract class BasePage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasePage"/> class.
    /// </summary>
    public BasePage(IPage driver, IElementHandlerRepository elementHandlerRepository, PageMetadata metadata, ISpaceOptions spaceOptions)
    {
        Driver = driver;
        ElementHandlerRepository = elementHandlerRepository;
        Metadata = metadata;
        SpaceOptions = spaceOptions;

        Chain = Chain.Resolve(spaceOptions);

        EventSource = spaceOptions.Services.Get<IEventSource>();
        _logger = spaceOptions.Services.Get<ILogger>();
    }

    /// <summary>Gets the Playwright page instance.</summary>
    protected IPage Driver { get; }

    /// <summary>Gets the chain of pending asynchronous steps built on this page.</summary>
    internal Chain Chain { get; }

    /// <summary>Makes the child conditions build into the same chain as this page.</summary>
    protected T Share<T>(T child) where T : BaseConditions
    {
        child.Chain = Chain;

        return child;
    }

    /// <summary>
    /// Enqueues a step to be executed when the page is awaited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void Enqueue(Func<Task> step)
    {
        Chain.Add(step);
    }

    /// <summary>
    /// Executes the pending steps and returns the awaited page back, so generated pages can expose <c>GetAwaiter</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected async Task<TPage> AwaitChainAsync<TPage>(TPage self)
    {
        await Chain.RunAsync().ConfigureAwait(false);

        return self;
    }

    /// <summary>Gets the element handler repository for caching resolved elements.</summary>
    protected IElementHandlerRepository ElementHandlerRepository { get; }

    /// <summary>Gets the page metadata.</summary>
    protected PageMetadata Metadata { get; }

    /// <summary>Gets the space options configuration.</summary>
    protected ISpaceOptions SpaceOptions { get; }

    /// <summary>Gets the event source for lifecycle events.</summary>
    protected IEventSource EventSource { get; }

    /// <summary>The logger instance.</summary>
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
