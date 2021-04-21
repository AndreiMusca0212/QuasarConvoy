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
            int screenWidth = Game1.ScreenWidth;
            int screenHeight = Game1.ScreenHeight;
            Position += (-1) * cam.velocity * scale + (new Vector2(Position.X-screenWidth/2,Position.Y-screenHeight/2))*0.2f*cam.zoomVelocity;
            if (base.Position.X > screenWidth + 10 || base.Position.X < -10 || base.Position.Y < -10 || base.Position.Y > screenHeight + 10)
                isRemoved = true;
        }
    }
}
