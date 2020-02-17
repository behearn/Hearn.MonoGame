using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hearn.MonoGame.Components
{
    public class InputState
    {

        private static int _x;
        private static int _y;
        private static bool _pressed;

        public static int X { get => _x; }
        public static int Y { get => _y; }
        public static Vector2 Coords { get => new Vector2(_x, _y); }

        public static bool UseMouse { get; set; }
        public static bool UseKeyboard { get; set; }

        public static bool Pressed { get => _pressed; }

        public static KeyboardState KeyboardState;
        private static Keys[] _keysThisTick;
        private static Keys[] _keysLastTick;
        private static Keys[] _keysPressed;


        public static void Update(TargetViewport targetViewport)
        {

            if (UseMouse)
            {
                var mouseState = Mouse.GetState();

                _x = (int)((mouseState.X - targetViewport.BarWidth) * targetViewport.ScaleX);
                _y = (int)((mouseState.Y - targetViewport.BarHeight) * targetViewport.ScaleY);

                _pressed = (mouseState.LeftButton == ButtonState.Pressed);

            }
            else
            {
                _pressed = false;
                var touchLocations = TouchPanel.GetState();
                foreach (var tl in touchLocations)
                {
                    _pressed = true;
                    _x = (int)((tl.Position.X - targetViewport.BarWidth) * targetViewport.ScaleX);
                    _y = (int)((tl.Position.Y - targetViewport.BarHeight) * targetViewport.ScaleY);
                }
            }

            if (UseKeyboard)
            {
                KeyboardState = Keyboard.GetState();

                _keysPressed = null;
                _keysThisTick = KeyboardState.GetPressedKeys();

                if (_keysLastTick != null)
                {
                    _keysPressed = _keysThisTick.Except(_keysLastTick).ToArray();
                }

                _keysLastTick = _keysThisTick;

            }

        }

        public static bool KeyPressed(Keys key)
        {
            if (_keysPressed != null)
            {
                return _keysPressed.Contains(key);
            }
            return false;
        }

    }
}
