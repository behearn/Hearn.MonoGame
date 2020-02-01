using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Physics
{
    public class DampedHarmonicMotion
    {

        //Physics - Simple Harmonic Motion with Damping (9 of 13) Underdamping
        //Michel van Biezen
        //https://www.youtube.com/watch?v=WPAH0fQ3JO0

        public const float DefaultMass = 10;

        private float _amplitude;
        private float _force;
        private float _friction;
        private float _mass;
        private float _phaseAngle;

        private double _t0;

        public float Amplitude { get => _amplitude; }
        public float Force { get => _force; }
        public float Friction { get => _friction; }
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
        /// Initialises a damped harmonic motion wave
        /// </summary>
        /// <param name="amplitude">Amplitide of wave</param>
        /// <param name="force">Force constant</param>
        /// <param name="friction">Friction constant</param>
        /// <returns></returns>
        public void Init(float amplitude, float force, float friction, GameTime gameTime)
        {
            Init(amplitude, force, friction, DefaultMass, 0f, gameTime);
        }

        /// <summary>
        /// Initialises a damped harmonic motion wave
        /// </summary>
        /// <param name="amplitude">Amplitide of wave</param>
        /// <param name="force">Force constant</param>
        /// <param name="friction">Friction constant</param>
        /// <returns></returns>
        public void Init(float amplitude, float force, float friction, float mass, float phaseAngle, GameTime gameTime)
        {
            _amplitude = amplitude;
            _force = force;
            _friction = friction;
            _mass = mass;
            _phaseAngle = phaseAngle;
            _t0 = gameTime.TotalGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Returns amplitude of a damped harmonic motion wave at a given time
        /// </summary>
        /// <returns>Amplitude at gameTime elapsed since initialised Time</returns>
        public float Calculate(GameTime gameTime)
        {
            var time = (gameTime.TotalGameTime.TotalMilliseconds - _t0);
            return Calculate(time);
        }

        /// <summary>
        /// Returns amplitude of a damped harmonic motion wave at a given time
        /// </summary>
        /// <returns>Amplitude at time</returns>
        public float Calculate(double time)
        {
            
            var omega = Math.Sqrt(_force / _mass);

            var y = -_friction / (2 * _mass);
            var eyt = Math.Pow(Math.E, y * time);

            var x = _amplitude * eyt * (float)Math.Cos(omega * time - _phaseAngle);

            return (float)x;

        }

    }
}
