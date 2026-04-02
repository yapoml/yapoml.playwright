using Microsoft.Playwright;
using System;
using System.Linq;
#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif
using System.Threading;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;
using Yapoml.Framework.Options;
using Yapoml.Framework;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Provides awaitable conditions for verifying the state of a list of components, including count, element-level predicates, and emptiness.
/// </summary>
/// <typeparam name="TSelf">The concrete list conditions type for fluent chaining.</typeparam>
/// <typeparam name="TComponentConditions">The conditions type for individual component expectations within the list.</typeparam>
public class BaseComponentListConditions<TSelf, TComponentConditions> : BaseConditions<TSelf>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseComponentListConditions{TSelf, TComponentConditions}"/> class.
    /// </summary>
    public BaseComponentListConditions(TimeSpan timeout, TimeSpan pollingInterval, IPage driver, IElementsListHandler elementsListHandler, IElementLocator elementLocator, IEventSource eventSource, ILogger logger, ISpaceOptions spaceOptions)
        : base(timeout, pollingInterval)
    {
        Driver = driver;
        ElementsListHandler = elementsListHandler;
        ElementLocator = elementLocator;
        EventSource = eventSource;
        Logger = logger;
        SpaceOptions = spaceOptions;
    }

    /// <summary>Gets the Playwright page instance.</summary>
    protected IPage Driver { get; }
    /// <summary>Gets the elements list handler for this list.</summary>
    protected IElementsListHandler ElementsListHandler { get; }
    /// <summary>Gets the element locator service.</summary>
    protected IElementLocator ElementLocator { get; }
    /// <summary>Gets the event source for lifecycle events.</summary>
    protected IEventSource EventSource { get; }
    /// <summary>Gets the logger instance.</summary>
    protected ILogger Logger { get; }
    /// <summary>Gets the space options configuration.</summary>
    protected ISpaceOptions SpaceOptions { get; }

    /// <summary>
    /// Conditions for the count of components in the list.
    /// </summary>
    public CountCollectionConditions<TSelf> Count => new CountCollectionConditions<TSelf>(_self, ElementsListHandler, Timeout, PollingInterval, Logger);

    /// <summary>
    /// Waits until every component in the list satisfies the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition that each component must satisfy.</param>
    /// <param name="timeout">How long to wait for all components to satisfy the condition.</param>
    /// <returns>The same conditions instance for further chaining.</returns>
#if NET6_0_OR_GREATER
        public TSelf Each(Action<TComponentConditions> predicate, TimeSpan? timeout = default, [CallerArgumentExpression("predicate")] string predicateExpression = null)
#else
    public TSelf Each(Action<TComponentConditions> predicate, TimeSpan? timeout = default)
