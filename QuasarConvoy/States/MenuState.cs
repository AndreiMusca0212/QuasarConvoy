using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using QuasarConvoy.Controls;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class MenuState : State
    {
        private List<Component> components;

        public MenuState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            var buttonTexture = _contentManager.Load<Texture2D>("Controls/Button");
            var buttonFont = _contentManager.Load<SpriteFont>("Fonts/Font");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(_graphicsDevice.PresentationParameters.BackBufferWidth / 2 - 100,
                                    _graphicsDevice.PresentationParameters.BackBufferHeight / 2 - 100),
                Text = "Start",         
            };

            newGameButton.Click += NewGameButton_Click;

            var quitButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(_graphicsDevice.PresentationParameters.BackBufferWidth / 2 - 100,
                                    _graphicsDevice.PresentationParameters.BackBufferHeight / 2 - 50),
                Text = "Quit",
            };

            quitButton.Click += QuitButton_Click;

            components = new List<Component>()
            {
                newGameButton,
                quitButton,
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //reminder, delete sprites when not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
                component.Update(gameTime);
        }
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            game.ChangeStates(new GameState(game, graphicsDevice, contentManager));
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }
    }
}
