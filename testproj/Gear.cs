using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRider
{
    class Gear : GameCharacterBase
    {
        public override void Initialize(Texture2D texture, Vector2 position)
        {
            _HP = 1;
            _Tag = "gear";
            base.Initialize(texture, position);
        }

        public override void Update(GameTime gameTime, List<GameCharacterBase> gameObjectList)
        {
            _Rotation += 0.05f;
            base.Update(gameTime, gameObjectList);
        }
    }
}
