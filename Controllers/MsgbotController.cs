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

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class MsgbotController: ControllerBase
    {
        private readonly ISendMessageService sendMessageService;
        private readonly IPeopleRepository peopleRepository;

        public MsgbotController(
            IPeopleRepository peopleRepository,
            ISendMessageService sendMessageService)
        {
            this.peopleRepository = peopleRepository;
            this.sendMessageService = sendMessageService;
        }

        // [HttpPost("change_state")]
        // public IActionResult ChangeState(int botId, Guid agentId, string service, int newStateId)
        // {
        //     // var serviceState = (ServiceState)newStateId;

        //     // if(serviceState == ServiceState.Subscribed)
        //     // {
        //     //     var stateContext = stateMachineService.Get(agentId);
                
        //     //     await stateContext.Start(botId, agentId);
        //     // }

        //     return Ok();
        // }
        
        // [HttpPost("in")]
        // public IActionResult In([FromBody] MessageModel<InMessage> model)
        // {
        //     if (!ModelState.IsValid)
        //     { 
        //         return BadRequest(ModelState);
        //     }

        //     var messageType = model.Message.DetermineMessageType();

        //     return Ok(messageType);
        // }

        [HttpPost("out")]
        public async Task<IActionResult> Out([FromBody] MessageModel<OutMessage> model)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            var contact = await peopleRepository.GetContactByPeopleId(model.AgentId);

            switch (model.Message.DetermineMessageType())
            {
                case MessageType.Text:
                    await sendMessageService.SendTextMessageAsync(model.BotId, contact.InfoTextId, model.Message);
                    break;
                case MessageType.Picture:
                    await sendMessageService.SendPictureMessageAsync(model.BotId, contact.InfoTextId, model.Message);
                    break;
                case MessageType.Location:
                    await sendMessageService.SendLocationMessageAsync(model.BotId, contact.InfoTextId, model.Message);
                    break;
                case MessageType.RichMedia:
                    await sendMessageService.SendRichMediaMessageAsync(model.BotId, contact.InfoTextId, model.Message);
                    break;
                case MessageType.Keyboard:
                    await sendMessageService.SendKeyboardMessageAsync(model.BotId, contact.InfoTextId, model.Message);
                    break;
            }

            return Ok();
        }
    }
}