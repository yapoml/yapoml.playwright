using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components
{
    public class StylesCollection
    {
        private readonly IElementHandler _elementHandler;

        private readonly TimeSpan _timeout;
        private readonly TimeSpan _pollingInterval;

        public StylesCollection(IElementHandler elementHandler, ISpaceOptions spaceOptions)
        {
            _elementHandler = elementHandler;

            _timeout = spaceOptions.Services.Get<TimeoutOptions>().Timeout;
            _pollingInterval = spaceOptions.Services.Get<TimeoutOptions>().PollingInterval;
        }

        public string this[string name]
        {
            get
            {
                var style = _elementHandler.Locate().EvaluateAsync($"node => window.getComputedStyle(node).getPropertyValue('{name}')").GetAwaiter().GetResult();

                return style.ToString();
            }
        }

        public string Color => this["color"];

        public string BackgroundColor => this["background-color"];

        public string Opacity => this["opacity"];

        private T RelocateOnStaleReference<T>(Func<T> act)
        {
            _elementHandler.Locate(_timeout, _pollingInterval);

            return act();
        }
    }
}
