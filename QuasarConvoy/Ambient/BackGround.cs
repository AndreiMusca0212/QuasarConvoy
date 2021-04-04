using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Ambient
{
    class Background:Component
    {
        protected Vector2 _position;

        public float scale;
        
        protected float Layer { set; get; }
        public virtual Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        protected Texture2D _texture;
        public Background(Texture2D texture)
        {
            _texture = texture;
            Layer = 0f;

            scale = Game1.ScreenWidth/(float)texture.Width;
        }

        public override void Draw(SpriteBatch spriteBatch)
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
                    Layer
                    );
        }

        public override void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
