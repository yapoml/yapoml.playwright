using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Events.Args.Element;

/// <summary>
/// Event arguments raised when the framework begins searching for a single component.
/// </summary>
public class FindingElementEventArgs : EventArgs
{
    /// <inheritdoc />
    public FindingElementEventArgs(string by, ComponentMetadata componentMetadata)
    {
        By = by;
        ComponentMetadata = componentMetadata;
    }

    /// <summary>
    /// Gets the locator selector used to find the component.
    /// </summary>
    public string By { get; }

    /// <summary>
    /// Gets the metadata describing the component being searched for.
    /// </summary>
    public ComponentMetadata ComponentMetadata { get; }
}
