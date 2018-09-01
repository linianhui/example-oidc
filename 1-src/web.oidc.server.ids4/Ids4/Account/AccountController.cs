using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ServerSite.Ids4.Account
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly TestUserStore _userStore;
        private readonly IIdentityServerInteractionService _idsInteraction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public AccountController(
            TestUserStore userStore,
            IIdentityServerInteractionService idsInteraction,
            IAuthenticationSchemeProvider schemeProvider)
        {
            _userStore = userStore;
            _idsInteraction = idsInteraction;
            _schemeProvider = schemeProvider;
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string resumeUrl)
        {
            var login = await _idsInteraction.GetAuthorizationContextAsync(resumeUrl);
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
                ExternalLoginList = await GetExternalLoginViewModels(resumeUrl)
            };

            return View(model);
        }

        private async Task<List<ExternalLoginViewModel>> GetExternalLoginViewModels(string resumeUrl)
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var externals = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalLoginViewModel
                {
                    DisplayName = x.DisplayName,
                    LoginUrl = Url.Action(nameof(ExternalLogin), new
                    {
                        scheme = x.Name,
                        resumeUrl
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
                    ExternalLoginList = await GetExternalLoginViewModels(form.ResumeUrl)
                });
            }

            if (_userStore.ValidateCredentials(form.UserName, form.Password))
            {
                var user = _userStore.FindByUsername(form.UserName);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };
                await HttpContext.SignInAsync(user.SubjectId, user.Username, properties);

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
                    ExternalLoginList = await GetExternalLoginViewModels(form.ResumeUrl)
                });
            }
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout = await _idsInteraction.GetLogoutContextAsync(logoutId);

            await HttpContext.SignOutAsync();

            ViewBag.SignOutIframeUrl = logout.SignOutIFrameUrl;
            ViewBag.RedirectUri = new Uri(logout.PostLogoutRedirectUri).GetLeftPart(UriPartial.Authority);

            return View("LoggedOut");
        }


        [HttpGet]
        [Route("external-login/{scheme}", Name = "external-login")]
        public IActionResult ExternalLogin(string scheme, string resumeUrl)
        {
            resumeUrl = Url.Action(nameof(ExternalLoginCallback), new { resumeUrl });
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
            var externalLogin = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var claims = GetClaims(externalLogin);
            var userId = GetUserId(claims);
            var scheme = GetScheme(externalLogin);
            var user = _userStore.FindByExternalProvider(scheme, userId);
            if (user != null)
            {
                await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
                await HttpContext.SignInAsync(user.SubjectId, user.Username, scheme, claims.ToArray());
                return Redirect(resumeUrl);
            }

            ViewBag.NickName = GetUserNickName(claims);
            ViewBag.AvatarUrl = GetUserAvatar(claims);
            return View("ExternalLoginNewUser");
        }

        [HttpPost]
        [Route("external-login/callback")]
        public async Task<ActionResult> ExternalLoginCreateNewUser(string resumeUrl, [FromForm]NewUserViewModel viewModel)
        {
            var externalLogin = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var claims = GetClaims(externalLogin);
            var userId = GetUserId(claims);
            var scheme = GetScheme(externalLogin);

            var user = _userStore.AutoProvisionUser(scheme, userId, claims);
            user.Username = viewModel.UserName;

            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            await HttpContext.SignInAsync(user.SubjectId, user.Username, scheme, claims.ToArray());

            return Redirect(resumeUrl);
        }

        [HttpGet]
        [Route("js")]
        public ActionResult IsLogin()
        {

            var isLogin = base.User?.Identity?.IsAuthenticated ?? false;
            var javaScript = "var account={"
                           + "is_login:" + isLogin.ToString().ToLower()
                           + "};";
            return Content(javaScript, "application/javascript");
        }

        private static string GetScheme(AuthenticateResult externalLogin)
        {
            return externalLogin.Properties.Items["scheme"];
        }

        private static List<Claim> GetClaims(AuthenticateResult externalLogin)
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

        private static string GetUserNickName(ICollection<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == JwtClaimTypes.NickName)?.Value;
        }

        private static string GetUserAvatar(ICollection<Claim> claims)
        {
            return claims.FirstOrDefault(x => x.Type == "avatar")?.Value;
        }

    }
}
