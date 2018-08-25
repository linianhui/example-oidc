<!-- TOC -->
# Table of content
- [OIDC Servers](#oidc-servers)
- [OIDC Clients](#oidc-clients)
- [OAuth2 Protected Resource](#oauth2-protected-resource)
- [OAuth2 Clients](#oauth2-clients)
- [OAuth2 QQConnect Middleware](#oauth2-qqconnect-middleware)
    - [QQ Config](#qq-config)
- [Deployed web site](#deployed-web-site)
- [How to run?](#how-to-run)
    - [docker-compose.yml (docker platform)](#docker-composeyml-docker-platform)
    - [build.ps1 (windows platform)](#buildps1-windows-platform)
- [Blog](#blog)
- [Old version(ids3 and owin)](#old-versionids3-and-owin)

<!-- /TOC -->
# OIDC Servers
1. [1-src/web.oidc.server.ids4](1-src/web.oidc.server.ids4) : ids4 (https://github.com/IdentityServer/IdentityServer4) example (with qqconnect external login).

# OIDC Clients
1. [1-src/web.oidc.client.hybrid](1-src/web.oidc.client.hybrid) : web site, hybrid flow.
1. [1-src/web.oidc.client.implicit](1-src/web.oidc.client.implicit) : web site, implicit flow.
1. [1-src/web.oidc.client.js](1-src/web.oidc.client.js) : web site(static), implicit flow .
1. [1-src/uwp.oidc.client.authorization-code](1-src/uwp.oidc.client.authorization-code) : uwp app, authorization code flow.
1. [1-src/console.oidc.client.client-credentials](1-src/console.oidc.client.client-credentials) ：console app，client credentials flow(oauth2).

# OAuth2 Protected API Resources
1. [1-src/web.oauth2.resources.aspnetcore](1-src/web.oauth2.resources.aspnetcore): asp.net core 2.
1. [1-src/web.oauth2.resources.owin](1-src/web.oauth2.resources.owin): asp.net owin.
1. [1-src/web.oauth2.resources.nodejs](1-src/web.oauth2.resources.nodejs): node.js.

# OAuth2 Clients
1. [1-src/web.oauth2.client.aspnetcore](1-src/web.oauth2.client.aspnetcore) : asp.net core 2.
1. [1-src/web.oauth2.client.owin](1-src/web.oauth2.client.owin) : asp.net owin.

# OAuth2 QQConnect Middleware
1. [1-src/oauth2.qqconnect.aspnetcore](1-src/oauth2.qqconnect.aspnetcore): asp.net core 2.
1. [1-src/oauth2.qqconnect.owin](1-src/oauth2.qqconnect.owin): asp.net owin.

## QQ Config
If you want use QQ Connect, please replace `ClientId` and `ClientSercet` in [1-src/_shared/QQConnectConfig.cs](1-src/_shared/QQConnectConfig.cs) file.
``` csharp
public static class QQConnectConfig
{
    public static readonly string ClientId = "You App Id";
    public static readonly string ClientSecret = "You App Secret";
}
```

# Deployed web site

1. http://localhost:8080 (traefik web ui:only docker)
1. http://oidc-server.test
1. http://oidc-client-hybrid.test
1. http://oidc-client-implicit.test (only windows)
1. http://oidc-client-js.test (use oidc login and use access_token call api)
1. http://oauth2-resources-aspnetcore.test
1. http://oauth2-resources-nodejs.test (only docker)
1. http://oauth2-resources-owin.test (only windows)
1. http://oauth2-client-aspnetcore.test
1. http://oauth2-client-owin.test (only windows)

# How to run?

## docker-compose.yml (docker platform)
```bash
## start docker
docker-compose up --detach --build

## stop docker
docker-compose down
```

Update local `/etc/hosts`.
```bash
echo "\
\n127.0.0.1 oidc-server.test \
\n127.0.0.1 oidc-client-hybrid.test \
\n127.0.0.1 oidc-client-js.test \
\n127.0.0.1 oauth2-resources-nodejs.test \
\n127.0.0.1 oauth2-resources-aspnetcore.test \
\n127.0.0.1 oauth2-client-aspnetcore.test " \
| sudo tee -a /etc/hosts
```

## build.ps1 (windows platform)
Use administrator run `build.ps1` to deploy demo web site to local IIS. (Need install `vs 2017` with `.net framework 4.5+ sdk` and `.net core 2.1 sdk` )

```powershell
build.ps1 -help

build.ps1 -target {Task}

Task                          Description
================================================================================
clean                         清理项目缓存
restore                       还原项目依赖
build                         编译项目
publish                       发布项目
deploy                        部署到本机IIS
open-browser                  用浏览器打开部署的站点
default                       默认执行open-browser
```

# Blog
Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

OIDC in Action: http://www.cnblogs.com/linianhui/category/1121078.html

# Old version(ids3 and owin)
1. ids3: https://github.com/linianhui/oidc.example/tree/ids3
1. ids4 and owin: https://github.com/linianhui/oidc.example/tree/owin
