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

        public ElementHandler(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ILocator webElement, ComponentMetadata componentMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource)
            : this(driver, parentElementHandler, elementLocator, by, componentMetadata, elementHandlerRepository, eventSource)
        {
            _webElement = webElement;
        }

        private ILocator _webElement;

        public string By { get; }

        public ComponentMetadata ComponentMetadata { get; }

        public IElementHandlerRepository ElementHandlerRepository { get; }

        public ILocator Locate()
        {
            return Locate(TimeSpan.Zero, TimeSpan.Zero);
        }

        public ILocator Locate(TimeSpan timeout, TimeSpan pollingInterval)
        {
            if (_webElement == null)
            {
                if (_parentElementHandler != null)
                {
                    var parentElement = _parentElementHandler.Locate(timeout, pollingInterval);

                    _eventSource.ComponentEventSource.RaiseOnFindingComponent(By, ComponentMetadata);

                    var stopwatch = Stopwatch.StartNew();

                    Exception lastException = null;

                    do
                    {
                        _webElement = _elementLocator.FindElement(parentElement, By);

                        break;

                    }
                    while (stopwatch.Elapsed <= timeout);

                    if (_webElement is null)
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
                        _webElement = _driver.Locator(By);

                        break;
                    }
                    while (stopwatch.Elapsed <= timeout);

                    if (_webElement is null)
                    {
                        throw lastException;
                    }
                }

                _eventSource.ComponentEventSource.RaiseOnFoundComponent(By, _driver, _webElement, ComponentMetadata);
            }

            return _webElement;
        }

        public void Invalidate()
        {
            _webElement = null;

            foreach (var elementHandler in ElementHandlerRepository.ElementHandlers)
            {
                elementHandler.Invalidate();
            }
        }
    }
}
