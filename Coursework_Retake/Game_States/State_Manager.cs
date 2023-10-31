using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    public abstract class State_Manager
    {
        protected ContentManager content;
        protected GraphicsDevice graphics;

        protected Game1 game;

        public State_Manager(Game1 g, ContentManager cm, GraphicsDevice gd)
        {
            game = g;

            content = cm;

            graphics = gd;
        }

        public abstract void Update(GameTime dt);

        public abstract void Draw(GameTime dt, SpriteBatch spriteB);

        public abstract void Unload(GameTime dt);
    }
}
