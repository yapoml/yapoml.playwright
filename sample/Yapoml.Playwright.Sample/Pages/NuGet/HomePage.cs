namespace Yapoml.Playwright.Sample.Pages.NuGet
{
    public partial class HomePage
    {
        public void Search(string text)
        {
            SearchInput.Type("yaml");
            SearchButton.Click();
        }
    }
}
