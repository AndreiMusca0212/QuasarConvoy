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
        GraphicsDevice Graphics;
        Background bg;
        List<Sprite> sprites;
        int mapWidth = 150000;
        int mapHeight = 150000;
        Vector2 offset= new Vector2((Game1.ScreenWidth - Game1.ScreenHeight) / 2 + Game1.ScreenHeight/2, Game1.ScreenHeight/2);
        public MapState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager):base(_game,_graphicsDevice,_contentManager)
        {
            Graphics = _graphicsDevice;
            bg = new Background(contentManager.Load<Texture2D>("Orbit"));
            bg.scale = Game1.ScreenHeight / (float)bg._texture.Height;
            bg.Position += offset - new Vector2(Game1.ScreenHeight / 2, Game1.ScreenHeight/2);
            sprites = new List<Sprite>();
            foreach(var plan in game.GameState._planets)
            {
                Sprite ps = new Sprite(plan._sprite);
                ps.Position = plan.Position * Game1.ScreenHeight / mapHeight;
                ps.Position += offset;
                ps.scale = 0.017f*plan.Size;
                sprites.Add(ps);
            }
            foreach(var td in _game.GameState._stations)
            {
                Sprite ps = new Sprite(td._texture);
                ps.Position = td.Position * Game1.ScreenHeight / mapHeight;
                ps.Position += offset;
                ps.scale = 0.02f;
                sprites.Add(ps);
            }
            sprites.Add(new Sprite(_game.GameState.shipYard._texture)
            {
                Position = _game.GameState.shipYard.Position * Game1.ScreenHeight / mapHeight + offset,
                scale = 0.02f
            });
            Sprite point = new Sprite(_contentManager.Load<Texture2D>("Pointer"))
            {
                Position = game.GameState.GetPlayerPos() * Game1.ScreenHeight / mapHeight + offset,
                scale = 0.02f,
                Rotation = game.GameState.GetPLayerRott(),
            };
            sprites.Add(point);
            foreach (var ship in _game.GameState._convoy)
            {
                sprites.Add(new Sprite(_contentManager.Load<Texture2D>("Pointer"))
                {
                    Position = ship.Position,
                    scale = 0.01f,
                    Rotation = ship.Rotation
                });
            }
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Graphics.Clear(Color.Black);
            spriteBatch.Begin();
            bg.Draw(gameTime, spriteBatch);
            foreach (var sprit in sprites)
                sprit.Draw(gameTime,spriteBatch,new Vector2(sprit._texture.Width/2,sprit._texture.Height/2));
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
