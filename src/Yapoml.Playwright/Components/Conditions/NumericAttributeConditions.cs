using System;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions
{
    public class NumericAttributeConditions<TConditions, TNumber> : NumericConditions<TConditions, TNumber>
        where TNumber : struct, IComparable<TNumber>
    {
        private readonly IElementHandler _elementHandler;
        private readonly string _attributeName;

        public NumericAttributeConditions(TConditions conditions, IElementHandler elementHandler, string attributeName, TimeSpan timeout, TimeSpan pollingInterval, string subject, ILogger logger)
            : base(conditions, timeout, pollingInterval, subject, logger)
        {
            _elementHandler = elementHandler;
            _attributeName = attributeName;
        }

        protected override Func<TNumber?> FetchValueFunc => () =>
        {
            var value = RelocateOnStaleReference(async () => await _elementHandler.Locate().GetAttributeAsync(_attributeName));

            if (value is null)
            {
                return null;
            }
            else
            {
                return (TNumber)Convert.ChangeType(value, typeof(TNumber));
            }
        };

        protected override string GetIsError(TNumber? latestValue, TNumber expectedValue)
        {
            return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is not '{expectedValue}' yet.";
        }

        protected override string GetIsNotError(TNumber? latestValue, TNumber expectedValue)
        {
            return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still '{expectedValue}'.";
        }

        protected override string GetIsGreaterThanError(TNumber? latestValue, TNumber expectedValue)
        {
            return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not greater than '{expectedValue}'.";
        }

        protected override string AtLeast(TNumber? latestValue, TNumber expectedValue)
        {
            return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not equal to or greater than '{expectedValue}'.";
        }

        protected override string GetIsLessThanError(TNumber? latestValue, TNumber expectedValue)
        {
            return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not less than '{expectedValue}'.";
        }

        protected override string GetAtMostError(TNumber? latestValue, TNumber expectedValue)
        {
            return $"Attribute '{_attributeName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component is still not equal to or less than '{expectedValue}'.";
        }

        private T RelocateOnStaleReference<T>(Func<T> act)
        {
            return act();
        }
    }
}
