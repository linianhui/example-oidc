using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace WPFClient.Models
{
    public class JwtModel
    {
        public object header { get; set; }

        public object payload { get; set; }

        public string signature { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
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
