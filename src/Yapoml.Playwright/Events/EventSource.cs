namespace Yapoml.Playwright.Events
{
    /// <summary>
    /// Default implementation of <see cref="IEventSource"/> that aggregates page and component event sources.
    /// </summary>
    public class EventSource : IEventSource
    {
        /// <inheritdoc />
        public IPageEventSource PageEventSource { get; } = new PageEventSource();

        /// <inheritdoc />
        public IComponentEventSource ComponentEventSource { get; } = new ComponentEventSource();
    }
}
