#addin "Cake.IIS"
#addin "Cake.FileHelpers"
#addin "Cake.Powershell"

///params
var target = Argument("target", "default");

/// web sites config
var webSites = new []{
    new {
        host="server.ids3.dev",
        path= "./src/server"
    },
    new {
        host="client.implicit.dev",
        path="./src/client.implicit"
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
                IdentityType = IdentityType.LocalSystem
            }
        });
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
.IsDependentOn("open-browser");

RunTarget(target);