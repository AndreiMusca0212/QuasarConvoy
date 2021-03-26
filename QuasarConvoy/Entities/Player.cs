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

        
        #endregion

        public Player(ContentManager Content):base(Content)
        {
            _animations = new Dictionary<string, Animation>()
            {
                {"W_Right", new Animation(Content.Load<Texture2D>("bbw_R"),4) },
                {"W_Left", new Animation(Content.Load<Texture2D>("bbw_L"),4) },
                {"W_Front", new Animation(Content.Load<Texture2D>("bbw_F"),4) },

            };
            _animationManager = new AnimationManager(_animations.First().Value);
            scale = 4;
        }

        protected virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
            {
                Velocity.Y -= Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Input.Down))
            {
                Velocity.Y += Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Velocity.X -= Speed;
            }
            if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Velocity.X += Speed;
            }
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
            SetAnimations();
            foreach (Sprite spri in sprites)
                Collide(spri);

            base.Update(gameTime, sprites);
            Position += Velocity;
            Velocity = Vector2.Zero;
        }
    }
}
