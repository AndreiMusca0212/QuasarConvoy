using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuasarConvoy.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using QuasarConvoy.Models;

namespace QuasarConvoy.States
{
    public class EscState : State
    {
        private SpriteFont font;
        private List<Component> components;

        private Texture2D Message, Effect;
        private Rectangle MessageFrame, EffectFrame;
        private float scale = 0.7f;

        Slider soundSlider;

        DBManager dBManager;

        public EscState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _contentManager) : base(_game, _graphicsDevice, _contentManager)
        {
            float width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            float height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            dBManager = new DBManager();

            font = _contentManager.Load<SpriteFont>("Fonts/Font");
            Message = _contentManager.Load<Texture2D>("UI Stuff/Images/EscMessage");
            MessageFrame = new Rectangle(0, 0, (int)width / 2 + 50, (int)height - 50);

            var resumeButtonTexture = _contentManager.Load<Texture2D>("UI Stuff/Buttons/Resume Button");
            var tutorialButtonTexture = _contentManager.Load<Texture2D>("UI Stuff/Buttons/Tutorial Button");
            var saveAndQuitButtonTexture = _contentManager.Load<Texture2D>("UI Stuff/Buttons/Save and Quit Button");

            var resumeButton = new Button(resumeButtonTexture, font, _contentManager, scale)
            {
                Position = new Vector2(MessageFrame.Width + resumeButtonTexture.Width / 3, resumeButtonTexture.Height / 2),
            };
            resumeButton.Click += ResumeButton_Click;

            var tutorialButton = new Button(tutorialButtonTexture, font, _contentManager, scale)
            {
                Position = new Vector2(resumeButton.Position.X, resumeButton.Position.Y + resumeButtonTexture.Height),
            };
            tutorialButton.Click += TutorialButton_Click;
            
            var saveAndQuitButton = new Button(saveAndQuitButtonTexture, font, _contentManager, scale)
            {
                Position = new Vector2(tutorialButton.Position.X, tutorialButton.Position.Y + tutorialButtonTexture.Height),
            };
            saveAndQuitButton.Click += SaveAndQuitButton_Click;

            soundSlider = new Slider(_graphicsDevice, _contentManager, (int)saveAndQuitButton.Position.X - 
                            (int)saveAndQuitButtonTexture.Width / 4, (int)saveAndQuitButton.Position.Y + 
                            (int)saveAndQuitButtonTexture.Height);

            Effect = _contentManager.Load<Texture2D>("UI Stuff/Images/TechEffect");
            EffectFrame = new Rectangle((int)resumeButton.Position.X + (int)(resumeButtonTexture.Width * scale), 10,
                            (int)(Effect.Width * scale), (int)saveAndQuitButton.Position.Y +(int)saveAndQuitButtonTexture.Height - 10);

            components = new List<Component>()
            {
                resumeButton,
                tutorialButton,
                saveAndQuitButton,
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Message, MessageFrame, Color.White);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);
            soundSlider.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(Effect, EffectFrame, Color.White);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        Input Input = new Input(Keyboard.GetState());
        public override void Update(GameTime gameTime)
        {
            //currentEscState = Keyboard.GetState();

            //bool keyPressed = (currentEscState.IsKeyUp(Keys.Escape) && previousEscState.IsKeyDown(Keys.Escape));
            
            if (Input.WasPressed(Keys.Escape))
                game.ChangeStates(game.GameState);
            //Input.Refresh();
            //previousEscState = currentEscState;

            foreach (var component in components)
                component.Update(gameTime);
            soundSlider.Update(gameTime);
        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {
            game.ChangeStates(game.GameState);
        }

        private void SaveAndQuitButton_Click(object sender, EventArgs e)
        {
            foreach(var ship in game.GameState._convoy)
            {
                dBManager.QueryIUD("UPDATE [Ships] SET PositionX = " + ship.Position.X.ToString() + ", PositionY = " + ship.Position.Y.ToString() + ", Rotation = " + ship.Rotation.ToString() + " WHERE ID = " + ship.ID.ToString());
            }
            game.Exit();
        }

        private void TutorialButton_Click(object sender, EventArgs e)
        {
            game.ChangeStates(new TutorialState(game, graphicsDevice, contentManager));
        }
    }
}
