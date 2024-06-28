using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Coursework_Retake.Collision_Types;
using Coursework_Retake.Game_Controls;
using Coursework_Retake.World;

namespace Coursework_Retake
{

    class Game_State : State_Manager
    {
        //Sets some object. 
        Camera camera;
        Player player;
        //Background
        Texture2D Backtext;
        Background background;

        //Levels
        private int levelIndex = -1;
        public Level level;
        private const int numOfLevels = 10;
        private bool Continuebut;

        //Inputs
        private KeyboardState ks;
        private MouseState ms;

        //HUD
        private SpriteFont font;
        private Texture2D winLayer;
        private Texture2D lostLayer;
        private static readonly TimeSpan Warning = TimeSpan.FromSeconds(15);

        //Saves  score 
        public int score;

        //Powerup
        public bool hasBoughtpowerup;

        //Object for bounding boxes
        Collision_Layer Collision_Layer = new Collision_Layer();
        Texture2D outlayer;
        private bool isLayersOn;

        //Command Manager for bindings
        Command_Manager Command_Manager = new Command_Manager();

        public Game_State(Game1 g, ContentManager cm, GraphicsDevice gd) : base(g, cm, gd)
        {
            camera = new Camera();
            background = new Background();

            //Loads layers for the HUD.
            winLayer = content.Load<Texture2D>("HUD\\WinLayer");
            lostLayer = content.Load<Texture2D>("HUD\\Lostlayer");
            font = content.Load<SpriteFont>("Font\\gameFont");

            Backtext = content.Load<Texture2D>("Background\\BG1");
            background.Load_Content(Backtext, graphics);

            LoadLevel();

            outlayer = content.Load<Texture2D>("HUD\\boundingBox");

            isLayersOn = true;

            Bindings();
        }

        //Loads the levels. 
        private void LoadLevel()
        {
            levelIndex = (levelIndex + 1) % numOfLevels;

            if (level != null)
                level.Clear();

            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream filestream = TitleContainer.OpenStream(levelPath))
                level = new Level(game.Services, filestream, levelIndex);

            SetLevelStatus(levelIndex);


            //levelIndex = (levelIndex + 1) % numOfLevels;
            //level?.Clear();
            //string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            //using (Stream filestream = TitleContainer.OpenStream(levelPath))
            //{
            //    level = new Level(game.Services, filestream, levelIndex);
            //}

            //SetLevelStatus(levelIndex);
            //levelIndex = (levelIndex + 1) % numOfLevels;

        }

        //Sets which level is currently played based on the Index given. 
        private void SetLevelStatus(int Index)
        {
            if (Index == 0)
            {
                level.isLevel1 = true;
                level.isLevel2 = false;
                level.isLevel3 = false;
                level.isLevel4 = false;
                level.isLevel5 = false;
                level.isLevel6 = false;
                level.isLevel7 = false;
                level.isLevel8 = false;
                level.isLevel9 = false;
                level.isBoss = false;
            }
            else if (Index == 1)
            {
                level.isLevel1 = false;
                level.isLevel2 = true;
                level.isLevel3 = false;
                level.isLevel4 = false;
                level.isLevel5 = false;
                level.isLevel6 = false;
                level.isLevel7 = false;
                level.isLevel8 = false;
                level.isLevel9 = false;
                level.isBoss = false;
            }
            else if (Index == 2)
            {
                level.isLevel1 = false;
                level.isLevel2 = false;
                level.isLevel3 = true;
                level.isLevel4 = false;
                level.isLevel5 = false;
                level.isLevel6 = false;
                level.isLevel7 = false;
                level.isLevel8 = false;
                level.isLevel9 = false;
                level.isBoss = false;
            }
            else if (Index == 3)
            {
                level.isLevel1 = false;
                level.isLevel2 = false;
                level.isLevel3 = false;
                level.isLevel4 = false;
                level.isLevel5 = false;
                level.isLevel6 = false;
                level.isLevel7 = false;
                level.isLevel8 = false;
                level.isLevel9 = false;
                level.isBoss = true;
            }
            else if (Index == 4)
            {
                level.isLevel1 = false;
                level.isLevel2 = false;
                level.isLevel3 = false;
                level.isLevel4 = false;
                level.isLevel5 = true;
                level.isLevel6 = false;
                level.isLevel7 = false;
                level.isLevel8 = false;
                level.isLevel9 = false;
                level.isBoss = false;
            }
            else if (Index == 5)
            {
                level.isLevel1 = false;
                level.isLevel2 = false;
                level.isLevel3 = false;
                level.isLevel4 = false;
                level.isLevel5 = false;
                level.isLevel5 = false;
                level.isLevel6 = true;
                level.isBoss = false;
            }
          
            else
            {
                level.isLevel1 = true;
                level.isLevel2 = false;
                level.isLevel3 = false;
                level.isLevel4 = false;
                level.isLevel5 = false;
                level.isLevel6 = false;
                level.isBoss = false;
            }
        }

