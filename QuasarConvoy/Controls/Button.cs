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
        private DBManager dBManager;

        private MouseState currentMouse;

        private MouseState previousMouse;

        private SpriteFont font;

        private bool isHovering;

        private Texture2D texture;

        private SoundEffect soundEffect;

        private float scale;

        private int state, SoundLevel;
        
        private string query = "SELECT SoundLevel FROM [User] WHERE ID = 1";

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
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D _texture, SpriteFont _font, ContentManager _content, float _scale)
        {
            dBManager = new DBManager();
            string aux = dBManager.SelectElement(query);
            SoundLevel = int.Parse(aux);

            //Button Specifics
            texture = _texture;

            font = _font;

            scale = _scale;

            PenColor = Color.Black;

            soundEffect = _content.Load<SoundEffect>("Sounds/ButtonTickSound_Zapsplat");

            state = 0;
        }
        public Button(Texture2D _texture, ContentManager _content)
        {
            dBManager = new DBManager();
            
            string aux = dBManager.SelectElement(query);
            SoundLevel = int.Parse(aux);

            //Button Specifics
            texture = _texture;

            scale = 1;

            soundEffect = _content.Load<SoundEffect>("Sounds/ButtonTickSound_Zapsplat");

            state = 0;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            if (isHovering)
                color = Color.Gray;

            if (scale == 1)
                spriteBatch.Draw(texture, Rectangle, color);
            else
                spriteBatch.Draw(texture, Position, null, color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            SoundLevel = int.Parse(dBManager.SelectElement(query));

            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            var instance = soundEffect.CreateInstance();
            instance.Volume = instance.Volume * SoundLevel / 100;
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
