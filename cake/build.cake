#addin nuget:?package=microsoft.win32.registry&version=4.0.0.0
#addin nuget:?package=system.reflection.typeextensions&version=4.1.0.0
#addin nuget:?package=system.serviceprocess.servicecontroller&version=4.1.0.0
#addin nuget:?package=cake.iis&version=0.4.2
#addin nuget:?package=cake.hosts&version=1.5.1
#addin nuget:?package=cake.powershell&version=0.4.8

#load website.cake

/// params
var target = Argument("target", "default");

/// constant
var rootPath    = "../";
var srcPath     = rootPath + "src/";
var slnPath     = rootPath + "example-oidc.sln";
var webSiteList = GetWebSiteList(srcPath);

Task("clean")
    .Description("清理项目缓存")
    .Does(() =>
{
    DeleteFiles(srcPath + "**/_*.log");
    CleanDirectories(srcPath + "**/bin");
    CleanDirectories(srcPath + "**/obj");
});


Task("restore")
    .Description("还原项目依赖")
    .Does(() =>
{
    DotNetCoreRestore(slnPath);
    NuGetRestore(slnPath);
});


Task("build")
    .Description("编译项目")
    .IsDependentOn("clean")
    .IsDependentOn("restore")
    .Does(() =>
{
    MSBuild(slnPath, new MSBuildSettings {
        Verbosity   = Verbosity.Minimal
    });
});


Task("deploy-iis")
    .Description("部署到本机IIS")
    .Does(() =>
{

    foreach(var webSite in webSiteList)
    {
        DeleteSite(webSite.Host);

        CreateWebsite(new WebsiteSettings {
            Name              = webSite.Host,
            Binding           = IISBindings.Http
                                   .SetHostName(webSite.Host)
                                   .SetIpAddress("*")
                                   .SetPort(80),
            ServerAutoStart   = true,
            PhysicalDirectory = webSite.Path,
            ApplicationPool   = webSite.ApplicationPool
        });

        AddHostsRecord("127.0.0.1", webSite.Host);

        StartSite(webSite.Host);
    }
});


Task("open-browser")
    .Description("用浏览器打开部署的站点")
    .Does(() =>
{
    var hostList = webSiteList.Select(_ => "http://" + _.Host).ToList();
    var urlList = string.Join(" , ", hostList);
    var script = "Start-Process -FilePath chrome.exe -ArgumentList '-incognito' , " + urlList;
    StartPowershellScript(script);
});


Task("default")
    .Description("默认执行open-browser")
    .IsDependentOn("build")
    .IsDependentOn("deploy-iis")
    .IsDependentOn("open-browser");

RunTarget(target);
