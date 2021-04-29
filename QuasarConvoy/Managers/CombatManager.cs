using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using QuasarConvoy.Entities;
using QuasarConvoy.Core;
using QuasarConvoy.Sprites;
using QuasarConvoy.States;
using QuasarConvoy.Entities.Ships;
using QuasarConvoy.Controls;

namespace QuasarConvoy.Managers
{
    public class CombatManager
    {
        List<Projectile> projectiles = new List<Projectile>();
        ContentManager content;
        Random random = new Random();
        public float DangerRangeUpperBound = 130000;
        public float DangerRangeLowerBound = 40000;
        public double EncounterInterval=50;
        double encounterTimer = 0f;
        public CombatManager(ContentManager con)
        {
            content = con;
        }

        public void AddProjectile(Vector2 position, float size, int damag, float velocity, float rotation, Ship source)
        {
            projectiles.Add(
                new Projectile(content, velocity, rotation)
                {
                    Position = position,
                    source = source,
                    friendly = source.Friendly,
                    scale = size,
                    damage = damag
                }
                );
        }
        public Vector2 Distance(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
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

        /*empty case for easy c-p
         case 0: 
            {
                break;
            }
        */
        private void SpawnConfiguration(List<Ship> enemylist, Vector2 pos, int option)
        {
            switch (option)
            {
                case 1:
                    {
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos
                            });
                        break;
                    }
                case 2:
                    {
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos
                            });
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos + new Vector2(0, 50)
                            });
                        break;
                    }
                case 3:
                    {
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos
                            });
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos + new Vector2(50, 50)
                            });
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos + new Vector2(-50, 50)
                            });
                        break;
                    }
                case 4:
                    {
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos
                            });
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos + new Vector2(-50, 50)
                            });
                        enemylist.Add(
                            new PirateSniper(content)
                            {
                                Position = pos + new Vector2(50, 50),
                                CombatManager = this
                            });
                        break;
                    }
                case 5:
                    {
                        enemylist.Add(
                            new PirateBrawler(content)
                            {
                                Position = pos
                            });
                        enemylist.Add(
                            new PirateSniper(content)
                            {
                                Position = pos + new Vector2(50, 50),
                                CombatManager = this
                            });
                        enemylist.Add(
                            new PirateSniper(content)
                            {
                                Position = pos + new Vector2(-50, 50),
                                CombatManager = this
                            });
                        break;
                    }
                case 6:
                    {
                        enemylist.Add(
                           new PirateBrawler(content)
                           {
                               Position = pos
                           });
                        enemylist.Add(
                           new PirateBrawler(content)
                           {
                               Position = pos + new Vector2(-50, 50)
                           });
                        enemylist.Add(
                            new PirateSniper(content)
                            {
                                Position = pos + new Vector2(50, 50),
                                CombatManager = this
                            });
                        enemylist.Add(
                            new PirateSniper(content)
                            {
                                Position = pos + new Vector2(100, 50),
                                CombatManager = this
                            });
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            
            
        }
        private Vector2 GenerateOutViewPosition(Camera cam)
        {
            int opt=random.Next(1,5);
            Vector2 viewPort = cam.ViewPortSize();
            switch(opt)
            {
                case 1:
                    {//up
                        return new Vector2(cam.cameraPos.X + random.Next(-1 * (int)viewPort.X / 2, (int)viewPort.X / 2), cam.cameraPos.Y - (int)viewPort.Y / 2 - random.Next(100, 300));
                    }
                case 2:
                    {//down
                        return new Vector2(cam.cameraPos.X + random.Next(-1 * (int)viewPort.X / 2, (int)viewPort.X / 2), cam.cameraPos.Y + (int)viewPort.Y / 2 + random.Next(100, 300));
                    }
                case 3:
                    {//right
                        return new Vector2(cam.cameraPos.X + viewPort.X / 2 + random.Next(100, 300), cam.cameraPos.Y+random.Next(-1* (int)viewPort.Y/2, (int)viewPort.Y / 2));
                    }
                case 4:
                    {//left
                        return new Vector2(cam.cameraPos.X - viewPort.X / 2 - random.Next(100, 300), cam.cameraPos.Y + random.Next(-1 * (int)viewPort.Y / 2, (int)viewPort.Y / 2));
                    }
                default:
                    {
                        return new Vector2(cam.cameraPos.X + viewPort.X / 2 + random.Next(100, 300), cam.cameraPos.Y + random.Next(-1 * (int)viewPort.Y / 2, (int)viewPort.Y / 2));
                    }
            }
        }
        private void SpawnEnemies(GameState game)
        {
            if (encounterTimer >= EncounterInterval)
            {
                encounterTimer = 0;
                if (game.GetPlayerPos().Length() > DangerRangeLowerBound && game.GetPlayerPos().Length() < DangerRangeUpperBound)
                {
                    SpawnConfiguration(game._enemies, GenerateOutViewPosition(game.Camera), random.Next(1, 9));
                    foreach (var ship in game._enemies)
                    {
                        if (!ship.hasHealthbar)
                        {
                            game._sprites.Add(ship);
                            game.UI.Add(new HealthBar(ship, Color.Red, new Vector2(100, 14), game.Graphics));
                            ship.blacklist = game._convoy;
                            ship.hasHealthbar = true;
                        }
                    }
                }
            }
        }
        public void Update(GameTime gameTime, Camera cam, List<Ship> ships, GameState game)
        {
            foreach (var proj in projectiles)
            {
                proj.Update(gameTime, cam);
            }
            encounterTimer += gameTime.ElapsedGameTime.TotalSeconds;
            SpawnEnemies(game);
            DamageDetect(ships);
            CleanUp();

        }

        public bool IsHit(Ship ship,Projectile proj)
        {
            if (ship != proj.source)
            {
                //Rectangle rect1 = new Rectangle(ship.Position.ToPoint(), new Point((int)(Math.Abs(ship._texture.Width * Math.Cos(ship.Rotation))), (int)(Math.Abs(ship._texture.Height * Math.Sin(ship.Rotation)))));
                //Rectangle rect2 = new Rectangle(proj.Position.ToPoint(), new Point((int)(Math.Abs(proj._texture.Width * Math.Cos(proj.Rotation))), (int)(Math.Abs(proj._texture.Height * Math.Sin(proj.Rotation)))));
                if (ship.Distance(proj.Position).Length() < ship._texture.Width*ship.scale/2+proj._texture.Width*proj.scale/2 && proj.friendly!=ship.Friendly)
                    return true;
                return false;
            }
            return false;
        }

        private void DamageDetect(List<Ship> ships)
        {
            foreach(var ship in ships)
            {
                foreach(var proj in projectiles)
                {
                    if(IsHit(ship,proj))
                    {
                        ship.Integrity -= proj.damage;
                        proj.isRemoved = true;
                    }
                }
            }
        }
        public void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            foreach (var proj in projectiles)
                proj.Draw(gameTime,spriteBatch);
        }
    }
}
