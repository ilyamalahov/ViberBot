using System;
using System.Net.Http;
using System.Threading.Tasks;
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
            
            return await httpClient.GetAsync(url);
        }
    }

    public interface IHttpClientService
    {
        /// <summary>
        /// Отправляет GET запрос на Web API сервис
        /// </summary>
        /// <param name="url">Адрес конечной точки (Endpoint)</param>
        /// <param name="parameters">Дополнительные параметры</param>
        /// <returns>Сообщение ответа HTTP</returns>
        Task<HttpResponseMessage> SendGetAsync(string url, object parameters = null);
    }
}