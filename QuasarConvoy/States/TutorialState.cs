using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class TutorialState : State
    {
        Texture2D _texture;
        SpriteFont font;
        string controls;
        string menus;
        public TutorialState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            font = contentManager.Load<SpriteFont>("Fonts/Font");
            controls = " W - Move Forward \n S - Move Backward \n D - RotateRight \n W - Rotate Left \n T - Change Perspective \n F - Open trade/Interact \n Space - Shoot";
            menus = "M - Map \n I - Inventory";
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, controls, new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, menus, new Vector2(Game1.ScreenWidth/2, 100), Color.White);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(Keys.Escape))
                game.ChangeStates(game.GameState);
        }
    }
}
