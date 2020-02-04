using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class CubicBezierCurve : BezierCurve
    {
                
        public CubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {

            _p = new Vector2[4];
            _p[0] = p0;
            _p[1] = p1;
            _p[2] = p2;
            _p[3] = p3;

            CreateCurve();
        }

        public CubicBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int steps)
        {

            if (steps <= 0)
            {
                throw new Exception("steps must be >= 1");
            }

            _p = new Vector2[4];
            _p[0] = p0;
            _p[1] = p1;
            _p[2] = p2;
            _p[3] = p3;

            _steps = steps;

            CreateCurve();
        }
        
        protected override Vector2 Interpolate(float t)
        {

            var x = (Math.Pow(1 - t, 3) * _p[0].X) + (3 * Math.Pow(1 - t, 2) * t * _p[1].X) + (3 * (1 - t) * Math.Pow(t, 2) * _p[2].X) + (Math.Pow(t, 3) * _p[3].X);
            var y = (Math.Pow(1 - t, 3) * _p[0].Y) + (3 * Math.Pow(1 - t, 2) * t * _p[1].Y) + (3 * (1 - t) * Math.Pow(t, 2) * _p[2].Y) + (Math.Pow(t, 3) * _p[3].Y);

            return new Vector2((float)x, (float)y);

        }

    }
}
