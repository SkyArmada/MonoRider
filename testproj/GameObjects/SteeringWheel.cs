using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider
{
    class SteeringWheel : Sprite
    {
        public SteeringWheel()
        {
            _HP = 1;
            _Tag = SpriteType.kWheelType;
            _zOrder = 20f;
        }

        public void Update(GameTime gameTime, float playerMomentum)
        {
            _Rotation = (playerMomentum / 200) * 10;
        }
    }
}
