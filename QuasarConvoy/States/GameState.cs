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
using QuasarConvoy.Entities.Planets;
using System.Linq;

namespace QuasarConvoy.States
{
    public class GameState : State
    {

        DBManager dBManager;
        private string query;

        private SpriteFont font;


        private string currency;
        private Vector2 currencyPos;


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

        public GraphicsDevice Graphics;

        SpriteFont _font;
        private Camera _camera;

        public Camera Camera
        {
            get { return _camera; }
        }

        Player _player;

        Background bg;

        BackgroundManager BackgroundManager;

        PlanetManager PlanetManager;

        public CombatManager _combatManager;

        public List<Sprite> _sprites;

        public List<Ship> _convoy;

        public List<Ship> _enemies;

        public List<Component> UI;

        public List<Planet> _planets;

        public List<TradeStation> _stations;

        public Vector2 spawnPos;

        #region Getters
        public Vector2 GetPlayerPos()
        {
            return _player.ControlledShip.Position;
        }

        public float GetPLayerRott()
        {
            return _player.ControlledShip.Rotation;
        }
        #endregion
        public GameState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager):base(_game,_graphicsDevice,_contentManager)
        {
            float width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            float height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            font = _contentManager.Load<SpriteFont>("Fonts/Font");

            Graphics = _graphicsDevice;
            dBManager = new DBManager();
            query = "SELECT Currency FROM [Saves] WHERE ID = 1";

            long result = Convert.ToInt64(dBManager.SelectElement(query));

            int key; string unitPrefix;
            for (key = 0; key <= 6 && ((int)(result / Math.Pow(10, 3 * key)) != 0); key++) ;
            key--;
            switch (key)
            {
                case 1:
                    unitPrefix = " k";
                    break;
                case 2:
                    unitPrefix = " m";
                    break;
                case 3:
                    unitPrefix = " b";
                    break;
                case 4:
                    unitPrefix = " g";
                    break;
                case 5:
                    unitPrefix = " t";
                    break;
                case 6:
                    unitPrefix = " t";
                    break;
                default:
                    unitPrefix = " ";
                    break;
            }
            long putere = Convert.ToInt64(Math.Pow(10, 3 * key));
            currency = (int)(result / putere) + unitPrefix + "CC";
            currencyPos = new Vector2((int)(3 * width) / 4, (int)(height / 16));


            //_graphics = new GraphicsDeviceManager(this);

            _planets = new List<Planet>
            {
                new Frost(_contentManager)
                {
                    Position=new Vector2(-41000,-54000)
                },
                new Terran(_contentManager)
                {
                    Position=new Vector2(21500,-10700)
                },
                new Gas(_contentManager)
                {
                    Position= new Vector2(5000, 55000)
                },
                new Dry(_contentManager)
                {
                    Position=new Vector2(-13000,-13000)
                },
                new Star(contentManager)
                {
                    Position=new Vector2(0,0)
                }
            };

            _stations = new List<TradeStation>();

            LoadContent(_graphicsDevice,_contentManager);
        }

