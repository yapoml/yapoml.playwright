namespace Yapoml.Playwright.Options
{
    public class BaseUrlOptions
    {
        public BaseUrlOptions(string baseUrl)
        {
            Url = baseUrl;
        }

        public string Url { get; }
    }
}
