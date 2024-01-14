Generates page object classes for Microsoft Playwright with ease.

Given that you have the following `LoginPage.po.yaml` file

```yaml
UsernameInput:
  by: id username

PasswordInput:
  by:
    css: .password

LoginButton:
  by: css .primary-button
```

Then you are able to immediately interact with web elements

```csharp
using Yapoml.Playwright;

await webDriver.Ya().LoginPage.UsernameInput.TypeAsync("user01");
```