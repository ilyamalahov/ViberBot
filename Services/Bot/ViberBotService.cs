using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
        private readonly IWebApiHttpService webApiHttpService;
        private readonly IPeopleRepository peopleRepository;
        private readonly ILogger<ViberBotService> logger;

        public ViberBotService(
            ISendMessageService sendMessageService,
            IWebApiHttpService webApiHttpService,
            IPeopleRepository peopleRepository,
            ILogger<ViberBotService> logger
            )
        {
            this.sendMessageService = sendMessageService;
            this.webApiHttpService = webApiHttpService;
            this.peopleRepository = peopleRepository;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task ConversationStarted(int botId, string userId, string userName, string userAvatarUrl)
        {
            // 
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

            await sendMessageService.SendKeyboardMessageAsync(botId, userId, outMessage);

            // 
            var people = await peopleRepository.GetOrAddPeopleAsync(userId, userName, userAvatarUrl);

            // 
            await SendChangedState(botId, people.Id, ServiceState.ConversationStarted);
        }

        /// <inheritdoc />
        public async Task Subscribed(int botId, string userId)
        {
            //
            await peopleRepository.UpdateContactServiceStateAsync(userId, ServiceState.Subscribed);

            // 
            var people = await peopleRepository.GetPeopleByViberIdAsync(userId);

            // 
            await SendChangedState(botId, people.Id, ServiceState.Subscribed);
        }

        /// <inheritdoc />
        public async Task UnSubscribed(int botId, string userId)
        {
            //
            await peopleRepository.UpdateContactServiceStateAsync(userId, ServiceState.Unsubscribed);

            // 
            var people = await peopleRepository.GetPeopleByViberIdAsync(userId);

            // 
            await SendChangedState(botId, people.Id, ServiceState.Unsubscribed);
        }

        /// <inheritdoc />
        public async Task ReceiveMessage(int botId, string senderId, long messageToken, MessageBase message)
        {
            InMessage inMessage = null;

            switch (message.Type)
            {
                case Viber.Bot.Enums.MessageType.Picture:
                    inMessage = CreatePictureMessage((PictureMessage)message);
                    break;
                case Viber.Bot.Enums.MessageType.Video:
                    inMessage = CreateVideoMessage((VideoMessage)message);
                    break;
                case Viber.Bot.Enums.MessageType.Text:
                    var textMessage = (TextMessage)message;

                    if (Enum.TryParse<ButtonType>(textMessage.Text, out var buttonType))
                    {
                        if (buttonType == ButtonType.OK)
                        {
                            await Subscribed(botId, senderId);

                            return;
                        }

                        inMessage = CreateButtonMessage(buttonType, messageToken);
                    }
                    else
                    {
                        inMessage = CreateTextMessage(textMessage);
                    }
                    break;
            }

            // 
            var people = await peopleRepository.GetPeopleByViberIdAsync(senderId);

            // 
            var model = new MessageModel<InMessage>
            {
                BotId = botId,
                AgentId = people.Id,
                Message = inMessage
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

                await httpClient.PostAsync("bot/change_state", model, new XmlMediaTypeFormatter());
            }
        }

        /// <summary>
        /// Create new unified text message from viber text message
        /// </summary>
        /// <param name="message">Viber text message</param>
        /// <returns>Unified message</returns>
        private InMessage CreateButtonMessage(ButtonType buttonContent, long messageToken)
        {
            return new InMessage
            {
                MessageToken = messageToken,
                ButtonId = (int)buttonContent
            };
        }

        /// <summary>
        /// Create new unified text message from viber text message
        /// </summary>
        /// <param name="message">Viber text message</param>
        /// <returns>Unified message</returns>
        private InMessage CreateTextMessage(TextMessage message)
        {
            return new InMessage
            {
                Text = message.Text
            };
        }

        /// <summary>
        /// Create new unified video message from viber video message
        /// </summary>
        /// <param name="message">Viber video message</param>
        /// <returns>Unified message</returns>
        private InMessage CreateVideoMessage(VideoMessage message)
        {
            return new InMessage
            {
                Video = message.Media
            };
        }

        /// <summary>
        /// Create new unified picture message from viber picture message
        /// </summary>
        /// <param name="message">Viber picture message</param>
        /// <returns>Unified message</returns>
        private InMessage CreatePictureMessage(PictureMessage message)
        {
            return new InMessage
            {
                Picture = message.Media
            };
        }

        /// <summary>
        /// Sends new contact service state to WebApi service
        /// </summary>
        /// <param name="botId">Bot identifier</param>
        /// <param name="agentId">People (agent) identifier</param>
        /// <param name="state">New service state</param>
        /// <returns>Http response</returns>
        private async Task SendChangedState(int botId, Guid agentId, ServiceState state)
        {
            var model = new MessageModel<ServiceState>
            {
                BotId = botId,
                AgentId = agentId,
                Message = state
            };

            // 
            // var parameters = new
            // {
            //     botId,
            //     agentId,
            //     state
            // };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

                await httpClient.PostAsync("bot/change_state", model, new XmlMediaTypeFormatter());
            }
        }
    }

    public enum ButtonType
    {
        OK = 1
    }
}