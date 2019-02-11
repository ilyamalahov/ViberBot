using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
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

        void SetState<T>() where T : State;
    }

    public interface IStateFactory
    {
        State GetState<T>() where T : State;
    }

    public class StateFactory : IStateFactory
    {
        private IContainer container;

        public StateFactory(IContainer container)
        {
            this.container = container;
        }

        public State GetState<T>() where T : State
        {
            return container.Resolve<T>();
        }
    }

    public class Context : IContext
    {
        private State currentState;
        private readonly IStateFactory stateFactory;

        public Context(IStateFactory stateFactory)
        {
            this.stateFactory = stateFactory;
        }

        public void SetState<T>() where T : State
        {
            currentState = stateFactory.GetState<T>();

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
}