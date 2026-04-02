using Yapoml.Framework.Options;
using Yapoml.Playwright.Options;

namespace Yapoml.Playwright;

/// <summary>
/// Provides a possibility to set default behavior for actions.
/// </summary>
public static class ComponentActionsExtensions
{
    /// <summary>
    /// Registers default <see cref="ScrollIntoViewOptions"/> for all scroll-into-view actions.
    /// </summary>
    /// <param name="spaceOptions">The space options to configure.</param>
    /// <param name="options">The scroll-into-view options to use as default.</param>
    /// <returns>The same space options instance for further chaining.</returns>
    public static ISpaceOptions WithScrollIntoViewOptions(this ISpaceOptions spaceOptions, ScrollIntoViewOptions options)
    {
        spaceOptions.Services.Register(options);

        return spaceOptions;
    }

    /// <summary>
    /// Registers default <see cref="FocusOptions"/> for all focus actions.
    /// </summary>
    /// <param name="spaceOptions">The space options to configure.</param>
    /// <param name="options">The focus options to use as default.</param>
    /// <returns>The same space options instance for further chaining.</returns>
    public static ISpaceOptions WithFocusOptions(this ISpaceOptions spaceOptions, FocusOptions options)
    {
        spaceOptions.Services.Register(options);

        return spaceOptions;
    }
}


