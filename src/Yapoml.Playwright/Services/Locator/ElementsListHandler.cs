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
    public virtual ILocator Locate()
    {
        if (From == ElementLocatorContext.Parent && _parentElementHandler != null)
        {
            return _parentElementHandler.Locate().Locator(By);
        }
        else if (From == ElementLocatorContext.Parent || From == ElementLocatorContext.Root)
        {
            return _driver.Locator(By);
        }
        else
        {
            throw new NotImplementedException($"Element locator context {From} is not supported yet.");
        }
    }

    /// <inheritdoc />
    public virtual async Task<IReadOnlyList<ILocator>> LocateManyAsync()
    {
        if (_elements == null)
        {
            _eventSource.ComponentEventSource.RaiseOnFindingComponents(By, ComponentsListMetadata);

            _elements = await FindAllFromAsync(Locate()).ConfigureAwait(false);

            _eventSource.ComponentEventSource.RaiseOnFoundComponents(By, _driver, _elements, ComponentsListMetadata);
        }

        return _elements;
    }

    /// <summary>
    /// Finds all matching locator instances from the given locator.
    /// </summary>
    /// <param name="locator">The base locator to enumerate.</param>
    /// <returns>A read-only list of all matching locators.</returns>
    protected virtual Task<IReadOnlyList<ILocator>> FindAllFromAsync(ILocator locator)
    {
        return locator.AllAsync();
    }
}
