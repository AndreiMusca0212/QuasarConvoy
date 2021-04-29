using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Ambient;
using QuasarConvoy.Core;
using QuasarConvoy.States;
using QuasarConvoy.Entities;
using QuasarConvoy.States;
using QuasarConvoy.Entities.Planets;
using QuasarConvoy.Sprites;

namespace QuasarConvoy.Managers
{
    class PlanetManager
    {
        List<Planet> _planets;
        //Dictionary<Planet,PlanetSprite> _sprites=new Dictionary<Planet, PlanetSprite>();
        Vector2 offset = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2);
        public PlanetManager(List<Planet> planets)
        {
            _planets = planets;
        }

        const int renderDistance=10000;
        public Vector2 Distance(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            foreach (var plan in _planets)
            {
                /*
                Vector2 dist = Distance(plan.Position, playerPos);
                if (dist.Length() < renderDistance * plan.Size && !plan.IsVisible)
                {
                    plan.IsVisible = true;
                    PlanetSprite ps = new PlanetSprite(plan._sprite);
                    if (!_sprites.ContainsKey(plan))
                        _sprites.Add(plan, ps);
                }
                else
                {
                    if (plan.IsVisible)
                    {
                        plan.IsVisible = false;
                        _sprites.Remove(plan);
                    }
                }
                if(plan.IsVisible)
                {
                    plan._sprite.UpdateOnSameBatch(gameTime,plan,playerPos);
                }*/
                Vector2 dist = Distance(plan.Position, playerPos);
                if (dist.Length() < renderDistance * plan.Size && !plan.IsVisible)
                {
                    plan.IsVisible = true;
                }
                else
                {
                    if (plan.IsVisible)
                    {
                        plan.IsVisible = false;
                    }
                }
                

            }
        }
        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            foreach(var plan in _planets)
            {
                if(plan.IsVisible)
                {
                    plan._sprite.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
