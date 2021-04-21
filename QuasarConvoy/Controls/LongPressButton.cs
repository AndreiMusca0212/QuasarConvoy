using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Controls
{
    public class LongPressButton : Component
    {
        #region Fields
        private DBManager dBManager;

        private MouseState currentMouse;

        private MouseState previousMouse;

        private SpriteFont font;

        private Texture2D texture;
        
        private bool Released;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color HoverColor { get; set; }

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

        public LongPressButton(Texture2D _texture, ContentManager contentManager)
        {
            texture = _texture;

            HoverColor = Color.White;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Clicked) HoverColor = Color.Gray;
            if (Released) HoverColor = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
