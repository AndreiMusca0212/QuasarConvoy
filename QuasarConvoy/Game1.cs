using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Sprites;
using System.Collections.Generic;
using QuasarConvoy.Managers;
using QuasarConvoy.Models;
using QuasarConvoy.Entities;
using QuasarConvoy.Controls;

namespace QuasarConvoy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Color _backgroundColour = Color.MediumPurple;
        private List<Component> _gameComponents;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1366;
            _graphics.PreferredBackBufferHeight = 764;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var startButton = new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
            {
                Position = new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, _graphics.PreferredBackBufferHeight / 2 - 50),
                Text = "Start",
            };

            startButton.Click += startButton_Click;

            var quitButton = new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
            {
                Position = new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, _graphics.PreferredBackBufferHeight / 2),
                Text = "Quit",
            };

            quitButton.Click += QuitButton_Click;

            _gameComponents = new List<Component>()
            {
                startButton,
                quitButton,
            };
        }
        private void startButton_Click(object sender, System.EventArgs e)
        {
            var random = new System.Random();

            _backgroundColour = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }
        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var component in _gameComponents)
                component.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColour);

            _spriteBatch.Begin();

            foreach (var component in _gameComponents)
                component.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
