using System;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions;

/// <summary>
/// Provides awaitable conditions for verifying HTML attributes of a component.
/// Supports indexer-based access and shorthand properties for well-known attributes.
/// </summary>
/// <typeparam name="TConditions">The conditions type for fluent chaining.</typeparam>
public class AttributesCollectionConditions<TConditions> : Conditions<TConditions>
{
    private readonly IElementHandler _elementHandler;

    /// <inheritdoc />
    public AttributesCollectionConditions(TConditions conditions, IElementHandler elementHandler, TimeSpan timeout, TimeSpan pollingInterval, ILogger logger)
        : base(conditions, timeout, pollingInterval, logger)
    {
        _elementHandler = elementHandler;
    }

    /// <summary>
    /// Gets textual conditions for the specified attribute.
    /// </summary>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>Textual conditions for the attribute value.</returns>
    public StringAttributeConditions<TConditions> this[string attributeName]
    {
        get
        {
            return new StringAttributeConditions<TConditions>(_conditions, _elementHandler, attributeName, _timeout, _pollingInterval, $"{attributeName} attribute of the {_elementHandler.ComponentMetadata.Name}", _logger) { Chain = Chain };
        }
    }

    /// <summary>
    /// Gets textual conditions for the <c>href</c> attribute.
    /// </summary>
    public StringAttributeConditions<TConditions> Href => this["href"];

    /// <summary>
    /// Gets textual conditions for the <c>value</c> attribute.
    /// </summary>
    public StringAttributeConditions<TConditions> Value => this["value"];

    /// <summary>
    /// Gets textual conditions for the <c>class</c> attribute.
    /// </summary>
    public StringAttributeConditions<TConditions> Class => this["class"];

    /// <summary>
    /// Gets textual conditions for the <c>style</c> attribute.
    /// </summary>
    public StringAttributeConditions<TConditions> Style => this["style"];

    /// <summary>
    /// Gets numeric conditions for the <c>width</c> attribute.
    /// </summary>
    public NumericAttributeConditions<TConditions, int> Width =>
        new NumericAttributeConditions<TConditions, int>(_conditions, _elementHandler, "width", _timeout, _pollingInterval, $"width attribute of the {_elementHandler.ComponentMetadata.Name}", _logger) { Chain = Chain };

    /// <summary>
    /// Gets numeric conditions for the <c>tabindex</c> attribute.
    /// </summary>
    public NumericAttributeConditions<TConditions, int> TabIndex =>
        new NumericAttributeConditions<TConditions, int>(_conditions, _elementHandler, "tabindex", _timeout, _pollingInterval, $"tabindex attribute of the {_elementHandler.ComponentMetadata.Name}", _logger) { Chain = Chain };
}
