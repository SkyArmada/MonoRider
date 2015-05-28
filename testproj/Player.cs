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
        private Vector2 center;
        private int loops = 0;
        public override void Initialize(Texture2D texture, Vector2 position)
        {
            _HP = 1;
            _Tag = "player";
            _LockInScreen = true;
            base.Initialize(texture, position);
            center = new Vector2(_Texture.Height / 2, _Texture.Width / 2);
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
            base.Update(gameTime, gameObjectList);
        }

        private void handleMovement(GameTime gameTime)
        {
            int friction = 10;
            if (!spinOut)
            {
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            //if (momentum >= friction)
            //{
            //    momentum -= friction;
            //}
            //else if(momentum <= -friction)
            //{
            //    momentum += friction;
            //}
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
