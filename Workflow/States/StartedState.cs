using System;
using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public class StartedState : State
    {
        public override async Task Start(string receiverId)
        {
            // Check if driver

            // If User - Another action

            // If Driver - Send Driver Started Menu

            context.SetState(new DriverStartedState());
        }
    }
}