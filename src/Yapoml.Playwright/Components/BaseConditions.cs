using System;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Base class for all awaitable condition objects, providing shared timeout and polling interval configuration.
/// </summary>
/// <typeparam name="TSelf">The concrete conditions type for fluent chaining.</typeparam>
public abstract class BaseConditions<TSelf>
{
    /// <summary>The concrete conditions instance for fluent chaining.</summary>
    protected TSelf _self;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseConditions{TSelf}"/> class.
    /// </summary>
    /// <param name="timeout">The maximum duration to wait for conditions.</param>
    /// <param name="pollingInterval">The interval between condition checks.</param>
    protected BaseConditions(TimeSpan timeout, TimeSpan pollingInterval)
    {
        Timeout = timeout;
        PollingInterval = pollingInterval;
    }

    /// <summary>Gets the maximum duration to wait for conditions.</summary>
    protected TimeSpan Timeout { get; }
    /// <summary>Gets the interval between condition checks.</summary>
    protected TimeSpan PollingInterval { get; }
}
