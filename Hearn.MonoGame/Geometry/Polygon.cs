using Hearn.MonoGame.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class Polygon
    {

        private Vector2 _location;
        private Vector2 _lastLocation;

        private Vector2 _originVector;
        private bool _centerOrigin;

        private readonly int _numVerticies;

        private float _angle;

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

        /// <summary>
        /// Creates a polygon with the specified number of verticies
        /// </summary>
        /// <param name="verticies"></param>
        public Polygon(Vector2[] verticies)
        {
            if (verticies == null)
            {
                throw new ArgumentException("verticies required");
            }
            if (verticies.Length < 3)
            {
                throw new ArgumentException("Minimum number of verticies is 3");
            }
            _numVerticies = verticies.Length;
            Verticies = verticies;
            Lines = new Line[_numVerticies];
            for (var i = 0; i < Verticies.Length; i++)
            {
                Lines[i] = new Line();
            }
            Recalculate();
        }

        /// <summary>
        /// Top left location 
        /// </summary>
        public Vector2 Location
        {
            get => _location;
            set
            {
                _location = value;
                Recalculate();
            }
        }

        /// <summary>
        /// Distance from location to rotate around.  Use CenterOrigin to keep it centered on the bounding rectangle.
        /// </summary>
        public Vector2 OriginVector
        {
            get => _originVector;
            set
            {
                if (_centerOrigin)
                {
                    throw new Exception("Origin cannot be set when centered");
                }
                _originVector = value;
            }
        }

        /// <summary>
        /// Rotation angle in degrees 
        /// </summary>
        public float Angle
        {
            get => _angle;
            set
            {
                if (value != _angle)
                {
                    _angle = value;
                    Recalculate();
                }
            }
        }

        /// <summary>
        /// Keeps Origin centered around the bounding rectangle
        /// </summary>
        public bool CenterOrigin
        {
            get => _centerOrigin;
            set
            {
                _centerOrigin = value;
                Recalculate();
            }
        }

        public Line[] Lines { get; }

        /// <summary>
        /// Verticies listed in clockwise order
        /// </summary>
        public Vector2[] Verticies { get; }

        /// <summary>
        /// Verticies starting and finishing at the same point to give a closed path
        /// </summary>
        public Vector2[] VerticiesClosed { get => Verticies.Concat(Verticies.Take(1)).ToArray(); }

        /// <summary>
        /// Determines if point v is within the polygon by ray casting on the x and y axis and counting edges
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Collides(Vector2 v)
        {

            //Cast vertical and horizontal lines from the vector passed in
            var rayCastX = new Line(v, v + new Vector2(v.X + 200, 0));
            var rayCastY = new Line(v, v + new Vector2(0, v.Y + 200));

            //Track intersections so that line joins aren't counted twice
            var intersections = new List<Vector2>();

            var intersectionsX = 0;
            var intersectionsY = 0;

            foreach (var l in Lines)
            {
                //Count horizontal intersections
                var intersectPointX = rayCastX.IntersectsAt(l);
                if (!intersections.Contains(intersectPointX))
                {
                    if (!float.IsNaN(intersectPointX.X))
                    {
                        if (intersectPointX.X >= rayCastX.Start.X)
                        {
                            intersectionsX++;
                            intersections.Add(intersectPointX);
                        }
                    }
                }

                //Count vertical intersections
                var intersectPointY = rayCastY.IntersectsAt(l);
                if (!intersections.Contains(intersectPointY))
                {
                    if (!float.IsNaN(intersectPointY.Y))
                    {
                        if (intersectPointY.Y >= rayCastY.Start.Y)
                        {
                            intersectionsY++;
                            intersections.Add(intersectPointY);
                        }
                    }
                }
            }

            //If both rays cross an odd number of edges we are inside
            return (intersectionsX % 2 == 1) && (intersectionsY % 2 == 1);
        }

        /// <summary>
        /// Applies Separating Axis Theorom to determine if the supplied polygon collides with this polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public bool Collides(Polygon polygon)
        {
            return Collides(polygon, out Vector2 penetration);
        }

        /// <summary>
        /// Applies Separating Axis Theorom to determine if the supplied polygon collides with this polygon
        /// </summary>
        /// <param name="polygon">Polygon to check</param>
        /// <param name="penetration">Amount this polygon is being pushed by</param>
        /// <returns></returns>
        public bool Collides(Polygon polygon, out Vector2 penetration)
        {

            //Based on Love2D Tutorial EP48: SAT Collision Detection by recursor 
            //https://github.com/bncastle/love2d-tutorial/tree/Episode48

            var axes0 = GetEdgeNormals();
            var axes1 = polygon.GetEdgeNormals();
            var axes = axes0.Union(axes1).ToArray();

            var overlap = float.MaxValue;

            penetration = new Vector2(0f / 0f);

            for (var i = 0; i < axes.Length; i++)
            {
                var axis = axes[i];
                var proj0 = Project(this, axis);
                var proj1 = Project(polygon, axis);

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

            var separationVector = (_location + _originVector) - (polygon._location + polygon._originVector);
            penetration.X = Math.Abs(penetration.X) * -Math.Sign(separationVector.X);
            penetration.Y = Math.Abs(penetration.Y) * -Math.Sign(separationVector.Y);

            return true;
        }

        private Projection Project(Polygon polygon, Vector2 axis)
        {
            var dp = polygon.Verticies.Select(v => Vector2Ex.DotProduct(axis, v));
            return new Projection(dp.Min(), dp.Max());
        }

        protected Vector2[] GetEdgeNormals()
        {
            var normals = new List<Vector2>();
            for (var i = 0; i < _numVerticies; i++)
            {
                var edge = Verticies[i] - Verticies[(i + 1) % _numVerticies];
                var normal = Vector2Ex.Normal(edge);
                normal.Normalize();
                if (!normals.Any(n => Math.Abs(Vector2Ex.DotProduct(n, normal)) == 1f))
                {
                    normals.Add(normal);
                }
            }
            return normals.ToArray();
        }

        protected virtual Vector2 RecalculateOrigin()
        {
            var width = Verticies.Max(v => v.X) - Verticies.Min(v => v.X);
            var height = Verticies.Max(v => v.Y) - Verticies.Min(v => v.Y);
            return new Vector2((width / 2), (height / 2));
        }

        protected virtual void UpdateVerticies()
        {
            var locationVector = _location - _lastLocation;
            for (var i = 0; i < Verticies.Length; i++)
            {
                Verticies[i] += locationVector;
            }
            _lastLocation = _location;
        }

        private void UpdateLines()
        {
            for (var i = 0; i < Verticies.Length; i++)
            {
                var a = Verticies[i];
                var b = Verticies[(i + 1) % Verticies.Length];
                Lines[i].Start = a;
                Lines[i].End = b;
            }
        }

        protected void RotateVerticiesAroundOrigin()
        {
            var radians = MathHelper.ToRadians(Angle);
            for (var i = 0; i < Verticies.Length; i++)
            {
                Verticies[i] -= (_location + _originVector);
                Verticies[i] = Vector2.Transform(Verticies[i], Matrix.CreateRotationZ(radians));
                Verticies[i] += (_location + _originVector);
            }
        }

        protected virtual void Recalculate()
        {

            UpdateVerticies();
            
            if (_centerOrigin)
            {
                _originVector = RecalculateOrigin();
            }

            RotateVerticiesAroundOrigin();

            UpdateLines();
        }

    }
}
