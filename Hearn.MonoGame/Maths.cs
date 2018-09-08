using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame
{
    public class Maths
    {

        public static float Distance(Vector2 v1, Vector2 v2)
        {
            var dx = v2.X - v1.X;
            var dy = v2.Y - v1.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

    }
}
