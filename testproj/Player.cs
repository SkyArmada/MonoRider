using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace MonoRider
{
    class Player : GameCharacterBase
    {
        public float momentum = 0;
        bool spinOut = false;
        private int loops = 0;
        Gear testGear;

        public Player()
        {
            _HP = 1;
            _Tag = "player";
            _LockInScreen = true;
            _zOrder = 15f;
            testGear = new Gear();
            _ChildrenList = new List<GameCharacterBase>();
        }

        public override void LoadContent(string path, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(path, Content);
            testGear.LoadContent("Graphics/gear1", Content);
            testGear._Position = new Vector2(200, 320);
            this.AddChild(testGear);
        }

        public override void Update(GameTime gameTime, List<GameCharacterBase> gameObjectList)
        {
            if(_Active)
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
            //testGear._Position = new Vector2(_Position.X + 20, _Position.Y);
            base.Update(gameTime, gameObjectList);
        }

        private void handleMovement(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float friction = 5.0f * delta;
            if (!spinOut)
            {
                KeyboardState state = Keyboard.GetState();
                int momentumGain = 15;
                if (state.IsKeyDown(Keys.A))
                {
                    momentum -= momentumGain * delta;
                }
                else if (state.IsKeyDown(Keys.D))
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

            if (momentum >= 0f)
            {
                momentum -= friction;
            }
            else if(momentum <= 0f)
            {
                momentum += friction;
            }
        }

        private void HandleCollistion(List<GameCharacterBase> gameObjectList)
        {
            foreach(GameCharacterBase obj in gameObjectList)
            {
                if(obj._Tag.Equals("player"))
                {
                    continue;
                }

                if(obj._Tag.Equals("gear"))
                {
                    if (_BoundingBox.Intersects(obj._BoundingBox))
                    {
                        obj.ReceiveDamage(1);
                    }
                }
            }
        }
    }
}
