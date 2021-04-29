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
    class PirateSniper : Ship
    {
        private bool confirmedAttack;
        private int damage = 80;
        private float agroDistance;
        private float campRange=30;
        private Ship target;

        public PirateSniper(ContentManager content) : base(content)
        {
            _texture = content.Load<Texture2D>("PirateSniper");
            AngSpeed = 0.07f;
            scale = 0.1f;
            SpeedCap = 10f;//ideal 10f
            Speed = 0.10f;
            Stability = 0.04f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 150;
            Integrity = MaxIntegrity;
            ShootInterval = 0.4f;
            Friendly = false;
            confirmedAttack = false;
            agroDistance = 300;
        }
        public override void Shoot()
        {
            if (shootTimer >= ShootInterval)
            {
                if (CombatManager != null)
                {
                    CombatManager.AddProjectile(Position,0.8f,20, 40f, Rotation, this);
                }
                shootTimer = 0f;
            }
        }
        public override void HostileTowards(Ship target)
        {
            if (target != null)
            {
                if (Distance(target.Position).Length() < agroDistance - 20)
                {
                    BackWard();
                }
                else
                    if (Distance(target.Position).Length() > agroDistance + 20)
                    Forward();
                TurnTowards(target.Position,0.2f);
                if (IsTurnedTowards(target.Position,0.2f))
                    Shoot();
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            base.Update(gameTime, sprites);
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
