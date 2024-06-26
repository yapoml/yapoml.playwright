﻿using Microsoft.Playwright;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

namespace Yapoml.Playwright.Components
{
    public class BaseComponentList<TComponent, TListConditions, TComponentConditions> : IReadOnlyList<TComponent>
        where TComponent : BaseComponent
        where TListConditions : BaseComponentListConditions<TListConditions, TComponentConditions>
        where TComponentConditions : BaseComponentConditions<TComponentConditions>
    {
        protected TListConditions listConditions;

        private IList<TComponent> _list;

        private readonly BasePage _page;
        private readonly BaseComponent _parentComponent;
        private readonly IPage _driver;
        protected readonly IElementsListHandler _elementsListHandler;
        private readonly ComponentsListMetadata _componentsListMetadata;
        private readonly IEventSource _eventSource;
        private readonly ISpaceOptions _spaceOptions;

        protected readonly ILogger _logger;

        public BaseComponentList(BasePage page, BaseComponent parentComponent, IPage driver, IElementsListHandler elementsListHandler, ComponentsListMetadata componentsListMetadata, IEventSource eventSource, ISpaceOptions spaceOptions)
        {
            _page = page;
            _parentComponent = parentComponent;
            _driver = driver;
            _elementsListHandler = elementsListHandler;
            _componentsListMetadata = componentsListMetadata;
            _eventSource = eventSource;
            _spaceOptions = spaceOptions;

            _logger = _spaceOptions.Services.Get<ILogger>();
        }

        public TComponent this[int index]
        {
            get
            {
                var factory = _spaceOptions.Services.Get<IComponentFactory>();
                var locator = _spaceOptions.Services.Get<IElementLocator>();

                bool condition()
                {
                    var elements = _elementsListHandler.LocateMany();

                    _list = new List<TComponent>(elements.Select(e => factory.Create<TComponent, TListConditions, TComponentConditions>(_page, _parentComponent, _driver, new ElementHandler(_driver, null, locator, _elementsListHandler.By, _elementsListHandler.From, e, _componentsListMetadata.ComponentMetadata, _elementsListHandler.ElementHandlerRepository.CreateNestedRepository(), _eventSource), _componentsListMetadata.ComponentMetadata, _spaceOptions)));

                    if (elements.Count > index)
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
                    Waiter.Until(condition, timeout, pollingInterval);
                }
                catch (TimeoutException exp)
                {
                    throw new ExpectException($"Couldn't get a {_componentsListMetadata.ComponentMetadata.Name} by index {index} from {_list.Count} {_componentsListMetadata.Name}.", exp);
                }

                return _list[index];
            }
        }

        public TComponent this[string text]
        {
            get
            {
                var factory = _spaceOptions.Services.Get<IComponentFactory>();
                var locator = _spaceOptions.Services.Get<IElementLocator>();

                TComponent component = null;

                bool condition()
                {
                    var elements = _elementsListHandler.LocateMany();

                    _list = new List<TComponent>(elements.Select(e => factory.Create<TComponent, TListConditions, TComponentConditions>(_page, _parentComponent, _driver, new ElementHandler(_driver, null, locator, _elementsListHandler.By, _elementsListHandler.From, e, _componentsListMetadata.ComponentMetadata, _elementsListHandler.ElementHandlerRepository.CreateNestedRepository(), _eventSource), _componentsListMetadata.ComponentMetadata, _spaceOptions)));

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
                    Waiter.Until(condition, timeout, pollingInterval);
                }
                catch (TimeoutException exp)
                {
                    throw new ExpectException($"{_componentsListMetadata.Name} contain no matching {_componentsListMetadata.ComponentMetadata.Name} with '{text}' text.", exp);
                }

                return component;
            }
        }

#if NET6_0_OR_GREATER
        public TComponent this[Func<TComponent, bool> predicate, [CallerArgumentExpression("predicate")] string predicateExpression = null]
#else
        public TComponent this[Func<TComponent, bool> predicate]
#endif
        {
            get
            {
                var factory = _spaceOptions.Services.Get<IComponentFactory>();
                var locator = _spaceOptions.Services.Get<IElementLocator>();

                TComponent component = null;

                bool condition()
                {
                    var elements = _elementsListHandler.LocateMany();

                    _list = new List<TComponent>(elements.Select(e => factory.Create<TComponent, TListConditions, TComponentConditions>(_page, _parentComponent, _driver, new ElementHandler(_driver, null, locator, _elementsListHandler.By, _elementsListHandler.From, e, _componentsListMetadata.ComponentMetadata, _elementsListHandler.ElementHandlerRepository.CreateNestedRepository(), _eventSource), _componentsListMetadata.ComponentMetadata, _spaceOptions)));

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
                    Waiter.Until(condition, timeout, pollingInterval);
                }
                catch (TimeoutException exp)
                {
#if NET6_0_OR_GREATER
                    throw new ExpectException($"{_componentsListMetadata.Name} contain no matching {_componentsListMetadata.ComponentMetadata.Name} satisfying condition '{predicateExpression}'.");
#else
                    throw new ExpectException($"{_componentsListMetadata.Name} contain no matching {_componentsListMetadata.ComponentMetadata.Name} satisfying condition.");
#endif
                }

                return component;
            }
        }

        public TComponent First()
        {
            return this[0];
        }

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

        public int Count
        {
            get
            {
                EnsureLocated();

                return _list.Count;
            }
        }

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
                var factory = _spaceOptions.Services.Get<IComponentFactory>();
                var locator = _spaceOptions.Services.Get<IElementLocator>();

                var elements = _elementsListHandler.LocateMany();

                _list = new List<TComponent>(elements.Select(e => factory.Create<TComponent, TListConditions, TComponentConditions>(_page, _parentComponent, _driver, new ElementHandler(_driver, null, locator, _elementsListHandler.By, _elementsListHandler.From, e, _componentsListMetadata.ComponentMetadata, _elementsListHandler.ElementHandlerRepository.CreateNestedRepository(), _eventSource), _componentsListMetadata.ComponentMetadata, _spaceOptions)));
            }
        }
    }
}
