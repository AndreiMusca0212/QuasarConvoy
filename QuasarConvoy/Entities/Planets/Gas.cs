using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities.Planets
{
    class Gas:Planet
    {
        public Gas(ContentManager content):base(content)
        {
            _sprite = new PlanetSprite(content.Load<Texture2D>("GasGiant"));
            Type = "giant";
            Size = 3f;
            _sprite.renderDistance = (int)Size * 10000;
        }
    }
}
