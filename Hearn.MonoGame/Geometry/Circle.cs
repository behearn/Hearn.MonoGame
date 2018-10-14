using Hearn.MonoGame.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class Circle
    {
        public Vector2 Location { get; set; }
        public float Radius { get; set; }

        public Circle()
        {
        }

        public Circle(Vector2 location, float radius)
        {
            Location = location;
            Radius = radius;
        }

        public bool Collides(Circle c)
        {
            return Vector2Ex.Distance(c.Location, Location) <= c.Radius + Radius;           
        }

        public bool Collides(Vector2 v)
        {
            return Vector2Ex.Distance(v, Location) <= Radius;
        }

        public bool Collides(Line l)
        {

            //http://mathworld.wolfram.com/Circle-LineIntersection.html

            var x1 = l.Start.X - Location.X;
            var y1 = l.Start.Y - Location.Y;

            var x2 = l.End.X - Location.X;
            var y2 = l.End.Y - Location.Y;

            var dx = x2 - x1;
            var dy = y2 - y1;
            var dr = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            var d = (x1 * y2) - (x2 * y1);

            var incidence = (Math.Pow(Radius, 2) * Math.Pow(dr, 2)) - Math.Pow(d, 2);

            if (incidence >= 0)
            {

                var ix1 = ((d * dy) + ((Math.Sign(dy) * dx) * Math.Sqrt(incidence))) / Math.Pow(dr, 2);
                var iy1 = ((-d * dx) + (Math.Abs(dy) * Math.Sqrt(incidence))) / Math.Pow(dr, 2);

                if (ix1 >= Math.Min(x1, x2) && ix1 <= Math.Max(x1, x2)
                    && iy1 >= Math.Min(y1, y2) && iy1 <= Math.Max(y1, y2))
                {
                    return true;
                }

                var ix2 = ((d * dy) - ((Math.Sign(dy) * dx) * Math.Sqrt(incidence))) / Math.Pow(dr, 2);
                var iy2 = ((-d * dx) - (Math.Abs(dy) * Math.Sqrt(incidence))) / Math.Pow(dr, 2);

                if (ix2 >= Math.Min(x1, x2) && ix2 <= Math.Max(x1, x2)
                    && iy2 >= Math.Min(y1, y2) && iy2 <= Math.Max(y1, y2))
                {
                    return true;
                }

            }

            return false;

        }
    }
}
