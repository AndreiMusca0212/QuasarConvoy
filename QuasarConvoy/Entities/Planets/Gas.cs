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
            _sprite = new Sprite(content.Load<Texture2D>("GasGiant"));
            Type = "giant";
            Size = 6f;
        }
    }
}
