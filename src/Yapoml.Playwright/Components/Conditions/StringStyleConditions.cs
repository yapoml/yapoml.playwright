﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components.Conditions
{
    public class StringStyleConditions<TConditions> : TextualConditions<TConditions>
    {
        private readonly IElementHandler _elementHandler;
        private readonly string _styleName;

        public StringStyleConditions(TConditions conditions, IElementHandler elementHandler, string styleName, TimeSpan timeout, TimeSpan pollingInterval, string subject, ILogger logger)
            : base(conditions, timeout, pollingInterval, subject, logger)
        {
            _elementHandler = elementHandler;
            _styleName = styleName;
        }

        protected override Func<string> FetchValueFunc => () => RelocateOnStaleReference(() => Task.Run(() => _elementHandler.Locate().EvaluateAsync($"node => window.getComputedStyle(node).getPropertyValue('{_styleName}')")).GetAwaiter().GetResult().ToString());

        public override NumericConditions<TConditions, int> Length
            => new TextualLengthConditons<TConditions>(_conditions, _timeout, _pollingInterval, FetchValueFunc, $"{_styleName} style of {_elementHandler.ComponentMetadata.Name}", _logger);

        protected override string GetIsError(string latestValue, string expectedValue)
        {
            return $"Style {_styleName} of the {_elementHandler.ComponentMetadata.Name} is not '{expectedValue}',{GetDifference("it was:", expectedValue, latestValue)}";
        }

        protected override string GetIsNotError(string latestValue, string expectedValue)
        {
            return $"Style {_styleName} of the {_elementHandler.ComponentMetadata.Name} is '{latestValue}', when expected to be not.";
        }

        protected override string GetIsEmptyError(string latestValue)
        {
            return $"Style {_styleName} '{latestValue}' of the {_elementHandler.ComponentMetadata.Name} is not empty, when expected to be empty.";
        }

        protected override string GetIsNotEmptyError(string latestValue)
        {
            return $"Style {_styleName} of the {_elementHandler.ComponentMetadata.Name} is empty, when expected to be not empty.";
        }

        protected override string GetStartsWithError(string latestValue, string expectedValue)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component does not start with '{expectedValue}' yet.";
        }

        protected override string GetDoesNotStartWithError(string latestValue, string expectedValue)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component starts with '{expectedValue}'.";
        }

        protected override string GetEndsWithError(string latestValue, string expectedValue)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component doesn't end with '{expectedValue}'.";
        }

        protected override string GetDoesNotEndWithError(string latestValue, string expectedValue)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component ends with '{expectedValue}'.";
        }

        protected override string GetContainsError(string latestValue, string expectedValue)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component doesn't contain '{expectedValue}' yet.";
        }

        protected override string GetDoesNotContainError(string latestValue, string expectedValue)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component contains '{expectedValue}'.";
        }

        protected override string GetMatchesError(string latestValue, Regex regex)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component doesn't match '{regex}'.";
        }

        protected override string GetDoesNotMatchError(string latestValue, Regex regex)
        {
            return $"Style '{_styleName} = {latestValue}' of the {_elementHandler.ComponentMetadata.Name} component matches '{regex}'.";
        }

        private T RelocateOnStaleReference<T>(Func<T> act)
        {
            return act();
        }
    }
}
