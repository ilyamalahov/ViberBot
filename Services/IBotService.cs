using System.Threading.Tasks;
using Viber.Bot.Messages;
using Viber.Bot.Models;

namespace ViberBot.Services
{
    public interface IBotService
    {
        Task ReceiveMessage(int botId, string senderId, string messageText);
        Task Subscribed(int botId, string userId);
        Task UnSubscribed(int botId, string userId);
        Task ConversationStarted(int botId, string userId, string userName, string userAvatarUrl);
    }
}