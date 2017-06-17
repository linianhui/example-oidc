using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;

namespace Ids3.Host.Ids3
{
    public static class InMemoryUsers
    {
        public static List<InMemoryUser> All = new List<InMemoryUser>
        {
            new InMemoryUser
            {
                Username = "lnh",
                Password = "123",
                Subject = "lnh-123",
                Enabled = true,
                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.NickName, "blackheart")
                }
            }
        };
    }
}