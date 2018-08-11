#addin nuget:?package=cake.json&version=3.0.1
#addin nuget:?package=newtonsoft.json&version=9.0.1

IList<WebProject> GetWebProjects(String srcPath, String noCLRWWWPath)
{

    List<WebProject> webProjects = new List<WebProject>();

    var webJsonFiles = GetFiles(srcPath + "**/web.json");
    foreach (var webJsonFile in webJsonFiles)
    {
        var webProject = DeserializeJsonFromFile<WebProject>(webJsonFile);
        webProject.NoCLRWWWPath = noCLRWWWPath;
        webProject.ProjectPath = System.IO.Path.GetDirectoryName(webJsonFile.FullPath);
        webProjects.Add(webProject);
    }

    return webProjects;
}

public class WebProject
{
    public static ApplicationPoolSettings ApplicationPoolNoCLR = new ApplicationPoolSettings
    {
        Name = "oidc-example.noclr",
        IdentityType = IdentityType.LocalSystem,
        MaxProcesses = 1,
        ManagedRuntimeVersion = null
    };

    public static ApplicationPoolSettings ApplicationPoolCLR4 = new ApplicationPoolSettings
    {
        Name = "oidc-example.clr4",
        IdentityType = IdentityType.LocalSystem,
        MaxProcesses = 1,
        ManagedRuntimeVersion = "v4.0"
    };

    public string NoCLRWWWPath { get; set; }

    public string Host { get; set; }

    public string Framework { get; set; }

    public string ProjectPath { get; set; }

    public bool IsNetCore
    {
        get { return Framework.Contains("netcore"); }
    }

    public bool IsStatic
    {
        get { return Framework.Contains("static"); }
    }

    public bool IsNoCLR
    {
        get { return IsNetCore || IsStatic; }
    }

    public string ProjectFile
    {
        get { return System.IO.Directory.GetFiles(ProjectPath, "*.csproj").FirstOrDefault(); }
    }

    public string WWWPath
    {
        get
        {
            if (IsNoCLR)
            {
                return NoCLRWWWPath + Host;
            }
            return ProjectPath;
        }
    }

    public ApplicationPoolSettings ApplicationPool
    {
        get
        {
            if (IsNoCLR)
            {
                return ApplicationPoolNoCLR;
            }
            return ApplicationPoolCLR4;
        }
    }
}