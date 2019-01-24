using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ViberBot.Workflow;

namespace ViberBot.Services
{
    public class InMemoryUserStateMachineService : IUserStateMachineService
    {
        private readonly ISendMessageService sendMessageService;
        private readonly ConcurrentDictionary<string, IProcess> stateProcesses;

        public InMemoryUserStateMachineService(ISendMessageService sendMessageService)
        {
            this.sendMessageService = sendMessageService;

            stateProcesses = new ConcurrentDictionary<string, IProcess>();
        }

        public IProcess Add(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));

            //if(stateProcesses.TryGetValue(userId, out var stateMachine))
            // {
            //   throw new Exception("State machine for userId already exists");  
            // } 

            return stateProcesses.GetOrAdd(userId, new Process());
        }

        public void Delete(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));
            
            if(!stateProcesses.TryRemove(userId, out var stateMachine))
            {
                throw new Exception($"Delete state machine for user \"{userId}\" failed");
            }
        }

        public IProcess Get(string userId)
        {
            if(userId == null) throw new ArgumentNullException(nameof(userId));
            
            if(!stateProcesses.TryGetValue(userId, out var stateMachine))
            {
                throw new Exception($"State machine for user \"{userId}\" not exists");
            }

            return stateMachine;
        }
    }

    public interface IUserStateMachineService
    {
        IProcess Add(string userId);
        IProcess Get(string userId);
        void Delete(string userId);
    }
}