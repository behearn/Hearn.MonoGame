using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class Triangle
    {

        private Vector2 _a;
        private Vector2 _b;
        private Vector2 _c;
        
        private Line _ab;
        private Line _bc;
        private Line _ca;

        public Triangle()
        {
            _ab = new Line();
            _bc = new Line();
            _ca = new Line();
        }

        public Triangle(Vector2 a, Vector2 b, Vector2 c) : this()
        {
            A = a;
            B = b;
            C = c;
        }

        public Vector2 A
        {
            get => _a;
            set
            {
                _a = value;
                _ab.Start = _a;
                _ca.End = _a;
            }
        }

        public Vector2 B
        {
            get => _b;
            set
            {
                _b = value;
                _bc.Start = _b;
                _ab.End = _b;
            }
        }

        public Vector2 C
        {
            get => _c;
            set
            {
                _c = value;
                _ca.Start = _c;
                _bc.End = _c;
            }
        }
        
        public Line AB { get => _ab; }
        public Line BC { get => _bc; }
        public Line CA { get => _ca; }

        public float Alpha
        {
            get => (float)Math.Acos(((BC.Length * BC.Length) + (CA.Length * CA.Length) - (AB.Length * AB.Length)) / (2 * BC.Length * CA.Length));
        }

        public float Beta
        {
            get => (float)Math.Acos(((AB.Length * AB.Length) + (CA.Length * CA.Length) - (BC.Length * BC.Length)) / (2 * AB.Length * CA.Length));
        }

        public float Gamma
        {
            get => (float)Math.Acos(((AB.Length * AB.Length) + (BC.Length * BC.Length) - (CA.Length * CA.Length)) / (2 * AB.Length * BC.Length));
        }

        public bool Collides(Triangle t)
        {
            return AB.Intersects(t.AB) || AB.Intersects(t.BC) || AB.Intersects(t.CA)
                || BC.Intersects(t.AB) || BC.Intersects(t.BC) || BC.Intersects(t.CA)
                || CA.Intersects(t.AB) || CA.Intersects(t.BC) || CA.Intersects(t.CA)
                || Collides(t.A) 
                || t.Collides(A);            
        }

        public bool Collides(Vector2 v)
        {
            var dx = v.X - C.X;
            var dy = v.Y - C.Y;
            var dx21 = C.X - B.X;
            var dy12 = B.Y - C.Y;
            var d = dy12 * (A.X - C.X) + dx21 * (A.Y - C.Y);
            var s = dy12 * dx + dx21 * dy;
            var t = (C.Y - A.Y) * dx + (A.X - C.X) * dy;
            if (d < 0)
                return s <= 0 && t <= 0 && s + t >= d;
            return s >= 0 && t >= 0 && s + t <= d;
        }

    }
}
