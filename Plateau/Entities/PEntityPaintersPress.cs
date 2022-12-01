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
    public class PEntityPaintersPress : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum PressState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 1 * 30;
        private PartialRecolorSprite sprite;
        private Item heldItem;
        private int timeRemaining;
        private PressState state;
        private ResultHoverBox resultHoverBox;

        private static Dictionary<Item, Item> DYE_MAP = new Dictionary<Item, Item> {
            {ItemDict.BLUEBELL, ItemDict.BLUE_DYE}, {ItemDict.BLUEGILL, ItemDict.BLUE_DYE},
            {ItemDict.FLAX, ItemDict.CYAN_DYE}, 
            {ItemDict.SEA_URCHIN, ItemDict.NAVY_DYE},
            {ItemDict.INKY_SQUID, ItemDict.BLACK_DYE}, {ItemDict.BLACKENED_OCTOPUS, ItemDict.BLACK_DYE}, {ItemDict.ONYX_EEL, ItemDict.BLACK_DYE},
            {ItemDict.RED_GINGER, ItemDict.RED_DYE}, {ItemDict.CHERRY, ItemDict.RED_DYE}, {ItemDict.APPLE, ItemDict.RED_DYE}, {ItemDict.WATERMELON_SLICE, ItemDict.RED_DYE},
            {ItemDict.CRIMSON_CORAL, ItemDict.CRIMSON_DYE}, {ItemDict.BEET, ItemDict.CRIMSON_DYE},
            {ItemDict.FAIRY_DUST, ItemDict.PINK_DYE}, {ItemDict.PEARL, ItemDict.PINK_DYE},
            {ItemDict.CACAO_BEAN, ItemDict.DARK_BROWN_DYE}, {ItemDict.POTATO, ItemDict.DARK_BROWN_DYE}, 
            {ItemDict.COCONUT, ItemDict.LIGHT_BROWN_DYE},  {ItemDict.CHICORY_ROOT, ItemDict.LIGHT_BROWN_DYE}, {ItemDict.CAVE_SOYBEAN, ItemDict.LIGHT_BROWN_DYE},
            {ItemDict.MOUNTAIN_WHEAT, ItemDict.WHEAT_DYE}, 
            {ItemDict.MARIGOLD, ItemDict.ORANGE_DYE}, {ItemDict.PUMPKIN, ItemDict.ORANGE_DYE}, {ItemDict.ORANGE, ItemDict.ORANGE_DYE}, {ItemDict.CARROT, ItemDict.ORANGE_DYE},
            {ItemDict.SUNFLOWER, ItemDict.YELLOW_DYE}, {ItemDict.BANANA, ItemDict.YELLOW_DYE}, {ItemDict.LEMON, ItemDict.YELLOW_DYE},
            {ItemDict.LAVENDER, ItemDict.PURPLE_DYE}, {ItemDict.ROYAL_JELLY, ItemDict.PURPLE_DYE}, {ItemDict.EGGPLANT, ItemDict.PURPLE_DYE},
            {ItemDict.EMERALD_MOSS, ItemDict.GREEN_DYE}, {ItemDict.CACTUS, ItemDict.GREEN_DYE},
            {ItemDict.MOSSY_BARK, ItemDict.OLIVE_DYE}, {ItemDict.OLIVE, ItemDict.OLIVE_DYE},
            {ItemDict.SNOWDROP, ItemDict.WHITE_DYE}, {ItemDict.POLLEN_PUFF, ItemDict.WHITE_DYE}, {ItemDict.WHITE_BLOWFISH, ItemDict.WHITE_DYE},
            {ItemDict.BAT_WING, ItemDict.LIGHT_GREY_DYE}, {ItemDict.OYSTER, ItemDict.LIGHT_GREY_DYE},
            {ItemDict.COAL, ItemDict.DARK_GREY_DYE}, {ItemDict.JUNGLE_PIRANHA, ItemDict.DARK_GREY_DYE},
            {ItemDict.WINTERGREEN, ItemDict.MINT_DYE}, 
            {ItemDict.SNOW_CRYSTAL, ItemDict.UN_DYE},
            {ItemDict.SARDINE, ItemDict.OIL }, {ItemDict.STRIPED_BASS, ItemDict.OIL}, {ItemDict.HERRING, ItemDict.OIL}, {ItemDict.MACKEREL, ItemDict.OIL},
            {ItemDict.CATFISH, ItemDict.OIL}, {ItemDict.CARP, ItemDict.OIL}, {ItemDict.SALMON, ItemDict.OIL}, {ItemDict.TUNA, ItemDict.OIL},
        };

        public PEntityPaintersPress(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = PressState.IDLE;
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
            string stateStr = saveState.TryGetData("state", PressState.IDLE.ToString());
            if (stateStr.Equals(PressState.IDLE.ToString()))
            {
                state = PressState.IDLE;
            }
            else if (stateStr.Equals(PressState.WORKING.ToString()))
            {
                state = PressState.WORKING;
            }
            else if (stateStr.Equals(PressState.FINISHED.ToString()))
            {
                state = PressState.FINISHED;
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
                if (state == PressState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == PressState.FINISHED)
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
            if (state == PressState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == PressState.FINISHED)
            {
                return "Empty";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == PressState.FINISHED)
            {
                area.AddEntity(new EntityItem(heldItem, new Vector2(position.X, position.Y - 10)));
                sprite.SetLoop("placement");
                heldItem = ItemDict.NONE;
                state = PressState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == PressState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH) && DYE_MAP.ContainsKey(addedItem))
                {
                    if(addedItem.HasTag(Item.Tag.GOLDEN_CROP) || addedItem.HasTag(Item.Tag.SILVER_CROP) || addedItem.HasTag(Item.Tag.PHANTOM_CROP))
                    {
                        addedItem = ItemDict.GetCropBaseForm(addedItem);
                    }
                    heldItem = addedItem;
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = PressState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't make a dye out of this.", Color.Red));
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
            if (timeRemaining <= 0 && state == PressState.WORKING)
            {
                heldItem = DYE_MAP[heldItem];
                state = PressState.FINISHED;
                sprite.SetLoop("placement");
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == PressState.IDLE || state == PressState.WORKING)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
