using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace WPFClient
{
    public partial class App : Application
    {
        private static string TokenFilePath => Environment.CurrentDirectory + "/token.json";

        public static JObject ReadToken()
        {
            if (File.Exists(TokenFilePath))
            {
                return JObject.Parse(File.ReadAllText(TokenFilePath));
            }

            return null;
        }

        public static void WriteToken(string token)
        {
            File.WriteAllText(TokenFilePath, token);
        }

        public static void DeleteToken()
        {
            if (File.Exists(TokenFilePath))
            {
                File.Delete(TokenFilePath);
            }
        }
    }
}
