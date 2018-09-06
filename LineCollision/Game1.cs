using Hearn.MonoGame.Geometry;
using Hearn.MonoGame.Widgets;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LineCollision
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        DrawBatch drawBatch;

        Line _line1;
        Line _line2;

        DraggableCircle[] _dragPoints;

        Pen _pen;

        Vector2 _intersection;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            drawBatch = new DrawBatch(GraphicsDevice);

            IsMouseVisible = true;

            const int Margin = 50;
            const int DragCircleRadius = 10;

            var x = GraphicsDevice.Viewport.Width - Margin;
            var y = GraphicsDevice.Viewport.Height - Margin;

            _line1 = new Line() { Start = new Vector2 (Margin, Margin), End = new Vector2(x, y) };
            _line2 = new Line() { Start = new Vector2(x, Margin), End = new Vector2(Margin, y) };

            _dragPoints = new DraggableCircle[4];

            _dragPoints[0] = new DraggableCircle()
            {
                Location = _line1.Start,
                Radius = DragCircleRadius,
                OnDrag = v => { _line1.Start = v; }
            };

            _dragPoints[1] = new DraggableCircle()
            {
                Location = _line1.End,
                Radius = DragCircleRadius,
                OnDrag = v => { _line1.End = v; }
            };

            _dragPoints[2] = new DraggableCircle()
            {
                Location = _line2.Start,
                Radius = DragCircleRadius,
                OnDrag = v => { _line2.Start = v; }
            };

            _dragPoints[3] = new DraggableCircle()
            {
                Location = _line2.End,
                Radius = DragCircleRadius,
                OnDrag = v => { _line2.End = v; }
            };

            _pen = Pen.Black;

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);            
        }
        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for (var i = 0; i < 4; i++)
            {
                _dragPoints[i].Update();
            }
            
            _intersection = _line1.Intersects(_line2, true);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawBatch.Begin();

            drawBatch.DrawLine(_pen, _line1.Start, _line1.End);
            drawBatch.DrawLine(_pen, _line2.Start, _line2.End);

            drawBatch.DrawCircle(_pen, _intersection, 5);

            for (var i = 0; i < 4; i++)
            {
                drawBatch.FillCircle(Brush.Red, _dragPoints[i].Location, _dragPoints[i].Radius);
            }

            drawBatch.End();

            base.Draw(gameTime);
        }
    }
}
