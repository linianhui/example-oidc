# OIDC Servers
1. `src/web.oidc.server.ids3` : ids3 (https://github.com/IdentityServer/IdentityServer3) example (with qqconnect external login).
1. `src/web.oidc.server.ids4` : ids4 (https://github.com/IdentityServer/IdentityServer4) example (with qqconnect external login).

# OIDC Clients
1. `src/web.oidc.client.implicit` : web site, implicit flow (use ids3 and ids4).
1. `src/web.oidc.client.js` : static web site, js client, implicit flow (use ids3).
1. `src/uwp.oidc.client.authorization-code-flow` : uwp app, use authorization code flow(use ids3).

# OAuth2 Protected Resource
1. `src/web.oauth2.resources` : use oauth2 access token protected api(https://github.com/IdentityServer/IdentityServer3.AccessTokenValidation)，authz server is ids3.

# OAuth2 Clients
1. `src/web.oauth2.client.aspnetcore` : asp.net core2 and qqconnect.
1. `src/web.oauth2.client.owin` : asp.net owin and qqconnect.

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
