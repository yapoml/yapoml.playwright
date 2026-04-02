using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element;

/// <summary>
/// Event arguments raised when the framework begins searching for a list of components.
/// </summary>
public class FindingElementsEventArgs : EventArgs
{
    /// <inheritdoc />
    public FindingElementsEventArgs(string by, ComponentsListMetadata componentsLIstMetadata)
    {
        By = by;
        ComponentsListMetadata = componentsLIstMetadata;
    }

    /// <summary>
    /// Gets the locator selector used to find the components.
    /// </summary>
    public string By { get; }

    /// <summary>
    /// Gets the metadata describing the components list being searched for.
    /// </summary>
    public ComponentsListMetadata ComponentsListMetadata { get; }
}
