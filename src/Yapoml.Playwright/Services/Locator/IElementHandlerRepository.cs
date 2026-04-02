using System.Collections.Generic;

namespace Yapoml.Playwright.Services.Locator;

/// <summary>
/// Repository for caching and retrieving <see cref="IElementHandler"/> instances by key.
/// Supports hierarchical nesting for component scoping.
/// </summary>
public interface IElementHandlerRepository
{
    /// <summary>
    /// Attempts to retrieve a cached element handler by key.
    /// </summary>
    /// <param name="key">The unique key identifying the element handler.</param>
    /// <param name="elementHandler">The cached element handler, if found.</param>
    /// <returns><c>true</c> if the element handler was found; otherwise, <c>false</c>.</returns>
    bool TryGet(string key, out IElementHandler elementHandler);

    /// <summary>
    /// Stores an element handler in the repository with the specified key.
    /// </summary>
    /// <param name="key">The unique key for the element handler.</param>
    /// <param name="elementHandler">The element handler to store.</param>
    void Set(string key, IElementHandler elementHandler);

    /// <summary>
    /// Gets the parent repository in the hierarchy, or <c>null</c> for root repositories.
    /// </summary>
    IElementHandlerRepository ParentRepository { get; }

    /// <summary>
    /// Creates a new nested repository with this repository as its parent.
    /// </summary>
    /// <returns>A new nested <see cref="IElementHandlerRepository"/>.</returns>
    IElementHandlerRepository CreateNestedRepository();

    /// <summary>
    /// Gets all element handlers stored in this repository.
    /// </summary>
    IReadOnlyCollection<IElementHandler> ElementHandlers { get; }
}
