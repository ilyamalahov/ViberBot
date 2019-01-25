using System;
using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public class ContainerPlacesFoundState : State
    {
        public override async Task Start(string receiverId) 
        { 
            context.SetState(new StartedState());

            await context.Start(receiverId);
        }
        
        public override async Task SelectContainerPlace(string containerPlaceName) 
        { 
            // Send Problem Menu

            context.SetState(new ProblemStartedState());
        }
    }
}