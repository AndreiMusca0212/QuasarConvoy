using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Core;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Ambient
{
    class Particle:Sprite
    {
        public bool isRemoved = false;
        public Vector2 Velocity { set; get; }
        public Particle(Texture2D texture, float sca):base(texture)
        {
            scale = sca;
            Layer = 0.1f;
        }

        public void Update(Camera cam)
        {
            Position += (-1) * cam.velocity*scale;
            if (Position.X > Game1.ScreenWidth+10 || Position.X < -10 || Position.Y < -10 || Position.Y > Game1.ScreenHeight+10)
                isRemoved = true;
        }
    }
}
