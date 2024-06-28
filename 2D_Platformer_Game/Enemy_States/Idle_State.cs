using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Coursework_Retake.Enemy_States
{
    class Idle_state : State
    {
        private double Current_Timer = 0.0;
        private const double DirChange = 0.5;

        public Idle_state()
        {
            Name = "idle";
        }

        public override void Enter(object owner)
        {
            Enemy_1 enemy_1 = owner as Enemy_1;
            Current_Timer = 0.0;
        }

        public override void Execute(object owner, GameTime dt)
        {
            Enemy_1 enemy_1 = owner as Enemy_1;
            if (enemy_1 == null) return;

            if (Current_Timer >= DirChange)
            {
                Current_Timer = 0.0;
                enemy_1.IdleAction(dt);
            }
            else
            {
                Current_Timer += dt.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Exit(object owner)
        {
            Enemy_1 enemy_1 = owner as Enemy_1;
            Current_Timer = 0.0;
        }
    }
}
