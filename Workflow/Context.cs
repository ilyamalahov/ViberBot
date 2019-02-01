using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ViberBot.Workflow.States;

namespace ViberBot.Workflow
{
    public interface IContext
    {
        Task Start(string receiverId);
        Task SearchContainerPlacesNerby();
        Task SendContainerPlaceName();
        Task SearchContainerPlacesByName();
        Task RegisterProblem();
        Task RegisterBeforeProblem();
        Task RegisterAfterProblem();
        Task SendProblemContent();
        Task ProcessFlow(string receiverId, string messageText);

        void SetState(State state);
        Task Trigger(Command command);
    }
    public class Context : IContext
    {
        private State currentState;

        public Context(State initialState)
        {
            triggers = new Dictionary<Command, Func<string, Task>>()
            {
                { Command.Start, async (string receiverId) => await currentState.Start(receiverId) },
                { Command.SearchContainerPlacesNerby, async (receiverId) => await currentState.SearchContainerPlacesNerby() }
            };

            SetState(initialState);
        }

        public void SetState(State state)
        {
            currentState = state;
            currentState.SetContext(this);
        }

        private IDictionary<Command, Func<string, Task>> triggers;

        public async Task Start(string receiverId) => await currentState.Start(receiverId);
        public async Task SearchContainerPlacesNerby() => await currentState.SearchContainerPlacesNerby();
        public async Task SendContainerPlaceName() => await currentState.SendContainerPlaceName();
        public async Task SearchContainerPlacesByName() => await currentState.SearchContainerPlacesByName();
        public async Task RegisterProblem() => await currentState.RegisterProblem();
        public async Task RegisterBeforeProblem() => await currentState.RegisterBeforeProblem();
        public async Task RegisterAfterProblem() => await currentState.RegisterAfterProblem();
        public async Task SendProblemContent() => await currentState.SendProblemContent();
        public async Task ProcessFlow(string receiverId, string messageText) { }

        public async Task Trigger(Command command)
        {
            if(triggers.TryGetValue(command, out var action))
            {
                throw new Exception("");
            }
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Command
    {
        Start,
        SearchContainerPlacesNerby,
        SendContainerPlaceName,
        SearchContainerPlacesByName,
        RegisterProblem,
        RegisterBeforeProblem,
        RegisterAfterProblem,
        SendProblemContent
    }
}