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
        private readonly IWebApiHttpService httpClientService;
        private readonly IPeopleRepository peopleRepository;
        private readonly ILogger<ViberBotService> logger;

        public ViberBotService(
            ISendMessageService sendMessageService,
            IWebApiHttpService httpClientService,
            IPeopleRepository peopleRepository,
            ILogger<ViberBotService> logger
            )
        {
            this.sendMessageService = sendMessageService;
            this.httpClientService = httpClientService;
            this.peopleRepository = peopleRepository;
            this.logger = logger;
        }

        public async Task ReceiveMessage(int botId, string senderId, string messageText)
        {
            try
            {
                if (messageText == "OK")
                {
                    await Subscribed(botId, senderId);

                    return;
                }

                // 
                var people = await peopleRepository.GetPeopleByIdAsync(senderId);

                await SendMessage(botId, people.Id, MessageType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ConversationStarted(int botId, string userId, string userName, string userAvatarUrl)
        {
            try
            {
                // 
                var people = await peopleRepository.GetOrAddPeopleAsync(userId, userName, userAvatarUrl);

                // 
                await sendMessageService.SendSubscribeMenuAsync(botId, userId);

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
                var people = await peopleRepository.GetPeopleByIdAsync(userId);

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
                var people = await peopleRepository.GetPeopleByIdAsync(userId);

                // 
                await SendChangedState(botId, people.Id, ServiceState.Unsubscribed);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ex.Message}", ex.Message);
            }
        }

        private async Task<HttpResponseMessage> SendMessage(int botId, Guid agentId, MessageType messageType)
        {
            // 
            var parameters = new
            {
                service = "Viber",
                botId,
                agentId,
                type = (int)messageType
            };

            return await httpClientService.SendGetAsync("in", parameters);
        }

        private async Task<HttpResponseMessage> SendChangedState(int botId, Guid agentId, ServiceState newState)
        {
            // 
            var parameters = new
            {
                service = "Viber",
                botId,
                agentId,
                newStateId = (int)newState
            };

            return await httpClientService.SendGetAsync("change_state", parameters);
        }
    }
}