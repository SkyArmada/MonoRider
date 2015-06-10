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
            _HP = startHP;
            _Tag = SpriteType.kCarType;
            _FlipY = true;
            _zOrder = 2f;
            speed += 100;
            midpoint = 160;
            _Position = new Vector2(-500, -500);
        }

        public override void LoadContent(string path, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(path, Content);
            base.SetupAnimation(1, 1, 1, true);
        }

        public override void Update(GameTime gameTime, List<Sprite> gameObjectList)
        {
            if (_CurrentState != SpriteState.kStateActive) return;

            _Position.Y += (speed + 60) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random num = new Random();
            if (_Position.Y > 500)
            {
                this._CurrentState = SpriteState.kStateInActive;
            }

            foreach(Sprite obj in gameObjectList)
            {
                if (obj._Tag != SpriteType.kCarType) continue;
                if (obj._CurrentState != SpriteState.kStateActive) continue;
                if (obj == this) continue;

                if(_BoundingBox.Intersects(obj._BoundingBox))
                {
                    obj.ReceiveDamage(1);
                }
            }
            base.Update(gameTime, gameObjectList);
        }

        public override void Activate()
        {
            Random num = new Random();
            _HP = startHP;
            _Position.Y = -num.Next(11) * num.Next(250);
            if (num.Next(0, 2) == 0)
            {
                _Position.X = midpoint - (num.Next(80));
            }
            else
            {
                _Position.X = midpoint + (num.Next(80));
            }
            ChangeColor(new Color(213, 255, 28, 255), new Color(num.Next(255), num.Next(255), num.Next(255), 255));
            base.Activate();
        }
    }
}
