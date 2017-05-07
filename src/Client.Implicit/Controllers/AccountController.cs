using System;
using System.Web;
using System.Web.Mvc;
using Client.Implicit.Oidc;
using Microsoft.Owin.Security;

namespace Client.Implicit.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("login", Name = "account-login")]
        public void Login(Uri returnUri)
        {
            var authenticationProperties = new AuthenticationProperties();
            if (returnUri != null)
            {
                if (string.Equals(base.Request.Url.Host, returnUri.Host, StringComparison.OrdinalIgnoreCase))
                {
                    //用来设置在登录成功后重定向的地址,由OidcMiddleware来维护。不设置的话默认是当前的request.url。
                    authenticationProperties.RedirectUri = returnUri.ToString();
                }
            }
            Request.GetOwinContext().Authentication.Challenge(authenticationProperties, Constants.AuthenticationTypeOfOidc);
        }
    }
}