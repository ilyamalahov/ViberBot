using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Autofac;
using ViberBot.Services.Http;
using ViberBot.Workflow;
using ViberBot.Workflow.States;

namespace ViberBot.Services.StateMachine
{
    public class StateMachineService : IStateMachineService
    {
        private readonly IContainer container;
        private readonly Dictionary<Guid, IContext> stateContexts;

        public StateMachineService(IContainer container)
        {
            this.container = container;

            stateContexts = new Dictionary<Guid, IContext>();
        }

        public bool Add(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            if (stateContexts.ContainsKey(userId))
            {
                throw new Exception($"State context for user {userId} already exists");
            }

            lock (stateContexts)
            {
                var stateContext = container.Resolve<IContext>();

                return stateContexts.TryAdd(userId, stateContext);
            }
        }

        public void Delete(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            if (stateContexts.ContainsKey(userId))
            {
                throw new Exception($"State context for user {userId} not exists");
            }

            if (!stateContexts.Remove(userId, out var stateMachine))
            {
                throw new Exception($"Delete state machine for user \"{userId}\" failed");
            }
        }

        public IContext Get(Guid userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");

            if (!stateContexts.ContainsKey(userId) && !Add(userId))
            {
                return null;
            }

            if (!stateContexts.TryGetValue(userId, out var stateMachine))
            {
                throw new Exception($"State machine for user \"{userId}\" not exists");
            }

            return stateMachine;
        }
    }
}