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
    public class GameState : State
    {
        DBManager dBManager;
        private string query;

        private SpriteFont font;

        struct element
        {
            public int x;
            public int y;
            public string value;
        }
        element currencyDisplay;

        public GameState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            float width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            float height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            dBManager = new DBManager();
            query = "SELECT Currency FROM UserInfo WHERE ID = 1";
            int result = int.Parse(dBManager.SelectElement(query));
            
            font = _contentManager.Load<SpriteFont>("Fonts/Font");

            currencyDisplay = new element();
            currencyDisplay.x = (int)(3 * width) / 4;
            currencyDisplay.y = (int)(height / 16);
            currencyDisplay.value = result + " CC";
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, currencyDisplay.value, new Vector2(currencyDisplay.x, currencyDisplay.y), Color.White);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            currentInventoryState = currentEscState = keyboard;

            bool invActivated = (currentInventoryState.IsKeyUp(Keys.I) && previousInventoryState.IsKeyDown(Keys.I));
            bool escActivated = (currentEscState.IsKeyUp(Keys.Escape) && previousEscState.IsKeyDown(Keys.Escape));

            if (escActivated)
                game.ChangeStates(new EscState(game, graphicsDevice, contentManager));
            previousEscState = currentEscState;

            if (invActivated)
                game.ChangeStates(new InventoryState(game, graphicsDevice, contentManager));
            previousInventoryState = currentInventoryState;
            

            query = "SELECT Currency FROM UserInfo WHERE ID = 1";
            int result = int.Parse(dBManager.SelectElement(query));
            currencyDisplay.value = result + " CC";
        }
    }
}
