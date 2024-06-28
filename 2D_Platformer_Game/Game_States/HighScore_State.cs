using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    class Highscore_State : State_Manager
    {
        //Background
        Texture2D texture;

        //Button for Quit
        Texture2D buttonTexture;
        SpriteFont ButtonFont;
        readonly Button QuitButton;
        public Highscore_State(Game1 g, ContentManager ContentManager, GraphicsDevice gd) : base(g, ContentManager, gd)
        {
            texture = content.Load<Texture2D>("Background\\HighscoresBG");
            buttonTexture = content.Load<Texture2D>("Button/Button");
            ButtonFont = content.Load<SpriteFont>("Font/Font");

            QuitButton = new Button(ButtonFont, buttonTexture)
            {
                Position = new Vector2(315, 400),
                text = "Quit",
            };

            QuitButton.Press += Quit_Pressed;
        }

        public override void Update(GameTime dt)
        {
            QuitButton.Update(dt);
        }

        private void Quit_Pressed(object sender, EventArgs e)
        {
            game.ChangeCurrentState(new MainMenu(game, content, graphics));
        }

        public override void Unload(GameTime dt)
        {
            content.Unload();
        }


        public override void Draw(GameTime dt, SpriteBatch spriteB)
        {
            spriteB.Begin();

            spriteB.Draw(texture, new Rectangle(0, 0, 800, 600), Color.White);

            game.DrawHighscore(dt, spriteB);

            QuitButton.Draw(dt, spriteB);

            spriteB.End();
        }
    }
}
