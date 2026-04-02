namespace Yapoml.Playwright.Events
{
    /// <summary>
    /// Provides access to page and component event sources for subscribing to lifecycle events.
    /// </summary>
    public interface IEventSource
    {
        /// <summary>
        /// Gets the event source for page-level events such as navigation.
        /// </summary>
        IPageEventSource PageEventSource { get; }

        /// <summary>
        /// Gets the event source for component-level events such as finding and found elements.
        /// </summary>
        IComponentEventSource ComponentEventSource { get; }
    }
}
