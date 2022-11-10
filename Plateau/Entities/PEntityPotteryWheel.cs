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
    public class PEntityPotteryWheel : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum PotteryWheelState
        {
            IDLE, WORKING, FINISHED
        }

        private static Dictionary<int, int> PROCESSING_TIME_BY_CLAY = new Dictionary<int, int> {
            { 1, 30 },
            { 2, 1 * 60 },
            { 4, 2 * 60 },
            { 8, 4 * 60 },
            { 16, 8 * 60 },
            { 32, 16 * 60 },
            { 64, 32 * 60 },
            { 99, 64 * 60 }};
        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private PotteryWheelState state;
        private ResultHoverBox resultHoverBox;

        public PEntityPotteryWheel(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = PotteryWheelState.IDLE;
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
            string stateStr = saveState.TryGetData("state", PotteryWheelState.IDLE.ToString());
            if (stateStr.Equals(PotteryWheelState.IDLE.ToString()))
            {
                state = PotteryWheelState.IDLE;
            }
            else if (stateStr.Equals(PotteryWheelState.WORKING.ToString()))
            {
                state = PotteryWheelState.WORKING;
            }
            else if (stateStr.Equals(PotteryWheelState.FINISHED.ToString()))
            {
                state = PotteryWheelState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == PotteryWheelState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("idle");
                }
            }

            if (state == PotteryWheelState.FINISHED)
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
            if (state == PotteryWheelState.IDLE)
            {
                return "Empty";
            }
            return "";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (state == PotteryWheelState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == PotteryWheelState.IDLE && heldItem.GetItem() != ItemDict.NONE)
            {
                return "Start";
            } else if (state == PotteryWheelState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if(state == PotteryWheelState.IDLE)
            {
                if(heldItem.GetItem() != ItemDict.NONE)
                {
                    timeRemaining = PROCESSING_TIME_BY_CLAY[heldItem.GetQuantity()];
                    state = PotteryWheelState.WORKING;
                    sprite.SetLoop("working");
                }
            }
            else if (state == PotteryWheelState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = PotteryWheelState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == PotteryWheelState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (addedItem == ItemDict.CLAY)
                {
                    if (heldItem.GetItem() == ItemDict.NONE)
                    {
                        heldItem = new ItemStack(ItemDict.CLAY, 1);
                        player.GetHeldItem().Subtract(1);
                    }
                    else
                    {
                        int clayNeeded = (heldItem.GetQuantity() == 64 ? 99 : (heldItem.GetQuantity() * 2)) - heldItem.GetQuantity();
                        if (player.HasItemStack(new ItemStack(ItemDict.CLAY, clayNeeded)))
                        {
                            player.RemoveItemStackFromInventory(new ItemStack(ItemDict.CLAY, clayNeeded));
                            heldItem.Add(clayNeeded);
                            sprite.SetLoop("placement");
                            if (heldItem.GetQuantity() == 99)
                            {
                                InteractRight(player, area, world);
                            }
                        }
                        else
                        {
                            player.AddNotification(new EntityPlayer.Notification("I would need at least " + clayNeeded + " more pieces of Clay if I want to add more.", Color.Red));
                        }
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I should add some Clay to make start making pottery.", Color.Red));
                }
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            if (state == PotteryWheelState.IDLE || state == PotteryWheelState.FINISHED)
            {
                if (heldItem.GetItem() != ItemDict.NONE)
                {
                    for (int i = 0; i < heldItem.GetQuantity(); i++)
                    {
                        area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                    }
                }
                heldItem = new ItemStack(ItemDict.NONE, 0);
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void Tick(int time, EntityPlayer player, Area area, World world)
        {
            timeRemaining -= time;
            if (timeRemaining <= 0 && state == PotteryWheelState.WORKING)
            {
                switch(heldItem.GetQuantity())
                {
                    case 1:
                        heldItem = new ItemStack(ItemDict.CLAY_BALL, 1);
                        break;
                    case 2:
                        heldItem = new ItemStack(ItemDict.POTTERY_MUG, 1);
                        break;
                    case 4:
                        heldItem = new ItemStack(ItemDict.POTTERY_JAR, 1);
                        break;
                    case 8:
                        heldItem = new ItemStack(ItemDict.CLAY_BOWL, 1);
                        break;
                    case 16:
                        heldItem = new ItemStack(ItemDict.POTTERY_PLATE, 1);
                        break;
                    case 32:
                        heldItem = new ItemStack(ItemDict.CLAY_SLATE, 1);
                        break;
                    case 64:
                        heldItem = new ItemStack(ItemDict.POTTERY_VASE, 1);
                        break;
                    case 99:
                        heldItem = new ItemStack(ItemDict.CLAY_DOLL, 1);
                        break;
                }
                sprite.SetLoop("placement");
                state = PotteryWheelState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == PotteryWheelState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
