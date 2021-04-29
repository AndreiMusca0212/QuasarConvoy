using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities.Planets
{
    class Dry:Planet
    {
        public Dry(ContentManager content):base(content)
        {
            _sprite = new PlanetSprite(content.Load<Texture2D>("DessertPlanet"));
            Type = "dessert";
            Size = 1.2f;
            _sprite.renderDistance = (int)Size * 10000;
        }
    }
}
