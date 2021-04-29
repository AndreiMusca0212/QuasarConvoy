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
    class PirateBrawler : Ship
    {
        private bool confirmedAttack;
        private int damage = 80;
        private float agroDistance;
        private Ship target;
        
        public PirateBrawler(ContentManager content) : base(content)
        {
            _texture = content.Load<Texture2D>("PirateBrawler");
            AngSpeed = 0.07f;
            scale = 0.1f;
            SpeedCap = 10f;//ideal 10f
            Speed = 0.15f;
            Stability = 0.04f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 200;
            Integrity = MaxIntegrity;
            Friendly = false;
            confirmedAttack = false;
            agroDistance = 500;
        }

        Vector2 escapePos=Vector2.Zero;
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
                    if (Distance(target.Position).Length() >= agroDistance)
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
            if (target == null||target.IsRemoved)
            {
                if (blacklist.Count > 0)
                    target = PickTargetClosest(blacklist);
            }
            else
                HostileTowards(target);
        }
    }
}