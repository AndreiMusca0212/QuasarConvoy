using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using QuasarConvoy.Sprites;

namespace QuasarConvoy.Ambient
{
    class Background:Sprite
    {
        public Background(Texture2D texture):base(texture)
        {
            Layer = 0f;

            scale = Game1.ScreenWidth/(float)texture.Width;
        }

        public override void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
