using System;
using System.Threading.Tasks;
using ViberBot.Services.Http;

namespace ViberBot.Workflow.States
{
    public class StartedState : State
    {
        private readonly IViberApiHttpService viberApiHttpService;

        public StartedState(IViberApiHttpService viberApiHttpService)
        {
            this.viberApiHttpService = viberApiHttpService;
        }

        public override async Task Start(Guid agentId)
        {
            // Check if driver

            // If User - Another action

            // If Driver - Send Driver Started Menu

            await viberApiHttpService.SendGetAsync("in");

            context.SetState(new DriverState(viberApiHttpService));
        }
    }
}