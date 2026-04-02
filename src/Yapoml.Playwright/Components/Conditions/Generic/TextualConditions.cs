using System;
using System.Text.RegularExpressions;
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
    protected abstract Func<string> FetchValueFunc { get; }

    /// <summary>
    /// Gets numeric conditions for the text length.
    /// </summary>
    public abstract NumericConditions<TSelf, int> Length { get; }

    /// <inheritdoc />
    public TSelf Is(string value, TimeSpan? timeout = default)
    {
        return Is(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf Is(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return latestValue.Equals(value, comparisonType);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetIsError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf IsNot(string value, TimeSpan? timeout = default)
    {
        return IsNot(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf IsNot(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return latestValue.Equals(value, comparisonType) == false;
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is not {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetIsNotError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf IsEmpty(TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return latestValue.Equals(string.Empty);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is empty"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetIsEmptyError(latestValue), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf IsNotEmpty(TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return !string.IsNullOrEmpty(latestValue);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is not empty"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetIsNotEmptyError(latestValue), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf StartsWith(string value, TimeSpan? timeout = default)
    {
        return StartsWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf StartsWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return latestValue.StartsWith(value, comparisonType);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} starts with {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetStartsWithError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf DoesNotStartWith(string value, TimeSpan? timeout = default)
    {
        return DoesNotStartWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf DoesNotStartWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return !latestValue.StartsWith(value, comparisonType);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} does not start with {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetDoesNotStartWithError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf EndsWith(string value, TimeSpan? timeout = default)
    {
        return EndsWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf EndsWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return latestValue.EndsWith(value, comparisonType);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} ends with {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetEndsWithError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf DoesNotEndWith(string value, TimeSpan? timeout = default)
    {
        return DoesNotEndWith(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf DoesNotEndWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return !latestValue.EndsWith(value, comparisonType);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} does not end with {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetDoesNotEndWithError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf Contains(string value, TimeSpan? timeout = default)
    {
        return Contains(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf Contains(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return latestValue.IndexOf(value, comparisonType) >= 0;
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} contains {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetContainsError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf DoesNotContain(string value, TimeSpan? timeout = default)
    {
        return DoesNotContain(value, StringComparison.CurrentCulture, timeout);
    }

    /// <inheritdoc />
    public TSelf DoesNotContain(string value, StringComparison comparisonType, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return latestValue.IndexOf(value, comparisonType) == -1;
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} does not contain {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetDoesNotContainError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf Matches(Regex regex, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return regex.IsMatch(latestValue);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} matches {regex} regular expression"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetMatchesError(latestValue, regex), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TSelf DoesNotMatch(Regex regex, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        string latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            return !regex.IsMatch(latestValue);
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} does not match {regex} regular expression"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetDoesNotMatchError(latestValue, regex), ex);
        }

        return _conditions;
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
