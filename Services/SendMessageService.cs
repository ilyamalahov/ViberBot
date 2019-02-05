using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Viber.Bot;
using Viber.Bot.Enums;
using Viber.Bot.Messages;
using Viber.Bot.Models;
using ViberBot.Factories;

namespace ViberBot.Services
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IViberBotFactory viberBotFactory;

        // private readonly IViberBotClient viberBotClient;

        public SendMessageService(IViberBotFactory viberBotFactory)
        {
            // this.viberBotClient = viberBotClient;
            this.viberBotFactory = viberBotFactory;
        }

        public async Task SendSubscribeMenuAsync(int botid, string receiverId)
        {
            var messageText = "Нажмите кнопку \"OK\", чтобы продолжить";

            // Keyboard buttons
            var buttons = new[]
            {
                new KeyboardButton
                {
                    Columns = 6,
                    Rows = 1,
                    Text = "OK",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "OK"
                }
            };

            // Send keyboard message
            await SendKeyboardMessage(botid, receiverId, messageText, buttons);
        }

        public async Task SendStartedMenuAsync(int botId, string receiverId, string messageText, string imageUrl)
        {
            var viberBotClient = await viberBotFactory.GetClient(botId);

            // var caption = "Test";
            // var imageUrl = "http://mgzavrebi.ge/site/local_sources/images/room_photo/room_photo_1_03a21d6e6d84361de45d6c83a51d77d0.jpg";

            Carousel carousel = null;

            var xml = "<carousel><keyboardButton columns=6 rows=2 text=\"Text\"/></carousel>";

            var serializer = new XmlSerializer(typeof(Carousel));

            using (var reader = new StringReader(xml))
            {
                carousel = (Carousel)serializer.Deserialize(reader);
            }

            var message = new CarouselMessage
            {
                Receiver = receiverId,
                MinApiVersion = 2,
                CarouselContent = carousel
                // CarouselContent = new Carousel
                // {
                //     AlternateText = "",
                //     ButtonsGroupColumns = 6,
                //     ButtonsGroupRows = 5,
                //     Buttons = new[]
                //     {
                //         new KeyboardButton
                //         {
                //             Columns = 6,
                //             Rows = 2,
                //             Text = messageText,
                //             ActionType = KeyboardActionType.None
                //         },
                //         new KeyboardButton
                //         {
                //             Columns = 6,
                //             Rows = 3,
                //             ActionType = KeyboardActionType.None,
                //             Image = imageUrl
                //         }
                //     }
                // }
            };

            await viberBotClient.SendCarouselMessageAsync(message);
        }

        public async Task SendContainerAreasMenuAsync(string receiverId)
        {
        }

        public async Task SendFixationMenuAsync(string receiverId)
        {
        }

        public async Task SendMessageAsync(string receiverId, string messageText)
        {
        }

        private async Task SendKeyboardMessage(int botId, string receiverId, string messageText, ICollection<KeyboardButton> buttons)
        {
            var viberBotClient = await viberBotFactory.GetClient(botId);

            // Keyboard message
            var keyboardMessage = new KeyboardMessage
            {
                Receiver = receiverId,
                Text = messageText,
                Keyboard = new Keyboard
                {
                    Buttons = buttons,
                    InputFieldState = KeyboardInputFieldState.Hidden,
                    DefaultHeight = false
                }
            };

            // Process keyboard message send
            await viberBotClient.SendKeyboardMessageAsync(keyboardMessage);
        }
    }

    public interface ISendMessageService
    {
        Task SendSubscribeMenuAsync(int botId, string receiverId);
        Task SendStartedMenuAsync(int botId, string receiverId, string messageText, string imageUrl);
        Task SendContainerAreasMenuAsync(string receiverId);
        Task SendFixationMenuAsync(string receiverId);
        Task SendMessageAsync(string receiverId, string messageText);
    }
}