#addin "Cake.IIS"
#addin "Cake.Hosts"
#addin "Cake.FileHelpers"
#addin "Cake.Powershell"

/// params
var target = Argument("target", "default");

/// web sites config
var webSites = new []{
    new {
        host="server.ids3.dev",
        path= "./src/servers/server"
    },
    new {
        host="client.implicit.dev",
        path="./src/clients/client.implicit"
    },
    new {
        host="client.js.dev",
        path="./src/clients/client.js"
    }
};


/// build task
Task("build")
    .IsDependentOn("clean")
    .IsDependentOn("restore-nuget-packages")
    .Does(() =>
{
    MSBuild("./ids3.demo.sln",new MSBuildSettings {
		Verbosity = Verbosity.Minimal
    });
});

Task("restore-nuget-packages")
    .Does(() =>
{
    NuGetRestore("./ids3.demo.sln");
});

Task("clean")
    .Does(() =>
{
	CleanDirectories(GetDirectories("./src/*/bin"));
});


/// deploy task
Task("deploy")
    .Does(() =>
{
    foreach(var website in webSites){
        DeleteSite(website.host);
        CreateWebsite(new WebsiteSettings()
        {
            Name = website.host,
            Binding = IISBindings.Http
                        .SetHostName(website.host)
                        .SetIpAddress("*")
                        .SetPort(80),
            PhysicalDirectory = website.path,
            ApplicationPool = new ApplicationPoolSettings()
            {
                Name = "ids3-dev",
                IdentityType = IdentityType.LocalSystem,
		        MaxProcesses = 1
            }
        });
    }
});

/// set local dns task
Task("set-local-dns")
    .Does(() =>
{
    foreach(var website in webSites){
        AddHostsRecord("127.0.0.1", website.host);
    }
});

/// open browser task
Task("open-browser")
    .Does(() =>
{
    StartPowershellScript("Start-Process", args =>
    {
        var urls = "";
        foreach(var website in webSites){
            urls += ",'http://" + website.host + "/'";
        }

        args.Append("chrome.exe")
            .Append("'-incognito'")
            .Append(urls);
    });
});


/// default task
Task("default")
.IsDependentOn("build")
.IsDependentOn("deploy")
.IsDependentOn("set-local-dns")
.IsDependentOn("open-browser");

RunTarget(target);
