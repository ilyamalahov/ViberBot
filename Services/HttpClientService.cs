using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using ViberBot.Extensions;

namespace ViberBot.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService(string baseUrl)
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

    public interface IHttpClientService
    {
        /// <summary>
        /// Отправляет GET запрос на Web API сервис
        /// </summary>
        /// <param name="url">Адрес конечной точки (Endpoint)</param>
        /// <param name="parametersObj">Дополнительные параметры</param>
        /// <returns>Сообщение ответа HTTP</returns>
        Task<HttpResponseMessage> SendGetAsync(string url, object parametersObj = null);

        /// <summary>
        /// Отправляет POST запрос на Web API сервис
        /// </summary>
        /// <param name="url">Адрес конечной точки (Endpoint)</param>
        /// <param name="parameters">Дополнительные параметры</param>
        /// <returns>Сообщение ответа HTTP</returns>
        Task<HttpResponseMessage> SendPostAsync(string url, object parametersObj = null);
    }
}