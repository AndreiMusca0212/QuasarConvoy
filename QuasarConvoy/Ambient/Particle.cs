using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Ambient
{
    class Particle
    {
        public Vector2 Position { set; get; }
        public Texture2D _texture;
        public bool isRemoved = false;
        public Vector2 Velocity { set; get; }
        float scale;
        public Particle(Texture2D texture, float sca)
        {
            _texture = texture;
            scale = sca;
            
        }

        public void Update(Camera cam)
        {
            Position += (-1) * cam.velocity*scale;
            if (Position.X > Game1.ScreenWidth+10 || Position.X < -10 || Position.Y < -10 || Position.Y > Game1.ScreenHeight+10)
                isRemoved = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0, 0, _texture.Width, _texture.Height),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0.1f
                    );
        }
    }
}
