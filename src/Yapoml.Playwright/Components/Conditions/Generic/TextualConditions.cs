using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yapoml.Framework;
using Yapoml.Framework.Logging;

namespace Yapoml.Playwright.Components.Conditions.Generic;

/// <inheritdoc cref="ITextualConditions{TConditions}"/>
public abstract class TextualConditions<TSelf> : Conditions<TSelf>, ITextualConditions<TSelf>
{
    /// <summary>
    /// A description of the subject being tested (e.g., "text of the search input").
    /// </summary>
    protected readonly string _subject;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextualConditions{TSelf}"/> class.
    /// </summary>
    protected TextualConditions(TSelf conditions, TimeSpan timeout, TimeSpan pollingInterval, string subject, ILogger logger)
        : base(conditions, timeout, pollingInterval, logger)
    {
        _subject = subject;
    }

    /// <summary>
    /// Gets the function that fetches the current textual value to be tested.
    /// </summary>
    protected abstract Func<Task<string>> FetchValueFunc { get; }

    /// <summary>
    /// Gets numeric conditions for the text length.
    /// </summary>
    public abstract NumericConditions<TSelf, int> Length { get; }

    private TSelf ExpectValue(string scopeName, TimeSpan? timeout, Func<string, bool> isSatisfied, Func<string, Exception, ExpectException> onTimeout)
    {
        timeout ??= _timeout;

        return Enqueue(async () =>
        {
            string latestValue = null;

            async Task<bool> condition()
            {
                latestValue = await FetchValueFunc().ConfigureAwait(false);

                return isSatisfied(latestValue);
            }

            try
            {
                using (var scope = _logger.BeginLogScope(scopeName))
                {
                    await scope.ExecuteAsync(() => Waiter.UntilAsync(condition, timeout.Value, _pollingInterval)).ConfigureAwait(false);
                }
            }
            catch (TimeoutException ex)
            {
                throw onTimeout(latestValue, ex);
            }
        });
    }

    /// <inheritdoc />
    public TSelf Is(string value, TimeSpan? timeout = default)
    {
        return Is(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf Is(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is {value}", timeout,
            latest => latest.Equals(value, comparisonType),
            (latest, ex) => new ExpectException(GetIsError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf IsNot(string value, TimeSpan? timeout = default)
    {
        return IsNot(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf IsNot(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is not {value}", timeout,
            latest => latest.Equals(value, comparisonType) == false,
            (latest, ex) => new ExpectException(GetIsNotError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf IsEmpty(TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is empty", timeout,
            latest => latest.Equals(string.Empty),
            (latest, ex) => new ExpectException(GetIsEmptyError(latest), ex));
    }

    /// <inheritdoc />
    public TSelf IsNotEmpty(TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is not empty", timeout,
            latest => !string.IsNullOrEmpty(latest),
            (latest, ex) => new ExpectException(GetIsNotEmptyError(latest), ex));
    }

    /// <inheritdoc />
    public TSelf StartsWith(string value, TimeSpan? timeout = default)
    {
        return StartsWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf StartsWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} starts with {value}", timeout,
            latest => latest.StartsWith(value, comparisonType),
            (latest, ex) => new ExpectException(GetStartsWithError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf DoesNotStartWith(string value, TimeSpan? timeout = default)
    {
        return DoesNotStartWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf DoesNotStartWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} does not start with {value}", timeout,
            latest => !latest.StartsWith(value, comparisonType),
            (latest, ex) => new ExpectException(GetDoesNotStartWithError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf EndsWith(string value, TimeSpan? timeout = default)
    {
        return EndsWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf EndsWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} ends with {value}", timeout,
            latest => latest.EndsWith(value, comparisonType),
            (latest, ex) => new ExpectException(GetEndsWithError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf DoesNotEndWith(string value, TimeSpan? timeout = default)
    {
        return DoesNotEndWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf DoesNotEndWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} does not end with {value}", timeout,
            latest => !latest.EndsWith(value, comparisonType),
            (latest, ex) => new ExpectException(GetDoesNotEndWithError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf Contains(string value, TimeSpan? timeout = default)
    {
        return Contains(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf Contains(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} contains {value}", timeout,
            latest => latest.IndexOf(value, comparisonType) >= 0,
            (latest, ex) => new ExpectException(GetContainsError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf DoesNotContain(string value, TimeSpan? timeout = default)
    {
        return DoesNotContain(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf DoesNotContain(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} does not contain {value}", timeout,
            latest => latest.IndexOf(value, comparisonType) == -1,
            (latest, ex) => new ExpectException(GetDoesNotContainError(latest, value), ex));
    }

    /// <inheritdoc />
    public TSelf Matches(Regex regex, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} matches {regex} regular expression", timeout,
            latest => regex.IsMatch(latest),
            (latest, ex) => new ExpectException(GetMatchesError(latest, regex), ex));
    }

    /// <inheritdoc />
    public TSelf DoesNotMatch(Regex regex, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} does not match {regex} regular expression", timeout,
            latest => !regex.IsMatch(latest),
            (latest, ex) => new ExpectException(GetDoesNotMatchError(latest, regex), ex));
    }

    /// <summary>Gets the error message when the "is" condition fails.</summary>
    protected abstract string GetIsError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "is not" condition fails.</summary>
    protected abstract string GetIsNotError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "is empty" condition fails.</summary>
    protected abstract string GetIsEmptyError(string latestValue);

    /// <summary>Gets the error message when the "is not empty" condition fails.</summary>
    protected abstract string GetIsNotEmptyError(string latestValue);

    /// <summary>Gets the error message when the "starts with" condition fails.</summary>
    protected abstract string GetStartsWithError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "does not start with" condition fails.</summary>
    protected abstract string GetDoesNotStartWithError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "ends with" condition fails.</summary>
    protected abstract string GetEndsWithError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "does not end with" condition fails.</summary>
    protected abstract string GetDoesNotEndWithError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "contains" condition fails.</summary>
    protected abstract string GetContainsError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "does not contain" condition fails.</summary>
    protected abstract string GetDoesNotContainError(string latestValue, string expectedValue);

    /// <summary>Gets the error message when the "matches" condition fails.</summary>
    protected abstract string GetMatchesError(string latestValue, Regex regex);

    /// <summary>Gets the error message when the "does not match" condition fails.</summary>
    protected abstract string GetDoesNotMatchError(string latestValue, Regex regex);
}
