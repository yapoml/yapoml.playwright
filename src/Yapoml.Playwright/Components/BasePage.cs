using Microsoft.Playwright;
using Yapoml.Framework.Logging;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components.Metadata;
using Yapoml.Playwright.Events;
using Yapoml.Playwright.Services.Locator;

namespace Yapoml.Playwright.Components
{
    public abstract class BasePage
    {
        public BasePage(IPage webDriver, IElementHandlerRepository elementHandlerRepository, PageMetadata metadata, ISpaceOptions spaceOptions)
        {
            WebDriver = webDriver;
            ElementHandlerRepository = elementHandlerRepository;
            Metadata = metadata;
            SpaceOptions = spaceOptions;

            EventSource = spaceOptions.Services.Get<IEventSource>();
            _logger = spaceOptions.Services.Get<ILogger>();
        }

        protected IPage WebDriver { get; }

        protected IElementHandlerRepository ElementHandlerRepository { get; }

        protected PageMetadata Metadata { get; }

        protected ISpaceOptions SpaceOptions { get; }

        protected IEventSource EventSource { get; }

        protected ILogger _logger;
    }
}
