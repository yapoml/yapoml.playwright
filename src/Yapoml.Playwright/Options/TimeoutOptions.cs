using System;

namespace Yapoml.Playwright.Options
{
    public class TimeoutOptions
    {
        public TimeoutOptions(TimeSpan? timeout, TimeSpan? pollingInterval)
        {
            Timeout = timeout ?? TimeSpan.FromSeconds(30);
            PollingInterval = pollingInterval ?? TimeSpan.FromMilliseconds(200);
        }

        public TimeSpan Timeout { get; private set; }

        public TimeSpan PollingInterval { get; private set; }
    }
}
