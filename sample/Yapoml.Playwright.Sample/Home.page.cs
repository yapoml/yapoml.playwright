namespace Yapoml.Playwright.Sample
{
    public partial class HomePage
    {
        public PackagesPage Search(string text)
        {
            using (_logger.BeginLogScope($"Searching for packages by '{text}' query"))
            {
                SearchInput.Fill(text);
                SearchButton.Click();

                return SpaceOptions.Services.Get<YaSpace>().PackagesPage;
            }
        }
    }
}
