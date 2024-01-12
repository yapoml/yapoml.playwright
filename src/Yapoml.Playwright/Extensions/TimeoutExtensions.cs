using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;

namespace Yapoml.Playwright
{
    public static class TimeoutExtensions
    {
        public static ISpaceOptions WithTimeout(this ISpaceOptions spaceOptions, TimeSpan? timeout = null, TimeSpan? pollingInterval = null)
        {
            var timeoutOptions = spaceOptions.Services.Get<TimeoutOptions>();

            if (timeout.HasValue)
            {
                timeoutOptions.Timeout = timeout.Value;
            }

            if (pollingInterval.HasValue)
            {
                timeoutOptions.PollingInterval = pollingInterval.Value;
            }

            return spaceOptions;
        }
    }
}
