#addin "Cake.IIS"
#addin "Cake.Hosts"
#addin "Cake.Powershell"

/// params
var target = Argument("target", "default");

/// iis app pool config
var appPoolClr4 = "oidc-example.clr4";
var appPoolNoClr = "oidc-example.noclr";

/// 修改.dev域名为.test域名（原因：新版chrome强制.dev采用HTTPS https://chromium-review.googlesource.com/c/chromium/src/+/669923）
/// iis web sites config
var webSiteConfigs = new []{
    new {
        host = "oidc-server.test",
        path = "./src/web.oidc.server.ids4/bin/publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oidc-client-hybrid.test",
        path = "./src/web.oidc.client.hybrid/bin/publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oidc-client-implicit.test",
        path = "./src/web.oidc.client.implicit",
        appPoolName = appPoolClr4
    },
    new {
        host = "oidc-client-js.test",
        path = "./src/web.oidc.client.js",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oauth2-resources-aspnetcore.test",
        path = "./src/web.oauth2.resources.aspnetcore/bin/publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oauth2-resources-owin.test",
        path = "./src/web.oauth2.resources.owin",
        appPoolName = appPoolClr4
    },
    new {
        host = "oauth2-client-aspnetcore.test",
        path = "./src/web.oauth2.client.aspnetcore/bin/publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oauth2-client-owin.test",
        path = "./src/web.oauth2.client.owin",
        appPoolName = appPoolClr4
    },
};

Task("clean")
    .Description("清理项目缓存")
    .Does(() =>
{
	CleanDirectories("./src/**/bin");
});


Task("restore")
    .Description("还原项目依赖")
    .Does(() =>
{
    NuGetRestore("./oidc.example.sln");
});


Task("build")
    .Description("编译项目")
    .IsDependentOn("clean")
    .IsDependentOn("restore")
    .Does(() =>
{
    MSBuild("./oidc.example.sln", new MSBuildSettings {
		Verbosity = Verbosity.Minimal
    });
});


Task("publish")
    .Description("发布项目")
    .Does(() =>
{ 
    StopPool(appPoolNoClr);

    CleanDirectories("./src/**/bin/publish");

    DotNetCorePublish("./src/web.oauth2.resources.aspnetcore/web.oauth2.resources.aspnetcore.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        OutputDirectory = "./src/web.oauth2.resources.aspnetcore/bin/publish"
    });

     DotNetCorePublish("./src/web.oidc.client.hybrid/web.oidc.client.hybrid.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        OutputDirectory = "./src/web.oidc.client.hybrid/bin/publish"
    });

    DotNetCorePublish("./src/web.oauth2.client.aspnetcore/web.oauth2.client.aspnetcore.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        OutputDirectory = "./src/web.oauth2.client.aspnetcore/bin/publish"
    });

    DotNetCorePublish("./src/web.oidc.server.ids4/web.oidc.server.ids4.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        OutputDirectory = "./src/web.oidc.server.ids4/bin/publish"
    });

    StartPool(appPoolNoClr);
});


Task("deploy")
    .Description("部署到本机IIS")
    .Does(() =>
{
    CreatePool(new ApplicationPoolSettings()
    {
        Name = appPoolClr4,
        IdentityType = IdentityType.LocalSystem,
        MaxProcesses = 1,
        ManagedRuntimeVersion = "v4.0"
    });

     CreatePool(new ApplicationPoolSettings()
    {
        Name = appPoolNoClr,
        IdentityType = IdentityType.LocalSystem,
        MaxProcesses = 1,
        ManagedRuntimeVersion = null
    });

    foreach(var webSiteConfig in webSiteConfigs){

        DeleteSite(webSiteConfig.host);

        CreateWebsite(new WebsiteSettings()
        {
            Name = webSiteConfig.host,
            Binding = IISBindings.Http
                                 .SetHostName(webSiteConfig.host)
                                 .SetIpAddress("*")
                                 .SetPort(80),
            ServerAutoStart = true,
            PhysicalDirectory = webSiteConfig.path,
            ApplicationPool = new ApplicationPoolSettings()
            {
                Name = webSiteConfig.appPoolName
            }
        });
        
        AddHostsRecord("127.0.0.1", webSiteConfig.host);

        StartSite(webSiteConfig.host);
    }
});


Task("open-browser")
    .Description("用浏览器打开部署的站点")
    .Does(() =>
{
    StartPowershellScript("Start-Process", args =>
    {
        var urls = "";
        foreach(var webSiteConfig in webSiteConfigs){
            urls += ",'http://" + webSiteConfig.host + "/'";
        }

        args.Append("chrome.exe")
            .Append("'-incognito'")
            .Append(urls);
    });
});


Task("default")
    .Description("默认执行open-browser")
    .IsDependentOn("build")
    .IsDependentOn("publish")
    .IsDependentOn("deploy")
    .IsDependentOn("open-browser");

RunTarget(target);
