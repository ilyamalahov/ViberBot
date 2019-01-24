using System;
using System.Threading.Tasks;
using ViberBot.Workflow.States;

namespace ViberBot.Workflow
{     
    public interface IProcess
    {
        Task Start(string receiverId);
        void SearchContainerPlacesNerby();
        void SendContainerPlaceName();
        void SearchContainerPlacesByName();
        void RegisterProblem();
        void RegisterBeforeProblem();
        void RegisterAfterProblem();
        void SendProblemContent();
        Task ProcessFlow(string receiverId, string messageText);
    }
    public class Process: IProcess
    {
        private readonly StartedState startedState;
        private readonly ContainerPlacesFound containerPlacesFound;

        public IState CurrentState { get; set; }

        public Process()
        {
            this.startedState = new StartedState(this);
            this.containerPlacesFound = new ContainerPlacesFound(this);

            CurrentState = startedState;
        }

        public async Task Start(string receiverId) => await CurrentState.Start(receiverId);

        public void SearchContainerPlacesNerby() => CurrentState.SearchContainerPlacesNerby();

        public void SendContainerPlaceName() => CurrentState.SendContainerPlaceName();

        public void SearchContainerPlacesByName() => CurrentState.SearchContainerPlacesByName();

        public void RegisterProblem() => CurrentState.RegisterProblem();

        public void RegisterBeforeProblem() => CurrentState.RegisterBeforeProblem();

        public void RegisterAfterProblem() => CurrentState.RegisterAfterProblem();

        public void SendProblemContent() => CurrentState.SendProblemContent();

        public Task ProcessFlow(string receiverId, string messageText)
        {
            throw new NotImplementedException();
        }
    }
}