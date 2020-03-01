using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hearn.MonoGame.Extensions
{
    public static class SpriteBatchEx
    {
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Vector2 scale, Vector2 origin)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle sourceRectangle, Vector2 scale, Vector2 origin)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }

    }
}
