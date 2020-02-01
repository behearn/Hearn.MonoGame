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
        double _elapsedTime;
        int _width;
        int _height;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _width = graphics.PreferredBackBufferWidth;
            _height = graphics.PreferredBackBufferHeight;
            
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

            _elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTime >= _width)
            {
                _elapsedTime = 0;
                _dampedHarmonicMotion.Init(_height / 2, 0.01f, 0.1f, 10, 0, gameTime);
            }

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

            var prevA = _dampedHarmonicMotion.Calculate(0);

            for (var t = 1; t < _width; t++)
            {

                var a = _dampedHarmonicMotion.Calculate(t);

                if (_bounce)
                {
                    a = Math.Abs(a);
                }

                var p0 = new Vector2(t, (_height / 2) - prevA);
                var p1 = new Vector2(t, (_height / 2) - a);

                drawBatch.DrawLine(Pen.Black, p0, p1);

                prevA = a;

            }
            
            var amplitude = _dampedHarmonicMotion.Calculate(gameTime);

            if (_bounce)
            {
                amplitude = Math.Abs(amplitude);
            }

            drawBatch.FillCircle(Brush.Yellow, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, (_height / 2) - amplitude), 10);

            drawBatch.FillCircle(Brush.Black, new Vector2((float)_elapsedTime, (_height / 2) - amplitude), 5);

            drawBatch.End();

            base.Draw(gameTime);
        }
    }
}
