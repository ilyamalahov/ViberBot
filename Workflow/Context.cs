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
        Task Start(int botId, Guid agentId);
        Task SearchGarbageAreas(double altitude, double latitude);
        Task SelectGarbageArea(string garbageAreaName);
        Task WaitMediaFile();

        void SetState(State state);
    }
    public class Context : IContext
    {
        private State currentState;

        // private IDictionary<Command, Func<string, Task>> triggers;
        public Context(State initialState)
        {
            // triggers = new Dictionary<Command, Func<string, Task>>()
            // {
            //     { Command.Start, async (string receiverId) => await currentState.Start(receiverId) },
            //     { Command.SearchContainerPlacesNerby, async (receiverId) => await currentState.SearchContainerPlacesNerby() }
            // };

            SetState(initialState);
        }

        public void SetState(State state)
        {
            currentState = state;
            currentState.SetContext(this);
        }

        /// <inheritdoc/>
        public async Task Start(int botId, Guid agentId) => await currentState.Start(botId, agentId);
        
        /// <inheritdoc/>
        public async Task SearchGarbageAreas(double altitude, double latitude) => await currentState.SearchGarbageAreas(altitude, latitude);

        /// <inheritdoc/>
        public async Task SelectGarbageArea(string garbageAreaName) => await currentState.SelectGarbageArea(garbageAreaName);

        /// <inheritdoc/>
        public async Task WaitMediaFile() => await currentState.WaitMediaFile();
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Command
    {
        Start,
        SearchGarbageAreas,
        SelectGarbageArea,
        WaitMediaFile
    }
}