# OIDC Servers
1. `src/web.oidc.server.ids4` : ids4 (https://github.com/IdentityServer/IdentityServer4) example (with qqconnect external login).
> ids3 (https://github.com/IdentityServer/IdentityServer3) example (with qqconnect external login)：https://github.com/linianhui/oidc.example/tree/ids3 

# OIDC Clients
1. `src/web.oidc.client.hybrid` : web site, hybrid flow.
1. `src/web.oidc.client.implicit` : web site, implicit flow.
1. `src/web.oidc.client.js` : web site(static), implicit flow .
1. `src/uwp.oidc.client.authorization-code` : uwp app, authorization code flow.
1. `src/console.oidc.client.client-credentials` ：console app，client credentials flow(oauth2).

# OAuth2 Protected Resource
1. `src/web.oauth2.resources.aspnetcore.` : asp.net core 2).
1. `src/web.oauth2.resources.owin.` : asp.net owin.

# OAuth2 Clients
1. `src/web.oauth2.client.aspnetcore` : asp.net core 2.
1. `src/web.oauth2.client.owin` : asp.net owin.

# OAuth2 QQConnect Middleware
1. `src/oauth2.qqconnect.aspnetcore` : asp.net core 2.
1. `src/oauth2.qqconnect.owin` : asp.net owin.

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
Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

OIDC in Action: http://www.cnblogs.com/linianhui/category/1121078.html
