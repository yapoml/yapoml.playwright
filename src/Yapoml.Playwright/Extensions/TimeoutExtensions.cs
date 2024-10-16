using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;

namespace Yapoml.Playwright
{
    public static class TimeoutExtensions
    {
        public static ISpaceOptions WithTimeout(this ISpaceOptions spaceOptions, TimeSpan? timeout = null, TimeSpan? pollingInterval = null)
        {
            var timeoutOptions = new TimeoutOptions(timeout, pollingInterval);

            spaceOptions.WithService(timeoutOptions);

            return spaceOptions;
        }
    }
}
