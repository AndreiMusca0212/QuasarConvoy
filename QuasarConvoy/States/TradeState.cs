using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Controls;
using QuasarConvoy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuasarConvoy.States
{
    public class TradeState : State
    {
        private SpriteFont font, midFont;

        private Texture2D textureBar, InventoryBox, TransactionInventoryBox, tradeButtonTexture, itemMeasure;
        private Rectangle textureBarFrame, userInventoryBoxFrame, planetInventoryBoxFrame, userTransactionFrame, planetTransactionFrame;

        private Texture2D scrollDown, scrollUp, currencyScrollDownTexture, currencyScrollUpTexture;
        private Button userInventoryScrollDown, userInventoryScrollUp, userTransactionScrollDown, userTransactionScrollUp,
                planetInventoryScrollDown, planetInventoryScrollUp, planetTransactionScrollDown, planetTransactionScrollUp;
        private Button tradeButton, currencyScrollDown, currencyScrollUp;

        private int transactionCurrencyValue = 0, planetCurrencyValue = 0;
        private string currency, transactionCurrency = "0 CC";
        private Vector2 currencyPos;
        private bool balance = true;

        private DBManager dBManager;
        private string query;
        private int planetID;

        float width, height;

        private int userItemCount, planetItemCount, userRow1 = 0, userRow2 = 1, planetRow1 = 0, planetRow2 = 1;
        private int userTransactionIndex = 0, planetTransactionIndex = 0;
        private int userTransactionRowCount = 0, planetTransactionRowCount = 0;
        private List<Item>[] userInventoryRows, planetInventoryRows, userTransactionRows, planetTransactionRows;
        private List<string> data;

        private int userValue = 0, planetValue = 0;

        private float scale = 0.6f;
        private List<Component> components;

        public void Load(ContentManager _contentManager)
        {
            font = _contentManager.Load<SpriteFont>("Fonts/Font");
            midFont = _contentManager.Load<SpriteFont>("Fonts/SmallFont");
            textureBar = _contentManager.Load<Texture2D>("UI Stuff/Images/TradeTexture Bar");
            InventoryBox = _contentManager.Load<Texture2D>("UI Stuff/Images/TradeState InventoryBox");
            TransactionInventoryBox = _contentManager.Load<Texture2D>("UI Stuff/Images/TransactionInventory");
            scrollDown = _contentManager.Load<Texture2D>("UI Stuff/ScrollDown");
            scrollUp = _contentManager.Load<Texture2D>("UI Stuff/ScrollUp");
            itemMeasure = _contentManager.Load<Texture2D>("ItemFrames/Dehydrated Water");
            tradeButtonTexture = _contentManager.Load<Texture2D>("UI Stuff/Buttons/Trade Button");
            currencyScrollDownTexture = _contentManager.Load<Texture2D>("UI Stuff/Currency Scroll2");
            currencyScrollUpTexture = _contentManager.Load<Texture2D>("UI Stuff/Currency Scroll");
        }

        public TradeState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager, int _planetID) : base(_game, _graphicsDevice, _contentManager)
        {
            Load(_contentManager);

            width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            height = _graphicsDevice.PresentationParameters.BackBufferHeight;
            planetID = _planetID;

            
            textureBarFrame = new Rectangle(0, (int)height / 25, (int)width, 30);
            userInventoryBoxFrame = new Rectangle(30, textureBarFrame.Y + textureBarFrame.Height + 30,
                                (int)width / 2 - 60, (int)(height / 2));
            planetInventoryBoxFrame = new Rectangle((int)width / 2 + 30, userInventoryBoxFrame.Y, (int)width / 2 - 60,
                                 (int)(height / 2));

            userTransactionFrame = new Rectangle(userInventoryBoxFrame.X, userInventoryBoxFrame.Y + userInventoryBoxFrame.Height + 30,
                                userInventoryBoxFrame.Width, (int)height - userInventoryBoxFrame.X - userInventoryBoxFrame.Height - 160);
            planetTransactionFrame = new Rectangle(planetInventoryBoxFrame.X, userTransactionFrame.Y,
                                planetInventoryBoxFrame.Width, userTransactionFrame.Height);

            SetButtons(_contentManager); //scris separat pt ca mult

            dBManager = new DBManager();
            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            long result = long.Parse(dBManager.SelectElement(query));
            int key; string unitPrefix;
            for (key = 0; key <= 6 && ((int)(result / Math.Pow(10, 3 * key)) != 0); key++) ;
            key--;
            switch (key)
            {
                case 1:
                    unitPrefix = " k";
                    break;
                case 2:
                    unitPrefix = " m";
                    break;
                case 3:
                    unitPrefix = " b";
                    break;
                case 4:
                    unitPrefix = " g";
                    break;
                case 5:
                    unitPrefix = " t";
                    break;
                case 6:
                    unitPrefix = " t";
                    break;
                default:
                    unitPrefix = " ";
                    break;
            }
            long putere = Convert.ToInt64(Math.Pow(10, 3 * key));
            currency = (int)(result / putere) + unitPrefix + "CC";
            currencyPos = new Vector2(width / 25, textureBarFrame.Y);

            //---------------------------------------------------------

            query = "DELETE FROM [UserInventory] WHERE ItemCount = 0";
            dBManager.QueryIUD(query);

            data = dBManager.SelectColumnFrom("[UserInventory]", "ID");
            userItemCount = data.Count;

            userInventoryRows = new List<Item>[10];
            for (int i = 0; i <= userItemCount / 4; i++)
                userInventoryRows[i] = new List<Item>();
            for (int i = 0; i < userItemCount; i++)
            {
                int ID = int.Parse(data[i]);
                Item currentItem = new Item(_contentManager, ID, dBManager, scale, 1) { Position = new Vector2() };
                currentItem.Click += delegate(object sender, EventArgs e) { CurrentUserItem_Click(sender, e, _contentManager, currentItem); };
                userInventoryRows[i / 4].Add(currentItem);
            }

            data = dBManager.SpecificSelectColumnFrom("[PlanetInventory]", "ID", "PlanetID", planetID + "");
            planetItemCount = data.Count;

            planetInventoryRows = new List<Item>[10];
            for (int i = 0; i <= planetItemCount / 4; i++)
                planetInventoryRows[i] = new List<Item>();
            for (int i = 0; i < planetItemCount; i++)
            {
                int ID = int.Parse(data[i]);
                Item currentItem = new Item(_contentManager, dBManager, ID, planetID, scale, 1) { Position = new Vector2() };
                currentItem.Click += delegate(object sender, EventArgs e) { CurrentPlanetItem_Click(sender, e, _contentManager, currentItem); };
                planetInventoryRows[i / 4].Add(currentItem);
            }

            planetTransactionRows = new List<Item>[10];
            userTransactionRows = new List<Item>[10];

            components = new List<Component>()
            {
                userInventoryScrollDown,
                userInventoryScrollUp,
                userTransactionScrollDown,
                userTransactionScrollUp,
                planetInventoryScrollDown,
                planetInventoryScrollUp,
                planetTransactionScrollDown,
                planetTransactionScrollUp,
                currencyScrollDown,
                currencyScrollUp,
                tradeButton,
            };

        }


        #region Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(textureBar, textureBarFrame, Color.White);
            spriteBatch.DrawString(font, currency, currencyPos, Color.White);

            spriteBatch.Draw(InventoryBox, userInventoryBoxFrame, Color.White);
            spriteBatch.Draw(InventoryBox, planetInventoryBoxFrame, Color.White);
            spriteBatch.Draw(TransactionInventoryBox, userTransactionFrame, Color.White);
            spriteBatch.Draw(TransactionInventoryBox, planetTransactionFrame, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);

            spriteBatch.DrawString(font, transactionCurrency, new Vector2(currencyScrollUp.Position.X + 
                                    currencyScrollUpTexture.Width + 10, currencyScrollUp.Position.Y + 10), Color.White);
            spriteBatch.DrawString(font, planetCurrencyValue + " CC", new Vector2(planetTransactionFrame.X +
                                3 * planetTransactionFrame.Width / 4, currencyScrollUp.Position.Y + 10), Color.White);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            if (userInventoryRows[userRow1] != null)
                foreach (Item item in userInventoryRows[userRow1].ToList())
                    item.Draw(gameTime, spriteBatch);
            if(userInventoryRows[userRow2] != null)
                foreach (Item item in userInventoryRows[userRow2].ToList())
                    item.Draw(gameTime, spriteBatch);

            if (planetInventoryRows[planetRow1] != null)
                foreach (Item item in planetInventoryRows[planetRow1].ToList())
                    item.Draw(gameTime, spriteBatch);
            if(planetInventoryRows[planetRow2] != null)
                foreach (Item item in planetInventoryRows[planetRow2].ToList())
                    item.Draw(gameTime, spriteBatch);

            if (planetTransactionRows[planetTransactionIndex] != null)
                foreach (Item item in planetTransactionRows[planetTransactionIndex].ToList())
                    item.Draw(gameTime, spriteBatch);
            if (userTransactionRows[userTransactionIndex] != null)
                foreach (Item item in userTransactionRows[userTransactionIndex].ToList())
                    item.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
        #endregion

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        #region Update
        public override void Update(GameTime gameTime)
        {
            Input Input = new Input(Keyboard.GetState());
            if (Input.WasPressed(Keys.Escape))
            {
                game.ChangeStates(new GameState(game, graphicsDevice, contentManager));
            }

            foreach (var component in components)
                component.Update(gameTime);

            int itemInRowCount = 1;
            if (userInventoryRows[userRow1] != null)
                foreach (Item item in userInventoryRows[userRow1].ToList())
                {
                    item.Position = new Vector2(userInventoryBoxFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1) + 20, userInventoryBoxFrame.Y + 30);
                    itemInRowCount++;

                    item.Update(gameTime);
                }

            itemInRowCount = 1;
            if(userInventoryRows[userRow2] != null)
                foreach (Item item in userInventoryRows[userRow2].ToList())
                {
                    item.Position = new Vector2(userInventoryBoxFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1) + 20, userInventoryBoxFrame.Y +
                        (int)(itemMeasure.Height * scale) + 60);
                    itemInRowCount++;

                    item.Update(gameTime);
                }

            itemInRowCount = 1;
            if(planetInventoryRows[planetRow1] != null)
                foreach (Item item in planetInventoryRows[planetRow1].ToList())
                {
                    item.Position = new Vector2(planetInventoryBoxFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1) + 20, planetInventoryBoxFrame.Y + 30);
                    itemInRowCount++;

                    item.Update(gameTime);
                }

            itemInRowCount = 1;
            if(planetInventoryRows[planetRow2] != null)
                foreach (Item item in planetInventoryRows[planetRow2].ToList())
                {
                    item.Position = new Vector2(planetInventoryBoxFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1) + 20, planetInventoryBoxFrame.Y +
                        (int)(itemMeasure.Height * scale) + 60);
                    itemInRowCount++;

                    item.Update(gameTime);
                }

            itemInRowCount = 1;
            if(planetTransactionRows[planetTransactionIndex] != null)
                foreach(Item item in planetTransactionRows[planetTransactionIndex].ToList())
                {
                    item.Position = new Vector2(planetTransactionFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1) + 20, planetTransactionFrame.Y + 5);
                    itemInRowCount++;

                    item.Click += delegate(object sender, EventArgs e) { PlanetItem_Click(sender, e, item); };

                    item.Update(gameTime);
                }

            itemInRowCount = 1;
            if (userTransactionRows[userTransactionIndex] != null)
                foreach (Item item in userTransactionRows[userTransactionIndex].ToList())
                {
                    item.Position = new Vector2(userTransactionFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1) + 20, userTransactionFrame.Y + 5);
                    itemInRowCount++;

                    item.Click += delegate (object sender, EventArgs e) { UserItem_Click(sender, e, item); };

                    item.Update(gameTime);
                }

            planetCurrencyValue = userValue - planetValue;
        }

        
        #endregion

        #region ButtonSetting
        private void SetButtons(ContentManager _contentManager)
        {
            userInventoryScrollDown = new Button(scrollDown, _contentManager)
            {
                Position = new Vector2(userInventoryBoxFrame.X + userInventoryBoxFrame.Width - scrollDown.Width - 10,
                                    userInventoryBoxFrame.Y + userInventoryBoxFrame.Height - scrollDown.Height - 10),
            };
            userInventoryScrollDown.Click += UserInventoryScrollDown_Click;

            userInventoryScrollUp = new Button(scrollUp, _contentManager)
            {
                Position = new Vector2(userInventoryScrollDown.Position.X, userInventoryBoxFrame.Y + 10),
            };
            userInventoryScrollUp.Click += UserInventoryScrollUp_Click;

            userTransactionScrollUp = new Button(scrollUp, _contentManager)
            {
                Position = new Vector2(userInventoryScrollDown.Position.X, userTransactionFrame.Y + 5),
            };
            userTransactionScrollUp.Click += UserTransactionScrollUp_Click;

            userTransactionScrollDown = new Button(scrollDown, _contentManager)
            {
                Position = new Vector2(userInventoryScrollDown.Position.X, 
                                    userTransactionScrollUp.Position.Y + scrollUp.Height + 10),
            };
            userTransactionScrollDown.Click += UserTransactionScrollDown_Click;
            

            planetInventoryScrollDown = new Button(scrollDown, _contentManager)
            {
                Position = new Vector2(planetInventoryBoxFrame.X + planetInventoryBoxFrame.Width - scrollDown.Width - 10,
                                    userInventoryScrollDown.Position.Y),
            };
            planetInventoryScrollDown.Click += PlanetInventoryScrollDown_Click;

            planetInventoryScrollUp = new Button(scrollUp, _contentManager)
            {
                Position = new Vector2(planetInventoryScrollDown.Position.X, userInventoryScrollUp.Position.Y),
            };
            planetInventoryScrollUp.Click += PlanetInventoryScrollUp_Click;

            planetTransactionScrollDown = new Button(scrollDown, _contentManager)
            {
                Position = new Vector2(planetInventoryScrollDown.Position.X, userTransactionScrollDown.Position.Y),
            };
            planetTransactionScrollDown.Click += PlanetTransactionScrollDown_Click;

            planetTransactionScrollUp = new Button(scrollUp, _contentManager)
            {
                Position = new Vector2(planetInventoryScrollDown.Position.X, userTransactionScrollUp.Position.Y),
            };
            planetTransactionScrollUp.Click += PlanetTransactionScrollUp_Click;

            currencyScrollUp = new Button(currencyScrollUpTexture, midFont, _contentManager, 0.8f)
            {
                Position = new Vector2(userTransactionFrame.X + 5, userTransactionFrame.Y + userTransactionFrame.Height -  2 * currencyScrollUpTexture.Height),
            };
            currencyScrollUp.Click += CurrencyScrollUp_Click;

            currencyScrollDown = new Button(currencyScrollDownTexture, midFont, _contentManager, 0.8f)
            {
                Position = new Vector2(currencyScrollUp.Position.X, currencyScrollUp.Position.Y + currencyScrollUpTexture.Height + 5),
            };
            currencyScrollDown.Click += CurrencyScrollDown_Click;

            tradeButton = new Button(tradeButtonTexture, midFont, _contentManager, 0.7f)
            {
                Position = new Vector2((planetTransactionFrame.X + (userTransactionFrame.X + userTransactionFrame.Width)) / 2 - (tradeButtonTexture.Width * 0.7f) / 2, 
                                        userTransactionFrame.Y + userTransactionFrame.Height - 30),
            };
            tradeButton.Click += TradeButton_Click;

        }

        #endregion

        #region Many click

        private void UserInventoryScrollDown_Click(object sender, EventArgs e)
        {
            if (userRow2 != userItemCount / 4 && userInventoryRows[userRow2] != null)
                userRow1 = userRow2++;
        }
        private void UserInventoryScrollUp_Click(object sender, EventArgs e)
        {
            if (userRow1 != 0)
                userRow2 = userRow1--;
        }
        private void UserTransactionScrollUp_Click(object sender, EventArgs e)
        {
            if (userTransactionIndex != 0)
                userTransactionIndex--;
        }
        private void UserTransactionScrollDown_Click(object sender, EventArgs e)
        {
            int k = 0;
            foreach (List<Item> list in userTransactionRows)
                if (list != null) k++;
                else break;

            if (userTransactionIndex != k - 1)
                userTransactionIndex++;
        }


        private void PlanetInventoryScrollDown_Click(object sender, EventArgs e)
        {
            if (planetRow2 != planetItemCount / 4 && planetInventoryRows[planetRow2] != null)
                planetRow1 = planetRow2++;
        }
        private void PlanetInventoryScrollUp_Click(object sender, EventArgs e)
        {
            if (planetRow1 != 0)
                planetRow2 = planetRow1--;
        }
        private void PlanetTransactionScrollUp_Click(object sender, EventArgs e)
        {
            if (planetTransactionIndex != 0)
                planetTransactionIndex--;
        }
        private void PlanetTransactionScrollDown_Click(object sender, EventArgs e)
        {
            int k = 0;
            foreach (List<Item> list in planetTransactionRows)
                if (list != null) k++;
                else break;

            if (planetTransactionIndex != k - 1)
                planetTransactionIndex++;
        }

        private void CurrentUserItem_Click(object sender, EventArgs e, ContentManager _contentManager, Item item)
        {
            bool altaVariabilaimDead = false;
            if (item.count != 0)
            { 
                item.count--;
                query = "SELECT AvgPrice FROM [Items] WHERE ID = " + item.itemID;
                int result = int.Parse(dBManager.SelectElement(query));
                userValue += result;
                altaVariabilaimDead = true; 
            }

            //query = "SELECT ID FROM [Items] WHERE Name = '" + item.ItemName + "'";
            //int id = int.Parse(dBManager.SelectElement(query));
            //query = "SELECT ID FROM [UserInventory] WHERE ItemID = " + id;
            //id = int.Parse(dBManager.SelectElement(query));

            Item newItem = new Item(_contentManager, item.ID, dBManager, scale, 1);
            newItem.count = 1;

            bool sem = false;
            int k = 0;
            if (userTransactionRows[0] != null)
            {
                if (altaVariabilaimDead)
                {
                    foreach (List<Item> list in userTransactionRows)
                    {
                        if (list != null)
                        {
                            int index = 0;
                            while (index != list.Count)
                            {
                                if (list[index].ItemName == newItem.ItemName)
                                {
                                    sem = true;
                                    list[index].count++;
                                    break;
                                }
                                index++;
                            }

                            k++;
                        }
                        else break;
                    }

                    if (!sem)
                    {
                        int length = userTransactionRows[userTransactionRowCount].Count;
                        if (length + 1 == 5)
                        {
                            userTransactionRowCount++;
                            userTransactionRows[userTransactionRowCount] = new List<Item>();
                        }
                        userTransactionRows[userTransactionRowCount].Add(newItem);
                    }
                }
            }
            else
            {
                userTransactionRows[0] = new List<Item>();
                userTransactionRows[0].Add(newItem);
            }
        }

        private void CurrentPlanetItem_Click(object sender, EventArgs e, ContentManager _contentManager, Item item)
        {
            bool altaVariabilaimDead = false;
            if (item.count > 0)
            { 
                item.count--;
                query = "SELECT AvgPrice FROM [Items] WHERE ID = " + item.itemID;
                int result = int.Parse(dBManager.SelectElement(query));
                planetValue += result;
                altaVariabilaimDead = true; 
            }

            //query = "SELECT ID FROM [Items] WHERE Name = '" + item.ItemName + "'";
            //int id = int.Parse(dBManager.SelectElement(query));
            //query = "SELECT ID FROM [PlanetInventory] WHERE PlanetID = " + planetID + " AND ItemID = " + id;
            //id = int.Parse(dBManager.SelectElement(query));

            Item newItem = new Item(_contentManager, dBManager, item.ID, planetID, scale, 1);
            newItem.count = 1;

            bool sem = false;
            int k = 0;
            if (planetTransactionRows[0] != null)
            {
                if (altaVariabilaimDead)
                {
                    foreach (List<Item> list in planetTransactionRows)
                    {
                        if (list != null)
                        {
                            int index = 0;
                            while (index != list.Count)
                            {
                                if (list[index].ItemName == newItem.ItemName)
                                {
                                    sem = true;
                                    list[index].count++;
                                    break;
                                }
                                index++;
                            }

                            k++;
                        }
                        else break;
                    }

                    if (!sem)
                    {
                        int length = planetTransactionRows[planetTransactionRowCount].Count;
                        if (length + 1 == 5)
                        {
                            planetTransactionRowCount++;
                            planetTransactionRows[planetTransactionRowCount] = new List<Item>();
                        }
                        planetTransactionRows[planetTransactionRowCount].Add(newItem);
                    }
                }
            }
            else
            {
                planetTransactionRows[0] = new List<Item>();
                planetTransactionRows[0].Add(newItem);
            }
        }

        private void PlanetItem_Click(object sender, EventArgs e, Item item)
        {
            if(item.count >= 1)
            {
                item.count--;
                query = "SELECT AvgPrice FROM [Items] WHERE ID = " + item.itemID;
                int result = int.Parse(dBManager.SelectElement(query));
                planetValue -= result;
                foreach (List<Item> list in planetInventoryRows)
                {
                    if (list != null)
                    {
                        int index = 0;
                        while (index != list.Count)
                        {
                            if (list[index].ItemName == item.ItemName)
                            {
                                list[index].count++;
                                break;
                            }
                            index++;
                        }
                    }
                    else break;
                }
            }
        }

        private void UserItem_Click(object sender, EventArgs e, Item item)
        {
            if (item.count >= 1)
            {
                item.count--;
                query = "SELECT AvgPrice FROM [Items] WHERE ID = " + item.itemID;
                int result = int.Parse(dBManager.SelectElement(query));
                userValue -= result;
                foreach (List<Item> list in userInventoryRows)
                {
                    if (list != null)
                    {
                        int index = 0;
                        while (index != list.Count)
                        {
                            if (list[index].ItemName == item.ItemName)
                            {
                                list[index].count++;
                                break;
                            }
                            index++;
                        }
                    }
                    else break;
                }
            }
        }


        private void TradeButton_Click(object sender, EventArgs e)
        {
            if (userValue >= planetValue)
            {
                List<Item> userInventory = new List<Item>();
                List<Item> planetInventory = new List<Item>();
                foreach(List<Item> list in userTransactionRows)
                {
                    if (list != null)
                        foreach (Item item in list)
                            userInventory.Add(item);
                }

                foreach (List<Item> list in planetTransactionRows)
                {
                    if (list != null)
                        foreach (Item item in list)
                            planetInventory.Add(item);
                }

                game.ChangeStates(new TradeLoadingState(game, graphicsDevice, contentManager, userInventory, planetInventory, planetID, transactionCurrencyValue, planetCurrencyValue));
            }
        }

        private void CurrencyScrollDown_Click(object sender, EventArgs e)
        {
            if (transactionCurrencyValue != 0)
            {
                transactionCurrencyValue -= 50;
                userValue -= 50;
            }
            transactionCurrency = transactionCurrencyValue + " CC";
        }

        private void CurrencyScrollUp_Click(object sender, EventArgs e)
        {
            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            long result = long.Parse(dBManager.SelectElement(query));

            if (transactionCurrencyValue + 50 != 10000 && transactionCurrencyValue + 50 < result)
            {
                transactionCurrencyValue += 50;
                userValue += 50;
            }
            transactionCurrency = transactionCurrencyValue + " CC";
        }

        #endregion
    }
}
