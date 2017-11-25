using System.Collections.Generic;
using IdentityServer4.Models;

namespace ServerSite.Ids4
{
    public static class Resources
    {
        public static IEnumerable<IdentityResource> AllIdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

        public static IEnumerable<ApiResource> AllApiResources => new List<ApiResource>
        {
            new ApiResource("my-api", "My API")
        };
    }
}
