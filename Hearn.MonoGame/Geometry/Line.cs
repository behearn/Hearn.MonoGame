using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    class Line
    {

        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public Vector2 Intersects(Line l)
        {
            return Intersects(l, false);
        }

        public Vector2 Intersects(Line l, bool extend)
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
                    return new Vector2()
                    {
                        X = intersectX,
                        Y = intersectY
                    };
                }

                var rx0 = (intersectX - Start.X) / (End.X - Start.X);
                var ry0 = (intersectY - Start.Y) / (End.Y - Start.Y);

                var rx1 = (intersectX - l.Start.X) / (l.End.X - l.Start.X);
                var ry1 = (intersectY - l.Start.Y) / (l.End.Y - l.Start.Y);

                if (((rx0 >= 0 && rx0 <= 1) || (ry0 >= 0 && ry0 <= 1)) &&
                    ((rx1 >= 0 && rx1 <= 1) || (ry1 >= 0 && ry1 <= 1)))
                {
                    return new Vector2()
                    {
                        X = intersectX,
                        Y = intersectY
                    };
                }
            }

            return new Vector2(0f / 0f);

        }
        
    }
}
