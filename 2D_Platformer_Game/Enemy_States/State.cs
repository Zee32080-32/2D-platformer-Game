using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake.Enemy_States
{
    public abstract class State
    {
        public abstract void Enter(object owner);
        public abstract void Exit(object owner);
        public abstract void Execute(object owner, GameTime dt);

        public string Name
        {
            get;
            set;
        }

        private List<Transition> TransitionsList = new List<Transition>();
        public List<Transition> Transitions
        {
            get { return TransitionsList; }
        }

        public void AddTransition(Transition a)
        {
            TransitionsList.Add(a);
        }

    }

    public class Transition
    {
        public readonly State NewState;
        public readonly Func<bool> Condition;

        public Transition(State nextstate, Func<bool> condition)
        {
            NewState = nextstate;
            Condition = condition;
        }
    }
}
