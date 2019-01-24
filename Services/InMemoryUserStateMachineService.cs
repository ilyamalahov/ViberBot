using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ViberBot.Workflow;

namespace ViberBot.Services
{
    public class InMemoryUserStateMachineService : IUserStateMachineService
    {
        private readonly ISendMessageService sendMessageService;
        private readonly ConcurrentDictionary<string, StateMachine> stateMachines;

        public InMemoryUserStateMachineService(ISendMessageService sendMessageService)
        {
            this.sendMessageService = sendMessageService;

            stateMachines = new ConcurrentDictionary<string, StateMachine>();
        }

        public StateMachine Add(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));

            if(stateMachines.TryGetValue(userId, out var stateMachine)) throw new Exception("State machine for userId already exists");

            return stateMachines.GetOrAdd(userId, new StateMachine(sendMessageService));
        }

        public void Delete(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));
            
            if(!stateMachines.TryGetValue(userId, out var stateMachine))
            {
                throw new Exception("State machine for userId already exists");
            }

            stateMachines.TryRemove(userId, out var removedStateMachine);
        }

        public StateMachine Get(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));
            
            if(!stateMachines.TryGetValue(userId, out var stateMachine))
            {
                throw new Exception("State machine for userId already exists");
            }

            return stateMachine;
        }
    }

    public interface IUserStateMachineService
    {
        StateMachine Add(string userId);
        StateMachine Get(string userId);
        void Delete(string userId);
    }
}