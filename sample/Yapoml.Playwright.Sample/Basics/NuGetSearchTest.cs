using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Sample.Basics
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
            _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
            _page = await _browser.NewPageAsync();

            await _page.GotoAsync("https://nuget.org");
        }

        [TearDown]
        public void TearDown()
        {
            _playwright?.Dispose();
        }

        [Test]
        public async Task SearchWithPlaywright()
        {
            await _page.Locator("#search").TypeAsync("selenium");
            await _page.Locator(".btn-search").ClickAsync();

            var packages = await _page.Locator(".package").AllAsync();

            Assert.That(packages.Count, Is.EqualTo(20));

            foreach (var package in packages)
            {
                Assert.That(await package.Locator("a.package-title").TextContentAsync(), Is.Not.Empty);
                Assert.That(await package.Locator(".package-details").TextContentAsync(), Is.Not.Empty);

                var tags = await package.Locator(".package-tags a").AllAsync();

                foreach (var tag in tags)
                {
                    Assert.That(await tag.TextContentAsync(), Is.Not.Empty);
                }
            }
        }

        [Test]
        public void SearchWithYapoml()
        {
            _page.Ya().Basics.Pages
                .HomePage.Search("selenium")
                .Packages.Expect(its => its.Count.Is(20).Each(package =>
                {
                    package.Title.IsNotEmpty();
                    package.Description.IsNotEmpty();
                    package.Tags.Each(tag => tag.IsNotEmpty());
                }));
        }

        [Test]
        public void NavigateWithYapoml()
        {
            var ya = _page.Ya(opts =>
                opts.WithBaseUrl("https://nuget.org"))
                .Basics.Pages;

            // it opens https://nuget.org/packages?q=yaml
            Assert.That(ya.PackagesPage.Open(q: "yaml").Packages.Count, Is.EqualTo(20));

            // it opens https://nuget.org/packages/Newtonsoft.Json
            Console.WriteLine(ya.PackageDetailsPage.Open("Newtonsoft.Json").Version.Text);
        }

        [Test]
        public void WaitWithYapoml()
        {
            // set global timeout
            var homePage = _page.Ya(opts =>
                    opts.WithTimeout(timeout: TimeSpan.FromSeconds(10)))
                .Basics.Pages.HomePage;

            // used global timeout
            var searchInput = homePage.SearchInput.Expect(it => it.IsDisplayed());

            // or explicitly only here
            var searchInput2 = homePage.SearchInput.Expect(it => it.IsDisplayed(timeout: TimeSpan.FromSeconds(20)));
        }

        [Test]
        public void ScrollEachPackageIntoView()
        {
            var packagesPage = _page.Ya(opts => opts.WithBaseUrl("https://nuget.org")).Basics.Pages.PackagesPage;

            foreach (var package in packagesPage.Open(q: "yaml").Packages)
            {
                package.ScrollIntoView();
            }
        }

        [Test]
        public void Cache()
        {
            var page = _page.Ya().Basics.Pages.HomePage;

            page.Expect(it => it.Title.Matches(new System.Text.RegularExpressions.Regex("Home"), TimeSpan.FromSeconds(3)));

            page.Search("yapoml");

            var packagesPage = _page.Ya().Basics.Pages.PackagesPage;

            foreach (var package in packagesPage.Packages)
            {
                Console.WriteLine(package.Title.Text);
            }

            var myPackage = packagesPage.Packages[p => p.Title == "Yapoml.Playwright"];
            Console.Write(myPackage);
        }

        [Test]
        public void CustomExpectation()
        {
            var page = _page.Ya().Basics.Pages.HomePage.Expect(its => its.SearchButton.IsNotWhite());

            page.SearchButton.Click(when => when.IsNotWhite());
        }
    }
}