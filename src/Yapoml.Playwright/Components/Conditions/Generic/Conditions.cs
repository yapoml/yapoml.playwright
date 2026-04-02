using System;
using Yapoml.Framework.Logging;

namespace Yapoml.Playwright.Components.Conditions.Generic;

/// <summary>
/// Base class for all condition objects, providing shared timeout, polling interval, and utility methods.
/// </summary>
/// <typeparam name="TConditions">The conditions type for fluent chaining.</typeparam>
public abstract class Conditions<TConditions>
{
    /// <summary>The conditions instance for fluent chaining.</summary>
    protected readonly TConditions _conditions;
    /// <summary>The maximum duration to wait for a condition.</summary>
    protected readonly TimeSpan _timeout;
    /// <summary>The interval between condition checks.</summary>
    protected readonly TimeSpan _pollingInterval;
    /// <summary>The logger for tracing condition evaluations.</summary>
    protected readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Conditions{TConditions}"/> class.
    /// </summary>
    public Conditions(TConditions conditions, TimeSpan timeout, TimeSpan pollingInterval, ILogger logger)
    {
        _conditions = conditions;
        _timeout = timeout;
        _pollingInterval = pollingInterval;
        _logger = logger;
    }

    /// <summary>
    /// Computes a visual string difference between two values for error messages.
    /// </summary>
    protected string GetDifference(string phrase, string first, string second)
    {
        if (first is not null && second is not null)
        {
            return Environment.NewLine + Formatters.StringFormatter.Format($"  {phrase} ", new string(' ', phrase.Length + 3), first, second) + Environment.NewLine;
        }
        else
        {
            return null;
        }
    }
}
