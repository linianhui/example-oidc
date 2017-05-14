using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;

namespace Server.Ids3
{

    public class UserService : UserServiceBase
    {
        private static List<InMemoryUser> Users => new List<InMemoryUser>
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

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = Users.SingleOrDefault(u => u.Username == context.UserName && u.Password == context.Password);

            if (user != null)
            {
                context.AuthenticateResult = new AuthenticateResult(user.Subject, user.Username);
            }

            return Task.FromResult(0);
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            if (context.Subject == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var user = Users.SingleOrDefault(u => u.Subject == context.Subject.GetSubjectId());
            if (user != null && user.Enabled == true)
            {
                context.IsActive = true;
            }

            return Task.FromResult(0);
        }
    }
}