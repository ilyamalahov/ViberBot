using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViberBot.Models;
using ViberBot.Services;
using ViberBot.Services.Http;
using ViberBot.Services.StateMachine;

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class MsgbotController : ControllerBase
    {
        private readonly IStateMachineService stateMachineService;
        private readonly IHttpClientService viberApiHttpService;

        public MsgbotController(
            IStateMachineService stateMachineService, 
            IViberApiHttpService viberApiHttpService)
        {
            this.stateMachineService = stateMachineService;
            this.viberApiHttpService = viberApiHttpService;
        }

        [HttpGet("change_state")]
        public async Task ChangeState(Guid agentId, int botId, string service, int newStateId)
        {
            var serviceState = (ServiceState)newStateId;

            if(serviceState == ServiceState.Subscribed)
            {
                var stateContext = stateMachineService.Get(agentId);
                
                await stateContext.Start(agentId);
            }
        }
        
        [HttpGet("in")]
        public async Task ReceiveMessage(Guid agentId, int botId, string service, int messageTypeId)
        {
            
        }
    }
}