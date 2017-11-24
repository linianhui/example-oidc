using System;
using System.Threading.Tasks;
using ClientSite.Oidc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSite.Controllers
{
    [Authorize]
    [Route("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("login", Name = "account-login")]
        public ChallengeResult Login(Uri returnUri)
        {
            return Challenge(BuildAuthenticationProperties(returnUri), Constants.AuthenticationSchemeOfOidc);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("qq-login", Name = "account-qq-login")]
        public ChallengeResult QQLogin(Uri returnUri)
        {
            base.HttpContext.Items["idp"] = "qq";
            return Challenge(BuildAuthenticationProperties(returnUri), Constants.AuthenticationSchemeOfOidc);
        }

        private AuthenticationProperties BuildAuthenticationProperties(Uri returnUri)
        {
            var authenticationProperties = new AuthenticationProperties();
            if (returnUri != null)
            {
                if (string.Equals(base.Request.Host.Host, returnUri.Host, StringComparison.OrdinalIgnoreCase))
                {
                    authenticationProperties.RedirectUri = returnUri.ToString();
                }
            }
            return authenticationProperties;
        }

        [HttpGet]
        [Route("logout", Name = "account-logout")]
        public IActionResult Logout()
        {
            return new SignOutResult(new[]
            {
                Constants.AuthenticationSchemeOfCookies,
                Constants.AuthenticationSchemeOfOidc
            });
        }
    }
}