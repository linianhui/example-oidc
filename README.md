# Ids3.demo
demo for https://github.com/IdentityServer/IdentityServer3.

If you want use QQ Login,please fill in *your AppId and AppSercet*.
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
