using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Viber.Bot.Enums;
using ViberBot.Models;

namespace ViberBot.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResult> SendPostAsync<T, TResult>(this HttpClient client, string url, T parameters = default(T), string mediaType = "application/json", HttpHeaders headers = null)
        {
            // 
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            // Create request
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            // Parameters
            if (parameters != null)
            {
                MediaTypeFormatter formatter = null;

                switch (mediaType)
                {
                    case "application/json":
                        formatter = new JsonMediaTypeFormatter();
                        break;
                    case "application/xml":
                        formatter = new XmlMediaTypeFormatter();
                        break;
                }

                request.Content = new ObjectContent<T>(parameters, formatter, mediaType);
            }

            // Headers

            // Send request and receive response
            var response = await client.SendAsync(request);

            // Read response content
            return await response.Content.ReadAsAsync<TResult>();
        }
    }
}