using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework_Retake.World;
using Coursework_Retake.items;

namespace Coursework_Retake
{
    class Health_Boost
    {

        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect healthpot;

        public const int AddPoints = 100;
        public readonly Color Color = Color.White;

        private Vector2 basePos;
        private float bounce;

        public int Width
        {
            get
            {
                // Return the width of the texture associated with the object
                return texture.Width;
            }
        }

        public int Height
        {
            get
            {
                // Return the width of the texture associated with the object
                return texture.Height;
            }
        }

        public Level Level
        {
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
                // Calculate the radius as one-third of the TileWidth
                float radius = Tiles.TileWidth / 3.0f;

                // Create a new Circle_Collisions object with the calculated radius and the current Position
                Circle_Collisions circle = new Circle_Collisions(Position, radius);

                // Return the created circle object
                return circle;
            }
        }

        public Health_Boost(Level level, Vector2 position)
        {
            this.level = level;
            basePos = position;

            LoadResources();
        }


        public void LoadResources()
        {
            texture = Level.Content.Load<Texture2D>("Tiles/Others/Health");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            healthpot = Level.Content.Load<SoundEffect>("Audio/healthpotion");
        }

        public void CollectedCoins(Player collis)
        {
            healthpot.Play();
        }

        public void Update(GameTime dt)
        {
            //Bounce properties
            const float BounceH = 0.15f;
            const float BounceR = 2.0f;
            const float BounceSync = -0.75f;

            //Bounce along sin curve 
            double t = dt.TotalGameTime.TotalSeconds * BounceR + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceH * texture.Height;
        }


        public void Draw(GameTime dt, SpriteBatch spriteB)
        {
            spriteB.Draw(texture, Position, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

    }
}
