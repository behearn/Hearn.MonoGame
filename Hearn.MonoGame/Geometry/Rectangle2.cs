using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class Rectangle2 : Polygon
    {
        
        private float _width;
        private float _height;

        /// <summary>
        /// Creates a rectangle based on the Polygon class
        /// </summary>
        public Rectangle2() : base(4)
        {            
        }

        /// <summary>
        /// Creates a rectangle based on the Polygon class
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle2(Vector2 location, float width, float height) : this()
        {
            Location = location;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a rectangle based on the Polygon class
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="centerOrigin"></param>
        public Rectangle2(Vector2 location, float width, float height, bool centerOrigin) : this(location, width, height)
        {
            CenterOrigin = centerOrigin;
        }

        /// <summary>
        /// Width of the rectangle.  Automatically updates Verticies.
        /// </summary>
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                Recalculate();
            }
        }

        /// <summary>
        /// Height of the rectangle.  Automatically updates Verticies.
        /// </summary>
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                Recalculate();
            }
        }

        protected override Vector2 RecalculateOrigin()
        {
            //Width & height known, no need to calculate them
            return new Vector2((_width / 2), (_height / 2));
        }

        protected override void UpdateVerticies()
        {
            //Set points working clockwise from _verticies[0]
            Verticies[0] = Location;
            Verticies[1] = Location + new Vector2(Width, 0);
            Verticies[2] = Location + new Vector2(Width, Height);
            Verticies[3] = Location + new Vector2(0, Height);
        }

    }
}
