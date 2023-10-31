using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake.Enemy_States
{
    class FSM
    {
        private object Owner;
        private List<State> States;

        private State Current_State;

        public FSM() : this(null)
        {
        }

        public FSM(object x)
        {
            Owner = x;
            States = new List<State>();
            Current_State = null;
        }

        public void Initialise_State(string name)
        {
            Current_State = States.Find(state => state.Name.Equals(name));
            if (Current_State != null)
            {
                Current_State.Enter(Owner);
            }
        }

        public void Add_State(State state)
        {
            States.Add(state);
        }

        public void Update(GameTime gameTime)
        {
            // Null check the current state of the FSM
            if (Current_State == null) return;

            for (int i = 0; i < Current_State.Transitions.Count; i++)
            {
                Transition t = Current_State.Transitions[i];

                if (t.Condition())
                {
                    Current_State.Exit(Owner);
                    Current_State = t.NewState;
                    Current_State.Enter(Owner);
                    break;
                }
            }

            Current_State.Execute(Owner, gameTime);
        }
    }
}
