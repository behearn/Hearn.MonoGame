using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Physics
{

    public class SimpleHarmonicMotion
    {

        //Physics - Simple Harmonic Motion with Damping (3 of 13) 
        //Michel van Biezen
        //https://www.youtube.com/watch?v=Vg5ng7m43Mo

        public const float DefaultMass = 10;

        private float _amplitude;
        private float _force;
        private float _mass;
        private float _phaseAngle;

        private double _t0;

        public float Amplitude { get => _amplitude; }
        public float Force { get => _force; }
        public float Mass { get => _mass; }
        public float PhaseAngle { get => _phaseAngle; }

        public float Frequency
        {
            get
            {
                var omega = Math.Sqrt(_force / _mass);
                return (float)omega / MathHelper.TwoPi;
            }
        }

        public double CycleTime
        {
            get
            {
                return 1d / Frequency;
            }
        }

        /// <summary>
        /// Initialises a simple harmonic motion wave
        /// </summary>
        /// <param name="amplitude">Amplitide of wave</param>
        /// <param name="force">Force constant</param>
        /// <returns></returns>
        public void Init(float amplitude, float force, float friction, GameTime gameTime)
        {
            Init(amplitude, force, DefaultMass, 0f, gameTime);
        }

        /// <summary>
        /// Initialises a simple harmonic motion wave
        /// </summary>
        /// <param name="amplitude">Amplitide of wave</param>
        /// <param name="force">Force constant</param>
        /// <returns></returns>
        public void Init(float amplitude, float force, float mass, float phaseAngle, GameTime gameTime)
        {
            _amplitude = amplitude;
            _force = force;
            _mass = mass;
            _phaseAngle = phaseAngle;
            _t0 = gameTime.TotalGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Returns amplitude of a simple harmonic motion wave at a given time
        /// </summary>
        /// <returns>Amplitude at gameTime elapsed since initialised Time</returns>
        public float Calculate(GameTime gameTime)
        {
            var time = (gameTime.TotalGameTime.TotalMilliseconds - _t0);
            return Calculate(time);
        }

        /// <summary>
        /// Returns amplitude of a simple harmonic motion wave at a given time
        /// </summary>
        /// <returns>Amplitude at time</returns>
        public float Calculate(double time)
        {

            var omega = Math.Sqrt(_force / _mass);

            var x = _amplitude * (float)Math.Cos(omega * time - _phaseAngle);

            return x;

        }

    }

}
