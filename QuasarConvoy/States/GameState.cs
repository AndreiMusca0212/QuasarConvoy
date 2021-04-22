using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using QuasarConvoy.Sprites;
using System.Collections.Generic;
using QuasarConvoy.Managers;
using QuasarConvoy.Models;
using QuasarConvoy.Entities;
using QuasarConvoy.Core;
using QuasarConvoy.Ambient;
using QuasarConvoy.Entities.Ships;
using QuasarConvoy.Controls;
using System;
using System.Text;

namespace QuasarConvoy.States
{
    public class GameState : State
    {

        DBManager dBManager;
        private string query;

        private SpriteFont font;

        struct element
        {
            public int x;
            public int y;
            public string value;
        }
        element currencyDisplay;
        
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int ver = 0;

        GraphicsDevice Graphics;

        SpriteFont _font;
        private Camera _camera;

        public Camera Camera
        {
            get { return _camera; }
        }

        Player _player;

        Background bg;

        BackgroundManager BackgroundManager;

        public CombatManager _combatManager;

        List<Sprite> _sprites;

        List<Ship> _convoy;

        //List<Ship> _convoy;

        public GameState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager):base(_game,_graphicsDevice,_contentManager)
        {
            float width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            float height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            Graphics = _graphicsDevice;
            dBManager = new DBManager();
            query = "SELECT Currency FROM UserInfo WHERE ID = 1";
            int result = int.Parse(dBManager.SelectElement(query));
            
            font = _contentManager.Load<SpriteFont>("Fonts/Font");

            currencyDisplay = new element();
            currencyDisplay.x = (int)(3 * width) / 4;
            currencyDisplay.y = (int)(height / 16);
            currencyDisplay.value = result + " CC";
            //_graphics = new GraphicsDeviceManager(this);

            LoadContent(_graphicsDevice,_contentManager);
        }

        

        protected void LoadContent(GraphicsDevice graphicsDevice, ContentManager Content)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);

            _camera = new Camera();


            bg = new Background(Content.Load<Texture2D>("spaceBG"));

            BackgroundManager = new BackgroundManager(Content.Load<Texture2D>("starparticle"));

            _font = Content.Load<SpriteFont>("Fonts/Font");

            _combatManager = new CombatManager(Content);

            _sprites = new List<Sprite>
            {
                new Projectile(Content,0f,0f),
            };

            _convoy = new List<Ship>
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
                },
                new Interceptor1(Content)
                {
                    Position=new Vector2(200,200),
                    CombatManager=_combatManager
                }
            };

            foreach (var ship in _convoy)
            {
                _sprites.Add(ship);
            }



            ver = 1;

            _player = new Player(_convoy);




        }

        private void ShipUpdate(Ship sprite, GameTime gameTime)
        {
            if (!sprite.IsControlled)
            {
                sprite.Update(gameTime, _sprites);
                sprite.Follow(_player.ControlledShip);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            currentInventoryState = currentEscState = keyboard;

            bool invActivated = (currentInventoryState.IsKeyUp(Keys.I) && previousInventoryState.IsKeyDown(Keys.I));
            bool escActivated = (currentEscState.IsKeyUp(Keys.Escape) && previousEscState.IsKeyDown(Keys.Escape));

            if (escActivated)
                game.ChangeStates(new EscState(game, graphicsDevice, contentManager));
            previousEscState = currentEscState;

            if (invActivated)
                game.ChangeStates(new InventoryState(game, graphicsDevice, contentManager));
            previousInventoryState = currentInventoryState;
            

            query = "SELECT Currency FROM UserInfo WHERE ID = 1";
            int result = int.Parse(dBManager.SelectElement(query));
            currencyDisplay.value = result + " CC";
            //cleo upp
            
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            BackgroundManager.Update(_camera);

            foreach (var sprite in _sprites)
            {
                if (!(sprite is Ship))
                    sprite.Update(gameTime, _sprites);
            }

            foreach (var ship in _convoy)
            {
                ShipUpdate(ship, gameTime);
            }
            _player.Update(gameTime, _sprites, _convoy);
            _camera.Update(_player.ControlledShip, _player.Input);

            _combatManager.Update(gameTime, _camera);



            //base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            
            Graphics.Clear(Color.Black);

            //Batch 1 Stationary: BG + text
            _spriteBatch.Begin();
            bg.Draw(gameTime,_spriteBatch);
            BackgroundManager.Draw(gameTime,_spriteBatch);
            _spriteBatch.DrawString(_font,
                string.Format("stars:{0} \n MaxStars:{1}",
                BackgroundManager.particles.Count, 200 + (_camera.Zoom < 1 ? 10 * (1 / _camera.Zoom - 1) : -50 * _camera.Zoom)),
                new Vector2(30, 30),
                Color.Orange,
                0f,
                new Vector2(0, 0),
                1f,
                SpriteEffects.None,
                0.6f);
            _spriteBatch.DrawString(_font,
                string.Format("posX={0} posY={1}",
                _player.ControlledShip.Position.X, _player.ControlledShip.Position.Y),
                new Vector2(30, 80),
                Color.White,
                0f,
                new Vector2(0, 0),
                1f,
                SpriteEffects.None,
                0.6f);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
              BlendState.AlphaBlend,
              SamplerState.PointClamp,
              null, null, null, _camera.Transform);

            foreach (var sprite in _sprites)
                sprite.Draw(gameTime,_spriteBatch);
            foreach (var ship in _convoy)
                ship.Draw(gameTime,_spriteBatch);
            _combatManager.Draw(gameTime, _spriteBatch);



            if (ver == 0)
                throw new System.Exception("Not loaded");

            _spriteBatch.End();
            _spriteBatch.Begin();

            _spriteBatch.DrawString(font, currencyDisplay.value, new Vector2(currencyDisplay.x, currencyDisplay.y), Color.White);

            _spriteBatch.End();


            //base.Draw(gameTime);
        }
        
        

        /*public GameState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            
        }*/
        
        /*
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }*/

        public override void PostUpdate(GameTime gameTime)
        {
            
        }
    }
}
