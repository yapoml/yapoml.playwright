using System;
using Yapoml.Framework;
using Yapoml.Framework.Logging;

namespace Yapoml.Playwright.Components.Conditions.Generic;

/// <inheritdoc cref="INumericConditions{TConditions, TNumber}"/>
public abstract class NumericConditions<TConditions, TNumber> : Conditions<TConditions>, INumericConditions<TConditions, TNumber> where TNumber : struct, IComparable<TNumber>
{
    /// <summary>
    /// A description of the subject being tested.
    /// </summary>
    protected readonly string _subject;

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericConditions{TConditions, TNumber}"/> class.
    /// </summary>
    protected NumericConditions(TConditions conditions, TimeSpan timeout, TimeSpan pollingInterval, string subject, ILogger logger)
        : base(conditions, timeout, pollingInterval, logger)
    {
        _subject = subject;
    }

    /// <summary>
    /// Gets the function that fetches the current numeric value to be tested.
    /// </summary>
    protected abstract Func<TNumber?> FetchValueFunc { get; }

    /// <inheritdoc />
    public TConditions Is(TNumber value, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        TNumber? latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            if (latestValue != null)
            {
                return latestValue.Equals(value);
            }
            else
            {
                return false;
            }
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
    public TConditions IsNot(TNumber value, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        TNumber? latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            if (latestValue != null)
            {
                return !latestValue.Equals(value);
            }
            else
            {
                return true;
            }
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
    public TConditions IsGreaterThan(TNumber value, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        TNumber? latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            if (latestValue != null)
            {
                return ((IComparable<TNumber>)latestValue).CompareTo(value) > 0;
            }
            else
            {
                return false;
            }
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is greater than {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetIsGreaterThanError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TConditions AtLeast(TNumber value, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        TNumber? latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            if (latestValue != null)
            {
                return ((IComparable<TNumber>)latestValue).CompareTo(value) >= 0;
            }
            else
            {
                return false;
            }
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is equal to or greater than {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(AtLeast(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TConditions IsLessThan(TNumber value, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        TNumber? latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            if (latestValue != null)
            {
                return ((IComparable<TNumber>)latestValue).CompareTo(value) < 0;
            }
            else
            {
                return false;
            }
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is less than {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetIsLessThanError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <inheritdoc />
    public TConditions AtMost(TNumber value, TimeSpan? timeout = default)
    {
        timeout ??= _timeout;

        TNumber? latestValue = null;

        bool condition()
        {
            latestValue = FetchValueFunc();

            if (latestValue != null)
            {
                return ((IComparable<TNumber>)latestValue).CompareTo(value) <= 0;
            }
            else
            {
                return false;
            }
        }

        try
        {
            using (var scope = _logger.BeginLogScope($"Expect {_subject} is less than {value}"))
            {
                scope.Execute(() =>
                {
                    Waiter.Until(condition, timeout.Value, _pollingInterval);
                });
            }
        }
        catch (TimeoutException ex)
        {
            throw new ExpectException(GetAtMostError(latestValue, value), ex);
        }

        return _conditions;
    }

    /// <summary>Gets the error message when the "is" condition fails.</summary>
    protected abstract string GetIsError(TNumber? latestValue, TNumber expectedValue);

    /// <summary>Gets the error message when the "is not" condition fails.</summary>
    protected abstract string GetIsNotError(TNumber? latestValue, TNumber expectedValue);

    /// <summary>Gets the error message when the "is greater than" condition fails.</summary>
    protected abstract string GetIsGreaterThanError(TNumber? latestValue, TNumber expectedValue);

    /// <summary>Gets the error message when the "at least" condition fails.</summary>
    protected abstract string AtLeast(TNumber? latestValue, TNumber expectedValue);

    /// <summary>Gets the error message when the "is less than" condition fails.</summary>
    protected abstract string GetIsLessThanError(TNumber? latestValue, TNumber expectedValue);

    /// <summary>Gets the error message when the "at most" condition fails.</summary>
    protected abstract string GetAtMostError(TNumber? latestValue, TNumber expectedValue);
}
