using System;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions;

/// <summary>
/// Provides awaitable conditions for verifying CSS styles of a component.
/// Supports indexer-based access and shorthand properties for well-known styles.
/// </summary>
/// <typeparam name="TConditions">The conditions type for fluent chaining.</typeparam>
public class StylesCollectionConditions<TConditions> : Conditions<TConditions>
{
    private readonly IElementHandler _elementHandler;

    /// <inheritdoc />
    public StylesCollectionConditions(TConditions conditions, IElementHandler elementHandler, TimeSpan timeout, TimeSpan pollingInterval, ILogger logger)
        : base(conditions, timeout, pollingInterval, logger)
    {
        _elementHandler = elementHandler;
    }

    /// <summary>
    /// Gets textual conditions for the specified CSS style property.
    /// </summary>
    /// <param name="styleName">The CSS property name.</param>
    /// <returns>Textual conditions for the computed style value.</returns>
    public StringStyleConditions<TConditions> this[string styleName]
    {
        get
        {
            return new StringStyleConditions<TConditions>(_conditions, _elementHandler, styleName, _timeout, _pollingInterval, $"{styleName} style of the {_elementHandler.ComponentMetadata.Name}", _logger) { Chain = Chain };
        }
    }

    /// <summary>
    /// Gets textual conditions for the <c>color</c> CSS property.
    /// </summary>
    public StringStyleConditions<TConditions> Color => this["color"];

    /// <summary>
    /// Gets textual conditions for the <c>background-color</c> CSS property.
    /// </summary>
    public StringStyleConditions<TConditions> BackgroundColor => this["background-color"];

    /// <summary>
    /// Gets numeric conditions for the <c>opacity</c> CSS property.
    /// </summary>
    public NumericStyleConditions<TConditions, double> Opacity =>
        new NumericStyleConditions<TConditions, double>(_conditions, _elementHandler, "opacity", _timeout, _pollingInterval, $"opacity of the {_elementHandler.ComponentMetadata.Name}", _logger) { Chain = Chain };
}
