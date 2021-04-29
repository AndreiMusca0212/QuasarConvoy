using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using QuasarConvoy.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuasarConvoy.Managers;
using QuasarConvoy.Models;
using QuasarConvoy.Entities;

namespace QuasarConvoy.Sprites
{
    public class PlanetSprite:Sprite
    {
        public int renderDistance;
        Vector2 offset = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2);
        public PlanetSprite(ContentManager contentManager):base(contentManager)
        {
            Layer = 0.001f;
        }

        public PlanetSprite(Texture2D texture):base(texture)
        {
            _texture = texture;
        }

        public PlanetSprite(PlanetSprite ps):base(ps)
        {
            
        }

        public Vector2 Distance(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public void Update(GameTime gameTime,Planet planet, Vector2 playerPos)
        {
            Vector2 dist = Distance(planet.Position,playerPos);
            Position = offset + dist * (float)Game1.ScreenWidth / renderDistance;
            float sca = scale;
            scale = (planet.Size / (dist.Length() * 0.0008f + 1)) * (sca + 3) / sca;
            if (scale > planet.Size)
                scale = planet.Size;
        }

        public void UpdateOnSameBatch(GameTime gameTime, Planet planet, Vector2 playerPos)
        {
            Vector2 dist = Distance(planet.Position, playerPos);
            Position = planet.Position;
            /*float sca = scale;
            scale = (planet.Size / (dist.Length() * 0.0008f + 1)) * (sca + 3) / sca;
            if (scale > planet.Size)
                scale = planet.Size;*/
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
                /*spriteBatch.Draw(_texture,
                    Position,
                    new Rectangle(0, 0, _texture.Width, _texture.Height),
                    Color.White,
                    Rotation,
                    new Vector2(_texture.Width/2,_texture.Height/2),
                    scale,
                    SpriteEffects.None,
                    Layer
                    );*/
                //spriteBatch.Draw(_texture, new Rectangle((int)(Position.X - _texture.Width / 2 * scale), (int)(Position.Y - _texture.Height / 2 * scale), (int)(_texture.Width * scale), (int)(_texture.Height * scale)), Color.White);
                base.Draw(gameTime, spriteBatch);
        }
    }
}
