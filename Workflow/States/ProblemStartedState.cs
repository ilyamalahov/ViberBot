using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public class ProblemStartedState : State
    {
        public override async Task Start(string receiverId)
        {
            context.SetState(new StartedState());

            await context.Start(receiverId);
        }

        public override async Task RegisterBeforeProblem()
        {

        }
        public override async Task RegisterProblem()
        {

        }
        public override async Task RegisterAfterProblem()
        {

        }
    }
}