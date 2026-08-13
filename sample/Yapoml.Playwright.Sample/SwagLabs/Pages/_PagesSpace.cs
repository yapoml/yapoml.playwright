namespace Yapoml.Playwright.Sample.SwagLabs.Pages;

partial class PagesSpace
{
    public InventoryPage Login(string username, string password)
    {
        var form = LoginPage.Open().Form;

        form.Username.Type(username);
        form.Password.Type(password);
        form.Login.Click();

        return InventoryPage.Expect().IsOpened();
    }
}
