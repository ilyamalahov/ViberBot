using System;
using ViberBot.Workflow;

namespace ViberBot.Services
{
    public interface IStateMachineService
    {
        IContext Add(Guid userId);
        IContext Get(Guid userId);
        void Delete(Guid userId);
    }
}