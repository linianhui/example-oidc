using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace ClientSite.Oidc
{

    [RoutePrefix("oidc")]
    public class OidcController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("front-channel-logout-callback")]
        public ActionResult FrontChannelLogoutCallback(string sid)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var sessionId = claimsPrincipal?.FindFirst(Constants.ClaimTypes.SessionId)?.Value;
            if (sessionId != null && sessionId == sid)
            {
                Request.GetOwinContext().Authentication.SignOut(Constants.AuthenticationTypeOfCookies);
            }

            return Content(this.Request.Url?.DnsSafeHost + "退出成功。", "text/html");
        }
    }
}