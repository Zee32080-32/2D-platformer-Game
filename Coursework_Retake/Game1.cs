using System;
using System.IO;
using System.Linq;
using INM379CWCGA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Coursework_Retake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static int ScreenWidth;
        public static int ScreenHeight;

        //States
        private State_Manager currState;
        private State_Manager nextState;

        ScoreManager rankingManager;
        SpriteFont Font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            //Initialises screen size and objects.
            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Font/gameFont");

            //Loads Main Menu
            currState = new MainMenu(this, Content, graphics.GraphicsDevice);

            rankingManager = ScoreManager.LoadFile();

        }

        public void UpdateScores(int scores)
        {
            rankingManager.AddScore(new ScoresFile()
            {
                Value = scores,
                playerN = "Zeeshan",
            });

            ScoreManager.SaveFile(rankingManager);
        }

        protected override void Update(GameTime dt)
        {
            if (nextState != null)
            {
                currState = nextState;
                nextState = null;
            }

            currState.Update(dt);

            base.Update(dt);
        }



        public void ChangeCurrentState(State_Manager state)
        {
            nextState = state;
        }

        protected override void Draw(GameTime dt)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            currState.Draw(dt, spriteBatch);

            base.Draw(dt);
        }

        public void DrawHighscore(GameTime dt, SpriteBatch spriteB)
        {
            spriteB.DrawString(Font, string.Join("\n", rankingManager.Highscores.Select(a => a.playerN + ": " + a.Value).ToArray()), new Vector2(350, 150), Color.Blue);
        }
    }
}