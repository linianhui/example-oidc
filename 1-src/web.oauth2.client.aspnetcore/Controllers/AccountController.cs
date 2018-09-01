using System;
using System.Threading.Tasks;
using ClientSite.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2.QQConnect;
using OAuth2.QQConnect.Extensions;

namespace ClientSite.Controllers
{
    [Authorize]
    [Route("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("login", Name = "account-login")]
        public async void Login(string idp, string returnUri, bool isMobile = false)
        {
            var properties = new AuthenticationProperties();
            if (returnUri != null)
            {
                properties.RedirectUri = returnUri;
            }

            if (OAuth2Constants.AuthenticationSchemeOfQQ.Equals(idp, StringComparison.OrdinalIgnoreCase))
            {
                properties.Items.SetQQConnectProperties(new QQConnectProperties
                {
                    IsMobile = isMobile
                });
            }

            await HttpContext.ChallengeAsync(idp, properties);
        }

        [HttpGet]
        [Route("logout", Name = "account-logout")]
        public async Task<RedirectResult> Logout()
        {
            await HttpContext.SignOutAsync(OAuth2Constants.AuthenticationSchemeOfCookie);
            return Redirect("~/");
        }
    }
}