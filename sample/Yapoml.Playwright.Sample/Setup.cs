using NUnit.Framework;

namespace Yapoml.Playwright.Sample;

[SetUpFixture]
class Setup
{
    [OneTimeSetUp]
    public void InstallPlaywright()
    {
        Microsoft.Playwright.Program.Main(new string[] { "install" });
    }
}
