# OIDC Servers
1. `src/web.oidc.server.ids4` : ids4 (https://github.com/IdentityServer/IdentityServer4) example (with qqconnect external login).

# OIDC Clients
1. `src/web.oidc.client.hybrid` : web site, hybrid flow.
1. `src/web.oidc.client.implicit` : web site, implicit flow.
1. `src/web.oidc.client.js` : web site(static), implicit flow .
1. `src/uwp.oidc.client.authorization-code` : uwp app, authorization code flow.
1. `src/console.oidc.client.client-credentials` ：console app，client credentials flow(oauth2).

# OAuth2 Protected Resource
1. `src/web.oauth2.resources` : resources api.

# OAuth2 Clients
1. `src/web.oauth2.client.aspnetcore` : asp.net core2 with qqconnect.
1. `src/web.oauth2.client.owin` : asp.net owin with qqconnect.

# OAuth2 QQConnect Middleware
1. `src/oauth2.qqconnect.owin` : asp.net owin.
1. `src/oauth2.qqconnect.aspnetcore` : asp.net core 2.

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

# ids3
ids3 (https://github.com/IdentityServer/IdentityServer3) example (with qqconnect external login)：https://github.com/linianhui/oidc.example/tree/old 
