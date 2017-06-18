using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Ids3.Host.Ids3
{
    internal static class Certificates
    {
        public static X509Certificate2 SigningCertificate
        {
            get
            {
                var resourceName = typeof(Certificates).Namespace + ".ids3-test.pfx";
                byte[] certificateBytes;
                using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    var length = (int)resourceStream.Length;
                    certificateBytes = new byte[length];
                    resourceStream.Read(certificateBytes, 0, length);
                }
                return new X509Certificate2(certificateBytes, "password-test");
            }
        }
    }
}