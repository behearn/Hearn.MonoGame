using System;
using System.Collections.Generic;
using Hearn.MonoGame.Geometry;
using Hearn.MonoGame.Widgets;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TriangleCollision
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        DrawBatch drawBatch;

        private Triangle _triangle1;
        private Triangle _triangle2;

        DraggableCircle[] _dragPoints;

        private Pen _pen1;
        private Pen _pen2;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            drawBatch = new DrawBatch(GraphicsDevice);

            IsMouseVisible = true;
            
            _triangle1 = new Triangle(new Vector2(400, 400), new Vector2(300, 200), new Vector2(700, 400));

            _triangle2 = new Triangle(new Vector2(100, 0), new Vector2(0, 100), new Vector2(100, 100));

            const int DragCircleRadius = 10;

            _dragPoints = new DraggableCircle[6];

            _dragPoints[0] = new DraggableCircle()
            {
                Location = _triangle1.A,
                Radius = DragCircleRadius,
                OnDrag = v => { _triangle1.A = v; }
            };

            _dragPoints[1] = new DraggableCircle()
            {
                Location = _triangle1.B,
                Radius = DragCircleRadius,
                OnDrag = v => { _triangle1.B = v; }
            };
            
            _dragPoints[2] = new DraggableCircle()
            {
                Location = _triangle1.C,
                Radius = DragCircleRadius,
                OnDrag = v => { _triangle1.C = v; }
            };

            _dragPoints[3] = new DraggableCircle()
            {
                Location = _triangle2.A,
                Radius = DragCircleRadius,
                OnDrag = v => { _triangle2.A = v; }
            };

            _dragPoints[4] = new DraggableCircle()
            {
                Location = _triangle2.B,
                Radius = DragCircleRadius,
                OnDrag = v => { _triangle2.B = v; }
            };

            _dragPoints[5] = new DraggableCircle()
            {
                Location = _triangle2.C,
                Radius = DragCircleRadius,
                OnDrag = v => { _triangle2.C = v; }
            };
            
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _pen1 = Pen.Black;
            _pen2 = Pen.Black;


            if (_triangle1.Intersects(_triangle2))
            {
                _pen1 = Pen.Red;
                _pen2 = Pen.Red;
            }
            else
            {
                var mouseState = Mouse.GetState();

                if (_triangle1.Intersects(mouseState.Position.ToVector2()))
                {
                    _pen1 = Pen.Yellow;
                }
                if (_triangle2.Intersects(mouseState.Position.ToVector2()))
                {
                    _pen2 = Pen.Yellow;
                }
            }


            for (var i = 0; i < _dragPoints.Length; i++)
            {
                _dragPoints[i].Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawBatch.Begin();

            DrawTriangle(_triangle1, _pen1);
            DrawTriangle(_triangle2, _pen2);

            for (var i = 0; i < _dragPoints.Length; i++)
            {
                drawBatch.FillCircle(Brush.Red, _dragPoints[i].Location, _dragPoints[i].Radius);
            }

            drawBatch.End();

            base.Draw(gameTime);
        }

        private void DrawTriangle(Triangle triangle, Pen pen)
        {
            var points = new List<Vector2>() { triangle.A, triangle.B, triangle.C, triangle.A };
            var path = new GraphicsPath(pen, points);
            drawBatch.DrawPath(path);
        }

    }
}
