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
        public override void Initialize(Texture2D texture, Vector2 position)
        {
            _HP = 1;
            _Tag = "steeringwheel";
            base.Initialize(texture, position);
        }

        public void Update(GameTime gameTime, Player player)
        {
            _Rotation = (player.momentum / 200) * 10;
        }
    }
}
