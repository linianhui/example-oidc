using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace ServerSite.Ids4
{
    public static class Users
    {
        public static List<TestUser> All = new List<TestUser>
        {
            new TestUser
            {
                Username = "lnh",
                Password = "123",
                SubjectId = "000001",
                Claims = new[]
                {
                    new Claim(JwtClaimTypes.NickName, "blackheart")
                }
            }
        };
    }
}