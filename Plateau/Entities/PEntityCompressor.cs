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

namespace Plateau.Entities
{
    public class PEntityCompressor : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum CompressorState
        {
            IDLE, WORKING, FINISHED
        }

        private class Result
        {
            public int processingTime;
            public Item result;

            public Result(Item result, int processingTime)
            {
                this.result = result;
                this.processingTime = processingTime;
            }
        }

        public static int PROCESSING_TIME_SHORT = 6 * 60;
        public static int PROCESSING_TIME_LONG = 5 * 23 * 60;

        private static Dictionary<Item, Result> RESULT_MAP = new Dictionary<Item, Result>
        {
            {ItemDict.SCRAP_IRON, new Result(ItemDict.IRON_ORE, PROCESSING_TIME_SHORT)},
            {ItemDict.EMERALD_MOSS, new Result(ItemDict.LICHEN_JUICE, PROCESSING_TIME_SHORT)},
            {ItemDict.TOMATO, new Result(ItemDict.TOMATO_SOUP, PROCESSING_TIME_SHORT)},
            {ItemDict.PERSIMMON, new Result(ItemDict.AUTUMN_MASH, PROCESSING_TIME_SHORT)},
            {ItemDict.CAVE_SOYBEAN, new Result(ItemDict.MILK, PROCESSING_TIME_SHORT) },
            {ItemDict.AQUAMARINE, new Result(ItemDict.WATER_CRYSTAL, PROCESSING_TIME_LONG) },
            {ItemDict.MOLTEN_SQUID, new Result(ItemDict.FIRE_CRYSTAL, PROCESSING_TIME_LONG)},
            {ItemDict.BLACKENED_OCTOPUS, new Result(ItemDict.FIRE_CRYSTAL, PROCESSING_TIME_LONG)},
            {ItemDict.CLAY, new Result(ItemDict.EARTH_CRYSTAL, PROCESSING_TIME_LONG) },
            {ItemDict.BLACK_FEATHER, new Result(ItemDict.WIND_CRYSTAL, PROCESSING_TIME_LONG) },
            {ItemDict.RED_FEATHER, new Result(ItemDict.WIND_CRYSTAL, PROCESSING_TIME_LONG) },
            {ItemDict.BLUE_FEATHER, new Result(ItemDict.WIND_CRYSTAL, PROCESSING_TIME_LONG) },
            {ItemDict.WHITE_FEATHER, new Result(ItemDict.PRISMATIC_FEATHER, PROCESSING_TIME_LONG) },
            {ItemDict.GOLD_BAR, new Result(ItemDict.GOLDEN_LEAF, PROCESSING_TIME_LONG) },
            {ItemDict.EMPRESS_BUTTERFLY, new Result(ItemDict.FAIRY_DUST, PROCESSING_TIME_LONG) },
            {ItemDict.OYSTER, new Result(ItemDict.PEARL, PROCESSING_TIME_LONG)},
            {ItemDict.SNOW_CRYSTAL, new Result(ItemDict.ICE_NINE, PROCESSING_TIME_LONG)},
            {ItemDict.WOOD, new Result(ItemDict.HARDWOOD, PROCESSING_TIME_LONG)},
            {ItemDict.EGG, new Result(ItemDict.GOLDEN_EGG, PROCESSING_TIME_LONG)},
            {ItemDict.WOOL, new Result(ItemDict.GOLDEN_WOOL, PROCESSING_TIME_LONG)},
            {ItemDict.HONEY_BEE, new Result(ItemDict.QUEENS_STINGER, PROCESSING_TIME_LONG)},
            {ItemDict.BAT_WING, new Result(ItemDict.ALBINO_WING, PROCESSING_TIME_LONG)},
            {ItemDict.COAL, new Result(ItemDict.DIAMOND, PROCESSING_TIME_LONG)},
            {ItemDict.STONE, new Result(ItemDict.COAL, PROCESSING_TIME_SHORT)},
        };
        private PartialRecolorSprite sprite;
        private Item heldItem;
        private int timeRemaining;
        private CompressorState state;
        private ResultHoverBox resultHoverBox;

        public PEntityCompressor(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 10, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = CompressorState.IDLE;
            this.timeRemaining = 0;
            this.resultHoverBox = new ResultHoverBox();
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y), layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("state", state.ToString());
            save.AddData("item", heldItem.GetName());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName()));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", CompressorState.IDLE.ToString());
            if (stateStr.Equals(CompressorState.IDLE.ToString()))
            {
                state = CompressorState.IDLE;
            }
            else if (stateStr.Equals(CompressorState.WORKING.ToString()))
            {
                state = CompressorState.WORKING;
            }
            else if (stateStr.Equals(CompressorState.FINISHED.ToString()))
            {
                state = CompressorState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("anim");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == CompressorState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == CompressorState.FINISHED)
            {
                resultHoverBox.AssignItemStack(heldItem);
            }
            else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (heldItem != ItemDict.NONE)
            {
                area.AddEntity(new EntityItem(heldItem, new Vector2(position.X, position.Y - 10)));
            }
            base.OnRemove(player, area, world);
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

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (state == CompressorState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == CompressorState.FINISHED)
            {
                return "Empty";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == CompressorState.FINISHED)
            {
                area.AddEntity(new EntityItem(heldItem, new Vector2(position.X, position.Y - 10)));
                sprite.SetLoop("placement");
                heldItem = ItemDict.NONE;
                state = CompressorState.IDLE;
            }
        }

        private Item GetResultFor(Item input)
        {
            if(RESULT_MAP.ContainsKey(input))
            {
                return RESULT_MAP[input].result;
            }
            else if (input.HasTag(Item.Tag.FISH) || input.HasTag(Item.Tag.FLOWER))
            {
                return ItemDict.OIL;
            }
            return ItemDict.NONE;
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == CompressorState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH) && GetResultFor(addedItem) != ItemDict.NONE)
                {
                    heldItem = addedItem;
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = CompressorState.WORKING;
                    timeRemaining = RESULT_MAP[heldItem].processingTime;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't compress this.", Color.Red));
                }
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void Tick(int time, EntityPlayer player, Area area, World world)
        {
            timeRemaining -= time;
            if (timeRemaining <= 0 && state == CompressorState.WORKING)
            {
                heldItem = RESULT_MAP[heldItem].result;
                state = CompressorState.FINISHED;
                sprite.SetLoop("placement");
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == CompressorState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
