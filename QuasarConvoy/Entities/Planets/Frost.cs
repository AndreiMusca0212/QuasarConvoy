using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities.Planets
{
    class Frost:Planet
    {
        public Frost(ContentManager content):base(content)
        {
            _sprite = new Sprite(content.Load<Texture2D>("FrozenPlanet"));
            Type = "frozen";
            Size = 1f;
        }
    }
}
