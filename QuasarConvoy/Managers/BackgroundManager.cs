using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Ambient;
using QuasarConvoy.Core;
using QuasarConvoy.States;

namespace QuasarConvoy.Managers
{
    class BackgroundManager
    {
        public List<Particle> particles;
        //private float timer;
        //private Particle prefab;
        private Texture2D texture;
        Random random=new Random();
        int screenWidth = Game1.ScreenWidth;
        int screenHeight = Game1.ScreenHeight;
        public BackgroundManager(Texture2D tex,Vector2 startpos)
        {
            texture = tex;
            particles = new List<Particle>();
            for (int i = 0; i < 200; i++)
            {
                particles.Add(GenerateParticle(random.Next(0, screenWidth), random.Next(0, screenHeight)));
            }
        }
        private Particle GenerateParticle(float x,float y)
        {
            return new Particle(texture, random.Next(1, 10) / 100f)
            {
                Position = new Vector2(x, y)
            };
        }

        private void GenerateBG(Camera cam)
        {
            if (particles.Count <= 200 + (cam.Zoom < 1 ? 10 * (1 / cam.Zoom - 1) : -50 * cam.Zoom))
            {
                if (cam.velocity.X > 0)
                    particles.Add(GenerateParticle(screenWidth + 5, random.Next(-5, screenHeight + 5)));
                else
                    if (cam.velocity.X < 0)
                        particles.Add(GenerateParticle(-5, random.Next(-5, screenHeight + 5)));

                if (cam.velocity.Y > 0)
                    particles.Add(GenerateParticle(random.Next(-5, screenWidth + 5), screenHeight + 5));
                else
                    if(cam.velocity.Y<0)
                        particles.Add(GenerateParticle(random.Next(-5, screenWidth + 5), -5));
                
            }
            if (cam.zoomVelocity < 0f)
            {
                while (particles.Count < 200 + (cam.Zoom < 1 ? 10 * (1 / cam.Zoom - 1) : -50 * cam.Zoom))
                {
                    particles.Add(GenerateParticle(random.Next(-5, screenWidth + 5), -5));
                    particles.Add(GenerateParticle(random.Next(-5, screenWidth + 5), screenHeight + 5));
                    particles.Add(GenerateParticle(-5, random.Next(-5, screenHeight + 5)));
                    particles.Add(GenerateParticle(screenWidth + 5, random.Next(-5, screenHeight + 5)));
                }
            }

        }

        private void CleanUp()
        {
            for (int i = 0; i < particles.Count; i++)
                if (particles[i].isRemoved)
                {
                    particles.RemoveAt(i);
                    i--;
                }
        }

        public void Update(Camera cam)
        {
            foreach (var part in particles)
                part.Update(cam);
            CleanUp();
            GenerateBG(cam);
        }

        public void Draw(GameTime gametime,SpriteBatch spriteBatch)
        {
            foreach (var part in particles)
                part.Draw(gametime,spriteBatch);
        }

    }
}
