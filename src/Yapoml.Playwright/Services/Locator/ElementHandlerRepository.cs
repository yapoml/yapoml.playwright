using System.Collections.Generic;
using System.Linq;

namespace Yapoml.Playwright.Services.Locator;

/// <summary>
/// Default implementation of <see cref="IElementHandlerRepository"/> that stores element handlers in a dictionary
/// and supports hierarchical nesting.
/// </summary>
public class ElementHandlerRepository : IElementHandlerRepository
{
    private readonly IDictionary<string, IElementHandler> _elementHandlers = new Dictionary<string, IElementHandler>();

    /// <summary>
    /// Initializes a new root-level element handler repository.
    /// </summary>
    public ElementHandlerRepository()
    {

    }

    /// <summary>
    /// Initializes a new nested element handler repository with the specified parent.
    /// </summary>
    /// <param name="parentRepository">The parent repository.</param>
    public ElementHandlerRepository(IElementHandlerRepository parentRepository)
    {
        ParentRepository = parentRepository;
    }

    /// <inheritdoc />
    public IElementHandlerRepository ParentRepository { get; private set; }

    /// <inheritdoc />
    public bool TryGet(string key, out IElementHandler elementHandler)
    {
        return _elementHandlers.TryGetValue(key, out elementHandler);
    }

    /// <inheritdoc />
    public void Set(string key, IElementHandler elementHandler)
    {
        _elementHandlers[key] = elementHandler;
    }

    /// <inheritdoc />
    public IElementHandlerRepository CreateNestedRepository()
    {
        return new ElementHandlerRepository(parentRepository: this);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IElementHandler> ElementHandlers
    {
        get
        {
            return _elementHandlers.Values.ToList().AsReadOnly();
        }
    }
}
