using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
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
        public async Task<IActionResult> Login(string resumeUrl)
        {
            var login = await this._idsInteraction.GetAuthorizationContextAsync(resumeUrl);
            if (string.IsNullOrWhiteSpace(login.IdP) == false)
            {
                return ExternalLogin(login.IdP, resumeUrl);
            }

            var model = new LoginViewModel(null)
            {
                ResumeUrl = resumeUrl,
                //default username and password
                UserName = "lnh",
                Password = "123",
                ExternalLoginList = GetExternalLoginViewModels(resumeUrl)
            };

            return View(model);
        }

        private List<ExternalLoginViewModel> GetExternalLoginViewModels(string resumeUrl)
        {
            var schemes = base.HttpContext.Authentication.GetAuthenticationSchemes();

            var externals = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalLoginViewModel
                {
                    DisplayName = x.DisplayName,
                    LoginUrl = Url.Action(nameof(ExternalLogin), new
                    {
                        scheme = x.AuthenticationScheme,
                        resumeUrl = resumeUrl
                    })
                }).ToList();
            return externals;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginFormModel form)
        {
            if (ModelState.IsValid == false)
            {
                return View(new LoginViewModel(form)
                {
                    ExternalLoginList = GetExternalLoginViewModels(form.ResumeUrl)
                });
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
                return View(new LoginViewModel(form)
                {
                    ExternalLoginList = GetExternalLoginViewModels(form.ResumeUrl)
                });
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


        [HttpGet]
        [Route("external-login/{scheme}", Name = "external-login")]
        public IActionResult ExternalLogin(string scheme, string resumeUrl)
        {
            resumeUrl = Url.Action(nameof(ExternalLoginCallback), new { resumeUrl = resumeUrl });
            var props = new AuthenticationProperties
            {
                RedirectUri = resumeUrl,
                Items = { { "scheme", scheme } }
            };
            return new ChallengeResult(scheme, props);
        }

        [HttpGet]
        [Route("external-login/callback")]
        public async Task<IActionResult> ExternalLoginCallback(string resumeUrl)
        {
            var externalLogin = await HttpContext.Authentication.GetAuthenticateInfoAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var claims = GetClaims(externalLogin);
            var userId = GetUserId(claims);
            var scheme = GetScheme(externalLogin);
            var user = _userStore.FindByExternalProvider(scheme, userId);
            if (user != null)
            {
                await HttpContext.Authentication.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
                await HttpContext.Authentication.SignInAsync(user.SubjectId, user.Username, scheme, claims.ToArray());
                return Redirect(resumeUrl);
            }
            return View("ExternalLoginNewUser");
        }

        [HttpPost]
        [Route("external-login/callback")]
        public async Task<ActionResult> ExternalLoginCreateNewUser(string resumeUrl, [FromForm]NewUserViewModel viewModel)
        {
            var externalLogin = await HttpContext.Authentication.GetAuthenticateInfoAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var claims = GetClaims(externalLogin);
            var userId = GetUserId(claims);
            var scheme = GetScheme(externalLogin);

            var user = _userStore.AutoProvisionUser(scheme, userId, claims);
            user.Username = viewModel.UserName;

            await HttpContext.Authentication.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            await HttpContext.Authentication.SignInAsync(user.SubjectId, user.Username, scheme, claims.ToArray());

            return Redirect(resumeUrl);
        }
        private static string GetScheme(AuthenticateInfo externalLogin)
        {
            return externalLogin.Properties.Items["scheme"];
        }

        private static List<Claim> GetClaims(AuthenticateInfo externalLogin)
        {
            var tempUser = externalLogin?.Principal;
            if (tempUser == null)
            {
                throw new Exception("External authentication error");
            }

            return tempUser.Claims.ToList();
        }

        private static string GetUserId(ICollection<Claim> claims)
        {
            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            }
            if (userIdClaim == null)
            {
                throw new Exception("Unknown userid");
            }
            return userIdClaim.Value;
        }

    }
}
