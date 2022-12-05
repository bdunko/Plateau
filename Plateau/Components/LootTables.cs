using Plateau.Entities;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class LootTables
    {
        public class LootEntry
        {
            public static Func<World.TimeData, Area, EntityPlayer, bool> noCondition = (timeData, area, player) => { return true; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> springOnly = (timeData, area, player) => { return timeData.season == World.Season.SPRING; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> summerOnly = (timeData, area, player) => { return timeData.season == World.Season.SUMMER; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> autumnOnly = (timeData, area, player) => { return timeData.season == World.Season.AUTUMN; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> winterOnly = (timeData, area, player) => { return timeData.season == World.Season.WINTER; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> notWinterOnly = (timeData, area, player) => { return timeData.season != World.Season.WINTER; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> farmOnly = (timeData, area, player) => { return area.GetAreaEnum() == Area.AreaEnum.FARM; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> townOnly = (timeData, area, player) => { return area.GetAreaEnum() == Area.AreaEnum.TOWN; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> s0Only = (timeData, area, player) => { return area.GetAreaEnum() == Area.AreaEnum.S0; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> s1Only = (timeData, area, player) => { return area.GetAreaEnum() == Area.AreaEnum.S1; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> s2Only = (timeData, area, player) => { return area.GetAreaEnum() == Area.AreaEnum.S2; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> s3Only = (timeData, area, player) => { return area.GetAreaEnum() == Area.AreaEnum.S3; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> s4Only = (timeData, area, player) => { return area.GetAreaEnum() == Area.AreaEnum.S4; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> notS1Only = (timeData, area, player) => { return area.GetAreaEnum() != Area.AreaEnum.S1; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> notS2Only = (timeData, area, player) => { return area.GetAreaEnum() != Area.AreaEnum.S2; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> notS3Only = (timeData, area, player) => { return area.GetAreaEnum() != Area.AreaEnum.S3; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> notS4Only = (timeData, area, player) => { return area.GetAreaEnum() != Area.AreaEnum.S4; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> rainyOnly = (timeData, area, player) => { return area.GetWeather() == World.Weather.RAINY; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> sunnyOnly = (timeData, area, player) => { return area.GetWeather() == World.Weather.SUNNY; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> nightOnly = (timeData, area, player) => { return timeData.timeOfDay == World.TimeOfDay.NIGHT; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> notNightOnly = (timeData, area, player) => { return timeData.timeOfDay != World.TimeOfDay.NIGHT; };
            public static Func<World.TimeData, Area, EntityPlayer, bool> morningOnly = (timeData, area, player) => { return timeData.timeOfDay == World.TimeOfDay.MORNING; };

            public int weight;
            public Item item;
            public Func<World.TimeData, Area, EntityPlayer, bool>[] requirements;

            public LootEntry(Item item, int weight)
            {
                this.weight = weight;
                this.item = item;
                this.requirements = new Func<World.TimeData, Area, EntityPlayer, bool>[] { LootEntry.noCondition };
            }

            public LootEntry(Item item, int weight, params Func<World.TimeData, Area, EntityPlayer, bool>[] requirements)
            {
                this.weight = weight;
                this.item = item;
                this.requirements = requirements;
            }


            public bool IsPossibleDrop(World.TimeData timeData, Area area, EntityPlayer player)
            {
                foreach(Func<World.TimeData, Area, EntityPlayer, bool> reqs in requirements)
                {
                    if(reqs(timeData, area, player) != true)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public class LootTable {
            protected List<LootEntry> table;
            protected int minRolls, maxRolls;

            public LootTable(int minRolls, int maxRolls, params LootEntry[] entries)
            {
                this.minRolls = minRolls;
                this.maxRolls = maxRolls;
                table = new List<LootEntry>();
                foreach (LootEntry lootEntry in entries)
                {
                    for(int i = 0; i < lootEntry.weight; i++)
                    {
                        table.Add(lootEntry);
                    }
                }
            }

            public virtual List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> loot = new List<Item>();

                int numRolls = Util.RandInt(minRolls, maxRolls);

                while(loot.Count < numRolls)
                {
                    LootEntry roll = table[Util.RandInt(0, table.Count - 1)];
                    if (roll.IsPossibleDrop(timeData, area, player))
                    {
                        loot.Add(roll.item);
                    }
                }

                PerformLuckRerolls(player, timeData, area, loot, table);

                return loot;
            }
        }

        private class ChestLootTable : LootTable
        {
            private int numTypes;
            public ChestLootTable(int minRolls, int maxRolls, int numTypes, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                this.numTypes = numTypes;
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> itemLoot = new List<Item>();

                int rollsCompleted = 0;

                while(rollsCompleted < numTypes)
                {
                    LootEntry roll = table[Util.RandInt(0, table.Count - 1)];
                    if (roll.IsPossibleDrop(timeData, area, player))
                    {
                        itemLoot.Add(roll.item);
                        rollsCompleted++;
                    }
                }

                PerformLuckRerolls(player, timeData, area, itemLoot, table);

                List<Item> loot = new List<Item>();
                
                foreach(Item item in itemLoot)
                {
                    for(int i = 0; i < Util.RandInt(minRolls, maxRolls); i++)
                    {
                        loot.Add(item);
                        if (item.HasTag(Item.Tag.ACCESSORY) || item.HasTag(Item.Tag.CLOTHING))
                        {
                            break;
                        }
                    }
                }

                return loot;
            }
        }

        private class ForageLootTable : LootTable {
            public enum ForageType {
                NORMAL, BEACH
            }

            private ForageType type;
            private LootTable insectTable;
            private float insectChance;

            public ForageLootTable(int minRolls, int maxRolls, ForageType type, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                this.type = type;
                this.insectTable = null;
                this.insectChance = 0.0f;
            }

            public ForageLootTable(int minRolls, int maxRolls, ForageType type, float insectChance, LootTable insectTable, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                this.type = type;
                this.insectTable = insectTable;
                this.insectChance = insectChance;
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                int bonusRolls = 0;
                if(player.HasEffect(AppliedEffects.FORAGING_VI) ||
                    (player.HasEffect(AppliedEffects.FORAGING_VI_SUMMER) && timeData.season == World.Season.SUMMER) ||
                    (player.HasEffect(AppliedEffects.FORAGING_VI_AUTUMN) && timeData.season == World.Season.AUTUMN) ||
                    (player.HasEffect(AppliedEffects.FORAGING_VI_SPRING) && timeData.season == World.Season.SPRING))
                {
                    bonusRolls = 3;
                }
                else if(player.HasEffect(AppliedEffects.FORAGING_V) ||
                    (player.HasEffect(AppliedEffects.FORAGING_V_WINTER) && timeData.season == World.Season.WINTER))
                {
                    bonusRolls = Util.RandInt(2, 3);
                }
                else if(player.HasEffect(AppliedEffects.FORAGING_IV) ||
                    (player.HasEffect(AppliedEffects.FORAGING_IV_BEACH) && type == ForageType.BEACH) ||
                    (player.HasEffect(AppliedEffects.FORAGING_IV_SUMMER) && timeData.season == World.Season.SUMMER) ||
                    (player.HasEffect(AppliedEffects.FORAGING_IV_WINTER) && timeData.season == World.Season.WINTER) ||
                    (player.HasEffect(AppliedEffects.FORAGING_IV_SPRING) && timeData.season == World.Season.SPRING))
                {
                    bonusRolls = 2;
                } else if (player.HasEffect(AppliedEffects.FORAGING_III) ||
                    (player.HasEffect(AppliedEffects.FORAGING_III_AUTUMN) && timeData.season == World.Season.AUTUMN) ||
                    (player.HasEffect(AppliedEffects.FORAGING_III_SPRING) && timeData.season == World.Season.SPRING) ||
                    (player.HasEffect(AppliedEffects.FORAGING_III_SUMMER) && timeData.season == World.Season.SUMMER) ||
                    (player.HasEffect(AppliedEffects.FORAGING_III_BEACH) && type == ForageType.BEACH))
                {
                    bonusRolls = Util.RandInt(1, 2);
                } else if (player.HasEffect(AppliedEffects.FORAGING_II) ||
                    (player.HasEffect(AppliedEffects.FORAGING_II_BEACH) && type == ForageType.BEACH) ||
                    (player.HasEffect(AppliedEffects.FORAGING_II_SPRING) && timeData.season == World.Season.SPRING) ||
                    (player.HasEffect(AppliedEffects.FORAGING_II_SUMMER) && timeData.season == World.Season.SUMMER) ||
                    (player.HasEffect(AppliedEffects.FORAGING_II_WINTER) && timeData.season == World.Season.WINTER) ||
                    (player.HasEffect(AppliedEffects.FORAGING_II_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    bonusRolls = 1;
                } else if (player.HasEffect(AppliedEffects.FORAGING_I) ||
                    (player.HasEffect(AppliedEffects.FORAGING_I_SPRING) && timeData.season == World.Season.SPRING) ||
                    (player.HasEffect(AppliedEffects.FORAGING_I_AUTUMN) && timeData.season == World.Season.AUTUMN) ||
                    (player.HasEffect(AppliedEffects.FORAGING_I_WINTER) && timeData.season == World.Season.WINTER))
                {
                    bonusRolls = Util.RandInt(0, 1);
                }

                List<Item> loot = new List<Item>();
                int numRolls = Util.RandInt(minRolls, maxRolls) + bonusRolls;

                while(loot.Count < numRolls)
                {
                    LootEntry lootEntry = table[Util.RandInt(0, table.Count - 1)];
                    if(lootEntry.IsPossibleDrop(timeData, area, player))
                    {
                        Item lootGiven = lootEntry.item;

                        if (player.HasEffect(AppliedEffects.FORAGING_VI_MUSHROOMS) && lootGiven.HasTag(Item.Tag.MUSHROOM))
                        {
                            loot.Add(lootGiven);
                            loot.Add(lootGiven);
                            loot.Add(lootGiven);
                        }
                        else if (player.HasEffect(AppliedEffects.FORAGING_IV_MUSHROOMS) && lootGiven.HasTag(Item.Tag.MUSHROOM))
                        {
                            loot.Add(lootGiven);
                            loot.Add(lootGiven);
                        }
                        else if (player.HasEffect(AppliedEffects.FORAGING_III_MUSHROOMS) && lootGiven.HasTag(Item.Tag.MUSHROOM))
                        {
                            loot.Add(lootGiven);
                            if (Util.RandInt(0, 1) == 1)
                            {
                                loot.Add(lootGiven);
                            }
                        }
                        else if (player.HasEffect(AppliedEffects.FORAGING_II_MUSHROOMS) && lootGiven.HasTag(Item.Tag.MUSHROOM))
                        {
                            loot.Add(lootGiven);
                        }
                        if (player.HasEffect(AppliedEffects.FORAGING_V_FLOWERS) && lootGiven.HasTag(Item.Tag.FLOWER))
                        {
                            loot.Add(lootGiven);
                            loot.Add(lootGiven);
                            if (Util.RandInt(0, 1) == 1)
                            {
                                loot.Add(lootGiven);
                            }
                        }
                        else if (player.HasEffect(AppliedEffects.FORAGING_IV_FLOWERS) && lootGiven.HasTag(Item.Tag.FLOWER))
                        {
                            loot.Add(lootGiven);
                            loot.Add(lootGiven);
                        }
                        else if (player.HasEffect(AppliedEffects.FORAGING_III_FLOWERS) && lootGiven.HasTag(Item.Tag.FLOWER))
                        {
                            loot.Add(lootGiven);
                            if (Util.RandInt(0, 1) == 1)
                            {
                                loot.Add(lootGiven);
                            }
                        }
                        else if (player.HasEffect(AppliedEffects.FORAGING_II_FLOWERS) && lootGiven.HasTag(Item.Tag.FLOWER))
                        {
                            loot.Add(lootGiven);
                        }

                        loot.Add(lootGiven);
                    }
                }

                PerformLuckRerolls(player, timeData, area, loot, table);

                if(insectChance > (Util.RandInt(0, 1000)/1000.0f))
                {
                    foreach(Item item in insectTable.RollLoot(player, area, timeData))
                    {
                        loot.Add(item);
                    }
                }

                return loot;
            }
        }

        private class TreeLootTable : LootTable
        {
            public TreeLootTable(int minRolls, int maxRolls, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                //do nothing
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> loot = new List<Item>();
                int woodBoost = 0;
                if (player.HasEffect(AppliedEffects.CHOPPING_VI))
                {
                    woodBoost = 6;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_V))
                {
                    woodBoost = 5;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_IV))
                {
                    woodBoost = 4;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_III) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_III_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    woodBoost = 3;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_II) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_II_SPRING) && timeData.season == World.Season.SPRING))
                {
                    woodBoost = 2;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_I)||
                    (player.HasEffect(AppliedEffects.CHOPPING_I_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    woodBoost = 1;
                }

                int numRolls = Util.RandInt(minRolls, maxRolls) + woodBoost;

                while(loot.Count < numRolls)
                {
                    LootEntry entry = table[Util.RandInt(0, table.Count - 1)];
                    if(entry.IsPossibleDrop(timeData, area, player))
                    {
                        loot.Add(entry.item);
                    }
                }

                LootTables.PerformLuckRerolls(player, timeData, area, loot, table);

                return loot;
            }
        }

        public class FishingLootTable : LootTable
        {
            public enum FishType {
                OCEAN, FRESHWATER, LAVA, CAVE, CLOUD
            }

            private FishType fishType;

            public FishType GetFishType()
            {
                return fishType;
            }

            public FishingLootTable(int minRolls, int maxRolls, FishType fishType, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                this.fishType = fishType;
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> loot = new List<Item>();
                bool replacedBySeasonal = false;

                if (fishType == FishType.OCEAN)
                {
                    switch (area.GetSeason())
                    {
                        case World.Season.SPRING:
                            if (Util.RandInt(0, table.Count + 25) < 25)
                            {
                                loot.Add(ItemDict.MACKEREL);
                                replacedBySeasonal = true;
                            }
                            break;
                        case World.Season.SUMMER:
                            int roll = Util.RandInt(0, table.Count + 12 + 7);
                            if (roll < 12)
                            {
                                loot.Add(ItemDict.SHRIMP);
                                replacedBySeasonal = true;
                            }
                            else if (roll < 12 + 7)
                            {
                                loot.Add(ItemDict.PUFFERFISH);
                                replacedBySeasonal = true;
                            }
                            break;
                        case World.Season.AUTUMN:
                            if (Util.RandInt(0, table.Count + 5) < 5)
                            {
                                loot.Add(ItemDict.SEA_TURTLE);
                                replacedBySeasonal = true;
                            }
                            break;
                        case World.Season.WINTER:
                            if (Util.RandInt(0, table.Count + 9) < 9)
                            {
                                loot.Add(ItemDict.TUNA);
                                replacedBySeasonal = true;
                            }
                            break;
                    }
                } else if (fishType == FishType.FRESHWATER)
                {
                    //seasonal freshwater fish...
                }


                if (!replacedBySeasonal) {
                    loot = base.RollLoot(player, area, timeData);
                }

                PerformLuckRerolls(player, timeData, area, loot, table);

                return loot;
            }
        }

        private class ChoppingLootTable : LootTable
        {
            public ChoppingLootTable(int minRolls, int maxRolls, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                //do nothing
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> loot = new List<Item>();
                int boost = 0;
                if (player.HasEffect(AppliedEffects.CHOPPING_VI))
                {
                    boost = 3;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_V))
                {
                    boost = Util.RandInt(2, 3);
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_IV))
                {
                    boost = 2;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_III) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_III_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    boost = Util.RandInt(1, 2);
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_II) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_II_SPRING) && timeData.season == World.Season.SPRING))
                {
                    boost = 1;
                }
                else if (player.HasEffect(AppliedEffects.CHOPPING_I) ||
                    (player.HasEffect(AppliedEffects.CHOPPING_I_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    boost = Util.RandInt(0, 1);
                }


                int numRolls = Util.RandInt(minRolls, maxRolls) + boost;

                while(loot.Count < numRolls)
                {
                    LootEntry entry = table[Util.RandInt(0, table.Count - 1)];

                    if(entry.IsPossibleDrop(timeData, area, player))
                    {
                        loot.Add(entry.item);
                    }
                }

                PerformLuckRerolls(player, timeData, area, loot, table);

                return loot;
            }
        }

        private class MiningLootTable : LootTable
        {
            public MiningLootTable(int minRolls, int maxRolls, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                //nothing
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> loot = new List<Item>();
                int boost = 0;
                if (player.HasEffect(AppliedEffects.MINING_VI) ||
                    (player.HasEffect(AppliedEffects.MINING_VI_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    boost = 3;
                }
                else if (player.HasEffect(AppliedEffects.MINING_V))
                {
                    boost = Util.RandInt(2, 3);
                }
                else if (player.HasEffect(AppliedEffects.MINING_IV))
                {
                    boost = 2;
                }
                else if (player.HasEffect(AppliedEffects.MINING_III) ||
                    (player.HasEffect(AppliedEffects.MINING_III_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    boost = Util.RandInt(1, 2);
                }
                else if (player.HasEffect(AppliedEffects.MINING_II))
                {
                    boost = 1;
                }
                else if (player.HasEffect(AppliedEffects.MINING_I))
                {
                    boost = Util.RandInt(0, 1);
                }

                int numRolls = Util.RandInt(minRolls, maxRolls) + boost;

                while(loot.Count < numRolls)
                {
                    LootEntry entry = table[Util.RandInt(0, table.Count - 1)];
                    if(entry.IsPossibleDrop(timeData, area, player))
                    {
                        loot.Add(entry.item);
                    }
                }

                PerformLuckRerolls(player, timeData, area, loot, table);

                return loot;
            }
        }

        private class GatheringLootTable : LootTable
        {
            public enum GatheringType
            {
                COW, CHICKEN, BOAR, SHEEP, PIG
            }

            private GatheringType type;

            public GatheringLootTable(int minRolls, int maxRolls, GatheringType gatheringType, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                this.type = gatheringType;
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> loot = new List<Item>();
                int boost = 0;
                if (player.HasEffect(AppliedEffects.GATHERING_BOAR) && type == GatheringType.BOAR)
                {
                    boost = Util.RandInt(1, 2);
                }
                else if (player.HasEffect(AppliedEffects.GATHERING_CHICKEN) && type == GatheringType.CHICKEN)
                {
                    boost = Util.RandInt(1, 2);
                }
                else if (player.HasEffect(AppliedEffects.GATHERING_COW) && type == GatheringType.COW)
                {
                    boost = Util.RandInt(1, 2);
                }
                else if (player.HasEffect(AppliedEffects.GATHERING_SHEEP) && type == GatheringType.SHEEP)
                {
                    boost = Util.RandInt(1, 2);
                }
                else if(player.HasEffect(AppliedEffects.GATHERING_PIG) && type == GatheringType.PIG)
                {
                    boost = Util.RandInt(1, 2);
                }

                int numRolls = Util.RandInt(minRolls, maxRolls) + boost;

                while(loot.Count < numRolls)
                {
                    LootEntry entry = table[Util.RandInt(0, table.Count - 1)];
                    if(entry.IsPossibleDrop(timeData, area, player))
                    {
                        loot.Add(entry.item);
                    }
                }

                PerformLuckRerolls(player, timeData, area, loot, table);

                return loot;
            }
        }

        private class InsectLootTable : LootTable
        {
            public InsectLootTable(int minRolls, int maxRolls, params LootEntry[] entries) : base(minRolls, maxRolls, entries)
            {
                //nothing
            }

            public override List<Item> RollLoot(EntityPlayer player, Area area, World.TimeData timeData)
            {
                List<Item> loot = new List<Item>();
                int boost = 0;
                if (player.HasEffect(AppliedEffects.BUG_CATCHING_VI) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_VI_NIGHT) && timeData.timeOfDay == World.TimeOfDay.NIGHT) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_VI_AUTUMN) && timeData.season == World.Season.AUTUMN) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_VI_SPRING) && timeData.season == World.Season.SPRING) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_VI_SUMMER) && timeData.season == World.Season.SUMMER))
                {
                    boost = 3;
                }
                else if (player.HasEffect(AppliedEffects.BUG_CATCHING_V))
                {
                    boost = Util.RandInt(2, 3);
                }
                else if (player.HasEffect(AppliedEffects.BUG_CATCHING_IV) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_IV_MORNING) && timeData.timeOfDay == World.TimeOfDay.MORNING) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_IV_NIGHT) && timeData.timeOfDay == World.TimeOfDay.NIGHT) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_IV_SUMMER) && timeData.season == World.Season.SUMMER) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_IV_SPRING) && timeData.season == World.Season.SPRING))
                {
                    boost = 2;
                }
                else if (player.HasEffect(AppliedEffects.BUG_CATCHING_III) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_III_SPRING) && timeData.season == World.Season.SPRING) ||
                    (player.HasEffect(AppliedEffects.BUG_CATCHING_III_SUMMER) && timeData.season == World.Season.SUMMER))
                {
                    boost = Util.RandInt(1, 2);
                }
                else if (player.HasEffect(AppliedEffects.BUG_CATCHING_II) ||
                     (player.HasEffect(AppliedEffects.BUG_CATCHING_II_SUMMER) && timeData.season == World.Season.SUMMER))
                {
                    boost = 1;
                }
                else if (player.HasEffect(AppliedEffects.BUG_CATCHING_I) ||
                     (player.HasEffect(AppliedEffects.BUG_CATCHING_I_SPRING) && timeData.season == World.Season.SPRING) ||
                     (player.HasEffect(AppliedEffects.BUG_CATCHING_I_SUMMER) && timeData.season == World.Season.SUMMER) ||
                     (player.HasEffect(AppliedEffects.BUG_CATCHING_I_AUTUMN) && timeData.season == World.Season.AUTUMN))
                {
                    boost = Util.RandInt(0, 1);
                }


                int numRolls = Util.RandInt(minRolls, maxRolls) + boost;

                while (loot.Count < numRolls)
                {
                    LootEntry entry = table[Util.RandInt(0, table.Count - 1)];
                    if (entry.IsPossibleDrop(timeData, area, player))
                    {
                        loot.Add(entry.item);
                    }
                }

                PerformLuckRerolls(player, timeData, area, loot, table);

                return loot;
            }
        }


        public static void PerformLuckRerolls(EntityPlayer player, World.TimeData timeData, Area area, List<Item> loot, List<LootEntry> table)
        {
            if(loot.Count == 0)
            {
                return;
            }  

            int rerolls = 0;

            if (player.HasEffect(AppliedEffects.LUCK_VI))
            {
                rerolls = 6;
            }
            else if (player.HasEffect(AppliedEffects.LUCK_V))
            {
                rerolls = 5;
            }
            else if (player.HasEffect(AppliedEffects.LUCK_IV))
            {
                rerolls = 4;
            }
            else if (player.HasEffect(AppliedEffects.LUCK_III))
            {
                rerolls = 3;
            }
            else if (player.HasEffect(AppliedEffects.LUCK_II))
            {
                rerolls = 2;
            }
            else if (player.HasEffect(AppliedEffects.LUCK_I))
            {
                rerolls = 1;
            }

            if (player.HasEffect(AppliedEffects.WISHBOAT_FORTUNE)) //stacks with luck
            {
                rerolls += Util.RandInt(1, 2);
            }

            for (int rerollAttempt = 0; rerollAttempt < rerolls; rerollAttempt++)
            {
                //find the cheapest item
                int lowestValue = Int32.MaxValue, lowestValueIndex = 0;
                for (int i = 0; i < loot.Count; i++)
                {
                    Item item = loot[i];
                    if (item.GetValue() < lowestValue)
                    {
                        lowestValueIndex = i;
                        lowestValue = item.GetValue();
                    }
                }

                System.Diagnostics.Debug.WriteLine("[LootTables] Luck - Rerolling a : " + loot[lowestValueIndex].GetName());

                bool validReroll = false;
                Item rerolledInto = ItemDict.NONE;

                while (!validReroll)
                {
                    LootEntry roll = table[Util.RandInt(0, table.Count - 1)];
                    if(roll.IsPossibleDrop(timeData, area, player))
                    {
                        validReroll = true;
                        rerolledInto = roll.item;
                    }
                }


                if (rerolledInto.GetValue() > loot[lowestValueIndex].GetValue())
                {
                    loot[lowestValueIndex] = rerolledInto; //perform the reroll
                }
            }

            //blessed - rerolls lowest value item into highest value
            if(player.HasEffect(AppliedEffects.BLESSED))
            {
                int lowestValue = Int32.MaxValue, lowestValueIndex = 0;
                for (int i = 0; i < loot.Count; i++)
                {
                    Item item = loot[i];
                    if (item.GetValue() < lowestValue)
                    {
                        lowestValueIndex = i;
                        lowestValue = item.GetValue();
                    }
                }

                int highestValue = Int32.MinValue, highestValueIndex = 0;
                for (int i = 0; i < table.Count; i++)
                {
                    LootEntry entry = table[i];
                    if(entry.IsPossibleDrop(timeData, area, player))
                    {
                        if (entry.item.GetValue() > highestValue)
                        {
                            highestValueIndex = i;
                            highestValue = entry.item.GetValue();
                        }
                    }
                }

                if (highestValue >= 500)
                {
                    System.Diagnostics.Debug.WriteLine("[LootTables] Blessed - Rerolling a : " + loot[lowestValueIndex].GetName() + " into " + table[highestValueIndex].item.GetName());
                    loot[lowestValueIndex] = table[highestValueIndex].item;
                    player.RemoveEffect(AppliedEffects.BLESSED);
                } else
                {
                    System.Diagnostics.Debug.WriteLine("[LootTables] Blessed - nothing value to reroll into.");
                }
            }
        }


        //ForageLootTable
        public static LootTable BEACH_FORAGE, SHELL, RED_GINGER;
        public static LootTable WEEDS, CHICKWEED, SUNFLOWER, NETTLES, BLUEBELL, MARIGOLD, LAVENDER, FALL_LEAF_PILE, WINTER_SNOW_PILE;
        public static LootTable MOREL_FORAGE, MOUNTAIN_WHEAT_FORAGE, SPICY_LEAF_FORAGE, BAMBOO, CAVE_SOYBEAN_FORAGE, EMERALD_MOSS_FORAGE, CAVE_FUNGI_FORAGE, 
            VANILLA_BEAN_FORAGE, CACAO_FORAGE, MAIZE_FORAGE, PINEAPPLE_FORAGE, SHIITAKE_FORAGE, SKY_ROSE_FORAGE;
        public static LootTable CHERRY, APPLE, ORANGE, OLIVE, LEMON, BANANA, COCONUT, WILD_CHERRY, WILD_APPLE, WILD_ORANGE, WILD_OLIVE, WILD_LEMON, WILD_BANANA, WILD_COCONUT;
        public static LootTable RASPBERRY, ELDERBERRY, BLUEBERRY, BLACKBERRY;
        public static LootTable STUMP_SPRING, STUMP_SUMMER, STUMP_AUTUMN, STUMP_WINTER, SANDCASTLE, SANDCASTLE_RED;
        public static LootTable SNOW;

        //MiningLootTable
        public static LootTable ROCK, FARM_BIG_ROCK;
        public static LootTable IRON_ROCK, GOLD_ROCK, MYTHRIL_ROCK, ADAMANTITE_ROCK, COAL_ROCK, SALT_ROCK, WIND_CRYSTAL, EARTH_CRYSTAL, FIRE_CRYSTAL, WATER_CRYSTAL;
        public static LootTable FOSSIL_ROCK, LAVA_GOLD_ROCK, LAVA_ROCK;
        public static LootTable POTTERY, MYTHRIL_MACHINE, STALAGMITE_STALACTITE;
        public static LootTable GEM_ROCK;

        //ChoppingLootTable
        public static LootTable TREE_SHAKE, PALM_SHAKE;
        public static LootTable FARM_BRANCH, FARM_BIG_BRANCH;
        public static LootTable CRATE, MINECART, CRATE_PILE;
        public static LootTable TREE_PINE, TREE_FRUIT, TREE_THIN, TREE_PALM;
        public static LootTable BUSH;
        public static LootTable BAMBOO_POT;

        //FishingLootTable
        public static LootTable FISH_OCEAN;

        //GatheringLootTable
        public static LootTable COW, PIG, SHEEP, CHICKEN, BOAR_TRAP; 

        //ChestLootTable
        public static LootTable ANCIENT_CHEST, IGNEOUS_CHEST, CRYSTAL_CHEST, METAMORPHIC_CHEST, SEDIMENTARY_CHEST;

        //LootTable
        public static LootTable HOEING;
        public static LootTable DYNAMITE, PAINTCANS;
        public static LootTable TRASHCAN, FILING_CABINET, VENDING_MACHINE, SCI_TABLE1, SCI_TABLE2;

        //InsectLootTable
        public static LootTable INSECT, BEE, INSECT_GRASS;

        public static void Initialize()
        {
            TREE_SHAKE = new LootTable(1, 1, new LootEntry(ItemDict.HONEYCOMB, 1), new LootEntry(ItemDict.BIRDS_NEST, 1), new LootEntry(ItemDict.MOSSY_BARK, 1), new LootEntry(ItemDict.OYSTER_MUSHROOM, 1));
            PALM_SHAKE = new LootTable(1, 1, new LootEntry(ItemDict.HONEYCOMB, 1), new LootEntry(ItemDict.BIRDS_NEST, 1), new LootEntry(ItemDict.HONEY_BEE, 1));
            INSECT = new InsectLootTable(1, 1,
                new LootEntry(ItemDict.STAG_BEETLE, 1, LootEntry.notS1Only, LootEntry.notS2Only, LootEntry.notS3Only, LootEntry.notS4Only),
                new LootEntry(ItemDict.HONEY_BEE, 10, LootEntry.notS2Only, LootEntry.notNightOnly),
                new LootEntry(ItemDict.HONEY_BEE, 16, LootEntry.notS2Only, LootEntry.sunnyOnly, LootEntry.notNightOnly),
                new LootEntry(ItemDict.BROWN_CICADA, 5, LootEntry.summerOnly, LootEntry.notS2Only),
                new LootEntry(ItemDict.BROWN_CICADA, 25, LootEntry.summerOnly, LootEntry.nightOnly, LootEntry.notS2Only),
                new LootEntry(ItemDict.LANTERN_MOTH, 24, LootEntry.nightOnly, LootEntry.notS2Only),
                new LootEntry(ItemDict.FIREFLY, 12, LootEntry.nightOnly, LootEntry.summerOnly, LootEntry.notS2Only),
                new LootEntry(ItemDict.SOLDIER_ANT, 6, LootEntry.autumnOnly, LootEntry.notS2Only),
                new LootEntry(ItemDict.YELLOW_BUTTERFLY, 8, LootEntry.farmOnly),
                new LootEntry(ItemDict.BANDED_DRAGONFLY, 8, LootEntry.s0Only),
                new LootEntry(ItemDict.PINK_LADYBUG, 5, LootEntry.s1Only),
                new LootEntry(ItemDict.CAVEWORM, 49, LootEntry.s2Only),
                new LootEntry(ItemDict.JEWEL_SPIDER, 1, LootEntry.s2Only),
                new LootEntry(ItemDict.STINGER_HORNET, 4, LootEntry.s3Only),
                new LootEntry(ItemDict.PINK_LADYBUG, 10, LootEntry.s4Only),
                new LootEntry(ItemDict.EMPRESS_BUTTERFLY, 1, LootEntry.s4Only));
            INSECT_GRASS = new InsectLootTable(1, 1,
                new LootEntry(ItemDict.CAVEWORM, 49, LootEntry.s2Only),
                new LootEntry(ItemDict.JEWEL_SPIDER, 1, LootEntry.s2Only),
                new LootEntry(ItemDict.SNAIL, 8, LootEntry.notS2Only),
                new LootEntry(ItemDict.SOLDIER_ANT, 18, LootEntry.autumnOnly, LootEntry.notS2Only),
                new LootEntry(ItemDict.RICE_GRASSHOPPER, 24, LootEntry.notS2Only));
            SNOW = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.SNOW_CRYSTAL, 999), new LootEntry(ItemDict.ICE_NINE, 1));
            BEE = new InsectLootTable(1, 1, new LootEntry(ItemDict.HONEY_BEE, 10, LootEntry.notS2Only), new LootEntry(ItemDict.STINGER_HORNET, 3, LootEntry.s3Only));
            BEACH_FORAGE = new ForageLootTable(1, 1, ForageLootTable.ForageType.BEACH, new LootEntry(ItemDict.SEAWEED, 30), new LootEntry(ItemDict.SEA_URCHIN, 22), new LootEntry(ItemDict.CRIMSON_CORAL, 18), new LootEntry(ItemDict.FLAWLESS_CONCH, 1), new LootEntry(ItemDict.SALT_SHARDS, 4), new LootEntry(ItemDict.WOOD, 6));
            WEEDS = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.WEEDS, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            BLUEBELL = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, 0.5f, BEE, new LootEntry(ItemDict.BLUEBELL, 299), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            NETTLES = new ForageLootTable(2, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.NETTLES, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            CHICKWEED = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.CHICKWEED, 299), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            SUNFLOWER = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, 0.5f, BEE, new LootEntry(ItemDict.SUNFLOWER, 299), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            MARIGOLD = new ForageLootTable(2, 2, ForageLootTable.ForageType.NORMAL, 0.5f, BEE, new LootEntry(ItemDict.MARIGOLD, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            LAVENDER = new ForageLootTable(2, 3, ForageLootTable.ForageType.NORMAL, 0.5f, BEE, new LootEntry(ItemDict.LAVENDER, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            FALL_LEAF_PILE = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, 0.5f, INSECT, new LootEntry(ItemDict.WILD_RICE, 100), new LootEntry(ItemDict.PERSIMMON, 100), new LootEntry(ItemDict.SASSAFRAS, 99), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WINTER_SNOW_PILE = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.SNOW_CRYSTAL, 99), new LootEntry(ItemDict.WINTERGREEN, 50), new LootEntry(ItemDict.CHICORY_ROOT, 50), new LootEntry(ItemDict.CHANTERELLE, 50), new LootEntry(ItemDict.SNOWDROP, 50), new LootEntry(ItemDict.CRYSTAL_KEY, 1));
            MOREL_FORAGE = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.MOREL, 299), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            MOUNTAIN_WHEAT_FORAGE = new ForageLootTable(2, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.MOUNTAIN_WHEAT, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            SPICY_LEAF_FORAGE = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.SPICY_LEAF, 299), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            VANILLA_BEAN_FORAGE = new ForageLootTable(2, 5, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.VANILLA_BEAN, 499), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            CACAO_FORAGE = new ForageLootTable(3, 5, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.CACAO_BEAN, 499), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            MAIZE_FORAGE = new ForageLootTable(2, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.MAIZE, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            PINEAPPLE_FORAGE = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.PINEAPPLE, 199), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            CAVE_FUNGI_FORAGE = new ForageLootTable(2, 3, ForageLootTable.ForageType.NORMAL, 0.33f, INSECT, new LootEntry(ItemDict.CAVE_FUNGI, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            CAVE_SOYBEAN_FORAGE = new ForageLootTable(1, 3, ForageLootTable.ForageType.NORMAL, 0.33f, INSECT, new LootEntry(ItemDict.CAVE_SOYBEAN, 399), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            EMERALD_MOSS_FORAGE = new ForageLootTable(3, 3, ForageLootTable.ForageType.NORMAL, 0.33f, INSECT, new LootEntry(ItemDict.EMERALD_MOSS, 499), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            SHIITAKE_FORAGE = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.SHIITAKE, 99), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            SKY_ROSE_FORAGE = new ForageLootTable(1, 1, ForageLootTable.ForageType.NORMAL, 0.5f, BEE, new LootEntry(ItemDict.SKY_ROSE, 99), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            ROCK = new MiningLootTable(1, 2, new LootEntry(ItemDict.STONE, 119), new LootEntry(ItemDict.SCRAP_IRON, 18), new LootEntry(ItemDict.IRON_ORE, 6), new LootEntry(ItemDict.TOPAZ, 6), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            FARM_BIG_ROCK = new MiningLootTable(2, 3, new LootEntry(ItemDict.STONE, 280), new LootEntry(ItemDict.SCRAP_IRON, 40), new LootEntry(ItemDict.IRON_ORE, 24), new LootEntry(ItemDict.TOPAZ, 12), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            FARM_BRANCH = new ChoppingLootTable(1, 2, new LootEntry(ItemDict.WOOD, 300), new LootEntry(ItemDict.HARDWOOD, 12), new LootEntry(ItemDict.METAMORPHIC_KEY, 1));
            FARM_BIG_BRANCH = new ChoppingLootTable(2, 3, new LootEntry(ItemDict.WOOD, 300), new LootEntry(ItemDict.HARDWOOD, 100), new LootEntry(ItemDict.METAMORPHIC_KEY, 1));
            BAMBOO = new ChoppingLootTable(2, 4, new LootEntry(ItemDict.BAMBOO, 400), new LootEntry(ItemDict.PINK_LADYBUG, 8), new LootEntry(ItemDict.BIRDS_NEST, 14), new LootEntry(ItemDict.METAMORPHIC_KEY, 1));
            TREE_PINE = new TreeLootTable(10, 14, new LootEntry(ItemDict.WOOD, 450), new LootEntry(ItemDict.PINECONE, 50), new LootEntry(ItemDict.FAIRY_DUST, 5), new LootEntry(ItemDict.METAMORPHIC_KEY, 1));
            TREE_FRUIT = new TreeLootTable(8, 11, new LootEntry(ItemDict.WOOD, 150), new LootEntry(ItemDict.HARDWOOD, 350), new LootEntry(ItemDict.GOLDEN_LEAF, 5), new LootEntry(ItemDict.METAMORPHIC_KEY, 1));
            TREE_THIN = new LootTable(4, 6, new LootEntry(ItemDict.WOOD, 300), new LootEntry(ItemDict.HARDWOOD, 100), new LootEntry(ItemDict.GOLDEN_LEAF, 25), new LootEntry(ItemDict.METAMORPHIC_KEY, 1));
            TREE_PALM = new LootTable(6, 9, new LootEntry(ItemDict.WOOD, 400), new LootEntry(ItemDict.FAIRY_DUST, 15), new LootEntry(ItemDict.METAMORPHIC_KEY, 1));
            CHERRY = new ForageLootTable(3, 4, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.CHERRY, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WILD_CHERRY = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.CHERRY, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            APPLE = new ForageLootTable(2, 4, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.APPLE, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WILD_APPLE = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.APPLE, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            LEMON = new ForageLootTable(3, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.LEMON, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WILD_LEMON = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.LEMON, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            OLIVE = new ForageLootTable(4, 6, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.OLIVE, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WILD_OLIVE = new ForageLootTable(2, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.OLIVE, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            ORANGE = new ForageLootTable(2, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.ORANGE, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WILD_ORANGE = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.ORANGE, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            BANANA = new ForageLootTable(3, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.BANANA, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WILD_BANANA = new ForageLootTable(2, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.BANANA, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            COCONUT = new LootTable(2, 5, new LootEntry(ItemDict.COCONUT, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WILD_COCONUT = new LootTable(1, 3, new LootEntry(ItemDict.COCONUT, 149), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            BUSH = new ForageLootTable(3, 4, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.WOOD, 250), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            BLACKBERRY = new ForageLootTable(3, 5, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.BLACKBERRY, 599), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            ELDERBERRY = new ForageLootTable(2, 5, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.ELDERBERRY, 599), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            BLUEBERRY = new ForageLootTable(4, 6, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.BLUEBERRY, 599), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            RASPBERRY = new ForageLootTable(3, 4, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.RASPBERRY, 599), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            FISH_OCEAN = new FishingLootTable(1, 1, FishingLootTable.FishType.OCEAN, new LootEntry(ItemDict.SARDINE, 20), new LootEntry(ItemDict.HERRING, 18), new LootEntry(ItemDict.WOOD, 4), new LootEntry(ItemDict.SEAWEED, 5),
                new LootEntry(ItemDict.STRIPED_BASS, 9), new LootEntry(ItemDict.SARDINE, 7), new LootEntry(ItemDict.INKY_SQUID, 8), new LootEntry(ItemDict.CRIMSON_CORAL, 3),
                new LootEntry(ItemDict.CRAB, 15), new LootEntry(ItemDict.SWORDFISH, 4), new LootEntry(ItemDict.GREAT_WHITE_SHARK, 1), new LootEntry(ItemDict.CRYSTAL_KEY, 1), new LootEntry(ItemDict.ANCIENT_KEY, 5));
            COW = new GatheringLootTable(1, 1, GatheringLootTable.GatheringType.COW, new LootEntry(ItemDict.MILK, 1));
            PIG = new GatheringLootTable(1, 1, GatheringLootTable.GatheringType.PIG, new LootEntry(ItemDict.TRUFFLE, 1), new LootEntry(ItemDict.MOREL, 2));
            CHICKEN = new GatheringLootTable(1, 1, GatheringLootTable.GatheringType.CHICKEN, new LootEntry(ItemDict.EGG, 99), new LootEntry(ItemDict.GOLDEN_EGG, 1));
            SHEEP = new GatheringLootTable(1, 1, GatheringLootTable.GatheringType.SHEEP, new LootEntry(ItemDict.WOOL, 99), new LootEntry(ItemDict.GOLDEN_WOOL, 1));
            CRATE = new ChoppingLootTable(2, 4, new LootEntry(ItemDict.WOOD, 40), new LootEntry(ItemDict.STONE, 24), new LootEntry(ItemDict.COAL, 16), new LootEntry(ItemDict.IRON_ORE, 10), new LootEntry(ItemDict.CLAY, 4), new LootEntry(ItemDict.IRON_BAR, 4), new LootEntry(ItemDict.ANCIENT_KEY, 1));
            CRATE_PILE = new ChoppingLootTable(5, 8, new LootEntry(ItemDict.WOOD, 52), new LootEntry(ItemDict.HARDWOOD, 6), new LootEntry(ItemDict.STONE, 25), new LootEntry(ItemDict.COAL, 18), new LootEntry(ItemDict.CLAY, 4), new LootEntry(ItemDict.IRON_ORE, 8), new LootEntry(ItemDict.IRON_BAR, 4), new LootEntry(ItemDict.ANCIENT_KEY, 1));
            MINECART = new ForageLootTable(3, 6, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.COAL, 43), new LootEntry(ItemDict.STONE, 3), new LootEntry(ItemDict.IRON_ORE, 3), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            TRASHCAN = new LootTable(1, 3, new LootEntry(ItemDict.HONEY_BEE, 3), new LootEntry(ItemDict.BOARD, 2), new LootEntry(ItemDict.BRICKS, 1),
                new LootEntry(ItemDict.EARTHWORM, 14), new LootEntry(ItemDict.CARP, 1), new LootEntry(ItemDict.THICK_COMPOST, 3),
                new LootEntry(ItemDict.STONE, 5), new LootEntry(ItemDict.LUCKY_COIN, 1), new LootEntry(ItemDict.QUALITY_COMPOST, 1), new LootEntry(ItemDict.FRIED_FISH, 1), 
                new LootEntry(ItemDict.WEEDS, 18), new LootEntry(ItemDict.BLACK_FEATHER, 3), new LootEntry(ItemDict.WHITE_FEATHER, 3), new LootEntry(ItemDict.CLAY, 16), 
                new LootEntry(ItemDict.LOAMY_COMPOST, 15), new LootEntry(ItemDict.TIGHTIES, 1), new LootEntry(ItemDict.ANCIENT_KEY, 2));
            IRON_ROCK = new MiningLootTable(2, 3, new LootEntry(ItemDict.IRON_ORE, 180), new LootEntry(ItemDict.SCRAP_IRON, 75), new LootEntry(ItemDict.AMETHYST, 15), new LootEntry(ItemDict.STONE, 50), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            MYTHRIL_ROCK = new MiningLootTable(2, 2, new LootEntry(ItemDict.MYTHRIL_ORE, 270), new LootEntry(ItemDict.SAPPHIRE, 15), new LootEntry(ItemDict.STONE, 50), new LootEntry(ItemDict.AQUAMARINE, 15), new LootEntry(ItemDict.IGNEOUS_KEY, 1), new LootEntry(ItemDict.CRYSTAL_KEY, 1)); 
            GOLD_ROCK = new MiningLootTable(2, 3, new LootEntry(ItemDict.GOLD_ORE, 180), new LootEntry(ItemDict.TOPAZ, 15), new LootEntry(ItemDict.STONE, 40), new LootEntry(ItemDict.IGNEOUS_KEY, 1)); 
            ADAMANTITE_ROCK = new MiningLootTable(2, 2, new LootEntry(ItemDict.ADAMANTITE_ORE, 100), new LootEntry(ItemDict.EMERALD, 5), new LootEntry(ItemDict.DIAMOND, 5), new LootEntry(ItemDict.CRYSTAL_KEY, 3)); 
            COAL_ROCK = new MiningLootTable(3, 4, new LootEntry(ItemDict.COAL, 300), new LootEntry(ItemDict.FIRE_CRYSTAL, 3), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            DYNAMITE = new LootTable(5, 8, new LootEntry(ItemDict.CLAY, 1), new LootEntry(ItemDict.STONE, 1), new LootEntry(ItemDict.IRON_ORE, 1), new LootEntry(ItemDict.COAL, 1), new LootEntry(ItemDict.SCRAP_IRON, 1));
            SALT_ROCK = new MiningLootTable(3, 4, new LootEntry(ItemDict.SALT_SHARDS, 499), new LootEntry(ItemDict.WATER_CRYSTAL, 1));
            SHELL = new ForageLootTable(1, 1, ForageLootTable.ForageType.BEACH, new LootEntry(ItemDict.CLAM, 30), new LootEntry(ItemDict.OYSTER, 20), new LootEntry(ItemDict.FLAWLESS_CONCH, 3), new LootEntry(ItemDict.PEARL, 1));
            RED_GINGER = new ForageLootTable(1, 1, ForageLootTable.ForageType.BEACH, 0.5f, BEE, new LootEntry(ItemDict.RED_GINGER, 299), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            PAINTCANS = new LootTable(1, 2, new LootEntry(ItemDict.BLUE_DYE, 1), new LootEntry(ItemDict.NAVY_DYE, 1), new LootEntry(ItemDict.RED_DYE, 1), new LootEntry(ItemDict.LIGHT_BROWN_DYE, 1), new LootEntry(ItemDict.DARK_BROWN_DYE, 1), new LootEntry(ItemDict.ORANGE_DYE, 1), new LootEntry(ItemDict.OLIVE_DYE, 1), new LootEntry(ItemDict.YELLOW_DYE, 1), new LootEntry(ItemDict.GREEN_DYE, 1));
            POTTERY = new MiningLootTable(1, 2, new LootEntry(ItemDict.CLAY, 25), new LootEntry(ItemDict.CHERRY_JELLY, 1), new LootEntry(ItemDict.CAVE_FUNGI, 1), new LootEntry(ItemDict.SHIITAKE, 1), 
                new LootEntry(ItemDict.MOREL, 3), new LootEntry(ItemDict.WOOD, 15), new LootEntry(ItemDict.STONE, 15),new LootEntry(ItemDict.PAPER, 5), new LootEntry(ItemDict.HARDWOOD, 5), 
                new LootEntry(ItemDict.BAMBOO, 15), new LootEntry(ItemDict.ANCIENT_KEY, 9), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            WIND_CRYSTAL = new MiningLootTable(2, 3, new LootEntry(ItemDict.WIND_CRYSTAL, 47), new LootEntry(ItemDict.DIAMOND, 2), new LootEntry(ItemDict.CRYSTAL_KEY, 1));
            WATER_CRYSTAL = new MiningLootTable(2, 3, new LootEntry(ItemDict.WATER_CRYSTAL, 45), new LootEntry(ItemDict.SAPPHIRE, 4), new LootEntry(ItemDict.CRYSTAL_KEY, 1));
            EARTH_CRYSTAL = new MiningLootTable(2, 3, new LootEntry(ItemDict.EARTH_CRYSTAL, 45), new LootEntry(ItemDict.EMERALD, 4), new LootEntry(ItemDict.CRYSTAL_KEY, 1));
            FIRE_CRYSTAL = new MiningLootTable(2, 3, new LootEntry(ItemDict.FIRE_CRYSTAL, 45), new LootEntry(ItemDict.RUBY, 4), new LootEntry(ItemDict.CRYSTAL_KEY, 1));
            STUMP_SPRING = new ForageLootTable(1, 3,ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.WEEDS, 30), new LootEntry(ItemDict.BLUEBELL, 10), new LootEntry(ItemDict.NETTLES, 10), new LootEntry(ItemDict.CHICKWEED, 10), new LootEntry(ItemDict.RICE_GRASSHOPPER, 10), new LootEntry(ItemDict.HONEYCOMB, 10), new LootEntry(ItemDict.MOSSY_BARK, 10), new LootEntry(ItemDict.WOOD, 10), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1), new LootEntry(ItemDict.ANCIENT_KEY, 1));
            STUMP_SUMMER = new ForageLootTable(2, 4, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.WEEDS, 30), new LootEntry(ItemDict.MARIGOLD, 10), new LootEntry(ItemDict.LAVENDER, 10), new LootEntry(ItemDict.SUNFLOWER, 10), new LootEntry(ItemDict.BROWN_CICADA, 20), new LootEntry(ItemDict.BIRDS_NEST, 10), new LootEntry(ItemDict.MOSSY_BARK, 10), new LootEntry(ItemDict.ANCIENT_KEY, 1), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            STUMP_AUTUMN = new ForageLootTable(1, 3, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.WEEDS, 10), new LootEntry(ItemDict.WILD_RICE, 20), new LootEntry(ItemDict.PERSIMMON, 20), new LootEntry(ItemDict.SASSAFRAS, 20), new LootEntry(ItemDict.SOLDIER_ANT, 40), new LootEntry(ItemDict.MOSSY_BARK, 10), new LootEntry(ItemDict.HARDWOOD, 20), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1), new LootEntry(ItemDict.ANCIENT_KEY, 1));
            STUMP_WINTER = new ForageLootTable(1, 2, ForageLootTable.ForageType.NORMAL, new LootEntry(ItemDict.SNOW_CRYSTAL, 30), new LootEntry(ItemDict.SNOWDROP, 10), new LootEntry(ItemDict.WINTERGREEN, 10), new LootEntry(ItemDict.CHICORY_ROOT, 10), new LootEntry(ItemDict.CHANTERELLE, 10), new LootEntry(ItemDict.HARDWOOD, 10), new LootEntry(ItemDict.COAL, 5), new LootEntry(ItemDict.CRYSTAL_KEY, 1));
            BOAR_TRAP = new GatheringLootTable(2, 3, GatheringLootTable.GatheringType.BOAR, new LootEntry(ItemDict.WILD_MEAT, 5), new LootEntry(ItemDict.BOAR_HIDE, 1));
            SANDCASTLE = new ForageLootTable(3, 5, ForageLootTable.ForageType.BEACH, new LootEntry(ItemDict.SALT_SHARDS, 16), new LootEntry(ItemDict.WHITE_FEATHER, 6), new LootEntry(ItemDict.WOOD, 8), new LootEntry(ItemDict.SEAWEED, 10), new LootEntry(ItemDict.CLAM, 10), new LootEntry(ItemDict.OYSTER, 6), new LootEntry(ItemDict.CRIMSON_CORAL, 10), new LootEntry(ItemDict.FLAWLESS_CONCH, 2), new LootEntry(ItemDict.CRAB, 2), new LootEntry(ItemDict.ANCIENT_KEY, 1));
            SANDCASTLE_RED = new ForageLootTable(4, 6, ForageLootTable.ForageType.BEACH, new LootEntry(ItemDict.SALT_SHARDS, 10), new LootEntry(ItemDict.RED_FEATHER, 6), new LootEntry(ItemDict.WOOD, 8), new LootEntry(ItemDict.WEEDS, 8), new LootEntry(ItemDict.VANILLA_BEAN, 6), new LootEntry(ItemDict.CACAO_BEAN, 6), new LootEntry(ItemDict.CRIMSON_CORAL, 10), new LootEntry(ItemDict.TRILOBITE, 8), new LootEntry(ItemDict.CRAB, 2), new LootEntry(ItemDict.STINGER_HORNET, 4), new LootEntry(ItemDict.SOLDIER_ANT, 8), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            BAMBOO_POT = new ChoppingLootTable(1, 2, new LootEntry(ItemDict.BAMBOO, 290), new LootEntry(ItemDict.PINK_LADYBUG, 9), new LootEntry(ItemDict.ANCIENT_KEY, 1));
            ANCIENT_CHEST = new ChestLootTable(4, 7, 2, new LootEntry(ItemDict.STONE, 5), new LootEntry(ItemDict.WOOD, 5), new LootEntry(ItemDict.HARDWOOD, 5), new LootEntry(ItemDict.CLAY, 5), new LootEntry(ItemDict.BAMBOO, 5),
                new LootEntry(ItemDict.BERRY_BUSH_PLANTER, 2), new LootEntry(ItemDict.DEW_COMPOST, 2), new LootEntry(ItemDict.THICK_COMPOST, 2), new LootEntry(ItemDict.SHINING_COMPOST, 2), new LootEntry(ItemDict.SWEET_COMPOST, 2),
                new LootEntry(ItemDict.SPINACH_SEEDS, 1), new LootEntry(ItemDict.POTATO_SEEDS, 1), new LootEntry(ItemDict.STRAWBERRY_SEEDS, 1), new LootEntry(ItemDict.CARROT_SEEDS, 1),
                new LootEntry(ItemDict.ONION_SEEDS, 1), new LootEntry(ItemDict.CUCUMBER_SEEDS, 1), new LootEntry(ItemDict.CACTUS_SEEDS, 1), new LootEntry(ItemDict.EGGPLANT_SEEDS, 1), new LootEntry(ItemDict.COTTON_SEEDS, 1),
                new LootEntry(ItemDict.TOMATO_SEEDS, 1), new LootEntry(ItemDict.WATERMELON_SEEDS, 1), new LootEntry(ItemDict.BEET_SEEDS, 1), new LootEntry(ItemDict.BELLPEPPER_SEEDS, 1), new LootEntry(ItemDict.BROCCOLI_SEEDS, 1),
                new LootEntry(ItemDict.FLAX_SEEDS, 1), new LootEntry(ItemDict.CABBAGE_SEEDS, 1), new LootEntry(ItemDict.PUMPKIN_SEEDS, 1), new LootEntry(ItemDict.COTTON_CLOTH, 1), new LootEntry(ItemDict.LINEN_CLOTH, 1), new LootEntry(ItemDict.WOOLEN_CLOTH, 1),
                new LootEntry(ItemDict.MOSSY_BARK, 1), new LootEntry(ItemDict.HONEYCOMB, 1), new LootEntry(ItemDict.BIRDS_NEST, 1), new LootEntry(ItemDict.BOARD, 1), new LootEntry(ItemDict.PLANK, 2),
                new LootEntry(ItemDict.BRICKS, 2), new LootEntry(ItemDict.GEARS, 2), new LootEntry(ItemDict.PAPER, 2), new LootEntry(ItemDict.COAL, 5), new LootEntry(ItemDict.IRON_BAR, 5),
                new LootEntry(ItemDict.WEEDS, 2), new LootEntry(ItemDict.SKELETON_KEY, 1), new LootEntry(ItemDict.BAT_WING, 1), new LootEntry(ItemDict.BLACK_FEATHER, 3), new LootEntry(ItemDict.BOAR_HIDE, 5),
                new LootEntry(ItemDict.BREAKFAST_POTATOES, 1), new LootEntry(ItemDict.FRIED_EGG, 1), new LootEntry(ItemDict.EGG_SCRAMBLE, 1), new LootEntry(ItemDict.POTATO_AND_BEET_FRIES, 1),
                new LootEntry(ItemDict.BAKED_POTATO, 1), new LootEntry(ItemDict.MOUNTAIN_WHEAT, 1), new LootEntry(ItemDict.MOUNTAIN_BREAD, 1), new LootEntry(ItemDict.ESCARGOT, 1), new LootEntry(ItemDict.CRISPY_GRASSHOPPER, 1),
                new LootEntry(ItemDict.SEARED_TUNA, 1), new LootEntry(ItemDict.SUSHI_ROLL, 1), new LootEntry(ItemDict.SAILCLOTH, 1), new LootEntry(ItemDict.TORCH, 1),
                new LootEntry(ItemDict.BUTTERFLY_CHARM, 1), new LootEntry(ItemDict.SNOUT_CHARM, 1), new LootEntry(ItemDict.SUNFLOWER_CHARM, 1), new LootEntry(ItemDict.MANTLE_CHARM, 1), new LootEntry(ItemDict.FLUFFY_BRACER, 1),
                new LootEntry(ItemDict.FROZEN_CREST, 1), new LootEntry(ItemDict.MUTATING_CREST, 1), new LootEntry(ItemDict.NEUTRALIZED_PENDANT, 1), new LootEntry(ItemDict.EROSION_PENDANT, 1), new LootEntry(ItemDict.LADYBUG_PENDANT, 1));
            CRYSTAL_CHEST = new ChestLootTable(2, 4, 3, new LootEntry(ItemDict.WATER_CRYSTAL, 1), new LootEntry(ItemDict.ADAMANTITE_ORE, 1), new LootEntry(ItemDict.ADAMANTITE_BAR, 1),
                new LootEntry(ItemDict.GOLD_BAR, 1), new LootEntry(ItemDict.MYTHRIL_BAR, 1), new LootEntry(ItemDict.AMETHYST, 1), new LootEntry(ItemDict.AQUAMARINE, 1), new LootEntry(ItemDict.DIAMOND, 1),
                new LootEntry(ItemDict.EARTH_CRYSTAL, 1), new LootEntry(ItemDict.EMERALD, 1), new LootEntry(ItemDict.FIRE_CRYSTAL, 1), new LootEntry(ItemDict.RUBY, 1), new LootEntry(ItemDict.SALT_SHARDS, 1),
                new LootEntry(ItemDict.SAPPHIRE, 1), new LootEntry(ItemDict.WATER_CRYSTAL, 1), new LootEntry(ItemDict.WIND_CRYSTAL, 1), new LootEntry(ItemDict.OPAL, 1), new LootEntry(ItemDict.TOPAZ, 1),
                new LootEntry(ItemDict.PRIMORDIAL_SHELL, 1), new LootEntry(ItemDict.ICE_NINE, 1), new LootEntry(ItemDict.GOLDEN_LEAF, 1), new LootEntry(ItemDict.OLD_BONE, 1), new LootEntry(ItemDict.ANCIENT_KEY, 1),
                new LootEntry(ItemDict.CRYSTAL_KEY, 1), new LootEntry(ItemDict.IGNEOUS_KEY, 1), new LootEntry(ItemDict.METAMORPHIC_KEY, 1), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1), new LootEntry(ItemDict.SKELETON_KEY, 1),
                new LootEntry(ItemDict.GOLDEN_EGG, 1), new LootEntry(ItemDict.GOLDEN_WOOL, 1), new LootEntry(ItemDict.TRUFFLE, 1), new LootEntry(ItemDict.PEARL, 1));
            IGNEOUS_CHEST = new ChestLootTable(3, 5, 5, new LootEntry(ItemDict.BLUE_DYE, 1), new LootEntry(ItemDict.NAVY_DYE, 1), new LootEntry(ItemDict.BLACK_DYE, 1), new LootEntry(ItemDict.RED_DYE, 1),
                new LootEntry(ItemDict.PINK_DYE, 1), new LootEntry(ItemDict.LIGHT_BROWN_DYE, 1), new LootEntry(ItemDict.DARK_BROWN_DYE, 1), new LootEntry(ItemDict.ORANGE_DYE, 1), new LootEntry(ItemDict.YELLOW_DYE, 1),
                new LootEntry(ItemDict.PURPLE_DYE, 1), new LootEntry(ItemDict.GREEN_DYE, 1), new LootEntry(ItemDict.OLIVE_DYE, 1), new LootEntry(ItemDict.WHITE_DYE, 1), new LootEntry(ItemDict.LIGHT_GREY_DYE, 1),
                new LootEntry(ItemDict.DARK_GREY_DYE, 1), new LootEntry(ItemDict.UN_DYE, 1), new LootEntry(ItemDict.BLACK_CANDLE, 1), new LootEntry(ItemDict.SALTED_CANDLE, 1), new LootEntry(ItemDict.SOOTHE_CANDLE, 1),
                new LootEntry(ItemDict.SPICED_CANDLE, 1), new LootEntry(ItemDict.SUGAR_CANDLE, 1), new LootEntry(ItemDict.COLD_INCENSE, 1), new LootEntry(ItemDict.FRESH_INCENSE, 1), new LootEntry(ItemDict.IMPERIAL_INCENSE, 1),
                new LootEntry(ItemDict.LAVENDER_INCENSE, 1), new LootEntry(ItemDict.SWEET_INCENSE, 1), new LootEntry(ItemDict.SKY_BOTTLE, 1), new LootEntry(ItemDict.INVINCIROID, 1), new LootEntry(ItemDict.HEART_VESSEL, 1),
                new LootEntry(ItemDict.PHILOSOPHERS_STONE, 1), new LootEntry(ItemDict.MINT_EXTRACT, 1), new LootEntry(ItemDict.VANILLA_EXTRACT, 1), new LootEntry(ItemDict.OIL, 1));
            SEDIMENTARY_CHEST = new ChestLootTable(8, 16, 2, new LootEntry(ItemDict.WEEDS, 5), new LootEntry(ItemDict.CRIMSON_CORAL, 5), new LootEntry(ItemDict.SEA_URCHIN, 5), new LootEntry(ItemDict.SEAWEED, 5),
                new LootEntry(ItemDict.LUCKY_COIN, 5), new LootEntry(ItemDict.RED_GINGER, 5), new LootEntry(ItemDict.PERSIMMON, 5), new LootEntry(ItemDict.BLACKBERRY, 5), new LootEntry(ItemDict.SASSAFRAS, 5),
                new LootEntry(ItemDict.BLUEBELL, 5), new LootEntry(ItemDict.WILD_RICE, 5), new LootEntry(ItemDict.CHICKWEED, 5), new LootEntry(ItemDict.NETTLES, 5), new LootEntry(ItemDict.RASPBERRY, 5),
                new LootEntry(ItemDict.SUNFLOWER, 5), new LootEntry(ItemDict.BLUEBERRY, 5), new LootEntry(ItemDict.ELDERBERRY, 5), new LootEntry(ItemDict.LAVENDER, 5), new LootEntry(ItemDict.MARIGOLD, 5),
                new LootEntry(ItemDict.CHICORY_ROOT, 5), new LootEntry(ItemDict.SNOWDROP, 5), new LootEntry(ItemDict.WINTERGREEN, 5), new LootEntry(ItemDict.CAVE_SOYBEAN, 5), new LootEntry(ItemDict.CACAO_BEAN, 5),
                new LootEntry(ItemDict.EMERALD_MOSS, 5), new LootEntry(ItemDict.MAIZE, 5), new LootEntry(ItemDict.PINEAPPLE, 5), new LootEntry(ItemDict.SPICY_LEAF, 5), new LootEntry(ItemDict.VANILLA_BEAN, 5),
                new LootEntry(ItemDict.BEET, 5), new LootEntry(ItemDict.BELLPEPPER, 5), new LootEntry(ItemDict.BROCCOLI, 5), new LootEntry(ItemDict.CABBAGE, 5), new LootEntry(ItemDict.CACTUS, 5),
                new LootEntry(ItemDict.CARROT, 5), new LootEntry(ItemDict.COTTON, 5), new LootEntry(ItemDict.CUCUMBER, 5), new LootEntry(ItemDict.EGGPLANT, 5), new LootEntry(ItemDict.FLAX, 5),
                new LootEntry(ItemDict.ONION, 5), new LootEntry(ItemDict.POTATO, 5), new LootEntry(ItemDict.PUMPKIN, 5), new LootEntry(ItemDict.SPINACH, 5), new LootEntry(ItemDict.STRAWBERRY, 5),
                new LootEntry(ItemDict.TOMATO, 5), new LootEntry(ItemDict.WATERMELON_SLICE, 5), new LootEntry(ItemDict.APPLE, 5), new LootEntry(ItemDict.BANANA, 5), new LootEntry(ItemDict.CHERRY, 5),
                new LootEntry(ItemDict.COCONUT, 5), new LootEntry(ItemDict.LEMON, 5), new LootEntry(ItemDict.OLIVE, 5), new LootEntry(ItemDict.ORANGE, 5), 
                new LootEntry(ItemDict.GOLDEN_BEET, 1), new LootEntry(ItemDict.GOLDEN_BELLPEPPER, 1), new LootEntry(ItemDict.GOLDEN_BROCCOLI, 1), new LootEntry(ItemDict.GOLDEN_CABBAGE, 1), new LootEntry(ItemDict.GOLDEN_CACTUS, 1),
                new LootEntry(ItemDict.GOLDEN_CARROT, 1), new LootEntry(ItemDict.GOLDEN_COTTON, 1), new LootEntry(ItemDict.GOLDEN_CUCUMBER, 1), new LootEntry(ItemDict.GOLDEN_EGGPLANT, 1), new LootEntry(ItemDict.GOLDEN_FLAX, 1),
                new LootEntry(ItemDict.GOLDEN_ONION, 1), new LootEntry(ItemDict.GOLDEN_POTATO, 1), new LootEntry(ItemDict.GOLDEN_PUMPKIN, 1), new LootEntry(ItemDict.GOLDEN_SPINACH, 1), new LootEntry(ItemDict.GOLDEN_STRAWBERRY, 1),
                new LootEntry(ItemDict.GOLDEN_TOMATO, 1), new LootEntry(ItemDict.GOLDEN_WATERMELON_SLICE, 1), new LootEntry(ItemDict.GOLDEN_APPLE, 1), new LootEntry(ItemDict.GOLDEN_BANANA, 1), new LootEntry(ItemDict.GOLDEN_CHERRY, 1),
                new LootEntry(ItemDict.GOLDEN_COCONUT, 1), new LootEntry(ItemDict.GOLDEN_LEMON, 1), new LootEntry(ItemDict.GOLDEN_OLIVE, 1), new LootEntry(ItemDict.GOLDEN_ORANGE, 1)); 
            METAMORPHIC_CHEST = new ChestLootTable(1, 1, 3, new LootEntry(ItemDict.ANATOMICAL_POSTER, 1), new LootEntry(ItemDict.BAMBOO_POT, 1), new LootEntry(ItemDict.BANNER, 1),
                new LootEntry(ItemDict.BELL, 1), new LootEntry(ItemDict.BLACKBOARD, 1), new LootEntry(ItemDict.BOOMBOX, 1), new LootEntry(ItemDict.BOX, 1), new LootEntry(ItemDict.BRAZIER, 1),
                new LootEntry(ItemDict.BUOY, 1), new LootEntry(ItemDict.CAMPFIRE, 1), new LootEntry(ItemDict.CANVAS, 1), new LootEntry(ItemDict.CART, 1), new LootEntry(ItemDict.CLOCK, 1),
                new LootEntry(ItemDict.CLOTHESLINE, 1), new LootEntry(ItemDict.CRATE, 1), new LootEntry(ItemDict.CUBE_STATUE, 1), new LootEntry(ItemDict.CYMBAL, 1), new LootEntry(ItemDict.DECORATIVE_BOULDER, 1),
                new LootEntry(ItemDict.DECORATIVE_LOG, 1), new LootEntry(ItemDict.DRUM, 1), new LootEntry(ItemDict.FIRE_HYDRANT, 1), new LootEntry(ItemDict.FIREPIT, 1), new LootEntry(ItemDict.FIREPLACE, 1),
                new LootEntry(ItemDict.FLAGPOLE, 1), new LootEntry(ItemDict.FROST_SCULPTURE, 1), new LootEntry(ItemDict.GARDEN_ARCH, 1), new LootEntry(ItemDict.GRANDFATHER_CLOCK, 1), new LootEntry(ItemDict.GUITAR_PLACEABLE, 1),
                new LootEntry(ItemDict.GYM_BENCH, 1), new LootEntry(ItemDict.HAMMOCK, 1), new LootEntry(ItemDict.HORIZONTAL_MIRROR, 1), new LootEntry(ItemDict.IGLOO, 1), new LootEntry(ItemDict.LAMP, 1),
                new LootEntry(ItemDict.LANTERN, 1), new LootEntry(ItemDict.LATTICE, 1), new LootEntry(ItemDict.LIFEBUOY_SIGN, 1), new LootEntry(ItemDict.LIGHTNING_ROD, 1), new LootEntry(ItemDict.MAILBOX, 1),
                new LootEntry(ItemDict.MARKET_STALL, 1), new LootEntry(ItemDict.MILK_JUG, 1), new LootEntry(ItemDict.MINECART, 1), new LootEntry(ItemDict.ORNATE_MIRROR, 1), new LootEntry(ItemDict.PET_BOWL, 1),
                new LootEntry(ItemDict.PIANO, 1), new LootEntry(ItemDict.POSTBOX, 1), new LootEntry(ItemDict.PYRAMID_STATUE, 1), new LootEntry(ItemDict.RECYCLING_BIN, 1), new LootEntry(ItemDict.SANDBOX, 1),
                new LootEntry(ItemDict.SANDCASTLE, 1), new LootEntry(ItemDict.SNOWMAN, 1), new LootEntry(ItemDict.SLIDE, 1), new LootEntry(ItemDict.SIGNPOST, 1), new LootEntry(ItemDict.SEESAW, 1),
                new LootEntry(ItemDict.SOFA, 1), new LootEntry(ItemDict.SOLAR_PANEL, 1), new LootEntry(ItemDict.SPHERE_STATUE, 1), new LootEntry(ItemDict.STATUE, 1), new LootEntry(ItemDict.STREETLIGHT, 1),
                new LootEntry(ItemDict.STREETLAMP, 1), new LootEntry(ItemDict.SURFBOARD, 1), new LootEntry(ItemDict.SWINGS, 1), new LootEntry(ItemDict.TELEVISION, 1), new LootEntry(ItemDict.TOOLBOX, 1),
                new LootEntry(ItemDict.TOOLRACK, 1), new LootEntry(ItemDict.TRAFFIC_LIGHT, 1), new LootEntry(ItemDict.TRIPLE_MIRRORS, 1), new LootEntry(ItemDict.UMBRELLA, 1), new LootEntry(ItemDict.UMBRELLA_TABLE, 1),
                new LootEntry(ItemDict.WAGON, 1), new LootEntry(ItemDict.WATERTOWER, 1), new LootEntry(ItemDict.WHEELBARROW, 1), new LootEntry(ItemDict.WHITEBOARD, 1), new LootEntry(ItemDict.DRUMSET, 1),
                new LootEntry(ItemDict.HARP, 1), new LootEntry(ItemDict.XYLOPHONE, 1), new LootEntry(ItemDict.CAPE, 1), new LootEntry(ItemDict.GUITAR, 1), new LootEntry(ItemDict.CAT_TAIL, 1),
                new LootEntry(ItemDict.FOX_TAIL, 1), new LootEntry(ItemDict.ROBO_ARMS, 1), new LootEntry(ItemDict.QUERADE_MASK, 1), new LootEntry(ItemDict.SUNGLASSES, 1), new LootEntry(ItemDict.GOGGLES, 1),
                new LootEntry(ItemDict.BLINDFOLD, 1), new LootEntry(ItemDict.BOXING_MITTS, 1), new LootEntry(ItemDict.TEN_GALLON, 1), new LootEntry(ItemDict.CAT_EARS, 1), new LootEntry(ItemDict.BUNNY_EARS, 1),
                new LootEntry(ItemDict.CHEFS_HAT, 1), new LootEntry(ItemDict.DOG_MASK, 1), new LootEntry(ItemDict.DINO_MASK, 1), new LootEntry(ItemDict.TOP_HAT, 1), new LootEntry(ItemDict.WHISKERS, 1),
                new LootEntry(ItemDict.ASCOT, 1), new LootEntry(ItemDict.NECKLACE, 1), new LootEntry(ItemDict.MEDAL, 1), new LootEntry(ItemDict.NOMAD_VEST, 1), new LootEntry(ItemDict.APRON, 1),
                new LootEntry(ItemDict.BATHROBE, 1), new LootEntry(ItemDict.OVERCOAT, 1), new LootEntry(ItemDict.PUNK_JACKET, 1), new LootEntry(ItemDict.WEDDING_DRESS, 1), new LootEntry(ItemDict.SUIT_JACKET, 1),
                new LootEntry(ItemDict.SPORTBALL_UNIFORM, 1), new LootEntry(ItemDict.RAINCOAT, 1), new LootEntry(ItemDict.ISLANDER_TATTOO, 1), new LootEntry(ItemDict.TIGHTIES, 1), new LootEntry(ItemDict.SUPER_SHORTS, 1),
                new LootEntry(ItemDict.BUTTON_DOWN, 1), new LootEntry(ItemDict.LINEN_BUTTON, 1), new LootEntry(ItemDict.TURTLENECK, 1), new LootEntry(ItemDict.STRIPED_SHIRT, 1), new LootEntry(ItemDict.WING_SANDLES, 1),
                new LootEntry(ItemDict.FLASH_HEELS, 1), new LootEntry(ItemDict.MISMATTCHED, 1), new LootEntry(ItemDict.FESTIVE_SOCKS, 1)); 
            VENDING_MACHINE = new LootTable(1, 1, new LootEntry(ItemDict.SUPER_JUICE, 1), new LootEntry(ItemDict.WATERMELON_ICE, 1), new LootEntry(ItemDict.VANILLA_ICE_CREAM, 1), new LootEntry(ItemDict.MINTY_MELT, 1),
                new LootEntry(ItemDict.BERRY_MILKSHAKE, 3), new LootEntry(ItemDict.TOMATO_SOUP, 1), new LootEntry(ItemDict.CREAM_OF_MUSHROOM, 1), new LootEntry(ItemDict.DARK_TEA, 5), new LootEntry(ItemDict.REJUVENATION_TEA, 5),
                new LootEntry(ItemDict.SWEET_COCO_TREAT, 1)); 
            FILING_CABINET = new LootTable(1, 1, new LootEntry(ItemDict.BUBBLE_WALLPAPER, 1)); //todo
            SCI_TABLE1 = new LootTable(4, 6, new LootEntry(ItemDict.RICE_GRASSHOPPER, 10), new LootEntry(ItemDict.BROWN_CICADA, 10), new LootEntry(ItemDict.CAVEWORM, 10), new LootEntry(ItemDict.EARTHWORM, 10), new LootEntry(ItemDict.FIREFLY, 10), new LootEntry(ItemDict.SNAIL, 10), new LootEntry(ItemDict.YELLOW_BUTTERFLY, 10), new LootEntry(ItemDict.STAG_BEETLE, 1), new LootEntry(ItemDict.JEWEL_SPIDER, 1), new LootEntry(ItemDict.EMPRESS_BUTTERFLY, 1));
            SCI_TABLE2 = new LootTable(1, 1, new LootEntry(ItemDict.MOSS_BOTTLE, 50), new LootEntry(ItemDict.TROPICAL_BOTTLE, 10), new LootEntry(ItemDict.SKY_BOTTLE, 3), new LootEntry(ItemDict.SHIMMERING_SALVE, 1)); //todo
            HOEING = new LootTable(1, 1, new LootEntry(ItemDict.CLAY, 137), new LootEntry(ItemDict.CAVEWORM, 32, LootEntry.s2Only), new LootEntry(ItemDict.EARTHWORM, 16, LootEntry.notS2Only), new LootEntry(ItemDict.EARTHWORM, 87, LootEntry.rainyOnly, LootEntry.notS2Only), new LootEntry(ItemDict.LUCKY_COIN, 2), new LootEntry(ItemDict.ANCIENT_KEY, 1));
            MYTHRIL_MACHINE = new MiningLootTable(2, 3, new LootEntry(ItemDict.MYTHRIL_CHIP, 35), new LootEntry(ItemDict.IRON_CHIP, 50), new LootEntry(ItemDict.GOLD_CHIP, 15));
            GEM_ROCK = new MiningLootTable(1, 1, new LootEntry(ItemDict.AQUAMARINE, 6), new LootEntry(ItemDict.DIAMOND, 1), new LootEntry(ItemDict.EMERALD, 3), new LootEntry(ItemDict.AMETHYST, 9), new LootEntry(ItemDict.OPAL, 6), new LootEntry(ItemDict.QUARTZ, 11), new LootEntry(ItemDict.RUBY, 3), new LootEntry(ItemDict.SAPPHIRE, 3), new LootEntry(ItemDict.TOPAZ, 7), new LootEntry(ItemDict.CRYSTAL_KEY, 1));
            STALAGMITE_STALACTITE = new MiningLootTable(2, 6, new LootEntry(ItemDict.STONE, 23), new LootEntry(ItemDict.TRILOBITE, 3), new LootEntry(ItemDict.SALT_SHARDS, 5), new LootEntry(ItemDict.COAL, 3), new LootEntry(ItemDict.IRON_ORE, 3), new LootEntry(ItemDict.MYTHRIL_ORE, 3), new LootEntry(ItemDict.SCRAP_IRON, 6), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1), new LootEntry(ItemDict.ANCIENT_KEY, 2), new LootEntry(ItemDict.METAMORPHIC_KEY, 1), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            LAVA_ROCK = new MiningLootTable(1, 2, new LootEntry(ItemDict.STONE, 30), new LootEntry(ItemDict.TRILOBITE, 9), new LootEntry(ItemDict.GOLD_ORE, 6), new LootEntry(ItemDict.RUBY, 2), new LootEntry(ItemDict.FIRE_CRYSTAL, 2), new LootEntry(ItemDict.IGNEOUS_KEY, 1));
            LAVA_GOLD_ROCK = new MiningLootTable(3, 5, new LootEntry(ItemDict.STONE, 4), new LootEntry(ItemDict.GOLD_ORE, 30), new LootEntry(ItemDict.COAL, 5), new LootEntry(ItemDict.TRILOBITE, 10), new LootEntry(ItemDict.SEDIMENTARY_KEY, 1));
            FOSSIL_ROCK = new MiningLootTable(1, 6, new LootEntry(ItemDict.TRILOBITE, 25), new LootEntry(ItemDict.FOSSIL_SHARDS, 55), new LootEntry(ItemDict.PRIMORDIAL_SHELL, 1), new LootEntry(ItemDict.OLD_BONE, 5), new LootEntry(ItemDict.COAL, 1), new LootEntry(ItemDict.IRON_ORE, 1), new LootEntry(ItemDict.GOLD_ORE, 1), new LootEntry(ItemDict.IGNEOUS_KEY, 2));
        }
    }
}
