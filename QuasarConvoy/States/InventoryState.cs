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
        private string query, location;
        private int currency;

        private Texture2D techEffectTexture, inventoryBoxTexture;
        private Rectangle techEffectFrame, inventoryBoxFrame;
        private Vector2 locationFrame, currencyFrame;


        private Button MSButton, CS1Button, CS2Button, CS3Button;
        private Texture2D MStexture, CS1, CS2, CS3;
        private int selectedInventory; // 0 1 2 3

        private Texture2D itemMeasure;

        SpriteFont font;

        private List<Component> components, items;

        public InventoryState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            font = _contentManager.Load<SpriteFont>("Fonts/Font");
            int width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            int height = _graphicsDevice.PresentationParameters.BackBufferHeight;
            selectedInventory = 0;

            itemMeasure = _contentManager.Load<Texture2D>("ItemFrames/Dehydrated Water");

            techEffectTexture = _contentManager.Load<Texture2D>("UI Stuff/Images/Inventory Tech Effect");
            techEffectFrame = new Rectangle(0, 0, width, techEffectTexture.Height);

            inventoryBoxTexture = _contentManager.Load<Texture2D>("UI Stuff/Inventory Box");
            inventoryBoxFrame = new Rectangle(width / 15, 3 * techEffectFrame.Height / 2, width / 2 + inventoryBoxTexture.Width / 5,
                                            height - 2 * techEffectFrame.Height);
            

            MStexture = _contentManager.Load<Texture2D>("UI Stuff/Buttons/MSInventory");
            MSButton = new Button(MStexture, _contentManager)
            {
                Position = new Vector2(inventoryBoxFrame.X + 5, inventoryBoxFrame.Y + 5),
            };
            MSButton.Click += MSButton_Click;

            CS1 = _contentManager.Load<Texture2D>("UI Stuff/Buttons/ConvoyShip1");
            CS1Button = new Button(CS1, _contentManager)
            {
                Position = new Vector2(MSButton.Position.X + MStexture.Width + 5, MSButton.Position.Y),
            };
            CS1Button.Click += CS1Button_Click;



            dBManager = new DBManager();

            /*query = "SELECT Planet FROM [User] WHERE ID = 1";
            location = dBManager.SelectElement(query);
            locationFrame = new Vector2(width / 15, techEffectTexture.Height / 5);*/

            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            currency = int.Parse(dBManager.SelectElement(query));
            currencyFrame = new Vector2(width - techEffectTexture.Width / 10, techEffectTexture.Height / 5);

            query = "DELETE FROM [UserInventory] WHERE ItemCount = 0";
            dBManager.QueryIUD(query);


            List<string> data = dBManager.SelectColumnFrom("UserInventory", "ItemName");
            int count = data.Count, contor1 = 0, contor2 = 0;
            /*
                Now, for each item in the User's inventory, I will create a full profile
                and I will display in the Inventory box the information he has access to, which would be:
                
                ItemName, Type, Rarity, AvgPrice, Quality, iCount
            */
            items = new List<Component>();
            foreach(string itemProps in data)
            {
                Item currentItem = new Item(_contentManager, itemProps, dBManager, 0.5f)
                {
                    Position = new Vector2(MSButton.Position.X + MStexture.Width * contor1, 
                                        MSButton.Position.Y + MStexture.Height + itemMeasure.Height * contor2 + 5),
                };
                items.Add(currentItem);

                contor1++;
                if (contor1 == 3)
                {
                    contor1 = 0;
                    contor2++;
                }
            }

            components = new List<Component>()
            {
                MSButton,
                CS1Button,
            };
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Upper Tech Effect - Current User Location - Currency - Inventory Box
            spriteBatch.Draw(techEffectTexture, techEffectFrame, Color.White);
            spriteBatch.DrawString(font, "Location: " + location, locationFrame, Color.White);
            spriteBatch.DrawString(font, currency + " CC", currencyFrame, Color.White);
            spriteBatch.Draw(inventoryBoxTexture, inventoryBoxFrame, Color.White);

            //Elements of InventoryState
            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            //The User's Inventory
            foreach (var item in items)
                item.Draw(gameTime, spriteBatch);

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

            //The User's Inventory
            foreach (var item in items)
                item.Update(gameTime);
        
            switch(selectedInventory)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        //-----------------------------------------------------
        private void MSButton_Click(object sender, EventArgs e)
        {
            selectedInventory = 0;
        }
        private void CS1Button_Click(object sender, EventArgs e)
        {
            selectedInventory = 1;
        }
        private void CS3Button_Click(object sender, EventArgs e)
        {
            selectedInventory = 2;
        }
        private void CS2Button_Click(object sender, EventArgs e)
        {
            selectedInventory = 3;
        }
    }
}
