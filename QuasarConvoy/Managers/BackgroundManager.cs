using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Ambient;
using QuasarConvoy.Core;

namespace QuasarConvoy.Managers
{
    class BackgroundManager
    {
        List<Particle> particles;
        private float timer;
        private Particle prefab;
        private Texture2D texture;
        Random random=new Random();
        public BackgroundManager(Texture2D tex)
        {
            texture = tex;
            particles = new List<Particle>();
            for (int i = 0; i < 200; i++)
            {
                particles.Add(
                    new Particle(texture, random.Next(1, 10) / 100f)
                    {
                        Position=new Vector2(random.Next(0,Game1.ScreenWidth), random.Next(0, Game1.ScreenHeight))
                    });
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
            if (particles.Count <= 200)
            {
                if (cam.velocity.X > 0)
                    particles.Add(GenerateParticle(Game1.ScreenWidth + 5, random.Next(-5, Game1.ScreenHeight + 5)));
                else
                    if (cam.velocity.X < 0)
                        particles.Add(GenerateParticle(-5, random.Next(-5, Game1.ScreenHeight + 5)));

                if (cam.velocity.Y > 0)
                    particles.Add(GenerateParticle(random.Next(-5, Game1.ScreenWidth + 5), Game1.ScreenHeight + 5));
                else
                    if(cam.velocity.Y<0)
                        particles.Add(GenerateParticle(random.Next(-5, Game1.ScreenWidth + 5), -5));
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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var part in particles)
                part.Draw(spriteBatch);
        }

    }
}
