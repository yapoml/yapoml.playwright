using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Components.Conditions
{
    /// <summary>
    /// Various conditions for awaiting url.
    /// </summary>
    /// <typeparam name="TConditions">Fluent original instance for chaining conditions.</typeparam>
    public class UrlConditions<TConditions> : TextualConditions<TConditions>
    {
        private readonly IPage _context;

        private readonly PageMetadata _pageMetadata;

        public UrlConditions(IPage context, TConditions conditions, TimeSpan timeout, TimeSpan pollingInterval, PageMetadata pageMetadata, ILogger logger)
            : base(conditions, timeout, pollingInterval, $"{pageMetadata.Name} page url", logger)
        {
            _context = context;
            _pageMetadata = pageMetadata;
        }

        protected override Func<string> FetchValueFunc => () => _context.Url;

        /// <summary>
        /// Conditions for url's length.
        /// </summary>
        public override NumericConditions<TConditions, int> Length
            => new TextualLengthConditons<TConditions>(_conditions, _timeout, _pollingInterval, FetchValueFunc, $"{_pageMetadata.Name} page url", _logger);

        /// <summary>
        /// Conditions for url's path.
        /// </summary>
        public UrlPathConditions<TConditions> Path
            => new UrlPathConditions<TConditions>(_context, _conditions, _timeout, _pollingInterval, _pageMetadata, _logger);

        protected override string GetIsError(string latestValue, string expectedValue)
        {
            return $"{_pageMetadata.Name} page url is not '{expectedValue}',{GetDifference("it was:", expectedValue, latestValue)}";
        }

        protected override string GetIsNotError(string latestValue, string expectedValue)
        {
            return $"{_pageMetadata.Name} page url is '{latestValue}', when expected to be not.";
        }

        protected override string GetIsEmptyError(string latestValue)
        {
            return $"{_pageMetadata.Name} page url '{latestValue}' is not empty, when expected to be empty.";
        }

        protected override string GetIsNotEmptyError(string latestValue)
        {
            return $"{_pageMetadata.Name} page url is empty, when expected to be not empty.";
        }

        protected override string GetStartsWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url doesn't start with '{expectedValue}'.";
        }

        protected override string GetDoesNotStartWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url starts with '{expectedValue}'.";
        }

        protected override string GetEndsWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url doesn't end with '{expectedValue}'.";
        }

        protected override string GetDoesNotEndWithError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url ends with '{expectedValue}'.";
        }

        protected override string GetContainsError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url doesn't contain '{expectedValue}' yet.";
        }

        protected override string GetDoesNotContainError(string latestValue, string expectedValue)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url contains '{expectedValue}'.";
        }

        protected override string GetMatchesError(string latestValue, Regex regex)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url doesn't match '{regex}'.";
        }

        protected override string GetDoesNotMatchError(string latestValue, Regex regex)
        {
            return $"'{latestValue}' {_pageMetadata.Name} page url matches '{regex}'.";
        }
    }
}
