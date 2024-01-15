using Microsoft.Playwright;
using System;
using System.Diagnostics;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;

namespace Yapoml.Playwright.Services.Locator
{
    public class ElementHandler : IElementHandler
    {
        private readonly IPage _driver;
        private readonly IElementHandler _parentElementHandler;
        private readonly IElementLocator _elementLocator;
        private readonly IEventSource _eventSource;

        public ElementHandler(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ComponentMetadata componentMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource)
        {
            _driver = driver;
            _parentElementHandler = parentElementHandler;
            _elementLocator = elementLocator;
            By = by;
            ComponentMetadata = componentMetadata;
            ElementHandlerRepository = elementHandlerRepository;
            _eventSource = eventSource;
        }

        public ElementHandler(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ILocator element, ComponentMetadata componentMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource)
            : this(driver, parentElementHandler, elementLocator, by, componentMetadata, elementHandlerRepository, eventSource)
        {
            _element = element;
        }

        private ILocator _element;

        public string By { get; }

        public ComponentMetadata ComponentMetadata { get; }

        public IElementHandlerRepository ElementHandlerRepository { get; }

        public ILocator Locate()
        {
            return Locate(TimeSpan.Zero, TimeSpan.Zero);
        }

        public ILocator Locate(TimeSpan timeout, TimeSpan pollingInterval)
        {
            if (_element == null)
            {
                if (_parentElementHandler != null)
                {
                    var parentElement = _parentElementHandler.Locate(timeout, pollingInterval);

                    _eventSource.ComponentEventSource.RaiseOnFindingComponent(By, ComponentMetadata);

                    var stopwatch = Stopwatch.StartNew();

                    Exception lastException = null;

                    do
                    {
                        _element = _elementLocator.FindElement(parentElement, By);

                        break;

                    }
                    while (stopwatch.Elapsed <= timeout);

                    if (_element is null)
                    {
                        throw lastException;
                    }
                }
                else
                {
                    _eventSource.ComponentEventSource.RaiseOnFindingComponent(By, ComponentMetadata);

                    var stopwatch = Stopwatch.StartNew();

                    Exception lastException = null;

                    do
                    {
                        _element = _driver.Locator(By);

                        break;
                    }
                    while (stopwatch.Elapsed <= timeout);

                    if (_element is null)
                    {
                        throw lastException;
                    }
                }

                _eventSource.ComponentEventSource.RaiseOnFoundComponent(By, _driver, _element, ComponentMetadata);
            }

            return _element;
        }

        public void Invalidate()
        {
            _element = null;

            foreach (var elementHandler in ElementHandlerRepository.ElementHandlers)
            {
                elementHandler.Invalidate();
            }
        }
    }
}
