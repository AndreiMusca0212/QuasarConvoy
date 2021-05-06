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

        public ShipYard shipYard;

        public Vector2 spawnPos;
        private const float secondsToBeElapsed = 10;
        private float secondsElapsed = secondsToBeElapsed;

        public int saveID;

        #region Getters
        public Vector2 GetPlayerPos()
        {
            return _player.ControlledShip.Position;
        }

        public float GetPLayerRott()
        {
            return _player.ControlledShip.Rotation;
        }

        public Ship GetControlledShip()
        {
            return _player.ControlledShip;
        }
        #endregion
        public GameState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager, int save):base(_game,_graphicsDevice,_contentManager)
        {
            float width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            float height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            saveID = save;
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

            _planets = new List<Planet>();

            for(int i=1;i<=dBManager.SelectColumnFrom("[Planets]","ID").Count;i++)
            {
                if (int.Parse(dBManager.SelectElement("SELECT SaveID FROM [Planets] WHERE ID = " + i.ToString())) == saveID)
                {
                    Planet plan = new Planet(contentManager.Load<Texture2D>(dBManager.SelectElement("SELECT Name FROM [Planets] WHERE ID=" + i.ToString())));
                    float X = float.Parse(dBManager.SelectElement("SELECT PositionX FROM [Planets] WHERE ID = " + i.ToString()));
                    float Y = float.Parse(dBManager.SelectElement("SELECT PositionY FROM [Planets] WHERE ID = " + i.ToString()));
                    plan.Position = new Vector2(X, Y);
                    plan.Size = float.Parse(dBManager.SelectElement("SELECT Size FROM [Planets] WHERE ID = " + i.ToString()));
                    plan.ID = i;
                    _planets.Add(plan);
                }
            }

            _stations = new List<TradeStation>();

            shipYard = new ShipYard(contentManager, new Vector2(32000, 0));
            
            
            LoadContent(_graphicsDevice,_contentManager);
        }

        public Ship CreateShip(int model, float x, float y, float rot ,int id)
        {
            Ship ship = null;
            switch(model)
            {
                case 1:
                    {
                        ship = new Interceptor1(contentManager)
                        {
                            CombatManager = _combatManager
                        };
                        break;
                    }
                case 2:
                    {
                        ship = new Mule1(contentManager);
                        break;
                    }
                case 3:
                    {
                        ship = new Collector(contentManager)
                        {
                            CombatManager = _combatManager
                        };
                        break;
                    }
                case 4:
                    {
                        ship = new Elephant(contentManager);
                        break;
                    }
                case 5:
                    {
                        ship = new Unicorn(contentManager);
                        break;
                    }
                case 6:
                    {
                        ship = new PirateSniper(contentManager)
                        {
                            CombatManager = _combatManager
                        };
                        break;
                    }
                case 7:
                    {
                        ship = new PirateBrawler(contentManager);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            ship.Position = new Vector2(x, y);
            ship.Rotation = rot;
            ship.ID = id;
            return ship;
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
                if(!(plan.ID==5))
                    _stations.Add(new TradeStation(contentManager, plan));
            }

            _convoy = new List<Ship>();
            for (int i = 1; i <= dBManager.SelectColumnFrom("[Ships]", "ID").Count; i++)
            {
                if (int.Parse(dBManager.SelectElement("SELECT SaveID FROM [Ships] WHERE ID = " + i.ToString())) == saveID)
                {
                    float X = float.Parse(dBManager.SelectElement("SELECT PositionX FROM [Ships] WHERE ID = " + i.ToString()));
                    float Y = float.Parse(dBManager.SelectElement("SELECT PositionY FROM [Ships] WHERE ID = " + i.ToString()));
                    float r = float.Parse(dBManager.SelectElement("SELECT Rotation FROM [Ships] WHERE ID = " + i.ToString()));
                    int model = int.Parse(dBManager.SelectElement("SELECT ID_Model FROM [Ships] WHERE ID = " + i.ToString()));
                    _convoy.Add(CreateShip(model, X, Y, r, i));
                    if(i.ToString()==dBManager.SelectElement("SELECT ShipID FROM [Saves] Where ID = "+ saveID.ToString()))
                    {
                        _player.ControlledShip = _convoy.Last<Ship>();
                    }
                }
            }

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

        public TradeStation GetClosestTrader()
        {
            float dist = _player.Distance(_stations[0].Position).Length();
            int j = 0;
            for(int i=1;i<_stations.Count;i++)
            {
                if (_player.Distance(_stations[i].Position).Length() < dist)
                {
                    dist = _player.Distance(_stations[i].Position).Length();
                    j = i;
                }
            }
            return _stations[j];
        }

        bool toolTipTradeVis = false;
        public void EnterTrade()
        {
            if(_player.Distance(GetClosestTrader().Position).Length()<200)
            {
                if(!toolTipTradeVis)
                    toolTipTradeVis = true;
                if (Input.IsPressed(_player.Input.OpenTrade, Keyboard.GetState()))
                {
                    game.ChangeStates(new TradeState(game, graphicsDevice, contentManager, GetClosestTrader().Home.ID));
                    Input.Refresh();
                }
            }
            else
                if (toolTipTradeVis)
                    toolTipTradeVis = false;
        }
        private void RandomizePlanetInventory()
        {
            Random rand = new Random(DateTime.Now.Minute);

            for (int i = 1; i <= 6; i++)
            {
                query = "SELECT ID FROM [PlanetInventory] WHERE PlanetID = " + i;
                List<string> IDS = dBManager.SpecificSelectColumnFrom("[PlanetInventory]", "ID", "PlanetID", i + "");
                foreach (string id in IDS)
                {
                    int ID = int.Parse(id);
                    query = "UPDATE [PlanetInventory] SET ItemCount = " + rand.Next(1, 15) + " WHERE ID = " + ID;
                    dBManager.QueryIUD(query);
                }
            }
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
            query = "SELECT Currency FROM [Saves] WHERE ID = " + saveID;
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

            shipYard.Update(gameTime, _player.ControlledShip.Position, this, dBManager);

            _player.Update(gameTime, _sprites, _convoy);
            _camera.Update(_player.ControlledShip, _player.Input);
            EnterTrade();
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            secondsElapsed -= timer;

            if (secondsElapsed <= 0)
            {
                RandomizePlanetInventory();
                secondsElapsed = secondsToBeElapsed;
            }
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
            shipYard.Draw(gameTime, _spriteBatch,_player.ControlledShip.Position);
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
                string.Format("posX={0} posY={1}",
                _player.ControlledShip.Position.X, _player.ControlledShip.Position.Y),
                new Vector2(30, 80),
                Color.White,
                0f,
                new Vector2(0, 0),
                1f,
                SpriteEffects.None,
                0.1f);
            if(toolTipTradeVis)
            _spriteBatch.DrawString(_font,
                "Press F to open trade",
                new Vector2(Game1.ScreenWidth / 2, 3 * Game1.ScreenHeight / 4),
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0.1f);
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
                        dBManager.QueryIUD("DELETE FROM [Ships] WHERE ID = " + ((Ship)_sprites[i]).ID);
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
