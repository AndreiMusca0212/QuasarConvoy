using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities
{
    
    public class TradeStation
    {
        public Texture2D _texture;
        public Vector2 Position;
        public Planet Home;
        public TradeStation(ContentManager contentManager, Planet plan)
        {
            _texture = contentManager.Load<Texture2D>("TradeTerminal");
            Position = plan.Position;
            Home = plan;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0, 0, _texture.Width, _texture.Height),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0.7f,
                    SpriteEffects.None,
                    0.04f
                    );
        
        }

    }
}
