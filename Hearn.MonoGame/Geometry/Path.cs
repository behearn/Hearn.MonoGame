using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Geometry
{
    public class Path
    {

        public Vector2[] Vertices { get; set; }

        public float Length
        {
            get
            {
                var length = 0f;
                if (Vertices != null && Vertices.Length > 0)
                {
                    for (var i = 1; i < Vertices.Length; i++)
                    {
                        length += Math.Abs(Vector2.Distance(Vertices[i - 1], Vertices[i]));
                    }
                }
                return length;
            }
        }

        public Vector2 Lerp(float amount)
        {

            if (Vertices == null)
            {
                return Vector2.Zero;
            }

            if (amount <= 0 || Vertices.Length == 0)
            {
                return Vertices[0];
            }

            if (amount >= 1)
            {
                return Vertices[Vertices.Length - 1];
            }

            var position = MathHelper.Lerp(0, Length, amount);
            var cumulativeDistance = 0f;

            var lerpVector = Vector2.Zero;

            var i = 1;
            while (i < Vertices.Length)
            {
                var distance = Math.Abs(Vector2.Distance(Vertices[i - 1], Vertices[i]));

                if (cumulativeDistance + distance < position)
                {
                    cumulativeDistance += distance;
                    i++;
                }
                else
                {
                    var remainder = position - cumulativeDistance;

                    var ratio = remainder / distance;

                    var segment = Vertices[i - 1] - Vertices[i];
                    segment *= ratio;

                    lerpVector = Vertices[i - 1] - segment;
                    return lerpVector;

                }

            }

            return Vector2.Zero;

        }
    }
}
