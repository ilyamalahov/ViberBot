using System.Threading.Tasks;
using Viber.Bot.Messages;
using Viber.Bot.Models;

namespace ViberBot.Services
{
    public interface IBotService
    {
        Task ReceiveMessage(string senderId, string messageText);
        Task Subscribed(string userId);
        Task UnSubscribed(string userId);
        Task ConversationStarted(string userId, string userName, string userAvatarUrl);
    }
}