using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using QuasarConvoy.Controls;

namespace QuasarConvoy.Controls
{
    public class Button : Component
    {
        #region Fields

        private MouseState currentMouse;

        private SpriteFont font;

        private bool isHovering;

        private MouseState previousMouse;

        private Texture2D texture;

        private SoundEffect soundEffect;

        private int state;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColor { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D _texture, SpriteFont _font, ContentManager _content)
        {
            texture = _texture;

            font = _font;

            PenColor = Color.Black;

            soundEffect = _content.Load<SoundEffect>("Sounds/ButtonTickSound_Zapsplat");

            state = 0;
        }
        public Button(Texture2D _texture, ContentManager _content)
        {
            texture = _texture;

            soundEffect = _content.Load<SoundEffect>("Sounds/ButtonTickSound_Zapsplat");

            state = 0;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;

            if (isHovering)
                color = Color.Gray;
            
            spriteBatch.Draw(texture, Rectangle, color);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            var instance = soundEffect.CreateInstance();
            instance.IsLooped = false;
            if (state == 0)
            {
                if (mouseRectangle.Intersects(Rectangle))
                {
                    instance.Play();
                    state = 1;
                }
            }

            isHovering = false;
            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            } else state = 0;
        }

        #endregion
    }
}
