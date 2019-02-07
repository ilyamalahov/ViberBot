using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Viber.Bot;
using ViberBot.Repositories;

namespace ViberBot.Factories
{
    public class ViberBotFactory : IBotFactory<IViberBotClient>
    {
        private readonly IBotRepository botRepository;
        private Dictionary<int, IViberBotClient> botClients;

        public ViberBotFactory(IBotRepository botRepository)
        {
            this.botRepository = botRepository;

            botClients = new Dictionary<int, IViberBotClient>();
        }

        public IViberBotClient GetClient(int botId)
        {
            if(!botClients.ContainsKey(botId))
            {
                var addResult = AddClient(botId);

                if(!addResult)
                {
                    return null;
                }
            }

            return botClients.GetValueOrDefault(botId);
        }

        public bool AddClient(int botId)
        {
            if(botId == 0)
            {
                throw new ArgumentNullException(nameof(botId));
            }

            var bot = botRepository.GetById(botId).Result;

            if(bot == null)
            {
                return false;
            }

            lock (botClients)
            {
                return botClients.TryAdd(botId, new ViberBotClient(bot.AppKey));
            }
        }
    }

    public interface IBotFactory<T> where T : IViberBotClient
    {
        T GetClient(int botId);

        bool AddClient(int botId);
    }
}