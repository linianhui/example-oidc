using System;
using System.Threading.Tasks;
using Core.OAuth2.Host.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using OAuth2.QQConnect;

namespace Core.OAuth2.Host.Controllers
{
    [Authorize]
    [Route("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("qq-login", Name = "account-qq-login")]
        public async void QQLogin(string returnUri, string displayMode)
        {
            var authenticationProperties = new AuthenticationProperties();
            if (returnUri != null)
            {
                authenticationProperties.RedirectUri = returnUri;
            }

            authenticationProperties = authenticationProperties.SetQQConncetSignInParams(new QQConnectSignInParams
            {
                DisplayMode = displayMode
            });

            await this.HttpContext
                 .Authentication
                 .ChallengeAsync(OAuth2Constants.AuthenticationSchemeOfQQ, authenticationProperties);
        }

        [HttpGet]
        [Route("logout", Name = "account-logout")]
        public async Task<RedirectResult> Logout()
        {
            await this.HttpContext.Authentication.SignOutAsync(OAuth2Constants.AuthenticationSchemeOfCookie);
            return Redirect("~/");
        }
    }
}