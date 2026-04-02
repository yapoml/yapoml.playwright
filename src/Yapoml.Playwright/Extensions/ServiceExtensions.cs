using Yapoml.Framework.Options;

namespace Yapoml.Playwright;

/// <summary>
/// Extension methods for registering custom services into space options.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Registers a custom service into the space options container.
    /// </summary>
    /// <typeparam name="T">The type of the service to register.</typeparam>
    /// <param name="spaceOptions">The space options to configure.</param>
    /// <param name="service">The service instance to register.</param>
    /// <returns>The same space options instance for further chaining.</returns>
    public static ISpaceOptions WithService<T>(this ISpaceOptions spaceOptions, T service)
    {
        spaceOptions.Services.Register(service);

        return spaceOptions;
    }
}
