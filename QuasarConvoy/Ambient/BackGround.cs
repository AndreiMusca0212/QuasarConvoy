using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Ambient
{
    class BackGround:Component
    {
        protected Vector2 _position;

        public float scale = 4f;
        
        public virtual Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        Texture2D _texture;
        public BackGround(Texture2D texture)
        {
            _texture = texture;
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
                    0f
                    );
        }

        public override void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
