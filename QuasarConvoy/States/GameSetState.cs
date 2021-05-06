using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Controls;
using QuasarConvoy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.States
{
    public class GameSetState : State
    {
        private Button newGameButton, loadGameButton, highScoresButton, backButton, startGameButton;
        private Texture2D newGame, loadGame, highScores, back, startGame;

        private string message = "", message2 = "";
        private bool showBox = false;
        private Texture2D enterNameBox, textInputBox;
        private Rectangle enterNameBoxFrame, textInputBoxFrame;
        private SpriteFont font, anotherFont;

        private KeyboardState oldKeyState, keyState;

        private string inputName = "";
        private Keys[] acceptedKeys = new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z, Keys.Space, Keys.Back };

        private int restartCheck = 0;

        private void Load(ContentManager _contentManager)
        {
            font = _contentManager.Load<SpriteFont>("Fonts/EvenSmallerFont");
            anotherFont = _contentManager.Load<SpriteFont>("Fonts/Font");
            newGame = _contentManager.Load<Texture2D>("UI Stuff/Buttons/New Game Button");
            loadGame = _contentManager.Load<Texture2D>("UI Stuff/Buttons/Load Game Button");
            highScores = _contentManager.Load<Texture2D>("UI Stuff/Buttons/High Scores Button");
            enterNameBox = _contentManager.Load<Texture2D>("UI Stuff/Images/enterName Box");
            textInputBox = _contentManager.Load<Texture2D>("UI Stuff/textInput Box");
            back = _contentManager.Load<Texture2D>("UI Stuff/Buttons/backButton");
            startGame = _contentManager.Load<Texture2D>("UI Stuff/Buttons/startGame Button");
        }

        private DBManager dBManager;
        private string query;
        private bool activeGame;

        private List<Component> components;

        public GameSetState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            Load(_contentManager);

            dBManager = new DBManager();
            query = "SELECT COUNT(*) FROM [Saves]";
            int check = int.Parse(dBManager.SelectElement(query));

            if (check != 0)
            {
                activeGame = true;
                game.GameState = new GameState(game, graphicsDevice, contentManager, 1);
            }
            else activeGame = false;

            newGameButton = new Button(newGame, _contentManager)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - newGame.Width / 2, Game1.ScreenHeight / 4),
            };
            newGameButton.Click += NewGameButton_Click;

            loadGameButton = new Button(loadGame, _contentManager)
            {
                Position = new Vector2(newGameButton.Position.X, newGameButton.Position.Y + loadGame.Height + 50),
            };
            loadGameButton.Click += LoadGameButton_Click;

            if (!activeGame)
            {
                loadGameButton.activeButton = false;
                message = "No saves currently available. Start a new game.";
            }

            highScoresButton = new Button(highScores, _contentManager)
            {
                Position = new Vector2(newGameButton.Position.X, loadGameButton.Position.Y + highScores.Height + 50),
            };
            highScoresButton.Click += HighScoresButton_Click;


            enterNameBoxFrame = new Rectangle(Game1.ScreenWidth / 2 - enterNameBox.Width / 2, Game1.ScreenHeight / 2 - enterNameBox.Height / 2, enterNameBox.Width, enterNameBox.Height);
            textInputBoxFrame = new Rectangle(enterNameBoxFrame.X + 10, enterNameBoxFrame.Y + 80, textInputBox.Width, textInputBox.Height);

            backButton = new Button(back, _contentManager)
            {
                Position = new Vector2(enterNameBoxFrame.X + 5, enterNameBoxFrame.Y + 5),
            };
            backButton.Click += BackButton_Click;

            startGameButton = new Button(startGame, _contentManager)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - startGame.Width / 2, enterNameBoxFrame.Y + enterNameBoxFrame.Height / 2 + startGame.Height / 2),
            };
            startGameButton.Click += StartGameButton_Click;

            components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                highScoresButton,
            };
        }

        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (!showBox)
            {
                foreach (var component in components)
                    component.Draw(gameTime, spriteBatch);

                spriteBatch.DrawString(font, message, new Vector2(loadGameButton.Position.X, loadGameButton.Position.Y + loadGame.Height + 10), Color.Red);
            }
            else
            {
                spriteBatch.Draw(enterNameBox, enterNameBoxFrame, Color.White);
                backButton.Draw(gameTime, spriteBatch);
                startGameButton.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(anotherFont, "Enter your name:", new Vector2(enterNameBoxFrame.X + 10, enterNameBoxFrame.Y + 50), Color.BlueViolet);
                spriteBatch.Draw(textInputBox, textInputBoxFrame, Color.White);
                spriteBatch.DrawString(anotherFont, inputName, new Vector2(textInputBoxFrame.X + 5, textInputBoxFrame.Y), Color.BlueViolet);
                spriteBatch.DrawString(font, message2, new Vector2(enterNameBoxFrame.X + 10, startGameButton.Position.Y + startGame.Height + 10), Color.Red);
                
            }


            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (!showBox)
            {
                if (Input.IsPressed(Keys.Escape))
                    game.ChangeStates(new MenuState(game, graphicsDevice, contentManager));

                foreach (var component in components)
                    component.Update(gameTime);
            }
            else
            {
                backButton.Update(gameTime);
                startGameButton.Update(gameTime);

                /*
                foreach (Keys key in acceptedKeys)
                {
                    if (Input.WasPressedOnce(key))
                    {
                        AddKey(key);
                        break;
                    }
                }*/

                oldKeyState = keyState;
                keyState = Keyboard.GetState();
                foreach (Keys key in keyState.GetPressedKeys())
                {
                    if (oldKeyState.IsKeyUp(key))
                       AddKey(key);
                }
            }
        }

        #region Clicc
        private void HighScoresButton_Click(object sender, EventArgs e)
        {

        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            if(game.GameState != null)
                game.ChangeStates(game.GameState);
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            showBox = true;
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            showBox = false;
        }
        private void StartGameButton_Click(object sender, EventArgs e)
        {
            if(inputName.Length < 3)
            {
                message2 = "Username shall be at least 3 characters long.";
                return;
            }

            bool eligibleForStart = false;

            if (restartCheck == 0)
            {
                message2 = "Found unfinished game activity on this device.\nDo you wish to restart? Press again if yes.";
                restartCheck++;
            }
            else eligibleForStart = true;

            if (eligibleForStart)
            {
                query = "SELECT COUNT(ID) FROM [User] WHERE UserName = '" + inputName + "'";
                int check = int.Parse(dBManager.SelectElement(query));
                if (check == 0)
                {
                    query = "INSERT INTO [User] (UserName,SoundLevel,HighScore) VALUES ('" + inputName + "', 90 , 0);";
                    dBManager.QueryIUD(query);
                }

                query = "SELECT ID FROM [User] WHERE UserName = '" + inputName + "'";
                int result = int.Parse(dBManager.SelectElement(query));
                //query = "UPDATE [Saves] SET Currency = 100, X = 0, Y = 0, ShipID = 1, Name = '', Date = getdate(), UserID = " + result.ToString() + " WHERE ID = 1;";

                query = "UPDATE [Saves] SET Currency = 5000, X = 21500, Y = -10700, ShipID = 1, Name = '', Date = getdate(), UserID = " + result.ToString() + " WHERE ID = 1;";
                dBManager.QueryIUD(query);

                if (int.Parse(dBManager.SelectElement("SELECT COUNT(*) FROM [Ships] WHERE ID = 1")) == 0)
                    query = "INSERT INTO [Ships] (PositionX, PositionY, InConvoy, ID_Model, Rotation, SaveID) VALUES (21500 , -10700 , 1 , 1 , 0 , 1) ;";
                    query = "UPDATE [Ships] SET PositionX = 21500, PositionY = -10700, InConvoy = 1, ID_Model = 1, Rotation = 0, SaveID = 1";
                dBManager.QueryIUD(query);
                query = "INSERT INTO [Ships] (PositionX, PositionY, InConvoy, ID_Model, Rotation, SaveID) VALUES (21600, -10700, 1, 2, 0, 1); ";
                dBManager.QueryIUD(query);
                query = "DELETE FROM [UserInventory];";
                dBManager.QueryIUD(query);
                game.GameState = new GameState(game, graphicsDevice, contentManager, 1);
                game.ChangeStates(game.GameState);
            }
        }

        #endregion

        private void AddKey(Keys key)
        {
            string letter = "";

            if (inputName.Length >= 20 && key != Keys.Back)
                return;

            switch (key)
            {
                case Keys.A:
                    letter += "a";
                    break;
                case Keys.B:
                    letter += "b";
                    break;
                case Keys.C:
                    letter += "c";
                    break;
                case Keys.D:
                    letter += "d";
                    break;
                case Keys.E:
                    letter += "e";
                    break;
                case Keys.F:
                    letter += "f";
                    break;
                case Keys.G:
                    letter += "g";
                    break;
                case Keys.H:
                    letter += "h";
                    break;
                case Keys.I:
                    letter += "i";
                    break;
                case Keys.J:
                    letter += "j";
                    break;
                case Keys.K:
                    letter += "k";
                    break;
                case Keys.L:
                    letter += "l";
                    break;
                case Keys.M:
                    letter += "m";
                    break;
                case Keys.N:
                    letter += "n";
                    break;
                case Keys.O:
                    letter += "o";
                    break;
                case Keys.P:
                    letter += "p";
                    break;
                case Keys.Q:
                    letter += "q";
                    break;
                case Keys.R:
                    letter += "r";
                    break;
                case Keys.S:
                    letter += "s";
                    break;
                case Keys.T:
                    letter += "t";
                    break;
                case Keys.U:
                    letter += "u";
                    break;
                case Keys.V:
                    letter += "v";
                    break;
                case Keys.W:
                    letter += "w";
                    break;
                case Keys.X:
                    letter += "x";
                    break;
                case Keys.Y:
                    letter += "y";
                    break;
                case Keys.Z:
                    letter += "z";
                    break;
                case Keys.Space:
                    if (inputName.Length > 1 && inputName[inputName.Length - 1].Equals(" "))
                        letter += " ";
                    break;
                case Keys.Back:
                    if (inputName.Length > 0)
                        inputName = inputName.Remove(inputName.Length - 1, 1);
                    break;
            }

            if (Input.IsPressed(Keys.LeftShift) || Input.IsPressed(Keys.RightShift))
                letter = letter.ToUpper();
            inputName += letter;
        }
    }
}
