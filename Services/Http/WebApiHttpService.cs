using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using ViberBot.Extensions;

namespace ViberBot.Services.Http
{
    public class WebApiHttpService : IWebApiHttpService
    {
        private readonly HttpClient httpClient;

        public WebApiHttpService(string baseUrl)
        {
            var httpClientHandler = new HttpClientHandler() { UseDefaultCredentials = true };

            httpClient = HttpClientFactory.Create(httpClientHandler);

            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public Task<HttpResponseMessage> SendGetAsync(string url, object parametersObj = null)
        {
            if(parametersObj != null)
            {
                var serializedParams = parametersObj.ToQueryString();

                url = string.Concat(url, "?", serializedParams);
            }
            
            return httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> SendPostAsync<T>(string url, T parametersObj = default(T), string mediaType = "application/xml")
        {
            MediaTypeFormatter formatter = null;

            switch (mediaType)
            {
                case "application/xml":
                    formatter = new XmlMediaTypeFormatter();
                    break;
                case "application/json":
                    formatter = new JsonMediaTypeFormatter();
                    break;
            }

            return await httpClient.PostAsync(url, parametersObj, formatter);
        }
    }

    public interface IWebApiHttpService : IHttpClientService
    {
    }
}