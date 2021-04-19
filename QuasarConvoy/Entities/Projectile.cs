using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using QuasarConvoy.Core;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using QuasarConvoy.Ambient;

namespace QuasarConvoy.Entities
{
    class Projectile:SpriteMoving
    {
        public bool isRemoved;
        public bool friendly;
        private float speedInd;
        public Projectile(ContentManager content,float velocity,float rott):base(content)
        {
            Layer = 0.2f;
            _texture = content.Load<Texture2D>("Pew");
            scale = 1f;
            speedInd = velocity;
            Rotation = rott - (float)Math.PI/2f;
            Velocity = speedInd * new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        }

        private void CheckGone(Camera cam)
        {
            if(Distance(cam.cameraPos).Length() > 11000)
            {
                isRemoved = true;
            }
        }

        private void Move()
        {
            
            
        }
        public void Update(GameTime gametime,Camera cam)
        {
            CheckGone(cam);
            Position += Velocity;
        }


    }
}
