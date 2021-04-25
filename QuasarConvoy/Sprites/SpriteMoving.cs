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
    class SpriteMoving:Sprite
    {
       
        public float Speed = 1f;

        public Vector2 Velocity;

        public float Angle { set; get; }
        protected float AngSpeed { set; get; }
        protected float SpeedCap { set; get; }

        

        public Vector2 Distance(Vector2 dest)
        {
            return new Vector2(this.Position.X - dest.X, this.Position.Y - dest.Y);
        }

        public float TrueAngle(float a1, float a2)
        {
            float ta = a1;
            if (a1 > 0)
                if (a2 > 0)
                    ta = Math.Abs(a1 - a2);
                else
                    ta = a1 + (-1 * a2) < 2 * (float)Math.PI - (a1 + (-1 * a2)) ? a1 + (-1 * a2) : 2 * (float)Math.PI - (a1 + (-1 * a2));
            else
                if (a2 > 0)
                ta = a2 + (-1 * a1) < 2 * (float)Math.PI - (a2 + (-1 * a1)) ? a2 + (-1 * a1) : 2 * (float)Math.PI - (a2 + (-1 * a1));
            else
                ta = Math.Abs(a1 - a2);
            if (ta > Math.PI * 2)
                ta -= (float)Math.PI * 2;
            else
                if (ta < 0)
                ta += (float)Math.PI * 2;
            return ta;
        }
        protected void BackWard()
        {
            Velocity.Y += Speed * (float)Math.Cos(Rotation);
            Velocity.X -= Speed * (float)Math.Sin(Rotation);
        }

        protected void Forward()
        {
            Velocity.Y -= Speed * (float)Math.Cos(Rotation);
            Velocity.X += Speed * (float)Math.Sin(Rotation);
        }

        protected void SpeedLimit()
        {
            if (Velocity.Length() > SpeedCap)
            {
                Velocity.Normalize();
                Velocity *= SpeedCap;
            }
        }
        protected void Rezistance(float amount)
        {
            Vector2 aux = Velocity;
            aux.Normalize();
            Vector2 res = aux * amount;
            if (Velocity.Length() > 0)
            {
                Velocity -= res;
                if (Velocity.Length() < amount)
                    Velocity = Vector2.Zero;
            }
        }
        public void MoveTo(Vector2 destination, bool drift)
        {
            Vector2 dist = new Vector2(this.Position.X - destination.X, this.Position.Y - destination.Y);
            Angle = (float)Math.Atan2(-dist.X, dist.Y);
            //angle = (float)(Math.Acos(Vector2.Dot(dist,new Vector2(10*(float)Math.Cos(Rotation), 10 * (float)Math.Sin(Rotation))) / dist.Length()));

            if (drift)
            {
                if (TrueAngle(Rotation, Angle) > 0.2)
                {
                    if (TrueAngle(Rotation - AngSpeed, Angle) < TrueAngle(Rotation + AngSpeed, Angle))
                        Rotation -= AngSpeed;
                    else
                        Rotation += AngSpeed;
                }
                Forward();
            }
            else
            {
                if (TrueAngle(Rotation, Angle) > 0.2)
                {
                    if (TrueAngle(Rotation - AngSpeed, Angle) < TrueAngle(Rotation + AngSpeed, Angle))
                        Rotation -= AngSpeed;
                    else
                        Rotation += AngSpeed;
                }
                else
                    Forward();
            }

        }
        public SpriteMoving(ContentManager content) : base(content)
        {
            Layer = 0.2f;
        }
        public override void Draw(GameTime gameTime,SpriteBatch spriteBatch)
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
                    Layer
                    );
        }
        
    }
}
