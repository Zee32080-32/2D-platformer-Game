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
    class RandomPowerUp
    {

        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect Win;

        private Vector2 basePos;
        private float bounce;
        public readonly Color Color = Color.Red;

        public int Width
        {
            get
            {
                return texture.Width;
            }
        }

        public int Height
        {
            get
            {
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
                float radius = Tiles.TileWidth / 3.0f;

                Circle_Collisions circle = new Circle_Collisions(Position, radius);

                return circle;
            }
        }

        public RandomPowerUp(Level level, Vector2 position)
        {
            this.level = level;
            basePos = position;

            LoadResources();
        }

        public void LoadResources()
        {
            texture = Level.Content.Load<Texture2D>("Tiles/Others/Chest");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            Win = Level.Content.Load<SoundEffect>("Audio/Win");
        }

        public void GotRandomPowerUp(Player collis)
        {
            Win.Play();
        }

        public void Update(GameTime dt)
        {

        }

        public void Draw(GameTime dt, SpriteBatch spriteB)
        {
            spriteB.Draw(texture, Position, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }

}

