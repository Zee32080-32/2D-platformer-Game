using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake.Game_Controls
{
    class Keyboard_Events : EventArgs
    {
        public readonly KeyboardState CurrKS;
        public readonly KeyboardState PrevKS;
        public readonly Keys Keys;

        public Keyboard_Events(Keys key, KeyboardState currKS, KeyboardState prevKS)
        {
            CurrKS = currKS;
            PrevKS = prevKS;
            Keys = key;
        }
    }
}
