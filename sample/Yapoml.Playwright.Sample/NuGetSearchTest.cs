using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Sample
{
    [TestFixture]
    public class NuGetSearchTest
    {
        private IPage _page;
        private IBrowser _browser;
        private IPlaywright _playwright;

        [OneTimeSetUp]
        public void DownloadPlaywright()
        {
            Program.Main(new string[] { "install" });
        }

        [SetUp]
        public async Task SetUp()
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = false });
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
            await _page.Locator("#search").TypeAsync("Yapoml");
            await _page.Locator(".btn-search").ClickAsync();

            var packageLocator = _page.Locator(".package");

            var count = await packageLocator.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var title = await packageLocator.Nth(i).Locator("a.package-title").TextContentAsync();
                Assert.That(title, Is.Not.Empty);

                var description = await packageLocator.Nth(i).Locator(".package-details").TextContentAsync();
                Assert.That(description, Is.Not.Empty);
            }
        }

        [Test]
        public void SearchWithYapoml()
        {
            _page.Ya(opts => opts.WithBaseUrl("https://nuget.org"))
                .HomePage.Open().Search("Yapoml")
                .Packages.Expect(its => its.Count.AtLeast(1).Each(package =>
                    {
                        package.Title.Contains("Yapoml");
                        package.Description.IsNotEmpty();
                        package.Tags.Each(tag => tag.IsNotEmpty());
                    })
                );
        }

        [Test]
        public void IntroShowcase()
        {
            _page.Ya().PackagesPage.Open(q: "yapoml")
                .Packages.Expect(it => it.IsNotEmpty().Each(package =>
                    {
                        package.Description.IsNotEmpty();
                        package.Tags.IsNotEmpty();
                    })
                );
        }
    }
}