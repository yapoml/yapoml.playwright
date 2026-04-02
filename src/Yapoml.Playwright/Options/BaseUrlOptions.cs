namespace Yapoml.Playwright.Options
{
    /// <summary>
    /// Holds the base URL used for resolving relative page URLs during navigation.
    /// </summary>
    public class BaseUrlOptions
    {
        /// <summary>
        /// Initializes a new instance with the specified base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL string.</param>
        public BaseUrlOptions(string baseUrl)
        {
            Url = baseUrl;
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        public string Url { get; }
    }
}
