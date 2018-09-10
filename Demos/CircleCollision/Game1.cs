using Hearn.MonoGame.Geometry;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CircleCollision
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        DrawBatch drawBatch;

        Circle _circle1;
        Circle _circle2;

        Brush _brush;

        int _lastScrollWheelValue;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            drawBatch = new DrawBatch(GraphicsDevice);

            var centerX = GraphicsDevice.Viewport.Width / 2;
            var centerY = GraphicsDevice.Viewport.Height / 2;

            _circle1 = new Circle() { Location = new Vector2(centerX, centerY), Radius = 100 };
            _circle2 = new Circle() { Radius = 50 };
            
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

            var mouseState = Mouse.GetState();
            _circle2.Location = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.ScrollWheelValue > _lastScrollWheelValue && _circle2.Radius < 500)
            {
                _circle2.Radius++;
            }
            else if (mouseState.ScrollWheelValue < _lastScrollWheelValue && _circle2.Radius > 0)
            {
                _circle2.Radius--;
            }
            _lastScrollWheelValue = mouseState.ScrollWheelValue;

            if (_circle1.Collides(_circle2))
            {
                _brush = Brush.Red;
            }
            else
            {
                _brush = Brush.White;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawBatch.Begin();

            drawBatch.FillCircle(_brush, _circle1.Location, _circle1.Radius);
            drawBatch.FillCircle(_brush, _circle2.Location, _circle2.Radius);

            drawBatch.End();

            base.Draw(gameTime);
        }
    }
}
