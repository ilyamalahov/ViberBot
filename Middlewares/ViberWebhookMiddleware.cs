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
using ViberBot.Services;

namespace ViberBot.Middlewares
{
    public class ViberWebhookMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IViberBotClient viberBotClient;
        private readonly ILogger<ViberWebhookMiddleware> logger;
        private readonly ViberBotOptions viberOptions;

        public ViberWebhookMiddleware(
            RequestDelegate next,
            IViberBotClient viberBotClient,
            ILogger<ViberWebhookMiddleware> logger,
            IOptions<ViberBotOptions> viberConfigOptions)
        {
            this.next = next;
            this.viberBotClient = viberBotClient;
            this.logger = logger;
            this.viberOptions = viberConfigOptions.Value;

        }

        public async Task InvokeAsync(HttpContext context, IViberBotService viberBotService)
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
                    logger.LogError("Webhook endpoint is not valid");
                    
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

                switch (callbackData.Event)
                {
                    case EventType.Subscribed:
                        await viberBotService.Subscribed(callbackData.User);
                        break;
                    case EventType.Unsubscribed:
                        await viberBotService.UnSubscribed(callbackData.UserId);
                        break;
                    case EventType.ConversationStarted:
                        await viberBotService.ConversationStarted(callbackData.User);
                        break;
                    case EventType.Webhook:
                        await viberBotService.Webhook();
                        break;
                    case EventType.Message:
                        await viberBotService.ReceiveMessage(callbackData.Sender, callbackData.Message);
                        break;
                    default:
                        logger.LogDebug("Event type: {callbackData.Event}", callbackData.Event);
                        break;
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);

                throw;
            }
        }
    }
}