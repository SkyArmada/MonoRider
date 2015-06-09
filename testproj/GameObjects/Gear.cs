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
            _Position = new Vector2(-500, -500);
            _HP = 1;
            _Tag = SpriteType.kGearType;
            _zOrder = 1f;
        }

        public override void Update(GameTime gameTime, List<Sprite> gameObjectList)
        {
            if (_CurrentState != SpriteState.kStateActive) return;

            _Rotation += 0.05f;
            _Position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_Position.Y > 500)
            {
                this._CurrentState = SpriteState.kStateInActive;
            }
            base.Update(gameTime, gameObjectList);
        }

        public override void Activate()
        {
            Random num = new Random();
            _Position.Y = -num.Next(11) * num.Next(250);
            _Position.X = num.Next(320 - frameWidth);
            base.Activate();
        }
    }
}
