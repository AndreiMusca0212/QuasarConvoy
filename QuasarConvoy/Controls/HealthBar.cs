using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Controls
{
    class HealthBar:Component
    {
        private Ship _ship;
        public Vector2 Position { set; get; }
        public Texture2D texture;
        private Color color1;
        public Vector2 _size;
        private GraphicsDevice graphics;
        private float prop;
        public HealthBar(Ship ship, Color color, Vector2 size, GraphicsDevice graphicsDevice)
        {
            _ship = ship;
            color1 = color;
            _size = size;
            graphics = graphicsDevice;
            prop = size.X / ship.Integrity;
            IsRemoved = false;
            texture = new Texture2D(graphicsDevice, 1, 1);
        }
        public override void Update(GameTime gameTime)
        {
            if (_ship.Integrity > 0)
                texture = new Texture2D(graphics, (int)(_ship.Integrity * prop), (int)_size.Y);
            else
                IsRemoved = true;
            Color[] data = new Color[texture.Width*texture.Height];
            for (int i = 0; i < data.Length;i++) data[i] = color1;
            texture.SetData(data);
            Position = _ship.Position + new Vector2(0, _ship._texture.Height/2*_ship.scale);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position,new Rectangle(0,0,texture.Width,texture.Height), Color.White , 0f,new Vector2(_size.X/2,0),1f,SpriteEffects.None,0.6f);
        }
    }
}
