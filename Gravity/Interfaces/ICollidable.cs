using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Interfaces
{
    public interface ICollidable
    {
        public FloatRect GetBoundingBox();
        public void OnWallCollide(FloatRect wallTile);
    }
}
