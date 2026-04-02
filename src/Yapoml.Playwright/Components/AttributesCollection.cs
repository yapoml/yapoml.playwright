using System;
using System.Threading.Tasks;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Provides access to HTML attribute values of a component.
/// Supports indexer-based access by attribute name and shorthand properties for well-known attributes.
/// </summary>
public class AttributesCollection
{
    private readonly IElementHandler _elementHandler;

    private readonly TimeSpan _timeout;
    private readonly TimeSpan _pollingInterval;

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributesCollection"/> class.
    /// </summary>
    /// <param name="elementHandler">The element handler for locating the component.</param>
    /// <param name="spaceOptions">The space options providing timeout configuration.</param>
    public AttributesCollection(IElementHandler elementHandler, ISpaceOptions spaceOptions)
    {
        _elementHandler = elementHandler;

        _timeout = spaceOptions.Services.Get<TimeoutOptions>().Timeout;
        _pollingInterval = spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;
    }

    /// <summary>
    /// Gets the value of the specified HTML attribute.
    /// </summary>
    /// <param name="name">The name of the attribute to retrieve.</param>
    /// <returns>The attribute value, or <c>null</c> if the attribute does not exist.</returns>
    public string this[string name]
    {
        get
        {
            return RelocateOnStaleReference(() => Task.Run(() => _elementHandler.Locate().GetAttributeAsync(name)).GetAwaiter().GetResult());
        }
    }

    /// <summary>
    /// Gets the value of the <c>href</c> attribute.
    /// </summary>
    public string Href => this["href"];

    /// <summary>
    /// Gets the value of the <c>value</c> attribute.
    /// </summary>
    public string Value => this["value"];

    /// <summary>
    /// Gets the value of the <c>class</c> attribute.
    /// </summary>
    public string Class => this["class"];

    /// <summary>
    /// Gets the value of the <c>style</c> attribute.
    /// </summary>
    public string Style => this["style"];

    private T RelocateOnStaleReference<T>(Func<T> act)
    {
        return act();
    }
}
