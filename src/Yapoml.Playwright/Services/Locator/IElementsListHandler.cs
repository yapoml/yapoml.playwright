using Microsoft.Playwright;
using System.Collections.Generic;
using Yapoml.Playwright.Components.Metadata;

namespace Yapoml.Playwright.Services.Locator
{
    public interface IElementsListHandler
    {
        void Invalidate();

        IReadOnlyList<ILocator> LocateMany();

        string By { get; }

        ComponentsListMetadata ComponentsListMetadata { get; }

        IElementHandlerRepository ElementHandlerRepository { get; }
    }
}
