using Microsoft.Playwright;
using System;
using System.Threading;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Conditions;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components
{
    public abstract class BasePageConditions<TSelf> : BaseConditions<TSelf>
    {
        public BasePageConditions(TimeSpan timeout, TimeSpan pollingInterval, IPage driver, IElementHandlerRepository elementHandlerRepository, IElementLocator elementLocator, PageMetadata pageMetadata, IEventSource eventSource, ILogger logger, ISpaceOptions spaceOptions)
            : base(timeout, pollingInterval)
        {
            Driver = driver;
            ElementHandlerRepository = elementHandlerRepository;
            ElementLocator = elementLocator;
            PageMetadata = pageMetadata;
            EventSource = eventSource;
            Logger = logger;
            SpaceOptions = spaceOptions;
        }

        protected IPage Driver { get; }
        protected IElementHandlerRepository ElementHandlerRepository { get; }
        protected IElementLocator ElementLocator { get; }
        protected PageMetadata PageMetadata { get; }
        protected IEventSource EventSource { get; }
        protected ILogger Logger { get; }
        protected ISpaceOptions SpaceOptions { get; }

        /// <summary>
        /// Evaluates document's state to be <c>complete</c> which means the page is fully loaded.
        /// It doesn't guarantee that some components on the page are present, they might be rendered dynamically.
        /// 
        /// If url is defined for the page, then it also evaluates current url.
        /// </summary>
        public virtual TSelf IsOpened(TimeSpan? timeout = default)
        {
            timeout ??= Timeout;

            string latestValue = null;

            try
            {
                using (var scope = Logger.BeginLogScope($"Expect the {PageMetadata.Name} document state is complete"))
                {
                    scope.Execute(() =>
                    {
                        Driver.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions { Timeout = (float)timeout.Value.TotalSeconds });
                    });
                }
            }
            catch (TimeoutException ex)
            {
                throw new ExpectException($"{PageMetadata.Name} page is not opened yet. Current state is '{latestValue}'.", ex);
            }

            return _self;
        }

        /// <summary>
        /// Various conditions for current page Url.
        /// </summary>
        public virtual UrlConditions<TSelf> Url
        {
            get
            {
                return new UrlConditions<TSelf>(Driver, _self, Timeout, PollingInterval, PageMetadata, Logger);
            }
        }

        /// <summary>
        /// Various conditions for current title of the page.
        /// </summary>
        public virtual TitleConditions<TSelf> Title
        {
            get
            {
                return new TitleConditions<TSelf>(Driver, _self, Timeout, PollingInterval, PageMetadata, Logger);
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
    }
}
