using System;
using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public class StartedState : IState
    {
        private Process process;

        public StartedState(Process process)
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

        public async Task Start(string receiverId)
        {
            throw new NotImplementedException();
        }
    }
}