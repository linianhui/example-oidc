using System;
using System.Web;
using System.Web.Mvc;
using ClientSite.OAuth2;
using Microsoft.Owin.Security;
using OAuth2.QQConnect;
using OAuth2.QQConnect.Extensions;

namespace ClientSite.Controllers
{
    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("qq-login", Name = "account-qq-login")]
        public void QQLogin(Uri returnUri, bool isMobile = false)
        {
            var authenticationProperties = new AuthenticationProperties();
            if (returnUri != null)
            {
                if (string.Equals(base.Request?.Url?.Host, returnUri.Host, StringComparison.OrdinalIgnoreCase))
                {
                    authenticationProperties.RedirectUri = returnUri.ToString();
                }
            }

            authenticationProperties.Dictionary.SetQQConnectProperties(new QQConnectProperties
            {
                IsMobile = isMobile
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