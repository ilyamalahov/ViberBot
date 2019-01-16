using System.Threading.Tasks;
using Viber.Bot;

namespace ViberBot.Services
{
    public class ViberBotService : IViberBotService
    {
        public Task ConversationStarted(CallbackData callbackData)
        {
            throw new System.NotImplementedException();
        }

        public Task ReceiveMessage(CallbackData callbackData)
        {
            throw new System.NotImplementedException();
        }

        public Task Subscribed(CallbackData callbackData)
        {
            throw new System.NotImplementedException();
        }

        public Task UnSubscribed(CallbackData callbackData)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IViberBotService
    {
        Task ReceiveMessage(CallbackData callbackData);
        Task ConversationStarted(CallbackData callbackData);
        Task Subscribed(CallbackData callbackData);
        Task UnSubscribed(CallbackData callbackData);
    }
}