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
    class TEntityFarmable : TileEntity, ITickDaily, IInteract, IInteractTool
    {
        public static CropInfo GetCropForSeed(Item seed)
        {
            foreach (CropInfo crop in CROP_LIST)
            {
                if(seed == crop.seed || seed == crop.shiningSeed)
                {
                    return crop;
                }
            }
            return null;
        }

        public class CropInfo
        {
            public enum Crop
            {
                CACTUS, SPINACH, BEET, FLAX, POTATO, ONION, COTTON, BELLPEPPER, CUCUMBER, CARROT, CABBAGE, EGGPLANT, TOMATO, PUMPKIN, STRAWBERRY, BROCCOLI, WATERMELON
            }

            public Crop crop;
            public float growthTime;
            public Item yield, seed, shiningSeed, silverYield, goldenYield;
            public bool regrowth;
            public int yieldAmount;
            public string growthStage1LoopName, growthStage2LoopName;
            public World.Season season;

            public CropInfo(Crop crop, float growthTime, Item yield, Item silverYield, Item goldenYield, int yieldAmount, bool regrowth, string gS1LN, string gS2LN, Item seed, Item shiningSeed, World.Season season)
            {
                this.crop = crop;
                this.growthTime = growthTime;
                this.yield = yield;
                this.silverYield = silverYield;
                this.goldenYield = goldenYield;
                this.regrowth = regrowth;
                this.yieldAmount = yieldAmount;
                this.growthStage1LoopName = gS1LN;
                this.growthStage2LoopName = gS2LN;
                this.shiningSeed = shiningSeed;
                this.seed = seed;
                this.season = season;
            }
        }

        private class FertilizerState
        {
            public bool loamy = false;
            public bool quality = false;
            public bool dew = false;
            public bool sweet = false;
            public bool decay = false;
            public bool frost = false;
            public bool thick = false;
            public bool shining = false;

            public string DRY;
            public string WET;

            public FertilizerState()
            {
                DRY = DRY_FARMLAND_LOOP;
                WET = WET_FARMLAND_LOOP;
            }

            public bool ApplyCompost(Item compost)
            {
                if (compost == ItemDict.LOAMY_COMPOST)
                {
                    if (!loamy)
                    {
                        loamy = true;
                        DRY = DRY_LOAMY_LOOP;
                        WET = WET_LOAMY_LOOP;
                        return true;
                    }
                }
                else if (compost == ItemDict.QUALITY_COMPOST)
                {
                    if (!quality)
                    {
                        DRY = DRY_QUALITY_LOOP;
                        WET = WET_QUALITY_LOOP;
                        quality = true;
                        return true;
                    }
                }
                else if (compost == ItemDict.DEW_COMPOST)
                {
                    if (!dew)
                    {
                        DRY = DRY_DEW_LOOP;
                        WET = WET_DEW_LOOP;
                        dew = true;
                        return true;
                    }
                }
                else if (compost == ItemDict.SWEET_COMPOST)
                {
                    if (!sweet)
                    {
                        DRY = DRY_SWEET_LOOP;
                        WET = WET_SWEET_LOOP;
                        sweet = true;
                        return true;
                    }
                }
                else if (compost == ItemDict.DECAY_COMPOST)
                {
                    if (!decay)
                    {
                        DRY = DRY_DECAY_LOOP;
                        WET = WET_DECAY_LOOP;
                        decay = true;
                        return true;
                    }
                } else if (compost == ItemDict.FROST_COMPOST)
                {
                    if (!frost)
                    {
                        DRY = DRY_FROST_LOOP;
                        WET = WET_FROST_LOOP;
                        frost = true;
                        return true;
                    }
                } else if (compost == ItemDict.THICK_COMPOST)
                {
                    if (!thick)
                    {
                        DRY = DRY_THICK_LOOP;
                        WET = WET_THICK_LOOP;
                        thick = true;
                        return true;
                    }
                } else if (compost == ItemDict.SHINING_COMPOST)
                {
                    if (!shining)
                    {
                        DRY = DRY_SHINING_LOOP;
                        WET = WET_SHINING_LOOP;
                        shining = true;
                        return true;
                    }
                } 
                return false;
            }

            public int CompostCount()
            {
                return (loamy ? 1 : 0) + (quality ? 1 : 0) + (dew ? 1 : 0) + (sweet ? 1 : 0) + (decay ? 1 : 0) + (frost ? 1 : 0) + (thick ? 1 : 0) + (shining ? 1 : 0);
            }

            public void Reset()
            {
                loamy = false;
                quality = false;
                dew = false;
                sweet = false;
                decay = false;
                frost = false;
                thick = false;
                shining = false;
                DRY = DRY_FARMLAND_LOOP;
                WET = WET_FARMLAND_LOOP;
            }
        }

        private static string DRY_FARMLAND_LOOP = "dry";
        private static string WET_FARMLAND_LOOP = "wet";
        private static string DRY_DECAY_LOOP = "dryDecay";
        private static string WET_DECAY_LOOP = "wetDecay";
        private static string DRY_DEW_LOOP = "dryDew";
        private static string WET_DEW_LOOP = "wetDew";
        private static string DRY_FROST_LOOP = "dryFrost";
        private static string WET_FROST_LOOP = "wetFrost";
        private static string DRY_LOAMY_LOOP = "dryLoamy";
        private static string WET_LOAMY_LOOP = "wetLoamy";
        private static string DRY_QUALITY_LOOP = "dryQuality";
        private static string WET_QUALITY_LOOP = "wetQuality";
        private static string DRY_SHINING_LOOP = "dryShining";
        private static string WET_SHINING_LOOP = "wetShining";
        private static string DRY_SWEET_LOOP = "drySweet";
        private static string WET_SWEET_LOOP = "wetSweet";
        private static string DRY_THICK_LOOP = "dryThick";
        private static string WET_THICK_LOOP = "wetThick";
        private static string SEED_LOOP = "seed";
        private static string CACTUS_G1_LOOP = "cactus1";
        private static string CACTUS_G2_LOOP = "cactus2";
        private static string SPINACH_G1_LOOP = "spinach1";
        private static string SPINACH_G2_LOOP = "spinach2";
        private static string BEET_G1_LOOP = "beet1";
        private static string BEET_G2_LOOP = "beet2";
        private static string FLAX_G1_LOOP = "flax1";
        private static string FLAX_G2_LOOP = "flax2";
        private static string SOYBEAN_G1_LOOP = "soybean1";
        private static string SOYBEAN_G2_LOOP = "soybean2";
        private static string POTATO_G1_LOOP = "potato1";
        private static string POTATO_G2_LOOP = "potato2";
        private static string ONION_G1_LOOP = "onion1";
        private static string ONION_G2_LOOP = "onion2";
        private static string COTTON_G1_LOOP = "cotton1";
        private static string COTTON_G2_LOOP = "cotton2";
        private static string BELLPEPPER_G1_LOOP = "bellpepper1";
        private static string BELLPEPPER_G2_LOOP = "bellpepper2";
        private static string STRAWBERRY_G1_LOOP = "strawberry1";
        private static string STRAWBERRY_G2_LOOP = "strawberry2";
        private static string BROCCOLI_G1_LOOP = "broccoli1";
        private static string BROCCOLI_G2_LOOP = "broccoli2";
        private static string CUCUMBER_G1_LOOP = "cucumber1";
        private static string CUCUMBER_G2_LOOP = "cucumber2";
        private static string CARROT_G1_LOOP = "carrot1";
        private static string CARROT_G2_LOOP = "carrot2";
        private static string CABBAGE_G1_LOOP = "cabbage1";
        private static string CABBAGE_G2_LOOP = "cabbage2";
        private static string EGGPLANT_G1_LOOP = "eggplant1";
        private static string EGGPLANT_G2_LOOP = "eggplant2";
        private static string TOMATO_G1_LOOP = "tomato1";
        private static string TOMATO_G2_LOOP = "tomato2";
        private static string PUMPKIN_G1_LOOP = "pumpkin1";
        private static string PUMPKIN_G2_LOOP = "pumpkin2";
        private static string WATERMELON_G1_LOOP = "watermelon1";
        private static string WATERMELON_G2_LOOP = "watermelon2";
        private static string GRASS_LOOP = "grass";

        private static CropInfo CACTUS_INFO = new CropInfo(CropInfo.Crop.CACTUS, 3, ItemDict.CACTUS, ItemDict.SILVER_CACTUS, ItemDict.GOLDEN_CACTUS, 1, false, CACTUS_G1_LOOP, CACTUS_G2_LOOP, ItemDict.CACTUS_SEEDS, ItemDict.SHINING_CACTUS_SEEDS, World.Season.SUMMER);
        private static CropInfo SPINACH_INFO = new CropInfo(CropInfo.Crop.SPINACH, 2, ItemDict.SPINACH, ItemDict.SILVER_SPINACH, ItemDict.GOLDEN_SPINACH, 1, false, SPINACH_G1_LOOP, SPINACH_G2_LOOP, ItemDict.SPINACH_SEEDS, ItemDict.SHINING_SPINACH_SEEDS, World.Season.SPRING);
        private static CropInfo BEET_INFO = new CropInfo(CropInfo.Crop.BEET, 2, ItemDict.BEET, ItemDict.SILVER_BEET, ItemDict.GOLDEN_BEET, 2, false, BEET_G1_LOOP, BEET_G2_LOOP, ItemDict.BEET_SEEDS, ItemDict.SHINING_BEET_SEEDS, World.Season.AUTUMN);
        private static CropInfo FLAX_INFO = new CropInfo(CropInfo.Crop.FLAX, 5, ItemDict.FLAX, ItemDict.SILVER_FLAX, ItemDict.GOLDEN_FLAX, 2, false, FLAX_G1_LOOP, FLAX_G2_LOOP, ItemDict.FLAX_SEEDS, ItemDict.SHINING_FLAX_SEEDS, World.Season.AUTUMN);
        private static CropInfo POTATO_INFO = new CropInfo(CropInfo.Crop.POTATO, 3, ItemDict.POTATO, ItemDict.SILVER_POTATO, ItemDict.GOLDEN_POTATO, 4, false, POTATO_G1_LOOP, POTATO_G2_LOOP, ItemDict.POTATO_SEEDS, ItemDict.SHINING_POTATO_SEEDS, World.Season.SPRING);
        private static CropInfo ONION_INFO = new CropInfo(CropInfo.Crop.ONION, 2, ItemDict.ONION, ItemDict.SILVER_ONION, ItemDict.GOLDEN_ONION, 2, false, ONION_G1_LOOP, ONION_G2_LOOP, ItemDict.ONION_SEEDS, ItemDict.SHINING_ONION_SEEDS, World.Season.SUMMER);
        private static CropInfo COTTON_INFO = new CropInfo(CropInfo.Crop.COTTON, 5, ItemDict.COTTON, ItemDict.SILVER_COTTON, ItemDict.GOLDEN_COTTON, 3, false, COTTON_G1_LOOP, COTTON_G2_LOOP, ItemDict.COTTON_SEEDS, ItemDict.SHINING_COTTON_SEEDS, World.Season.SUMMER);
        private static CropInfo BELLPEPPER_INFO = new CropInfo(CropInfo.Crop.BELLPEPPER, 3, ItemDict.BELLPEPPER, ItemDict.SILVER_BELLPEPPER, ItemDict.GOLDEN_BELLPEPPER, 1, true, BELLPEPPER_G1_LOOP, BELLPEPPER_G2_LOOP, ItemDict.BELLPEPPER_SEEDS, ItemDict.SHINING_BELLPEPPER_SEEDS, World.Season.AUTUMN);
        private static CropInfo STRAWBERRY_INFO = new CropInfo(CropInfo.Crop.STRAWBERRY, 4, ItemDict.STRAWBERRY, ItemDict.SILVER_STRAWBERRY, ItemDict.GOLDEN_STRAWBERRY, 2, true, STRAWBERRY_G1_LOOP, STRAWBERRY_G2_LOOP, ItemDict.STRAWBERRY_SEEDS, ItemDict.SHINING_STRAWBERRY_SEEDS, World.Season.SPRING);
        private static CropInfo BROCCOLI_INFO = new CropInfo(CropInfo.Crop.BROCCOLI, 3, ItemDict.BROCCOLI, ItemDict.SILVER_BROCCOLI, ItemDict.GOLDEN_BROCCOLI, 3, false, BROCCOLI_G1_LOOP, BROCCOLI_G2_LOOP, ItemDict.BROCCOLI_SEEDS, ItemDict.SHINING_BROCCOLI_SEEDS, World.Season.AUTUMN);
        private static CropInfo CUCUMBER_INFO = new CropInfo(CropInfo.Crop.CUCUMBER, 2, ItemDict.CUCUMBER, ItemDict.SILVER_CUCUMBER, ItemDict.GOLDEN_CUCUMBER, 1, true, CUCUMBER_G1_LOOP, CUCUMBER_G2_LOOP, ItemDict.CUCUMBER_SEEDS, ItemDict.SHINING_CUCUMBER_SEEDS, World.Season.SUMMER);
        private static CropInfo CARROT_INFO = new CropInfo(CropInfo.Crop.CARROT, 5, ItemDict.CARROT, ItemDict.SILVER_CARROT, ItemDict.GOLDEN_CARROT, 3, false, CARROT_G1_LOOP, CARROT_G2_LOOP, ItemDict.CARROT_SEEDS, ItemDict.SHINING_CARROT_SEEDS, World.Season.SPRING);
        private static CropInfo CABBAGE_INFO = new CropInfo(CropInfo.Crop.CABBAGE, 6, ItemDict.CABBAGE, ItemDict.SILVER_CABBAGE, ItemDict.GOLDEN_CABBAGE, 1, false, CABBAGE_G1_LOOP, CABBAGE_G2_LOOP, ItemDict.CABBAGE_SEEDS, ItemDict.SHINING_CABBAGE_SEEDS, World.Season.AUTUMN);
        private static CropInfo EGGPLANT_INFO = new CropInfo(CropInfo.Crop.EGGPLANT, 4, ItemDict.EGGPLANT, ItemDict.SILVER_EGGPLANT, ItemDict.GOLDEN_EGGPLANT, 1, true, EGGPLANT_G1_LOOP, EGGPLANT_G2_LOOP, ItemDict.EGGPLANT_SEEDS, ItemDict.SHINING_EGGPLANT_SEEDS, World.Season.SUMMER);
        private static CropInfo TOMATO_INFO = new CropInfo(CropInfo.Crop.TOMATO, 5, ItemDict.TOMATO, ItemDict.SILVER_TOMATO, ItemDict.GOLDEN_TOMATO, 2, true, TOMATO_G1_LOOP, TOMATO_G2_LOOP, ItemDict.TOMATO_SEEDS, ItemDict.SHINING_TOMATO_SEEDS, World.Season.SUMMER);
        private static CropInfo PUMPKIN_INFO = new CropInfo(CropInfo.Crop.PUMPKIN, 7, ItemDict.PUMPKIN, ItemDict.SILVER_PUMPKIN, ItemDict.GOLDEN_PUMPKIN, 1, false, PUMPKIN_G1_LOOP, PUMPKIN_G2_LOOP, ItemDict.PUMPKIN_SEEDS, ItemDict.SHINING_PUMPKIN_SEEDS, World.Season.AUTUMN);
        private static CropInfo WATERMELON_INFO = new CropInfo(CropInfo.Crop.WATERMELON, 6, ItemDict.WATERMELON_SLICE, ItemDict.SILVER_WATERMELON_SLICE, ItemDict.GOLDEN_WATERMELON_SLICE, 5, false, WATERMELON_G1_LOOP, WATERMELON_G2_LOOP, ItemDict.WATERMELON_SEEDS, ItemDict.SHINING_WATERMELON_SEEDS, World.Season.SUMMER);

        public static CropInfo[] CROP_LIST = { CACTUS_INFO, SPINACH_INFO, BEET_INFO, FLAX_INFO, POTATO_INFO, ONION_INFO, COTTON_INFO, BELLPEPPER_INFO,
        STRAWBERRY_INFO, BROCCOLI_INFO, CUCUMBER_INFO, CARROT_INFO, CABBAGE_INFO, EGGPLANT_INFO, TOMATO_INFO, PUMPKIN_INFO, WATERMELON_INFO};

        private AnimatedSprite sprite, cropSprite;
        private float growth, quality;
        private bool isWatered, geyserWater;
        private CropInfo planted;
        private FertilizerState fertilizerState;

        public TEntityFarmable(AnimatedSprite sprite, Vector2 tilePosition, AnimatedSprite cropSprite) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.sprite = sprite;
            sprite.AddLoop(DRY_FARMLAND_LOOP, 0, 0, true);
            sprite.AddLoop(WET_FARMLAND_LOOP, 1, 1, true);
            sprite.AddLoop(DRY_DECAY_LOOP, 2, 2, true);
            sprite.AddLoop(WET_DECAY_LOOP, 3, 3, true);
            sprite.AddLoop(DRY_DEW_LOOP, 4, 4, true);
            sprite.AddLoop(WET_DEW_LOOP, 5, 5, true);
            sprite.AddLoop(DRY_FROST_LOOP, 6, 6, true);
            sprite.AddLoop(WET_FROST_LOOP, 7, 7, true);
            sprite.AddLoop(DRY_LOAMY_LOOP, 8, 8, true);
            sprite.AddLoop(WET_LOAMY_LOOP, 9, 9, true);
            sprite.AddLoop(DRY_QUALITY_LOOP, 10, 10, true);
            sprite.AddLoop(WET_QUALITY_LOOP, 11, 11, true);
            sprite.AddLoop(DRY_SHINING_LOOP, 12, 12, true);
            sprite.AddLoop(WET_SHINING_LOOP, 13, 13, true);
            sprite.AddLoop(DRY_SWEET_LOOP, 14, 14, true);
            sprite.AddLoop(WET_SWEET_LOOP, 15, 15, true);
            sprite.AddLoop(DRY_THICK_LOOP, 16, 16, true);
            sprite.AddLoop(WET_THICK_LOOP, 17, 17, true);
            fertilizerState = new FertilizerState();
            sprite.SetLoop(fertilizerState.DRY);
            this.planted = null;
            this.cropSprite = cropSprite;
            cropSprite.AddLoop(SEED_LOOP, 0, 0, true);
            cropSprite.AddLoop(CACTUS_G1_LOOP, 1, 1, true);
            cropSprite.AddLoop(CACTUS_G2_LOOP, 2, 2, true);
            cropSprite.AddLoop(SPINACH_G1_LOOP, 3, 3, true);
            cropSprite.AddLoop(SPINACH_G2_LOOP, 4, 4, true);
            cropSprite.AddLoop(BEET_G1_LOOP, 5, 5, true);
            cropSprite.AddLoop(BEET_G2_LOOP, 6, 6, true);
            cropSprite.AddLoop(FLAX_G1_LOOP, 7, 7, true);
            cropSprite.AddLoop(FLAX_G2_LOOP, 8, 8, true);
            cropSprite.AddLoop(SOYBEAN_G1_LOOP, 9, 9, true); //UNUSED
            cropSprite.AddLoop(SOYBEAN_G2_LOOP, 10, 10, true);
            cropSprite.AddLoop(POTATO_G1_LOOP, 11, 11, true);
            cropSprite.AddLoop(POTATO_G2_LOOP, 12, 12, true);
            cropSprite.AddLoop(ONION_G1_LOOP, 13, 13, true);
            cropSprite.AddLoop(ONION_G2_LOOP, 14, 14, true);
            cropSprite.AddLoop(COTTON_G1_LOOP, 15, 15, true);
            cropSprite.AddLoop(COTTON_G2_LOOP, 16, 16, true);
            cropSprite.AddLoop(BELLPEPPER_G1_LOOP, 17, 17, true);
            cropSprite.AddLoop(BELLPEPPER_G2_LOOP, 18, 18, true);
            cropSprite.AddLoop(STRAWBERRY_G1_LOOP, 19, 19, true);
            cropSprite.AddLoop(STRAWBERRY_G2_LOOP, 20, 20, true);
            cropSprite.AddLoop(BROCCOLI_G1_LOOP, 21, 21, true);
            cropSprite.AddLoop(BROCCOLI_G2_LOOP, 22, 22, true);
            cropSprite.AddLoop(CUCUMBER_G1_LOOP, 23, 23, true);
            cropSprite.AddLoop(CUCUMBER_G2_LOOP, 24, 24, true);
            cropSprite.AddLoop(CARROT_G1_LOOP, 25, 25, true);
            cropSprite.AddLoop(CARROT_G2_LOOP, 26, 26, true);
            cropSprite.AddLoop(CABBAGE_G1_LOOP, 27, 27, true);
            cropSprite.AddLoop(CABBAGE_G2_LOOP, 28, 28, true);
            cropSprite.AddLoop(EGGPLANT_G1_LOOP, 29, 29, true);
            cropSprite.AddLoop(EGGPLANT_G2_LOOP, 30, 30, true);
            cropSprite.AddLoop(TOMATO_G1_LOOP, 31, 31, true);
            cropSprite.AddLoop(TOMATO_G2_LOOP, 32, 32, true);
            cropSprite.AddLoop(PUMPKIN_G1_LOOP, 33, 33, true);
            cropSprite.AddLoop(PUMPKIN_G2_LOOP, 34, 34, true);
            cropSprite.AddLoop(WATERMELON_G1_LOOP, 35, 35, true);
            cropSprite.AddLoop(WATERMELON_G2_LOOP, 36, 36, true);
            cropSprite.AddLoop(GRASS_LOOP, 37, 37, true);
            cropSprite.SetLoop(SEED_LOOP);
            this.quality = 0;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.SetLoopIfNot(isWatered ? fertilizerState.WET : fertilizerState.DRY);
            sprite.Draw(sb, new Vector2(position.X, position.Y+1), Color.White, layerDepth);
            if(planted != null)
            {
                cropSprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
            }
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("entitytype", EntityType.FARMABLE.ToString());
            save.AddData("planted", planted == null ? "NOTHING" : planted.crop.ToString());
            save.AddData("growth", growth.ToString());
            save.AddData("quality", quality.ToString());
            save.AddData("isWatered", isWatered.ToString());
            return save;
        }
        public override bool ShouldBeSaved()
        {
            return true;
        }

        public bool RemovedAtSeasonShift(World.Season newSeason)
        {
            if(planted != null && fertilizerState.frost)
            {
                return false;
            }
            return true;
        }

        public void WaterGeyser(Area area)
        {
            Water(area);
            geyserWater = true;
        }
        public void Water(Area area)
        {
            if(!isWatered)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(position + new Vector2(3 + (i / 2) + Util.RandInt(0, 10) / 10.0f, 15f - i), ParticleBehavior.RUSH_OUTWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_WATER_PRIMARY.color, ParticleFactory.DURATION_SHORT));
                    }
                }
            }

            isWatered = true;
            //water particles
        }

        private bool Plant(Item seed)
        {
            CropInfo info = GetCropForSeed(seed);
            if(info == null)
            {
                return false;
            }

            if (info.shiningSeed == seed )
            {
                quality += 25;
            }

            planted = info;
            UpdateFrame();
            return true;
        }

        public override void LoadSave(SaveState state)
        {
            string cropStr = state.TryGetData("planted", "NOTHING");
            if(!cropStr.Equals("NOTHING"))
            {
                foreach(CropInfo crop in CROP_LIST)
                {
                    if(cropStr.Equals(crop.crop.ToString()))
                    {
                        planted = crop;
                    }
                }
            }

            growth = float.Parse(state.TryGetData("growth", "0.0"));
            quality = float.Parse(state.TryGetData("quality", "0.0"));
            isWatered = bool.Parse(state.TryGetData("isWatered", "false"));

            UpdateFrame();
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            if (planted != null)
            {
                /*if (area.GetWeather() == World.Weather.SUNNY)
                {
                    quality += 3f;
                    growth += 0.25f;
                }*/

                if (geyserWater)
                {
                    quality += 2f;
                }

                if(player.HasEffect(AppliedEffects.WISHBOAT_HEALTH))
                {
                    growth += 0.35f;
                    quality += 3f;
                }


                if (fertilizerState.dew)
                {
                    growth += 0.5f;
                }
                    
                growth += isWatered ? 1 : 0.55f;
                quality += isWatered ? Util.RandInt(2, 5) : -3;

                if (planted.season != area.GetSeason() && !fertilizerState.frost)
                {
                    planted = null;
                    growth = 0;
                    fertilizerState.Reset();
                    quality = 0;
                } 

            } else
            {
                growth = 0;
                quality = 0;
            }

            if (!fertilizerState.thick && !geyserWater)
            {
                isWatered = false;
            }
            geyserWater = false;
            
            UpdateFrame();
        }

        private void UpdateFrame()
        {
            if (growth < 1)
            {
                cropSprite.SetLoop(SEED_LOOP);
            }
            else if (growth < planted.growthTime)
            {
                cropSprite.SetLoop(planted.growthStage1LoopName);
            }
            else
            {
                cropSprite.SetLoop(planted.growthStage2LoopName);
            }   
        }

        public override void Update(float deltaTime, Area area)
        {
            if(area.GetWeather() == World.Weather.RAINY || area.GetWeather() == World.Weather.SNOWY)
            {
                Water(area);
            }

            sprite.Update(deltaTime);
            if(planted != null)
            {
                cropSprite.Update(deltaTime);
            }
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (planted != null && growth >= planted.growthTime)
            {
                float qualityScore = quality + Util.RandInt(0, 15);
                for (int i = 0; i < planted.yieldAmount * (fertilizerState.shining ? 2 : 1); i++)
                {
                    Item yield;
                    if (fertilizerState.decay)
                    {
                        yield = ItemDict.CLAY;
                    }
                    else if (qualityScore >= 55 || Util.RandInt(0, 99) == 0)
                    {
                        yield = planted.goldenYield;

                    }
                    else if (qualityScore >= 30 || Util.RandInt(0, 99) < 5)
                    {
                        yield = planted.silverYield;
                    }
                    else
                    {
                        yield = planted.yield;
                    }

                    area.AddEntity(new EntityItem(yield, new Vector2(position.X, position.Y - 8)));
                }
                if (fertilizerState.loamy)
                {
                    if (Util.RandInt(0, 1) == 0)
                    {
                        area.AddEntity(new EntityItem(planted.seed, new Vector2(position.X, position.Y - 8)));
                    }
                }
                if (fertilizerState.sweet)
                {
                    LootTables.LootTable insects = LootTables.INSECT;
                    for (int i = 0; i < Util.RandInt(1, 4); i++)
                    {
                        List<Item> loot = insects.RollLoot(player, area, world.GetTimeData());
                        foreach (Item ins in loot)
                        {
                            area.AddEntity(new EntityItem(ins, new Vector2(position.X, position.Y - 16)));
                        }
                    }
                }

                if (planted.regrowth)
                {
                    growth = planted.growthTime - 1f;
                    UpdateFrame();
                }
                else
                {
                    growth = 0;
                    planted = null;
                    fertilizerState.Reset();
                    quality = 0;
                }

                if (Util.RandInt(1, 500) == 1)
                {
                    area.AddEntity(new EntityItem(ItemDict.METAMORPHIC_KEY, new Vector2(position.X, position.Y - 16)));
                }
                else if (Util.RandInt(1, 100) == 1)
                {
                    area.AddEntity(new EntityItem(ItemDict.ANCIENT_KEY, new Vector2(position.X, position.Y - 16)));
                }

                GameState.STATISTICS[GameState.STAT_CROPS_HARVESTED] += 1;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            Item heldItem = player.GetHeldItem().GetItem();

            if (heldItem.HasTag(Item.Tag.SEED) && planted == null)
            {
                if (Plant(heldItem))
                {
                    if (fertilizerState.quality)
                    {
                        quality += 20;
                    }
                    if (fertilizerState.shining)
                    {
                        quality += 10;
                        growth += 0.5f;
                    }
                    player.GetHeldItem().Subtract(1);
                    UpdateFrame();
                }
            }
            else if (heldItem.HasTag(Item.Tag.COMPOST) && planted == null)
            {
                if (fertilizerState.CompostCount() < 1)
                {
                    if (fertilizerState.ApplyCompost(heldItem))
                    {
                        player.GetHeldItem().Subtract(1);
                    }
                }
            } else if (heldItem == ItemDict.PHILOSOPHERS_STONE && growth >= planted.growthTime)
            {
                quality += 1000;
                player.GetHeldItem().Subtract(1);
                InteractRight(player, area, world);
            }
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if(player.GetHeldItem().GetItem().HasTag(Item.Tag.COMPOST))
            {
                return "Apply";
            } else if (player.GetHeldItem().GetItem().HasTag(Item.Tag.SEED))
            {
                return "Sow";
            }  else
            {
                return "";
            }
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if(planted != null && growth >= planted.growthTime)
            {
                return "Harvest";
            } 
            return "";
        }

        public void Accelerate(int daysAccelerated)
        {
            growth += daysAccelerated;

            if (planted != null)
            {
                UpdateFrame();
            }
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractTool(EntityPlayer player, Area area, World world)
        {
            Item heldItem = player.GetHeldItem().GetItem();
            if (heldItem.HasTag(Item.Tag.WATERING_CAN))
            {
                Water(area);
            }
            else if (heldItem.HasTag(Item.Tag.PICKAXE))
            {
                area.RemoveTileEntity(player, (int)this.tilePosition.X, (int)this.tilePosition.Y, world);
            }
        }
    }
}
