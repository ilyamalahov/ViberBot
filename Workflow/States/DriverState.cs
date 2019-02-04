using System;
using System.Threading.Tasks;
using ViberBot.Services.Http;

namespace ViberBot.Workflow.States
{
    public class DriverState : State
    {
        private readonly IViberApiHttpService viberApiHttpService;

        public DriverState(IViberApiHttpService viberApiHttpService)
        {
            this.viberApiHttpService = viberApiHttpService;
        }

        public override async Task Start(Guid agentId)
        {
            context.SetState(new StartedState(viberApiHttpService));

            await context.Start(agentId);
        }

        public override async Task SearchGarbageAreas(double altitude, double latitude)
        {
            context.SetState(new StartedState(viberApiHttpService));
        }
    }
}