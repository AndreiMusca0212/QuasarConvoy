using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class TradeLoadingState : State
    {
        private DBManager dBManager;

        private string query, loadingMessage = "Trade Complete! Press Space to continue...";

        private SpriteFont font;

        int width, height, planetId;

        int counter = 1;
        int limit = 10;
        float countDuration = 2f;
        float currentTime = 0f;

        public TradeLoadingState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager,List<Item> userInventory, List<Item> planetInventory, int planetID, int userCC, int planetCC) : base(_game, _graphicsDevice, _contentManager)
        {
            font = _contentManager.Load<SpriteFont>("Fonts/Font");
            width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            dBManager = new DBManager();

            planetId = planetID;

            foreach(Item item in userInventory)
            {
                query = "SELECT ID FROM [Items] WHERE Name = '" + item.ItemName + "'";
                int id = int.Parse(dBManager.SelectElement(query));
                query = "SELECT ItemCount FROM [UserInventory] WHERE ItemID = " + id;
                int count = int.Parse(dBManager.SelectElement(query));

                if (count == item.count)
                    query = "DELETE FROM [UserInventory] WHERE ItemID = " + id;
                else
                    query = "UPDATE [UserInventory] SET ItemCount = " + (count - item.count) + " WHERE ItemID = " + id;
                dBManager.QueryIUD(query);
            }

            foreach (Item item in planetInventory)
            {
                query = "SELECT ID FROM [Items] WHERE Name = '" + item.ItemName + "'";
                int id = int.Parse(dBManager.SelectElement(query));
                query = "SELECT ItemCount FROM [PlanetInventory] WHERE ItemID = " + id + " AND PlanetID = " + planetID;
                int count = int.Parse(dBManager.SelectElement(query));

                query = "UPDATE [PlanetInventory] SET ItemCount = " + (count - item.count) + " WHERE ItemID = " + id + " AND PlanetID = " + planetID;
                dBManager.QueryIUD(query);

                query = "SELECT Count(*) FROM [UserInventory] WHERE ItemID = " + id;
                int check = int.Parse(dBManager.SelectElement(query));

                if (check != 0)
                {
                    query = "SELECT ItemCount FROM [UserInventory] WHERE ItemID = " + id;
                    count = int.Parse(dBManager.SelectElement(query));
                    query = "UPDATE [UserInventory] SET ItemCount = " + (count + item.count) + " WHERE ItemID = " + id + ";";
                }
                else
                    query = "INSERT INTO [UserInventory] VALUES(1, " + item.count + ", 1, " + id + ", 1);";
                dBManager.QueryIUD(query);
            }

            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            int result = int.Parse(dBManager.SelectElement(query));
            query = "UPDATE [Saves] SET Currency = " + (result - userCC + planetCC) + " WHERE ID = 1";
            dBManager.QueryIUD(query);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, loadingMessage, new Vector2(width / 2 - 100, height / 2 - 20), Color.White);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                game.ChangeStates(new TradeState(game, graphicsDevice, contentManager, planetId));
        }
    }
}
