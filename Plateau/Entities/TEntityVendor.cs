using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Entities;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public abstract class TEntityVendor : TileEntity, IInteract, IHaveHoveringInterface
    {
        protected class SaleItem
        {
            public static SaleItem NONE = new SaleItem(ItemDict.NONE, 0, 1, null);

            public int quantity;
            public DialogueNode examineDialogue;
            public Item item;
            public int cost;

            public SaleItem(Item item, int quantity, int cost, DialogueNode examineDialogue)
            {
                this.item = item;
                this.quantity = quantity;
                this.examineDialogue = examineDialogue;
                this.cost = cost;
            }
        }

        protected AnimatedSprite sprite;
        protected SaleItem currentSaleItem;
        protected List<SaleItem> saleItems;
        protected int quantityRemaining;

        public TEntityVendor(Vector2 tilePosition, AnimatedSprite sprite) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.sprite = sprite;
            this.currentSaleItem = SaleItem.NONE;
            quantityRemaining = 0;
            saleItems = new List<SaleItem>();
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, this.position + new Vector2(0, 1), Color.White);
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
        }

        public virtual string GetLeftClickAction(EntityPlayer player)
        {
            return "";
            
        }

        public virtual string GetLeftShiftClickAction(EntityPlayer player)
        {
            if (currentSaleItem != SaleItem.NONE)
            {
                return "Buy 1";
            }
            return "";
        }

        public virtual string GetRightClickAction(EntityPlayer player)
        {
            if (currentSaleItem != SaleItem.NONE)
            {
                return "Examine";
            }
            return "";
            
        }

        public virtual string GetRightShiftClickAction(EntityPlayer player)
        {
            if (currentSaleItem != SaleItem.NONE)
            {
                return "Buy 5";
            }
            return "";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public virtual void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            if (currentSaleItem != SaleItem.NONE && quantityRemaining > 0)
            {
                if (player.GetGold() >= currentSaleItem.cost)
                {
                    player.SpendGold(currentSaleItem.cost);
                    area.AddEntity(new EntityItem(currentSaleItem.item, new Vector2(position.X, position.Y - 10)));
                    quantityRemaining--;
                    if (quantityRemaining == 0)
                    {
                        currentSaleItem = SaleItem.NONE;
                    }
                } else
                {
                    player.AddNotification(new EntityPlayer.Notification("Not enough gold!", Color.Red, EntityPlayer.Notification.Length.SHORT));
                }
            }
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (currentSaleItem != SaleItem.NONE)
            {
                player.SetCurrentDialogue(currentSaleItem.examineDialogue);
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            for(int i = 0; i < 5; i++)
            {
                InteractLeftShift(player, area, world);
            }
        }

        public virtual HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if(currentSaleItem != SaleItem.NONE)
            {
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.TextElement(currentSaleItem.item.GetName())), 
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(currentSaleItem.item)),
                    new HoveringInterface.Row(
                        new HoveringInterface.TextElement("Price: " + currentSaleItem.cost.ToString() + "G")),
                    new HoveringInterface.Row(
                        new HoveringInterface.TextElement("Stock: " + quantityRemaining))); 
            }
            return new HoveringInterface(
                new HoveringInterface.Row(
                    new HoveringInterface.TextElement("Sold Out!")));
        }
    }
}
