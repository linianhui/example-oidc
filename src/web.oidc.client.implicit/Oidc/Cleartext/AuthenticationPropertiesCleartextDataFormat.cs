using System.Collections.Generic;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace ClientSite.Oidc.Cleartext
{
    /// <summary>
    /// 方便调试，明文。
    /// </summary>
    public class AuthenticationPropertiesCleartextDataFormat : ISecureDataFormat<AuthenticationProperties>
    {
        public string Protect(AuthenticationProperties data)
        {
            return JsonConvert.SerializeObject(data.Dictionary);
        }

        public AuthenticationProperties Unprotect(string protectedText)
        {
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(protectedText);
            return new AuthenticationProperties(properties);
        }
    }
}