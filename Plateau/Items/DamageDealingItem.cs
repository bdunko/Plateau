using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plateau.Items.Item;

namespace Plateau.Items
{
    public class DamageDealingItem : Item
    {
        private int damage;

        public DamageDealingItem(string name, string texturePath, int stackCapacity, int damage, string description, int value, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.damage = damage;
        }

        public int GetDamage(EntityPlayer player, World.TimeData timeData)
        {
            return GetDamage(player, timeData, LootTables.FishingLootTable.FishType.CLOUD);
        }

        public int GetDamage(EntityPlayer player, World.TimeData timeData, LootTables.FishingLootTable.FishType fishType)
        {
            float modifiedDamage = damage;
            if(HasTag(Item.Tag.AXE))
            {
                if (player.HasEffect(AppliedEffects.CHOPPING_VI))
                {
                    modifiedDamage += 12;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_V))
                {
                    modifiedDamage += 10;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_IV))
                {
                    modifiedDamage += 8;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_III) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_III_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    modifiedDamage += 6;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_II) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_II_SPRING) && timeData.season == World.Season.SPRING))
                {
                    modifiedDamage += 4;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_I) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_I_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    modifiedDamage += 2;
                }
                else if (player.HasEffect(AppliedEffects.DIZZY))
                {
                    modifiedDamage *= 0.25f;
                }
            } else if (HasTag(Item.Tag.FISHING_ROD))
            {
                if (player.HasEffect(AppliedEffects.FISHING_VI) ||
                   (player.HasEffect(AppliedEffects.FISHING_VI_SUMMER) && timeData.season == World.Season.SUMMER) ||
                   (player.HasEffect(AppliedEffects.FISHING_VI_CAVE) && fishType == LootTables.FishingLootTable.FishType.CAVE) ||
                   (player.HasEffect(AppliedEffects.FISHING_VI_CLOUD) && fishType == LootTables.FishingLootTable.FishType.CLOUD) ||
                   (player.HasEffect(AppliedEffects.FISHING_VI_LAVA) && fishType == LootTables.FishingLootTable.FishType.LAVA))
                {
                    modifiedDamage += 12;
                }
                else if (player.HasEffect(AppliedEffects.FISHING_V)
                     || (player.HasEffect(AppliedEffects.FISHING_V_OCEAN) && fishType == LootTables.FishingLootTable.FishType.OCEAN))
                {
                    modifiedDamage += 10;
                }
                else if (player.HasEffect(AppliedEffects.FISHING_IV)
                     || (player.HasEffect(AppliedEffects.FISHING_IV_CAVE) && fishType == LootTables.FishingLootTable.FishType.CAVE)
                     || (player.HasEffect(AppliedEffects.FISHING_IV_CLOUD) && fishType == LootTables.FishingLootTable.FishType.CLOUD)
                     || (player.HasEffect(AppliedEffects.FISHING_IV_FRESHWATER) && fishType == LootTables.FishingLootTable.FishType.FRESHWATER)
                     || (player.HasEffect(AppliedEffects.FISHING_IV_LAVA) && fishType == LootTables.FishingLootTable.FishType.LAVA)
                     || (player.HasEffect(AppliedEffects.FISHING_IV_OCEAN) && fishType == LootTables.FishingLootTable.FishType.OCEAN))
                {
                    modifiedDamage += 8;
                }
                else if (player.HasEffect(AppliedEffects.FISHING_III)
                     || (player.HasEffect(AppliedEffects.FISHING_III_AUTUMN) && timeData.season == World.Season.AUTUMN)
                     || (player.HasEffect(AppliedEffects.FISHING_III_OCEAN) && fishType == LootTables.FishingLootTable.FishType.OCEAN)
                     || (player.HasEffect(AppliedEffects.FISHING_III_FRESHWATER) && fishType == LootTables.FishingLootTable.FishType.FRESHWATER)
                     || (player.HasEffect(AppliedEffects.FISHING_III_LAVA) && fishType == LootTables.FishingLootTable.FishType.LAVA)
                     || (player.HasEffect(AppliedEffects.FISHING_III_CLOUD) && fishType == LootTables.FishingLootTable.FishType.CLOUD))
                {
                    modifiedDamage += 6;
                }
                else if (player.HasEffect(AppliedEffects.FISHING_II)
                    || (player.HasEffect(AppliedEffects.FISHING_II_SUMMER) && timeData.season == World.Season.SUMMER)
                    || (player.HasEffect(AppliedEffects.FISHING_II_OCEAN) && fishType == LootTables.FishingLootTable.FishType.OCEAN))
                {
                    modifiedDamage += 4;
                }
                else if (player.HasEffect(AppliedEffects.FISHING_I))
                {
                    modifiedDamage += 2;
                }
                else if (player.HasEffect(AppliedEffects.DIZZY))
                {
                    modifiedDamage *= 0.25f;
                }
            } else if (HasTag(Item.Tag.PICKAXE))
            {
                if (player.HasEffect(AppliedEffects.MINING_VI) ||
                    (player.HasEffect(AppliedEffects.MINING_VI_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    modifiedDamage += 12;
                }
                else if (player.HasEffect(AppliedEffects.MINING_V))
                {
                    modifiedDamage += 10;
                }
                else if (player.HasEffect(AppliedEffects.MINING_IV))
                {
                    modifiedDamage += 8;
                }
                else if (player.HasEffect(AppliedEffects.MINING_III) ||
                    (player.HasEffect(AppliedEffects.MINING_III_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    modifiedDamage += 6;
                }
                else if (player.HasEffect(AppliedEffects.MINING_II))
                {
                    modifiedDamage += 4;
                }
                else if (player.HasEffect(AppliedEffects.MINING_I))
                {
                    modifiedDamage += 2;
                }
                else if (player.HasEffect(AppliedEffects.DIZZY))
                {
                    modifiedDamage *= 0.25f;
                }
            } else if (HasTag(Item.Tag.HOE) || HasTag(Item.Tag.WATERING_CAN))
            {
                if (player.HasEffect(AppliedEffects.DIZZY))
                {
                    modifiedDamage /= 2;
                    if (modifiedDamage < 1)
                    {
                        modifiedDamage = 1;
                    }
                }
            } 

            if(player.HasEffect(AppliedEffects.WISHBOAT_HEALTH))
            {
                if(HasTag(Item.Tag.HOE) || HasTag(Item.Tag.WATERING_CAN))
                {
                    modifiedDamage += 2;
                } else
                {
                    modifiedDamage += 5;
                }
            }

            return (int)modifiedDamage;
        }
    }
}
