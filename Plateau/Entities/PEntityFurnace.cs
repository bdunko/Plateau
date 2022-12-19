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
    public class PEntityFurance : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum FurnaceState
        {
            IDLE, WORKING, FINISHED
        }

        private Dictionary<Item, Item> RESULT_MAP = new Dictionary<Item, Item>()
        {
            {ItemDict.IRON_ORE, ItemDict.IRON_BAR},
            {ItemDict.GOLD_ORE, ItemDict.GOLD_BAR},
            {ItemDict.MYTHRIL_ORE, ItemDict.MYTHRIL_BAR},
            {ItemDict.ADAMANTITE_ORE, ItemDict.ADAMANTITE_BAR},
            {ItemDict.WOOD, ItemDict.COAL},
            {ItemDict.HARDWOOD, ItemDict.COAL}
        };
        private static int PROCESSING_TIME = 22 * 60;
        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private FurnaceState state;
        private ResultHoverBox resultHoverBox;

        public PEntityFurance(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = FurnaceState.IDLE;
            this.timeRemaining = 0;
            this.resultHoverBox = new ResultHoverBox();
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
            string stateStr = saveState.TryGetData("state", FurnaceState.IDLE.ToString());
            if (stateStr.Equals(FurnaceState.IDLE.ToString()))
            {
                state = FurnaceState.IDLE;
            }
            else if (stateStr.Equals(FurnaceState.WORKING.ToString()))
            {
                state = FurnaceState.WORKING;
            }
            else if (stateStr.Equals(FurnaceState.FINISHED.ToString()))
            {
                state = FurnaceState.FINISHED;
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
                if (state == FurnaceState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("idle");
                }
            }

            if (state == FurnaceState.FINISHED)
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
            if (state == FurnaceState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == FurnaceState.IDLE && heldItem.GetItem() != ItemDict.NONE)
            {
                return "Empty";
            }
            else if (state == FurnaceState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == FurnaceState.FINISHED || state == FurnaceState.IDLE)
            {
                if (heldItem.GetItem() != ItemDict.NONE)
                {
                    for (int i = 0; i < heldItem.GetQuantity(); i++)
                    {
                        area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                    }
                    sprite.SetLoop("placement");
                    heldItem = new ItemStack(ItemDict.NONE, 0);
                    state = FurnaceState.IDLE;
                }
            }
        }

        private void startSmelting()
        {
            timeRemaining = PROCESSING_TIME;
            state = FurnaceState.WORKING;
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == FurnaceState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if(addedItem == ItemDict.COAL && heldItem.GetQuantity() == 3)
                {
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    startSmelting();
                } else if (addedItem == ItemDict.COAL && heldItem.GetQuantity() != 3)
                {
                    player.AddNotification(new EntityPlayer.Notification("I should add my ores before the coal.", Color.Red));
                }
                else if (RESULT_MAP.ContainsKey(addedItem) && heldItem.GetItem() == ItemDict.NONE && player.HasItemStack(new ItemStack(addedItem, 3)))
                {
                    player.RemoveItemStackFromInventory(new ItemStack(addedItem, 3));
                    heldItem = new ItemStack(addedItem, 3);
                    sprite.SetLoop("placement");

                    if (heldItem.GetItem() == ItemDict.WOOD || heldItem.GetItem() == ItemDict.HARDWOOD)
                        startSmelting();
                }
                else
                {
                    if(addedItem == ItemDict.SCRAP_IRON)
                    {
                        player.AddNotification(new EntityPlayer.Notification("Scrap iron is too low quality to smelt.", Color.Red));
                    }
                    if (heldItem.GetQuantity() == 3)
                    {
                        player.AddNotification(new EntityPlayer.Notification("It's already full; now I need to add some coal.", Color.Red));
                    }
                    else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I need to add 3 of the same type of ore to smelt a bar.", Color.Red));
                    }
                }
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            if (state == FurnaceState.IDLE || state == FurnaceState.FINISHED)
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
            if (timeRemaining <= 0 && state == FurnaceState.WORKING)
            {
                int resultQuantity = 1;
                if(heldItem.GetItem() == ItemDict.WOOD)
                {
                    resultQuantity = 2;
                } else if (heldItem.GetItem() == ItemDict.HARDWOOD)
                {
                    resultQuantity = 3;
                } 
                heldItem = new ItemStack(RESULT_MAP[heldItem.GetItem()], resultQuantity);
                sprite.SetLoop("placement");
                state = FurnaceState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == FurnaceState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
