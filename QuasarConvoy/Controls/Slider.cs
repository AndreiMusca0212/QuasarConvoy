using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Controls
{
    public partial class Slider : Component
    {
        public Texture2D sliderTexture;
        private Rectangle sliderFrame;

        SliderHandler handler;

        public Slider(GraphicsDevice _graphicsDevice, ContentManager _contentManager, int X, int Y)
        {
            sliderTexture = _contentManager.Load<Texture2D>("Controls/Sound Bar");
            sliderFrame = new Rectangle(X, Y, sliderTexture.Width, sliderTexture.Height);

            handler = new SliderHandler(_graphicsDevice, _contentManager, sliderFrame);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sliderTexture, sliderFrame, Color.White);
            handler.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            handler.Update(gameTime);
        }
    }
}
