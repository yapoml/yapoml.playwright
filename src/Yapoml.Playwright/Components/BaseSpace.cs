using Microsoft.Playwright;
using Yapoml.Framework.Options;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Base class for a hierarchical namespace (space) that contains pages and nested spaces.
/// </summary>
/// <typeparam name="TParentSpace">The type of the parent space for navigation back.</typeparam>
public abstract class BaseSpace<TParentSpace> : BaseSpace
{
    /// <summary>The parent space instance for navigation.</summary>
    protected TParentSpace _parentSpace;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpace{TParentSpace}"/> class.
    /// </summary>
    public BaseSpace(TParentSpace parentSpace, IPage driver, ISpaceOptions spaceOptions)
        : base(driver, spaceOptions)
    {
        _parentSpace = parentSpace;

    }
}

/// <summary>
/// Non-generic base class for namespace spaces, holding shared Playwright page and options references.
/// </summary>
public abstract class BaseSpace
{
    /// <summary>The Playwright page instance.</summary>
    protected IPage _driver;

    /// <summary>The space options configuration.</summary>
    protected ISpaceOptions _spaceOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpace"/> class.
    /// </summary>
    protected BaseSpace(IPage driver, ISpaceOptions spaceOptions)
    {
        _driver = driver;
        _spaceOptions = spaceOptions;
    }
}
