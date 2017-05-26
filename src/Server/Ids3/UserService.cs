using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OAuth2.QQConnect;

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

        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            var externalIdentity = context.ExternalIdentity;

            var qqConncetProfile = new QQConncetProfile(externalIdentity.Claims);

            var user = Users.SingleOrDefault(u => u.Provider == externalIdentity.Provider && u.ProviderId == externalIdentity.ProviderId);

            //默认用户名
            string nickName = "new user";
            //bool isNewUser = false;
            //新用户
            if (user == null)
            {
                var nickNameClaimValue = externalIdentity.Claims.FirstOrDefault(x => x.Type == Constants.ClaimTypes.NickName)?.Value;
                if (string.IsNullOrWhiteSpace(nickNameClaimValue) == false)
                {
                    nickName = nickNameClaimValue;
                }

                user = new InMemoryUser
                {
                    Subject = Guid.NewGuid().ToString(),
                    Provider = externalIdentity.Provider,
                    ProviderId = externalIdentity.ProviderId,
                    Claims = new List<Claim>
                    {
                        new Claim(Constants.ClaimTypes.NickName, nickName)
                    }
                };
                Users.Add(user);
                //isNewUser = true;
            }

            nickName = user.Claims.First(x => x.Type == Constants.ClaimTypes.NickName).Value;

            //if (isNewUser)
            //{
            //    context.AuthenticateResult = new AuthenticateResult("~/account/bind-new-user", user.Subject, nickName, identityProvider: user.Provider);
            //}
            //else
            //{
            context.AuthenticateResult = new AuthenticateResult(user.Subject, nickName, claims: externalIdentity.Claims, identityProvider: user.Provider);
            //}

            return Task.FromResult(0);
        }

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = Users.SingleOrDefault(u => u.Username == context.UserName && u.Password == context.Password);

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

            var user = Users.SingleOrDefault(u => u.Subject == context.Subject.GetSubjectId());
            if (user != null && user.Enabled == true)
            {
                context.IsActive = true;
            }

            return Task.FromResult(0);
        }
    }
}