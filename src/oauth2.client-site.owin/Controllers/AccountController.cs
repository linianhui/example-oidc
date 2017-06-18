using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using OAuth2.QQConnect;
using Owin.OAuth2.Host.OAuth2;

namespace Owin.OAuth2.Host.Controllers
{
    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("qq-login", Name = "account-qq-login")]
        public void QQLogin(Uri returnUri, string displayMode)
        {
            var authenticationProperties = new AuthenticationProperties();
            if (returnUri != null)
            {
                if (string.Equals(base.Request?.Url?.Host, returnUri.Host, StringComparison.OrdinalIgnoreCase))
                {
                    authenticationProperties.RedirectUri = returnUri.ToString();
                }
            }

            authenticationProperties = authenticationProperties.SetQQConncetSignInParams(new QQConnectSignInParams
            {
                DisplayMode = displayMode
            });

            Request.GetOwinContext()
                .Authentication
                .Challenge(authenticationProperties, OAuth2Constants.AuthenticationTypeOfQQ);
        }

        [HttpGet]
        [Route("logout", Name = "account-logout")]
        public RedirectResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(OAuth2Constants.AuthenticationTypeOfCookie);
            return Redirect("~/");
        }
    }
}