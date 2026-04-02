using Microsoft.Playwright;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Factory for creating namespace space instances.
/// </summary>
public interface ISpaceFactory
{
    /// <summary>
    /// Creates a new space instance of the specified type.
    /// </summary>
    /// <typeparam name="TSpace">The space type to create.</typeparam>
    /// <param name="parentSpace">The parent space instance.</param>
    /// <param name="driver">The Playwright page instance.</param>
    /// <param name="spaceOptions">The space options configuration.</param>
    /// <returns>A new instance of <typeparamref name="TSpace"/>.</returns>
    TSpace Create<TSpace>(BaseSpace parentSpace, IPage driver, ISpaceOptions spaceOptions);
}
