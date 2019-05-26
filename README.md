<!-- TOC -->
# Table of content
- [Table of content](#table-of-content)
- [CI](#ci)
- [OIDC Servers](#oidc-servers)
- [OIDC Clients](#oidc-clients)
- [OAuth2 Resources Servers](#oauth2-resources-servers)
- [OAuth2 Clients](#oauth2-clients)
- [OAuth2 Middleware](#oauth2-middleware)
  - [Global Config](#global-config)
- [Deployed web site](#deployed-web-site)
- [How to run?](#how-to-run)
  - [docker-compose.yml (docker platform)](#docker-composeyml-docker-platform)
  - [build.ps1 (windows platform)](#buildps1-windows-platform)
- [Blog](#blog)
- [Old version(ids3 and owin)](#old-versionids3-and-owin)

<!-- /TOC -->

# CI
| CI Service | Build Platform | Stauts                                                                                                                                           |
| ---------- | -------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| AppVeyor   | Windows        | [![Build status](https://ci.appveyor.com/api/projects/status/qx3m0b5etxe339yt?svg=true)](https://ci.appveyor.com/project/linianhui/oidc-example) |

# OIDC Servers
1. [1-src/web.oidc.server.ids4](1-src/web.oidc.server.ids4) : ids4 (https://github.com/IdentityServer/IdentityServer4) example (with qqconnect external login).

# OIDC Clients
1. [1-src/web.oidc.client.hybrid](1-src/web.oidc.client.hybrid) : web site, hybrid flow.
1. [1-src/web.oidc.client.implicit](1-src/web.oidc.client.implicit) : web site, implicit flow.
1. [1-src/web.oidc.client.js](1-src/web.oidc.client.js) : web site(static), implicit flow .
1. [1-src/uwp.oidc.client.authorization-code](1-src/uwp.oidc.client.authorization-code) : uwp app, authorization code flow.
1. [1-src/wpf.oidc.client.authorization-code](1-src/wpf.oidc.client.authorization-code) : wpf app, authorization code flow.
1. [1-src/console.oidc.client.client-credentials](1-src/console.oidc.client.client-credentials) ：console app，client credentials flow(oauth2).

# OAuth2 Resources Servers
1. [1-src/web.oauth2.resources.aspnetcore](1-src/web.oauth2.resources.aspnetcore): asp.net core 2.
1. [1-src/web.oauth2.resources.owin](1-src/web.oauth2.resources.owin): asp.net owin.
1. [1-src/web.oauth2.resources.nodejs](1-src/web.oauth2.resources.nodejs): node.js.
1. [1-src/web.oauth2.resources.java](1-src/web.oauth2.resources.java): java (spring boot).

# OAuth2 Clients
1. [1-src/web.oauth2.client.aspnetcore](1-src/web.oauth2.client.aspnetcore) : asp.net core 2.
1. [1-src/web.oauth2.client.owin](1-src/web.oauth2.client.owin) : asp.net owin.

# OAuth2 Middleware
1. [1-src/oauth2.github.aspnetcore](1-src/oauth2.github.aspnetcore): asp.net core 2.
1. [1-src/oauth2.qqconnect.aspnetcore](1-src/oauth2.qqconnect.aspnetcore): asp.net core 2.
1. [1-src/oauth2.qqconnect.owin](1-src/oauth2.qqconnect.owin): asp.net owin.

## Global Config
If you want use QQ Connect or Github, please replace `ClientId` and `ClientSercet` in [1-src/_shared/GlobalConfig.cs](1-src/_shared/GlobalConfig.cs) file.
```csharp
public static class GlobalConfig
{
    public static class QQConnect
    {
        public static readonly string ClientId = "You App Id";
        public static readonly string ClientSecret = "You App Secret";
    }

    public static class Github
    {
        public static readonly string ClientId = "You App Id";
        public static readonly string ClientSecret = "You App Secret";
    }
}
```

# Deployed web site

| web site                                       | docker | windows | description                                                 |
| :--------------------------------------------- | :----: | :-----: | :---------------------------------------------------------- |
| http://localhost:8080                          |   ✔    |         | reverse proxy ：traefik                                     |
| http://oidc-server.test                        |   ✔    |    ✔    | oidc server : asp.net core 2.2                              |
| http://oidc-client-hybrid.test                 |   ✔    |    ✔    | oidc client : asp.net core 2.2                              |
| http://oidc-client-implicit.test               |        |    ✔    | oidc client : asp.net owin 4                                |
| http://oidc-client-js.test                     |   ✔    |    ✔    | oidc client : html js(use `access_token` call resource api) |
| http://oauth2-resources-aspnetcore.test &nbsp; |   ✔    |    ✔    | oauth2 resources api : asp.net core 2.2                     |
| http://oauth2-resources-nodejs.test            |   ✔    |         | oauth2 resources api : node.js                              |
| http://oauth2-resources-java.test              |   ✔    |         | oauth2 resources api : java (spring boot)                   |
| http://oauth2-resources-owin.test              |        |    ✔    | oauth2 resources api : asp.net webapi 2                     |
| http://oauth2-client-aspnetcore.test           |   ✔    |    ✔    | oauth2 client : asp.net core 2.2                            |
| http://oauth2-client-owin.test                 |        |    ✔    | oauth2 client : asp.net owin 4                              |

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
cat <<EOF >> /etc/hosts
127.0.0.1 oidc-server.test
127.0.0.1 oidc-client-hybrid.test
127.0.0.1 oidc-client-js.test
127.0.0.1 oauth2-resources-nodejs.test
127.0.0.1 oauth2-resources-aspnetcore.test
127.0.0.1 oauth2-resources-java.test
127.0.0.1 oauth2-client-aspnetcore.test
EOF
```

## build.ps1 (windows platform)
Use administrator run `build.ps1` to deploy demo web site to local IIS. Required : 
1. vs 2019
2. .net framework 4.5+ sdk
3. .net core 2.2 sdk
4. ASP.NET Core Module

```powershell
build.ps1 -help

build.ps1 -target {Task}

Task                          Description
================================================================================
clean                         清理项目缓存
restore                       还原项目依赖
build                         编译项目
deploy-iis                    部署到本机IIS
open-browser                  用浏览器打开部署的站点
default                       默认执行open-browser
```

# Blog
Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

OIDC in Action: http://www.cnblogs.com/linianhui/category/1121078.html

# Old version(ids3 and owin)
1. ids3: https://github.com/linianhui/oidc.example/tree/ids3
2. ids4 and owin: https://github.com/linianhui/oidc.example/tree/owin
