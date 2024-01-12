using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components
{
    public class AttributesCollection
    {
        private readonly IElementHandler _elementHandler;

        private readonly TimeSpan _timeout;
        private readonly TimeSpan _pollingInterval;

        public AttributesCollection(IElementHandler elementHandler, ISpaceOptions spaceOptions)
        {
            _elementHandler = elementHandler;

            _timeout = spaceOptions.Services.Get<TimeoutOptions>().Timeout;
            _pollingInterval = spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;
        }

        public string this[string name]
        {
            get
            {
                return RelocateOnStaleReference(() => _elementHandler.Locate().GetAttributeAsync(name).GetAwaiter().GetResult());
            }
        }

        public string Href => this["href"];

        public string Value => this["value"];

        public string Class => this["class"];

        // or even more complex object?
        public string Style => this["style"];

        private T RelocateOnStaleReference<T>(Func<T> act)
        {
            return act();
        }
    }
}