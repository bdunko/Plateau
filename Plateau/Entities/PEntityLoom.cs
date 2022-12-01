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
    public class PEntityLoom : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum LoomState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 12 * 60;
        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private LoomState state;
        private ResultHoverBox resultHoverBox;

        public PEntityLoom(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = LoomState.IDLE;
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
            save.AddData("item", heldItem.GetItem().GetName());
            save.AddData("quantity", heldItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", LoomState.IDLE.ToString());
            if (stateStr.Equals(LoomState.IDLE.ToString()))
            {
                state = LoomState.IDLE;
            }
            else if (stateStr.Equals(LoomState.WORKING.ToString()))
            {
                state = LoomState.WORKING;
            }
            else if (stateStr.Equals(LoomState.FINISHED.ToString()))
            {
                state = LoomState.FINISHED;
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
                if (state == LoomState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == LoomState.FINISHED)
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
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
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
            if (state == LoomState.IDLE)
            {
                return "Add Item";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == LoomState.FINISHED)
            {
                return "Empty";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == LoomState.FINISHED)
            {
                sprite.SetLoop("placement");
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                    heldItem = new ItemStack(ItemDict.NONE, 0);
                }
                state = LoomState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == LoomState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH) && (addedItem == ItemDict.WOOL || addedItem == ItemDict.GOLDEN_WOOL || addedItem == ItemDict.FLAX || addedItem == ItemDict.SILVER_FLAX || addedItem == ItemDict.GOLDEN_FLAX ||
                    addedItem == ItemDict.PHANTOM_FLAX || addedItem == ItemDict.COTTON || addedItem == ItemDict.SILVER_COTTON || addedItem == ItemDict.GOLDEN_COTTON || addedItem == ItemDict.PHANTOM_COTTON))
                {
                    heldItem = new ItemStack(addedItem, 1);
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = LoomState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can only use Wool, Flax, or Cotton with a Loom.", Color.Red));
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
            if (timeRemaining <= 0 && state == LoomState.WORKING)
            {
                if(heldItem.GetItem() == ItemDict.WOOL)
                {
                    heldItem = new ItemStack(ItemDict.WOOLEN_CLOTH, 1);
                } else if (heldItem.GetItem() == ItemDict.GOLDEN_WOOL)
                {
                    heldItem = new ItemStack(ItemDict.WOOLEN_CLOTH, Util.RandInt(3, 7));
                } else if (heldItem.GetItem() == ItemDict.COTTON)
                {
                    heldItem = new ItemStack(ItemDict.COTTON_CLOTH, 1);
                } else if (heldItem.GetItem() == ItemDict.SILVER_COTTON)
                {
                    heldItem = new ItemStack(ItemDict.COTTON_CLOTH, 2);
                }
                else if (heldItem.GetItem() == ItemDict.GOLDEN_COTTON)
                {
                    heldItem = new ItemStack(ItemDict.COTTON_CLOTH, 3);
                }
                else if (heldItem.GetItem() == ItemDict.PHANTOM_COTTON)
                {
                    heldItem = new ItemStack(ItemDict.COTTON_CLOTH, Util.RandInt(4, 6));
                }
                else if (heldItem.GetItem() == ItemDict.FLAX)
                {
                    heldItem = new ItemStack(ItemDict.LINEN_CLOTH, 1);
                }
                else if (heldItem.GetItem() == ItemDict.SILVER_FLAX)
                {
                    heldItem = new ItemStack(ItemDict.LINEN_CLOTH, 2);
                }
                else if (heldItem.GetItem() == ItemDict.GOLDEN_FLAX)
                {
                    heldItem = new ItemStack(ItemDict.LINEN_CLOTH, 3);
                }
                else if (heldItem.GetItem() == ItemDict.PHANTOM_FLAX)
                {
                    heldItem = new ItemStack(ItemDict.LINEN_CLOTH, Util.RandInt(4, 6));
                }
                sprite.SetLoop("placement");
                state = LoomState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == LoomState.IDLE || state == LoomState.WORKING)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}


