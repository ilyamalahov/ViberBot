using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Viber.Bot;
using Viber.Bot.Enums;
using Viber.Bot.Messages;
using Viber.Bot.Models;
using ViberBot.Services;

namespace ViberBot.Middlewares
{
    public class ViberBotMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IViberBotClient viberBotClient;
        private readonly IBotService viberBotService;
        private readonly ILogger<ViberBotMiddleware> logger;

        public ViberBotMiddleware(
            RequestDelegate next,
            IViberBotClient viberBotClient,
            IBotService viberBotService,
            ILogger<ViberBotMiddleware> logger)
        {
            this.next = next;
            this.viberBotClient = viberBotClient;
            this.viberBotService = viberBotService;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // try
            // {
            //     var body = await new StreamReader(context.Request.Body).ReadToEndAsync();

            //     var signatureHeader = context.Request.Headers[ViberBotClient.XViberContentSignatureHeader];

            //     var isSignatureValid = viberBotClient.ValidateWebhookHash(signatureHeader, body);

            //     if (!isSignatureValid)
            //     {
            //         throw new InvalidOperationException("Invalid signature");
            //     }

            //     // Deserialize JSON callback data
            //     var callbackData = JsonConvert.DeserializeObject<CallbackData>(body);

            //     // 
            //     var user = callbackData.User;

            //     switch (callbackData.Event)
            //     {
            //         case EventType.Subscribed:
            //             await viberBotService.Subscribed(0, user.Id);
            //             break;
            //         case EventType.Unsubscribed:
            //             await viberBotService.UnSubscribed(0, callbackData.UserId);
            //             break;
            //         case EventType.ConversationStarted:
            //             await viberBotService.ConversationStarted(0, user.Id, user.Name, user.Avatar);
            //             break;
            //         case EventType.Message:
            //             await viberBotService.ReceiveMessage(0, callbackData.Sender.Id, (callbackData.Message as TextMessage).Text);
            //             break;
            //     }
            // }
            // catch (Exception ex)
            // {
            //     logger.LogError(ex, "Error: {ex.Message}", ex.Message);

            //     await next.Invoke(context);
            // }
        }
    }
}