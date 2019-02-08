using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Viber.Bot;
using Viber.Bot.Enums;
using Viber.Bot.Messages;
using Viber.Bot.Models;
using ViberBot.Factories;
using ViberBot.Repositories;
using ViberBot.Services;
using ViberBot.Services.Bot;

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class ViberController : ControllerBase
    {
        private readonly IBotFactory<IViberBotClient> viberBotFactory;
        private readonly IBotService botService;
        private readonly ILogger<ViberController> logger;

        public ViberController(
            IBotFactory<IViberBotClient> viberBotFactory,
            IBotService botService,
            ILogger<ViberController> logger)
        {
            this.viberBotFactory = viberBotFactory;
            this.botService = botService;
            this.logger = logger;
        }

        [HttpPost("{botId:int}")]
        public async Task<IActionResult> Index(int botId)
        {
            try
            {
                var viberBotClient = viberBotFactory.GetClient(botId);

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
                    case EventType.ConversationStarted:
                        await botService.ConversationStarted(botId, user.Id, user.Name, user.Avatar);
                        break;
                    case EventType.Subscribed:
                        await botService.Subscribed(botId, user.Id);
                        break;
                    case EventType.Unsubscribed:
                        await botService.UnSubscribed(botId, callbackData.UserId);
                        break;
                    case EventType.Message:
                        await botService.ReceiveMessage(botId, callbackData.Sender.Id, callbackData.MessageToken, callbackData.Message);
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
                
                return BadRequest(ex.Message);
            }
        }
    }
}