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
    public class MainMenu : State_Manager
    {

        Texture2D Texture;
        SpriteFont Font;

        //Buttons for the Main Menu
        readonly Button NewGamebutton;
        readonly Button Highscorebutton;
        readonly Button Controlsbutton;
        readonly Button Quitbutton;

        private List<Component_Skeleton> components;

        //Background
        Texture2D Backtext;

        public MainMenu(Game1 g, ContentManager cm, GraphicsDevice gd) : base(g, cm, gd)
        {
            Texture = content.Load<Texture2D>("Button/Button");
            Font = content.Load<SpriteFont>("Font/Font");

            Backtext = content.Load<Texture2D>("Background\\MainMenuBG");

            NewGamebutton = new Button(Font, Texture)
            {
                Position = new Vector2(315, 200),
                text = "New Game",
            };

            Highscorebutton = new Button(Font, Texture)
            {
                Position = new Vector2(315, 250),
                text = "Highscore",
            };

            Controlsbutton = new Button(Font, Texture)
            {
                Position = new Vector2(315, 300),
                text = "Controls",
            };

            Quitbutton = new Button(Font, Texture)
            {
                Position = new Vector2(315, 400),
                text = "Quit",
            };

            NewGamebutton.Press += NewGame_Pressed;
            Highscorebutton.Press += Highscore_Pressed;
            Controlsbutton.Press += Controls_Pressed;
            Quitbutton.Press += Quit_Pressed;

            //List of components
            components = new List<Component_Skeleton>()
            {
                NewGamebutton,
                Highscorebutton,
                Controlsbutton,
                Quitbutton,
            };
        }

        public override void Update(GameTime dt)
        {
            foreach (var comp in components)
            {
                comp.Update(dt);
            }
        }


        public override void Draw(GameTime dt, SpriteBatch spriteB)
        {
            spriteB.Begin();

            spriteB.Draw(Backtext, new Rectangle(0, 0, 800, 600), Color.White);

            foreach (var comp in components)
            {
                comp.Draw(dt, spriteB);
            }

            spriteB.End();
        }

        public override void Unload(GameTime dt)
        {
            content.Unload();
        }

        private void NewGame_Pressed(object a, EventArgs b)
        {
            game.ChangeCurrentState(new Game_State(game, content, graphics));
        }

        private void Highscore_Pressed(object a, EventArgs b)
        {
            game.ChangeCurrentState(new Highscore_State(game, content, graphics));
        }

        private void Controls_Pressed(object a, EventArgs b)
        {
            game.ChangeCurrentState(new Controls_State(game, content, graphics));
        }

        private void Quit_Pressed(object a, EventArgs b)
        {
            game.Exit();
        }
    }
}
