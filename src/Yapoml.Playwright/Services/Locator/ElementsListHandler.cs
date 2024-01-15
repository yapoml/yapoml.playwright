using Microsoft.Playwright;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;

namespace Yapoml.Playwright.Services.Locator
{
    public class ElementsListHandler : IElementsListHandler
    {
        private readonly IPage _driver;
        private readonly IElementHandler _parentElementHandler;
        private readonly IElementLocator _elementLocator;
        private readonly IEventSource _eventSource;

        public ElementsListHandler(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ComponentsListMetadata componentsListMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource)
        {
            _driver = driver;
            _parentElementHandler = parentElementHandler;
            _elementLocator = elementLocator;
            By = by;
            ComponentsListMetadata = componentsListMetadata;
            ElementHandlerRepository = elementHandlerRepository;
            _eventSource = eventSource;
        }

        public string By { get; }

        public ComponentsListMetadata ComponentsListMetadata { get; }

        public IElementHandlerRepository ElementHandlerRepository { get; }

        public void Invalidate()
        {
            _webElements = null;

            foreach (var elementHandler in ElementHandlerRepository.ElementHandlers)
            {
                elementHandler.Invalidate();
            }
        }

        IReadOnlyList<ILocator> _webElements;

        public IReadOnlyList<ILocator> LocateMany()
        {
            if (_webElements == null)
            {
                if (_parentElementHandler != null)
                {
                    var parentElement = _parentElementHandler.Locate();

                    _eventSource.ComponentEventSource.RaiseOnFindingComponents(By, ComponentsListMetadata);

                    _webElements = _elementLocator.FindElements(parentElement, By);
                }
                else
                {
                    _eventSource.ComponentEventSource.RaiseOnFindingComponents(By, ComponentsListMetadata);

                    _webElements = _driver.Locator(By).AllAsync().GetAwaiter().GetResult();
                }

                _eventSource.ComponentEventSource.RaiseOnFoundComponents(By, _driver, _webElements, ComponentsListMetadata);
            }

            return _webElements;
        }
    }
}
