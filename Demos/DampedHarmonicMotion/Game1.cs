using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DampedHarmonicMotion
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        DrawBatch drawBatch;

        Hearn.MonoGame.Physics.DampedHarmonicMotion _dampedHarmonicMotion;

        bool _bounce;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _dampedHarmonicMotion = new Hearn.MonoGame.Physics.DampedHarmonicMotion();

            //base.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 50);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawBatch = new DrawBatch(graphics.GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _bounce = Keyboard.GetState().IsKeyDown(Keys.Space);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawBatch.Begin();
            
            drawBatch.FillCircle(Brush.Red, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), 5);

            var length = graphics.GraphicsDevice.Viewport.Width * 3;

            var a = graphics.GraphicsDevice.Viewport.Height / 2;
            var k = 10f;
            var b = 0.75f;

            var prevX = _dampedHarmonicMotion.Calculate(a, k, b, 0);

            var step = 0.005f;
            var spacing = (1f / step);

            for (var t = step; t < length / spacing; t += step)
            {

                var x = _dampedHarmonicMotion.Calculate(a, k, b, t);

                if (_bounce)
                {
                    x = Math.Abs(x);
                }

                var p0 = new Vector2((t - step) * spacing, a - prevX);
                var p1 = new Vector2(t * spacing, a - x);

                drawBatch.DrawLine(Pen.Black, p0, p1);

                prevX = x;

            }

            var gt = (gameTime.TotalGameTime.TotalMilliseconds % length) * step;
            
            var pos = _dampedHarmonicMotion.Calculate(a, k, b, (float)gt);

            if (_bounce)
            {
                pos = Math.Abs(pos);
            }

            drawBatch.FillCircle(Brush.Yellow, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, a - pos), 10);

            drawBatch.FillCircle(Brush.Black, new Vector2((float)(gt / step), a - pos), 5);

            drawBatch.End();

            base.Draw(gameTime);
        }
    }
}
