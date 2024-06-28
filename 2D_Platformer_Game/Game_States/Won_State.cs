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
    class Won_State : State_Manager
    {
        Texture2D texture;

        public Won_State(Game1 g, ContentManager contentManager, GraphicsDevice gd) : base(g, contentManager, gd)
        {
            texture = content.Load<Texture2D>("HUD\\WinLayer");
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
                game.ChangeCurrentState(new MainMenu(game, content, graphics));
        }

        public override void Unload(GameTime gameTime)
        {
            content.Unload();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteB)
        {
            spriteB.Begin();

            spriteB.Draw(texture, new Rectangle(0, 0, 800, 480), Color.White);

            spriteB.End();
        }
    }
}
