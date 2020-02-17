using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class MultiPointBezierCurve : BezierCurve
    {

        public MultiPointBezierCurve(Vector2[] p)
        {

            _p = p;

            CreateCurve();
        }

        public MultiPointBezierCurve(Vector2[] p, int steps)
        {

            if (steps <= 0)
            {
                throw new Exception("steps must be >= 1");
            }

            _p = p;

            _steps = steps;

            CreateCurve();
        }
        
        protected override Vector2 Interpolate(float t)
        {

            var n = _p.Length - 1;

            var x = 0f;
            var y = 0f;

            for (var i = 0; i <= n; i++)
            {

                //binomial coefficient
                var bc = Factorial(n) / (Factorial(n - i) * Factorial(i));

                x += Calculate(bc, n, i, _p[i].X, t);
                y += Calculate(bc, n, i, _p[i].Y, t);

            }

            return new Vector2(x, y);

        }

        private float Calculate(int bc, int n, int i, float p, float t)
        {

            return (float)(bc * Math.Pow(1 - t, n - i) * Math.Pow(t, i) * p);

        }

        private int Factorial(int n)
        {
            var v = n == 0 ? 1 : n;
            while (n > 1)
            {
                v = v * (n - 1);
                n--;
            }
            return v;
        }

    }
}
