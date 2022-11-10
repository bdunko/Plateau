using Microsoft.Xna.Framework;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class TEntityShippingBin : TileEntity, IInteract, IPersist, ITickDaily, IHaveHoveringInterface, IHaveID
    {
        private List<Item> shippedItems;
        private int binValue;
        public static int TOTAL_VALUE;
        private string id;
        private AnimatedSprite sprite;

        public TEntityShippingBin(String id, Vector2 tilePosition, AnimatedSprite sprite) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.id = id;
            this.sprite = sprite;
            TOTAL_VALUE = 0;
            binValue = 0;
            shippedItems = new List<Item>();
            sprite.AddLoop("still", 0, 0, true);
            sprite.AddLoop("anim", 1, 2, false);
            sprite.SetLoop("still");
        }

        public override SaveState GenerateSave()
        {
            SaveState save = new SaveState(SaveState.Identifier.SHIPPING_BIN);
            save.AddData("id", GetID());
            save.AddData("numItems", shippedItems.Count.ToString());
            for(int i = 0; i < shippedItems.Count; i++)
            {
                save.AddData("item" + i, shippedItems[i].GetName());
            }
            return save;
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if(sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("still");
            }
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, this.position + new Vector2(0, 1), Color.White, layerDepth);
        }

        public override void LoadSave(SaveState state)
        {
            int numItems = Int32.Parse(state.TryGetData("numItems", "0"));
            for(int i = 0; i < numItems; i++)
            {
                Item loaded = ItemDict.GetItemByName(state.TryGetData("item" + i, "ERROR"));
                TOTAL_VALUE += loaded.GetValue();
                binValue += loaded.GetValue();
                shippedItems.Add(loaded);
            }
        }

        public virtual string GetLeftClickAction(EntityPlayer player)
        {
            return "Ship";
        }

        public virtual string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "Ship Stack";
        }

        public virtual string GetRightClickAction(EntityPlayer player)
        {
            return "Empty";
        }

        public virtual string GetRightShiftClickAction(EntityPlayer player)
        {
            return "Ship All";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            ItemStack selected = player.GetHeldItem();
            if(!selected.GetItem().HasTag(Item.Tag.NO_TRASH))
            {
                shippedItems.Add(selected.GetItem());
                TOTAL_VALUE += selected.GetItem().GetValue();
                binValue += selected.GetItem().GetValue();
                if (TOTAL_VALUE > EntityPlayer.MAX_GOLD)
                {
                    TOTAL_VALUE = EntityPlayer.MAX_GOLD;
                }
                if(binValue > EntityPlayer.MAX_GOLD)
                {
                    binValue = EntityPlayer.MAX_GOLD;
                }
                player.GetHeldItem().Subtract(1);
                sprite.SetLoop("anim");
            } else if (selected.GetItem() != ItemDict.NONE)
            {
                player.AddNotification(new EntityPlayer.Notification("I can't sell that.", Color.Red));
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            ItemStack selected = player.GetHeldItem();
            if (!selected.GetItem().HasTag(Item.Tag.NO_TRASH))
            {
                while(selected.GetQuantity() != 0) { 
                    shippedItems.Add(selected.GetItem());
                    TOTAL_VALUE += selected.GetItem().GetValue();
                    binValue += selected.GetItem().GetValue();
                    if(TOTAL_VALUE > EntityPlayer.MAX_GOLD)
                    {
                        TOTAL_VALUE = EntityPlayer.MAX_GOLD;
                    }
                    if(binValue > EntityPlayer.MAX_GOLD)
                    {
                        binValue = EntityPlayer.MAX_GOLD;
                    }
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("anim");
                }
            }
            else if (selected.GetItem() != ItemDict.NONE)
            {
                player.AddNotification(new EntityPlayer.Notification("I can't sell that.", Color.Red));
            }
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (shippedItems.Count != 0)
            {
                foreach (Item item in shippedItems)
                {
                    TOTAL_VALUE -= item.GetValue();
                    binValue -= item.GetValue();
                    area.AddEntity(new EntityItem(item, new Vector2(position.X, position.Y - 10)));
                    sprite.SetLoop("anim");
                }
                shippedItems.Clear();
            } else
            {
                player.AddNotification(new EntityPlayer.Notification("The bin is empty.", Color.Red));
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            ItemStack selected = player.GetHeldItem();
            if (!selected.GetItem().HasTag(Item.Tag.NO_TRASH))
            {
                Item shipAllOf = selected.GetItem();
                while (player.RemoveItemFromInventory(shipAllOf))
                {
                    shippedItems.Add(shipAllOf);
                    TOTAL_VALUE += shipAllOf.GetValue();
                    binValue -= shipAllOf.GetValue();
                    if (TOTAL_VALUE > EntityPlayer.MAX_GOLD)
                    {
                        TOTAL_VALUE = EntityPlayer.MAX_GOLD;
                    }
                    if(binValue > EntityPlayer.MAX_GOLD)
                    {
                        binValue = EntityPlayer.MAX_GOLD;
                    }
                    sprite.SetLoop("anim");
                }
            }
            else if (selected.GetItem() != ItemDict.NONE)
            {
                player.AddNotification(new EntityPlayer.Notification("I can't sell that.", Color.Red));
            }
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            player.GainGold(binValue);
            GameState.STATISTICS[GameState.STAT_SHIPPED_VALUE] += binValue;
            shippedItems.Clear();
            TOTAL_VALUE = 0;
            binValue = 0;
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            ItemStack recent = new ItemStack(ItemDict.NONE, 0);
            if(shippedItems.Count > 0)
            {
                recent = new ItemStack(shippedItems[shippedItems.Count - 1], 1);
            } 
            return new HoveringInterface(
                new HoveringInterface.Row(
                    new HoveringInterface.TextElement("Shipment Bin")),
                new HoveringInterface.Row(
                    new HoveringInterface.TextElement("Total Value: " + binValue)),
                new HoveringInterface.Row(
                    new HoveringInterface.ItemStackElement(recent)));
        }

        public override bool ShouldBeSaved()
        {
            return true;
        }

        public string GetID()
        {
            return this.id;
        }
    }
}
