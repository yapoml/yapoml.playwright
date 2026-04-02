using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;

namespace Yapoml.Playwright.Services.Locator;

/// <summary>
/// Default implementation of <see cref="IElementsListHandler"/> that locates and caches a list of elements,
/// supporting both parent-relative and root-relative search contexts.
/// </summary>
public class ElementsListHandler : IElementsListHandler
{
    private readonly IPage _driver;
    private readonly IElementHandler _parentElementHandler;
    private readonly IElementLocator _elementLocator;
    private readonly IEventSource _eventSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElementsListHandler"/> class.
    /// </summary>
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

    /// <inheritdoc />
    public string By { get; }

    /// <inheritdoc />
    public ElementLocatorContext From { get; }

    /// <inheritdoc />
    public ComponentsListMetadata ComponentsListMetadata { get; }

    /// <inheritdoc />
    public IElementHandlerRepository ElementHandlerRepository { get; }

    /// <inheritdoc />
    public virtual void Invalidate()
    {
        _elements = null;

        foreach (var elementHandler in ElementHandlerRepository.ElementHandlers)
        {
            elementHandler.Invalidate();
        }
    }

    IReadOnlyList<ILocator> _elements;

    /// <inheritdoc />
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

    /// <summary>
    /// Finds all matching locator instances from the given locator.
    /// </summary>
    /// <param name="locator">The base locator to enumerate.</param>
    /// <returns>A read-only list of all matching locators.</returns>
    protected virtual IReadOnlyList<ILocator> FindAllFrom(ILocator locator)
    {
        return Task.Run(() => locator.AllAsync()).GetAwaiter().GetResult();
    }
}
