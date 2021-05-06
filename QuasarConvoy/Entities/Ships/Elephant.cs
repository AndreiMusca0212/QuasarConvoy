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
    class Elephant:Ship
    {
        public int damage = 100;
        private bool confirmedAttack;
        private int battleDistance;
        public Elephant(ContentManager contentManager):base(contentManager)
        {
            _texture = contentManager.Load<Texture2D>("elephant");
            AngSpeed = 0.05f;
            SpeedCap = 6f;
            Speed = 0.08f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 700;
            Integrity = MaxIntegrity;
            Friendly = true;
            ShootInterval = 0.2f;
            agroDistance = 400;
            battleDistance = 100;
        }

        Vector2 escapePos = Vector2.Zero;
        public override void HostileTowards(Ship target)
        {
            if (target != null)
            {
                if (!confirmedAttack)
                {
                    MoveTo(target.Position, false);
                    if (Distance(target.Position).Length() <= target._texture.Width * target.scale / 2 + _texture.Width * scale)
                    {
                        confirmedAttack = true;
                        target.Integrity -= damage;
                        escapePos = Position + new Vector2((float)(agroDistance * Math.Cos(Rotation + Math.PI)) * 2, (float)(agroDistance * Math.Sin(Rotation + Math.PI)) * 2);
                    }
                }
                else
                {
                    MoveTo(escapePos, false);
                    if (Distance(target.Position).Length() >= battleDistance)
                    {
                        confirmedAttack = false;
                    }
                }
            }
            else
            {
                Velocity = Vector2.Zero;
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
                }
                else
                    HostileTowards(target);
                if (target != null)
                    if (Distance(target.Position).Length() > agroDistance)
                        target = null;
                if (target != null && !inCombat)
                    inCombat = true;
                if (target == null && inCombat)
                    inCombat = false;
            }
        }
    }
}
