﻿using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Components.Conditions
{
    /// <summary>
    /// Various conditions for page title.
    /// </summary>
    /// <typeparam name="TConditions">Fluent original instance for chaining conditions.</typeparam>
    public class TitleConditions<TConditions> : TextualConditions<TConditions>
    {
        private readonly IPage _driver;
        private readonly PageMetadata _pageMetadata;

        public TitleConditions(IPage driver, TConditions conditions, TimeSpan timeout, TimeSpan pollingInterval, PageMetadata pageMetadata, ILogger logger)
            : base(conditions, timeout, pollingInterval, $"{pageMetadata.Name} page title", logger)
        {
            _driver = driver;
            _pageMetadata = pageMetadata;
        }

        protected override Func<string> FetchValueFunc => () => Task.Run(() => _driver.TitleAsync()).GetAwaiter().GetResult();

        public override NumericConditions<TConditions, int> Length
            => new TextualLengthConditons<TConditions>(_conditions, _timeout, _pollingInterval, FetchValueFunc, $"{_pageMetadata.Name} page title", _logger);

        protected override string GetIsError(string latestValue, string expectedValue)
        {
            return $"{_pageMetadata.Name} page title is not '{expectedValue}',{GetDifference("it was:", expectedValue, latestValue)}";
        }

        protected override string GetIsNotError(string latestValue, string expectedValue)
        {
            return $"{_pageMetadata.Name} page title is '{latestValue}', when expected to be not.";
        }

        protected override string GetIsEmptyError(string latestValue)
        {
            return $"{_pageMetadata.Name} page title '{latestValue}' is not empty, when expected to be empty.";
        }

        protected override string GetIsNotEmptyError(string latestValue)
        {
            return $"{_pageMetadata.Name} page title is empty, when expected to be not empty.";
        }

        protected override string GetStartsWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title doesn't start with '{expectedValue}'.";
        }

        protected override string GetDoesNotStartWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title starts with '{expectedValue}'.";
        }

        protected override string GetEndsWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title doesn't end with '{expectedValue}'.";
        }

        protected override string GetDoesNotEndWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title ends with '{expectedValue}'.";
        }

        protected override string GetContainsError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title doesn't contain '{expectedValue}' yet.";
        }

        protected override string GetDoesNotContainError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title contains '{expectedValue}'.";
        }

        protected override string GetMatchesError(string latestValue, Regex regex)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title doesn't match '{regex}' regular expression.";
        }

        protected override string GetDoesNotMatchError(string latestValue, Regex regex)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page title matches '{regex}' regular expression.";
        }
    }
}
