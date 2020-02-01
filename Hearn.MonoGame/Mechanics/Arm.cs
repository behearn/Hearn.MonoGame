using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Mechanics
{
    public class Arm
    {

        public Arm(int length)
        {
            Length = length;
        }

        public Arm(int length, Arm parent)
        {
            Parent = parent;
            Length = length;
        }

        public Vector2 Position { get; set; }

        public int Length { get; set; }

        public float Angle { get; set; }

        public Arm Parent { get; set; }

        public Vector2 EndPosition
        {
            get
            {
                var x = (float)(Position.X + Math.Cos(Angle) * Length);
                var y = (float)(Position.Y + Math.Sin(Angle) * Length);
                return new Vector2(x, y);
            }
        }

        internal void Drag(Vector2 targetPosition)
        {
            PointAt(targetPosition);
            var x = (float)Math.Cos(Angle) * Length;
            var y = (float)Math.Sin(Angle) * Length;
            Position = new Vector2(targetPosition.X - x, targetPosition.Y - y);

            if (Parent != null)
            {
                Parent.Drag(Position);
            }

        }

        private void PointAt(Vector2 targetPosition)
        {
            var dx = targetPosition.X - Position.X;
            var dy = targetPosition.Y - Position.Y;
            Angle = (float)Math.Atan2(dy, dx);
        }

    }
}
