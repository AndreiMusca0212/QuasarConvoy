using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using QuasarConvoy.Controls;
using QuasarConvoy.Sprites;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class MenuState : State
    {
        private List<Component> components;
        private Texture2D background;
        private Rectangle mainFrame;

        private Texture2D transitionTexture;
        private bool isTransitioning = false;
        private bool beginTransitionFade = false;
        private float transitionAlpha = 0.0f;

        public MenuState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            float width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            float height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            var playButtonTexture = _contentManager.Load<Texture2D>("UI Stuff/Play Button");
            var quitButtonTexture = _contentManager.Load<Texture2D>("UI Stuff/Quit Button");

            var newGameButton = new Button(playButtonTexture, _contentManager)
            {
                Position = new Vector2(width / 2 - playButtonTexture.Width / 2, height / 2 -
                            (playButtonTexture.Height * 4) / 5),         
            };
            newGameButton.Click += NewGameButton_Click;

            var quitButton = new Button(quitButtonTexture, _contentManager)
            {
                Position = new Vector2(width / 2 - quitButtonTexture.Width / 2, height / 2 + quitButtonTexture.Height / 4),
            };
            quitButton.Click += QuitButton_Click;

            components = new List<Component>()
            {
                newGameButton,
                quitButton,
            };

            background = _contentManager.Load<Texture2D>("UI Stuff/UI Tech Effect");
            mainFrame = new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);

            transitionTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            transitionTexture.SetData(new Color[] { Color.Black });
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, mainFrame, Color.White);
            if(isTransitioning)
                spriteBatch.Draw(transitionTexture, mainFrame, Color.White * transitionAlpha);
            else
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
            if(beginTransitionFade)
            {
                isTransitioning = true;
                if (transitionAlpha < 1.4f)
                    transitionAlpha += 0.02f;
                if(transitionAlpha >= 1.4f)
                    game.ChangeStates(game.GameState);
            }
            else
                foreach (var component in components)
                    component.Update(gameTime);
        }
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            beginTransitionFade = true;
            isTransitioning = true;
            game.GameState = new GameState(game, graphicsDevice, contentManager,1);
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }
    }
}
