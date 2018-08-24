#addin nuget:?package=cake.iis&version=0.3.0
#addin nuget:?package=cake.hosts&version=1.1.0
#addin nuget:?package=cake.powershell&version=0.4.5
#load web.project.cake

/// params
var target = Argument("target", "default");

/// constant
var rootPath = "../";
var srcPath = rootPath + "1-src/";
var wwwPath = rootPath + "www/";
var slnPath = rootPath + "oidc.example.sln";

IList<WebProject> webProjects = GetWebProjects(srcPath, wwwPath);

Task("clean")
    .Description("清理项目缓存")
    .Does(() =>
{
    CleanDirectories(wwwPath + "*");
    CleanDirectories(srcPath + "**/bin");
    CleanDirectories(srcPath + "**/obj");
});


Task("restore")
    .Description("还原项目依赖")
    .Does(() =>
{
    DotNetCoreRestore(slnPath);
});


Task("build")
    .Description("编译项目")
    .IsDependentOn("clean")
    .IsDependentOn("restore")
    .Does(() =>
{
    MSBuild(slnPath, new MSBuildSettings {
        Verbosity = Verbosity.Minimal
    });
});


Task("publish")
    .Description("发布项目")
    .Does(() =>
{
    foreach(var webProject in webProjects){

        if(webProject.IsNetCore){
            DotNetCorePublish(webProject.ProjectFile, new DotNetCorePublishSettings {
                ArgumentCustomization = args=>args.Append("/p:DebugType=None"),
                Framework = webProject.Framework,
                OutputDirectory = webProject.WWWPath,
                Configuration = "Release"
            });
        }

        if(webProject.IsStatic){
            CopyDirectory(webProject.ProjectPath, webProject.WWWPath);
        }
    }
});


Task("deploy")
    .Description("部署到本机IIS")
    .Does(() =>
{

    foreach(var webProject in webProjects){

        DeleteSite(webProject.Host);

        CreateWebsite(new WebsiteSettings {
            Name = webProject.Host,
            Binding = IISBindings.Http
                                 .SetHostName(webProject.Host)
                                 .SetIpAddress("*")
                                 .SetPort(80),
            ServerAutoStart = true,
            PhysicalDirectory = webProject.WWWPath,
            ApplicationPool = webProject.ApplicationPool
        });
        
        AddHostsRecord("127.0.0.1", webProject.Host);

        StartSite(webProject.Host);
    }
});


Task("open-browser")
    .Description("用浏览器打开部署的站点")
    .Does(() =>
{
    StartPowershellScript("Start-Process", args =>
    {
        var urls = "";
        foreach(var webProject in webProjects){
            urls += ",'http://" + webProject.Host + "/'";
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
