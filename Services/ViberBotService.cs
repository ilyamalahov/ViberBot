using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Viber.Bot;
using ViberBot.Enums;
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
        /// Вызывается, когда пользователь отправляет сообщение боту
        /// </summary>
        /// <param name="sender">Пользователь Viber</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Асинхронная задача</returns>
        public async Task ReceiveMessage(User sender, MessageBase message)
        {
            logger.LogInformation("Receive message from user \"{sender.Id}\"", sender.Id);

            switch (message.Type)
            {
                case MessageType.Text:
                    await HandleTextMessage(sender, (TextMessage)message);
                    break;
                case MessageType.Picture:
                    await HandlePictureMessage(sender, (PictureMessage)message);
                    break;
                case MessageType.File:
                    await HandleFileMessage(sender, (FileMessage)message);
                    break;
                case MessageType.Url:
                    await HandleUrlMessage(sender, (UrlMessage)message);
                    break;
                case MessageType.Location:
                    await HandleLocationMessage(sender, (LocationMessage)message);
                    break;
                case MessageType.Contact:
                    await HandleContactMessage(sender, (ContactMessage)message);
                    break;
                default:
                    logger.LogError("The messages for this type \"{message.Type}\" is not processed", message.Type);
                    break;
            }
        }

        /// <summary>
        /// Вызывается, когда пользователь начал общение с ботом, но еще не подписан на него
        /// </summary>
        /// <param name="user">Пользователь Viber</param>
        /// <returns>Асинхронная задача</returns>
        public async Task ConversationStarted(User user)
        {
            // Conversation started
            logger.LogInformation("User \"{sender.Id}\" started conversation", user.Id);

            // Send keyboard to user
            logger.LogInformation("Send keyboard to user");

            var keyboardMessage = new KeyboardMessage
            {
                Receiver = user.Id,
                Text = "Выберите нужный элемент из меню",
                Keyboard = new Keyboard
                {
                    Buttons = new[]
                    {
                        new KeyboardButton
                        {
                            Columns = 2,
                            Rows = 1,
                            Text = "<b><font color='#fff'>Отправить локацию</font></b>",
                            TextHorizontalAlign = TextHorizontalAlign.Center,
                            TextVerticalAlign = TextVerticalAlign.Middle,
                            ActionType = KeyboardActionType.Reply,
                            ActionBody = KeyboardReply.SendLocationBefore.ToString(),
                            BackgroundColor = "#b20000"
                        },
                        new KeyboardButton
                        {
                            Columns = 2,
                            Rows = 1,
                            Text = "<b><font color='#fff'>Прикрепить изображение</font></b>",
                            TextHorizontalAlign = TextHorizontalAlign.Center,
                            TextVerticalAlign = TextVerticalAlign.Middle,
                            ActionType = KeyboardActionType.Reply,
                            ActionBody = KeyboardReply.AttachPicture.ToString(),
                            BackgroundColor = "#0053b2"
                        },
                        new KeyboardButton
                        {
                            Columns = 2,
                            Rows = 1,
                            Text = "<b><font color='#fff'>Отправить сообщение</font></b>",
                            TextHorizontalAlign = TextHorizontalAlign.Center,
                            TextVerticalAlign = TextVerticalAlign.Middle,
                            ActionType = KeyboardActionType.Reply,
                            ActionBody = KeyboardReply.SendMessage.ToString(),
                            BackgroundColor = "#00c621"
                        },
                        new KeyboardButton
                        {
                            Columns = 2,
                            Rows = 1,
                            Text = "<b><font color='#fff'>Другое действие</font></b>",
                            TextHorizontalAlign = TextHorizontalAlign.Center,
                            TextVerticalAlign = TextVerticalAlign.Middle,
                            ActionType = KeyboardActionType.Reply,
                            ActionBody = KeyboardReply.OtherAction.ToString(),
                            BackgroundColor = "#d3db00"
                        }
                    }
                }
            };

            // Process keyboard message send
            await viberBotClient.SendKeyboardMessageAsync(keyboardMessage);
        }

        /// <summary>
        /// Вызывается, когда пользователь подписывается на бота
        /// </summary>
        /// <param name="user">Пользователь Viber</param>
        /// <returns>Асинхронная задача</returns>
        public async Task Subscribed(User user)
        {
            // User subscribe to channel
            logger.LogInformation("User \"{user.Id}\" subscribed to channel", user.Id);

            // var databaseUser = new ViberBot.Entities.User { UserId = user.Id };
            // var insertResult = await userRepository.Add(databaseUser);

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

        /// <summary>
        /// Вызывается, когда пользователь отписывается от бота
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя Viber</param>
        /// <returns>Асинхронная задача</returns>
        public async Task UnSubscribed(string userId)
        {
            // User subscribe to channel
            logger.LogInformation("User \"{userId}\" unsubscribed from channel", userId);

            // Delete a user directory
            var userFolder = Path.Combine("assets", "users", userId);

            if(Directory.Exists(userFolder))
            {
                Directory.Delete(userFolder, true);
            }

            // var deleteResult = await userRepository.Delete(userId);
            var deleteResult = true;

            if (!deleteResult)
            {
                throw new Exception("Database delete error");
            }
        }

        /// <summary>
        /// Вызывается, когда устанавливается конечная точка Webhook
        /// </summary>
        /// <returns>Асинхронная задача</returns>
        public async Task Webhook()
        {
            logger.LogInformation("Webhook callback");
        }


        /// <summary>
        /// Обрабатывает текстовое сообщение от пользователя
        /// </summary>
        /// <param name="user">Пользователь Viber</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Асинхронная задача</returns>
        private async Task HandleTextMessage(User user, TextMessage message)
        {
            var keyboardReply = Enum.Parse<KeyboardReply>(message.Text);
            
            var isKeyboardReplyDefined = Enum.IsDefined(typeof(KeyboardReply), keyboardReply);

            if (isKeyboardReplyDefined)
            {
                switch (keyboardReply)
                {
                    case KeyboardReply.SendLocationBefore:
                        logger.LogInformation("Button action: Send location before");
                        break;
                    case KeyboardReply.AttachPicture:
                        logger.LogInformation("Button action: Attach picture");
                        break;
                    case KeyboardReply.SendLocationAfter:
                        logger.LogInformation("Button action: Send location after");
                        break;
                    case KeyboardReply.SendMessage:
                        logger.LogInformation("Button action: Send message");
                        break;
                    case KeyboardReply.OtherAction:
                        logger.LogInformation("Button action: Other action");
                        break;
                }
            }
            else
            {
                // Reply message
                var replyMessage = new TextMessage
                {
                    Text = $"Вы отправили мне сообщение!\n\nВаш ответ: \"{message.Text}\"",
                    Receiver = user.Id
                };

                // Send welcome message to user
                logger.LogInformation("Send text reply to user", user.Id);

                await viberBotClient.SendTextMessageAsync(replyMessage);
            }
        }

        /// <summary>
        /// Обрабатывает сообщение-изображение от пользователя
        /// </summary>
        /// <param name="user">Пользователь Viber</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Асинхронная задача</returns>
        private async Task HandlePictureMessage(User user, PictureMessage message)
        {
            // Download picture
            logger.LogInformation("Download picture \"{message.FileName}\" from Viber server", message.FileName);

            var downloadResult = DownloadFile(message.Media, message.FileName, user.Id);

            // Send reply to user
            logger.LogInformation("Send reply to user");

            var replyMessage = new TextMessage
            {
                Text = "Я получил Ваше изображение, спасибо!",
                Receiver = user.Id
            };

            // Process message send
            await viberBotClient.SendTextMessageAsync(replyMessage);
        }

        /// <summary>
        /// Обрабатывает сообщение-файл от пользователя
        /// </summary>
        /// <param name="user">Пользователь Viber</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Асинхронная задача</returns>
        private async Task HandleFileMessage(User user, FileMessage message)
        {
            // Download file
            logger.LogInformation("Download file \"{message.FileName}\" from Viber server", message.FileName);

            var downloadResult = DownloadFile(message.Media, message.FileName, user.Id);

            // Send reply to user
            logger.LogInformation("Send reply to user");

            var replyMessage = new TextMessage
            {
                Text = "Я получил Ваш файл, спасибо!",
                Receiver = user.Id
            };

            // Process message send
            await viberBotClient.SendTextMessageAsync(replyMessage);
        }

        private Task HandleLocationMessage(User sender, LocationMessage message)
        {
            throw new NotImplementedException();
        }

        private Task HandleUrlMessage(User sender, UrlMessage message)
        {
            throw new NotImplementedException();
        }

        private Task HandleContactMessage(User sender, ContactMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загружает файл/изображение из Viber на сервер
        /// </summary>
        /// <param name="url">Web-адрес</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="userId">Уникальный идентификатор пользователя Viber</param>
        /// <returns>Результат загрузки</returns>
        private bool DownloadFile(string url, string fileName, string userId)
        {
            // Create a user directory if it does not exists
            logger.LogInformation("Create a user directory if it does not exists");

            var userFolder = Path.Combine("assets", "users", userId);

            Directory.CreateDirectory(userFolder);

            // Asset path
            var assetPath = Path.Combine(userFolder, fileName);

            if (File.Exists(assetPath))
            {
                logger.LogError("File \"{fileName}\" already exists", fileName);

                return false;
            }

            // Download file
            logger.LogInformation("Process file download");

            using (var client = new WebClient())
            {
                client.DownloadFileAsync(new Uri(url), assetPath);
            }

            return true;
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