#endif
    {
        timeout ??= Timeout;

        bool condition()
        {
            var elements = ElementsListHandler.LocateMany();

            for (int i = 0; i < elements.Count; i++)
            {
                var elementHandler = new ElementHandler(Driver, null, ElementLocator, ElementsListHandler.By, ElementsListHandler.From, elements[i], ElementsListHandler.ComponentsListMetadata.ComponentMetadata, ElementsListHandler.ElementHandlerRepository.CreateNestedRepository(), EventSource);
                var elementCondition = (TComponentConditions)Activator.CreateInstance(typeof(TComponentConditions), TimeSpan.FromMilliseconds(-1), PollingInterval, Driver, elementHandler, ElementLocator, EventSource, Logger, SpaceOptions);

                try
                {
                    predicate(elementCondition);
                }
                catch (TimeoutException ex)
                {
                    var indexPostfix = (i + 1) switch
                    {
                        1 => "st",
                        2 => "nd",
                        3 => "rd",
                        _ => "th"
                    };
#if NET6_0_OR_GREATER
                        throw new ExpectException($"The {i + 1}{indexPostfix} {elementHandler.ComponentMetadata.Name} of {elements.Count} does not satisfy condition '{predicateExpression}'.", ex);
#else
                    throw new ExpectException($"The {i + 1}{indexPostfix} {elementHandler.ComponentMetadata.Name} of {elements.Count} does not satisfy condition.", ex);
#endif
                }
            }

            return true;
        }

        try
        {
            using (var scope = Logger.BeginLogScope($"Expect each {ElementsListHandler.ComponentsListMetadata.ComponentMetadata.Name} satisfy conditions"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, PollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException($"Not all {ElementsListHandler.ComponentsListMetadata.Name} satisfy condition.", ex);
        }

        return _self;
    }

    /// <summary>
    /// Waits until at least one component in the list satisfies the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition that at least one component must satisfy.</param>
    /// <param name="timeout">How long to wait for a matching component.</param>
    /// <returns>The same conditions instance for further chaining.</returns>
#if NET6_0_OR_GREATER
        public TSelf Contains(Action<TComponentConditions> predicate, TimeSpan? timeout = default, [CallerArgumentExpression("predicate")] string predicateExpression = null)
#else
    public TSelf Contains(Action<TComponentConditions> predicate, TimeSpan? timeout = default)
#endif
    {
        timeout ??= Timeout;

        bool condition()
        {
            var elements = ElementsListHandler.LocateMany();

            foreach (var element in elements)
            {
                try
                {
                    var elementHandler = new ElementHandler(Driver, null, ElementLocator, ElementsListHandler.By, ElementsListHandler.From, element, ElementsListHandler.ComponentsListMetadata.ComponentMetadata, ElementsListHandler.ElementHandlerRepository.CreateNestedRepository(), EventSource);
                    var elementCondition = (TComponentConditions)Activator.CreateInstance(typeof(TComponentConditions), TimeSpan.FromMilliseconds(-1), PollingInterval, Driver, elementHandler, ElementLocator, EventSource, Logger, SpaceOptions);

                    predicate(elementCondition);

                    return true;
                }
                catch (ExpectException)
                {
                    continue;
                }
            }

            return false;
        }

        try
        {
            Waiter.Until(condition, timeout.Value, PollingInterval);
        }
        catch (TimeoutException ex)
        {
#if NET6_0_OR_GREATER
                throw new ExpectException($"The {ElementsListHandler.ComponentsListMetadata.Name} does not contain any {ElementsListHandler.ComponentsListMetadata.ComponentMetadata.Name} satisfying condition '{predicateExpression}'.", ex);
#else
            throw new ExpectException($"The {ElementsListHandler.ComponentsListMetadata.Name} does not contain any {ElementsListHandler.ComponentsListMetadata.ComponentMetadata.Name} satisfying condition.", ex);
#endif
        }

        return _self;
    }

    /// <summary>
    /// Waits until no component in the list satisfies the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition that no component should satisfy.</param>
    /// <param name="timeout">How long to wait for the condition to be met.</param>
    /// <returns>The same conditions instance for further chaining.</returns>
#if NET6_0_OR_GREATER
        public TSelf DoNotContain(Action<TComponentConditions> predicate, TimeSpan? timeout = default, [CallerArgumentExpression("predicate")] string predicateExpression = null)
#else
    public TSelf DoNotContain(Action<TComponentConditions> predicate, TimeSpan? timeout = default)
#endif
    {
        timeout ??= Timeout;

        bool condition()
        {
            var elements = ElementsListHandler.LocateMany();

            bool result = true;

            foreach (var element in elements)
            {
                try
                {
                    var elementHandler = new ElementHandler(Driver, null, ElementLocator, ElementsListHandler.By, ElementsListHandler.From, element, ElementsListHandler.ComponentsListMetadata.ComponentMetadata, ElementsListHandler.ElementHandlerRepository.CreateNestedRepository(), EventSource);
                    var elementCondition = (TComponentConditions)Activator.CreateInstance(typeof(TComponentConditions), TimeSpan.FromMilliseconds(-1), PollingInterval, Driver, elementHandler, ElementLocator, EventSource, Logger, SpaceOptions);

                    predicate(elementCondition);

                    // this one still satisfy condition, so returning false for reiterating
                    result = false;
                }
                catch (ExpectException)
                {
                    // do noting and leave result true b default, proceding next item
                }
            }

            return result;
        }

        try
        {
            Waiter.Until(condition, timeout.Value, PollingInterval);
        }
        catch (TimeoutException ex)
        {
#if NET6_0_OR_GREATER
                throw new ExpectException($"The {ElementsListHandler.ComponentsListMetadata.Name} contain at least one {ElementsListHandler.ComponentsListMetadata.ComponentMetadata.Name} satisfying condition '{predicateExpression}'.", ex);
#else
            throw new ExpectException($"The {ElementsListHandler.ComponentsListMetadata.Name} contain at least one {ElementsListHandler.ComponentsListMetadata.ComponentMetadata.Name} satisfying condition.", ex);
#endif
        }

        return _self;
    }

    /// <summary>
    /// Waits until the list contains no components.
    /// </summary>
    /// <param name="timeout">How long to wait for the list to become empty.</param>
    /// <returns>The same conditions instance for further chaining.</returns>
    public virtual TSelf IsEmpty(TimeSpan? timeout = default)
    {
        return Count.Is(0, timeout);
    }

    /// <summary>
    /// Waits until the list contains at least one component.
    /// </summary>
    /// <param name="timeout">How long to wait for the list to become non-empty.</param>
    /// <returns>The same conditions instance for further chaining.</returns>
    public virtual TSelf IsNotEmpty(TimeSpan? timeout = default)
    {
        return Count.IsGreaterThan(0, timeout);
    }

    /// <summary>
    /// Waits specified amount of time.
    /// </summary>
    /// <param name="duration">Aamount of time to wait.</param>
    /// <returns></returns>
    public virtual TSelf Elapsed(TimeSpan duration)
    {
        Thread.Sleep(duration);

        return _self;
    }
}
