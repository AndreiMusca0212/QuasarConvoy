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
        public Interceptor1(ContentManager content):base(content)
        {
            _texture = content.Load<Texture2D>("interceptor");
            AngSpeed = 0.07f;
            SpeedCap = 6f;
            Speed = 0.15f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 200;
            Integrity = MaxIntegrity;
        }

        public override void Shoot()
        {
            base.Shoot();
        }
    }
}
