using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViberBot.Services;

namespace ViberBot.Workflow.StateMachine
{
    public enum State
    {
        Inactive,
        Active,
        Paused,
        Terminated
    }

    public enum Command
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }


    public class Transition
    {
        readonly private State currentState;
        readonly private Command command;

        public Transition(State currentState, Command command)
        {
            this.currentState = currentState;
            this.command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * currentState.GetHashCode() + 31 * command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Transition;

            return other != null && this.currentState == other.currentState && this.command == other.command;
        }
    }

    public class Context
    {
        private Dictionary<Transition, State> transitions;
        public State CurrentState { get; private set; }

        public Context()
        {
            CurrentState = State.Inactive;

            transitions = new Dictionary<Transition, State>
            {
                { new Transition(State.Inactive, Command.Exit), State.Terminated },
                { new Transition(State.Inactive, Command.Begin), State.Active },
                { new Transition(State.Active, Command.End), State.Inactive },
                { new Transition(State.Active, Command.Pause), State.Paused },
                { new Transition(State.Paused, Command.End), State.Inactive },
                { new Transition(State.Paused, Command.Resume), State.Active }
            };
        }

        private void Start(string arg1, string arg2)
        {
        }

        public State GetNext(Command command)
        {
            var currentTransition = new Transition(CurrentState, command);

            if (!transitions.TryGetValue(currentTransition, out var nextState))
            {
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            }

            return nextState;
        }

        public void Trigger(Command command)
        {
            var currentTransition = new Transition(CurrentState, command);

            if (!transitions.TryGetValue(currentTransition, out var nextState))
            {
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            }
        }
    }
}