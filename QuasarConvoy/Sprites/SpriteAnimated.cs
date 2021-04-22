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
    
    class SpriteAnimated:Sprite
    {
        #region Fields
        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        public float Speed = 1f;

        public Vector2 Velocity;
        #endregion

        #region Properties
        public override Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        /*
        public override Rectangle Collisionbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
            }
        }*/
        
        #endregion
        public override void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            base.Draw(gameTime,spriteBatch);
                if (_animationManager != null)
                    _animationManager.Draw(spriteBatch,scale);
                else throw new Exception("Not ok");
        }

        
        public SpriteAnimated(ContentManager Content):base(Content)
        {

        }

        protected virtual void SetAnimations()
        {
            
        }

        #region Collisions
        /*
        protected bool isTouchingLeft(Sprite obj)
        {
            return this.Collisionbox.Left + this.Velocity.X < obj.Collisionbox.Right &&
                this.Collisionbox.Top < obj.Collisionbox.Bottom &&
                this.Collisionbox.Bottom > obj.Collisionbox.Top &&
                this.Collisionbox.Right > obj.Collisionbox.Right;
        }

        protected bool isTouchingRight(Sprite obj)
        {
            return this.Collisionbox.Right + this.Velocity.X > obj.Collisionbox.Left &&
                this.Collisionbox.Top < obj.Collisionbox.Bottom &&
                this.Collisionbox.Bottom > obj.Collisionbox.Top &&
                this.Collisionbox.Left < obj.Collisionbox.Left;
        }

        protected bool isTouchingTop(Sprite obj)
        {
            return this.Collisionbox.Top + this.Velocity.Y < obj.Collisionbox.Bottom &&
                this.Collisionbox.Bottom > obj.Collisionbox.Bottom &&
                this.Collisionbox.Left < obj.Collisionbox.Right &&
                this.Collisionbox.Right > obj.Collisionbox.Left;
        }

        protected bool isTouchingBot(Sprite obj)
        {
            return this.Collisionbox.Bottom + this.Velocity.Y > obj.Collisionbox.Top &&
                this.Collisionbox.Top < obj.Collisionbox.Top &&
                this.Collisionbox.Left < obj.Collisionbox.Right &&
                this.Collisionbox.Right > obj.Collisionbox.Left;
        }
        */

        #endregion

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        { 
            _animationManager.Update(gameTime);
        }
    }
}
