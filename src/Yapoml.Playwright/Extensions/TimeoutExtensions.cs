using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;

namespace Yapoml.Playwright;

/// <summary>
/// Extension methods for configuring global timeout and polling interval settings.
/// </summary>
public static class TimeoutExtensions
{
    /// <summary>
    /// Configures the default timeout and polling interval for all waiting operations.
    /// </summary>
    /// <param name="spaceOptions">The space options to configure.</param>
    /// <param name="timeout">The maximum duration to wait for conditions. Defaults to 30 seconds if not specified.</param>
    /// <param name="pollingInterval">The interval between condition checks. Defaults to 200ms if not specified.</param>
    /// <returns>The same space options instance for further chaining.</returns>
    public static ISpaceOptions WithTimeout(this ISpaceOptions spaceOptions, TimeSpan? timeout = null, TimeSpan? pollingInterval = null)
    {
        var timeoutOptions = new TimeoutOptions(timeout, pollingInterval);

        spaceOptions.WithService(timeoutOptions);

        return spaceOptions;
    }
}
