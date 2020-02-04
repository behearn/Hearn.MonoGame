using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public abstract class BezierCurve : Path
    {

        const int DefaultSteps = 100;

        protected int _steps;

        protected Vector2[] _p;

        public BezierCurve()
        {
            _steps = DefaultSteps;
        }

        protected void CreateCurve()
        {

            Vertices = new Vector2[_steps + 1];

            var delta = 1f / _steps;

            for (var i = 0; i <= _steps; i++)
            {
                var t = i * (1f / _steps);
                Vertices[i] = Interpolate(t);
            }

        }
      
        protected abstract Vector2 Interpolate(float t);

        public Vector2 GetControlPoint(int index)
        {
            if (index < 0 || index >= _p.Length)
            {
                throw new IndexOutOfRangeException($"index must be in the range 0..{_p.Length - 1}");
            }

            return _p[index];
        }

        public void SetControlPoint(int index, Vector2 p)
        {

            if (index < 0 || index >= _p.Length)
            {
                throw new IndexOutOfRangeException($"index must be in the range 0..{_p.Length - 1}");
            }

            _p[index] = p;

            CreateCurve();

        }

    }
}
