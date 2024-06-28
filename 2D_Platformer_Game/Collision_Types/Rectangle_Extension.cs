using System;
using Microsoft.Xna.Framework;

namespace Coursework_Retake
{
    public static class RectangleExtensions
    {
        public static Vector2 Intersection_Depth(this Rectangle a, Rectangle b)
        {
            float Half_Width_A = a.Width / 2.0f;
            float Half_Height_A = a.Height / 2.0f;

            float Half_Width_B = b.Width / 2.0f;
            float Half_Height_B = b.Height / 2.0f;

            Vector2 Centre_Of_Rectangle_A = new Vector2(a.Left + Half_Width_A, a.Top + Half_Height_A);
            Vector2 Centre_Of_Rectangle_B = new Vector2(b.Left + Half_Width_B, b.Top + Half_Height_B);

            //Calculates current non-intersecting distances between centers.
            float dist_X = Centre_Of_Rectangle_A.X - Centre_Of_Rectangle_B.X;
            float dist_Y = Centre_Of_Rectangle_A.Y - Centre_Of_Rectangle_B.Y;

            //Calculates minimum non-intersecting distances between centers.
            float min_Non_Intersect_Dist_X = Half_Width_A + Half_Width_B;
            float min_Non_Intersect_Dist_Y = Half_Height_A + Half_Height_B;

            //If there is no intersection, program will return (0,0)
            if (Math.Abs(dist_X) >= min_Non_Intersect_Dist_X || Math.Abs(dist_Y) >= min_Non_Intersect_Dist_Y)
                return Vector2.Zero;

            //Calculates intersection depths
            float Intersection_Depth_X = dist_X > 0 ? min_Non_Intersect_Dist_X - dist_X : -min_Non_Intersect_Dist_X - dist_X;
            float Intersection_Depth_Y = dist_Y > 0 ? min_Non_Intersect_Dist_Y - dist_Y : -min_Non_Intersect_Dist_Y - dist_Y;

            return new Vector2(Intersection_Depth_X, Intersection_Depth_Y);
        }

        public static Vector2 BottomCentre(this Rectangle a)
        {
            return new Vector2(a.X + a.Width / 2.0f, a.Bottom);
        }
    }

}
