using Yapoml.Framework.Options;

namespace Yapoml.Playwright
{
    public static class ServiceExtensions
    {
        public static ISpaceOptions WithService<T>(this ISpaceOptions spaceOptions, T service)
        {
            spaceOptions.Services.Register<T>(service);

            return spaceOptions;
        }
    }
}
