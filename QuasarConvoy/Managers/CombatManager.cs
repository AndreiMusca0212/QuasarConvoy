using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Entities;
using QuasarConvoy.Core;


namespace QuasarConvoy.Managers
{
    public class CombatManager
    {
        List<Projectile> projectiles=new List<Projectile>();
        ContentManager content;
        public CombatManager(ContentManager con)
        {
            content = con;
        }

        public void AddProjectile(Vector2 position, float velocity, float rotation, bool isFriendly)
        {
            projectiles.Add(
                new Projectile(content, velocity, rotation)
                {
                    Position = position,
                    friendly = isFriendly
                }
                );
        }

        private void CleanUp()
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].isRemoved)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Update(GameTime gameTime, Camera cam)
        {
            foreach (var proj in projectiles)
            {
                proj.Update(gameTime, cam);
            }
            CleanUp();
        }

        

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var proj in projectiles)
                proj.Draw(spriteBatch);
        }
    }
}
