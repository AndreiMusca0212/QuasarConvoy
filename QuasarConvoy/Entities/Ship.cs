using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Models;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities
{
    class Ship:SpriteAnimated
    {
        bool isAnimated = false;
        
        //bool isControlled = false;

        protected float SpeedCap { set; get; }
        protected float AngSpeed { set; get; }
        public bool IsControlled { set; get; }
        public Ship(ContentManager content):base(content)
        {
            scale = 0.3f;
        }

        public void Follow(Ship mainShip)
        {
            Vector2 dist = new Vector2(this.Position.X - mainShip.Position.X, this.Position.Y - mainShip.Position.Y);
            if (dist.Length()>200)
            {
                MoveTo(mainShip.Position);
            }
            else
            {
                if (Velocity.Length() > 0)
                    Rezistance(0.2f);
            }
        }

        public void MoveTo(Vector2 destination)
        {
            Vector2 dist = new Vector2(this.Position.X - destination.X, this.Position.Y - destination.Y);
            float angle =(float) Math.Atan2(dist.X,-dist.Y) + (float)Math.PI;
            if (Math.Abs(this.Rotation-angle)>0.2)
            {
                if (this.Rotation > angle)
                    Rotation -= AngSpeed;
                else
                    Rotation += AngSpeed;
            }
            else
                Forward();
            
        }
        public virtual void Move(Input Input=null, Ship MainShip = null)
        {
            if (Input != null)
            {
                if (Keyboard.GetState().IsKeyDown(Input.Up))
                {
                    Forward();
                }
                if (Keyboard.GetState().IsKeyDown(Input.Down))
                {
                    BackWard();
                }
                if (Keyboard.GetState().IsKeyDown(Input.Left))
                {
                    Rotation -= AngSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Input.Right))
                {
                    Rotation += AngSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Input.Reset))
                {
                    Position = new Vector2(150, 150);
                    Velocity = Vector2.Zero;
                    Rotation = 0f;
                }
                if (Keyboard.GetState().IsKeyDown(Input.Reset) && Keyboard.GetState().IsKeyDown(Input.Right))
                {
                    Position = new Vector2(150, 150);
                    Velocity = Vector2.Zero;
                    Rotation = (float)Math.PI / 2;
                }
            }
            
        }

        private void BackWard()
        {
            Velocity.Y += Speed * (float)Math.Cos(Rotation);
            Velocity.X -= Speed * (float)Math.Sin(Rotation);
        }

        private void Forward()
        {
            Velocity.Y -= Speed * (float)Math.Cos(Rotation);
            Velocity.X += Speed * (float)Math.Sin(Rotation);
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
                    scale,
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
            if (Velocity.Length() > SpeedCap)
            {
                Velocity.Normalize();
                Velocity *= SpeedCap;
            }
        }
        private void Rezistance(float amount)
        {
            Vector2 aux = Velocity;
            aux.Normalize();
            Vector2 res =  aux * amount;
            if (Velocity.Length()>0)
            {
                Velocity -= res;
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
            Rezistance(0.01f);
            Position += Velocity;

            //Velocity = Vector2.Zero;
        }
    }
}
