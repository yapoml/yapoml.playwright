Generates page object classes for Microsoft Playwright with ease.

Given that you have the following `LoginPage.po.yaml` file

```yaml
UsernameInput:
  by: "#username" # search by id

PasswordInput:
  by: .password # search by css

LoginButton:
  by: .primary-button
```

Then you are able to immediately interact with web elements

```csharp
using Yapoml.Playwright;

page.Ya().LoginPage.UsernameInput.Type("user01");
```

# Installation
Install [Yapoml.Playwright](https://www.nuget.org/packages/Yapoml.Playwright) nuget package and create your `*.po.yaml` files.