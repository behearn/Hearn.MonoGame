using Hearn.MonoGame.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var intersections = IntersectPoints(l);
            return intersections.Any();
        }

        public bool Collides(Polygon p)
        {
            //Circle ecompasses polygon
            foreach (var v in p.Verticies)
            {
                if (Collides(v))
                {
                    return true;
                }
            }

            //Polygon ecompasses circle
            if (p.Collides(Location))
            {
                return true;
            }

            //Circle intersects edge of polygon
            if (IntersectPoints(p).Any())
            {
                return true;
            }
            return false;
        }

        public List<Vector2> IntersectPoints(Polygon p)
        {
            var allIntersections = new List<Vector2>();
            foreach (var l in p.Lines)
            {
                var intersections = IntersectPoints(l);
                allIntersections.AddRange(intersections);
            }
            return allIntersections;
        }

        public List<Vector2> IntersectPoints(Line l)
        {
            
            //Weisstein, Eric W. "Circle-Line Intersection." From MathWorld--A Wolfram Web Resource. 
            //http://mathworld.wolfram.com/Circle-LineIntersection.html 
            //Except 
            //1) sgn(x) function has been modified to work with horizontal lines
            //2) Added condition to constrain to bounds of the line

            var intersections = new List<Vector2>();

            var x1 = (int)(l.Start.X - Location.X);
            var y1 = (int)(l.Start.Y - Location.Y);

            var x2 = (int)(l.End.X - Location.X);
            var y2 = (int)(l.End.Y - Location.Y);

            var dx = x2 - x1;
            var dy = y2 - y1;
            var dr = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            var d = (x1 * y2) - (x2 * y1);

            var discriminant = (Math.Pow(Radius, 2) * Math.Pow(dr, 2)) - Math.Pow(d, 2);            
            if (discriminant >= 0)
            {
                // Note: NOT the same as reference which states x < 0 = -1 (which is how Math.Sign works also)
                var sgn = new Func<float, int>(x => x <= 0 ? -1 : 1); 

                var ix1 = ((d * dy) + ((sgn(dy) * dx) * Math.Sqrt(discriminant))) / Math.Pow(dr, 2);
                var iy1 = ((-d * dx) + (Math.Abs(dy) * Math.Sqrt(discriminant))) / Math.Pow(dr, 2);

                if (ix1 >= Math.Min(x1, x2) && ix1 <= Math.Max(x1, x2)
                    && iy1 >= Math.Min(y1, y2) && iy1 <= Math.Max(y1, y2))
                {
                    intersections.Add(new Vector2((int)ix1 + Location.X, (int)iy1 + Location.Y));
                }

                var ix2 = ((d * dy) - ((sgn(dy) * dx) * Math.Sqrt(discriminant))) / Math.Pow(dr, 2);
                var iy2 = ((-d * dx) - (Math.Abs(dy) * Math.Sqrt(discriminant))) / Math.Pow(dr, 2);

                if (ix2 >= Math.Min(x1, x2) && ix2 <= Math.Max(x1, x2)
                    && iy2 >= Math.Min(y1, y2) && iy2 <= Math.Max(y1, y2))
                {
                    intersections.Add(new Vector2((int)ix2 + Location.X, (int)iy2 + Location.Y));
                }

            }

            return intersections;

        }
    }
}
