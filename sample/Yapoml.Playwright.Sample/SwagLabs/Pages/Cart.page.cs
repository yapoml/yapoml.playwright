using System.Threading.Tasks;

namespace Yapoml.Playwright.Sample.SwagLabs.Pages;

partial class CartPage
{
    public async Task<CartPage> RemoveAllItems()
    {
        using (_logger.BeginLogScope("Removing all items from cart"))
        {
            await Items.ForEach(i => i.RemoveButton.Click());
        }

        return this;
    }
}
