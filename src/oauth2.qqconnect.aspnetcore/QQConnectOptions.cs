using System;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using BasicQQConnectOptions = OAuth2.QQConnect.Basic.QQConnectOptions;

namespace OAuth2.QQConnect.AspNetCore
{
    public class QQConnectOptions : OAuthOptions
    {
        public QQConnectOptions()
        {
            base.AuthorizationEndpoint = "xx";
            base.TokenEndpoint = "yy";
            base.CallbackPath = new PathString("/oauth2/qq-connect/callback");
            base.Scope.Add("get_user_info");
        }

        public bool IsMobile { get; set; } = false;

        internal BasicQQConnectOptions BuildQQConnectOptions(Func<string> redirectUrl)
        {
            return new BasicQQConnectOptions(ClientId, ClientSecret, IsMobile, Scope)
            {
                RedirectUrl = redirectUrl
            };
        }
    }
}