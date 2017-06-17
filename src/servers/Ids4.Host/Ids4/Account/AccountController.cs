using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Ids4.Host.Ids4.Account
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly TestUserStore _userStore;
        private readonly IIdentityServerInteractionService _idsInteraction;

        public AccountController(
            TestUserStore userStore,
            IIdentityServerInteractionService idsInteraction)
        {
            _userStore = userStore;
            _idsInteraction = idsInteraction;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string resumeUrl)
        {
            var model = new LoginViewModel(null)
            {
                ResumeUrl = resumeUrl,
                //default username and password
                UserName = "lnh",
                Password = "123"
            };
            return View(model);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginFormModel form)
        {
            if (ModelState.IsValid == false)
            {
                return View(new LoginViewModel(form));
            }

            if (_userStore.ValidateCredentials(form.UserName, form.Password))
            {

                var user = _userStore.FindByUsername(form.UserName);
                await HttpContext.Authentication.SignInAsync(user.SubjectId, user.Username);

                if (_idsInteraction.IsValidReturnUrl(form.ResumeUrl) || Url.IsLocalUrl(form.ResumeUrl))
                {
                    return Redirect(form.ResumeUrl);
                }

                return Redirect("~/");
            }
            else
            {
                ViewBag.Error = "invalid username or password.";
                return View(new LoginViewModel(form));
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _idsInteraction.GetLogoutContextAsync(logoutId);

            await HttpContext.Authentication.SignOutAsync();

            return Redirect(logout.PostLogoutRedirectUri);
        }
    }
}
