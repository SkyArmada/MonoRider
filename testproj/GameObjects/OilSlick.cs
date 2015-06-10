using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoRider
{
    class OilSlick : Sprite
    {
        public OilSlick()
        {
            Setup();
        }

        public override void Setup()
        {
            _HP = startHP;
            _Tag = SpriteType.kOilType;
            _zOrder = 2f;
            midpoint = 160;
            _Position = new Vector2(-500, -500);
        }

        public override void LoadContent(string path, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(path, Content);
        }

        public override void Update(GameTime gameTime, List<Sprite> gameObjectList)
        {
            if (_CurrentState != SpriteState.kStateActive) return;

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
            base.Activate();
        }

    }
}
