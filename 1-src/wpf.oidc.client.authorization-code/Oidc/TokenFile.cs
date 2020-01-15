using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace WPFClient.Oidc
{
    public static class TokenFile
    {
        private static string TokenFilePath => Environment.CurrentDirectory + "/token.json";

        public static JObject Read()
        {
            if (File.Exists(TokenFilePath))
            {
                return JObject.Parse(File.ReadAllText(TokenFilePath));
            }

            return null;
        }

        public static void Write(string token)
        {
            File.WriteAllText(TokenFilePath, token);
        }

        public static void Delete()
        {
            if (File.Exists(TokenFilePath))
            {
                File.Delete(TokenFilePath);
            }
        }
    }
}
