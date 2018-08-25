using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ClientCredentials
{
    public class DefaultObserver : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            var key = value.Key;
            if (key == "System.Net.Http.Response")
            {
                var v = value.Value;
                var response = v.GetType().GetProperty("Response").GetValue(v);
                LogHttp(response as HttpResponseMessage);
            }
        }

        private void LogHttp(HttpResponseMessage httpResponseMessage)
        {
            var color = ConsoleColor.Green;
            if (httpResponseMessage.IsSuccessStatusCode == false)
            {
                color = ConsoleColor.Red;
            }

            LogLine(Environment.NewLine, color);
            LogHttpRequest(httpResponseMessage.RequestMessage, color);
            LogLine("", color);
            LogHttpResponse(httpResponseMessage, color);
            LogLine(Environment.NewLine, color);

        }

        private void LogHttpRequest(HttpRequestMessage request, ConsoleColor color)
        {
            LogLine($"{request.Method.Method} {request.RequestUri}", color);
            LogLine(request.Headers, color);
        }

        private void LogHttpResponse(HttpResponseMessage response, ConsoleColor color)
        {
            LogLine($"{(int)response.StatusCode} {response.ReasonPhrase}", color);
            LogLine(response.Headers, color);
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            if (response.Content.Headers.ContentType.MediaType.Contains("json"))
            {
                LogLine(JObject.Parse(responseContent), color);
            }
            else
            {
                LogLine(responseContent, color);
            }
        }

        private void LogLine(object value, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = oldColor;
        }
    }
}