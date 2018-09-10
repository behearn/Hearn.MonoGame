using Hearn.MonoGame.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Widgets
{
    public class DraggableCircle
    {

        private Circle _circle;
        private Vector2 _offset;

        private bool _selected;
        public bool Selected { get => _selected; }

        public Vector2 Location
        {
            get => _circle.Location;
            set => _circle.Location = value;
        }

        public float Radius
        {
            get => _circle.Radius;
            set => _circle.Radius = value;
        }

        public Action<Vector2> OnDrag { get; set; }

        public DraggableCircle()
        {
            _circle = new Circle();
        }

        public void Update()
        {

            var mouseState = Mouse.GetState();

            var mouseLocation = new Vector2(mouseState.X, mouseState.Y);

            if (!_selected && _circle.Collides(mouseLocation))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    _offset = new Vector2(mouseState.X - Location.X, mouseLocation.Y - Location.Y);
                    _selected = true;
                }
            }

            if (_selected)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Location = mouseLocation - _offset;
                    OnDrag?.Invoke(Location);
                }
                else
                {
                    _selected = false;
                }
            }
            
        }

    }
}

