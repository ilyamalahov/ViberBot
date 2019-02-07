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

namespace ViberBot.Services.Bot
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
            InMessage inMessage = null;

            // 
            var people = await peopleRepository.GetPeopleByViberIdAsync(senderId);

            switch (message.Type)
            {
                case Viber.Bot.Enums.MessageType.Picture:
                    inMessage = CreateInPictureMessage((PictureMessage)message);
                    break;
                case Viber.Bot.Enums.MessageType.Video:
                    inMessage = CreateInVideoMessage((VideoMessage)message);
                    break;
                case Viber.Bot.Enums.MessageType.Text:
                    inMessage = CreateInTextMessage((TextMessage)message);
                    break;
            }

            // 
            var model = new MessageModel<InMessage>
            {
                BotId = botId,
                AgentId = people.Id,
                Message = inMessage
            };

            // 
            await viberApiHttpService.SendPostAsync("msgbot/in", model);
        }

        private InMessage CreateInTextMessage(TextMessage message)
        {
            return new InMessage
            {
                Text = message.Text
            };
        }

        private InMessage CreateInVideoMessage(VideoMessage message)
        {
            return new InMessage
            {
                Video = message.Media
            };
        }

        private InMessage CreateInPictureMessage(PictureMessage message)
        {
            return new InMessage
            {
                Picture = message.Media
            };
        }

        public async Task ConversationStarted(int botId, string userId, string userName, string userAvatarUrl)
        {
            try
            {
                // 
                var people = await peopleRepository.GetOrAddPeopleAsync(userId, userName, userAvatarUrl);

                var outMessage = new OutMessage
                {
                    Text = "Нажмите \"OK\", чтобы продолжить",
                    ButtonPlace = PlaceType.Window,
                    Buttons = new Button[]
                    {
                        new Button
                        {
                            Columns = 6,
                            Rows = 2,
                            Title = "OK",
                            Id = 1
                        }
                    }
                };

                await viberApiHttpService.SendPostAsync("msgbot/out", outMessage);

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