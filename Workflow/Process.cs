using System;
using System.Threading.Tasks;

namespace ViberBot.Workflow
{ 
    public interface IState
    {
        void Start();

        //
        void SearchContainerPlacesNerby();
        void SendContainerPlaceName();

        //
        void SearchContainerPlacesByName();

        // 
        void RegisterProblem();
        void RegisterBeforeProblem();
        void RegisterAfterProblem();
        void SendProblemContent();
    }
    

    public class Process
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

        public void Start() => CurrentState.Start();

        public void SearchContainerPlacesNerby() => CurrentState.SearchContainerPlacesNerby();

        public void SendContainerPlaceName() => CurrentState.SendContainerPlaceName();

        public void SearchContainerPlacesByName() => CurrentState.SearchContainerPlacesByName();

        public void RegisterProblem() => CurrentState.RegisterProblem();

        public void RegisterBeforeProblem() => CurrentState.RegisterBeforeProblem();

        public void RegisterAfterProblem() => CurrentState.RegisterAfterProblem();

        public void SendProblemContent() => CurrentState.SendProblemContent();
    }

    internal class ContainerPlacesFound : IState
    {
        private Process process;

        public ContainerPlacesFound(Process process)
        {
            this.process = process;
        }

        public void RegisterAfterProblem()
        {
            throw new NotImplementedException();
        }

        public void RegisterBeforeProblem()
        {
            throw new NotImplementedException();
        }

        public void RegisterProblem()
        {
            throw new NotImplementedException();
        }

        public void SearchContainerPlacesByName()
        {
            throw new NotImplementedException();
        }

        public void SearchContainerPlacesNerby()
        {
            throw new NotImplementedException();
        }

        public void SendContainerPlaceName()
        {
            throw new NotImplementedException();
        }

        public void SendProblemContent()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }

    public class StartedState : IState
    {
        private Process process;

        public StartedState(Process process)
        {
            this.process = process;
        }

        public void Start()
        {
            process.CurrentState = new StartedState(process);
        }

        public void RegisterAfterProblem()
        {
            throw new NotImplementedException();
        }

        public void RegisterBeforeProblem()
        {
            throw new NotImplementedException();
        }

        public void RegisterProblem()
        {
            throw new NotImplementedException();
        }

        public void SearchContainerPlacesByName()
        {
            throw new NotImplementedException();
        }

        public void SearchContainerPlacesNerby()
        {
            throw new NotImplementedException();
        }

        public void SendContainerPlaceName()
        {
            throw new NotImplementedException();
        }

        public void SendProblemContent()
        {
            throw new NotImplementedException();
        }
    }
}