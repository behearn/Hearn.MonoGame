using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Extensions
{
    public class Vector2Ex
    {

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
