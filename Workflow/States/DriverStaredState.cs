using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public class DriverStartedState : State
    {
        public override async Task Start(string receiverId)
        {
            context.SetState(new StartedState());

            await context.Start(receiverId);
        }

        public override async Task SearchContainerPlacesByName()
        {

        }

        public override async Task SearchContainerPlacesNerby()
        {

        }
    }
}