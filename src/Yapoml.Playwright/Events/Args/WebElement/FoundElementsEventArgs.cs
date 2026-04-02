using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element;

/// <summary>
/// Event arguments raised when a list of components has been successfully located.
/// </summary>
public class FoundElementsEventArgs : EventArgs
{
    /// <inheritdoc />
    public FoundElementsEventArgs(string by, IPage driver, IReadOnlyList<ILocator> elements, ComponentsListMetadata componentsListMetadata)
    {
        By = by;
        Driver = driver;
        Elements = elements;
        ComponentsListMetadata = componentsListMetadata;
    }

    /// <summary>
    /// Gets the locator selector that was used to find the components.
    /// </summary>
    public string By { get; }

    /// <summary>
    /// Gets the Playwright page instance.
    /// </summary>
    public IPage Driver { get; }

    /// <summary>
    /// Gets the located Playwright locators for the elements.
    /// </summary>
    public IReadOnlyList<ILocator> Elements { get; }

    /// <summary>
    /// Gets the metadata describing the located components list.
    /// </summary>
    public ComponentsListMetadata ComponentsListMetadata { get; }
}
