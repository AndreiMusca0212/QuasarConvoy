using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Models;
using QuasarConvoy.Sprites;
using QuasarConvoy.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities
{
    class Ship:SpriteMoving
    {
        //bool isAnimated = false;

        //bool isControlled = false;
        protected float shootTimer;

        protected float ShootInterval { set; get; }
        public bool IsControlled { set; get; }
        public int Integrity { set; get; }
        public int MaxIntegrity { set; get; }

        public CombatManager CombatManager = null;
        public Ship(ContentManager content):base(content)
        {
            scale = 0.3f;
        }

        public void Follow(Ship mainShip)
        {
            Vector2 dist = Distance(mainShip.Position);
            if (dist.Length()>200)
            {
                MoveTo(mainShip.Position,true);
            }
            else
            {
                if (Velocity.Length() > 0)
                    Rezistance(0.2f);
            }
        }
        
        
        
        public virtual void MoveControlled(Input Input=null, Ship MainShip = null)
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
                if (Keyboard.GetState().IsKeyDown(Input.Shoot))
                {
                    Shoot();
                }
            }
            
        }

        public virtual void Shoot()
        {
            if (shootTimer >= ShootInterval)
            {
                if (CombatManager != null)
                {
                    CombatManager.AddProjectile(Position, 6f, Rotation, false);
                }
                shootTimer = 0f;
            }
        }
        
        protected void KeepAway(Ship sprit,float minDist)
        {
            Vector2 dist = Distance(sprit.Position);
            Vector2 aux = dist;
            /*float angelDist = (float)Math.Atan2(-dist.X, dist.Y);
            float angleVel= (float)Math.Atan2(-Velocity.X, Velocity.Y);*/
            if (dist.Length() < minDist)
            {
                aux.Normalize();
                aux= aux* (minDist- dist.Length());
                Position += aux;
            }

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            MoveControlled();
            SpeedLimit();
            Rezistance(0.04f);
            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Sprite spri in sprites)
                if (spri is Ship && spri != this)
                    KeepAway((Ship)spri,_texture.Width*scale);
            Position += Velocity;

            //Velocity = Vector2.Zero;
        }
    }
}
