using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Components
{
    public class FrameRate
    {
        private int _frameRate = 0;
        private int _frameCounter = 0;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                _elapsedTime -= TimeSpan.FromSeconds(1);
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }

        public int GetFramesPerSecond()
        {
            _frameCounter++;
            return _frameRate;
        }
    }
}
