using Hearn.MonoGame.Geometry;
using Hearn.MonoGame.Widgets;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace CirclePolygonCollision
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawBatch drawBatch;

        Circle _circle;
        Polygon _polygon;

        Brush _brush;
        

        List<Vector2> _intersections;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            drawBatch = new DrawBatch(GraphicsDevice);            

            var x = GraphicsDevice.Viewport.Width / 2;
            var y = GraphicsDevice.Viewport.Height / 2;
            var size = 125;

            _polygon = new Rectangle2(new Vector2(x - size, y - size), 2 * size, 2 * size);
            
            _circle = new Circle() { Radius = 100 };

            _intersections = new List<Vector2>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            var mouseState = Mouse.GetState();
            _circle.Location = new Vector2(mouseState.X, mouseState.Y);

            _brush = Brush.White;
            if (_circle.Collides(_polygon))
            {
                _brush = Brush.Red;
            }

            _intersections.Clear();
            foreach (var line in _polygon.Lines)
            {                
                var lineIntersections = _circle.IntersectPoints(line);
                _intersections.AddRange(lineIntersections);                
            }
            if (_intersections.Any())
            {
                _brush = Brush.Orange;
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawBatch.Begin();

            drawBatch.FillCircle(_brush, _circle.Location, _circle.Radius);

            foreach (var line in _polygon.Lines)
            {
                drawBatch.DrawLine(Pen.Black, line.Start, line.End);
            }            

            foreach (var intersection in _intersections)
            {
                drawBatch.DrawCircle(Pen.Purple, intersection, 5);
            }

            drawBatch.End();
            base.Draw(gameTime);
        }
    }
}
