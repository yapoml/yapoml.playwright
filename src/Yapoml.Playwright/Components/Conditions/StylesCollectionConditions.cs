using System;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions
{
    public class StylesCollectionConditions<TConditions> : Conditions<TConditions>
    {
        private readonly IElementHandler _elementHandler;

        public StylesCollectionConditions(TConditions conditions, IElementHandler elementHandler, TimeSpan timeout, TimeSpan pollingInterval, ILogger logger)
            : base(conditions, timeout, pollingInterval, logger)
        {
            _elementHandler = elementHandler;
        }

        public StringStyleConditions<TConditions> this[string styleName]
        {
            get
            {
                return new StringStyleConditions<TConditions>(_conditions, _elementHandler, styleName, _timeout, _pollingInterval, $"{styleName} style of the {_elementHandler.ComponentMetadata.Name}", _logger);
            }
        }

        // TODO make it strongly typed
        public StringStyleConditions<TConditions> Color => this["color"];

        public StringStyleConditions<TConditions> BackgroundColor => this["background-color"];

        public NumericStyleConditions<TConditions, double> Opacity =>
            new NumericStyleConditions<TConditions, double>(_conditions, _elementHandler, "opacity", _timeout, _pollingInterval, $"opacity of the {_elementHandler.ComponentMetadata.Name}", _logger);
    }
}
