using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Viber.Bot.Messages;
using Viber.Bot.Models;
using ViberBot.Extensions;
using ViberBot.Factories;
using ViberBot.Models;
using ViberBot.Repositories;
using ViberBot.Services;
using ViberBot.Services.Http;
using ViberBot.Services.StateMachine;

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IStateMachineService stateMachineService;

        public BotController(
            IStateMachineService stateMachineService)
        {
            this.stateMachineService = stateMachineService;
        }

        [HttpPost("change_state")]
        public async Task<IActionResult> ChangeState([FromBody] PayloadModel<ServiceState> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Message == ServiceState.Subscribed)
            {
                var stateContext = stateMachineService.Get(model.AgentId);

                await stateContext.Start(model.BotId, model.AgentId);
            }

            return Ok();
        }

        [HttpPost("in")]
        public IActionResult In([FromBody] PayloadModel<InMessage> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messageType = model.Message.DetermineMessageType();

            var stateContext = stateMachineService.Get(model.AgentId);

            return Ok();
        }
    }
}