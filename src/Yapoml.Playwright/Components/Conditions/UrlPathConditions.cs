using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using Yapoml.Framework.Logging;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Components.Conditions;

/// <summary>
/// Various conditions for awaiting url's path.
/// </summary>
/// <typeparam name="TConditions">Fluent original instance for chaining conditions.</typeparam>
public class UrlPathConditions<TConditions> : TextualConditions<TConditions>
{
    private readonly IPage _driver;

    private readonly PageMetadata _pageMetadata;

    /// <inheritdoc />
    public UrlPathConditions(IPage driver, TConditions conditions, TimeSpan timeout, TimeSpan pollingInterval, PageMetadata pageMetadata, ILogger logger)
        : base(conditions, timeout, pollingInterval, $"{pageMetadata.Name} page url path", logger)
    {
        _driver = driver;
        _pageMetadata = pageMetadata;
    }

    /// <inheritdoc />
    protected override Func<string> FetchValueFunc => () => new Uri(_driver.Url).AbsolutePath;

    /// <inheritdoc />
    public override NumericConditions<TConditions, int> Length
        => new TextualLengthConditons<TConditions>(_conditions, _timeout, _pollingInterval, FetchValueFunc, $"{_pageMetadata.Name} page url", _logger);

    /// <inheritdoc />
    protected override string GetIsError(string latestValue, string expectedValue)
    {
        return $"{_pageMetadata.Name} page url path is not '{expectedValue}',{GetDifference("it was:", expectedValue, latestValue)}";
    }

    /// <inheritdoc />
    protected override string GetIsNotError(string latestValue, string expectedValue)
    {
        return $"{_pageMetadata.Name} page url path is '{latestValue}', when expected to be not.";
    }

    /// <inheritdoc />
    protected override string GetIsEmptyError(string latestValue)
    {
        return $"{_pageMetadata.Name} page url path '{latestValue}' is not empty, when expected to be empty.";
    }

    /// <inheritdoc />
    protected override string GetIsNotEmptyError(string latestValue)
    {
        return $"{_pageMetadata.Name} page url path is empty, when expected to be not empty.";
    }

    /// <inheritdoc />
    protected override string GetStartsWithError(string latestValue, string expectedValue)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path doesn't start with '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotStartWithError(string latestValue, string expectedValue)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path starts with '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetEndsWithError(string latestValue, string expectedValue)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path doesn't end with '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotEndWithError(string latestValue, string expectedValue)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path ends with '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetContainsError(string latestValue, string expectedValue)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path doesn't contain '{expectedValue}' yet.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotContainError(string latestValue, string expectedValue)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path contains '{expectedValue}'.";
    }

    /// <inheritdoc />
    protected override string GetMatchesError(string latestValue, Regex regex)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path doesn't match '{regex}'.";
    }

    /// <inheritdoc />
    protected override string GetDoesNotMatchError(string latestValue, Regex regex)
    {
        return $"'{latestValue}' {_pageMetadata.Name} page url path matches '{regex}'.";
    }
}
