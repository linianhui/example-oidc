using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;

namespace Server.Ids3
{
    internal static class Users
    {
        public static List<InMemoryUser> All => new List<InMemoryUser>
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