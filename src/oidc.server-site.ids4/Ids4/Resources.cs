using System.Collections.Generic;
using IdentityServer4.Models;

namespace Ids4.Host.Ids4
{
    public static class Resources
    {
        public static IEnumerable<IdentityResource> AllIdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };
    }
}
