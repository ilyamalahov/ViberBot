using System;
using System.Net.Http;
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

        public Task<HttpResponseMessage> SendPostAsync(string url, object parametersObj = null)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IWebApiHttpService : IHttpClientService
    {
    }
}