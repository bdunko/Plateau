using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Plateau.Entities
{
    interface ILightSource
    {
        Color GetLightColor();
        Area.LightSource.Strength GetLightStrength();
        Vector2 GetLightPosition();
    }
}
