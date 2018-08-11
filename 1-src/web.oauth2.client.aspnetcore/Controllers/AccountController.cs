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
        [Route("qq-login", Name = "account-qq-login")]
        public async void QQLogin(string returnUri, bool isMobile = false)
        {
            var properties = new AuthenticationProperties();
            if (returnUri != null)
            {
                properties.RedirectUri = returnUri;
            }

            properties.Items.SetQQConnectProperties(new QQConnectProperties
            {
                IsMobile = isMobile
            });

            await HttpContext.ChallengeAsync(OAuth2Constants.AuthenticationSchemeOfQQ, properties);
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