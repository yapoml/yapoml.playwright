using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using Yapoml.Playwright.Sample.TheInternet.Pages;

namespace Yapoml.Playwright.Sample.TheInternet
{
    internal class TheInternetFixture
    {
        private IPage _page;
        private IBrowser _browser;
        private IPlaywright _playwright;

        private PagesSpace _ya;

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

            _ya = _page.Ya(opts => opts.WithBaseUrl("https://the-internet.herokuapp.com")).TheInternet.Pages;
        }

        [TearDown]
        public void TearDown()
        {
            _playwright?.Dispose();
        }

        [Test]
        public void Hover()
        {
            var user = _ya.HoversPage.Open().Users[1].Hover();

            user.Name.Expect(it => it.IsDisplayed().Is("name: user2"));
        }

        [Test]
        public void DragAndDrop()
        {
            var page = _ya.DragAndDropPage.Open();

            page.Expect().ColumnA.Is("A");
            page.Expect().ColumnB.Is("B");

            page.ColumnA.DragAndDrop(page.ColumnB);

            page.Expect().ColumnA.Is("B");
            page.Expect().ColumnB.Is("A");
        }
    }
}
