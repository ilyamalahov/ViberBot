using System;
using System.Threading.Tasks;
using ViberBot.Services;

namespace ViberBot.Workflow
{
    public enum State
    {
        ContainerPlacesFound,
        ProblemRegistered,
        AfterProblemRegistered,
        BeforeProblemRegistered,
        ProblemContentSended,
        ProblemStarted,
        Started
    }

    public class StateProcess
    {
        private ISendMessageService sendMessageService;

        public StateProcess(ISendMessageService sendMessageService)
        {
            this.sendMessageService = sendMessageService;
        }

        public void SetState(State value)
        {
        }

        public async Task ProcessFlow(string userId, string messageText)
        {
            if (messageText == "ToStart")
            {
                await Start(userId);

                return;
            }

            if (messageText == "SearchContainerPlacesNearby")
            {
                var places = new string[] { "Площадка 1", "Площадка 2", "Площадка 3" };

                await sendMessageService.SendContainerPlacesMenuAsync(userId, places);

                SetState(State.ContainerPlacesFound);

                return;
            }

            // Регистрация проблемы / до проблемы / после проблемы
            if (messageText == "RegisterProblem")
            {
                // 

                SetState(State.ProblemRegistered);

                return;
            }
            if (messageText == "RegisterBeforeProblem")
            {
                // 

                SetState(State.AfterProblemRegistered);

                return;
            }
            if (messageText == "RegisterAfterProblem")
            {
                // 

                SetState(State.BeforeProblemRegistered);

                return;
            }
            if (messageText == "SendProblemContent")
            {
                // 

                SetState(State.ProblemContentSended);

                return;
            }

            // Поиск контейнерной площадки (КП) по имени
            if (messageText == "SearchContainerPlacesByName")
            {
                // 

                SetState(State.ContainerPlacesFound);

                return;
            }

            if (messageText == "SendContainerPlaceName")
            {
                // 
                var places = new string[] { "", "", "" };

                await sendMessageService.SendContainerPlacesMenuAsync(userId, places);

                // 
                SetState(State.ProblemContentSended);

                return;
            }

            // РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ ???
            if (messageText == "SelectedPlace")
            {
                // 
                await sendMessageService.SendProblemMenuAsync(userId);

                SetState(State.ProblemStarted);

                return;
            }


            // РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ ???
            if (messageText == "КОНКРЕТНАЯ КП")
            {
                // 

                SetState(State.ProblemStarted);

                return;
            }
        }

        public async Task Start(string receiverId)
        {
            await sendMessageService.SendStartedMenuAsync(receiverId);

            SetState(State.Started);
        }
    }
}