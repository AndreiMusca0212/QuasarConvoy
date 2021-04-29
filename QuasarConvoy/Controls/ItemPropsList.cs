using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Controls
{
    public class ItemPropsList : Component
    {
        private ContentManager contentManager;

        private DBManager dBManager;

        private Texture2D texture;

        private string itemName, rarity, itemType;

        private int invItemID, count, avgPrice, quality;

        private string query;

        private SpriteFont bigFont, smallFont;

        private Color rarityColor;

        public Rectangle ListFrame { get; set; }

        private void Load()
        {
            bigFont = contentManager.Load<SpriteFont>("Fonts/Font");
            smallFont = contentManager.Load<SpriteFont>("Fonts/SmallFont");


            query = "SELECT Quality FROM [UserInventory] WHERE ID = " + invItemID;
            quality = int.Parse(dBManager.SelectElement(query));

            query = "SELECT ItemCount FROM [UserInventory] WHERE ID = " + invItemID;
            count = int.Parse(dBManager.SelectElement(query));

            query = "SELECT ItemID FROM [UserInventory] WHERE ID = " + invItemID;
            int itemID = int.Parse(dBManager.SelectElement(query));

            query = "SELECT Name FROM [Items] WHERE ID = " + itemID;
            itemName = dBManager.SelectElement(query);

            texture = contentManager.Load<Texture2D>("ItemFrames/" + itemName);

            query = "SELECT Rarity FROM [Items] WHERE ID = " + itemID;
            rarity = dBManager.SelectElement(query);

            query = "SELECT AvgPrice FROM [Items] WHERE ID = " + itemID;
            avgPrice = int.Parse(dBManager.SelectElement(query));

            query = "SELECT ItemTypeID FROM [Items] WHERE ID = " + itemID;
            int typeID = int.Parse(dBManager.SelectElement(query));

            query = "SELECT Label FROM [ItemType] WHERE ID = " + typeID;
            itemType = dBManager.SelectElement(query);
        }

        public ItemPropsList(ContentManager _contentManager, int _invItemID, DBManager _dBManager)
        {
            invItemID = _invItemID;
            contentManager = _contentManager;
            dBManager = _dBManager;

            Load();

            switch(rarity)
            {
                case "COMMON": rarityColor = Color.AntiqueWhite;
                    break;
                case "RARE": rarityColor = Color.CornflowerBlue;
                    break;
                case "EPIC": rarityColor = Color.Indigo;
                    break;
                case "LEGENDARY": rarityColor = Color.Orchid;
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(smallFont, itemName, new Vector2(ListFrame.X, ListFrame.Y), Color.White);
            spriteBatch.Draw(texture, new Vector2(ListFrame.X, ListFrame.Y + 30), null, Color.White, 0, new Vector2(0, 0), 0.9f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bigFont, "x" + count, new Vector2(ListFrame.X, ListFrame.Y + (texture.Height * 0.9f) + 60), Color.White);
            spriteBatch.DrawString(bigFont, rarity, new Vector2(ListFrame.X, ListFrame.Y + ListFrame.Height / 2), rarityColor);
            spriteBatch.DrawString(smallFont, avgPrice + " CC", new Vector2(ListFrame.X, ListFrame.Y + ListFrame.Height / 2 + 50), Color.Gold);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
