using System.Collections.Generic;
using System.Threading.Tasks;
using Viber.Bot;
using Viber.Bot.Enums;
using Viber.Bot.Messages;
using Viber.Bot.Models;

namespace ViberBot.Services
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IViberBotClient viberBotClient;

        public SendMessageService(IViberBotClient viberBotClient)
        {
            this.viberBotClient = viberBotClient;
        }

        public async Task SendSubscribeMenuAsync(string receiverId)
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
            await SendKeyboardMessage(receiverId, messageText, buttons);
        }

        public async Task SendStartedMenuAsync(string receiverId)
        {
            var caption = "Test";
            var imageUrl = "http://mgzavrebi.ge/site/local_sources/images/room_photo/room_photo_1_03a21d6e6d84361de45d6c83a51d77d0.jpg";

            var message = new CarouselMessage
            {
                Receiver = receiverId,
                MinApiVersion = 2,
                CarouselContent = new Carousel
                {
                    AlternateText = "",
                    ButtonsGroupColumns = 6,
                    ButtonsGroupRows = 6,
                    Buttons = new[]
                    {
                        new KeyboardButton
                        {
                            Columns = 6,
                            Rows = 2,
                            Text = caption,
                            ActionType = KeyboardActionType.None
                        },
                        new KeyboardButton
                        {
                            Columns = 6,
                            Rows = 3,
                            ActionType = KeyboardActionType.None,
                            Image = imageUrl
                        },
                        new KeyboardButton
                        {
                            Columns = 6,
                            Rows = 1,
                            Text = "OK",
                            ActionType = KeyboardActionType.None
                        }
                    }
                }
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

        private async Task SendKeyboardMessage(string receiverId, string messageText, ICollection<KeyboardButton> buttons)
        {
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
        Task SendSubscribeMenuAsync(string receiverId);
        Task SendStartedMenuAsync(string receiverId);
        Task SendContainerAreasMenuAsync(string receiverId);
        Task SendFixationMenuAsync(string receiverId);
        Task SendMessageAsync(string receiverId, string messageText);
    }
}