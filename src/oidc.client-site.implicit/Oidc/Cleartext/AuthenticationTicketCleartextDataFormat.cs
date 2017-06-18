using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace ClientSite.Oidc.Cleartext
{
    /// <summary>
    /// 方便调试，明文。
    /// </summary>
    public class AuthenticationTicketCleartextDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        public string Protect(AuthenticationTicket data)
        {
            return JsonConvert.SerializeObject(new Data
            {
                Claims = data.Identity.Claims.Select(claim => new KeyValuePair<string, string>(claim.Type, claim.Value)).ToList(),
                Properties = data.Properties.Dictionary
            });
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            var data = JsonConvert.DeserializeObject<Data>(protectedText);
            var claims = data.Claims.Select(keyValue => new Claim(keyValue.Key, keyValue.Value)).ToList();

            return new AuthenticationTicket(
                new ClaimsIdentity(claims),
                new AuthenticationProperties(data.Properties)
            );
        }

        public class Data
        {
            public List<KeyValuePair<string, string>> Claims { get; set; }

            public IDictionary<string, string> Properties { get; set; }
        }
    }
}