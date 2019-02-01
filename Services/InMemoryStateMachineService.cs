using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ViberBot.Workflow;
using ViberBot.Workflow.States;

namespace ViberBot.Services
{
    public class InMemoryStateMachineService : IStateMachineService
    {
        private readonly ISendMessageService sendMessageService;
        private readonly ConcurrentDictionary<string, IContext> stateProcesses;

        public InMemoryStateMachineService(ISendMessageService sendMessageService)
        {
            this.sendMessageService = sendMessageService;

            stateProcesses = new ConcurrentDictionary<string, IContext>();
        }

        public IContext Add(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));

            //if(stateProcesses.TryGetValue(userId, out var stateMachine))
            // {
            //   throw new Exception("State machine for userId already exists");  
            // } 

            return stateProcesses.GetOrAdd(userId, new Context(new StartedState()));
        }

        public void Delete(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));
            
            if(!stateProcesses.TryRemove(userId, out var stateMachine))
            {
                throw new Exception($"Delete state machine for user \"{userId}\" failed");
            }
        }

        public IContext Get(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));
            
            if(!stateProcesses.TryGetValue(userId, out var stateMachine))
            {
                throw new Exception($"State machine for user \"{userId}\" not exists");
            }

            return stateMachine;
        }
    }
}