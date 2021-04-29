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
    class Mule1:Ship
    {
        public Mule1(ContentManager content):base(content)
        {
            _texture = content.Load<Texture2D>("mule");
            AngSpeed = 0.05f;
            SpeedCap = 4f;
            Speed = 0.1f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 500;
            Friendly = true;
            Integrity = MaxIntegrity;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            base.Update(gameTime, sprites);
        }

    }
}
