using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using ViberBot.Extensions;

namespace ViberBot.Services.Http
{
    public class ViberApiHttpService : IViberApiHttpService
    {
        private readonly HttpClient httpClient;

        public ViberApiHttpService(string baseUrl)
        {
            var httpClientHandler = new HttpClientHandler() { UseDefaultCredentials = true };

            httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(baseUrl) };
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> SendGetAsync(string url, object parametersObj = null)
        {
            if(parametersObj != null)
            {
                var serializedParams = parametersObj.ToQueryString();

                url = string.Concat(url, "?", serializedParams);
            }
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            return await SendAsync(request);
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> SendPostAsync(string url, object parametersObj = null)
        {
            var jsonParameters = JsonConvert.SerializeObject(parametersObj);
            
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(jsonParameters, Encoding.UTF8, "application/json");

            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await httpClient.SendAsync(request);
        }
    }

    public interface IViberApiHttpService : IHttpClientService
    {
    }
}