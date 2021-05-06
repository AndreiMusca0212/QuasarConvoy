using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Models;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities.Ships
{
    class Collector:Ship
    {
        int battleDistance = 300;
        
        public Collector(ContentManager content):base(content)
        {
            _texture = content.Load<Texture2D>("collector");
            AngSpeed = 0.06f;
            SpeedCap = 9f;//ideal 10f
            Speed = 0.12f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 300;
            Integrity = MaxIntegrity;
            Friendly = true;
            ShootInterval = 0.3f;
            agroDistance = 500;
        }

        public override void Shoot()
        {
            if (shootTimer >= ShootInterval)
            {
                if (CombatManager != null)
                {
                    CombatManager.AddProjectile(Position + new Vector2((float)(20*Math.Cos(Rotation)), (float)(20 *Math.Sin(Rotation))), 0.2f, 4, 20f, Rotation, this);
                    CombatManager.AddProjectile(Position - new Vector2((float)(20 * Math.Cos(Rotation)), (float)(20 * Math.Sin(Rotation))), 0.2f, 4, 20f, Rotation, this);
                }
                shootTimer = 0f;
            }
        }

        public override void HostileTowards(Ship target)
        {
            if (target != null)
            {
                if (Distance(target.Position).Length() < battleDistance - 20)
                {
                    BackWard();
                }
                else
                    if (Distance(target.Position).Length() > battleDistance + 20)
                    Forward();
                TurnTowards(target.Position, 0.05f);
                if (IsTurnedTowards(target.Position, 0.05f))
                    Shoot();
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            base.Update(gameTime, sprites);
            if (!IsControlled)
            {
                if (target == null || target.IsRemoved)
                {
                    if (blacklist.Count > 0)
                        target = PickTargetClosest(blacklist);
                    else target = null;
                }
                else
                    HostileTowards(target);
                if (target !=null)
                    if(Distance(target.Position).Length() > agroDistance)
                        target = null;
                if (target != null && !inCombat)
                    inCombat = true;
                if (target == null && inCombat)
                    inCombat = false;
            }
        }
    }
}
