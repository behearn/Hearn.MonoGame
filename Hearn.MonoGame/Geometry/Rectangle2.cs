using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class Rectangle2 : Polygon
    {

        private Vector2 _location;

        private float _width;
        private float _height;

        private Vector2 _origin;
        private bool _centerOrigin;

        private float _angle;

        public Rectangle2() : base(4)
        {            
        }

        public Rectangle2(Vector2 location, float width, float height) : this()
        {
            Location = location;
            Width = width;
            Height = height;
        }

        public Rectangle2(Vector2 location, float width, float height, bool centerOrigin) : this(location, width, height)
        {
            _centerOrigin = centerOrigin;
        }

        public Vector2 Location
        {
            get => _location;
            set
            {
                _location = value;
                RefreshRect();
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                RefreshRect();
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                RefreshRect();
            }
        }

        public float Angle
        {
            get => _angle;
            set
            {
                if (value != _angle)
                {
                    _angle = value;
                    RefreshRect();
                }
            }
        }

        public Vector2 Origin
        {
            get => _origin;
            set
            {
                if (_centerOrigin)
                {
                    throw new Exception("Rectangle2 Origin cannot be set when centered");
                }
                _origin = value;
            }
        }
        
        private void RefreshRect()
        {

            //Set points working clockwise from p[0]
            _p[0] = _location;
            _p[1] = _location + new Vector2(Width, 0);
            _p[2] = _location + new Vector2(Width, Height);
            _p[3] = _location + new Vector2(0, Height);

            if (_centerOrigin)
            {
                _origin = new Vector2(_p[0].X + (_width / 2), _p[0].Y + (_height/ 2));
            }

            var radians = MathHelper.ToRadians(Angle);
            for (var i = 0; i < _p.Length; i++)
            {
                _p[i] -= _origin;
                _p[i] = Vector2.Transform(_p[i], Matrix.CreateRotationZ(radians));
                _p[i] += _origin;
            }

        }

    }
}
