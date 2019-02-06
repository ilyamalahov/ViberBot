using System;
using System.Threading.Tasks;
using ViberBot.Factories;
using ViberBot.Repositories;
using ViberBot.Models;
using System.Collections.Generic;
using Viber.Bot.Models;
using Viber.Bot.Messages;
using Viber.Bot.Enums;
using System.Linq;
using System.Collections;

namespace ViberBot.Services
{
    public class MessageService : IMessageService
    {
        private readonly ISendMessageService sendMessageService;
        private readonly IPeopleRepository peopleRepository;
        private readonly IViberBotFactory viberBotFactory;

        public MessageService(
            ISendMessageService sendMessageService,
            IPeopleRepository peopleRepository,
            IViberBotFactory viberBotFactory)
        {
            this.sendMessageService = sendMessageService;
            this.peopleRepository = peopleRepository;
            this.viberBotFactory = viberBotFactory;
        }

        public async Task SendCarouselMessage(int botId, Guid agentId, Message message)
        {
            var viberBotClient = await viberBotFactory.GetClient(botId);

            var contact = await peopleRepository.GetContactByPeopleId(agentId);

            var buttons = message.Buttons.Select(button => new KeyboardButton
            {
                Text = button.Title,
                ActionBody = button.Id.ToString(),
                Columns = button.Columns,
                Rows = button.Rows,
                BackgroundColor = button.Style?.BackgroundColor,
                TextHorizontalAlign = (TextHorizontalAlign)button.Style?.TextHorizontalAlign,
                TextVerticalAlign = (TextVerticalAlign)button.Style?.TextVerticalAlign,
                TextSize = (TextSize)button.Style?.TextSize
            });

            var carouselMessage = new CarouselMessage
            {
                Receiver = contact.InfoTextId,
                MinApiVersion = 4,
                CarouselContent = new Carousel
                {
                    AlternateText = message.Text,
                    Buttons = buttons.ToArray()
                }
            };

            await viberBotClient.SendCarouselMessageAsync(carouselMessage);
        }

        public Task SendKeyboardMessage(int botId, Guid agentId, Message message)
        {
            throw new NotImplementedException();
        }

        public Task SendMessage(int botId, Guid agentId, Message message)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMessageService
    {
        Task SendCarouselMessage(int botId, Guid agentId, Message message);
        Task SendKeyboardMessage(int botId, Guid agentId, Message message);
        Task SendMessage(int botId, Guid agentId, Message message);
    }
}