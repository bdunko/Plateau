using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Items
{
    public class PlantableItem : Item
    {
        private EntityType type;
        private Vector2 placementOffset;

        public PlantableItem(string name, string texturePath, EntityType type, Vector2 placementOffset, int stackCapacity, string description, int value, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.type = type;
            this.placementOffset = placementOffset;
        }

        public Vector2 GetPlacementOffset()
        {
            return placementOffset;
        }

        public EntityType GetPlantedType()
        {
            return type;
        }
    }
}
