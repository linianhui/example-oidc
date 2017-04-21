#addin "Cake.IIS"
#addin "Cake.FileHelpers"

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
.IsDependentOn("create-demo.server.ids3-web-site");

Task("create-demo.server.ids3-web-site")
	.IsDependentOn("create-app-pool")
    .Does(() =>
{
    CreateWebsite(new WebsiteSettings()
    {
        Name = "demo.server.ids3",
        Binding = IISBindings.Http
                    .SetHostName("demo.server.ids3")
                    .SetIpAddress("*")
                    .SetPort(80),
        PhysicalDirectory = "./src/server",
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

/// default task
Task("default")
.IsDependentOn("build")
.IsDependentOn("deploy");

RunTarget(target);