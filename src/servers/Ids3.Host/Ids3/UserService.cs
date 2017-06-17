using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;

namespace Ids3.Host.Ids3
{
    public class UserService : UserServiceBase
    {
        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            var externalIdentity = context.ExternalIdentity;
            var user = InMemoryUsers.All.SingleOrDefault(u => u.Provider == externalIdentity.Provider && u.ProviderId == externalIdentity.ProviderId);
            if (user == null)
            {
                context.AuthenticateResult = new AuthenticateResult($"~/external-login/{externalIdentity.Provider}", externalIdentity);
            }
            else
            {
                context.AuthenticateResult = new AuthenticateResult(user.Subject, user.Username, claims: externalIdentity.Claims, identityProvider: user.Provider);
            }

            return Task.FromResult(0);
        }

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = InMemoryUsers.All.SingleOrDefault(u => u.Username == context.UserName && u.Password == context.Password);

            if (user != null)
            {
                context.AuthenticateResult = new AuthenticateResult(user.Subject, user.Username);
            }

            return Task.FromResult(0);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims = new List<Claim>(context.Subject.Claims);

            return Task.FromResult(0);
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            if (context.Subject == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var user = InMemoryUsers.All.SingleOrDefault(u => u.Subject == context.Subject.GetSubjectId());
            if (user != null && user.Enabled == true)
            {
                context.IsActive = true;
            }

            return Task.FromResult(0);
        }
    }
}