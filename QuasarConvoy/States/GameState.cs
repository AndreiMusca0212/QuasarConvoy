using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class GameState : State
    {
        private List<Component> components;
        private Texture2D HUD;
        private Rectangle mainFrame;

        public GameState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            HUD = _contentManager.Load<Texture2D>("UI Stuff/HUD");
            mainFrame = new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);

            var minimapIconTexture = _contentManager.Load<Texture2D>("UI Stuff/Icons/Minimap Icon");
            var bigMapIconTexture = _contentManager.Load<Texture2D>("UI Stuff/Icons/BigMap Icon");
            var wayPointIconTexture = _contentManager.Load<Texture2D>("UI Stuff/Icons/Waypoint Icon");
            var inventoryIconTexture = _contentManager.Load<Texture2D>("UI Stuff/Icons/Inventory Icon");
            var convoyTexture = _contentManager.Load<Texture2D>("UI Stuff/Icons/Convoy Icon");
            var settingsTexture = _contentManager.Load<Texture2D>("UI Stuff/Icons/Setting Icon");

            var minimapButton = new Button(minimapIconTexture, _contentManager)
            {
                Position = new Vector2(10, _graphicsDevice.PresentationParameters.BackBufferHeight / 8 - 50),
            };
            minimapButton.Click += MinimapButton_Click;

            var bigMapButton = new Button(bigMapIconTexture, _contentManager)
            {
                Position = new Vector2(10, _graphicsDevice.PresentationParameters.BackBufferHeight / 8),
            };
            bigMapButton.Click += BigMapButton_Click;

            var wayPointButton = new Button(wayPointIconTexture, _contentManager)
            {
                Position = new Vector2(10, _graphicsDevice.PresentationParameters.BackBufferHeight / 8 + 50),
            };
            wayPointButton.Click += WayPointButton_Click;

            var inventoryButton = new Button(inventoryIconTexture, _contentManager)
            {
                Position = new Vector2(_graphicsDevice.PresentationParameters.BackBufferWidth - 70,
                                    _graphicsDevice.PresentationParameters.BackBufferHeight / 8 - 50),
            };
            inventoryButton.Click += InventoryButton_Click;

            var convoyButton = new Button(convoyTexture, _contentManager)
            {
                Position = new Vector2(_graphicsDevice.PresentationParameters.BackBufferWidth - 60,
                                    _graphicsDevice.PresentationParameters.BackBufferHeight / 8 + 20),
            };
            convoyButton.Click += ConvoyButton_Click;

            var settingsButton = new Button(settingsTexture, _contentManager)
            {
                Position = new Vector2(_graphicsDevice.PresentationParameters.BackBufferWidth - 70,
                                    _graphicsDevice.PresentationParameters.BackBufferHeight / 8 + 90),
            };
            settingsButton.Click += SettingsButton_Click;

            components = new List<Component>()
            {
                minimapButton,
                bigMapButton,
                wayPointButton,
                inventoryButton,
                convoyButton,
                settingsButton,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(HUD, mainFrame, Color.White);
            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
                component.Update(gameTime);
        }

        //----------------------------------------------------
        private void MinimapButton_Click(object sender, EventArgs e)
        {

        }
        private void BigMapButton_Click(object sender, EventArgs e)
        {

        }
        private void WayPointButton_Click(object sender, EventArgs e)
        {

        }
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeStates(new SettingsState(game, graphicsDevice, contentManager));
        }
        private void ConvoyButton_Click(object sender, EventArgs e)
        {
            game.ChangeStates(new ConvoyManagementState(game, graphicsDevice, contentManager));
        }
        private void InventoryButton_Click(object sender, EventArgs e)
        {
            game.ChangeStates(new InventoryState(game, graphicsDevice, contentManager));
        }
    }
}
