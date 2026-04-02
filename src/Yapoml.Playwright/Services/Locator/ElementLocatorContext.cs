namespace Yapoml.Playwright.Services.Locator
{
    /// <summary>
    /// Specifies the context from which an element is located.
    /// </summary>
    public enum ElementLocatorContext
    {
        /// <summary>
        /// Locate the element relative to its parent component.
        /// </summary>
        Parent,

        /// <summary>
        /// Locate the element from the root of the page (document level).
        /// </summary>
        Root
    }
}
