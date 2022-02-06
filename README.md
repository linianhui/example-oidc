<!-- TOC -->
# Table of content
- [Table of content](#table-of-content)
- [CI](#ci)
- [How to run?](#how-to-run)
  - [docker-compose.yml (docker platform)](#docker-composeyml-docker-platform)
  - [build.ps1 (windows platform)](#buildps1-windows-platform)
  - [Deployed web site](#deployed-web-site)
- [OIDC Servers](#oidc-servers)
- [OIDC Clients](#oidc-clients)
- [OAuth2 Clients](#oauth2-clients)
- [OAuth2 Resources Servers](#oauth2-resources-servers)
- [OAuth2 Middleware](#oauth2-middleware)
  - [Global Config](#global-config)
- [Blog](#blog)
- [Old version(ids3 and owin)](#old-versionids3-and-owin)

<!-- /TOC -->

# CI

| CI            | Platform | Stauts                                      |
| ------------- | -------- | ------------------------------------------- |
| GitHub Action | Windows  | [![GitHub-Actions-Img]][GitHub-Actions-Url] |

# How to run?

** use ie11 to run**

>
> <https://github.com/linianhui/example-oidc/issues/34>  
> <mark><b>⚠️ disable chrome samesite for test: </b></mark> <a href="chrome://flags/#same-site-by-default-cookies">chrome://flags/#same-site-by-default-cookies</a>

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
127.0.0.1 traefik.test
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
1. vs 2019 16.4 + 
2. .net framework 4.6.1 sdk
3. .net core 3.1 sdk 
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


## Deployed web site

| web site                                       | docker | windows | description                                                 |
| :--------------------------------------------- | :----: | :-----: | :---------------------------------------------------------- |
| http://traefik.test                            |   ✔    |         | reverse proxy : traefik                                     |
| http://oidc-server.test                        |   ✔    |    ✔    | oidc server : asp.net core 3.1                              |
| http://oidc-client-hybrid.test                 |   ✔    |    ✔    | oidc client : asp.net core 3.1                              |
| http://oidc-client-implicit.test               |        |    ✔    | oidc client : asp.net owin 4                                |
| http://oidc-client-js.test                     |   ✔    |    ✔    | oidc client : html js(use `access_token` call resource api) |
| http://oauth2-resources-aspnetcore.test &nbsp; |   ✔    |    ✔    | oauth2 resources api : asp.net core 3.1                     |
| http://oauth2-resources-nodejs.test            |   ✔    |         | oauth2 resources api : node.js                              |
| http://oauth2-resources-java.test              |   ✔    |         | oauth2 resources api : java (spring boot)                   |
| http://oauth2-resources-owin.test              |        |    ✔    | oauth2 resources api : asp.net webapi 2                     |
| http://oauth2-client-aspnetcore.test           |   ✔    |    ✔    | oauth2 client : asp.net core 3.1                            |
| http://oauth2-client-owin.test                 |        |    ✔    | oauth2 client : asp.net owin 4                              |


# OIDC Servers

1. [src/web.oidc.server.ids4](src/web.oidc.server.ids4) : ids4 (https://github.com/IdentityServer/IdentityServer4) example (with github, qqconnect external login).


# OIDC Clients

1. [src/web.oidc.client.hybrid](src/web.oidc.client.hybrid) : web site, hybrid flow.
1. [src/web.oidc.client.implicit](src/web.oidc.client.implicit) : web site, implicit flow.
1. [src/web.oidc.client.js](src/web.oidc.client.js) : web site(static), implicit flow .
1. [src/uwp.oidc.client.authorization-code](src/uwp.oidc.client.authorization-code) : uwp app, authorization code flow.
1. [src/wpf.oidc.client.authorization-code](src/wpf.oidc.client.authorization-code) : wpf app, authorization code flow.


# OAuth2 Clients

1. [src/web.oauth2.client.aspnetcore](src/web.oauth2.client.aspnetcore) : asp.net core 3.1.
1. [src/web.oauth2.client.owin](src/web.oauth2.client.owin) : asp.net owin.
1. [src/console.oauth2.client.client-credentials](src/console.oauth2.client.client-credentials) : console app, client credentials flow(oauth2).
1. [src/console.oauth2.client.resource-owner-password-credentials](src/console.oauth2.client.resource-owner-password-credentials) : console app, resource owner password credentials flow(oauth2).


# OAuth2 Resources Servers

1. [src/web.oauth2.resources.aspnetcore](src/web.oauth2.resources.aspnetcore): asp.net core 3.1.
1. [src/web.oauth2.resources.owin](src/web.oauth2.resources.owin): asp.net owin.
1. [src/web.oauth2.resources.nodejs](src/web.oauth2.resources.nodejs): node.js.
1. [src/web.oauth2.resources.java](src/web.oauth2.resources.java): java (spring boot 2.2.1).


# OAuth2 Middleware

1. [src/oauth2.github.aspnetcore](src/oauth2.github.aspnetcore): asp.net core 3.1.
1. [src/oauth2.qqconnect.aspnetcore](src/oauth2.qqconnect.aspnetcore): asp.net core 3.1.
1. [src/oauth2.qqconnect.owin](src/oauth2.qqconnect.owin): asp.net owin.


## Global Config

If you want use QQ Connect or Github, please replace `ClientId` and `ClientSercet` in [src/_shared/GlobalConfig.cs](src/_shared/GlobalConfig.cs) file.
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


# Blog

Authentication and Authorization: http://www.cnblogs.com/linianhui/category/929878.html

OIDC in Action: http://www.cnblogs.com/linianhui/category/1121078.html


# Old version(ids3 and owin)

1. ids3: https://github.com/linianhui/example-oidc/tree/ids3
2. ids4 and owin: https://github.com/linianhui/example-oidc/tree/owin


[GitHub-Actions-Img]:https://github.com/linianhui/example-oidc/workflows/build/badge.svg
[GitHub-Actions-Url]:https://github.com/linianhui/example-oidc/actions
