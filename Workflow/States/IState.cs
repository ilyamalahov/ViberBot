using System;
using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public interface IState
    {
        Task Start(string receiverId);

        //
        void SearchContainerPlacesNerby();
        void SendContainerPlaceName();

        //
        void SearchContainerPlacesByName();

        // 
        void RegisterProblem();
        void RegisterBeforeProblem();
        void RegisterAfterProblem();
        void SendProblemContent();
    }
}