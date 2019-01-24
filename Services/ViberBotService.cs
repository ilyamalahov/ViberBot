using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Viber.Bot;
using ViberBot.Repositories;

namespace ViberBot.Services
{
    public class ViberBotService : IBotService
    {
        private readonly IViberBotClient viberBotClient;
        private readonly ISendMessageService sendMessageService;
        private readonly IUserStateMachineService userStateMachineService;
        private readonly ILogger<ViberBotService> logger;

        public ViberBotService(
            IViberBotClient viberBotClient,
            ISendMessageService sendMessageService,
            IUserStateMachineService userStateMachineService,
            ILogger<ViberBotService> logger
            )
        {
            this.viberBotClient = viberBotClient;
            this.sendMessageService = sendMessageService;
            this.userStateMachineService = userStateMachineService;
            this.logger = logger;
        }

        public async Task ReceiveMessage(User sender, MessageBase message)
        {
            logger.LogInformation("Receive message from user \"{sender.Id}\"", sender.Id);

            var stateMachine = userStateMachineService.Get(sender.Id);

            await stateMachine.ProcessFlow(sender.Id, (message as TextMessage).Text);
        }

        public async Task ConversationStarted(string userId)
        {
            try
            {
                // Conversation started
                logger.LogInformation("User \"{sender.Id}\" started conversation", userId);

                // 
                var stateMachine = userStateMachineService.Add(userId);

                // 
                await stateMachine.Start(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        public async Task Subscribed(User user)
        {
            try
            {
                // User subscribe to channel
                logger.LogInformation("User \"{user.Id}\" subscribed to channel", user.Id);

                // 
                var stateMachine = userStateMachineService.Add(user.Id);

                // 
                await stateMachine.Start(user.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        public async Task UnSubscribed(string userId)
        {

            try
            {
                // User subscribe to channel
                logger.LogInformation("User \"{userId}\" unsubscribed from channel", userId);

                // 
                userStateMachineService.Delete(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        // /// <summary>
        // /// Загружает файл/изображение из Viber на сервер
        // /// </summary>
        // /// <param name="url">Web-адрес</param>
        // /// <param name="fileName">Имя файла</param>
        // /// <param name="userId">Уникальный идентификатор пользователя Viber</param>
        // /// <returns>Результат загрузки</returns>
        // private bool DownloadFile(string url, string fileName, string userId)
        // {
        //     // Create a user directory if it does not exists
        //     logger.LogInformation("Create a user directory if it does not exists");

        //     var userFolder = Path.Combine("assets", "users", userId);

        //     Directory.CreateDirectory(userFolder);

        //     // Asset path
        //     var assetPath = Path.Combine(userFolder, fileName);

        //     if (File.Exists(assetPath))
        //     {
        //         logger.LogError("File \"{fileName}\" already exists", fileName);

        //         return false;
        //     }

        //     // Download file
        //     logger.LogInformation("Process file download");

        //     using (var client = new WebClient())
        //     {
        //         client.DownloadFileAsync(new Uri(url), assetPath);
        //     }

        //     return true;
        // }

        public async Task HandleRequest(HttpContext context)
        {
            // Validate viber signature
            logger.LogInformation("Validate viber content signature");

            var body = new StreamReader(context.Request.Body).ReadToEnd();

            var signatureHeader = context.Request.Headers[ViberBotClient.XViberContentSignatureHeader];

            var isSignatureValid = viberBotClient.ValidateWebhookHash(signatureHeader, body);

            if (!isSignatureValid)
            {
                throw new InvalidOperationException("Invalid signature");
            }

            // Signature is valid
            logger.LogInformation("Signature valid");

            // Deserialize JSON callback data
            var callbackData = JsonConvert.DeserializeObject<CallbackData>(body);

            switch (callbackData.Event)
            {
                case EventType.Subscribed:
                    await Subscribed(callbackData.User);
                    break;
                case EventType.Unsubscribed:
                    await UnSubscribed(callbackData.UserId);
                    break;
                case EventType.ConversationStarted:
                    await ConversationStarted(callbackData.User.Id);
                    break;
                case EventType.Message:
                    await ReceiveMessage(callbackData.Sender, callbackData.Message);
                    break;
            }
        }
    }
}