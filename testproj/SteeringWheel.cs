using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider
{
    class SteeringWheel : GameCharacterBase
    {
        private Vector2 center;
        public override void Initialize(Texture2D texture, Vector2 position)
        {
            _HP = 1;
            _Tag = "steeringwheel";
            base.Initialize(texture, position);
            center = new Vector2(_Texture.Height / 2, _Texture.Width / 2);
        }

        public void Update(GameTime gameTime, Player player)
        {
            _Rotation = (player.momentum / 200) * 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, _Position, null, Color.White, _Rotation, center, 1f, SpriteEffects.None, 0f);
        }
    }
}
