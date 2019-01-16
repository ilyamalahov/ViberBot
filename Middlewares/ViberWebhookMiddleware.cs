using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Viber.Bot;
using ViberBot.Options;
using ViberBot.Repositories;

namespace ViberBot.Middlewares
{
    public class ViberWebhookMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IViberBotClient viberBotClient;
        private readonly ILogger<ViberWebhookMiddleware> logger;
        private readonly ViberBotOptions viberOptions;
        private readonly IUserRepository userRepository;

        public ViberWebhookMiddleware(
            RequestDelegate next,
            IViberBotClient viberBotClient,
            ILogger<ViberWebhookMiddleware> logger,
            IOptions<ViberBotOptions> viberConfigOptions,
            IUserRepository userRepository)
        {
            this.next = next;
            this.viberBotClient = viberBotClient;
            this.logger = logger;
            this.viberOptions = viberConfigOptions.Value;
            this.userRepository = userRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Check endpoint equality
                var absolutePath = string.Concat(context.Request.Scheme, "://", context.Request.Host, context.Request.Path);

                logger.LogInformation("Check equality of endpoint \"{absolutePath}\"", absolutePath);

                // var isEndpointValid = absolutePath.Contains(viberOptions.WebhookEndpoint);

                var isEndpointValid = true;

                if (!isEndpointValid)
                {
                    await next.Invoke(context);

                    return;
                }

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

                // Get account info
                var accountInfo = await viberBotClient.GetAccountInfoAsync();

                logger.LogDebug("Event type: {callbackData.Event}", callbackData.Event);

                switch (callbackData.Event)
                {
                    case EventType.Subscribed:
                        await OnSubscribed(callbackData);
                        break;
                    case EventType.ConversationStarted:
                        await OnConversationStarted(callbackData);
                        break;
                    case EventType.Webhook:
                        logger.LogInformation("Webhook callback");
                        break;
                    case EventType.Message:
                        await OnMessage(callbackData);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);

                throw;
            }
        }

        /// <summary>
        /// Вызывается при принятии сообщения от пользователя
        /// </summary>
        /// <param name="callbackData">Данные из запроса от Viber REST API</param>
        /// <returns></returns>
        private async Task OnMessage(CallbackData callbackData)
        {
            logger.LogInformation("Receive message from user");

            var message = callbackData.Message;

            switch (message.Type)
            {
                case MessageType.File:
                    var fileMessage = (FileMessage)message;
                    break;
                case MessageType.Text:
                    await SendTextMessage(callbackData.Sender, (TextMessage)message);
                    break;
                case MessageType.Picture:
                    // Send welcome message to user
                    logger.LogInformation("Send picture reply to user");
                    message.Receiver = callbackData.Sender.Id;

                    await viberBotClient.SendPictureMessageAsync((PictureMessage)message);
                    break;
            }
        }

        private async Task SendTextMessage(User sender, TextMessage message)
        {
            // Welcome message
            var welcomeMessage = new TextMessage
            {
                Text = "Hey! You send me a message!",
                Receiver = sender.Id
            };

            message.Receiver = sender.Id;

            // Send welcome message to user
            logger.LogInformation("Send text reply to user");

            await viberBotClient.SendTextMessageAsync(message);
        }

        /// <summary>
        /// Вызывается ...
        /// </summary>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task OnConversationStarted(CallbackData callbackData)
        {
            // Conversation started
            logger.LogInformation("Conversation started");

            // Welcome message
            var welcomeMessage = new TextMessage
            {
                Text = "Welcome!",
                Receiver = callbackData.User.Id
            };

            // Send welcome message to user
            logger.LogInformation("Send welcome message to user");

            await viberBotClient.SendTextMessageAsync(welcomeMessage);
        }

        private async Task OnSubscribed(CallbackData callbackData)
        {
            // User subscribe to channel
            logger.LogInformation("User subscribe to channel");

            var insertResult = await userRepository.Add(callbackData.Sender);

            if (!insertResult)
            {
                throw new Exception("Database insert error");
            }

            // Subscribe message
            var subscribeMessage = new TextMessage
            {
                Text = "Thanks for subscribe!",
                Receiver = callbackData.User.Id
            };

            // Send thanks message to subscribed user
            logger.LogInformation("Send thanks message to subscribed user");

            await viberBotClient.SendTextMessageAsync(subscribeMessage);
        }
    }
}