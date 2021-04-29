using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using QuasarConvoy.Controls;
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

        public float rott=0f;

        protected Vector2 _position;

        public float scale = 1f;


        public float Rotation
        {
            set
            {
                rott = value;
                while (rott > Math.PI)
                    rott -= (float)Math.PI * 2;
                while (rott < -1 * Math.PI)
                    rott += (float)Math.PI * 2;
            }

            get { return rott; }
        }
        #endregion

        #region Properties

        public Texture2D _texture { set; get; }
        public virtual Vector2 Position
        {
            get { return _position; }
            set {_position = value;}
        }

        public float Layer { set; get; }

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

        public override void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0,0,_texture.Width, _texture.Height),
                    Color.White,
                    Rotation,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    Layer
                    );
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 origin)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0, 0, _texture.Width, _texture.Height),
                    Color.White,
                    Rotation,
                    origin,
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

        public Sprite(Sprite original)
        {
            _texture = original._texture;
            scale = original.scale;
            Rotation = original.Rotation;
            Position = original.Position;
        }
        
        public virtual void Update(GameTime gametime, List<Sprite> sprites)
        {  }
        public virtual void Update(GameTime gameTime, SpriteBatch spriteBatch)
        { }

        public override void Update(GameTime gameTime)
        { }
    }
}
