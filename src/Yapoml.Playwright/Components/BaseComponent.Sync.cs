using Microsoft.Playwright;

namespace Yapoml.Playwright.Components
{
    public partial class BaseComponent
    {
        /// <inheritdoc cref="ILocator.TextContentAsync"/>
        public string TextContent() => TextContentAsync().GetAwaiter().GetResult();

        /// <inheritdoc cref="ILocator.ClickAsync(ElementHandleClickOptions?)"/>
        public void Click(LocatorClickOptions options = null) => ClickAsync(options).GetAwaiter().GetResult();

        /// <inheritdoc cref="ILocator.TypeAsync(string, ElementHandleTypeOptions?)"/>
        public void Type(string text, LocatorTypeOptions options = null) => TypeAsync(text, options).GetAwaiter().GetResult();
    }
}