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
    class Unicorn:Ship
    {
        public Unicorn(ContentManager contentManager):base(contentManager)
        {
            _texture = contentManager.Load<Texture2D>("unicorn");
            AngSpeed = 0.1f;
            SpeedCap = 12f;
            Speed = 0.17f;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            MaxIntegrity = 200;
            Friendly = true;
            Integrity = MaxIntegrity;
        }

    }
}
