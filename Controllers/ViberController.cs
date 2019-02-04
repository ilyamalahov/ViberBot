using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Viber.Bot;
using Viber.Bot.Enums;
using Viber.Bot.Messages;
using Viber.Bot.Models;
using ViberBot.Services;

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class ViberController : ControllerBase
    {
        private readonly IViberBotClient viberBotClient;
        private readonly IBotService botService;

        public ViberController(
            IViberBotClient viberBotClient,
            IBotService botService)
        {
            this.viberBotClient = viberBotClient;
            this.botService = botService;
        }

        [HttpPost("{id}")]
        public async Task Index(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));

            var body = await new StreamReader(Request.Body).ReadToEndAsync();

            var signatureHeader = Request.Headers[ViberBotClient.XViberContentSignatureHeader];

            var isSignatureValid = viberBotClient.ValidateWebhookHash(signatureHeader, body);

            if (!isSignatureValid)
            {
                throw new InvalidOperationException("Invalid signature");
            }

            // Deserialize JSON callback data
            var callbackData = JsonConvert.DeserializeObject<CallbackData>(body);

            // 
            var user = callbackData.User;

            switch (callbackData.Event)
            {
                case EventType.Subscribed:
                    await botService.Subscribed(user.Id);
                    break;
                case EventType.Unsubscribed:
                    await botService.UnSubscribed(callbackData.UserId);
                    break;
                case EventType.ConversationStarted:
                    await botService.ConversationStarted(user.Id, user.Name, user.Avatar);
                    break;
                case EventType.Message:
                    await botService.ReceiveMessage(callbackData.Sender.Id, (callbackData.Message as TextMessage).Text);
                    break;
            }
        }

        [HttpGet("in")]
        public async Task ReceiveMessage()
        {
            var context = HttpContext;
        }
    }
}