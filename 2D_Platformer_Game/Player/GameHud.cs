using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework_Retake.World;

namespace Coursework_Retake
{
    class GameHud
    {
        //Loads content Manager.
        public ContentManager Content
        {
            get 
            { 
                return content; 
            }
        }
        ContentManager content;

        //Level object
        private Level level;

        //Overlays
        private SpriteFont font;
        private Texture2D winLayer;
        private Texture2D lostLayer;
        private Texture2D diedLayer;
        private static readonly TimeSpan Warning = TimeSpan.FromSeconds(15);

        public void LoadContent()
        {
            //Loads layers for the HUD.
            winLayer = Content.Load<Texture2D>("HUD\\WinLayer");
            lostLayer = Content.Load<Texture2D>("HUD\\Lostlayer");
            diedLayer = Content.Load<Texture2D>("HUD\\DiedLayer");
            font = Content.Load<SpriteFont>("Font\\gameFont");
        }

        public void Draw(GameTime dt, SpriteBatch spriteBatch, int score)
        {
            Vector2 centre = new Vector2(level.Player.Position.X, level.Player.Position.Y);
            Vector2 Location = new Vector2(level.Player.Position.X - 380, level.Player.Position.Y - 250);
            Texture2D status = null;

            // Determine the status overlay message to show.
            if (level.TimeLeft == TimeSpan.Zero)
            {
                if (level.LevelCompleted)
                {
                    status = winLayer;
                }
                else
                {
                    status = lostLayer;
                }
            }
            else if (!level.Player.isAlive)
            {
                status = diedLayer;
            }

            if (status != null)
            {
                // Draw status message.
                Vector2 layerHud = new Vector2(level.Player.Position.X - 368, level.Player.Position.Y - 300);
                spriteBatch.Draw(status, layerHud, Color.White);
            }

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string timeString = "Time Remaining: " + level.TimeLeft.Minutes.ToString("00") + ":" + level.TimeLeft.Seconds.ToString("00");
            Color timeColor;
            if (level.TimeLeft > Warning ||
                level.LevelCompleted ||
                (int)level.TimeLeft.TotalSeconds % 2 == 0)
            {
                timeColor = Color.Blue;
            }
            else
            {
                timeColor = Color.Red;
            }

            DrawShadowedString(spriteBatch, font, timeString, centre + new Vector2(-350, -200), timeColor);

            // Draw score
            DrawShadowedString(spriteBatch, font, "Level Points: " + level.Score.ToString(), centre + new Vector2(-347, -180), Color.Cyan);
            DrawShadowedString(spriteBatch, font, "Total Points: " + score.ToString(), centre + new Vector2(-347, 230), Color.Yellow);

            spriteBatch.DrawString(font, "Health: " + level.Player.Health, centre + new Vector2(300, -200), Color.Red);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
