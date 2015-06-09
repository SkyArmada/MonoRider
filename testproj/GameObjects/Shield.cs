﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoRider
{
    class Shield : Sprite
    {
        public Shield()
        {
            Setup();
        }

        public override void Setup()
        {
            _HP = 1;
            _Tag = SpriteType.kShieldType;
            _zOrder = 1f;
            _Position = new Vector2(-500, -500);
        }

        public override void LoadContent(string path, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(path, Content);
            base.SetupAnimation(5, 30, 1, true);
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
            _Position.Y = -num.Next(11) * num.Next(250);
            _Position.X = num.Next(320 - frameWidth);
            base.Activate();
        }
    }
}
