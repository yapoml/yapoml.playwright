using System;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions;

/// <summary>
/// Numeric conditions for verifying the numeric value of a specific HTML attribute.
/// </summary>
/// <typeparam name="TConditions">The conditions type for fluent chaining.</typeparam>
/// <typeparam name="TNumber">The numeric type of the attribute value.</typeparam>
public class NumericAttributeConditions<TConditions, TNumber> : NumericConditions<TConditions, TNumber>
    where TNumber : struct, IComparable<TNumber>
{
    private readonly IElementHandler _elementHandler;
    private readonly string _attributeName;

    /// <inheritdoc />
    public NumericAttributeConditions(TConditions conditions, IElementHandler elementHandler, string attributeName, TimeSpan timeout, TimeSpan pollingInterval, string subject, ILogger logger)
        : base(conditions, timeout, pollingInterval, subject, logger)
    {
        _elementHandler = elementHandler;
        _attributeName = attributeName;
    }

    /// <inheritdoc />
    protected override Func<Task<TNumber?>> FetchValueFunc => async () =>
    {
        var value = await _elementHandler.Locate().GetAttributeAsync(_attributeName).ConfigureAwait(false);

        if (value is null)
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
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is not '{expectedValue}' yet.";
    }

    /// <inheritdoc />
    protected override string GetIsNotError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetIsGreaterThanError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not greater than '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string AtLeast(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not equal to or greater than '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetIsLessThanError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not less than '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetAtMostError(TNumber? latestValue, TNumber expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not equal to or less than '{expectedValue}'.";
    }

    private T RelocateOnStaleReference<T>(Func<T> act)
    {
        return act();
    }
}
