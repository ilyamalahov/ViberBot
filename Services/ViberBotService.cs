using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Viber.Bot;
using ViberBot.Repositories;

namespace ViberBot.Services
{
    public class ViberBotService : IViberBotService
    {
        private readonly IUserRepository userRepository;
        private readonly IViberBotClient viberBotClient;
        private readonly ILogger<ViberBotService> logger;

        public ViberBotService(
            IUserRepository userRepository,
            IViberBotClient viberBotClient,
            ILogger<ViberBotService> logger
            )
        {
            this.userRepository = userRepository;
            this.viberBotClient = viberBotClient;
            this.logger = logger;
        }

        /// <summary>
        /// Вызывается при принятии сообщения от пользователя
        /// </summary>
        /// <param name="callbackData">Данные из запроса от Viber REST API</param>
        /// <returns></returns>
        public async Task ReceiveMessage(User sender, MessageBase message)
        {
            logger.LogInformation("Receive message from user");

            switch (message.Type)
            {
                case MessageType.Text:
                    await HandleTextMessage(sender, (TextMessage)message);
                    break;
                case MessageType.Picture:
                    logger.LogInformation("Send picture reply to user");

                    await HandlePictureMessage(sender, (PictureMessage)message);
                    break;
                case MessageType.File:
                    logger.LogInformation("Send file reply to user");

                    message.Receiver = sender.Id;

                    await viberBotClient.SendFileMessageAsync((FileMessage)message);
                    break;
            }
        }

        public async Task ConversationStarted(User sender)
        {
            // Conversation started
            logger.LogInformation("User {sender.Id} started conversation", sender.Id);

            // Welcome message
            var welcomeMessage = new TextMessage
            {
                Text = "Welcome!",
                Receiver = sender.Id
            };

            // Send welcome message to user
            logger.LogInformation("Send welcome reply to user");

            await viberBotClient.SendTextMessageAsync(welcomeMessage);
        }

        public async Task Subscribed(User user)
        {
            // User subscribe to channel
            logger.LogInformation("User {user.Id} subscribed to channel", user.Id);

            // var insertResult = await userRepository.Add(user);
            var insertResult = true;

            if (!insertResult)
            {
                throw new Exception("Database insert error");
            }

            // Subscribe message
            var subscribeMessage = new TextMessage
            {
                Text = "Thank you for subscribe!",
                Receiver = user.Id
            };

            // Send respected reply to user
            logger.LogInformation("Send respected reply to user");

            await viberBotClient.SendTextMessageAsync(subscribeMessage);
        }

        public async Task UnSubscribed(string userId)
        {
            // User subscribe to channel
            logger.LogInformation("User {userId} unsubscribed to channel", userId);
        }

        public async Task Webhook()
        {
            logger.LogInformation("Webhook callback");
        }



        private async Task HandleTextMessage(User sender, TextMessage message)
        {
            // Welcome message
            var replyMessage = new TextMessage
            {
                Text = $"Hey! You send me a message!\n\nYour reply: \"{message.Text}\"",
                Receiver = sender.Id
            };

            // Send welcome message to user
            logger.LogInformation("Send text reply to user #{sender.Id}", sender.Id);

            await viberBotClient.SendTextMessageAsync(replyMessage);
        }

        private async Task HandlePictureMessage(User sender, PictureMessage message)
        {
            message.Receiver = sender.Id;

            await viberBotClient.SendPictureMessageAsync(message);
        }
    }

    public interface IViberBotService
    {
        Task ReceiveMessage(User sender, MessageBase message);
        Task ConversationStarted(User user);
        Task Subscribed(User user);
        Task UnSubscribed(string userId);
        Task Webhook();
    }
}