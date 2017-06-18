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
        host = "server.ids3.dev",
        path = "./src/oidc.server-site.ids3",
        appPoolName = appPoolClr4
    },
    new {
        host = "client.implicit.dev",
        path = "./src/oidc.client-site.implicit",
        appPoolName = appPoolClr4
    },
    new {
        host = "client.js.dev",
        path = "./src/oidc.client-site.js",
        appPoolName = appPoolClr4
    },
    new {
        host = "oauth2.asp-net-owin.dev",
        path = "./src/oauth2.client-site.owin",
        appPoolName = appPoolClr4
    },
    new {
        host = "oauth2.asp-net-core.dev",
        path = "./src/oauth2.client-site.core/_publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "server.ids4.dev",
        path = "./src/oidc.server-site.ids4/_publish",
        appPoolName = appPoolNoClr
    }
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

    DotNetCorePublish("./src/oauth2.client-site.core/oauth2.client-site.core.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp1.1",
        OutputDirectory = "./src/oauth2.client-site.core/_publish/"
    });

    DotNetCorePublish("./src/oidc.server-site.ids4/oidc.server-site.ids4.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp1.1",
        OutputDirectory = "./src/oidc.server-site.ids4/_publish/"
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