        protected void LoadContent(GraphicsDevice graphicsDevice, ContentManager Content)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);

            _camera = new Camera();

            bg = new Background(Content.Load<Texture2D>("spaceBG"));

            spawnPos = new Vector2(21500, -10700);

            BackgroundManager = new BackgroundManager(Content.Load<Texture2D>("starparticle"),spawnPos-new Vector2(Game1.ScreenWidth/2,Game1.ScreenHeight/2));

            PlanetManager = new PlanetManager(_planets);

            _font = Content.Load<SpriteFont>("Fonts/Font");

            _combatManager = new CombatManager(Content);

            _sprites = new List<Sprite>
            {
                
            };

            foreach(var plan in _planets)
            {
                PlanetSprite ps = new PlanetSprite(plan._sprite)
                {
                    Layer = 0.001f,
                    scale = plan.Size * 2f,
                    Position = plan.Position-new Vector2(plan._sprite._texture.Width/2* plan.Size * 2f, plan._sprite._texture.Height / 2 * plan.Size * 2f)
                };
                _sprites.Add(ps);
                if(!(plan is Star))
                    _stations.Add(new TradeStation(contentManager, plan));
            }

            _convoy = new List<Ship>
            {
                new Interceptor1(Content)
                {
                    Position=spawnPos+new Vector2(0,0),
                    CombatManager=_combatManager,
                    //Speed=5f,
                    //SpeedCap=30f,
                    
                },
                new Interceptor1(Content)
                {
                    Position=spawnPos+new Vector2(100,0),
                    CombatManager=_combatManager,
                    //Speed=5f,
                    //SpeedCap=30f,

                },
                new Interceptor1(Content)
                {
                    Position=spawnPos+new Vector2(0,100),
                    CombatManager=_combatManager,
                    //Speed=5f,
                    //SpeedCap=30f,

                },
                /*
                new Mule1(Content)
                {
                    Position=spawnPos+new Vector2(100,100)
                },
                new Mule1(Content)
                {
                    Position=spawnPos+new Vector2(200,100)
                },
                new Mule1(Content)
                {
                    Position=spawnPos+new Vector2(300,100)
                },*/
                
            };

            _enemies = new List<Ship>()
            {
                new PirateSniper(Content)
                {
                    CombatManager=_combatManager,
                    Position=spawnPos+new Vector2(400,600),
                },
                /*
                new PirateBrawler(Content)
                {
                    CombatManager=_combatManager,
                    Position=spawnPos+new Vector2(400,400),
                }*/
            };


            UI = new List<Component>();

            foreach (var ship in _convoy)
            {
                _sprites.Add(ship);
                UI.Add(new HealthBar(ship, Color.Green, new Vector2(100, 14), graphicsDevice));
                ship.blacklist = _enemies;
                ship.hasHealthbar = true;
            }

            foreach(var ship in _enemies)
            {
                _sprites.Add(ship);
                UI.Add(new HealthBar(ship, Color.Red, new Vector2(100, 14), graphicsDevice));
                ship.blacklist = _convoy;
                ship.hasHealthbar = true;
            }



            ver = 1;

            _player = new Player(_convoy);
        }

        Input Input = new Input(Keyboard.GetState());
        public void StateControl()
        {
            

            if (Input.WasPressed(Keys.Escape))
            {  
                game.ChangeStates(new EscState(game, graphicsDevice, contentManager));
            }

            if (Input.WasPressed(Keys.I,Keyboard.GetState()))
            {
                game.ChangeStates(new InventoryState(game, graphicsDevice, contentManager));
                Input.Refresh();
            }

            if (Input.WasPressed(Keys.M,Keyboard.GetState()))
            {
                game.ChangeStates(new MapState(game, graphicsDevice, contentManager));
                Input.Refresh();
            }

            Input.Refresh();
            
        }
        private void ShipUpdate(Ship sprite, GameTime gameTime, List<Ship> bl)
        {
            if (!sprite.IsControlled)
            {
                sprite.blacklist = bl;
                sprite.Update(gameTime, _sprites);
                if(sprite.Friendly&&!sprite.inCombat)
                    sprite.Follow(_player.ControlledShip);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            StateControl();
            query = "SELECT Currency FROM [Saves] WHERE ID = 1";
            int result = int.Parse(dBManager.SelectElement(query));
            currencyDisplay.value = result + " CC";
            //cleo upp
           

            BackgroundManager.Update(_camera);
            //PlanetManager.Update(gameTime,_player.ControlledShip.Position);
            foreach (var ship in _convoy)
            {
                ShipUpdate(ship, gameTime, _enemies);
            }

            foreach (var ship in _enemies)
            {
                ShipUpdate(ship, gameTime, _convoy);
            }
            foreach (var sprite in _sprites)
            {
                if (!(sprite is Ship))
                    sprite.Update(gameTime, _sprites);
            }
            //Update pentru ambele tabere
            

            //Update pentru elemente de UI
            foreach (var co in UI)
            {
                co.Update(gameTime);
            }

            _player.Update(gameTime, _sprites, _convoy);
            _camera.Update(_player.ControlledShip, _player.Input);

            _combatManager.Update(gameTime, _camera,_convoy.Concat(_enemies).ToList(),this);

            PostUpdate(gameTime);
            //base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            
            Graphics.Clear(Color.Black);

            //Batch 1 Stationary: BG + text
            _spriteBatch.Begin();

            bg.Draw(gameTime,_spriteBatch);
            BackgroundManager.Draw(gameTime,_spriteBatch);
           
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
              BlendState.AlphaBlend,
              SamplerState.PointClamp,
              null, null, null, _camera.Transform);
            //PlanetManager.Draw(gameTime, _spriteBatch);
            foreach (var st in _stations)
                st.Draw(gameTime, _spriteBatch);
            foreach (var sprite in _sprites)
                sprite.Draw(gameTime,_spriteBatch);
            foreach (var ship in _convoy)
                ship.Draw(gameTime,_spriteBatch);
            foreach (var ship in _enemies)
                ship.Draw(gameTime, _spriteBatch);
            foreach (var co in UI)
            {
                co.Draw(gameTime,_spriteBatch);
            }
            _combatManager.Draw(gameTime, _spriteBatch);



            if (ver == 0)
                throw new System.Exception("Not loaded");

            _spriteBatch.End();
            _spriteBatch.Begin();

            _spriteBatch.DrawString(font, currencyDisplay.value, new Vector2(currencyDisplay.x, currencyDisplay.y), Color.White);
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
            for(int i=0;i<_sprites.Count;i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    if (_sprites[i] is Ship)
                    {
                        _convoy.Remove((Ship)_sprites[i]);
                        _enemies.Remove((Ship)_sprites[i]);
                    }
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < UI.Count; i++)
            {
                if (UI[i].IsRemoved)
                {
                    UI.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
