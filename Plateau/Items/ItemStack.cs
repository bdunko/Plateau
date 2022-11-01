using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Items
{
    public class ItemStack
    { 
        private int quantity;
        private Item item;

        public ItemStack(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        } 

        public int Add(int addAmount)
        {
            int overflow = 0;
            quantity += addAmount;
            if(quantity > item.GetStackCapacity())
            {
                overflow = quantity - item.GetStackCapacity();
                quantity = item.GetStackCapacity();
            }
            return overflow;
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public int Subtract(int subAmount)
        {
            int underflow = 0;
            quantity -= subAmount;
            if(quantity < 0)
            {
                underflow = -quantity;
                quantity = 0;
            }
            if(quantity == 0)
            {
                item = ItemDict.NONE;
            }
            return underflow;
        }

        public int GetMaxQuantity()
        {
            return item.GetStackCapacity();
        }

        public Item GetItem()
        {
            return item;
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public bool IsFull()
        {
            return quantity >= item.GetStackCapacity();
        }

    }
}
