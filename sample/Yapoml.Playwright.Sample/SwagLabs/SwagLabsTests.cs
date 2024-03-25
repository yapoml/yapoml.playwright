using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using Yapoml.Playwright.Sample.SwagLabs.Pages;

namespace Yapoml.Playwright.Sample.SwagLabs
{
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
        public void IncorrectLogin()
        {
            var error = _ya.LoginPage.Open().Form
                .Login.Click().Error;

            error.Expect(it => it.IsDisplayed().Text.Is("Epic sadface: Username is required"));

            error.Close.Click();

            error.Expect().IsNotDisplayed();
        }

        [Test]
        public void AddToCart()
        {
            _ya.Login("standard_user", "secret_sauce")
                .Products[p => p.Name == "Sauce Labs Backpack"].AddToCartButton.Click();

            _ya.InventoryPage.PrimaryHeader.ShoppingCart
                .Expect(its => its.Badge.Is("1").Styles.BackgroundColor.Is("rgb(226, 35, 26)"))
                .Click();

            _ya.CartPage.Expect().IsOpened()
                .Items.Expect().Count.Is(1);

            _ya.CartPage.RemoveAllItems()
                .Expect(its => its.Items.IsEmpty())
                .Expect(its => its.PrimaryHeader.ShoppingCart.Badge.IsNotDisplayed());
        }
    }
}
