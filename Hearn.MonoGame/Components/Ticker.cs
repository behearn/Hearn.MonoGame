using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Components
{
    public class Ticker
    {

        private TimeSpan _elapsedTime;
        private Action _onTick;
        private TimeSpan _interval;
        private bool _paused;

        public bool ResetOnResume { get; set; }

        public bool Paused
        {
            get
            {
                return _paused;
            }
            set
            {
                _paused = value;
                if (ResetOnResume)
                {
                    _elapsedTime = TimeSpan.Zero;
                }
            }
        }

        public Ticker(TimeSpan interval, Action onTick)
        {
            if (interval == null)
            {
                throw new ArgumentException("interval required");
            }

            if (onTick == null)
            {
                throw new ArgumentException("onTick required");
            }

            _interval = interval;
            _onTick = onTick;
        }

        public Ticker(int milliseconds, Action onTick)
        {
            if (milliseconds <= 0)
            {
                throw new ArgumentException("invalid milliseconds");
            }

            if (onTick == null)
            {
                throw new ArgumentException("onTick required");
            }

            _interval = TimeSpan.FromMilliseconds(milliseconds);
            _onTick = onTick;
        }

        public void Update(GameTime gameTime)
        {
            if (!_paused)
            {
                _elapsedTime += gameTime.ElapsedGameTime;

                if (_elapsedTime > _interval)
                {
                    _elapsedTime -= _interval;
                    _onTick();
                }
            }
        }

    }
}
