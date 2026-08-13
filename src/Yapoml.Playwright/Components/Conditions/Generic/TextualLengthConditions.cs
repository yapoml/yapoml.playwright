using System;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;

namespace Yapoml.Playwright.Components.Conditions.Generic;

/// <summary>
/// Numeric conditions for verifying the length of a textual value.
/// </summary>
/// <typeparam name="TConditions">The conditions type for fluent chaining.</typeparam>
public class TextualLengthConditons<TConditions> : NumericConditions<TConditions, int>
{
    private readonly Func<Task<string>> _getTextualValueFunc;

    private string _lastTextualValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextualLengthConditons{TConditions}"/> class.
    /// </summary>
    public TextualLengthConditons(TConditions conditions, TimeSpan timeout, TimeSpan pollingInterval, Func<Task<string>> getTextualValueFunc, string subject, ILogger logger)
    : base(conditions, timeout, pollingInterval, subject, logger)
    {
        _getTextualValueFunc = getTextualValueFunc;
    }

    /// <inheritdoc />
    protected override Func<Task<int?>> FetchValueFunc => async () =>
    {
        _lastTextualValue = await _getTextualValueFunc().ConfigureAwait(false);

        return _lastTextualValue.Length;
    };

    /// <inheritdoc />
    protected override string GetIsError(int? latestValue, int expectedValue)
    {
        return $"The {_subject} remains {latestValue} characters long, which is still not the {expectedValue} characters expected.\n  it was: {_lastTextualValue}";
    }

    /// <inheritdoc />
    protected override string GetIsNotError(int? latestValue, int expectedValue)
    {
        return $"The {_subject} remains {latestValue} characters long.\n  it was: {_lastTextualValue}";
    }

    /// <inheritdoc />
    protected override string GetIsGreaterThanError(int? latestValue, int expectedValue)
    {
        return $"The {_subject} remains {latestValue} characters long, which is still not greater than the {expectedValue} characters expected.\n  it was: {_lastTextualValue}";
    }

    /// <inheritdoc />
    protected override string AtLeast(int? latestValue, int expectedValue)
    {
        return $"The {_subject} remains {latestValue} characters long, which is still not equal to or greater than the {expectedValue} characters expected.\n  it was: {_lastTextualValue}";
    }

    /// <inheritdoc />
    protected override string GetIsLessThanError(int? latestValue, int expectedValue)
    {
        return $"The {_subject} remains {latestValue} characters long, which is still not less than the {expectedValue} characters expected.\n  it was: {_lastTextualValue}";
    }

    /// <inheritdoc />
    protected override string GetAtMostError(int? latestValue, int expectedValue)
    {
        return $"The {_subject} remains {latestValue} characters long, which is still not equal to or less than the {expectedValue} characters expected.\n  it was: {_lastTextualValue}";
    }
}
