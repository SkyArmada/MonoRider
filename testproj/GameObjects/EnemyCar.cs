using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider
{
    class EnemyCar : Sprite
    {
        public EnemyCar()
        {
            Setup();
        }

        public override void Setup()
        {
            _HP = 1;
            _Tag = "enemycar";
            _FlipY = true;
            _zOrder = 2f;
            speed += 100;
            midpoint = 85;
        }

        public override void Update(GameTime gameTime, List<Sprite> objs)
        {
            _Position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random num = new Random();
            if (_Position.Y > 500)
            {
                _Position.Y = -10 * num.Next(250);
                if (num.Next(0, 2) == 0)
                {
                    _Position.X = midpoint + num.Next(85 - _Texture.Width/2);
                }
                else
                {
                    _Position.X = midpoint - num.Next(85 + _Texture.Width/2);
                }
            }
        }
    }
}
