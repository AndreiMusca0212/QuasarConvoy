using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Models;
using QuasarConvoy.Sprites;
using QuasarConvoy.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Entities
{
    struct BuySlot
    {

        public Rectangle rectangle;
        public int price;
        public string description;
        public int id;
        public BuySlot(Rectangle r, int p, string d, int i)
        {rectangle = r;
            price = p;
            description = d;
            id = i;
        }
    }
    public class ShipYard
    {

        public Texture2D _texture;
        public Vector2 Position;
        float scale = 0.7f;
        List<BuySlot> buySlots;
        List<Texture2D> icons;
        private SpriteFont font;

        private Rectangle ScaledRect(int x, int y, int w, int h, float s)
        {
            return new Rectangle((int)( x * s +Position.X - _texture.Width/2 * scale ), (int)(y * s + Position.Y - _texture.Height / 2 * scale), (int)(w * s), (int)(h * s));
        }
        public ShipYard(ContentManager contentManager, Vector2 pos)
        {
            _texture = contentManager.Load<Texture2D>("ShipYard");
            Position = pos;
            font = contentManager.Load<SpriteFont>("Fonts/Font");
            buySlots = new List<BuySlot>
            {
                new BuySlot(
                    ScaledRect(0,50,262,262,scale),
                    2000,
                    "Interceptor \n Basic fighter ship, Integrity: low \n Firepower: high \n Cargo capacity: none \n Speed: high", 1),

                new BuySlot(
                    ScaledRect(0,370,262,262,scale),
                    1700,
                    "Mule \n Basic cargo ship, Integrity: medium \n Firepower: none \n Cargo capacity: medium \n Speed: low",2),

                new BuySlot(
                    ScaledRect(0,685,262,262,scale),
                    3000,
                    "Collector \n Mixed ship, Integrity: low \n Firepower: medium \n Cargo capacity: low \n Speed: medium",3),

                new BuySlot(
                    ScaledRect(736,50,262,262,scale),
                    5000,
                    "Elephant \n High capacity cargo ship, Integrity: high \n Capable of brawling \n Cargo capacity: high \n Speed: low",4),

                new BuySlot(
                    ScaledRect(736,370,262,262,scale),
                    5000,
                    "Unicorn \n High speed cargo ship, Integrity: low \n Firepower: none \n Cargo capacity: medium \n Speed: high",5),

                new BuySlot(
                    ScaledRect(736,685,262,262,scale),
                    0,
                    "Sell current ship",0),
            };

            icons = new List<Texture2D>
            {
                contentManager.Load<Texture2D>("interceptor"),
                contentManager.Load<Texture2D>("mule"),
                contentManager.Load<Texture2D>("collector"),
                contentManager.Load<Texture2D>("elephant"),
                contentManager.Load<Texture2D>("unicorn"),
            };


        }

        //private string activeDescription;
        Input Input = new Input();
        public void Update(GameTime gameTime, Vector2 playerPos, GameState gameState, DBManager dBManager)
        {
            foreach (var b in buySlots)
            {
                if (b.id!=0 && b.rectangle.Contains(playerPos.ToPoint()) && Input.WasPressed(Keys.F,Keyboard.GetState()))
                {
                    Vector2 pos = gameState._combatManager.GenerateOutViewPosition(gameState.Camera);
                    dBManager.QueryIUD("INSERT INTO [Ships] (PositionX, PositionY, InConvoy, ID_Model, Rotation, SaveID) VALUES ( " + pos.X.ToString() + " , " + pos.Y.ToString() + " , 1 , " + b.id.ToString() + " , 0 , " + gameState.saveID.ToString() + " )");
                    int missing = -1;
                    for (int i = 1; i <= dBManager.SelectColumnFrom("[Ships]", "ID").Count; i++)
                    {
                        if (int.Parse(dBManager.SelectElement("SELECT SaveID FROM [Ships] WHERE ID = " + i.ToString())) == gameState.saveID)
                        {
                            int s = 0;
                            for (; s < gameState._convoy.Count; s++)
                            {
                                if (gameState._convoy[s].ID == i)
                                    break;
                            }
                            if (s >= gameState._convoy.Count)
                                missing = i;

                        }
                    }
                    if (missing != -1)
                    {
                        float X = float.Parse(dBManager.SelectElement("SELECT PositionX FROM [Ships] WHERE ID = " + missing.ToString()));
                        float Y = float.Parse(dBManager.SelectElement("SELECT PositionY FROM [Ships] WHERE ID = " + missing.ToString()));
                        float r = float.Parse(dBManager.SelectElement("SELECT Rotation FROM [Ships] WHERE ID = " + missing.ToString()));
                        int model = int.Parse(dBManager.SelectElement("SELECT ID_Model FROM [Ships] WHERE ID = " + missing.ToString()));
                        Ship ship = gameState.CreateShip(model, X, Y, r, missing);
                        gameState._convoy.Add(ship);
                        gameState._sprites.Add(ship);
                        gameState.UI.Add(new QuasarConvoy.Controls.HealthBar(ship, Color.Green, new Vector2(100, 14), gameState.graphicsDevice));
                        ship.blacklist = gameState._enemies;
                        ship.hasHealthbar = true;
                    }
                    long currency = int.Parse(dBManager.SelectElement("SELECT Currency FROM [Saves] WHERE ID = " + gameState.saveID.ToString()));
                    currency -= b.price;
                    dBManager.QueryIUD("UPDATE [Saves] SET Currency = " + currency.ToString() + " WHERE ID = " + gameState.saveID.ToString());
                }
                else
                    if(b.id==0 && b.rectangle.Contains(playerPos.ToPoint()) && Input.WasPressed(Keys.F, Keyboard.GetState()))
                {
                    gameState.GetControlledShip().Integrity = 0;
                    long currency = int.Parse(dBManager.SelectElement("SELECT Currency FROM [Saves] WHERE ID = " + gameState.saveID.ToString()));
                    int id = int.Parse(dBManager.SelectElement("SELECT ID_Model FROM [Ships] WHERE ID = " + gameState.GetControlledShip().ID.ToString()));
                    currency += buySlots[id - 1].price * 70 / 100;
                    dBManager.QueryIUD("UPDATE [Saves] SET Currency = " + currency.ToString() + " WHERE ID = " + gameState.saveID.ToString());
                }
            }
            Input.Refresh();

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 playerPos)
        {

            if (_texture != null)
                spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0, 0, _texture.Width, _texture.Height),
                    Color.White,
                    0f,
                    new Vector2(_texture.Width / 2, _texture.Height / 2),
                    scale,
                    SpriteEffects.None,
                    0.04f
                    );
            for(int i=0;i<icons.Count;i++)
            {
                spriteBatch.Draw(icons[i], buySlots[i].rectangle, Color.White);
            }
            foreach (var b in buySlots)
            {
                if (b.rectangle.Contains(playerPos.ToPoint()) && b.id!=0)
                {
                    spriteBatch.DrawString(font,
                        "Press F to buy \n" + b.description,
                        playerPos,
                        Color.White,
                        0f,
                        new Vector2(-100,0),
                        1f,
                        SpriteEffects.None,
                        0.4f);
                }
                else
                    if(b.id==0 && b.rectangle.Contains(playerPos.ToPoint()))
                    {
                    spriteBatch.DrawString(font,
                    "Press F to " + b.description,
                    playerPos,
                    Color.White,
                    0f,
                    new Vector2(-100, 0),
                    1f,
                    SpriteEffects.None,
                    0.4f);
                }
            }
            

        }

    }
}
