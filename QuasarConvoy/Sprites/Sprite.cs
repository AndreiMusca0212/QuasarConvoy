using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuasarConvoy.Managers;
using QuasarConvoy.Models;

namespace QuasarConvoy.Sprites
{
    class Sprite
    {
        #region Fields

        protected Vector2 _position;

        private Texture2D _texture;

        public int scale = 4;
        

        #endregion

        #region Properties

        public virtual Vector2 Position
        {
            get { return _position; }
            set {_position = value;}
        }

        public virtual Rectangle Collisionbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width*scale, _texture.Height*scale);
            }
        }


        #endregion


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0,0,_texture.Width, _texture.Height),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    (float)scale,
                    SpriteEffects.None,
                    0f
                    );
        }

        
        public Sprite(ContentManager Content)
        { 
        
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }
        
        public virtual void Update(GameTime gametime, List<Sprite> sprites)
        {  }
    }
}
