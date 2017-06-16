# Ids3.demo
demo for https://github.com/IdentityServer/IdentityServer3.

Use administrator run build.ps1 to deploy demo web site to local IIS. If you want use QQ Login,please fill in *your AppId and AppSercet*.
``` csharp
app.UseQQConnectAuthentication(new QQConnectAuthenticationOptions
 {
    Caption = "QQ",
    AuthenticationType = Ids3Constants.QQIdp,
    SignInAsAuthenticationType = signInAsAuthenticationType,
    AppId = "You App Id",
    AppSecret = "You App Secret"
});
```
