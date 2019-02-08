using System;
using System.Threading.Tasks;
using ViberBot.Services.Http;

namespace ViberBot.Workflow.States
{
    public class StartedState : State
    {
        public StartedState()
        {
        }

        public override async Task Start(int botId, Guid agentId)
        {
            context.SetState(new SubscribedState());

            await context.Start(botId, agentId);
        }

        public override async Task SearchGarbageAreas(double altitude, double latitude)
        {
            context.SetState(new SubscribedState());
        }
    }
}