using Microsoft.Playwright;

namespace Yapoml.Playwright.Components
{
    public partial class BaseComponent
    {
        /// <inheritdoc cref="IElementHandle.TextContentAsync"/>
        public string TextContent() => TextContentAsync().GetAwaiter().GetResult();

        /// <inheritdoc cref="IElementHandle.ClickAsync(ElementHandleClickOptions?)"/>
        public void Click(ElementHandleClickOptions options = null) => ClickAsync(options).GetAwaiter().GetResult();

        /// <inheritdoc cref="IElementHandle.TypeAsync(string, ElementHandleTypeOptions?)"/>
        public void Type(string text, ElementHandleTypeOptions options = null) => TypeAsync(text, options).GetAwaiter().GetResult();
    }
}