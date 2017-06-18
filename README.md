# OIDC Example
1. `oidc.server-site.ids3.csproj` ：ids3 (https://github.com/IdentityServer/IdentityServer3) example.
1. `oidc.server-site.ids3.csproj` ：ids4 (https://github.com/IdentityServer/IdentityServer4) example.
1. `oidc.client-site.implicit.csproj` : web site, implicit flow.
1. `oidc.client-site.js.csproj` : js client, implicit flow.
1. `oidc.client-uwp.authorization-code-flow.csproj` : uwp app, use authorization code flow.

# OAuth2 QQConnect Middleware
1. `oauth2.middleware.qqconnect-owin.csproj` : asp.net owin.
1. `oauth2.middleware.qqconnect-core.csproj` : asp.net core.

# How to run?
Use administrator run build.ps1 to deploy demo web site to local IIS. 

If you want use QQ Connect, please replace `*ClientId and ClientSercet*` in `*/src/_shared/QQConnectConfig.cs*` file.
``` csharp
public static class QQConnectConfig
{
    public static readonly string ClientId = "You App Id";
    public static readonly string ClientSecret = "You App Secret";
}
```
