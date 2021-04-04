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
        public Sprite FollowedSprite { get; set; }

        public Vector2 cameraPos;
        public Vector2 cameraPosOld;
        public Vector2 velocity;
        public void Follow(Sprite target)
        {
            FollowedSprite = target;
            var position = Matrix.CreateTranslation(
                    -target.Position.X - (target.Collisionbox.Width / 2),
                    -target.Position.Y - (target.Collisionbox.Height/2),
                    0);
            var offset = Matrix.CreateTranslation(
                Game1.ScreenWidth / 2,
                Game1.ScreenHeight / 2,
                0);
            cameraPosOld = cameraPos;
            cameraPos = target.Position;
            velocity = Vector2.Add(cameraPos, -1 * cameraPosOld);

            Transform = position * offset;
        }
    }
}
