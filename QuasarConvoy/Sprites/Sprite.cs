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
    public class Sprite:Component
    {
        #region Fields

        protected Vector2 _position;

        public float scale = 4f;
        #endregion

        #region Properties

        public Texture2D _texture { set; get; }
        public virtual Vector2 Position
        {
            get { return _position; }
            set {_position = value;}
        }

        protected float Layer { set; get; }

        /*
        public virtual Rectangle Collisionbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width*(int)scale, _texture.Height*(int)scale);
            }
        }*/

        public Vector2 Origin {set; get; }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0,0,_texture.Width, _texture.Height),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    Layer
                    );
        }

        

        public Sprite(ContentManager Content)
        { }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }
        
        public virtual void Update(GameTime gametime, List<Sprite> sprites)
        {  }
        public override void Update(GameTime gameTime, SpriteBatch spriteBatch)
        { }
    }
}
