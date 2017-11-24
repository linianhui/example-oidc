using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ClientSite.Oidc;
using Microsoft.Owin.Security;

namespace ClientSite.Controllers
{
    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("login", Name = "account-login")]
        public void Login(Uri returnUri)
        {
            Request.GetOwinContext()
                .Authentication
                .Challenge(BuildAuthenticationProperties(returnUri), Constants.AuthenticationTypeOfOidc);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("qq-login", Name = "account-qq-login")]
        public void QQLogin(Uri returnUri)
        {
            Request.GetOwinContext()
                .Set("idp", "qq")
                .Authentication
                .Challenge(BuildAuthenticationProperties(returnUri), Constants.AuthenticationTypeOfOidc);
        }

        private AuthenticationProperties BuildAuthenticationProperties(Uri returnUri)
        {
            var authenticationProperties = new AuthenticationProperties();
            if (returnUri != null)
            {
                if (string.Equals(base.Request?.Url?.Host, returnUri.Host, StringComparison.OrdinalIgnoreCase))
                {
                    //用来设置在登录成功后重定向的地址,由OidcMiddleware来维护。不设置的话默认是当前的request.url。
                    authenticationProperties.RedirectUri = returnUri.ToString();
                }
            }
            return authenticationProperties;
        }

        [HttpGet]
        [Route("logout", Name = "account-logout")]
        public void Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("logout-callback")]
        public void LogoutCallback(string sid)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var sessionId = claimsPrincipal?.FindFirst(Constants.ClaimTypes.SessionId)?.Value;
            if (sessionId != null && sessionId == sid)
            {
                Request.GetOwinContext().Authentication.SignOut(Constants.AuthenticationTypeOfCookies);
            }
        }
    }
}