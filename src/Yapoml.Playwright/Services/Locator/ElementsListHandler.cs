using Microsoft.Playwright;
using System;
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

        public ElementsListHandler(IPage driver, IElementHandler parentElementHandler, IElementLocator elementLocator, string by, ElementLocatorContext from, ComponentsListMetadata componentsListMetadata, IElementHandlerRepository elementHandlerRepository, IEventSource eventSource)
        {
            _driver = driver;
            _parentElementHandler = parentElementHandler;
            _elementLocator = elementLocator;
            By = by;
            From = from;
            ComponentsListMetadata = componentsListMetadata;
            ElementHandlerRepository = elementHandlerRepository;
            _eventSource = eventSource;
        }

        public string By { get; }

        public ElementLocatorContext From { get; }

        public ComponentsListMetadata ComponentsListMetadata { get; }

        public IElementHandlerRepository ElementHandlerRepository { get; }

        public virtual void Invalidate()
        {
            _elements = null;

            foreach (var elementHandler in ElementHandlerRepository.ElementHandlers)
            {
                elementHandler.Invalidate();
            }
        }

        IReadOnlyList<ILocator> _elements;

        public virtual IReadOnlyList<ILocator> LocateMany()
        {
            if (_elements == null)
            {
                if (From == ElementLocatorContext.Parent)
                {
                    if (_parentElementHandler != null)
                    {
                        var parentElement = _parentElementHandler.Locate();

                        _eventSource.ComponentEventSource.RaiseOnFindingComponents(By, ComponentsListMetadata);

                        _elements = FindAllFrom(parentElement.Locator(By));
                    }
                    else
                    {
                        _eventSource.ComponentEventSource.RaiseOnFindingComponents(By, ComponentsListMetadata);

                        _elements = FindAllFrom(_driver.Locator(By));
                    }
                }
                else if (From == ElementLocatorContext.Root)
                {
                    _eventSource.ComponentEventSource.RaiseOnFindingComponents(By, ComponentsListMetadata);

                    _elements = FindAllFrom(_driver.Locator(By));
                }
                else
                {
                    throw new NotImplementedException($"Element locator context {From} is not supported yet.");
                }

                _eventSource.ComponentEventSource.RaiseOnFoundComponents(By, _driver, _elements, ComponentsListMetadata);
            }

            return _elements;
        }

        protected virtual IReadOnlyList<ILocator> FindAllFrom(ILocator locator)
        {
            return locator.AllAsync().GetAwaiter().GetResult();
        }
    }
}
