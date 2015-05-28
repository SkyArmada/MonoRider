﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider
{
    class EnemyCar : GameCharacterBase
    {
        public override void Initialize(Texture2D texture, Vector2 position)
        {
            _HP = 1;
            _Tag = "enemycar";
            base.Initialize(texture, position);
        }

        public override void Update(GameTime gameTime, List<GameCharacterBase> objs)
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
    }
}
