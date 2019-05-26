IList<WebSite> GetWebSiteList(String srcPath)
{
    List<WebSite> webSiteList = new List<WebSite>();

    webSiteList.Add(new WebSite {
        Host            = "oauth2-client-aspnetcore.test",
        Path            = srcPath + "web.oauth2.client.aspnetcore",
        ApplicationPool = ApplicationPoolNoCLR
    });

    webSiteList.Add(new WebSite {
        Host            = "oauth2-client-owin.test",
        Path            = srcPath + "web.oauth2.client.owin",
        ApplicationPool = ApplicationPoolCLR4
    });

    webSiteList.Add(new WebSite {
        Host            = "oauth2-resources-aspnetcore.test",
        Path            = srcPath + "web.oauth2.resources.aspnetcore",
        ApplicationPool = ApplicationPoolNoCLR
    });

    webSiteList.Add(new WebSite {
        Host            = "oauth2-resources-owin.test",
        Path            = srcPath + "web.oauth2.resources.owin",
        ApplicationPool = ApplicationPoolCLR4
    });

    webSiteList.Add(new WebSite {
        Host            = "oidc-client-hybrid.test",
        Path            = srcPath + "web.oidc.client.hybrid",
        ApplicationPool = ApplicationPoolNoCLR
    });

    webSiteList.Add(new WebSite {
        Host            = "oidc-client-implicit.test",
        Path            = srcPath + "web.oidc.client.implicit",
        ApplicationPool = ApplicationPoolCLR4
    });

    webSiteList.Add(new WebSite {
        Host            = "oidc-client-js.test",
        Path            = srcPath + "web.oidc.client.js",
        ApplicationPool = ApplicationPoolNoCLR
    });

    webSiteList.Add(new WebSite {
        Host            = "oidc-server.test",
        Path            = srcPath + "web.oidc.server.ids4",
        ApplicationPool = ApplicationPoolNoCLR
    });

    return webSiteList;
}

public class WebSite
{
    public string Host { get; set; }

    public string Path { get; set; }

    public ApplicationPoolSettings ApplicationPool { get; set; }
}

public static ApplicationPoolSettings ApplicationPoolNoCLR = new ApplicationPoolSettings
{
    Name = "oidc.example.noclr",
    IdentityType = IdentityType.LocalSystem,
    MaxProcesses = 1,
    ManagedRuntimeVersion = null
};

public static ApplicationPoolSettings ApplicationPoolCLR4 = new ApplicationPoolSettings
{
    Name = "oidc.example.clr4",
    IdentityType = IdentityType.LocalSystem,
    MaxProcesses = 1,
    ManagedRuntimeVersion = "v4.0"
};