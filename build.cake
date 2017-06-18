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
        path = "./src/servers/ids3.host",
        appPoolName = appPoolClr4
    },
    new {
        host = "client.implicit.dev",
        path = "./src/clients/client.implicit",
        appPoolName = appPoolClr4
    },
    new {
        host = "client.js.dev",
        path = "./src/clients/client.js",
        appPoolName = appPoolClr4
    },
    new {
        host = "oauth2.owin.dev",
        path = "./src/middleware/hosts/owin.oauth2.host",
        appPoolName = appPoolClr4
    },
    new {
        host = "oauth2.asp-net-core.dev",
        path = "./src/middleware/hosts/core.oauth2.host/_publish",
        appPoolName = appPoolNoClr
    },
    new {
        host = "server.ids4.dev",
        path = "./src/servers/ids4.host/_publish",
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

    DotNetCorePublish("./src/middleware/hosts/core.oauth2.host/core.oauth2.host.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp1.1",
        OutputDirectory = "./src/middleware/hosts/core.oauth2.host/_publish/"
    });

    DotNetCorePublish("./src/servers/ids4.host/ids4.host.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp1.1",
        OutputDirectory = "./src/servers/ids4.host/_publish/"
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
