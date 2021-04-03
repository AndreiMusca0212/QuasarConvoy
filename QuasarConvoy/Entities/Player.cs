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
    class Player
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
        
        int shipIndex=0;
        
        public Ship ControlledShip { set; get; }
        #endregion
        
        public Player(Ship ship)
        {
            ControlledShip = ship;
        }

        public void SwitchShip(List<Ship> convoy)
        {
            if (shipIndex > convoy.Count)
                shipIndex = 0;

        }
        #region Old player(ship)
        /*
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
                Rotation = 0f;
            }
            if(Keyboard.GetState().IsKeyDown(Input.Reset)&& Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Position = new Vector2(150, 150);
                Velocity = Vector2.Zero;
                Rotation = (float)Math.PI/2;
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
                    0.2f
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
        private void SpeedLimit()
        {
            if (Velocity.Length() > speedCap)
            {
                Velocity.Normalize();
                Velocity*= speedCap;
            }
            
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();
            if (isAnimated)
                SetAnimations();
            foreach (Sprite spri in sprites)
                Collide(spri);

            if (isAnimated) base.Update(gameTime, sprites);
            SpeedLimit();
            Position += Velocity;

            //Velocity = Vector2.Zero;
        }*/
        #endregion

        public void Update(GameTime gametime, List<Sprite> sprites)
        {
            if (!ControlledShip.IsControlled)
                ControlledShip.IsControlled = true;
            ControlledShip.Move(Input);
            ControlledShip.Update(gametime, sprites);
        }


    }
}
