using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viber.Bot;
using ViberBot.Repositories;

namespace ViberBot.Factories
{
    public class ViberBotFactory : IViberBotFactory
    {
        private readonly IViberBotRepository viberBotRepository;
        private Dictionary<int, IViberBotClient> botClients;

        public ViberBotFactory(IViberBotRepository viberBotRepository)
        {
            this.viberBotRepository = viberBotRepository;

            botClients = new Dictionary<int, IViberBotClient>();
        }

        // private async Task InitializeBotClients(IViberBotRepository viberBotRepository)
        // {
        //     var botSettings = await viberBotRepository.GetAll();

        //     foreach (var botSetting in botSettings)
        //     {
        //         botClients.Add(botSetting.Id, new ViberBotClient(botSetting.AppKey));
        //     }
        // }

        public async Task<IViberBotClient> GetClient(int botId)
        {
            if(!botClients.ContainsKey(botId))
            {
                var addResult = await AddClient(botId);

                if(!addResult)
                {
                    return null;
                }
            }

            return botClients.GetValueOrDefault(botId);
        }

        public async Task<bool> AddClient(int botId)
        {
            if(botId == 0)
            {
                throw new ArgumentNullException(nameof(botId));
            }

            var botSetting = await viberBotRepository.GetById(botId);

            if(botSetting == null)
            {
                return false;
            }

            lock (botClients)
            {
                return botClients.TryAdd(botId, new ViberBotClient(botSetting.AppKey));
            }
        }
    }

    public interface IViberBotFactory
    {
        Task<IViberBotClient> GetClient(int botId);

        Task<bool> AddClient(int botId);
    }
}