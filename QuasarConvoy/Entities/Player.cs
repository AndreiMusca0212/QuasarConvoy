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
using QuasarConvoy.Sprites;

namespace QuasarConvoy.Entities
{
    class Player:SpriteAnimated
    {
        #region Sprite Properties&Fields
        public Input Input =
            new Input()
            {
                Up = Keys.W,
                Down = Keys.S,
                Left = Keys.A,
                Right = Keys.D,

            };
        bool isAnimated = false;
        float angSpeed = 0.1f;

        #endregion
        
        public Player(ContentManager Content,bool animated):base(Content)
        {
            if (animated)
                isAnimated = true;
            else
                _texture = Content.Load<Texture2D>("mule");
            scale = 0.3f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            Speed = 0.5f;
            /*_animations = new Dictionary<string, Animation>()
            {
            };
            _animationManager = new AnimationManager(_animations.First().Value);
            scale = 4;*/

        }
        

        protected virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
            {
                Velocity.Y -= Speed * (float)Math.Cos(Rotation);
                Velocity.X += Speed * (float)Math.Sin(Rotation);
            }
            if (Keyboard.GetState().IsKeyDown(Input.Down))
            {
                Velocity.Y += Speed * (float)Math.Cos(Rotation);
                Velocity.X -= Speed * (float)Math.Sin(Rotation);
            }
            if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Rotation-=angSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Rotation += angSpeed;
            }
            if(Keyboard.GetState().IsKeyDown(Input.Reset))
            {
                Position = new Vector2(150, 150);
                Velocity = Vector2.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0, 0, _texture.Width, _texture.Height),
                    Color.White,
                    Rotation,
                    Origin,
                    (float)scale,
                    SpriteEffects.None,
                    0f
                    );
        }
        protected void Collide(Sprite sprit)
        {
            if (isTouchingLeft(sprit) || isTouchingRight(sprit))
                Velocity.X = 0;
            if (isTouchingTop(sprit) || isTouchingBot(sprit))
                Velocity.Y = 0;
        }
        protected override void SetAnimations()
        {
            if (Velocity.X > 0)
                _animationManager.Play(_animations["W_Right"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["W_Left"]);
            else
                _animationManager.Play(_animations["W_Front"]);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();
            if(isAnimated)
                SetAnimations();
            foreach (Sprite spri in sprites)
                Collide(spri);

            if(isAnimated)base.Update(gameTime, sprites);
            Position += Velocity;
            
            //Velocity = Vector2.Zero;
        }
    }
}
