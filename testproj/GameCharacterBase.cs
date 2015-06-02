using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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
        public float _zOrder;
        public float _Scale = 1.0f;
        public bool _FlipX = false;
        public bool _FlipY = false;
        public bool _LockInScreen = false;
        public List<GameCharacterBase> _ChildrenList;
        public Color _MyColor = Color.White;
        public GameCharacterBase parent = null;
        private ContentManager content;

        public Vector2 _Center
        {
            get
            {
                return new Vector2(_Texture.Width / 2, _Texture.Height / 2);
            }
        }

        public Rectangle _BoundingBox
        {
            get
            {
                return new Rectangle((int)_Position.X, (int)_Position.Y, _Texture.Width, _Texture.Height);
            }
        }

        public virtual void LoadContent(string path, ContentManager Content)
        {
            content = Content;
            _Texture = content.Load<Texture2D>(path);
        }

        public virtual void Update(GameTime gameTime, List<GameCharacterBase> gameObjectList)
        {
            if (_Active)
            {
                if (_ChildrenList != null)
                {
                    if (_ChildrenList.Count >= 1)
                    {
                        foreach (GameCharacterBase child in _ChildrenList)
                        {
                            child.Update(gameTime, gameObjectList);
                        }
                    }
                }

                if (_LockInScreen)
                {
                    LockInBounds();
                }
            }
            else
            {
                _Position = new Vector2(-500, -500);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_Draw)
            {
                Rectangle sr = new Rectangle(0, 0, _Texture.Width, _Texture.Height);
                if(!_FlipX && !_FlipY)
                {
                    spriteBatch.Draw(_Texture, _Position, sr, _MyColor, _Rotation, _Center, _Scale, SpriteEffects.None, 0f);
                }
                else if(_FlipX)
                {
                    spriteBatch.Draw(_Texture, _Position, sr, _MyColor, _Rotation, _Center, _Scale, SpriteEffects.FlipHorizontally, 0f);
                }
                else if(_FlipY)
                {
                    spriteBatch.Draw(_Texture, _Position, sr, _MyColor, _Rotation, _Center, _Scale, SpriteEffects.FlipVertically, 0f);
                }
                else if(_FlipX && _FlipY)
                {
                    spriteBatch.Draw(_Texture, _Position, sr, _MyColor, (_Rotation + (float)Math.PI), _Center, _Scale, SpriteEffects.None, 0f);
                }

                if (_ChildrenList != null)
                {
                    if (_ChildrenList.Count >= 1)
                    {
                        foreach (GameCharacterBase child in _ChildrenList)
                        {
                            child.Draw(spriteBatch);
                        }
                    }
                }
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

        public void AddChild(GameCharacterBase child)
        {
            child.parent = this;
            _ChildrenList.Add(child);
        }

        public void LockInBounds()
        {
            if ((_Position.X - (_Texture.Width / 2)) <= 0)
            {
                _Position.X = _Texture.Width / 2;
                //momentum = 0;
            }
            if ((_Position.X + (_Texture.Width / 2)) > 320)
            {
                _Position.X = 320 - (_Texture.Width / 2);
                //momentum = 0;
            }
        }

        public void ChangeColor(Color searchColor, Color toColor)
        {
            Color[] data = new Color[_Texture.Width * _Texture.Height];
            _Texture.GetData(data);
            for(int i = 0; i < data.Length; i++)
            {
                if(data[i] == searchColor)
                {
                    data[i] = toColor;
                }
            }

            _Texture.SetData(data);
        }
    }
}
