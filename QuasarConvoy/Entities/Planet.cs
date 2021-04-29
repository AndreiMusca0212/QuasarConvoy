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
        public PlanetSprite _sprite;
        public Sprite _icon;
        public float Size { get; protected set; }
        public string Type { get; protected set; }
        public bool IsVisible { set; get; }
        public Planet(ContentManager Content)
        {
            IsVisible = false;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

    }
}