        private void StateCheck(GameTime gameTime)
        {
            //Button to pass onto the next level
            bool continuelevel = ks.IsKeyDown(Keys.P);

            if (!Continuebut && continuelevel)
            {
                if (!level.Player.isAlive)
                {
                    level.Restart();
                }
                if (level.TimeLeft == TimeSpan.Zero)
                {
                    level.Restart();
                }
                if (level.LevelCompleted)
                {
                    if (level.Player.Health > 3)
                    {
                        int a = level.Player.Health - 3;
                        score += a * 100;
                    }
                    score += level.Score;
                    LoadLevel();
                }
            }
            Continuebut = continuelevel;


        }

        private void Bindings()
        {
            //Player Movement
            Command_Manager.AddKeyboardBinding(Keys.A, Left); 
            Command_Manager.AddKeyboardBinding(Keys.D, Right); //Moves to the right
            Command_Manager.AddKeyboardBinding(Keys.Space, Jump);

            //Back Button
            Command_Manager.AddKeyboardBinding(Keys.Escape, BackButton);

            //Debugging Purposes
            Command_Manager.AddKeyboardBinding(Keys.L, Layers); //Turns on/off bounding box layers

            Command_Manager.AddKeyboardBinding(Keys.S, RandPowerUp);
        }

        public void Layers(Button_States button, Vector2 amount)
        {
            if (button == Button_States.UP)
            {
                Layers();
            }
        }

        public void Left(Button_States button, Vector2 amount)
        {
            if (button == Button_States.DOWN)
            {
                level.Player.Speed = -1;
            }
            else
            {
                level.Player.Speed = 0;
            }
        }

        public void Right(Button_States button, Vector2 amount)
        {
            if (button == Button_States.DOWN)
            {
                level.Player.Speed = 1;
            }
            else
            {
                level.Player.Speed = 0;
            }
        }

        public void Jump(Button_States button, Vector2 amount)
        {
            if (button == Button_States.DOWN)
            {
                level.Player.isJumping = true;
            }
            else
            {
                level.Player.isJumping = false;
            }
        }

        public void BackButton(Button_States button, Vector2 amount)
        {
            if (button == Button_States.DOWN)
            {
                game.ChangeCurrentState(new MainMenu(game, content, graphics));
            }
            else
            {

            }
        }

        public void RandPowerUp(Button_States button, Vector2 amount)
        {
            if (button == Button_States.DOWN && level.Player.hasBought == false)
            {
                level.Player.hasBoughtpowerup = true;
            }
            else
            {

            }
        }

        public void Layers()
        {
            if (!isLayersOn)
            {
                isLayersOn = true;
            }
            else
            {
                isLayersOn = false;
            }
        }

        public override void Unload(GameTime dt)
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            //Uses timepass float for the scrolling background. 
            float Timepass = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ks = Keyboard.GetState();

            level.Update(gameTime, ks, game.Window.CurrentOrientation);

            StateCheck(gameTime);

            //Sets camera position using Matrix depending on the player. 
            camera.Follow(level);

            if (level.isLevel1)
                background.Update(Timepass * 3);

            //Checks whether the game finishes
            if (level.isBoss && level.LevelCompleted)
            {
                game.UpdateScores(score);
                game.ChangeCurrentState(new Won_State(game, content, graphics));
            }
            else if(!level.isBoss && level.LevelCompleted)
            {

                //game.ChangeCurrentState(new Won_State(game, content, graphics));
            }

            if (level.Player.Health <= 0)
            {
                level.Player.Kill(null);
            }

