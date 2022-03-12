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
        public async Task Search()
        {
            await _page.TypeAsync("#search", "yaml");
            await _page.ClickAsync(".btn-search");

            foreach (var package in await _page.QuerySelectorAllAsync(".package"))
            {
                var title = await (await package.QuerySelectorAsync("xpath=.//a")).TextContentAsync();
                Assert.That(title, Is.Not.Empty);

                var description = await (await package.QuerySelectorAsync(".package-details")).TextContentAsync();
                Assert.That(description, Is.Not.Empty);
            }
        }

        [Test]
        public void SearchWithYapoml()
        {
            var ya = _page.Ya().Pages.NuGet;

            ya.HomePage.Search("yaml");

            foreach (var package in ya.SearchResultsPage.Packages)
            {
                Assert.That(package.Title.TextContent, Is.Not.Empty);
                Assert.That(package.Description.TextContent, Is.Not.Empty);
            }
        }
    }
}