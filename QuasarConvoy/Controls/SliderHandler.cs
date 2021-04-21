using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Controls
{
    public class SliderHandler : Component
    {
        DBManager dBManager;
        private string query;

        SpriteFont font;
        private Texture2D handlerTexture;
        private Rectangle handlerFrame, _slideFrame;
        private int boundX1, boundX2;

        private Color handlerColor;
        private int soundlevel;


        public SliderHandler(GraphicsDevice _graphicsDevice, ContentManager _contentManager, Rectangle sliderFrame)
        {
            dBManager = new DBManager();
            query = "SELECT SoundLevel FROM UserInfo WHERE ID = 1";
            soundlevel = int.Parse(dBManager.SelectElement(query));

            font = _contentManager.Load<SpriteFont>("Fonts/Font");
            handlerTexture = _contentManager.Load<Texture2D>("Controls/Slider Handler");
            
            int x = sliderFrame.X + (soundlevel * sliderFrame.Width)/100;
            int y = sliderFrame.Y - 2;
            boundX1 = sliderFrame.X;
            boundX2 = sliderFrame.X + sliderFrame.Width;
            _slideFrame = sliderFrame;
            handlerFrame = new Rectangle(x, y, handlerTexture.Width, handlerTexture.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(handlerTexture, handlerFrame, handlerColor);
            //spriteBatch.DrawString(font, soundlevel.ToString(), new Vector2(boundX2 + 10, handlerFrame.Y), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            handlerColor = Color.White;
            if (mouseRectangle.Intersects(handlerFrame)) handlerColor = Color.Gray;

            if((mouseRectangle.Intersects(handlerFrame) || mouseRectangle.Intersects(_slideFrame)) && mouse.LeftButton == ButtonState.Pressed)
            {
                handlerColor = Color.Gray;
                handlerFrame.X = mouse.X - handlerTexture.Width / 2;
            }
            if (handlerFrame.X <= boundX1) handlerFrame.X = boundX1 + 1;
            if (handlerFrame.X + handlerTexture.Width >= boundX2) handlerFrame.X = boundX2 - handlerTexture.Width - 1;

            if (mouse.LeftButton == ButtonState.Released)
            {
                soundlevel = (handlerFrame.X - _slideFrame.X) * 100 / _slideFrame.Width;
                query = "UPDATE UserInfo SET SoundLevel = " + soundlevel + " WHERE ID = 1;";
                dBManager.QueryIUD(query);
            }
        }
    }
}
