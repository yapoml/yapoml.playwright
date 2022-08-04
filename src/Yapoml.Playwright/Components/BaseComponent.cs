using Microsoft.Playwright;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Events;

namespace Yapoml.Playwright.Components
{
    public partial class BaseComponent
    {
        protected IPage Page { get; private set; }

        public ILocator WrappedLocator { get; private set; }

        protected ISpaceOptions SpaceOptions { get; private set; }

        protected IComponentEventSource EventSource { get; private set; }

        public BaseComponent(IPage page, ILocator locator, ISpaceOptions spaceOptions)
        {
            Page = page;
            WrappedLocator = locator;
            SpaceOptions = spaceOptions;

            EventSource = spaceOptions.Services.Get<IEventSource>().ComponentEventSource;
        }

        
    }
}