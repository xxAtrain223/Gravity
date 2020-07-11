using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Extensions
{
    public static class Vector2fExtentions
    {
        public static double Length(this Vector2f vector)
        {
            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        public static Vector2f Normalize(this Vector2f vector)
        {
            var length = vector.Length();

            return new Vector2f
            {
                X = (float)(vector.X / length),
                Y = (float)(vector.Y / length)
            };
        }

        public static Vector2f Scale(this Vector2f vector, double scale)
        {
            return new Vector2f
            {
                X = (float)(vector.X * scale),
                Y = (float)(vector.Y * scale)
            };
        }
    }
}
