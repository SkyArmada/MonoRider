using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoRider
{
    public class Sprite
    {
        public Texture2D _Texture;
        public Vector2 _Position;
        public bool _Draw = true;
        public int _HP;
        public float _Rotation = 0.0f;
        public float _zOrder;
        public float _Scale = 1.0f;
        public bool _FlipX = false;
        public bool _FlipY = false;
        public bool _LockInScreen = false;
        public List<Sprite> _ChildrenList;
        public Color _MyColor = Color.White;
        public Sprite parent = null;
        private ContentManager content;
        public float speed = 0f;
        public int midpoint = 160;
        public GamePlayScene parentScene = null;

        public enum SpriteState
        {
            kStateActive,
            kStateInActive
        }
        public SpriteState _CurrentState = SpriteState.kStateInActive;

        public enum SpriteType
        {
            kPlayerType,
            kGearType,
            kCarType,
            kRockType,
            kWheelType,
            kNoneType
        }
        public SpriteType _Tag = SpriteType.kNoneType;
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

        public virtual void Update(GameTime gameTime, List<Sprite> gameObjectList)
        {
            if (_CurrentState == SpriteState.kStateActive)
            {
                if (_ChildrenList != null)
                {
                    if (_ChildrenList.Count >= 1)
                    {
                        foreach (Sprite child in _ChildrenList)
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
                        foreach (Sprite child in _ChildrenList)
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
            _CurrentState = SpriteState.kStateInActive;
            _Draw = false;
        }

        public void AddChild(Sprite child)
        {
            child.parent = this;
            _ChildrenList.Add(child);
        }

        public virtual void LockInBounds()
        {
            if ((_Position.X - (_Texture.Width / 2)) <= 0)
            {
                _Position.X = _Texture.Width / 2;
            }
            if ((_Position.X + (_Texture.Width / 2)) > 320)
            {
                _Position.X = 320 - (_Texture.Width / 2);
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

        public virtual void ResetSelf()
        {
            _Texture = null;
            _Position = Vector2.Zero;
            _Draw = true;
            _CurrentState = SpriteState.kStateActive;
            _Tag = SpriteType.kNoneType;
            _Rotation = 0.0f;
            _Scale = 1.0f;
            _FlipX = false;
            _FlipY = false;
            _LockInScreen = false; 
            if (_ChildrenList != null)
            {
                if (_ChildrenList.Count >= 1)
                {
                    _ChildrenList.Clear();
                }
            }
            _MyColor = Color.White;
            parent = null;
            content = null;
            speed = 0f; 
            midpoint = 85;
            Setup();
        }

        public virtual void Setup()
        {

        }

        public virtual void Activate()
        {
            _CurrentState = SpriteState.kStateActive;
            _Draw = true;
        }
    }
}
