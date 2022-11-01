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
    class TEntitySeasonalGatherableContainer : TEntityGatherableContainer
    {
        private LootTables.LootTable springLoot, summerLoot, autumnLoot, winterLoot;
        private Texture2D springTexture, summerTexture, autumnTexture, winterTexture;

        public TEntitySeasonalGatherableContainer(Texture2D springTexture, Texture2D summerTexture, Texture2D autumnTexture, Texture2D winterTexture, Texture2D harvestedTexture, Vector2 tilePosition, EntityType type, Color particle1, Color particle2, float refreshChance, 
            LootTables.LootTable springLoot, LootTables.LootTable summerLoot, LootTables.LootTable autumnLoot, LootTables.LootTable winterLoot, Area area) : base(springTexture, harvestedTexture, tilePosition, type, particle1, particle2, springLoot, refreshChance)
        {
            this.springLoot = springLoot;
            this.summerLoot = summerLoot;
            this.autumnLoot = autumnLoot;
            this.winterLoot = winterLoot;
            this.springTexture = springTexture;
            this.summerTexture = summerTexture;
            this.autumnTexture = autumnTexture;
            this.winterTexture = winterTexture;
            UpdateForSeason(area.GetSeason());  
        }

        private void UpdateForSeason(World.Season season)
        {
            switch (season)
            {
                case World.Season.SPRING:
                    texture = springTexture;
                    lootTable = springLoot;
                    break;
                case World.Season.SUMMER:
                    lootTable = summerLoot;
                    texture = summerTexture;
                    break;
                case World.Season.AUTUMN:
                    lootTable = autumnLoot;
                    texture = autumnTexture;
                    break;
                case World.Season.WINTER:
                    lootTable = winterLoot;
                    texture = winterTexture;
                    break;
                default:
                    break;
            }
        }

        public override void LoadSave(SaveState state)
        {
            World.Season season;
            Enum.TryParse(state.TryGetData("season", World.Season.SPRING.ToString()), out season);
            UpdateForSeason(season);
            base.LoadSave(state);
        }

        public override SaveState GenerateSave()
        {
            SaveState state = base.GenerateSave();
            World.Season currentSeason = World.Season.NONE;
            if(texture == springTexture)
            {
                currentSeason = World.Season.SPRING;
            } else if (texture == summerTexture)
            {
                currentSeason = World.Season.SUMMER;
            } else if (texture == autumnTexture)
            {
                currentSeason = World.Season.AUTUMN;
            } else if (texture == winterTexture)
            {
                currentSeason = World.Season.WINTER;
            }

            state.AddData("season", currentSeason.ToString());
            return state;
        }

        public override void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            base.TickDaily(timeData, area, player);
            UpdateForSeason(area.GetSeason());
        }

    }
}
