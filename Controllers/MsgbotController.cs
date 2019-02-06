using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Viber.Bot.Messages;
using Viber.Bot.Models;
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
        private readonly IMessageService messageService;
        private readonly IViberBotFactory viberBotFactory;
        private readonly IPeopleRepository peopleRepository;

        // private readonly IHttpClientService viberApiHttpService;

        public MsgbotController(
            IMessageService messageService,
            IViberBotFactory viberBotFactory,
            IPeopleRepository peopleRepository)
        {
            this.messageService = messageService;
            this.viberBotFactory = viberBotFactory;
            this.peopleRepository = peopleRepository;
            // this.viberApiHttpService = viberApiHttpService;
        }

        [HttpGet("change_state")]
        public async Task ChangeState(int botId, Guid agentId, string service, int newStateId)
        {
            // var serviceState = (ServiceState)newStateId;

            // if(serviceState == ServiceState.Subscribed)
            // {
            //     var stateContext = stateMachineService.Get(agentId);
                
            //     await stateContext.Start(botId, agentId);
            // }
        }
        
        [HttpGet("in")]
        public async Task In(int botId, Guid agentId)
        {
            
        }

        [HttpPost("out")]
        public async Task Out(int botId, Guid agentId)
        {
            Message message = null;

            using (var xmlReader = XmlReader.Create(Request.Body))
            {
                var serializer = new XmlSerializer(typeof(Message));

                message = (Message)serializer.Deserialize(xmlReader);   
            }

            var viberBotClient = viberBotFactory.GetClient(botId);

            var contact = await peopleRepository.GetContactByPeopleId(agentId);

            switch (message.ButtonPlace)
            {
                case PlaceType.Message:
                    await messageService.SendCarouselMessage(botId, agentId, message);
                    break;
                case PlaceType.Window:
                    await messageService.SendKeyboardMessage(botId, agentId, message);
                    break;
                case PlaceType.Undefined:
                    await messageService.SendMessage(botId, agentId, message);
                    break;
            }
        }
    }
}