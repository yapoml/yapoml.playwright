using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Sample.Basics;

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
        await _page.Locator("#search").FillAsync("selenium");
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
    public async Task SearchWithYapoml()
    {
        await _page.Ya().Basics.Pages
            .HomePage.Search("selenium")
            .Packages.Expect(its => its.Count.Is(20).Each(package =>
            {
                package.Title.IsNotEmpty();
                package.Description.IsNotEmpty();
                package.Tags.Each(tag => tag.IsNotEmpty());
            }));
    }

    [Test]
    public async Task NavigateWithYapoml()
    {
        var ya = _page.Ya(opts =>
            opts.WithBaseUrl("https://nuget.org"))
            .Basics.Pages;

        // it opens https://nuget.org/packages?q=yaml
        Assert.That(await ya.PackagesPage.Open(q: "yaml").Packages.Count, Is.EqualTo(20));

        // it opens https://nuget.org/packages/Newtonsoft.Json
        Console.WriteLine(await ya.PackageDetailsPage.Open("Newtonsoft.Json").Version.Text);
    }

    [Test]
    public async Task WaitWithYapoml()
    {
        // set global timeout
        var homePage = _page.Ya(opts =>
                opts.WithTimeout(timeout: TimeSpan.FromSeconds(10)))
            .Basics.Pages.HomePage;

        // used global timeout
        var searchInput = await homePage.SearchInput.Expect(it => it.IsDisplayed());

        // or explicitly only here
        var searchInput2 = await homePage.SearchInput.Expect(it => it.IsDisplayed(timeout: TimeSpan.FromSeconds(20)));
    }

    [Test]
    public async Task ScrollEachPackageIntoView()
    {
        var packagesPage = _page.Ya(opts => opts.WithBaseUrl("https://nuget.org")).Basics.Pages.PackagesPage;

        await foreach (var package in packagesPage.Open(q: "yaml").Packages)
        {
            await package.ScrollIntoView();
        }
    }

    [Test]
    public async Task Cache()
    {
        var page = _page.Ya().Basics.Pages.HomePage;

        await page.Expect(it => it.Title.Matches(new System.Text.RegularExpressions.Regex("Home"), TimeSpan.FromSeconds(3)));

        await page.Search("yapoml");

        var packagesPage = _page.Ya().Basics.Pages.PackagesPage;

        await foreach (var package in packagesPage.Packages)
        {
            Console.WriteLine(await package.Title.Text);
        }

        var myPackage = await packagesPage.Packages[async p => await p.Title.Text == "Yapoml.Playwright"];
        Console.Write(myPackage);
    }

    [Test]
    public async Task CustomExpectation()
    {
        var page = await _page.Ya().Basics.Pages.HomePage.Expect(its => its.SearchButton.IsNotWhite());

        await page.SearchButton.Click(when => when.IsNotWhite());
    }
}
