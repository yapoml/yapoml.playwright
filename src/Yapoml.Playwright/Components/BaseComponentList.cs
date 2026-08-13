using Microsoft.Playwright;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using Yapoml;

#endif
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Options;
using Yapoml.Playwright.Services.Factory;
using Yapoml.Playwright.Services.Locator;
using Yapoml.Framework;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Represents a read-only list of page components, supporting indexing by position, text, and predicate.
/// </summary>
/// <typeparam name="TComponent">The concrete component type in the list.</typeparam>
/// <typeparam name="TListConditions">The conditions type for list-level expectations.</typeparam>
/// <typeparam name="TComponentConditions">The conditions type for individual component expectations.</typeparam>
public class BaseComponentList<TComponent, TListConditions, TComponentConditions> : IReadOnlyList<TComponent>
    where TComponent : BaseComponent
    where TListConditions : BaseComponentListConditions<TListConditions, TComponentConditions>
    where TComponentConditions : BaseComponentConditions<TComponentConditions>
{
    /// <summary>The list-level conditions instance. Assigning it shares the list's chain with the conditions.</summary>
    protected TListConditions listConditions
    {
        get => _listConditions;
        set
        {
            _listConditions = value;

            if (value != null) value.Chain = Chain;
        }
    }

    private TListConditions _listConditions;

    private IList<TComponent> _list;

    private readonly BasePage _page;
    private readonly BaseComponent _parentComponent;
    private readonly IPage _driver;
    /// <summary>The elements list handler for locating components.</summary>
    protected readonly IElementsListHandler _elementsListHandler;
    private readonly ComponentsListMetadata _componentsListMetadata;
    private readonly IEventSource _eventSource;
    private readonly ISpaceOptions _spaceOptions;

    /// <summary>The logger for tracing list operations.</summary>
    protected readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseComponentList{TComponent, TListConditions, TComponentConditions}"/> class.
    /// </summary>
    public BaseComponentList(BasePage page, BaseComponent parentComponent, IPage driver, IElementsListHandler elementsListHandler, ComponentsListMetadata componentsListMetadata, IEventSource eventSource, ISpaceOptions spaceOptions)
    {
        _page = page;
        _parentComponent = parentComponent;
        _driver = driver;
        _elementsListHandler = elementsListHandler;
        _componentsListMetadata = componentsListMetadata;
        _eventSource = eventSource;
        _spaceOptions = spaceOptions;

        Chain = Chain.Resolve(spaceOptions);

        _logger = _spaceOptions.Services.Get<ILogger>();
    }

    /// <summary>Gets the chain of pending asynchronous steps shared with the page and parent component.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Chain Chain { get; }

    /// <summary>
    /// Executes the pending steps and returns the awaited list back, so generated lists can expose <c>GetAwaiter</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected async Task<TList> AwaitChainAsync<TList>(TList self)
    {
        await Chain.RunAsync().ConfigureAwait(false);

        return self;
    }

    /// <summary>
    /// Executes the pending steps and reads a value. Blocks the caller until list reads become awaitable.
    /// </summary>
    private T Read<T>(Func<Task<T>> read)
    {
        return Task.Run(async () =>
        {
            await Chain.RunAsync().ConfigureAwait(false);

            return await read().ConfigureAwait(false);
        }).GetAwaiter().GetResult();
    }

    private async Task<IList<TComponent>> LocateAllAsync()
    {
        var factory = _spaceOptions.Services.Get<IComponentFactory>();
        var locator = _spaceOptions.Services.Get<IElementLocator>();

        var elements = await _elementsListHandler.LocateManyAsync().ConfigureAwait(false);

        return new List<TComponent>(elements.Select(e => factory.Create<TComponent, TListConditions, TComponentConditions>(_page, _parentComponent, _driver, new ElementHandler(_driver, null, locator, _elementsListHandler.By, _elementsListHandler.From, e, _componentsListMetadata.ComponentMetadata, _elementsListHandler.ElementHandlerRepository.CreateNestedRepository(), _eventSource), _componentsListMetadata.ComponentMetadata, _spaceOptions)));
    }

    /// <summary>
    /// Gets the component at the specified index. Waits until sufficient elements are located.
    /// </summary>
    /// <param name="index">The zero-based index of the component.</param>
    /// <returns>The component at the specified index.</returns>
    /// <exception cref="ExpectException">Thrown when the component at the index cannot be found within the timeout.</exception>
    public TComponent this[int index]
    {
        get
        {
            return Read(async () =>
            {
                async Task<bool> condition()
                {
                    _list = await LocateAllAsync().ConfigureAwait(false);

                    if (_list.Count > index)
                    {
                        return true;
                    }
                    else
                    {
                        _elementsListHandler.Invalidate();

                        return false;
                    }
                }

                var timeout = _spaceOptions.Services.Get<TimeoutOptions>().Timeout;
                var pollingInterval = _spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;

                try
                {
                    await Waiter.UntilAsync(condition, timeout, pollingInterval).ConfigureAwait(false);
                }
                catch (TimeoutException exp)
                {
                    throw new ExpectException($"Couldn't get a {_componentsListMetadata.ComponentMetadata.Name} by index {index} from {_list.Count} {_componentsListMetadata.Name}.", exp);
                }

                return _list[index];
            });
        }
    }

    /// <summary>
    /// Gets the first component whose text content matches the specified string. Waits until a matching element is found.
    /// </summary>
    /// <param name="text">The text content to match.</param>
    /// <returns>The first component with matching text.</returns>
    /// <exception cref="ExpectException">Thrown when no component with the specified text is found within the timeout.</exception>
    public TComponent this[string text]
    {
        get
        {
            return Read(async () =>
            {
                TComponent component = null;

                async Task<bool> condition()
                {
                    _list = await LocateAllAsync().ConfigureAwait(false);

                    component = _list.FirstOrDefault(c => c.Text == text);

                    if (component is null)
                    {
                        _elementsListHandler.Invalidate();

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                var timeout = _spaceOptions.Services.Get<TimeoutOptions>().Timeout;
                var pollingInterval = _spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;

                try
                {
                    await Waiter.UntilAsync(condition, timeout, pollingInterval).ConfigureAwait(false);
                }
                catch (TimeoutException exp)
                {
                    throw new ExpectException($"{_componentsListMetadata.Name} contain no matching {_componentsListMetadata.ComponentMetadata.Name} with '{text}' text.", exp);
                }

                return component;
            });
        }
    }

    /// <summary>
    /// Gets the first component satisfying the specified predicate. Waits until a matching element is found.
    /// </summary>
    /// <param name="predicate">The predicate to match components against.</param>
    /// <returns>The first component satisfying the predicate.</returns>
    /// <exception cref="ExpectException">Thrown when no component satisfying the predicate is found within the timeout.</exception>
#if NET6_0_OR_GREATER
        public TComponent this[Func<TComponent, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = null]
#else
    public TComponent this[Func<TComponent, bool> predicate]
#endif
    {
        get
        {
            return Read(async () =>
            {
                TComponent component = null;

                async Task<bool> condition()
                {
                    _list = await LocateAllAsync().ConfigureAwait(false);

                    component = _list.FirstOrDefault(predicate);

                    if (component is null)
                    {
                        _elementsListHandler.Invalidate();

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                var timeout = _spaceOptions.Services.Get<TimeoutOptions>().Timeout;
                var pollingInterval = _spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;

                try
                {
                    await Waiter.UntilAsync(condition, timeout, pollingInterval).ConfigureAwait(false);
                }
                catch (TimeoutException)
                {
#if NET6_0_OR_GREATER
                    throw new ExpectException($"{_componentsListMetadata.Name} contain no matching {_componentsListMetadata.ComponentMetadata.Name} satisfying condition '{predicateExpression}'.");
#else
                    throw new ExpectException($"{_componentsListMetadata.Name} contain no matching {_componentsListMetadata.ComponentMetadata.Name} satisfying condition.");
#endif
                }

                return component;
            });
        }
    }

    /// <summary>
    /// Gets the first component in the list.
    /// </summary>
    /// <returns>The first component.</returns>
    public TComponent First()
    {
        return this[0];
    }

    /// <summary>
    /// Gets the first component satisfying the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match components against.</param>
    /// <returns>The first component satisfying the predicate.</returns>
#if NET6_0_OR_GREATER
        public TComponent First(Func<TComponent, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = null)
#else
    public TComponent First(Func<TComponent, bool> predicate)
#endif
    {
#if NET6_0_OR_GREATER
            return this[predicate, predicateExpression];
#else
        return this[predicate];
#endif
    }

    /// <summary>
    /// Gets the number of components in the list.
    /// </summary>
    public int Count
    {
        get
        {
            EnsureLocated();

            return _list.Count;
        }
    }

    /// <inheritdoc />
    public IEnumerator<TComponent> GetEnumerator()
    {
        EnsureLocated();

        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Performs the specified action on each component.
    /// </summary>
    /// <param name="action">The action to be performed.</param>
    public void ForEach(Action<TComponent> action)
    {
        EnsureLocated();

        foreach (var item in _list)
        {
            action(item);
        }
    }

    private void EnsureLocated()
    {
        if (_list == null)
        {
            _list = Read(() => LocateAllAsync());
        }
    }
}
