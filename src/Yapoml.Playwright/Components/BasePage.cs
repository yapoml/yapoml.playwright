using Microsoft.Playwright;
using Yapoml.Options;

namespace Yapoml.Playwright.Components
{
    public abstract class BasePage
    {
        public BasePage(IPage page, ISpaceOptions spaceOptions)
        {
            Page = page;
            SpaceOptions = spaceOptions;

            EventSource = spaceOptions.Get<Events.IEventSource>().ComponentEventSource;
        }

        protected IPage Page { get; }
        protected ISpaceOptions SpaceOptions { get; }

        protected Events.IComponentEventSource EventSource;
    }
}
