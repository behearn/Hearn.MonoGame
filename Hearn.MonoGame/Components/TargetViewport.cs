using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Components
{
    public class TargetViewport
    {

        //Based on http://www.infinitespace-studios.co.uk/general/monogame-scaling-your-game-using-rendertargets-and-touchpanel/

        public enum Orientations
        {
            Landscape = 0,
            Portrait = 1
        }

        const int NATIVE_LANDSCAPE_WIDTH = 1920;
        const int NATIVE_LANDSCAPE_HEIGHT = 1080;

        private int _windowWidth;
        private int _windowHeight;
        
        private RenderTarget2D _contentRenderTarget;

        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        public TargetViewport(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
        }
        
        public int BarWidth { get; internal set; }
        public int BarHeight { get;  internal set; }

        public Rectangle FullScreenRect { get; internal set; }
        public Rectangle ContentRect { get; internal set; }

        public float PreferredAspect { get; internal set; }

        public float ScaleX { get; internal set; }
        public float ScaleY { get; internal set; }

        public Orientations Orientation { get; internal set; }

        public int NativeWidth { get => Orientation == Orientations.Landscape ? NATIVE_LANDSCAPE_WIDTH : NATIVE_LANDSCAPE_HEIGHT; }
        public int NativeHeight { get => Orientation == Orientations.Landscape ? NATIVE_LANDSCAPE_HEIGHT : NATIVE_LANDSCAPE_WIDTH; }

        private bool _beginCalled;

        public void Update(Game game)
        {

            BarWidth = 0;
            BarHeight = 0;

            _windowWidth = game.Window.ClientBounds.Width;
            _windowHeight = game.Window.ClientBounds.Height;

            if (_windowWidth >= _windowHeight)
            {
                Orientation = Orientations.Landscape;
            }
            else
            {
                Orientation = Orientations.Portrait;
            }

            FullScreenRect = new Rectangle(0, 0, _windowWidth, _windowHeight);

            var outputAspect = _windowWidth / (float)_windowHeight;
            PreferredAspect = NativeWidth / (float)NativeHeight;

            if (outputAspect == PreferredAspect)
            {
                ContentRect = new Rectangle(0, 0, _windowWidth, _windowHeight);
            }
            else if (outputAspect < PreferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                var presentHeight = (int)((game.Window.ClientBounds.Width / PreferredAspect) + 0.5f);
                BarHeight = (game.Window.ClientBounds.Height - presentHeight) / 2;
                ContentRect = new Rectangle(0, BarHeight, game.Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                var presentWidth = (int)((game.Window.ClientBounds.Height * PreferredAspect) + 0.5f);
                BarWidth = (game.Window.ClientBounds.Width - presentWidth) / 2;
                ContentRect = new Rectangle(BarWidth, 0, presentWidth, game.Window.ClientBounds.Height);
            }

            ScaleX = NativeWidth / (float)(_windowWidth - 2 * BarWidth);
            ScaleY = NativeHeight / (float)(_windowHeight - 2 * BarHeight);

            _contentRenderTarget = new RenderTarget2D(
               _graphicsDevice,
               NativeWidth, NativeHeight, false,
               SurfaceFormat.Color,
               DepthFormat.None,
               _graphicsDevice.PresentationParameters.MultiSampleCount,
               RenderTargetUsage.DiscardContents
           );

        }

        public void Begin()
        {
            _graphicsDevice.SetRenderTarget(_contentRenderTarget);
            _beginCalled = true;
        }

        public void End()
        {
            if (!_beginCalled)
            {
                throw new Exception("Call Begin");
            }

            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            ScaleContentToViewport();

            _beginCalled = false;
        }

        private void ScaleContentToViewport()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(_contentRenderTarget, ContentRect, Color.White);
            _spriteBatch.End();
        }
    }
}