            Command_Manager.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteB)
        {
            spriteB.Begin(transformMatrix: camera.Transform);

            //Sets Rolling & static background depending on the current level loaded in. 
            if (level.isLevel1)
                background.Draw(spriteB);
            else
            {
                spriteB.Draw(Backtext, new Rectangle((int)camera.Position.X, (int)camera.Position.Y - 350, 1920, 1080), Color.White);
                spriteB.Draw(Backtext, new Rectangle((int)camera.Position.X - 1920, (int)camera.Position.Y - 350, 1920, 1080), Color.White);
                spriteB.Draw(Backtext, new Rectangle((int)camera.Position.X + 1920, (int)camera.Position.Y - 350, 1920, 1080), Color.White);
            }

            //Draws from level including the player. 
            level.Draw(gameTime, spriteB);
            //Player.Draw(spriteBatch);

            //Draws HUD. 
            Hud(spriteB);

            if (isLayersOn)
                BoundingBoxes(spriteB);

            spriteB.End();
        }

        private void Hud(SpriteBatch spriteB)
        {
            Vector2 center = new Vector2(level.Player.Position.X, level.Player.Position.Y);
            Vector2 statusLocation = new Vector2(level.Player.Position.X - 368, level.Player.Position.Y - 210);
            Texture2D statusTexture = null;

            // Determine the status overlay message to show.
            if (level.TimeLeft == TimeSpan.Zero)
            {
                if (level.LevelCompleted)
                {
                    statusTexture = winLayer;
                }
                else
                {
                    statusTexture = lostLayer;
                }
            }
            else if (!level.Player.isAlive)
            {
                game.ChangeCurrentState(new Lost_State(game, content, graphics));
            }


            if (statusTexture != null)
            {
                // Draw status message.
                spriteB.Draw(statusTexture, statusLocation, Color.White);
            }


            // Draw time remaining. Uses modulo division to cause blinking when the player is running out of time.
            string timeString = "Time Left: " + level.TimeLeft.ToString(@"mm\:ss");
            Color timeColor = (level.TimeLeft > Warning || level.LevelCompleted || (int)level.TimeLeft.TotalSeconds % 2 == 0) ? Color.Cyan : Color.Red;

            //string powerUpTimeString = "Power-up Time Left: " + level.CountDownTimer.ToString(@"mm\:ss");
            string HealthPotionString = "Health Potion Collected!";
            string TimePotionString = "Time Potion Coleected!";
            string InvisibilityPotionString = "Invisibility Potion Collected! ";


            DrawShadowedString(font, timeString, center + new Vector2(-350, -200), timeColor, spriteB);

            if (level.CountDownTimer > TimeSpan.Zero && level.RandPotion == 0)
            {
                DrawShadowedString(font, HealthPotionString, center + new Vector2(0, 230), Color.Red, spriteB);
            }
            if (level.CountDownTimer > TimeSpan.Zero && level.RandPotion == 1)
            {
                DrawShadowedString(font, TimePotionString, center + new Vector2(0, 230), Color.Blue, spriteB);
            }
            if (level.CountDownTimer > TimeSpan.Zero && level.RandPotion == 2)
            {
                DrawShadowedString(font, InvisibilityPotionString, center + new Vector2(0, 230), Color.Green, spriteB);
            }

            // Draw score
            DrawShadowedString(font, "Level Points: " + level.Score.ToString(), center + new Vector2(-347, -180), Color.Cyan, spriteB);
            DrawShadowedString(font, "Total Points: " + score.ToString(), center + new Vector2(-347, 230), Color.Yellow, spriteB);

            spriteB.DrawString(font, "Health: " + level.Player.Health, center + new Vector2(300, -200), Color.Red);
        }



        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color, SpriteBatch spriteB)
        {
            spriteB.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteB.DrawString(font, value, position, color);
        }
        void BoundingBoxes(SpriteBatch spriteB)
        {
            //Player
            Collision_Layer.Rectangle(Color.Green, level.Player.BoundingRect, 1, spriteB, outlayer);

            // Enemies
            for (int i = 0; i < level.enemy_1.Count; i++)
            {
                var enemy = level.enemy_1[i];
                Collision_Layer.Rectangle(Color.Blue, enemy.Bounding_Rectangle, 1, spriteB, outlayer);
            }

            // Coins
            for (int i = 0; i < level.coins.Count; i++)
            {
                var coin = level.coins[i];
                Collision_Layer.Circles(Color.Green, coin.Position, coin.Width / 4, 1, spriteB, outlayer);
            }

            // Diamonds
            for (int i = 0; i < level.diamonds.Count; i++)
            {
                var diamond = level.diamonds[i];
                Collision_Layer.Circles(Color.Green, diamond.Position, diamond.Width / 2, 1, spriteB, outlayer);
            }

            for (int i = 0; i < level.randomPowerUp.Count; i++)
            {
                var shop = level.randomPowerUp[i];
                Collision_Layer.Circles(Color.Green, shop.Position, shop.Width / 2, 1, spriteB, outlayer);
            }

            // Health Packs
            for (int i = 0; i < level.HealthPotions.Count; i++)
            {
                var healthPotion = level.HealthPotions[i];
                Collision_Layer.Circles(Color.Green, healthPotion.Position, healthPotion.Width / 2, 1, spriteB, outlayer);
            }

            // Time Potions
            for (int i = 0; i < level.TimePotions.Count; i++)
            {
                var timePotion = level.TimePotions[i];
                Collision_Layer.Circles(Color.Green, timePotion.Position, timePotion.Width / 2, 1, spriteB, outlayer);
            }

            for (int i = 0; i < level.InvisibilityPotions.Count; i++)
            {
                var InvisiblePotion = level.InvisibilityPotions[i];
                Collision_Layer.Circles(Color.Green, InvisiblePotion.Position, InvisiblePotion.Width / 2, 1, spriteB, outlayer);
            }
        }
    }
}
