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

        public override async Task Start(Guid agentId) 
        { 
            context.SetState(new StartedState(viberApiHttpService));

            await context.Start(agentId);
        }
        
        public override async Task SelectGarbageArea(string containerPlaceName) 
        { 
            // Send Menu

            context.SetState(new GarbageAreaSelected());
        }
    }
}