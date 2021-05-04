using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class GameOverState : State
    {
        private string message = "GAME OVER. Press Space to return to main screen.";
        private string score, query;

        private SpriteFont font;
        private float width, height;

        private DBManager dBManager;

        public GameOverState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            font = _contentManager.Load<SpriteFont>("Fonts/Font");

            dBManager = new DBManager();
            query = "SELECT CURRENCY FROM [SAVES] WHERE ID = 1";
            score = dBManager.SelectElement(query) + " CC";
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, message, new Vector2(width / 3, height / 2 - 30), Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(width / 3, height / 2), Color.White);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                game.ChangeStates(new MenuState(game, graphicsDevice, contentManager));
        }
    }
}
