using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    class Circle
    {
        public Vector2 Location { get; set; }
        public float Radius { get; set; }

        public bool Intersects(Circle c)
        {
            return Maths.Distance(c.Location, Location) <= c.Radius + Radius;           
        }

        public bool Intersects(Vector2 v)
        {
            return Maths.Distance(v, Location) <= Radius;
        }

    }
}
