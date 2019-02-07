using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using ViberBot.Extensions;

namespace ViberBot.Services.Http
{
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
        Task<HttpResponseMessage> SendPostAsync<T>(string url, T parametersObj = default(T), string mediaType = "application/xml");
    }
}