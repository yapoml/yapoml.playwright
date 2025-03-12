Generates page object classes for Microsoft Playwright with ease.

Given that you have the following `Login.page.yaml` file

```yaml
username input: "#username"
```

Then you are able to immediately interact with web elements

```csharp
using Yapoml.Playwright;

page.Ya().LoginPage.UsernameInput.Fill("user01");
```

# Installation
Install [Yapoml.Playwright](https://www.nuget.org/packages/Yapoml.Playwright) nuget package and create your `*.page.yaml` files.