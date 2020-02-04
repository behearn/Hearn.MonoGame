using Hearn.MonoGame.Geometry;
using Hearn.MonoGame.Widgets;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BezierCurves
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        DrawBatch _drawBatch;

        BezierCurve _quadraticBezierCurve;
        Vector2 _quadraticCurrentVector;
        DraggableCircle[] _quadraticDragPoints;

        BezierCurve _cubicBezierCurve;
        Vector2 _cubicCurrentVector;
        DraggableCircle[] _cubicDragPoints;
        
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

            IsMouseVisible = true;

            const int DragCircleRadius = 10;
            
            _quadraticBezierCurve = new QuadraticBezierCurve(
                new Vector2(50, 10),
                new Vector2(50, graphics.PreferredBackBufferHeight - 50),
                new Vector2(graphics.PreferredBackBufferWidth - 50, graphics.PreferredBackBufferHeight - 50),
                100
            );

            _quadraticDragPoints = new DraggableCircle[3];
            for (var i = 0; i < 3; i++)
            {
                var index = i;
                _quadraticDragPoints[i] = new DraggableCircle()
                {
                    Location = _quadraticBezierCurve.GetControlPoint(index),
                    Radius = DragCircleRadius,
                    OnDrag = v => { _quadraticBezierCurve.SetControlPoint(index, v); }
                };
            }
            
            _cubicBezierCurve = new CubicBezierCurve(
                new Vector2(10, 10),
                new Vector2(10, graphics.PreferredBackBufferHeight - 10),
                new Vector2(graphics.PreferredBackBufferWidth - 10, graphics.PreferredBackBufferHeight - 10),
                new Vector2(graphics.PreferredBackBufferWidth - 10, 10),
                100
            );

            _cubicDragPoints = new DraggableCircle[4];
            for (var i = 0; i < 4; i++)
            {
                var index = i;
                _cubicDragPoints[i] = new DraggableCircle()
                {
                    Location = _cubicBezierCurve.GetControlPoint(index),
                    Radius = DragCircleRadius,
                    OnDrag = v => { _cubicBezierCurve.SetControlPoint(index, v); }
                };
            }

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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            _quadraticCurrentVector = _quadraticBezierCurve.Lerp(r);
            _cubicCurrentVector = _cubicBezierCurve.Lerp(r);

            for (var i = 0; i < 3; i++)
            {
                _quadraticDragPoints[i].Update();
            }

            for (var i = 0; i < 4; i++)
            {
                _cubicDragPoints[i].Update();
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

            _drawBatch.Begin();

            for (var i = 1; i < _quadraticBezierCurve.Vertices.Length; i++)
            {
                _drawBatch.DrawLine(Pen.Black, _quadraticBezierCurve.Vertices[i - 1], _quadraticBezierCurve.Vertices[i]);
            }

            _drawBatch.FillCircle(Brush.Red, _quadraticCurrentVector, 10);

            for (var i = 0; i < 3; i++)
            {
                _drawBatch.FillCircle(Brush.Red, _quadraticDragPoints[i].Location, _quadraticDragPoints[i].Radius);
                if (i < 2)
                {
                    _drawBatch.DrawLine(Pen.Red, _quadraticDragPoints[i].Location, _quadraticDragPoints[i + 1].Location);
                }
            }

            for (var i = 1; i < _cubicBezierCurve.Vertices.Length; i++)
            {
                _drawBatch.DrawLine(Pen.Black, _cubicBezierCurve.Vertices[i - 1], _cubicBezierCurve.Vertices[i]);
            }

            _drawBatch.FillCircle(Brush.Green, _cubicCurrentVector, 10);

            for (var i = 0; i < 4; i++)
            {
                _drawBatch.FillCircle(Brush.Green, _cubicDragPoints[i].Location, _cubicDragPoints[i].Radius);
                if (i < 3)
                {
                    _drawBatch.DrawLine(Pen.Green, _cubicDragPoints[i].Location, _cubicDragPoints[i + 1].Location);
                }
            }

            _drawBatch.End();

            base.Draw(gameTime);
        }
    }
}
