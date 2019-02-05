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

        public override async Task Start(int botId, Guid agentId)
        {
            var parameters = new
            {
                botId,
                agentId
            };

            await viberApiHttpService.SendGetAsync("start", parameters);

            context.SetState(new DriverState(viberApiHttpService));
        }
    }
}