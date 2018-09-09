using Hearn.MonoGame.Geometry;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RectangleCollision
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        DrawBatch drawBatch;

        private Rectangle2 _rectangle0;
        private Rectangle2 _rectangle1;

        private Pen _pen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            drawBatch = new DrawBatch(GraphicsDevice);

            const int Width = 200;
            const int Height = 300;
            
            var x = (GraphicsDevice.Viewport.Width / 2) - (Width / 2);
            var y = (GraphicsDevice.Viewport.Height / 2) - (Height/ 2);

            _rectangle0 = new Rectangle2(new Vector2(x, y), Width, Height, true);            
            _rectangle0.Angle = 0;

            _rectangle1 = new Rectangle2(new Vector2(0, 0), 70, 50, true);
            _rectangle1.Angle = 73;

            base.Initialize();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _rectangle0.Angle = Mouse.GetState().ScrollWheelValue / 15;

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _rectangle0.Height += 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                _rectangle0.Height -= 1;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _rectangle0.Width -= 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                _rectangle0.Width += 1;
            }


            var mouseState = Mouse.GetState();
            _rectangle1.Location = mouseState.Position.ToVector2();

            //var x = (GraphicsDevice.Viewport.Width / 2) - (_rectangle0.Width / 2);
            //var y = (GraphicsDevice.Viewport.Height / 2) - (_rectangle0.Height / 2);
            //_rectangle0.Location = new Vector2(x, y);

            _pen = Pen.Black;
            if (_rectangle1.Intersects(_rectangle0, out Vector2 penetration))
            {
                System.Console.WriteLine(penetration);
                _rectangle0.Location += penetration;
                _pen = Pen.Red;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawBatch.Begin();
            
            
            var path0 = new GraphicsPath(_pen, _rectangle0.VerticiesClosed);
            drawBatch.DrawPath(path0);


            var path1 = new GraphicsPath(_pen, _rectangle1.VerticiesClosed);
            drawBatch.DrawPath(path1);

            drawBatch.End();

            base.Draw(gameTime);
        }
    }
}
