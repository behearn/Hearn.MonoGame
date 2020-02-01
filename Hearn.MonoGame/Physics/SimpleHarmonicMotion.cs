using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Physics
{
    public class SimpleHarmonicMotion
    {

        /// <summary>
        /// Returns position of a simple harmonic motion wave at a given time
        /// </summary>
        /// <param name="amplitude">Amplitide of wave</param>
        /// <param name="force">Force constant k</param>
        /// <param name="time">Time offset</param>
        /// <returns></returns>
        public float Calculate (float amplitude, float force, float time)
        {
            return Calculate(amplitude, force, time, 1f, 0f);
        }

        /// <summary>
        /// Returns position of a simple harmonic motion wave at a given time
        /// </summary>
        /// <param name="amplitude">Amplitide of wave</param>
        /// <param name="force">Force constant k</param>
        /// <param name="time">Time offset</param>
        /// <param name="mass">Mass of object in KG</param>
        /// <param name="phaseAngle">Starting offset of wave</param>
        /// <returns></returns>
        public float Calculate(float amplitude, float force, float time, float mass, float phaseAngle)
        {

            var omega = Math.Sqrt(force / mass);

            var x = amplitude * (float)Math.Cos(omega * time - phaseAngle);

            return x;

        }

    }
}
