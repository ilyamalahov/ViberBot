using System.Collections.Generic;
using System.Threading.Tasks;
using Viber.Bot;

namespace ViberBot.Services
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IViberBotClient viberBotClient;

        public SendMessageService(IViberBotClient viberBotClient)
        {
            this.viberBotClient = viberBotClient;
        }

        public async Task SendContainerPlaceNameMenuAsync(string receiverId)
        {
            var buttons = new[]
            {
                new KeyboardButton
                {
                    Columns = 3,
                    Rows = 1,
                    Text = "Отправка текста",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "SendContainerPlaceName"
                },
                new KeyboardButton
                {
                    Columns = 3,
                    Rows = 1,
                    Text = "Назад",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "ToStart"
                }
            };

            await SendKeyboardMessageAsync(receiverId, buttons);
        }

        public async Task SendContainerPlacesMenuAsync(string receiverId, string[] containerPlaces)
        {
            var buttons = new List<KeyboardButton>();

            foreach (var containerPlace in containerPlaces)
            {
                buttons.Add(new KeyboardButton
                {
                    Columns = 1,
                    Rows = 1,
                    Text = containerPlace,
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "SelectedPlace"
                });
            }

            // 
            buttons.Add(new KeyboardButton
            {
                Columns = 6,
                Rows = 1,
                Text = "Назад",
                ActionType = KeyboardActionType.Reply,
                ActionBody = "ToStart"
            });

            await SendKeyboardMessageAsync(receiverId, buttons);
        }

        public async Task SendProblemContentMenuAsync(string receiverId)
        {
            var buttons = new[]
            {
                new KeyboardButton
                {
                    Columns = 3,
                    Rows = 1,
                    Text = "Отправка текста/фото/видео",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "SendProblemContent"
                },
                new KeyboardButton
                {
                    Columns = 3,
                    Rows = 1,
                    Text = "Назад",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "ToProblemStart"
                }
            };

            await SendKeyboardMessageAsync(receiverId, buttons);
        }

        public async Task SendProblemMenuAsync(string receiverId)
        {
            var buttons = new[]
            {
                new KeyboardButton
                {
                    Columns = 2,
                    Rows = 1,
                    Text = "Проблема",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "RegisterProblem"
                },
                new KeyboardButton
                {
                    Columns = 2,
                    Rows = 1,
                    Text = "До",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "RegisterBeforeProblem"
                },
                new KeyboardButton
                {
                    Columns = 2,
                    Rows = 1,
                    Text = "После",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "RegisterAfterProblem"
                }
            };
            
            await SendKeyboardMessageAsync(receiverId, buttons);
        }

        public async Task SendStartedMenuAsync(string receiverId)
        {
            var buttons = new []
            {
                new KeyboardButton
                {
                    Columns = 2,
                    Rows = 1,
                    Text = "КП рядом",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "ControlPointNear"
                },
                new KeyboardButton
                {
                    Columns = 2,
                    Rows = 1,
                    Text = "Поиск КП по имени",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "ControlPointSearch"
                },
                new KeyboardButton
                {
                    Columns = 2,
                    Rows = 1,
                    Text = "<b>В начало</b>",
                    ActionType = KeyboardActionType.Reply,
                    ActionBody = "ToStart",
                    BackgroundColor = "#f4e542"
                }
            };

            await SendKeyboardMessageAsync(receiverId, buttons);
        }

        public async Task SendMessageAsync(string receiverId, string messageText)
        {
            var textMessage = new TextMessage
            {
                Receiver = receiverId,
                Text = messageText
            };

            // Process keyboard message send
            await viberBotClient.SendTextMessageAsync(textMessage);
        }

        private async Task SendKeyboardMessageAsync(string receiverId, ICollection<KeyboardButton> buttons)
        {
            // Keyboard message
            var keyboardMessage = new KeyboardMessage
            {
                Receiver = receiverId,
                Text = "Выберите нужный элемент из списка",
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
        Task SendMessageAsync(string receiverId, string messageText);

        Task SendStartedMenuAsync(string receiverId);
        Task SendContainerPlacesMenuAsync(string receiverId, string[] containerPlaces);
        Task SendProblemMenuAsync(string receiverId);
        Task SendProblemContentMenuAsync(string receiverId);
        Task SendContainerPlaceNameMenuAsync(string receiverId);
    }
}