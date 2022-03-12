namespace Yapoml.Playwright.Events
{
    public interface IEventSource
    {
        IPageEventSource PageEventSource { get; }

        IComponentEventSource ComponentEventSource { get; }
    }
}
