using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class QuadraticBezierCurve : BezierCurve
    {
        
        public QuadraticBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            _p = new Vector2[3];
            _p[0] = p0;
            _p[1] = p1;
            _p[2] = p2;
            
            CreateCurve();
        }

        public QuadraticBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, int steps)
        {

            if (steps <= 0)
            {
                throw new Exception("steps must be >= 1");
            }

            _p = new Vector2[3];
            _p[0] = p0;
            _p[1] = p1;
            _p[2] = p2;

            _steps = steps;
            
            CreateCurve();
        }
        
        protected override Vector2 Interpolate(float t)
        {

            var x = (Math.Pow(1 - t, 2) * _p[0].X) + (2 * (1 - t) * t * _p[1].X) + (Math.Pow(t, 2) * _p[2].X);
            var y = (Math.Pow(1 - t, 2) * _p[0].Y) + (2 * (1 - t) * t * _p[1].Y) + (Math.Pow(t, 2) * _p[2].Y);

            return new Vector2((float)x, (float)y);

        }

    }
}
