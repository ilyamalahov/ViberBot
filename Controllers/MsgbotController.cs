using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViberBot.Models;
using ViberBot.Services;

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class MsgbotController : ControllerBase
    {
        private readonly IStateMachineService stateMachineService;

        public MsgbotController(IStateMachineService stateMachineService)
        {
            this.stateMachineService = stateMachineService;
        }

        [HttpGet("change_state")]
        public async Task ChangeState(Guid agentId, int botId, string service, int newStateId)
        {
            var serviceState = (ServiceState)newStateId;

            if(serviceState == ServiceState.Subscribed)
            {
                var stateContext = stateMachineService.Add(agentId);

                // ???????
            }
        }
    }
}