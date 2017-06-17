# OIDC Example
Use ids3 (https://github.com/IdentityServer/IdentityServer3) as oidc server.

OIDC (OpenID Connect) ：http://openid.net/connect

# How to run?
Use administrator run build.ps1 to deploy demo web site to local IIS. 

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
