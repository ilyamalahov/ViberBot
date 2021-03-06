using System;
using System.Threading.Tasks;
using ViberBot.Services.Http;

namespace ViberBot.Workflow.States
{
    public class StartedState : State
    {
        public StartedState()
        {
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
            var model = new PayloadModel<OutMessage>
            {
                BotId = botId,
                AgentId = agentId,
                Message = outMessage
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
        {
            context.SetState<SubscribedState>();

            await context.Start(botId, agentId);
        }

        public override async Task SearchGarbageAreas(double altitude, double latitude)
        {
        }
    }
}