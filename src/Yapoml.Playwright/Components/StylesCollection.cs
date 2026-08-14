using System;
using System.Threading.Tasks;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Provides access to computed CSS style values of a component.
/// Supports indexer-based access by CSS property name and shorthand properties for well-known styles.
/// </summary>
public class StylesCollection
{
    private readonly IElementHandler _elementHandler;

    private readonly Chain _chain;

    private readonly TimeSpan _timeout;
    private readonly TimeSpan _pollingInterval;

    /// <summary>
    /// Initializes a new instance of the <see cref="StylesCollection"/> class.
    /// </summary>
    /// <param name="elementHandler">The element handler for locating the component.</param>
    /// <param name="spaceOptions">The space options providing timeout configuration.</param>
    public StylesCollection(IElementHandler elementHandler, ISpaceOptions spaceOptions)
    {
        _elementHandler = elementHandler;

        _chain = Chain.Resolve(spaceOptions);

        _timeout = spaceOptions.Services.Get<TimeoutOptions>().Timeout;
        _pollingInterval = spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;
    }

    /// <summary>
    /// Gets the computed value of the specified CSS property.
    /// </summary>
    /// <param name="name">The CSS property name (e.g., "color", "font-size").</param>
    /// <returns>The computed style value as a string.</returns>
    public Task<string> this[string name]
    {
        get
        {
            return ReadAsync(async () =>
            {
                var style = await _elementHandler.Locate().EvaluateAsync($"node => window.getComputedStyle(node).getPropertyValue('{name}')").ConfigureAwait(false);

                return style.ToString();
            });
        }
    }

    /// <summary>
    /// Gets the computed <c>color</c> CSS property value.
    /// </summary>
    public Task<string> Color => this["color"];

    /// <summary>
    /// Gets the computed <c>background-color</c> CSS property value.
    /// </summary>
    public Task<string> BackgroundColor => this["background-color"];

    /// <summary>
    /// Gets the computed <c>opacity</c> CSS property value.
    /// </summary>
    public Task<string> Opacity => this["opacity"];

    private async Task<T> ReadAsync<T>(Func<Task<T>> read)
    {
        await _chain.RunAsync().ConfigureAwait(false);

        return await read().ConfigureAwait(false);
    }
}
