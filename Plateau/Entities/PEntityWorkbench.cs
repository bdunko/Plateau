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
    public class PEntityWorkbench : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum WorkbenchState
        {
            IDLE, WORKING, FINISHED
        }

        private class QuantifiedIOSet
        {
            public Item inputItem;
            public Item outputItem;
            public int quantity;

            public QuantifiedIOSet(Item input, Item output, int quantity)
            {
                this.inputItem = input;
                this.outputItem = output;
                this.quantity = quantity;
            }
        }

        private static List<QuantifiedIOSet> IO_PAIRS = null;

        private static float TIME_BETWEEN_ANIM_WHILE_IDLE = 5.0f;
        private float idleAnimTimer;
        private static int PROCESSING_TIME = 60;
        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private WorkbenchState state;
        private ResultHoverBox resultHoverBox;

        public PEntityWorkbench(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("idleAnim", 4, 7, false);
            sprite.AddLoop("working", 7, 10, true);
            sprite.AddLoop("placement", 0, 3, false); 
            sprite.SetLoop("placement");
            this.state = WorkbenchState.IDLE;
            this.timeRemaining = 0;
            this.idleAnimTimer = 0;
            this.resultHoverBox = new ResultHoverBox();
            if(IO_PAIRS == null)
            {
                IO_PAIRS = new List<QuantifiedIOSet> {
                    new QuantifiedIOSet(ItemDict.WOOD, ItemDict.BOARD, 5),
                    new QuantifiedIOSet(ItemDict.HARDWOOD, ItemDict.PLANK, 1),
                    new QuantifiedIOSet(ItemDict.STONE, ItemDict.BRICKS, 5),
                    new QuantifiedIOSet(ItemDict.BAMBOO, ItemDict.PAPER, 5),
                    new QuantifiedIOSet(ItemDict.IRON_BAR, ItemDict.GEARS, 1)};
            }
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
            string stateStr = saveState.TryGetData("state", WorkbenchState.IDLE.ToString());
            if (stateStr.Equals(WorkbenchState.IDLE.ToString()))
            {
                state = WorkbenchState.IDLE;
            }
            else if (stateStr.Equals(WorkbenchState.WORKING.ToString()))
            {
                state = WorkbenchState.WORKING;
            }
            else if (stateStr.Equals(WorkbenchState.FINISHED.ToString()))
            {
                state = WorkbenchState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            idleAnimTimer += deltaTime;
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == WorkbenchState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                    idleAnimTimer = 0;
                }
                else
                {
                    if(idleAnimTimer > TIME_BETWEEN_ANIM_WHILE_IDLE)
                    {
                        sprite.SetLoopIfNot("idleAnim");
                        idleAnimTimer = 0;
                    }
                }
            }

            if (state == WorkbenchState.FINISHED)
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
            if (state == WorkbenchState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == WorkbenchState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == WorkbenchState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = WorkbenchState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == WorkbenchState.IDLE)
            {
                QuantifiedIOSet setForInput = null;
                Item addedItem = player.GetHeldItem().GetItem();
                foreach(QuantifiedIOSet set in IO_PAIRS)
                {
                    if(set.inputItem == addedItem)
                    {
                        setForInput = set;
                    }
                }
                if(setForInput != null)
                {
                    if(player.HasItemStack(new ItemStack(setForInput.inputItem, setForInput.quantity)))
                    {
                        player.RemoveItemStackFromInventory(new ItemStack(setForInput.inputItem, setForInput.quantity));
                        heldItem = new ItemStack(setForInput.inputItem, setForInput.quantity);
                        sprite.SetLoop("placement");
                        state = WorkbenchState.WORKING;
                        timeRemaining = PROCESSING_TIME;
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification(setForInput.outputItem.GetName() + " is made in sets of " + setForInput.quantity + " at a time;\nI need at least that many " + setForInput.inputItem.GetName() + " to do this.", Color.Red));
                    }
                } else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't make anything on the Workbench using this.", Color.Red));
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
            if (timeRemaining <= 0 && state == WorkbenchState.WORKING)
            {
                foreach (QuantifiedIOSet set in IO_PAIRS)
                {
                    if (set.inputItem == heldItem.GetItem())
                    {
                        heldItem = new ItemStack(set.outputItem, set.quantity);
                        break;
                    }
                }
                sprite.SetLoop("placement");
                state = WorkbenchState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == WorkbenchState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
