using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    class TEntitySeasonalForageLightSource : TEntitySeasonalForage, ILightSource
    {
        private Color lightColor;
        private Area.LightSource.Strength lightStrength;
        public TEntitySeasonalForageLightSource(Texture2D tex, Vector2 position, EntityType type, Color particle1, Color particle2, World.Season season, LootTables.LootTable lootTable, Color lightColor, Area.LightSource.Strength lightStrength) : base(tex, position, type, particle1, particle2, season, lootTable)
        {
            this.lightColor = lightColor;
            this.lightStrength = lightStrength;
        }

        public Color GetLightColor()
        {
            return lightColor;
        }

        public Area.LightSource.Strength GetLightStrength()
        {
            return lightStrength;
        }

        public Vector2 GetLightPosition()
        {
            Vector2 lightPos = new Vector2(position.X, position.Y);
            lightPos.X += texture.Width / 2;
            lightPos.Y += 6;
            return lightPos;
        }
    }
}
