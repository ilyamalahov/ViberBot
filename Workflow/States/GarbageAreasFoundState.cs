using System;
using System.Threading.Tasks;
using ViberBot.Services.Http;

namespace ViberBot.Workflow.States
{
    public class GarbageAreasFoundState : State
    {
        public GarbageAreasFoundState()
        {
        }

        public override async Task Start(int botId, Guid agentId) 
        { 
            context.SetState<SubscribedState>();

            await context.Start(botId, agentId);
        }
        
        public override async Task SelectGarbageArea(string containerPlaceName) 
        { 
            context.SetState<GarbageAreaSelectedState>();
        }
    }
}