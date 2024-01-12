using System;

namespace Yapoml.Playwright.Options
{
    public class TimeoutOptions
    {
        public TimeoutOptions(TimeSpan timeout, TimeSpan pollingInterval)
        {
            Timeout = timeout;
            PollingInterval = pollingInterval;
        }

        public TimeSpan Timeout { get; set; }
        public TimeSpan PollingInterval { get; set; }
    }
}
