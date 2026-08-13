using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using Yapoml.Playwright.Sample.SwagLabs.Pages;

namespace Yapoml.Playwright.Sample.SwagLabs;

internal class SwagLabsTests
{
    private IPlaywright _playwright;
    private PagesSpace _ya;

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        var browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        _ya = page.Ya(opts => opts.WithBaseUrl("https://www.saucedemo.com")).SwagLabs.Pages;
    }

    [TearDown]
    public void TearDown()
    {
        _playwright?.Dispose();
    }

    [Test]
    public async Task IncorrectLogin()
    {
        var form = _ya.LoginPage.Open().Form;

        form.Login.Click();

        var error = await form.Error;

        await error.Expect(it => it.IsDisplayed().Text.Is("Epic sadface: Username is required"));

        await error.Close.Click();

        await error.Expect().IsNotDisplayed();
    }

    [Test]
    public async Task AddToCart()
    {
        await _ya.Login("standard_user", "secret_sauce")
            .Products[p => p.Name == "Sauce Labs Backpack"].AddToCartButton.Click();

        await _ya.InventoryPage.PrimaryHeader.ShoppingCart
            .Expect(its => its.Badge.Is("1").Styles.BackgroundColor.Is("rgb(226, 35, 26)"))
            .Click();

        await _ya.CartPage.Expect().IsOpened()
            .Items.Expect().Count.Is(1);

        await _ya.CartPage.RemoveAllItems()
            .Expect(its => its.Items.IsEmpty())
            .Expect(its => its.PrimaryHeader.ShoppingCart.Badge.IsNotDisplayed());
    }
}
