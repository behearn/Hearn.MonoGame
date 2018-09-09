using Hearn.MonoGame.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public abstract class Polygon
    {

        protected Vector2 _location;

        protected Vector2[] _p;

        protected readonly int _numVerticies;

        private struct Projection
        {
            public Projection(float min, float max)
            {
                Min = min;
                Max = max;
            }
            public float Min;
            public float Max;
        }

        public Polygon(int verticies)
        {
            _numVerticies = verticies;
            _p = new Vector2[verticies];
        }

        public Vector2[] Verticies { get => _p; }

        public Vector2[] VerticiesClosed { get => _p.Concat(_p.Take(1)).ToArray(); }

        public Vector2[] GetEdgeNormals()
        {
            var normals = new List<Vector2>();
            for (var i = 0; i < _numVerticies; i++)
            {
                var p0 = _p[i];
                var p1 = _p[(i + 1) % _numVerticies];
                var edge = p0 - p1;
                var normal = Vector2Ex.Normal(edge);
                normal.Normalize();
                if (!normals.Any(n => Math.Abs(Vector2Ex.DotProduct(n, normal)) == 1f))
                {
                    normals.Add(normal);
                }
            }
            return normals.ToArray();
        }

        public bool Intersects(Polygon p)
        {
            return Intersects(p, out Vector2 penetration);
        }
        
        public bool Intersects(Polygon p, out Vector2 penetration)
        {
            //Separating Axis Theorom Implementation
            //Ported from Love2D Tutorial

            var axes0 = GetEdgeNormals();
            var axes1 = p.GetEdgeNormals();
            var axes = axes0.Union(axes1).ToArray();
            
            var overlap = float.MaxValue;

            penetration = new Vector2(0f / 0f);
            
            for (var i = 0; i < axes.Length; i++)
            {
                var axis = axes[i];
                var proj0 = Project(this, axis);
                var proj1 = Project(p, axis);

                if (proj0.Min > proj1.Max || proj1.Min > proj0.Max)
                {
                    penetration = new Vector2(0f / 0f);
                    return false;
                }
                else
                {
                    var o = Math.Min(proj0.Max, proj1.Max) - Math.Max(proj0.Min, proj1.Min);
                    if (o < overlap)
                    {
                        overlap = o;
                        penetration = axis;
                    }
                }
            }

            var overlapVector = new Vector2(overlap);
            penetration = penetration * overlapVector;

            //Still not quite right when colliding at bottom right
            var separationVector = _location - p._location;
            if (Math.Sign(separationVector.X) != Math.Sign(overlapVector.X))
            {
                penetration.X *= -1;
            }

            if (Math.Sign(separationVector.Y) == Math.Sign(overlapVector.Y))
            {
                penetration.Y *= -1;
            }

            return true;
        }

        private Projection Project(Polygon polygon, Vector2 axis)
        {
            var dp = polygon.Verticies.Select(v => Vector2Ex.DotProduct(axis, v));
            return new Projection(dp.Min(), dp.Max());
        }

    }
}
