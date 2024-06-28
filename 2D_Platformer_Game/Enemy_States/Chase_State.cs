using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_Retake.Enemy_States
{
    class Chase_state : State
    {
        public Chase_state()
        {
            Name = "Chase";
        }

        public override void Enter(object owner)
        {
            Enemy_1 enemy_1 = owner as Enemy_1;
        }

        public override void Execute(object owner, GameTime dt)
        {
            Enemy_1 enemy_1 = owner as Enemy_1;
            if (enemy_1 == null) return;
        }

        public override void Exit(object owner)
        {
            Enemy_1 enemy_1 = owner as Enemy_1;
        }
    }
}
