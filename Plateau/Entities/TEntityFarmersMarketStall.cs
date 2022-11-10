using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateau.Entities
{
    public class TEntityFarmersMarketStall : TileEntity, ITickDaily, IHaveHoveringInterface, IInteract
    {
        public enum MarketBehavior
        {
            PIPER, TROY, ART, FINLEY
        }

        private Dictionary<TEntityMarketStall.StallType, TEntityMarketStall> stallDict;
        private TEntityMarketStall activeStall;
        private MarketBehavior behavior;
        private bool empty;
        private Texture2D emptyTexture;
        private bool updatedStock;

        public TEntityFarmersMarketStall(Vector2 tilePosition, int tileHeight, int tileWidth, Dictionary<TEntityMarketStall.StallType, TEntityMarketStall> stallDict, Area area, Texture2D emptyStall, MarketBehavior behavior) : base(tilePosition, tileHeight, tileWidth, DrawLayer.NORMAL)
        {
            this.stallDict = stallDict;
            foreach (TEntityMarketStall s in stallDict.Values)
            {
                s.RandomizeStock();
            }
            this.behavior = behavior;
            this.emptyTexture = emptyStall;
            updatedStock = false;
        }

        private void RandomizeStall(Area area)
        {
            empty = false;
            switch(behavior)
            {
                case MarketBehavior.PIPER:
                    if(area.GetDay() < 4 || area.GetWorldSeason() == World.Season.WINTER)
                    {
                        switch(Util.RandInt(1, 4))
                        {
                            case 1:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_JAM];
                                break;
                            case 2:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_DYES];
                                break;
                            case 3:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_CLOTHING];
                                break;
                            case 4:
                            default:
                                empty = true;
                                break;
                        }
                    } else
                    {
                        switch(area.GetWorldSeason())
                        {
                            case World.Season.SPRING:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FARM_SPRING];
                                break;
                            case World.Season.SUMMER:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FARM_SUMMER];
                                break;
                            default:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FARM_AUTUMN];
                                break;
                        }
                    }
                    break;
                case MarketBehavior.TROY:
                    switch(Util.RandInt(1, 10))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_BUTCHER];
                            break;
                        case 7:
                        case 8:
                            activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FISH];
                            break;
                        case 9:
                        case 10:
                        default:
                            activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_MEDIUMS];
                            break;
                    }
                    break;
                case MarketBehavior.ART:
                    activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_PAINTINGS];
                    break;
                case MarketBehavior.FINLEY:
                default:
                    if(Util.RandInt(1, 7) == 1)
                    {
                        empty = true;
                    } else
                    {
                        switch (area.GetWorldSeason())
                        {
                            case World.Season.SPRING:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_SPRING];
                                break;
                            case World.Season.SUMMER:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_SUMMER];
                                break;
                            case World.Season.AUTUMN:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_AUTUMN];
                                break;
                            case World.Season.WINTER:
                            default:
                                activeStall = stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_WINTER];
                                break;
                        }
                    }
                    break;
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            if (!empty)
            {
                activeStall.InteractLeftShift(player, area, world);
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (!empty)
            {
                return activeStall.GetHoveringInterface(player);
            }
            return new HoveringInterface();
        }

        public override void Update(float deltaTime, Area area)
        {
            if(!updatedStock)
            {
                RandomizeStall(area);
                updatedStock = true;
            }
            foreach(TEntityMarketStall s in stallDict.Values) {
                s.Update(deltaTime, area);
            }
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            foreach (TEntityMarketStall s in stallDict.Values)
            {
                s.RandomizeStock();
            }
            updatedStock = false;
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            if (!empty && activeStall != null)
            {
                activeStall.Draw(sb, layerDepth);
            } else
            {
                sb.Draw(emptyTexture, position + new Vector2(0, 1), emptyTexture.Bounds, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            }
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            if (!empty)
            {
                return activeStall.GetLeftShiftClickAction(player);
            }
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            if (!empty)
            {
                return activeStall.GetRightShiftClickAction(player);
            }
            return "";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (!empty)
            {
                return activeStall.GetLeftClickAction(player);
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (!empty)
            {
                return activeStall.GetRightClickAction(player);
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (!empty)
            {
                activeStall.InteractRight(player, area, world);
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (!empty)
            {
                activeStall.InteractLeft(player, area, world);
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            if (!empty)
            {
                activeStall.InteractRightShift(player, area, world);
            }
        }
    }
}
