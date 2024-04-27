namespace Yapoml.Playwright.Sample
{
    public partial class HomePage
    {
        public PackagesPage Search(string text)
        {
            using var scope = _logger.BeginLogScope($"Searching for packages by '{text}' query");
            
            return scope.Execute(() =>
            {
                SearchInput.Fill(text);
                SearchButton.Click();

                return SpaceOptions.Services.Get<YaSpace>().PackagesPage;
            });
        }
    }
}
