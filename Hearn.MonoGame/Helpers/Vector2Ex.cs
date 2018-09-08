using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Helpers
{
    public class Vector2Ex
    {

        public static float Distance(Vector2 v0, Vector2 v1)
        {
            var dx = v1.X - v0.X;
            var dy = v1.Y - v0.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static float DotProduct(Vector2 v0, Vector2 v1)
        {
            return v0.X * v1.X + v0.Y * v1.Y;
        }

        public static Vector2 Normal(Vector2 v)
        {
            return new Vector2(v.Y, -v.X);            
        }


    }
}
