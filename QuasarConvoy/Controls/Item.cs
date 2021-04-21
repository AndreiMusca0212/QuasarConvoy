using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Controls
{
    public class Item : Component
    {
        //ItemName Type Rarity Unlocked MaxDropRegion MinDropRegion AvgPrice
        //string string string bool     string        string        int

        #region Fields

        private SpriteFont font;
        private MouseState previousMouse, currentMouse;

        private Texture2D texture;

        private float scale = 1f;

        public string ItemName;
        public string Type;
        public string Rarity;
        public bool Unlocked;
        public string MaxDropRegion;
        public string MinDropRegion;
        public int AvgPrice;

        private int count;

        private DBManager dBManager;

        private string query;

        #endregion

        #region Properties

        public EventHandler Click;

        public bool Clicked { get; private set; }

        public Vector2 Position { get; set; }

        public Vector2 ItemNamePosition
        {
            get
            {
                return new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height + 5);
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        #endregion

        public Item(ContentManager _contentManager, string _ItemName, DBManager _dBManager, float _scale)
        {
            ItemName = _ItemName;

            dBManager = _dBManager;

            query = "SELECT iCount FROM UserInventory WHERE ItemName = '" + ItemName + "'";
            count = int.Parse(_dBManager.SelectElement(query));

            texture = _contentManager.Load<Texture2D>("ItemFrames/" + ItemName);

            scale = scale;

            font = _contentManager.Load<SpriteFont>("Fonts/Font");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "Dehydrated Water x" + count, ItemNamePosition, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            /*
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            */  
         }
    }
}
