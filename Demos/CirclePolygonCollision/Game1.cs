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

        const int Radius = 10;

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

            //var size = 125;
            //_polygon = new Rectangle2(new Vector2(x - size, y - size), 2 * size, 2 * size);
            //var verticies = _polygon.Verticies;

            var verticies = new Vector2[] {
                new Vector2(0,0),
                new Vector2(64, 32),
                new Vector2(96, 64),
                new Vector2(128, 96),
                new Vector2(160, 128),
                new Vector2(-64, 64)
            };
            _polygon = new Polygon(verticies);
            
            var width = verticies.Max(v => v.X) - verticies.Min(v => v.X);
            var height = verticies.Max(v => v.Y) - verticies.Min(v => v.Y);        
            var offsetX = verticies[0].X - verticies.Min(v => v.X);
            var offsetY = verticies[0].Y - verticies.Min(v => v.Y);
            _polygon.Location = new Vector2(x, y) - new Vector2(width / 2, height / 2) + new Vector2(offsetX, offsetY);

            _polygon.Angle = 330;

            _circle = new Circle() { Radius = Radius };

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

            _circle.Radius = Radius + (mouseState.ScrollWheelValue / 10);

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
