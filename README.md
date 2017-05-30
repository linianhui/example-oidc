# Ids3.demo
demo for https://github.com/IdentityServer/IdentityServer3.

Use administrator run build.ps1 to deploy demo web site to IIS, then need edit hosts file add this two line：
```
127.0.0.1     server.ids3.dev
127.0.0.1     client.implicit.dev
```
Add QQ Connect Authenticate Login (AS ids3 External IdentityProvider).If you want use QQ Login,please fill in *your AppId and AppSercet*.
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
