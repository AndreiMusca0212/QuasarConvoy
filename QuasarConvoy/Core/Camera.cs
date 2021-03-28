using Microsoft.Xna.Framework;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Core
{
    class Camera
    {
        public Matrix Transform { private set; get; }

        public void Follow(Sprite target)
        {
            var position = Matrix.CreateTranslation(
                    -target.Position.X - (target.Collisionbox.Width / 2),
                    -target.Position.Y - (target.Collisionbox.Height/2),
                    0);
            var offset = Matrix.CreateTranslation(
                Game1.ScreenWidth / 2,
                Game1.ScreenHeight / 2,
                0);

            Transform = position * offset;
        }
    }
}
