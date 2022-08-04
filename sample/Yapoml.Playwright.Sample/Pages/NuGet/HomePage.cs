using System.Threading.Tasks;

namespace Yapoml.Playwright.Sample.Pages.NuGet
{
    public partial class HomePage
    {
        public async Task SearchAsync(string text)
        {
            await SearchInput.TypeAsync("yaml");
            await SearchButton.ClickAsync();
        }
    }
}
