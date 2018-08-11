using System;
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
        public ActionResult Login(Uri returnUri)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Redirect("/");
            }

            Request.GetOwinContext()
                .Authentication
                .Challenge(BuildAuthenticationProperties(returnUri), Constants.AuthenticationTypeOfOidc);

            return new EmptyResult();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login-qq", Name = "account-login-qq")]
        public ActionResult LoginQQ(Uri returnUri)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Redirect("/");
            }

            Request.GetOwinContext()
                .Set("idp", "qq")
                .Authentication
                .Challenge(BuildAuthenticationProperties(returnUri), Constants.AuthenticationTypeOfOidc);

            return new EmptyResult();
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
    }
}