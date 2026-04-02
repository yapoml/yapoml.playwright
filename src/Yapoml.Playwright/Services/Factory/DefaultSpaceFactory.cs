using Microsoft.Playwright;
using System;
using Yapoml.Framework.Options;
using Yapoml.Playwright.Components;

namespace Yapoml.Playwright.Services.Factory;

/// <summary>
/// Default implementation of <see cref="ISpaceFactory"/> that creates spaces via reflection.
/// </summary>
public class DefaultSpaceFactory : ISpaceFactory
{
    /// <inheritdoc />
    public TSpace Create<TSpace>(BaseSpace parentSpace, IPage driver, ISpaceOptions spaceOptions)
    {
        var space = (TSpace)Activator.CreateInstance(typeof(TSpace), parentSpace, driver, spaceOptions);

        return space;
    }
}
