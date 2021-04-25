using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities.Planets
{
    class Terran:Planet
    {
        public Terran(ContentManager content) : base(content)
        {
            _sprite = new Sprite(content.Load<Texture2D>("TerranPlanet"));
            Type = "terran";
            Size = 1f;
        }
    }
}
