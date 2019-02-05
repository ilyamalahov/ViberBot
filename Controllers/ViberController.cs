using System;
using System.IO;
using System.Linq;
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

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class ViberController : ControllerBase
    {
        private readonly IViberBotFactory viberBotFactory;
        private readonly IBotService botService;
        private readonly ISendMessageService sendMessageService;
        private readonly IPeopleRepository peopleRepository;
        private readonly IViberBotRepository viberBotRepository;
        private readonly ILogger<ViberController> logger;

        public ViberController(
            IViberBotFactory viberBotFactory,
            IBotService botService,
            ISendMessageService sendMessageService,
            IPeopleRepository peopleRepository,
            IViberBotRepository viberBotRepository,
            ILogger<ViberController> logger)
        {
            this.viberBotFactory = viberBotFactory;
            this.botService = botService;
            this.sendMessageService = sendMessageService;
            this.peopleRepository = peopleRepository;
            this.viberBotRepository = viberBotRepository;
            this.logger = logger;
        }

        [HttpPost("{botId:int}")]
        public async Task Index(int botId)
        {
            try
            {
                var viberBotClient = await viberBotFactory.GetClient(botId);

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
                        await botService.Subscribed(botId, user.Id);
                        break;
                    case EventType.Unsubscribed:
                        await botService.UnSubscribed(botId, callbackData.UserId);
                        break;
                    case EventType.ConversationStarted:
                        await botService.ConversationStarted(botId, user.Id, user.Name, user.Avatar);
                        break;
                    case EventType.Message:
                        await botService.ReceiveMessage(botId, callbackData.Sender.Id, (callbackData.Message as TextMessage).Text);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        [HttpGet("start")]
        public async Task Start(int botId, Guid agentId)
        {
            //
            var contact = await peopleRepository.GetContactById(agentId);

            //
            var botSetting = await viberBotRepository.GetById(botId);

            // 
            var textAttribute = botSetting.InvitationMessage.Attributes().First(attr => attr.Name == "text");

            // 
            var imageAttribute = botSetting.InvitationMessage.Attributes().First(attr => attr.Name == "img");

            // 
            await sendMessageService.SendStartedMenuAsync(botId, contact.InfoTextId, textAttribute.Value, imageAttribute.Value);
        }
    }
}