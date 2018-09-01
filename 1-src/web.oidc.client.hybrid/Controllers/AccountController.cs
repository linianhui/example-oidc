using System;
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
        public IActionResult Login(string idp, Uri returnUri)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(idp) == false)
            {
                base.HttpContext.Items["idp"] = idp.Trim();
            }
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