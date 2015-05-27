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
        public override void Initialize(Texture2D texture, Vector2 position)
        {
            HP = 1;
            Tag = "player";
            base.Initialize(texture, position);
            center = new Vector2(_Texture.Height / 2, _Texture.Width / 2);
        }


        public override void Update(GameTime gameTime, List<GameCharacterBase> objs)
        {
            if(_Active)
            {
                handleMovement(gameTime);
                HandleCollistion(objs);
                if(spinOut)
                {
                    rotation += 0.05f;
                }
                LockInBounds();
            }
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

        private void HandleCollistion(List<GameCharacterBase> objs)
        {
            foreach(GameCharacterBase obj in objs)
            {
                if(obj.Tag.Equals("player"))
                {
                    continue;
                }

                if(obj.Tag.Equals("gear"))
                {
                    if(this.BoundingBox.Intersects(obj.BoundingBox))
                    {
                        obj.ReceiveDamage(1);
                    }
                }
            }
        }

        private void LockInBounds()
        {
            if(_Position.X <= 0)
            {
                _Position.X = 0;
                //momentum = 0;
            }
            if (_Position.X >= (320 - _Texture.Width))
            {
                _Position.X = (320 - _Texture.Width);
                //momentum = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, _Position, null, Color.White, rotation, center, 1f, SpriteEffects.None, 0f);
        }
    }
}
