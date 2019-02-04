using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ViberBot.Services.Http;
using ViberBot.Workflow;
using ViberBot.Workflow.States;

namespace ViberBot.Services.StateMachine
{
    public class InMemoryStateMachineService : IStateMachineService
    {
        private readonly ISendMessageService sendMessageService;
        private readonly IViberApiHttpService viberApiHttpService;
        private readonly Dictionary<Guid, IContext> stateContexts;

        public InMemoryStateMachineService(ISendMessageService sendMessageService, IViberApiHttpService viberApiHttpService)
        {
            this.sendMessageService = sendMessageService;
            this.viberApiHttpService = viberApiHttpService;

            stateContexts = new Dictionary<Guid, IContext>();
        }

        // public IContext Add(Guid userId)
        // {
        //     if (userId == null) throw new ArgumentNullException(nameof(userId));

        //     if (!stateContexts.ContainsKey(userId))
        //     {
        //         lock (stateContexts)
        //         {
        //             if (!stateContexts.TryAdd(userId, new Context(new StartedState())))
        //             {
        //                 return null;
        //             }
        //         }
        //     }

        //     return Get(userId);
        // }

        public void Delete(Guid userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            if (!stateContexts.Remove(userId, out var stateMachine))
            {
                throw new Exception($"Delete state machine for user \"{userId}\" failed");
            }
        }

        public IContext Get(Guid userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            if (!stateContexts.ContainsKey(userId))
            {
                lock (stateContexts)
                {
                    if (!stateContexts.TryAdd(userId, new Context(new StartedState(viberApiHttpService))))
                    {
                        return null;
                    }
                }
            }

            if (!stateContexts.TryGetValue(userId, out var stateMachine))
            {
                throw new Exception($"State machine for user \"{userId}\" not exists");
            }

            return stateMachine;
        }
    }
}