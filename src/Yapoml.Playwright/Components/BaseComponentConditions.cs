﻿using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Conditions;
using Yapoml.Playwright.Components.Conditions.Generic;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components
{
    public abstract class BaseComponentConditions<TSelf> : BaseConditions<TSelf>, ITextualConditions<TSelf>
    {
        public BaseComponentConditions(TimeSpan timeout, TimeSpan pollingInterval, IPage driver, IElementHandler elementHandler, IElementLocator elementLocator, IEventSource eventSource, ILogger logger, ISpaceOptions spaceOptions)
            : base(timeout, pollingInterval)
        {
            Driver = driver;
            ElementHandler = elementHandler;
            ElementLocator = elementLocator;
            EventSource = eventSource;
            Logger = logger;
            SpaceOptions = spaceOptions;
        }

        protected IPage Driver { get; }
        protected IElementHandler ElementHandler { get; }
        protected IElementLocator ElementLocator { get; }
        protected IEventSource EventSource { get; }
        protected ILogger Logger { get; }
        protected ISpaceOptions SpaceOptions { get; }

        /// <summary>
        /// Waits until the component is displayed.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is displayed.</param>
        /// <returns></returns>
        public virtual TSelf IsDisplayed(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is displayed"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is not displayed yet '{ElementHandler.By}'.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is not displayed.
        /// Detached component from DOM is also considered as not displayed.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is not displayed.</param>
        /// <returns></returns>
        public virtual TSelf IsNotDisplayed(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is not displayed"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeHiddenAsync(new LocatorAssertionsToBeHiddenOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is still displayed '{ElementHandler.By}'.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is appeared in the DOM.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is appeared.</param>
        /// <returns></returns>
        public virtual TSelf Exists(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} exists"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeAttachedAsync(new LocatorAssertionsToBeAttachedOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} does not exist yet '{ElementHandler.By}'.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component disappeared drom the DOM.
        /// </summary>
        /// <param name="timeout">How long to wait until the component disappeared.</param>
        /// <returns></returns>
        public virtual TSelf DoesNotExist(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} does not exist"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeAttachedAsync(new LocatorAssertionsToBeAttachedOptions { Attached = false, Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} still exists '{ElementHandler.By}'.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is enabled.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is enabled.</param>
        /// <returns></returns>
        public virtual TSelf IsEnabled(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is enabled"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is not enabled yet.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is disabled.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is disabled.</param>
        /// <returns></returns>
        public virtual TSelf IsDisabled(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is disabled"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeDisabledAsync(new LocatorAssertionsToBeDisabledOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is still enabled.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is disabled.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is disabled.</param>
        /// <returns></returns>
        public TSelf IsNotEnabled(TimeSpan? timeout = null)
        {
            return IsDisabled(timeout);
        }

        /// <summary>
        /// Waits until the component is checked.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is checked.</param>
        /// <returns></returns>
        /// <exception cref="ExpectException"></exception>
        public virtual TSelf IsChecked(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is checked"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeCheckedAsync(new LocatorAssertionsToBeCheckedOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is still not checked.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is unchecked.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is unchecked.</param>
        /// <returns></returns>
        /// <exception cref="ExpectException"></exception>
        public virtual TSelf IsNotChecked(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is unchecked"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeCheckedAsync(new LocatorAssertionsToBeCheckedOptions { Checked = false, Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is still checked.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is unchecked.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is unchecked.</param>
        /// <returns></returns>
        public virtual TSelf IsUnchecked(TimeSpan? timeout = null)
        {
            return IsNotChecked(timeout);
        }

        /// <summary>
        /// Waits until the component is in view.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is in view.</param>
        /// <returns></returns>
        public virtual TSelf IsInView(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is in view"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).ToBeInViewportAsync(new LocatorAssertionsToBeInViewportOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is not in view yet.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Waits until the component is not in view.
        /// </summary>
        /// <param name="timeout">How long to wait until the component is not in view.</param>
        /// <returns></returns>
        public virtual TSelf IsNotInView(TimeSpan? timeout = null)
        {
            timeout ??= Timeout;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect {ElementHandler.ComponentMetadata.Name} is not in view"))
                {
                    scope.Execute(() =>
                    {
                        Assertions.Expect(ElementHandler.Locate()).Not.ToBeInViewportAsync(new LocatorAssertionsToBeInViewportOptions { Timeout = (float)timeout.Value.TotalMilliseconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{ElementHandler.ComponentMetadata.Name} is still in view.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Various expected conditions for component's text.
        /// </summary>
        public virtual TextConditions<TSelf> Text
        {
            get
            {
                return new TextConditions<TSelf>(_self, ElementHandler, Timeout, PollingInterval, $"text of the {ElementHandler.ComponentMetadata.Name}", Logger);
            }
        }

        /// <summary>
        /// Various expected conditions for component attributes.
        /// </summary>
        public virtual AttributesCollectionConditions<TSelf> Attributes
        {
            get
            {
                return new AttributesCollectionConditions<TSelf>(_self, ElementHandler, Timeout, PollingInterval, Logger);
            }
        }

        /// <summary>
        /// Various expected conditions for CSS styles.
        /// </summary>
        public virtual StylesCollectionConditions<TSelf> Styles
        {
            get
            {
                return new StylesCollectionConditions<TSelf>(_self, ElementHandler, Timeout, PollingInterval, Logger);
            }
        }

        public virtual ValueConditions<TSelf> Value
        {
            get
            {
                return new ValueConditions<TSelf>(_self, ElementHandler, Timeout, PollingInterval, $"value of the {ElementHandler.ComponentMetadata.Name}", Logger);
            }
        }

        /// <summary>
        /// Waits specified amount of time.
        /// </summary>
        /// <param name="duration">Aamount of time to wait.</param>
        /// <returns></returns>
        public virtual TSelf Elapsed(TimeSpan duration)
        {
            Thread.Sleep(duration);

            return _self;
        }

        #region Textual Conditions

        public TSelf Is(string value, TimeSpan? timeout = default)
        {
            return Text.Is(value, timeout);
        }

        public TSelf Is(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.Is(value, comparisonType, timeout);
        }

        public TSelf IsNot(string value, TimeSpan? timeout = default)
        {
            return Text.IsNot(value, timeout);
        }

        public TSelf IsNot(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.IsNot(value, comparisonType, timeout);
        }

        public TSelf IsEmpty(TimeSpan? timeout = default)
        {
            return Text.IsEmpty(timeout);
        }

        public TSelf IsNotEmpty(TimeSpan? timeout = default)
        {
            return Text.IsNotEmpty(timeout);
        }

        public TSelf StartsWith(string value, TimeSpan? timeout = default)
        {
            return Text.StartsWith(value, timeout);
        }

        public TSelf StartsWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.StartsWith(value, comparisonType, timeout);
        }

        public TSelf DoesNotStartWith(string value, TimeSpan? timeout = default)
        {
            return Text.DoesNotStartWith(value, timeout);
        }

        public TSelf DoesNotStartWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.DoesNotStartWith(value, comparisonType, timeout);
        }

        public TSelf EndsWith(string value, TimeSpan? timeout = default)
        {
            return Text.EndsWith(value, timeout);
        }

        public TSelf EndsWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.EndsWith(value, comparisonType, timeout);
        }

        public TSelf DoesNotEndWith(string value, TimeSpan? timeout = default)
        {
            return Text.DoesNotEndWith(value, timeout);
        }

        public TSelf DoesNotEndWith(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.DoesNotEndWith(value, comparisonType, timeout);
        }

        public TSelf Contains(string value, TimeSpan? timeout = default)
        {
            return Text.Contains(value, timeout);
        }

        public TSelf Contains(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.Contains(value, comparisonType, timeout);
        }

        public TSelf DoesNotContain(string value, TimeSpan? timeout = default)
        {
            return Text.DoesNotContain(value, timeout);
        }

        public TSelf DoesNotContain(string value, StringComparison comparisonType, TimeSpan? timeout = default)
        {
            return Text.DoesNotContain(value, comparisonType, timeout);
        }

        public TSelf Matches(Regex regex, TimeSpan? timeout = default)
        {
            return Text.Matches(regex, timeout);
        }

        public TSelf DoesNotMatch(Regex regex, TimeSpan? timeout = default)
        {
            return Text.DoesNotMatch(regex, timeout);
        }
        #endregion
    }
}
