using Microsoft.Xna.Framework;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using QuasarConvoy.Models;

namespace QuasarConvoy.Core
{
    public class Camera
    {
        public Matrix Transform { private set; get; }
        public Matrix offset;
        public Sprite FollowedSprite { get; set; }
        private float MaxZoom=2f;
        public float _zoom=1f;
        public float Zoom
        {
            get {
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
                return _zoom;
            }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
            }
        }

        public Vector2 cameraPos;
        public Vector2 cameraPosOld;
        public Vector2 velocity;
        private float oldZoom;
        public float zoomVelocity;

        public void ZoomIN(float amount)
        {
            if(Zoom<2f)
            Zoom += amount * (Zoom);
        }

        public void ZoomOUT(float amount)
        {
            Zoom -= amount * (Zoom);
        }

        public void ModZoom(Input input=null)
        {
            if (Input.IsPressed(input.ZoomIN))
            {
                oldZoom = Zoom;
                ZoomIN(0.1f);
                zoomVelocity = Zoom - oldZoom;
            }
            else
                if (Input.IsPressed(input.ZoomOUT))
                {
                    oldZoom = Zoom;
                    ZoomOUT(0.1f);
                    zoomVelocity = Zoom - oldZoom;
                }
                else
                    zoomVelocity = 0f;
        }

        public Vector2 ViewPortSize()
        {
            return new Vector2(Game1.ScreenWidth * (1/Zoom), Game1.ScreenHeight * (1/Zoom));
        }
        public Matrix GetZoomScale()
        {
            return Matrix.CreateScale(Zoom, Zoom, 0);
        }
        public void Follow(Sprite target)
        {
            FollowedSprite = target;
            var position = Matrix.CreateTranslation(
                    -target.Position.X - (target._texture.Width*target.scale / 2),
                    -target.Position.Y - (target._texture.Height * target.scale / 2),
                    0);
            offset = Matrix.CreateTranslation(
                Game1.ScreenWidth /( 2 * Zoom),
                Game1.ScreenHeight / (2 * Zoom),
                0);
            cameraPosOld = cameraPos;
            cameraPos = target.Position;
            
            velocity = Vector2.Add(cameraPos, -1 * cameraPosOld);

            Transform = position * offset * GetZoomScale();
        }

        public void Update(Sprite target, Input input = null)
        {
            Follow(target);
            ModZoom(input);
        }
    }
}
