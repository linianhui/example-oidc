using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Services.InMemory;
using OAuth2.QQConnect;
using ServerSite.Ids3;

namespace ServerSite.Controllers
{
    [RouteArea(Ids3Constants.ServerPath)]
    [RoutePrefix("external-login")]
    public class ExternalLoginController : Controller
    {
        [HttpGet]
        [Route(Ids3Constants.QQIdp)]
        public async Task<ActionResult> QQLogin()
        {
            var owinContext = Request.GetOwinContext();
            var identity = await owinContext.Environment.GetIdentityServerPartialLoginAsync();
            if (identity == null || identity.IsAuthenticated == false)
            {
                return new HttpUnauthorizedResult();
            }

            var qqConnectProfile = new QQConncetProfile(identity.Claims);

            return View(new NewUserViewModel
            {
                UserName = qqConnectProfile.NickName,
                AvatarUrl = qqConnectProfile.Avatar
            });
        }

        [HttpPost]
        [Route(Ids3Constants.QQIdp)]
        public async Task<ActionResult> QQLogin(NewUserViewModel viewModel)
        {
            var owinContext = Request.GetOwinContext();
            var identity = await owinContext.Environment.GetIdentityServerPartialLoginAsync();
            if (identity == null || identity.IsAuthenticated == false)
            {
                return new HttpUnauthorizedResult();
            }

            var qqConnectProfile = new QQConncetProfile(identity.Claims);

            var newUser = new InMemoryUser
            {
                Username = viewModel.UserName ?? "Default Name",
                Subject = Guid.NewGuid().ToString(),
                Enabled = true,
                Provider = qqConnectProfile.Issuer,
                ProviderId = qqConnectProfile.OpenId,
                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.NickName, qqConnectProfile.NickName),
                    new Claim(Constants.ClaimTypes.Picture, qqConnectProfile.Avatar),
                }
            };

            Users.All.Add(newUser);

            var resumeUrl = await owinContext.Environment.GetPartialLoginResumeUrlAsync();

            return Redirect(resumeUrl);
        }

        public class NewUserViewModel
        {
            public string UserName { get; set; }

            public string AvatarUrl { get; set; }
        }
    }
}