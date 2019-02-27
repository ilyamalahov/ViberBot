using System.Threading.Tasks;
using ViberBot.Models;

namespace ViberBot.Services
{
    public interface ISendMessageService
    {
        /// <summary>
        /// Sends text message to user
        /// </summary>
        /// <param name="botId">Bot identifier</param>
        /// <param name="receiverId">Receiver identifier</param>
        /// <param name="message">Unified message</param>
        Task SendTextMessageAsync(int botId, string receiverId, OutMessage message);
        
        /// <summary>
        /// Sends picture message to user
        /// </summary>
        /// <param name="botId">Bot identifier</param>
        /// <param name="receiverId">Receiver identifier</param>
        /// <param name="message">Unified message</param>
        Task SendPictureMessageAsync(int botId, string receiverId, OutMessage message);
        
        /// <summary>
        /// Sends location message to user
        /// </summary>
        /// <param name="botId">Bot identifier</param>
        /// <param name="receiverId">Receiver identifier</param>
        /// <param name="message">Unified message</param>
        Task SendLocationMessageAsync(int botId, string receiverId, OutMessage message);
        
        /// <summary>
        /// Sends rich media message to user
        /// </summary>
        /// <param name="botId">Bot identifier</param>
        /// <param name="receiverId">Receiver identifier</param>
        /// <param name="message">Unified message</param>
        Task SendRichMediaMessageAsync(int botId, string receiverId, OutMessage message);
        
        /// <summary>
        /// Sends keyboard message to user
        /// </summary>
        /// <param name="botId">Bot identifier</param>
        /// <param name="receiverId">Receiver identifier</param>
        /// <param name="message">Unified message</param>
        Task SendKeyboardMessageAsync(int botId, string receiverId, OutMessage message);
    }
}