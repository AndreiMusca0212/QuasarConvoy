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
            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            int score = int.Parse(dBManager.SelectElement(query));

            query = "SELECT UserID FROM [Saves] WHERE ID = 1";
            int id = int.Parse(dBManager.SelectElement(query));

            query = "SELECT HighScore FROM [User] WHERE ID = " + id;
            int high = int.Parse(dBManager.SelectElement(query));
            if (high < score)
            {
                query = "UPDATE [User] SET HighScore = " + score + "WHERE ID = " + id + ";";
                dBManager.QueryIUD(query);
            }

            query = "UPDATE [Saves] SET Currency = 0, X = 0, Y = 0;";
            dBManager.QueryIUD(query);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, message, new Vector2(width / 3, height / 2 - 30), Color.White);
            spriteBatch.DrawString(font, "Score: " + score + " CC", new Vector2(width / 3, height / 2), Color.White);

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


//i added this useless comment here because no one will ever notice its existence
/*      say hi to Bill the ghost
 *      _____________   
 *      (  u  w u   )
 *      /           \
 *     /             \ 
 *    /               \
 *    ------------------
 */
