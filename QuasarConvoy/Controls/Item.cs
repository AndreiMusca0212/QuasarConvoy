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
        private int fontSize;
        private MouseState previousMouse, currentMouse;

        private Texture2D texture;

        private float scale = 1f;

        public string ItemName;
        public int ID, itemID;
        private int planetID;

        public int count;

        private DBManager dBManager;

        private string query;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Vector2 Position { get; set; }

        public Vector2 ItemNamePosition
        {
            get
            {
                return new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height);
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
            }
        }

        #endregion
        private void Load(ContentManager _contentManager)
        {
            query = "SELECT ItemCount FROM [UserInventory] WHERE ID = " + ID;
            count = int.Parse(dBManager.SelectElement(query));

            query = "SELECT ItemID FROM [UserInventory] WHERE ID = " + ID;
            itemID = int.Parse(dBManager.SelectElement(query));

            query = "SELECT Name from [Items] WHERE ID = " + itemID;
            ItemName = dBManager.SelectElement(query);

            texture = _contentManager.Load<Texture2D>("ItemFrames/" + ItemName);

            switch(fontSize)
            {
                case 1:
                    font = _contentManager.Load<SpriteFont>("Fonts/EvenSmallerFont");
                    break;
                case 2:
                    font = _contentManager.Load<SpriteFont>("Fonts/SmallFont");
                    break;
                case 3:
                    font = _contentManager.Load<SpriteFont>("Fonts/Font");
                    break;
            }
            
        }

        public Item(ContentManager _contentManager, int _invItemID, DBManager _dBManager, float _scale, int _fontSize) // fontSize = 1 2 3
        {
            ID = _invItemID;

            dBManager = _dBManager;

            scale = _scale;

            fontSize = _fontSize;

            Load(_contentManager);
        }

        private void Load2(ContentManager _contentManager)
        {
            query = "SELECT ItemID FROM [PlanetInventory] WHERE ID = " + ID;
            itemID = int.Parse(dBManager.SelectElement(query));

            query = "SELECT ItemCount FROM [PlanetInventory] WHERE ItemID = " + itemID + " AND PLanetID = " + planetID;
            count = int.Parse(dBManager.SelectElement(query));

            query = "SELECT Name from [Items] WHERE ID = " + itemID;
            ItemName = dBManager.SelectElement(query);

            texture = _contentManager.Load<Texture2D>("ItemFrames/" + ItemName);

            switch (fontSize)
            {
                case 1:
                    font = _contentManager.Load<SpriteFont>("Fonts/EvenSmallerFont");
                    break;
                case 2:
                    font = _contentManager.Load<SpriteFont>("Fonts/SmallFont");
                    break;
                case 3:
                    font = _contentManager.Load<SpriteFont>("Fonts/Font");
                    break;
            }
        }

        public Item(ContentManager _contentManager, DBManager _dBManager, int _ItemID, int _planetID, float _scale, int _fontSize)
        {
            planetID = _planetID;

            dBManager = _dBManager;

            //query = "SELECT ItemID FROM [PlanetInventory] WHERE ID = " + _ItemID;
            ID = _ItemID;

            scale = _scale;

            fontSize = _fontSize;

            Load2(_contentManager);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, ItemName + " x" + count, ItemNamePosition, Color.White);
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
    }
}
