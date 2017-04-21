#addin "Cake.IIS"
#addin "Cake.FileHelpers"
#addin "Cake.Powershell"

var target = Argument("target", "default");
var appPoolName = "demo-ids3-app-pool";

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
.IsDependentOn("create-app-pool")
.IsDependentOn("create-demo.ids3.server-web-site")
.IsDependentOn("create-demo.implicit.client-web-site");

Task("create-demo.ids3.server-web-site")
    .Does(() =>
{
    CreateWebsite(new WebsiteSettings()
    {
        Name = "demo.ids3.server",
        Binding = IISBindings.Http
                    .SetHostName("demo.ids3.server")
                    .SetIpAddress("*")
                    .SetPort(80),
        PhysicalDirectory = "./src/server",
        ApplicationPool = new ApplicationPoolSettings()
        {
            Name = appPoolName
        }
    });
});

Task("create-demo.implicit.client-web-site")
    .Does(() =>
{
    CreateWebsite(new WebsiteSettings()
    {
        Name = "demo.implicit.client",
        Binding = IISBindings.Http
                    .SetHostName("demo.implicit.client")
                    .SetIpAddress("*")
                    .SetPort(80),
        PhysicalDirectory = "./src/client.implicit",
        ApplicationPool = new ApplicationPoolSettings()
        {
            Name = appPoolName
        }
    });
});

Task("create-app-pool")
    .Does(() =>
{
	if(PoolExists(appPoolName) == false){
		CreatePool(new ApplicationPoolSettings()
		{
			Name = appPoolName,
			IdentityType=IdentityType.LocalSystem
		});
	}
});

/// open browser task
Task("open-browser")
    .Does(() =>
{
    StartPowershellScript("Start-Process", args =>
    {
        args.Append("chrome.exe")
            .AppendQuoted("-incognito")
            .Append(",")
            .AppendQuoted("http://demo.ids3.server")
            .Append(",")
            .AppendQuoted("http://demo.implicit.client");
    });
});


/// default task
Task("default")
.IsDependentOn("build")
.IsDependentOn("deploy")
.IsDependentOn("open-browser");

RunTarget(target);