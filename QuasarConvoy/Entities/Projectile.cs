using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using QuasarConvoy.Core;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using QuasarConvoy.Ambient;

namespace QuasarConvoy.Entities
{
    class Projectile:SpriteMoving
    {
        public Projectile(ContentManager content):base(content)
        {
            Layer = 0.2f;
            _texture = content.Load<Texture2D>("Pew");

        }

        public void Update(GameTime gametime)
        {
            Position += Velocity;
        }

    }
}
