using Microsoft.Playwright;
using System;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Services.Locator
{
    public interface IElementHandler
    {
        ILocator Locate();

        ILocator Locate(TimeSpan timeout, TimeSpan pollingInterval);

        void Invalidate();

        string By { get; }

        ElementLocatorContext From { get; }

        ComponentMetadata ComponentMetadata { get; }

        IElementHandlerRepository ElementHandlerRepository { get; }
    }
}
