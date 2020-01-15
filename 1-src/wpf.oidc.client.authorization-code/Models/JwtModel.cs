using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace WPFClient.Models
{
    public class JwtModel
    {
        public object header { get; set; }

        public object payload { get; set; }

        public string signature { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static JwtModel From(string jwtString)
        {
            var jwt = new JwtSecurityToken(jwtString);
            return new JwtModel
            {
                header = jwt.Header,
                payload = jwt.Payload,
                signature = jwt.RawSignature
            };
        }
    }

}
