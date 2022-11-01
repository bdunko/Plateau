using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;
using Plateau.Particles;

namespace Plateau.Entities
{
    class TEntityToolableLightSource : TEntityToolable, ILightSource
    {
        private Color lightColor;
        private Area.LightSource.Strength lightStrength;

        public TEntityToolableLightSource(Texture2D texture, Vector2 tilePosition, EntityType type, Item.Tag toolUsed, HealthBar hb, LootTables.LootTable lootTable, Color particlePrimary, Color particleSecondary, Color lightColor, Area.LightSource.Strength lightStrength) :
            base(texture, tilePosition, type, toolUsed, hb, lootTable, particlePrimary, particleSecondary)
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
