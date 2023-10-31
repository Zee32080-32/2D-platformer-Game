using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    class Lost_State : State_Manager
    {
        Texture2D texture;

        public Lost_State(Game1 g, ContentManager cm, GraphicsDevice gd) : base(g, cm, gd)
        {
            texture = content.Load<Texture2D>("Background\\LostBG");
        }

        public override void Update(GameTime dt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.ChangeCurrentState(new MainMenu(game, content, graphics));
        }

        public override void Unload(GameTime dt)
        {
            content.Unload();
        }

        public override void Draw(GameTime dt, SpriteBatch spriteB)
        {
            spriteB.Begin();

            spriteB.Draw(texture, new Rectangle(0, 0, 800, 480), Color.White);

            spriteB.End();
        }
    }
}
