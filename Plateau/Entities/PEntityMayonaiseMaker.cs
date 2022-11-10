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
    public class PEntityMayonaiseMaker : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum MMState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 6 * 60;
        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private MMState state;
        private ResultHoverBox resultHoverBox;

        public PEntityMayonaiseMaker(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 5, 6, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            sprite.AddLoop("finished", 4, 4, true);
            this.state = MMState.IDLE;
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
            string stateStr = saveState.TryGetData("state", MMState.IDLE.ToString());
            if (stateStr.Equals(MMState.IDLE.ToString()))
            {
                state = MMState.IDLE;
            }
            else if (stateStr.Equals(MMState.WORKING.ToString()))
            {
                state = MMState.WORKING;
            }
            else if (stateStr.Equals(MMState.FINISHED.ToString()))
            {
                state = MMState.FINISHED;
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
                if (state == MMState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                } else if (state == MMState.FINISHED)
                {
                    sprite.SetLoopIfNot("finished");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == MMState.FINISHED)
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
            if (state == MMState.IDLE)
            {
                return "Add Item";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == MMState.FINISHED)
            {
                return "Empty";
            }
            return "";
        }


        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == MMState.FINISHED)
            {
                for(int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = MMState.IDLE;
                sprite.SetLoop("placement");
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == MMState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH) && (addedItem == ItemDict.EGG || addedItem == ItemDict.GOLDEN_EGG))
                {
                    heldItem = new ItemStack(addedItem, 1);
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = MMState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can only add Eggs to a Mayonaise Maker.", Color.Red));
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
            if (timeRemaining <= 0 && state == MMState.WORKING)
            {
                if(heldItem.GetItem() == ItemDict.EGG)
                {
                    heldItem = new ItemStack(ItemDict.MAYONNAISE, 1);
                } else if (heldItem.GetItem() == ItemDict.GOLDEN_EGG)
                {
                    heldItem = new ItemStack(ItemDict.MAYONNAISE, Util.RandInt(3, 6));
                }
                sprite.SetLoop("placement");
                state = MMState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == MMState.IDLE || state == MMState.WORKING)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}