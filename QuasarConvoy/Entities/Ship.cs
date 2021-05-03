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
    public class Ship:SpriteMoving
    {
        //bool isAnimated = false;

        //bool isControlled = false;
        protected float shootTimer;
        public bool hasHealthbar = false;
        protected float ShootInterval { set; get; }
        public bool IsControlled { set; get; }
        public int Integrity { set; get; }
        public int MaxIntegrity { set; get; }
        public bool Friendly { set; get; }
        public List<Ship> blacklist;
        public float Stability=0.04f;
        public bool inCombat=false;

        public int ID;

        public CombatManager CombatManager = null;
        public Ship(ContentManager content):base(content)
        {
            scale = 0.3f;
            IsRemoved = false;
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
                    CombatManager.AddProjectile(Position,0.2f,10, 6f, Rotation, this);
                }
                shootTimer = 0f;
            }
        }

        public virtual void HostileTowards(Ship target)
        {

        }

        public Ship PickTargetRandom(List<Ship> ships)
        {
            Random r = new Random();
            if (ships.Count > 0)
            {
                int i = r.Next(0, ships.Count - 1);
                while (ships[i].Friendly == this.Friendly)
                    i = r.Next(0, ships.Count - 1);
                return ships[i];
            }
            else
                return null;
        }
        public Ship PickTargetClosest(List<Ship> ships)
        {
            if (ships.Count > 0)
            {
                int indTarg = 0;
                bool found = false;
                for (int i = 0; i < ships.Count; i++)
                    if (ships[i].Friendly != Friendly)
                    {
                        if (Distance(ships[i].Position).Length() < Distance(ships[indTarg].Position).Length())
                        {
                            indTarg = i;
                        }
                        found = true;
                    }
                if (found)
                    return ships[indTarg];
                else
                    return null;
            }
            else
                return null;
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
            if (Integrity <= 0)
                IsRemoved = true;
            MoveControlled();
            SpeedLimit();
            Rezistance(Stability);
            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Sprite spri in sprites)
                if (spri is Ship && spri != this)
                    KeepAway((Ship)spri,_texture.Width*scale);
            Position += Velocity;
            //Velocity = Vector2.Zero;
        }
    }
}
