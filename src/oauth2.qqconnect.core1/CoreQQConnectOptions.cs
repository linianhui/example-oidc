using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OAuth2.QQConnect.Core1
{
    public class CoreQQConnectOptions : OAuthOptions
    {
        public CoreQQConnectOptions()
        {
            base.AuthorizationEndpoint = "xx";
            base.TokenEndpoint = "yy";
            base.AuthenticationScheme = "qq.connect";
            base.DisplayName = "QQ Connect";
            base.CallbackPath = new PathString("/oauth2/qq-connect/callback");
            base.Scope.Add("get_user_info");
        }

        public bool IsMobile { get; set; } = false;

        internal QQConnectOptions BuildQQConnectOptions(Func<string> redirectUrl)
        {
            return new QQConnectOptions(ClientId, ClientSecret, IsMobile, Scope)
            {
                RedirectUrl = redirectUrl
            };
        }
    }
}