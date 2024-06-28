using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    class Game_Engine
    {
        private static Game_Engine currentInstance = null;
        public static Game_Engine Instance
        {
            get
            {
                if (currentInstance == null)
                    currentInstance = new Game_Engine();
                return currentInstance;
            }

            set { currentInstance = value; }
        }

        public PlayerStats PlayerStats;
        public Enemy_1_Sets Enemy_1_Sets;
    }

    public class PlayerStats
    {
        public int Health = 3;
    }


    public class Enemy_1_Sets
    {
        public float speed = 0.0f;
    }
}
