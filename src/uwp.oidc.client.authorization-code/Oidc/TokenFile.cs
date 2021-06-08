using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UWPClient.Oidc
{
    public static class TokenFile
    {
        private static StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;
        private const string FileName = "token.json";

        public static async Task<JObject> ReadAsync()
        {
            try
            {
                using (var fileStream = await LocalFolder.OpenStreamForReadAsync(FileName))
                {
                    var jsonBytes = new byte[fileStream.Length];
                    fileStream.Read(jsonBytes, 0, jsonBytes.Length);
                    var jsonText = Encoding.UTF8.GetString(jsonBytes);
                    return JObject.Parse(jsonText);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            return null;
        }

        public static async Task WriteAsync(string token)
        {
            var file = await LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenStreamForWriteAsync())
            {
                var jsonBytes = Encoding.UTF8.GetBytes(token);
                fileStream.Write(jsonBytes, 0, jsonBytes.Length);
            }
        }

        public static async Task DeleteAsync()
        {
            try
            {
                var file = await LocalFolder.GetFileAsync(FileName);
                await file.DeleteAsync();
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
