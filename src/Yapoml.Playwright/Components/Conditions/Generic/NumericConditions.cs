using System;
using System.Threading.Tasks;
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
    protected abstract Func<Task<TNumber?>> FetchValueFunc { get; }

    private TConditions ExpectValue(string scopeName, TimeSpan? timeout, Func<TNumber?, bool> isSatisfied, Func<TNumber?, Exception, ExpectException> onTimeout)
    {
        timeout ??= _timeout;

        return Enqueue(async () =>
        {
            TNumber? latestValue = null;

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
    public TConditions Is(TNumber value, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is {value}", timeout,
            latest => latest != null && latest.Equals(value),
            (latest, ex) => new ExpectException(GetIsError(latest, value), ex));
    }

    /// <inheritdoc />
    public TConditions IsNot(TNumber value, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is not {value}", timeout,
            latest => latest == null || !latest.Equals(value),
            (latest, ex) => new ExpectException(GetIsNotError(latest, value), ex));
    }

    /// <inheritdoc />
    public TConditions IsGreaterThan(TNumber value, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is greater than {value}", timeout,
            latest => latest != null && ((IComparable<TNumber>)latest).CompareTo(value) > 0,
            (latest, ex) => new ExpectException(GetIsGreaterThanError(latest, value), ex));
    }

    /// <inheritdoc />
    public TConditions AtLeast(TNumber value, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is equal to or greater than {value}", timeout,
            latest => latest != null && ((IComparable<TNumber>)latest).CompareTo(value) >= 0,
            (latest, ex) => new ExpectException(AtLeast(latest, value), ex));
    }

    /// <inheritdoc />
    public TConditions IsLessThan(TNumber value, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is less than {value}", timeout,
            latest => latest != null && ((IComparable<TNumber>)latest).CompareTo(value) < 0,
            (latest, ex) => new ExpectException(GetIsLessThanError(latest, value), ex));
    }

    /// <inheritdoc />
    public TConditions AtMost(TNumber value, TimeSpan? timeout = default)
    {
        return ExpectValue($"Expect {_subject} is less than {value}", timeout,
            latest => latest != null && ((IComparable<TNumber>)latest).CompareTo(value) <= 0,
            (latest, ex) => new ExpectException(GetAtMostError(latest, value), ex));
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
