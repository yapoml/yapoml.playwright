using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using Yapoml.Playwright;

namespace Yapoml.Playwright.Sample
{
    [TestFixture]
    public class NuGetSearchTest
    {
        private IPage _page;
        private IBrowser _browser;
        private IPlaywright _playwright;

        [SetUp]
        public async Task SetUp()
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Firefox.LaunchAsync(new() { Headless = false});
            _page = await _browser.NewPageAsync();

            await _page.GotoAsync("https://nuget.org");
        }

        [TearDown]
        public async Task TearDown()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Test]
        public async Task SearchWithPlaywright()
        {
            await _page.Locator("#search").TypeAsync("yaml");
            await _page.Locator(".btn-search").ClickAsync();

            var packageLocator = _page.Locator(".package");

            var count = await packageLocator.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var title = await packageLocator.Nth(i).Locator(".package-title").TextContentAsync();
                Assert.That(title, Is.Not.Empty);

                var description = await packageLocator.Nth(i).Locator(".package-details").TextContentAsync();
                Assert.That(description, Is.Not.Empty);
            }
        }

        [Test]
        public void SearchWithYapoml()
        {
            var nuget = _page.Ya().Pages.NuGet;

            nuget.HomePage.Search("yaml");

            foreach (var package in nuget.SearchResultsPage.Packages)
            {
                Assert.That(package.Title.TextContent, Is.Not.Empty);
                Assert.That(package.Description.TextContent, Is.Not.Empty);
            }
        }
    }
}