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
            HP = 1;
            Tag = "gear";
            base.Initialize(texture, position);
        }
    }
}
