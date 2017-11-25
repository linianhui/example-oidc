#addin "Cake.IIS"
#addin "Cake.Hosts"
#addin "Cake.FileHelpers"
#addin "Cake.Powershell"

/// params
var target = Argument("target", "default");

/// iis app pool config
var appPoolClr4 = "clr4.oidc-example";
var appPoolNoClr = "noclr.oidc-example";

/// iis web sites config
var webSiteConfigs = new []{
    new {
        host = "oidc-server.dev",
        path = "./src/web.oidc.server.ids4/_publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oidc-client-hybrid.dev",
        path = "./src/web.oidc.client.hybrid/_publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oidc-client-implicit.dev",
        path = "./src/web.oidc.client.implicit",
        appPoolName = appPoolClr4
    },
    new {
        host = "oidc-client-js.dev",
        path = "./src/web.oidc.client.js",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oauth2-protected-resources.dev",
        path = "./src/web.oauth2.resources",
        appPoolName = appPoolClr4
    },
    new {
        host = "oauth2-client-asp-net-core.dev",
        path = "./src/web.oauth2.client.aspnetcore/_publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "oauth2-client-asp-net-owin.dev",
        path = "./src/web.oauth2.client.owin",
        appPoolName = appPoolClr4
    },
};

/// clean task
Task("clean")
    .Does(() =>
{
	CleanDirectories("./src/**/bin");
});


/// restor nuget packages task
Task("restore")
    .Does(() =>
{
    NuGetRestore("./oidc.example.sln");
});

/// build task
Task("build")
    .IsDependentOn("clean")
    .IsDependentOn("restore")
    .Does(() =>
{
    MSBuild("./oidc.example.sln", new MSBuildSettings {
		Verbosity = Verbosity.Minimal
    });
});

/// publish asp.net core project task
Task("publish")
    .Does(() =>
{ 
    StopPool(appPoolNoClr);

    CleanDirectories("./src/**/_publish");

    DotNetCorePublish("./src/web.oidc.client.hybrid/web.oidc.client.hybrid.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        OutputDirectory = "./src/web.oidc.client.hybrid/_publish"
    });

    DotNetCorePublish("./src/web.oauth2.client.aspnetcore/web.oauth2.client.aspnetcore.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        OutputDirectory = "./src/web.oauth2.client.aspnetcore/_publish/"
    });

    DotNetCorePublish("./src/web.oidc.server.ids4/web.oidc.server.ids4.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        OutputDirectory = "./src/web.oidc.server.ids4/_publish/"
    });

    StartPool(appPoolNoClr);
});

/// deploy task
Task("deploy")
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

/// open browser task
Task("open-browser")
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


/// default task
Task("default")
.IsDependentOn("build")
.IsDependentOn("publish")
.IsDependentOn("deploy")
.IsDependentOn("open-browser");

RunTarget(target);
