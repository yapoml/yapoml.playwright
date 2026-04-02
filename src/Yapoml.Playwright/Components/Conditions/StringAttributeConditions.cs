using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions;

/// <summary>
/// Textual conditions for verifying the string value of a specific HTML attribute.
/// </summary>
/// <typeparam name="TConditions">The conditions type for fluent chaining.</typeparam>
public class StringAttributeConditions<TConditions> : TextualConditions<TConditions>
{
    private readonly IElementHandler _elementHandler;
    private readonly string _attributeName;

    /// <inheritdoc />
    public StringAttributeConditions(TConditions conditions, IElementHandler elementHandler, string attributeName, TimeSpan timeout, TimeSpan pollingInterval, string subject, ILogger logger)
        : base(conditions, timeout, pollingInterval, subject, logger)
    {
        _elementHandler = elementHandler;
        _attributeName = attributeName;
    }

    /// <inheritdoc />
    protected override Func<string> FetchValueFunc => () => RelocateOnStaleReference(() => Task.Run(() => _elementHandler.Locate().GetAttributeAsync(_attributeName)).GetAwaiter().GetResult());

    /// <inheritdoc />
    public override NumericConditions<TConditions, int> Length
        => new TextualLengthConditons<TConditions>(_conditions, _timeout, _pollingInterval, FetchValueFunc, $"{_attributeName} attribute of {_elementHandler.ComponentMetadata.Name}", _logger);

    /// <inheritdoc />
    protected override string GetIsError(string latestValue, string expectedValue)
    {
        return $"Attribute {_attributeName} of the {_elementHandler.ComponentMetadata.Name} is not '{expectedValue}',{GetDifference("it was:", expectedValue, latestValue)}";
    }

    /// <inheritdoc />
    protected override string GetIsNotError(string latestValue, string expectedValue)
    {
        return $"Attribute {_attributeName} of the {_elementHandler.ComponentMetadata.Name} component is '{latestValue}', when expected to be not.";
    }

    /// <inheritdoc />
    protected override string GetIsEmptyError(string latestValue)
    {
        return $"Attribute {_attributeName} '{latestValue}' of the {_elementHandler.ComponentMetadata.Name} is not empty, when expected to be empty.";
    }

    /// <inheritdoc />
    protected override string GetIsNotEmptyError(string latestValue)
    {
        return $"Attribute {_attributeName} of the {_elementHandler.ComponentMetadata.Name} is empty, when expected to be not empty.";
    }

    /// <inheritdoc />
    protected override string GetStartsWithError(string latestValue, string expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component does not start with '{expectedValue}' yet.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotStartWithError(string latestValue, string expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component starts with '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetEndsWithError(string latestValue, string expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component does not end with'{expectedValue}' yet.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotEndWithError(string latestValue, string expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component ends with '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetContainsError(string latestValue, string expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component doesn't contain '{expectedValue}' yet.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotContainError(string latestValue, string expectedValue)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component contains '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetMatchesError(string latestValue, Regex regex)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component doesn't match '{regex}'.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotMatchError(string latestValue, Regex regex)
    {
        return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component matches '{regex}'.";
    }

    private T RelocateOnStaleReference<T>(Func<T> act)
    {
        return act();
    }
}
