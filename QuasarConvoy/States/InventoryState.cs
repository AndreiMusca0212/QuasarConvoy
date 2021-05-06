using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Controls;
using QuasarConvoy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class InventoryState : State
    {
        private DBManager dBManager;
        private string query;
        private string currency;
        private Vector2 currencyPos;
        private Vector2 currencyFrame;

        private Texture2D techEffectTexture, inventoryBoxTexture;
        private Rectangle techEffectFrame, inventoryBoxFrame;

        private List<Item>[] Rows, RowsSorted;
        private int row1 = 0, row2 = 1;
        private int itemCount;

        private Button scrollUpButton, scrollDownButton, previousItemTypeButton, nextItemTypeButton;
        private Texture2D scrollUp, scrollDown, previousItemType, nextItemType;
        private float scale = 0.8f;

        private Texture2D itemMeasure;
        private ItemPropsList itemPropsList;
        private int itemDisplay = 5, previousItemDisplay = 6;

        SpriteFont font;
        private List<string> ItemTypes, data;
        private List<Component> components;
        int width, height;

        public void Load(ContentManager _contentManager)
        {
            font = _contentManager.Load<SpriteFont>("Fonts/Font");
            itemMeasure = _contentManager.Load<Texture2D>("ItemFrames/Dehydrated Water");
            techEffectTexture = _contentManager.Load<Texture2D>("UI Stuff/Images/Inventory Tech Effect");
            inventoryBoxTexture = _contentManager.Load<Texture2D>("UI Stuff/Inventory Box");
            scrollUp = _contentManager.Load<Texture2D>("UI Stuff/ScrollUp");
            scrollDown = _contentManager.Load<Texture2D>("UI Stuff/ScrollDown");
            previousItemType = _contentManager.Load<Texture2D>("UI Stuff/previousShipButton");
            nextItemType = _contentManager.Load<Texture2D>("UI Stuff/nextShipButton");
        }

        public InventoryState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            Load(_contentManager);
            width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            techEffectFrame = new Rectangle(0, 0, width, techEffectTexture.Height);
            inventoryBoxFrame = new Rectangle(width / 20, 3 * techEffectFrame.Height / 2, (int)(0.8f * itemMeasure.Width * 5 + 200),
                                            height - 2 * techEffectFrame.Height);
            

            scrollUpButton = new Button(scrollUp, _contentManager)
            {
                Position = new Vector2(inventoryBoxFrame.X + inventoryBoxFrame.Width - scrollUp.Width - 10, inventoryBoxFrame.Y + 10),
            };
            scrollUpButton.Click += ScrollUpButton_Click;

            scrollDownButton = new Button(scrollDown, _contentManager)
            {
                Position = new Vector2(inventoryBoxFrame.X + inventoryBoxFrame.Width - scrollDown.Width - 10, inventoryBoxFrame.Y + inventoryBoxFrame.Height - scrollDown.Height - 10),
            };
            scrollDownButton.Click += ScrollDownButton_Click;

            previousItemTypeButton = new Button(previousItemType, _contentManager)
            {
                Position = new Vector2(inventoryBoxFrame.X + 5, inventoryBoxFrame.Y + 5),
            };
            previousItemTypeButton.Click += PreviousItemTypeButton_Click;

            nextItemTypeButton = new Button(nextItemType, _contentManager)
            {
                Position = new Vector2(previousItemTypeButton.Position.X + previousItemType.Width + 200, previousItemTypeButton.Position.Y),
            };
            nextItemTypeButton.Click += NextItemTypeButton_Click;

            dBManager = new DBManager();
            
            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            long result = Convert.ToInt64(dBManager.SelectElement(query));
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
            currencyPos = new Vector2(width - techEffectTexture.Width / 8, techEffectTexture.Height / 6);


            /*query = "SELECT Planet FROM [User] WHERE ID = 1";
            location = dBManager.SelectElement(query);
            locationFrame = new Vector2(width / 15, techEffectTexture.Height / 5);*/

            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            currency = dBManager.SelectElement(query);
            currencyFrame = new Vector2(width - techEffectTexture.Width / 10, techEffectTexture.Height / 5);

            query = "DELETE FROM [UserInventory] WHERE ItemCount = 0";
            dBManager.QueryIUD(query);

            ItemTypes = dBManager.SelectColumnFrom("[ItemType]", "Label");
            ItemTypes.Add("ALL ITEMS");
            /*
                Now, for each item in the User's inventory, I will create a full profile
                and I will display in the Inventory box the information he has access to, which would be:
                
                ItemName, Type, Rarity, AvgPrice, Quality, ItemCount
            */

            data = dBManager.SelectColumnFrom("[UserInventory]", "ID");
            itemCount = data.Count;

            Rows = new List<Item>[10];
            for (int i = 0; i <= itemCount / 5; i++)
                Rows[i] = new List<Item>();
            for(int i = 0; i < itemCount; i++)
            {
                int ID = int.Parse(data[i]);
                Item currentItem = new Item(_contentManager, ID, dBManager, scale, 2) { Position = new Vector2()};
                currentItem.Click += delegate(object sender, EventArgs e) { CurrentItem_Click(sender, e, ID); };
                Rows[i / 5].Add(currentItem);
            }

            
            components = new List<Component>()
            {
                scrollUpButton,
                scrollDownButton,
                previousItemTypeButton,
                nextItemTypeButton,
            };
        }

        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Upper Tech Effect - Currency - Inventory Box
            spriteBatch.Draw(techEffectTexture, techEffectFrame, Color.White);
            spriteBatch.DrawString(font, currency, currencyPos, Color.White);
            spriteBatch.Draw(inventoryBoxTexture, inventoryBoxFrame, Color.White);
            spriteBatch.DrawString(font, ItemTypes[itemDisplay].Trim(), new Vector2(previousItemTypeButton.Position.X + (nextItemTypeButton.Position.X - previousItemTypeButton.Position.X - previousItemType.Width) / 2, previousItemTypeButton.Position.Y + previousItemType.Height / 3), Color.BlanchedAlmond);

            //Elements of InventoryState
            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            //The User's Inventory
            if(RowsSorted[row1]!= null)
                foreach (Item item in RowsSorted[row1])
                    item.Draw(gameTime, spriteBatch);
            if (RowsSorted[row2] != null)
                foreach (Item item in RowsSorted[row2])
                    item.Draw(gameTime, spriteBatch);
            

            if(itemPropsList != null)
                itemPropsList.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        Input Input = new Input(Keyboard.GetState());
        public override void Update(GameTime gameTime)
        {
            //Escape InventoryState with either 'I' or 'Esc'
            currentInventoryState = currentEscState = Keyboard.GetState();

            bool key1Pressed = (currentInventoryState.IsKeyUp(Keys.I) && previousInventoryState.IsKeyDown(Keys.I));

            if (Input.WasPressed(Keys.I, Keyboard.GetState()) ||Input.WasPressed(Keys.Escape))
                game.ChangeStates(game.GameState);
            Input.Refresh();
            /*
            previousInventoryState = currentInventoryState;

            bool key2Pressed = (currentEscState.IsKeyUp(Keys.Escape) && previousEscState.IsKeyDown(Keys.Escape));

            if (key2Pressed)
                game.ChangeStates(game.GameState);
            previousEscState = currentEscState;*/

            //Elements of InventoryState
            foreach (var component in components)
                component.Update(gameTime);

            if (previousItemDisplay != itemDisplay)
            {
                previousItemDisplay = itemDisplay;

                RowsSorted = new List<Item>[10];
                for (int i = 0; i <= itemCount / 5; i++)
                    RowsSorted[i] = new List<Item>();
                int k = 0, number = 0;
                foreach (List<Item> list in Rows)
                {
                    if (list == null) break;
                    foreach (Item item in list)
                    {
                        query = "SELECT ItemID FROM [UserInventory] WHERE ID = " + item.ID;
                        int id = int.Parse(dBManager.SelectElement(query));
                        query = "SELECT ItemTypeID FROM [Items] WHERE ID = " + id;
                        int typeID = int.Parse(dBManager.SelectElement(query));
                        if (typeID == itemDisplay + 1 || itemDisplay == 5)
                        {
                            RowsSorted[k].Add(item); number++;
                            if (number == 5)
                            {
                                k++;
                                number = 0;
                            }
                        }
                    }
                }
            }

            int itemInRowCount = 1;
            if (RowsSorted[row1] != null)
                foreach (Item item in RowsSorted[row1])
                {
                    item.Position = new Vector2(inventoryBoxFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1), inventoryBoxFrame.Y +
                        previousItemTypeButton.Rectangle.Height + 20);
                    itemInRowCount++;

                    item.Update(gameTime);
                }

            itemInRowCount = 1;
            if(RowsSorted[row2] != null)
                foreach (Item item in RowsSorted[row2])
                {
                    item.Position = new Vector2(inventoryBoxFrame.X + 10 * itemInRowCount +
                        (int)(itemMeasure.Width * scale) * (itemInRowCount - 1), inventoryBoxFrame.Y +
                        previousItemTypeButton.Rectangle.Height + 100 + (int)(itemMeasure.Height * scale));
                    itemInRowCount++;

                    item.Update(gameTime);
                }

            // Position = new Vector2(inventoryBoxFrame.X + 10 * itemInRowCount + (int)(itemMeasure.Width * scale) * (itemCount - 1),
            //inventoryBoxFrame.Y + previousShipButton.Rectangle.Width + 10 * itemRow + (int)(itemMeasure.Width * scale) * (itemRow - 1))
        }

        //-----------------------------------------------------

        private void CurrentItem_Click(object sender, EventArgs e, int id)
        {
            itemPropsList = new ItemPropsList(contentManager, id, dBManager) 
            {
                ListFrame = new Rectangle(inventoryBoxFrame.X + inventoryBoxFrame.Width + 40, inventoryBoxFrame.Y, width - inventoryBoxFrame.X - inventoryBoxFrame.Width - 40, inventoryBoxFrame.Height),
            };
        }

        private void ScrollDownButton_Click(object sender, EventArgs e)
        {
            if (row2 != itemCount / 5 && RowsSorted[row2] != null)
            {
                row1 = row2;
                row2++;
            }
        }

        private void ScrollUpButton_Click(object sender, EventArgs e)
        {
            if(row1 != 0)
            {
                row2 = row1;
                row1--;
            }
        }

        private void NextItemTypeButton_Click(object sender, EventArgs e)
        {
            if (itemDisplay + 1 == 6) itemDisplay = 0;
            else itemDisplay++;
        }

        private void PreviousItemTypeButton_Click(object sender, EventArgs e)
        {
            if (itemDisplay - 1 < 0) itemDisplay = 5;
            else itemDisplay--;
        }
    }
}
