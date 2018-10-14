using Hearn.MonoGame.Geometry;
using Hearn.MonoGame.Widgets;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CircleLineCollision
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawBatch drawBatch;

        Circle _circle;
        Line _line;

        Brush _brush;

        DraggableCircle[] _dragPoints;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            drawBatch = new DrawBatch(GraphicsDevice);

            const int Margin = 50;
            const int DragCircleRadius = 10;

            var x = GraphicsDevice.Viewport.Width - Margin;
            var y = GraphicsDevice.Viewport.Height - Margin;

            _line = new Line() { Start = new Vector2(Margin, Margin), End = new Vector2(x, y) };

            _dragPoints = new DraggableCircle[4];

            _dragPoints[0] = new DraggableCircle()
            {
                Location = _line.Start,
                Radius = DragCircleRadius,
                OnDrag = v => { _line.Start = v; }
            };

            _dragPoints[1] = new DraggableCircle()
            {
                Location = _line.End,
                Radius = DragCircleRadius,
                OnDrag = v => { _line.End = v; }
            };

            _circle = new Circle() { Radius = 50 };

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

            for (var i = 0; i < 2; i++)
            {
                _dragPoints[i].Update();
            }

            var mouseState = Mouse.GetState();
            _circle.Location = new Vector2(mouseState.X, mouseState.Y);

            _brush = Brush.White;
            if (_circle.Collides(_line))
            {
                _brush = Brush.Red;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawBatch.Begin();

            drawBatch.FillCircle(_brush, _circle.Location, _circle.Radius);

            drawBatch.DrawLine(Pen.Black, _line.Start, _line.End);

            for (var i = 0; i < 2; i++)
            {
                drawBatch.FillCircle(Brush.Green, _dragPoints[i].Location, _dragPoints[i].Radius);
            }

            drawBatch.End();
            base.Draw(gameTime);
        }
    }
}
