using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace Ids4.Host.Ids4
{
    public static class Users
    {
        public static List<TestUser> All = new List<TestUser>
        {
            new TestUser
            {
                Username = "lnh",
                Password = "123",
                SubjectId = "lnh-123",
                Claims = new[]
                {
                    new Claim(JwtClaimTypes.NickName, "blackheart")
                }
            }
        };
    }
}