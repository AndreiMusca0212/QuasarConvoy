using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace QuasarConvoy.States
{
    public abstract class State
    {
        #region Fields

        protected ContentManager contentManager;

        protected GraphicsDevice graphicsDevice;

        protected Game1 game;

        protected KeyboardState previousInventoryState, currentInventoryState;
        protected KeyboardState previousEscState, currentEscState;

        #endregion

        #region Methods
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public State(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager)
        {
            contentManager = _contentManager;
            graphicsDevice = _graphicsDevice;
            game = _game;
        }

        #endregion
    }
}
