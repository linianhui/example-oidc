# OIDC Example
1. `src/web.oidc.server.ids3` : ids3 (https://github.com/IdentityServer/IdentityServer3) example.
1. `src/web.oidc.server.ids4` : ids4 (https://github.com/IdentityServer/IdentityServer4) example.
1. `src/web.oidc.client.implicit` : web site, implicit flow.
1. `src/web.oidc.client.js` : js client, implicit flow.
1. `src/uwp.oidc.client.authorization-code-flow` : uwp app, use authorization code flow.
1. `src/web.oauth2.resources` : use oauth2 access token protected api(https://github.com/IdentityServer/IdentityServer3.AccessTokenValidation).

# OAuth2 QQConnect Middleware
1. `src/oauth2.qqconnect.owin` : asp.net owin.
1. `src/oauth2.qqconnect.core1` : asp.net core 1.

# How to run?
Use administrator run build.ps1 to deploy demo web site to local IIS. 

If you want use QQ Connect, please replace `ClientId` and `ClientSercet` in `src/_shared/QQConnectConfig.cs` file.
``` csharp
public static class QQConnectConfig
{
    public static readonly string ClientId = "You App Id";
    public static readonly string ClientSecret = "You App Secret";
}
```
# Blog
http://www.cnblogs.com/linianhui/category/929878.html
