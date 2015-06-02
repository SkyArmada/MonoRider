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
            _HP = 1;
            _Tag = "enemycar";
            _FlipY = true;
            _zOrder = 2f;
            //_MyColor = Color.Red;
        }

        public override void Update(GameTime gameTime, List<Sprite> objs)
        {
            float speed = 240.0f;
            _Position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random num = new Random();
            if (_Position.Y > 500)
            {
                _Position.Y = -10 * num.Next(250);
                _Position.X = num.Next(320 - _Texture.Width);
            }
        }

        public override void ResetSelf()
        {
            base.ResetSelf();
            _HP = 1;
            _Tag = "enemycar";
            _FlipY = true;
            _zOrder = 2f;
            //_MyColor = Color.Red;
        }
    }
}
