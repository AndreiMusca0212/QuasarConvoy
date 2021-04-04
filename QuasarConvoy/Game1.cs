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

        SpriteFont _font;
        private Camera _camera;

        Player _player;

        Background bg;

        BackgroundManager BackgroundManager;

        List<Sprite> _sprites;

        //List<Ship> _convoy;

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
                new Mule1(Content),
                new Mule1(Content)
                {
                    Position=new Vector2(100,100)
                },
                new Mule1(Content)
                {
                    Position=new Vector2(200,100)
                },
                new Mule1(Content)
                {
                    Position=new Vector2(300,100)
                }
            };

            ver = 1;

            _player = new Player((Ship)_sprites[0]);

            bg = new Background(Content.Load<Texture2D>("spaceBG"));

            BackgroundManager = new BackgroundManager(Content.Load<Texture2D>("starparticle"));

            _font = Content.Load<SpriteFont>("Font");


        }

        private void ShipUpdate(Ship sprite,GameTime gameTime)
        {
            if (!sprite.IsControlled)
            {
                sprite.Update(gameTime, _sprites);
                sprite.Follow(_player.ControlledShip);
            }
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            BackgroundManager.Update(_camera);
            
            foreach (var sprite in _sprites)
            {
                if (sprite is Ship)
                {
                    ShipUpdate((Ship)sprite,gameTime);
                }
                else
                {
                    sprite.Update(gameTime, _sprites);
                }
            }
            _player.Update(gameTime, _sprites);
            _camera.Follow(_player.ControlledShip);

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
                bg.Draw(_spriteBatch);
                BackgroundManager.Draw(_spriteBatch);
            _spriteBatch.End();
            
            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
              BlendState.AlphaBlend,
              SamplerState.PointClamp,
              null, null, null, _camera.Transform);

            foreach (var sprite in _sprites)
                sprite.Draw(_spriteBatch);
            
            _spriteBatch.DrawString(_font,
                string.Format("posX={0} posY={1}",
                _sprites[0].Position.X, _sprites[0].Position.Y),
                new Vector2(_sprites[0].Position.X, _sprites[0].Position.Y + 30),
                Color.White,
                0f,
                new Vector2(0,0),
                1f,
                SpriteEffects.None,
                0.6f);
            

            if (ver == 0)
               throw new System.Exception("Not loaded");

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
