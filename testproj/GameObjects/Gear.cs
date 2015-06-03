using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider
{
    class Gear : Sprite
    {
        public Gear()
        {
            Setup();
        }

        public override void Setup()
        {
            _HP = 1;
            _Tag = "gear";
            _zOrder = 1f;
        }

        public override void Update(GameTime gameTime, List<Sprite> gameObjectList)
        {
            _Rotation += 0.05f;
            _Position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random num = new Random();
            if (_Position.Y > 500)
            {
                _Position.Y = -10 * num.Next(250);
                _Position.X = num.Next(320 - _Texture.Width);
            }
            base.Update(gameTime, gameObjectList);
        }
    }
}
