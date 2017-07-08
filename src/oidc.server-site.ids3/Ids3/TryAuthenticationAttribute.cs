using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace ServerSite.Ids3
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TryAuthenticationAttribute : FilterAttribute, IAuthorizationFilter
    {
        public string AuthenticationType { get; }

        public TryAuthenticationAttribute(string authenticationType)
        {
            this.AuthenticationType = authenticationType;
        }

        public async void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            var authenticateResult = await GetAuthenticateResult(httpContext);
            if (authenticateResult != null)
            {
                httpContext.User = new ClaimsPrincipal(authenticateResult.Identity);
            }
        }

        private async Task<AuthenticateResult> GetAuthenticateResult(HttpContextBase httpContext)
        {
            var owinContext = httpContext.GetOwinContext();

            if (owinContext != null)
            {
                return await owinContext.Authentication.AuthenticateAsync(this.AuthenticationType);
            }

            return null;
        }
    }
}