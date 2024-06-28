using Microsoft.Xna.Framework.Audio;
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
    class Coins
    {
        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect collect;

        public const int AddPoints = 15;
        public readonly Color Color = Color.Brown;

        private Vector2 basePos;
        private float bounce;

        public int Width
        {
            //get { return texture.Width; }
            get
            {
                // Return the width of the texture associated with the object
                return texture.Width;
            }
        }

        public int Height
        {
            //get { return texture.Height; }
            get
            {
                // Return the width of the texture associated with the object
                return texture.Height;
            }
        }

        public Level Level
        {
            //get { return level; }
            get 
            { 
                return level; 
            }
        }
        Level level;

        public Vector2 Position
        {
            get 
            {
                return basePos + new Vector2(0.0f, bounce);
            }
        }

        public Circle_Collisions BoundingCircle
        {

            get
            {
                // Calculate the radius as a third of the TileWidth
                float radius = Tiles.TileWidth / 3.0f;

                // Create a new Circle_Collisions object
                Circle_Collisions circle = new Circle_Collisions(Position, radius);

                return circle;
            }
        }

        public Coins(Level level, Vector2 position)
        {
            this.level = level;
            basePos = position;

            LoadResources();
        }

        public void LoadResources()
        {
            texture = Level.Content.Load<Texture2D>("Tiles/Others/Coin");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            collect = Level.Content.Load<SoundEffect>("Audio/CoinCollect");
        }

        public void CollectedCoins(Player collis)
        {
            collect.Play();
        }

        public void Update(GameTime gameTime)
        {
            //Bounce properties
            const float BounceH = 0.15f;
            const float BounceR = 2.0f;
            const float BounceSync = -0.75f;

            //Bounce along sin curve 
            double t = gameTime.TotalGameTime.TotalSeconds * BounceR + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceH * texture.Height;


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteB)
        {
            spriteB.Draw(texture, Position, null, Color, 0.0f, origin, 0.5f, SpriteEffects.None, 0.0f);
        }
    }
}
