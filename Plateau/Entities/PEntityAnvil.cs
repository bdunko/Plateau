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
    public class PEntityAnvil : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum AnvilState
        {
            IDLE, WORKING, FINISHED
        }

        private class IOSet
        {
            public Item inputItem;
            public Item outputItem;
            public Item payment;

            public IOSet(Item input, Item output, Item payment)
            {
                this.inputItem = input;
                this.outputItem = output;
                this.payment = payment;
            }
        }

        private static List<IOSet> IO_PAIRS = null;

        private static int BAR_QUANTITY = 5;
        private static int TOOL_BAR_QUANTITY = 3;
        private static float TIME_BETWEEN_ANIM_WHILE_IDLE = 5.0f;
        private float idleAnimTimer;
        private static int PROCESSING_TIME = 60 * 3 * 23;
        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private AnvilState state;
        private ResultHoverBox resultHoverBox;

        public PEntityAnvil(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("idleAnim", 4, 11, false);
            sprite.AddLoop("working", 12, 15, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = AnvilState.IDLE;
            this.timeRemaining = 0;
            this.idleAnimTimer = 0;
            this.resultHoverBox = new ResultHoverBox();
            if (IO_PAIRS == null)
            {
                IO_PAIRS = new List<IOSet> {
                    new IOSet(ItemDict.IRON_BAR, ItemDict.HORSESHOE, ItemDict.NONE),
                    new IOSet(ItemDict.MYTHRIL_BAR, ItemDict.HERALDIC_SHIELD, ItemDict.NONE),
                    new IOSet(ItemDict.GOLD_BAR, ItemDict.DECORATIVE_SWORD, ItemDict.NONE),
                    new IOSet(ItemDict.ADAMANTITE_BAR, ItemDict.SUIT_OF_ARMOR, ItemDict.NONE),
                    new IOSet(ItemDict.HOE, ItemDict.IRON_HOE, ItemDict.IRON_BAR),
                    new IOSet(ItemDict.IRON_HOE, ItemDict.MITHRIL_HOE, ItemDict.MYTHRIL_BAR),
                    new IOSet(ItemDict.MITHRIL_HOE, ItemDict.ADAMANTITE_HOE, ItemDict.ADAMANTITE_BAR),
                    new IOSet(ItemDict.AXE, ItemDict.IRON_AXE, ItemDict.IRON_BAR),
                    new IOSet(ItemDict.IRON_AXE, ItemDict.MITHRIL_AXE, ItemDict.MYTHRIL_BAR),
                    new IOSet(ItemDict.MITHRIL_AXE, ItemDict.ADAMANTITE_AXE, ItemDict.ADAMANTITE_BAR),
                    new IOSet(ItemDict.PICKAXE, ItemDict.IRON_PICKAXE, ItemDict.IRON_BAR),
                    new IOSet(ItemDict.IRON_PICKAXE, ItemDict.MITHRIL_PICKAXE, ItemDict.MYTHRIL_BAR),
                    new IOSet(ItemDict.MITHRIL_PICKAXE, ItemDict.ADAMANTITE_PICKAXE, ItemDict.ADAMANTITE_BAR),
                    new IOSet(ItemDict.WATERING_CAN, ItemDict.IRON_CAN, ItemDict.IRON_BAR),
                    new IOSet(ItemDict.IRON_CAN, ItemDict.MITHRIL_CAN, ItemDict.MYTHRIL_BAR),
                    new IOSet(ItemDict.MITHRIL_CAN, ItemDict.ADAMANTITE_CAN, ItemDict.ADAMANTITE_BAR),
                    new IOSet(ItemDict.FISHING_ROD, ItemDict.IRON_ROD, ItemDict.IRON_BAR),
                    new IOSet(ItemDict.IRON_ROD, ItemDict.MITHRIL_ROD, ItemDict.MYTHRIL_BAR),
                    new IOSet(ItemDict.MITHRIL_ROD, ItemDict.ADAMANTITE_ROD, ItemDict.ADAMANTITE_BAR),
                };
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y));
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
            string stateStr = saveState.TryGetData("state", AnvilState.IDLE.ToString());
            if (stateStr.Equals(AnvilState.IDLE.ToString()))
            {
                state = AnvilState.IDLE;
            }
            else if (stateStr.Equals(AnvilState.WORKING.ToString()))
            {
                state = AnvilState.WORKING;
            }
            else if (stateStr.Equals(AnvilState.FINISHED.ToString()))
            {
                state = AnvilState.FINISHED;
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
                if (state == AnvilState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                    idleAnimTimer = 0;
                }
                else
                {
                    if (idleAnimTimer > TIME_BETWEEN_ANIM_WHILE_IDLE)
                    {
                        sprite.SetLoopIfNot("idleAnim");
                        idleAnimTimer = 0;
                    }
                }
            }

            if (state == AnvilState.FINISHED)
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
                if(heldItem.GetItem().HasTag(Item.Tag.TOOL) && state != AnvilState.FINISHED)
                {
                    foreach (IOSet set in IO_PAIRS)
                    {
                        if (set.inputItem == heldItem.GetItem())
                        {
                            for(int i = 0; i < TOOL_BAR_QUANTITY; i++)
                            {
                                area.AddEntity(new EntityItem(set.payment, new Vector2(position.X, position.Y - 10)));
                            }
                        }
                    }
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
            if (state == AnvilState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == AnvilState.FINISHED)
            {
                return "Collect";
            } else if (state == AnvilState.IDLE)
            {
                return "Add Tool";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == AnvilState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = AnvilState.IDLE;
            } else if (state == AnvilState.IDLE)
            {
                IOSet setForInput = null;
                Item addedItem = player.GetHeldItem().GetItem();
                foreach (IOSet set in IO_PAIRS)
                {
                    if (set.inputItem == addedItem)
                    {
                        setForInput = set;
                    }
                }
                if (setForInput != null)
                {
                    if (setForInput.inputItem.HasTag(Item.Tag.TOOL))
                    {
                        if (player.HasItemStack(new ItemStack(setForInput.payment, TOOL_BAR_QUANTITY)))
                        {
                            player.RemoveItemStackFromInventory(new ItemStack(setForInput.payment, TOOL_BAR_QUANTITY));
                            player.RemoveItemStackFromInventory(new ItemStack(setForInput.inputItem, 1));
                            heldItem = new ItemStack(setForInput.inputItem, 1);
                            sprite.SetLoop("placement");
                            state = AnvilState.WORKING;
                            timeRemaining = PROCESSING_TIME;
                        }
                        else
                        {
                            player.AddNotification(new EntityPlayer.Notification("I need " + TOOL_BAR_QUANTITY + " bars of metal to do that.", Color.Red));
                        }
                    }
                    else
                    {
                        player.AddNotification(new EntityPlayer.Notification("This item isn't a tool!", Color.Red));
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't make anything on an Anvil using this.", Color.Red));
                }
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == AnvilState.IDLE)
            {
                IOSet setForInput = null;
                Item addedItem = player.GetHeldItem().GetItem();
                foreach (IOSet set in IO_PAIRS)
                {
                    if (set.inputItem == addedItem)
                    {
                        setForInput = set;
                    }
                }
                if (setForInput != null)
                {
                    if (heldItem.GetItem().HasTag(Item.Tag.TOOL))
                    {
                        player.AddNotification(new EntityPlayer.Notification("Use right click when interacting with the anvil to upgrade tools.", Color.Red));
                    }
                    else
                    {
                        if (player.HasItemStack(new ItemStack(setForInput.inputItem, BAR_QUANTITY)))
                        {
                            player.RemoveItemStackFromInventory(new ItemStack(setForInput.inputItem, BAR_QUANTITY));
                            heldItem = new ItemStack(setForInput.inputItem, BAR_QUANTITY);
                            sprite.SetLoop("placement");
                            state = AnvilState.WORKING;
                            timeRemaining = PROCESSING_TIME;
                        }
                        else
                        {
                            player.AddNotification(new EntityPlayer.Notification("I need " + BAR_QUANTITY + " bars of metal to do that.", Color.Red));
                        }
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't make anything on an Anvil using this.", Color.Red));
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
            if (timeRemaining <= 0 && state == AnvilState.WORKING)
            {
                foreach (IOSet set in IO_PAIRS)
                {
                    if (set.inputItem == heldItem.GetItem())
                    {
                        heldItem = new ItemStack(set.outputItem, 1);
                        break;
                    }
                }
                sprite.SetLoop("placement");
                state = AnvilState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == AnvilState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
