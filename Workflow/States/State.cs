using System;
using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public abstract class State
    {
        protected IContext context;

        public void SetContext(IContext context)
        {
            this.context = context;
        }

        protected Task RouteError()
        {
            return Task.CompletedTask;
        }
        
        public virtual Task Start(string receiverId) { return Task.CompletedTask; }

        //
        public virtual Task SearchContainerPlacesNerby() { return Task.CompletedTask; }
        public virtual Task SendContainerPlaceName() { return Task.CompletedTask; }

        //
        public virtual Task SearchContainerPlacesByName() { return Task.CompletedTask; }
        public virtual Task SelectContainerPlace(string containerPlaceName) { return Task.CompletedTask; }

        // 
        public virtual Task RegisterProblem() { return Task.CompletedTask; }
        public virtual Task RegisterBeforeProblem() { return Task.CompletedTask; }
        public virtual Task RegisterAfterProblem() { return Task.CompletedTask; }
        public virtual Task SendProblemContent() { return Task.CompletedTask; }
    }
}