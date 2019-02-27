using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Viber.Bot;
using Viber.Bot.Enums;
using Viber.Bot.Messages;
using Viber.Bot.Models;
using ViberBot.Factories;
using ViberBot.Models;
using ViberBot.Extensions;

namespace ViberBot.Services
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IBotFactory<IViberBotClient> viberBotFactory;

        private readonly IViberBotClient viberBotClient;

        public SendMessageService(IBotFactory<IViberBotClient> viberBotFactory)
        {
            var botId = 1;

            this.viberBotClient = viberBotFactory.GetClient(botId);
        }

        public async Task SendTextMessageAsync(int botId, string receiverId, OutMessage message)
        {
            //
            if(message.Text == null) throw new ArgumentNullException("text");

            // 
            var textMessage = new TextMessage
            {
                Receiver = receiverId,
                MinApiVersion = 4,
                Text = message.Text
            };

            await viberBotClient.SendTextMessageAsync(textMessage);
        }

        public async Task SendPictureMessageAsync(int botId, string receiverId, OutMessage message)
        {
            //
            if(message.Picture == null) throw new ArgumentNullException("picture");

            // 
            var pictureMessage = new PictureMessage
            {
                Receiver = receiverId,
                MinApiVersion = 4,
                Media = message.Picture
            };

            await viberBotClient.SendPictureMessageAsync(pictureMessage);
        }

        public async Task SendLocationMessageAsync(int botId, string receiverId, OutMessage message)
        {
            //
            if(message.Location == null) throw new ArgumentNullException("location");

            // 
            var locationMessage = new LocationMessage
            {
                Receiver = receiverId,
                MinApiVersion = 4,
                Location = new Viber.Bot.Models.Location
                {
                    Lat = message.Location.Latitude,
                    Lon = message.Location.Lontitude
                }
            };

            await viberBotClient.SendLocationMessageAsync(locationMessage);
        }

        /// <inheritdoc />
        public async Task SendRichMediaMessageAsync(int botId, string receiverId, OutMessage message)
        {
            // 
            var buttons = message.Buttons.Select(button => {
                var keyboardButton = new KeyboardButton
                {
                    Text = button.Title,
                    ActionBody = button.Id.ToString(),
                    Columns = button.Columns,
                    Rows = button.Rows
                };

                if(button.Style != null)
                {
                    keyboardButton.BackgroundColor = button.Style.BackgroundColor;
                    keyboardButton.TextHorizontalAlign = button.Style.TextHorizontalAlign.ToTextHorizontalAlign();
                    keyboardButton.TextVerticalAlign = button.Style.TextVerticalAlign.ToTextVerticalAlign();
                    keyboardButton.TextSize = button.Style.TextSize.ToTextSize();
                }

                return keyboardButton;
            });

            // 
            var carouselMessage = new CarouselMessage
            {
                Receiver = receiverId,
                MinApiVersion = 4,
                CarouselContent = new Carousel
                {
                    AlternateText = message.Text,
                    ButtonsGroupColumns = 6,
                    ButtonsGroupRows = 6,
                    Buttons = buttons.ToList()
                }
            };

            await viberBotClient.SendCarouselMessageAsync(carouselMessage);
        }

        public async Task SendKeyboardMessageAsync(int botId, string receiverId, OutMessage message)
        {
            // 
            var buttons = message.Buttons.Select(button => {
                var keyboardButton = new KeyboardButton
                {
                    Text = button.Title,
                    // ActionBody = "Helllo",
                    ActionType = KeyboardActionType.LocationPicker,
                    Columns = button.Columns,
                    Rows = button.Rows
                };

                if(button.Style != null)
                {
                    keyboardButton.BackgroundColor = button.Style.BackgroundColor;
                    keyboardButton.TextHorizontalAlign = button.Style.TextHorizontalAlign.ToTextHorizontalAlign();
                    keyboardButton.TextVerticalAlign = button.Style.TextVerticalAlign.ToTextVerticalAlign();
                    keyboardButton.TextSize = button.Style.TextSize.ToTextSize();
                }

                return keyboardButton;
            });

            // 
            var keyboardMessage = new KeyboardMessage
            {
                Receiver = receiverId,
                MinApiVersion = 4,
                Text = message.Text,
                Keyboard = new Keyboard
                {
                    Buttons = buttons.ToList()
                }
            };

            await viberBotClient.SendKeyboardMessageAsync(keyboardMessage);
        }
    }
}