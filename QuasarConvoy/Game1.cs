using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Sprites;
using System.Collections.Generic;
using QuasarConvoy.Managers;
using QuasarConvoy.Models;
using QuasarConvoy.Entities;
using QuasarConvoy.Core;
using QuasarConvoy.Ambient;
using QuasarConvoy.Entities.Ships;

namespace QuasarConvoy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int ver = 0;

        private Camera _camera;

        Player _player;

        BackGround bg;

        List<Sprite> _sprites;

        List<Ship> _convoy;

        public static int ScreenWidth=1366;
        public static int ScreenHeight=764;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _camera = new Camera();

            

            _sprites = new List<Sprite>
            {    
                
            };

            _convoy = new List<Ship>
            {
                new Mule1(Content),
                new Mule1(Content)
                {
                    Position=new Vector2(100,100)
                }
            };
            ver = 1;

            _player = new Player(_convoy[0]);

            bg = new BackGround(Content.Load<Texture2D>("nebula"))
            {
                scale = 1f
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var sprite in _sprites)
                sprite.Update(gameTime,_sprites);
            foreach (var ship in _convoy)
            {
                if (!ship.IsControlled)
                {
                    ship.Update(gameTime, _sprites);
                    ship.Follow(_player.ControlledShip);
                }
            }   
            _player.Update(gameTime, _sprites);
            _camera.Follow(_player.ControlledShip);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
              BlendState.AlphaBlend,
              SamplerState.PointClamp,
              null, null, null, _camera.Transform);

            foreach (var sprite in _sprites)
                sprite.Draw(_spriteBatch);
            foreach (var ship in _convoy)
                ship.Draw(_spriteBatch);
            bg.Draw(_spriteBatch);

            if (ver == 0)
               throw new System.Exception("Not loaded");

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
