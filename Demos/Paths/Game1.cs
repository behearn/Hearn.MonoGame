using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Paths
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        DrawBatch _drawBatch;
        Hearn.MonoGame.Geometry.Path _path;
        Vector2 _currentVector;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            _path = new Hearn.MonoGame.Geometry.Path();
            _path.Vertices = new Vector2[]
            {
                new Vector2() { X = 100, Y = 200},
                new Vector2() { X = 200, Y = 200},
                new Vector2() { X = 200, Y = 300},
                new Vector2() { X = 300, Y = 300},
                new Vector2() { X = 300, Y = 400},
                new Vector2() { X = 400, Y = 400}                
            };

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
            _drawBatch = new DrawBatch(GraphicsDevice);

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

            const float TravelTime = 1000;

            var s = gameTime.TotalGameTime.TotalMilliseconds % TravelTime;
            var r = (float)s / TravelTime;

            _currentVector = _path.Lerp(r);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _drawBatch.Begin();

            for (var i = 1; i < _path.Vertices.Length; i++)
            {
                _drawBatch.DrawLine(Pen.Black, _path.Vertices[i - 1], _path.Vertices[i]);
            }

            _drawBatch.FillCircle(Brush.Red, _currentVector, 10);

            _drawBatch.End();

            base.Draw(gameTime);
        }
    }
}
