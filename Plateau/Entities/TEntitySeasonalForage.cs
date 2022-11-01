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
    class TEntitySeasonalForage : TEntityForage
    {
        private World.Season season;

        public TEntitySeasonalForage(Texture2D tex, Vector2 position, EntityType type, Color particle1, Color particle2, World.Season season, LootTables.LootTable lootTable) : base(tex, position, type, particle1, particle2, lootTable)
        {
            this.season = season;
        }

        public World.Season GetSeason()
        {
            return this.season;
        }
    }
}
