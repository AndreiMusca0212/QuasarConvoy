using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities
{
    public class Planet
    {
        public Vector2 Position { set; get; }
        public Sprite _sprite;
        public float Size { get; protected set; }
        public string Type { get; protected set; }
        public Planet(ContentManager Content)
        {

        }

    }
}
