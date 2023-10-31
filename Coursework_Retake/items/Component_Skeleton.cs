using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    public abstract class Component_Skeleton
    {
        public abstract void Update(GameTime dt);

        public abstract void Draw(GameTime dt, SpriteBatch spriteB);
    }
}
