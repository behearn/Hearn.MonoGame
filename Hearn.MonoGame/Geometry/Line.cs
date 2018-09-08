using Hearn.MonoGame.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class Line
    {

        public Line()
        {
        }

        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public float Length { get => Math.Abs(Vector2Ex.Distance(Start, End)); }

        public bool Intersects(Line l)
        {
            return CalculateIntersection(l, false, out Vector2 v);
        }

        public Vector2 IntersectsAt(Line l)
        {
            CalculateIntersection(l, false, out Vector2 v);
            return v;
        }

        public Vector2 IntersectPoint(Line l)
        {
            CalculateIntersection(l, true, out Vector2 v);
            return v;
        }

        private bool CalculateIntersection(Line l, bool extend, out Vector2 intersectVector)
        {

            //Coding Math Episode 32/34
            
            var a1 = End.Y - Start.Y;
            var b1 = Start.X - End.X;
            var c1 = a1 * Start.X + b1 * Start.Y;

            var a2 = l.End.Y - l.Start.Y;
            var b2 = l.Start.X - l.End.X;
            var c2 = a2 * l.Start.X + b2 * l.Start.Y;

            var denominator = a1 * b2 - a2 * b1;

            if (denominator != 0)
            {

                var intersectX = (b2 * c1 - b1 * c2) / denominator;
                var intersectY = (a1 * c2 - a2 * c1) / denominator;

                if (extend)
                {
                    intersectVector = new Vector2()
                    {
                        X = intersectX,
                        Y = intersectY
                    };
                    return true;
                }

                var rx0 = (intersectX - Start.X) / (End.X - Start.X);
                var ry0 = (intersectY - Start.Y) / (End.Y - Start.Y);

                var rx1 = (intersectX - l.Start.X) / (l.End.X - l.Start.X);
                var ry1 = (intersectY - l.Start.Y) / (l.End.Y - l.Start.Y);

                if (((rx0 >= 0 && rx0 <= 1) || (ry0 >= 0 && ry0 <= 1)) &&
                    ((rx1 >= 0 && rx1 <= 1) || (ry1 >= 0 && ry1 <= 1)))
                {
                    intersectVector = new Vector2()
                    {
                        X = intersectX,
                        Y = intersectY
                    };
                    return true;
                }
            }

            intersectVector = new Vector2(0f / 0f);
            return false;

        }
        
    }
}
