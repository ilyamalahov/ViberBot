using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using ViberBot.Models;
using ViberBot.Repositories;
using ViberBot.Services.Http;

namespace ViberBot.Workflow.States
{
    public class SubscribedState : State
    {
        private readonly IBotRepository botRepository;

        public SubscribedState(IBotRepository botRepository)
        {
            this.botRepository = botRepository;  
        }

        public override async Task Start(int botId, Guid agentId)
        {    
            var bot = await botRepository.GetById(botId);

            OutMessage outMessage = null;

            var serializer = new DataContractSerializer(typeof(OutMessage));

            using (var reader = XmlReader.Create(bot.InvitationMessage))
            {
                outMessage = (OutMessage)serializer.ReadObject(reader);   
            }

            // 
            var model = new MessageModel<OutMessage>
            {
                BotId = botId,
                AgentId = agentId,
                Message = outMessage
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

                await httpClient.PostAsync("msgbot/out", model, new XmlMediaTypeFormatter());
            }

            context.SetState<StartedState>();
        }
    }
}