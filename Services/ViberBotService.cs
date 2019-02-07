using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Viber.Bot;
using Viber.Bot.Enums;
using Viber.Bot.Messages;
using Viber.Bot.Models;
using ViberBot.Models;
using ViberBot.Repositories;
using ViberBot.Services.Http;

namespace ViberBot.Services
{
    public class ViberBotService : IBotService
    {
        private readonly ISendMessageService sendMessageService;
        private readonly IViberApiHttpService viberApiHttpService;
        private readonly IPeopleRepository peopleRepository;
        private readonly ILogger<ViberBotService> logger;

        public ViberBotService(
            ISendMessageService sendMessageService,
            IViberApiHttpService viberApiHttpService,
            IPeopleRepository peopleRepository,
            ILogger<ViberBotService> logger
            )
        {
            this.sendMessageService = sendMessageService;
            this.viberApiHttpService = viberApiHttpService;
            this.peopleRepository = peopleRepository;
            this.logger = logger;
        }

        public async Task ReceiveMessage(int botId, string senderId, MessageBase message)
        {
            // if (messageText == "OK")
            // {
            //     await Subscribed(botId, senderId);

            //     return;
            // }

            // 
            var people = await peopleRepository.GetPeopleByViberIdAsync(senderId);

            var inMessage = new InMessage();

            switch (message.Type)
            {
                case Viber.Bot.Enums.MessageType.Picture:
                    inMessage.Picture = (message as PictureMessage).Media;
                    break;
                case Viber.Bot.Enums.MessageType.Video:
                    inMessage.Video = (message as VideoMessage).Media;
                    break;
                case Viber.Bot.Enums.MessageType.Text:
                    inMessage.Text = (message as TextMessage).Text;
                    break;
            }

            // 
            var payload = new MessageModel<InMessage>
            {
                BotId = botId,
                AgentId = people.Id,
                Message = inMessage
            };

            // 
            await viberApiHttpService.SendPostAsync("msgbot/in", payload);
        }

        public async Task ConversationStarted(int botId, string userId, string userName, string userAvatarUrl)
        {
            try
            {
                // 
                var people = await peopleRepository.GetOrAddPeopleAsync(userId, userName, userAvatarUrl);

                // 
                // await sendMessageService.SendSubscribeMenuAsync(botId, userId);

                // 
                await SendChangedState(botId, people.Id, ServiceState.ConversationStarted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        public async Task Subscribed(int botId, string userId)
        {
            try
            {
                //
                await peopleRepository.UpdateContactServiceStateAsync(userId, ServiceState.Subscribed);

                // 
                var people = await peopleRepository.GetPeopleByViberIdAsync(userId);

                // 
                await SendChangedState(botId, people.Id, ServiceState.Subscribed);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        public async Task UnSubscribed(int botId, string userId)
        {
            try
            {
                //
                await peopleRepository.UpdateContactServiceStateAsync(userId, ServiceState.Unsubscribed);

                // 
                var people = await peopleRepository.GetPeopleByViberIdAsync(userId);

                // 
                await SendChangedState(botId, people.Id, ServiceState.Unsubscribed);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        private async Task<HttpResponseMessage> SendChangedState(int botId, Guid agentId, ServiceState newState)
        {
            // 
            var parameters = new
            {
                botId,
                agentId,
                stateId = (int)newState
            };

            return await viberApiHttpService.SendGetAsync("msgbot/change_state", parameters);
        }
    }
}