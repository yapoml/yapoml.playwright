using Yapoml.Framework.Logging;
using Yapoml.Playwright.Events.Args.Element;
using Yapoml.Playwright.Events.Args.Page;

namespace Yapoml.Playwright.Events
{
    public class LogEventsProducer
    {
        private ILogger _logger;

        private readonly IEventSource _source;

        public LogEventsProducer(ILogger logger, IEventSource eventSource)
        {
            _logger = logger;
            _source = eventSource;
        }

        public void Init()
        {
            _source.ComponentEventSource.OnFindingComponent += ComponentEventSource_OnFindingComponent;
            _source.ComponentEventSource.OnFindingComponents += ComponentEventSource_OnFindingComponents;
            _source.ComponentEventSource.OnFoundComponents += ComponentEventSource_OnFoundComponents;
        }

        private void ComponentEventSource_OnFoundComponents(object sender, FoundElementsEventArgs e)
        {
            _logger.Trace($"Found {e.Elements.Count} {e.ComponentsListMetadata.Name}");
        }

        private void ComponentEventSource_OnFindingComponents(object sender, FindingElementsEventArgs e)
        {
            _logger.Trace($"Finding {e.ComponentsListMetadata.Name} {e.By}");
        }

        private void ComponentEventSource_OnFindingComponent(object sender, FindingElementEventArgs e)
        {
            _logger.Trace($"Finding {e.ComponentMetadata.Name} {e.By}");
        }
    }
}
