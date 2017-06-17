#addin "Cake.IIS"
#addin "Cake.Hosts"
#addin "Cake.FileHelpers"
#addin "Cake.Powershell"

/// params
var target = Argument("target", "default");

/// iis app pool config
var appPoolConfigs = new []{
    new {
        name = "clr4.oidc-example",
        clrVersion = "v4.0",
    },
    new {
        name = "noclr.oidc-example",
        clrVersion = "",
    }
};

/// iis web sites config
var webSiteConfigs = new []{
    new {
        host = "server.ids3.dev",
        path = "./src/servers/ids3.host",
        appPoolName = appPoolConfigs[0].name
    },
    new {
        host = "server.ids4.dev",
        path = "./src/servers/ids4.host/_publish",
        appPoolName = appPoolConfigs[1].name
    },
    new {
        host = "client.implicit.dev",
        path = "./src/clients/client.implicit",
        appPoolName = appPoolConfigs[0].name
    },
    new {
        host = "client.js.dev",
        path = "./src/clients/client.js",
        appPoolName = appPoolConfigs[0].name
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
    DotNetCorePublish("./src/servers/ids4.host/ids4.host.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp1.1",
        OutputDirectory = "./src/servers/ids4.host/_publish/"
    });
});

/// undeploy task
Task("undeploy")
    .Does(() =>
{
    foreach(var appPoolConfig in appPoolConfigs){
        StopPool(appPoolConfig.name);
    }

    foreach(var webSiteConfig in webSiteConfigs){
        DeleteSite(webSiteConfig.host);
    }
});

/// deploy task
Task("deploy")
    .Does(() =>
{
    foreach(var appPoolConfig in appPoolConfigs){
        CreatePool(new ApplicationPoolSettings()
        {
            Name = appPoolConfig.name,
            IdentityType = IdentityType.LocalSystem,
            MaxProcesses = 1,
            ManagedRuntimeVersion = appPoolConfig.clrVersion
        });

        StartPool(appPoolConfig.name);
    }

    foreach(var webSiteConfig in webSiteConfigs){
        AddHostsRecord("127.0.0.1", webSiteConfig.host);
        
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
.IsDependentOn("undeploy")
.IsDependentOn("build")
.IsDependentOn("publish")
.IsDependentOn("deploy")
.IsDependentOn("open-browser");

RunTarget(target);
