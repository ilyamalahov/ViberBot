using ViberBot.Workflow;

namespace ViberBot.Services
{
    public interface IUserStateMachineService
    {
        IContext Add(string userId);
        IContext Get(string userId);
        void Delete(string userId);
    }
}