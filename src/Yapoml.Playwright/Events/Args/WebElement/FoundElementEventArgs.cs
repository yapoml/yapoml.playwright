using Microsoft.Playwright;
using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element;

/// <summary>
/// Event arguments raised when a single component has been successfully located.
/// </summary>
public class FoundElementEventArgs : EventArgs
{
    /// <inheritdoc />
    public FoundElementEventArgs(string by, IPage driver, ILocator element, ComponentMetadata componentMetadata)
    {
        By = by;
        Driver = driver;
        Element = element;
        ComponentMetadata = componentMetadata;
    }

    /// <summary>
    /// Gets the locator selector that was used to find the component.
    /// </summary>
    public string By { get; }

    /// <summary>
    /// Gets the Playwright page instance.
    /// </summary>
    public IPage Driver { get; }

    /// <summary>
    /// Gets the located Playwright locator for the element.
    /// </summary>
    public ILocator Element { get; }

    /// <summary>
    /// Gets the metadata describing the located component.
    /// </summary>
    public ComponentMetadata ComponentMetadata { get; }
}
