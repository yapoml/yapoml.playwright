using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions;

/// <summary>
/// Numeric conditions for verifying the numeric value of a specific CSS style property.
/// </summary>
/// <typeparam name="TConditions">The conditions type for fluent chaining.</typeparam>
/// <typeparam name="TNumber">The numeric type of the style value.</typeparam>
public class NumericStyleConditions<TConditions, TNumber> : NumericConditions<TConditions, TNumber>
    where TNumber : struct, IComparable<TNumber>
{
    private readonly IElementHandler _elementHandler;
    private readonly string _styleName;

    /// <inheritdoc />
    public NumericStyleConditions(TConditions conditions, IElementHandler elementHandler, string styleName, TimeSpan timeout, TimeSpan pollingInterval, string subject, ILogger logger)
        : base(conditions, timeout, pollingInterval, subject, logger)
    {
        _elementHandler = elementHandler;
        _styleName = styleName;
    }

    /// <inheritdoc />
    protected override Func<Task<TNumber?>> FetchValueFunc => async () =>
    {
        var value = (await _elementHandler.Locate().EvaluateAsync($"node => window.getComputedStyle(node).getPropertyValue('{_styleName}')").ConfigureAwait(false)).ToString();

        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        else
        {
            return (TNumber)Convert.ChangeType(value, typeof(TNumber));
        }
    };

    /// <inheritdoc />
    protected override string GetIsError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is not '{expectedValue}' yet.";
    }

    /// <inheritdoc />
    protected override string GetIsNotError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetIsGreaterThanError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not greater than '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string AtLeast(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not equal to or greater than '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetIsLessThanError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not less than '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetAtMostError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not equal to or less than '{expectedValue}'.";
    }
}
