using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Http.Diagnostic
{
    public class HttpDefaultObserver : IObserver<KeyValuePair<String, Object>>
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
            LogLine(new String('<', 32), color);
            LogLine(Environment.NewLine, color);
            LogHttpRequest(httpResponseMessage.RequestMessage, color);
            LogLine(Environment.NewLine, color);
            LogHttpResponse(httpResponseMessage, color);
            LogLine(Environment.NewLine, color);
            LogLine(new String('>', 32), color);
            LogLine(Environment.NewLine, color);
        }

        private void LogHttpRequest(HttpRequestMessage request, ConsoleColor color)
        {
            LogLine($"{request.Method.Method} {request.RequestUri}", color);
            LogLine(request.Headers, color);
            LogHttpContent(request.Content, color);
        }

        private void LogHttpResponse(HttpResponseMessage response, ConsoleColor color)
        {
            LogLine($"{(Int32)response.StatusCode} {response.ReasonPhrase}", color);
            LogLine(response.Headers, color);
            LogHttpContent(response.Content, color);
        }

        private void LogHttpContent(HttpContent httpContent, ConsoleColor color)
        {
            if (httpContent == null)
            {
                return;
            }
            var responseContent = httpContent.ReadAsStringAsync().GetAwaiter().GetResult();
            if (httpContent.Headers.ContentType.MediaType.Contains("json"))
            {
                LogLine(JToken.Parse(responseContent), color);
            }
            else
            {
                LogLine(responseContent, color);
            }
        }

        private void LogLine(Object value, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = oldColor;
        }
    }
}