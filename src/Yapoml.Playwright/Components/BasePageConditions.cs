using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Conditions;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Factory;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Provides awaitable conditions for verifying the state of a page, including load state, URL, and title.
/// </summary>
/// <typeparam name="TSelf">The concrete conditions type for fluent chaining.</typeparam>
public abstract class BasePageConditions<TSelf> : BaseConditions<TSelf>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasePageConditions{TSelf}"/> class.
    /// </summary>
    public BasePageConditions(TimeSpan timeout, TimeSpan pollingInterval, IPage driver, IElementHandlerRepository elementHandlerRepository, IElementLocator elementLocator, PageMetadata pageMetadata, IEventSource eventSource, ILogger logger, ISpaceOptions spaceOptions)
        : base(timeout, pollingInterval)
    {
        Driver = driver;
        ElementHandlerRepository = elementHandlerRepository;
        ElementLocator = elementLocator;
        PageMetadata = pageMetadata;
        EventSource = eventSource;
        Logger = logger;
        SpaceOptions = spaceOptions;
    }

    /// <summary>Gets the Playwright page instance.</summary>
    protected IPage Driver { get; }
    /// <summary>Gets the element handler repository.</summary>
    protected IElementHandlerRepository ElementHandlerRepository { get; }
    /// <summary>Gets the element locator service.</summary>
    protected IElementLocator ElementLocator { get; }
    /// <summary>Gets the page metadata.</summary>
    protected PageMetadata PageMetadata { get; }
    /// <summary>Gets the event source for lifecycle events.</summary>
    protected IEventSource EventSource { get; }
    /// <summary>Gets the logger instance.</summary>
    protected ILogger Logger { get; }
    /// <summary>Gets the space options configuration.</summary>
    protected ISpaceOptions SpaceOptions { get; }

    /// <summary>
    /// Evaluates document's state to be <c>complete</c> which means the page is fully loaded.
    /// It doesn't guarantee that some components on the page are present, they might be rendered dynamically.
    /// 
    /// If url is defined for the page, then it also evaluates current url.
    /// </summary>
    public virtual TSelf IsOpened(TimeSpan? timeout = default)
    {
        timeout ??= Timeout;

        return Enqueue(async () =>
        {
            try
            {
                using (var scope = Logger.BeginLogScope($"Expect the {PageMetadata.Name} document state is complete"))
                {
                    await scope.ExecuteAsync(() => Driver.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions { Timeout = (float)timeout.Value.TotalSeconds })).ConfigureAwait(false);
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{PageMetadata.Name} page is not opened yet.", ex);
            }
        });
    }

    /// <summary>
    /// Various conditions for current page Url.
    /// </summary>
    public virtual UrlConditions<TSelf> Url
    {
        get
        {
            return new UrlConditions<TSelf>(Driver, _self, Timeout, PollingInterval, PageMetadata, Logger) { Chain = Chain };
        }
    }

    /// <summary>
    /// Various conditions for current title of the page.
    /// </summary>
    public virtual TitleConditions<TSelf> Title
    {
        get
        {
            return new TitleConditions<TSelf>(Driver, _self, Timeout, PollingInterval, PageMetadata, Logger) { Chain = Chain };
        }
    }

    /// <summary>
    /// Waits specified amount of time.
    /// </summary>
    /// <param name="duration">Aamount of time to wait.</param>
    /// <returns></returns>
    public virtual TSelf Elapsed(TimeSpan duration)
    {
        return Enqueue(() => Task.Delay(duration));
    }

    /// <summary>
    /// Resolves an element handler from the page conditions' repository, creating and caching it if not found.
    /// </summary>
    protected IElementHandler ResolveElementHandler(string key, string by, ElementLocatorContext byFrom, string metadataName)
    {
        if (ElementHandlerRepository.TryGet(key, out var cachedElementHandler))
            return cachedElementHandler;

        var metadata = new ComponentMetadata { Name = metadataName };
        var elementHandler = new ElementHandler(Driver, null, ElementLocator, by, byFrom, metadata, ElementHandlerRepository.CreateNestedRepository(), EventSource);
        ElementHandlerRepository.Set(key, elementHandler);
        return elementHandler;
    }

    /// <summary>
    /// Creates an elements list handler for plural components in page conditions.
    /// </summary>
    protected IElementsListHandler CreateElementsListHandler(string by, ElementLocatorContext byFrom, string singularName, string pluralName)
    {
        var metadata = new ComponentMetadata { Name = singularName };
        var listMetadata = new ComponentsListMetadata { Name = pluralName, ComponentMetadata = metadata };
        var factory = SpaceOptions.Services.Get<IElementsListHandlerFactory>();
        return factory.Create(Driver, null, ElementLocator, by, byFrom, listMetadata, ElementHandlerRepository.CreateNestedRepository(), EventSource);
    }
}
