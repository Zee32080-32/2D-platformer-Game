using Coursework_Retake.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    class Camera
    {
        public Matrix Transform { get; private set; }

        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 position;

        public void Follow(Level level)
        {
            //Transform = Matrix.CreateTranslation(-level.Player.Position.X - (level.Player.sprite.Width / 2), -level.Player.Position.Y - (level.Player.sprite.Height / 2), 0) * Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0);

            float playerX = -level.Player.Position.X - (level.Player.sprite.Width / 2);
            float playerY = -level.Player.Position.Y - (level.Player.sprite.Height / 2);
            float screenWidthHalf = Game1.ScreenWidth / 2;
            float screenHeightHalf = Game1.ScreenHeight / 2;

            Matrix translation1 = Matrix.CreateTranslation(playerX, playerY, 0);
            Matrix translation2 = Matrix.CreateTranslation(screenWidthHalf, screenHeightHalf, 0);

            Transform = translation1 * translation2;
        }
    }
}
