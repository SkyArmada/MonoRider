using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider
{
    class GameCharacterBase
    {
        public Texture2D _Texture;
        public Vector2 _Position;
        public bool _Draw = true;
        public bool _Active = true;
        public int _HP;
        public string _Tag = "base";

        public float _Rotation = 0.0f;
        public Vector2 _Center
        {
            get
            {
                return new Vector2(_Texture.Width / 2, _Texture.Height / 2);
            }
        }
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)_Position.X, (int)_Position.Y, _Texture.Width, _Texture.Height);
            }
        }

        public virtual void Initialize(Texture2D texture, Vector2 position)
        {
            _Texture = texture;
            _Position = position;
        }

        public virtual void Update(GameTime gameTime, List<GameCharacterBase> gameObjectList)
        {
            float speed = 240.0f;
            _Position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random num = new Random();
            if (_Position.Y > 500)
            {
                _Position.Y = -10 * num.Next(250);
                _Position.X = num.Next(320 - _Texture.Width);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_Draw)
            {
                Rectangle sr = new Rectangle(0, 0, _Texture.Width, _Texture.Height);
                spriteBatch.Draw(_Texture, _Position, sr, Color.White, _Rotation, _Center, 1.0f, SpriteEffects.None, 1f);
            }
        }

        public virtual void ReceiveDamage(int amt)
        {
            _HP -= amt;
            if(_HP <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _Active = false;
            _Draw = false;
        }
    }
}
