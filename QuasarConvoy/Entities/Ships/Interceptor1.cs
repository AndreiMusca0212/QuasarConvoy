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
    class Interceptor1:Ship
    {
        int battleDistance = 300;
        public Interceptor1(ContentManager content):base(content)
        {
            _texture = content.Load<Texture2D>("interceptor");
            AngSpeed = 0.07f;
            SpeedCap = 10f;//ideal 10f
            Speed = 0.15f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 200;
            Integrity = MaxIntegrity;
            Friendly = true;
            ShootInterval = 0.2f;
            agroDistance = 1000;
        }

        public override void Shoot()
        {
            if (shootTimer >= ShootInterval)
            {
                if (CombatManager != null)
                {
                    CombatManager.AddProjectile(Position,0.2f,10, 20f, Rotation, this);
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
                TurnTowards(target.Position,0.05f);
                if (IsTurnedTowards(target.Position,0.05f))
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
                if (target != null && !inCombat)
                    inCombat = true;
                if (target == null && inCombat)
                    inCombat = false;
            }
        }
    }
}
