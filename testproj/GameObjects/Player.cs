using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace MonoRider
{
    class Player : Sprite
    {
        public float momentum = 0;
        private float last_momentum = 0;
        bool spinOut = false;
        private int loops = 0;

        public Player()
        {
            Setup();
        }

        public override void Setup()
        {
            momentum = 0;
            last_momentum = 0;
            spinOut = false;
            loops = 0;
            _HP = 1;
            _Tag = SpriteType.kPlayerType;
            _LockInScreen = true;
            _zOrder = 15f;
            _ChildrenList = new List<Sprite>();
            speed = 100f;
        }

        public override void LoadContent(string path, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(path, Content);
        }

        public override void Update(GameTime gameTime, List<Sprite> gameObjectList)
        {
            if(_CurrentState == SpriteState.kStateActive)
            {
                handleMovement(gameTime);
                HandleCollistion(gameObjectList);
                if(spinOut)
                {
                    _Rotation += 0.15f;
                    if(_Rotation >= Math.PI * 2)
                    {
                        loops++;
                        _Rotation = 0;
                        if(loops >= 2)
                        {
                            spinOut = false;
                            _Rotation = 0;
                            loops = 0;
                        }
                    }
                }
            }
            base.Update(gameTime, gameObjectList);
        }

        private void handleMovement(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float friction = 7.5f * delta;
            if (!spinOut)
            {
                KeyboardState state = Keyboard.GetState();
                int momentumGain = 15;
                if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
                {
                    momentum -= momentumGain * delta;
                }
                else if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
                {
                    momentum += momentumGain * delta;
                }
                if (state.IsKeyDown(Keys.F))
                {
                    spinOut = true;
                }
                if (momentum >= 200)
                {
                    momentum = 180;
                }
                else if (momentum <= -200)
                {
                    momentum = -180;
                }
            }
            _Position.X = _Position.X + (momentum);
            LockInBounds();
            if (momentum != 0)
            {
                if (momentum >= 0f)
                {
                    momentum -= friction;
                    if(momentum <= 0.02f)
                    {
                        momentum = 0;
                    }
                }
                else if (momentum <= 0f)
                {
                    momentum += friction;
                    if(momentum >= -0.02f)
                    {
                        momentum = 0;
                    }
                }
            }
            last_momentum = momentum;
        }

        private void HandleCollistion(List<Sprite> gameObjectList)
        {
            foreach(Sprite obj in gameObjectList)
            {
                if(obj._Tag == SpriteType.kPlayerType)
                {
                    continue;
                }

                if(obj._Tag == SpriteType.kGearType && obj._CurrentState == SpriteState.kStateActive)
                {
                    if (_BoundingBox.Intersects(obj._BoundingBox))
                    {
                        obj.ReceiveDamage(1);
                        speed += 10f;
                    }
                }

                if((obj._Tag == SpriteType.kCarType || obj._Tag == SpriteType.kRockType) && obj._CurrentState == SpriteState.kStateActive)
                {
                    if(_BoundingBox.Intersects(obj._BoundingBox))
                    {
                        this.ReceiveDamage(1);
                    }
                }
            }
        }

        public override void LockInBounds()
        {
            if ((_Position.X - (_Texture.Width / 2)) <= 0)
            {
                _Position.X = _Texture.Width / 2;
                momentum = last_momentum;
            }
            if ((_Position.X + (_Texture.Width / 2)) > 320)
            {
                _Position.X = 320 - (_Texture.Width / 2);
                momentum = last_momentum;
            }
        }
    }
}
