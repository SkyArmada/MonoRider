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
        bool shielded = false;
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
            _HP = startHP;
            _Tag = SpriteType.kPlayerType;
            _LockInScreen = true;
            _zOrder = 15f;
            _ChildrenList = new List<Sprite>();
            speed = 100f;
            isAnimated = true;
        }

        public override void LoadContent(string path, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(path, Content);
            base.SetupAnimation(5, 30, 1, true);
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
            Animate(0);
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
                        parentScene.gearsCollected++;
                    }
                }

                if((obj._Tag == SpriteType.kCarType || obj._Tag == SpriteType.kRockType) && obj._CurrentState == SpriteState.kStateActive)
                {
                    if(_BoundingBox.Intersects(obj._BoundingBox))
                    {
                        if (this.shielded)
                        {
                            this.shielded = false;
                            this.ChangeColor(new Color(255, 255, 255, 255), new Color(213, 255, 28, 255));
                            obj.ReceiveDamage(1);
                        }
                        else
                        {
                            obj.ReceiveDamage(1);
                            this.ReceiveDamage(1);
                        }
                    }
                }

                if(obj._Tag == SpriteType.kShieldType && obj._CurrentState == SpriteState.kStateActive)
                {
                    if(this._BoundingBox.Intersects(obj._BoundingBox))
                    {
                        obj.ReceiveDamage(1);
                        this.shielded = true;
                        this.ChangeColor(new Color(213, 255, 28, 255), new Color(255, 255, 255, 255));
                    }
                }

                if (obj._Tag == SpriteType.kOilType && obj._CurrentState == SpriteState.kStateActive)
                {
                    if (this._BoundingBox.Intersects(obj._BoundingBox))
                    {
                        obj.ReceiveDamage(1);
                        this.spinOut = true;
                        _Rotation = 0;
                    }
                }
            }
        }

        public override void LockInBounds()
        {
            if ((_Position.X - (frameWidth / 2)) <= 0)
            {
                _Position.X = frameWidth / 2;
                momentum = last_momentum;
            }
            if ((_Position.X + (frameWidth / 2)) > 320)
            {
                _Position.X = 320 - (frameWidth / 2);
                momentum = last_momentum;
            }
        }
    }
}
