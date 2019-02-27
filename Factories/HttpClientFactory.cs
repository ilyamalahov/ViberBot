using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viber.Bot;
using ViberBot.Repositories;

namespace ViberBot.Factories
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Dictionary<string, HttpClient> httpClients;

        public HttpClientFactory(IBotRepository botRepository)
        {
            httpClients = new Dictionary<string, HttpClient>();
        }

        public HttpClient Get(string url)
        {
            if(url == null)
            {
                throw new ArgumentNullException("url");
            }

            if(!httpClients.ContainsKey(url) && !AddClient(url))
            {
                return null;
            }

            if(!httpClients.TryGetValue(url, out var client))
            {
                throw new Exception($"Http client for {url} not found");
            }

            return client;
        }

        private bool AddClient(string url)
        {
            if(url == null)
            {
                throw new ArgumentNullException("url");
            }

            if(httpClients.ContainsKey(url))
            {
                throw new Exception($"Http client for {url} already exists");
            }

            lock (httpClients)
            {
                return httpClients.TryAdd(url, new HttpClient() { BaseAddress = new Uri(url) });
            }
        }
    }

    public interface IHttpClientFactory
    {
        HttpClient Get(string url);
    }
}