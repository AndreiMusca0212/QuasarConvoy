using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using QuasarConvoy.Sprites;
using QuasarConvoy.Models;
using QuasarConvoy.Ambient;

namespace QuasarConvoy.States
{
    class MapState:State
    {
        // Size 150 000 * 150 000
        Background bg;
        List<Sprite> sprites;
        int mapWidth = 150000;
        int mapHeight = 150000;
        public MapState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager):base(_game,_graphicsDevice,_contentManager)
        {
            bg = new Background(contentManager.Load<Texture2D>("spaceBg"));
            sprites = new List<Sprite>();
            foreach(var plan in game.GameState._planets)
            {
                Sprite ps = plan._sprite;
                ps.Position = plan.Position * Game1.ScreenHeight / mapHeight;
                ps.Position += new Vector2((Game1.ScreenWidth-Game1.ScreenHeight)/2,0);
                ps.scale = 0.02f*plan.Size;
                sprites.Add(ps);
            }
            Sprite point = new Sprite(_contentManager.Load<Texture2D>("Pointer"))
            {
                Position = game.GameState.GetPlayerPos() * Game1.ScreenHeight / mapHeight + new Vector2((Game1.ScreenWidth - Game1.ScreenHeight) / 2, 0),
                scale = 0.02f,
                Rotation = game.GameState.GetPLayerRott(),
            };
            sprites.Add(point);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            bg.Draw(gameTime, spriteBatch);
            foreach (var sprit in sprites)
                sprit.Draw(gameTime,spriteBatch);
            spriteBatch.End();
        }

        Input Input = new Input(Keyboard.GetState());
        public override void Update(GameTime gameTime)
        {

            if (Input.WasPressed(Keys.Escape))
                game.ChangeStates(game.GameState);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }
    }
}
