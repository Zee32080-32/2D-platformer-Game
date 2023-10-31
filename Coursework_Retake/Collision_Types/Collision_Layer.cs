using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake.Collision_Types
{
    internal class Collision_Layer
    {
        public void Draw_Lines(SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 startPoint, Vector2 endPoint, int lineWidth)
        {

            float angle = (float)Math.Atan2(startPoint.Y - endPoint.Y, startPoint.X - endPoint.X);
            float length = Vector2.Distance(startPoint, endPoint);

            spriteBatch.Draw(texture, endPoint, new Rectangle((int)endPoint.X, (int)endPoint.Y, (int)length, 3),
                color, angle, Vector2.Zero, lineWidth, SpriteEffects.None, 0);
        }


        public void Rectangle(Color color, Rectangle rect, int lineWidth, SpriteBatch spriteBatch, Texture2D texture)
        {
            Vector2[] vertices = new Vector2[4];
            vertices[0] = new Vector2(rect.Left, rect.Top);
            vertices[1] = new Vector2(rect.Right, rect.Top);
            vertices[2] = new Vector2(rect.Right, rect.Bottom);
            vertices[3] = new Vector2(rect.Left, rect.Bottom);

            Bounding_Box_Polygons(color, vertices, lineWidth, spriteBatch, texture);
        }

        public void Circles(Color color, Vector2 centre, float radius, int lineWidth, SpriteBatch spriteBatch, Texture2D texture, int segments = 16)
        {

            Vector2[] vertices = new Vector2[segments];

            double add = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for (int i = 0; i < segments; i++)
            {
                vertices[i] = centre + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += add;
            }

            Bounding_Box_Polygons(color, vertices, lineWidth, spriteBatch, texture);
        }

        //Polygons for the Bounding Box
        public void Bounding_Box_Polygons(Color color, Vector2[] vertices, int lineWidth, SpriteBatch spriteBatch, Texture2D texture)
        {

            int count = vertices.Length;
            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    Draw_Lines(spriteBatch, texture, color, vertices[i], vertices[i + 1], lineWidth);
                }
                Draw_Lines(spriteBatch, texture, color, vertices[count - 1], vertices[0], lineWidth);
            }
        }



    }
}
