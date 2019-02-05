using System;
using System.Threading.Tasks;
using ViberBot.Services.Http;

namespace ViberBot.Workflow.States
{
    public class GarbageAreasFoundState : State
    {
        private readonly IViberApiHttpService viberApiHttpService;

        public GarbageAreasFoundState(IViberApiHttpService viberApiHttpService)
        {
            this.viberApiHttpService = viberApiHttpService;
        }

        public override async Task Start(int botId, Guid agentId) 
        { 
            context.SetState(new StartedState(viberApiHttpService));

            await context.Start(botId, agentId);
        }
        
        public override async Task SelectGarbageArea(string containerPlaceName) 
        { 
            context.SetState(new GarbageAreaSelected());
        }
    }
}