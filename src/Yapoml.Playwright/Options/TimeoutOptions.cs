using System;

namespace Yapoml.Playwright.Options;

/// <summary>
/// Holds the timeout and polling interval configuration for all waiting operations.
/// </summary>
public class TimeoutOptions
{
    /// <summary>
    /// Initializes a new instance with the specified timeout and polling interval.
    /// </summary>
    /// <param name="timeout">The maximum duration to wait. Defaults to 30 seconds if <c>null</c>.</param>
    /// <param name="pollingInterval">The interval between condition checks. Defaults to 200ms if <c>null</c>.</param>
    public TimeoutOptions(TimeSpan? timeout, TimeSpan? pollingInterval)
    {
        Timeout = timeout ?? TimeSpan.FromSeconds(30);
        PollingInterval = pollingInterval ?? TimeSpan.FromMilliseconds(200);
    }

    /// <summary>
    /// Gets the maximum duration to wait for a condition to be met.
    /// </summary>
    public TimeSpan Timeout { get; private set; }

    /// <summary>
    /// Gets the interval between polling attempts when waiting for a condition.
    /// </summary>
    public TimeSpan PollingInterval { get; private set; }
}
