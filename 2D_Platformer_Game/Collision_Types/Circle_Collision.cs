using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    struct Circle_Collisions
    {
        public Vector2 Centre;
        public float Radius;

        public Circle_Collisions(Vector2 position, float radius)
        {
            Centre = position;
            Radius = radius;
        }

        public bool IntersectsWithRectangle(Rectangle rectangle)
        {
            float closestX = MathHelper.Clamp(Centre.X, rectangle.Left, rectangle.Right);
            float closestY = MathHelper.Clamp(Centre.Y, rectangle.Top, rectangle.Bottom);

            Vector2 closestPoint = new Vector2(closestX, closestY);
            Vector2 direction = Centre - closestPoint;
            float distanceSquared = direction.LengthSquared();

            return distanceSquared <= Radius * Radius;
        }
    }
}

