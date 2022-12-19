using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class ResultHoverBox
    {
        private static float TIME_BETWEEN_Y_CHANGE = 0.8f;
        private float yMod;
        private float timer;
        private ItemStack itemStack;

        public ResultHoverBox()
        {
            this.timer = 0;
            this.yMod = 0;
            this.itemStack = new ItemStack(ItemDict.NONE, 0);
        }

        public void AssignItemStack(Item item)
        {
            this.itemStack = new ItemStack(item, 1);
        }

        public void AssignItemStack(ItemStack stack)
        {
            this.itemStack = stack;
        }

        public void RemoveItemStack()
        {
            this.itemStack = new ItemStack(ItemDict.NONE, 0);
        }

        public void Update(float deltaTime)
        {
            timer += deltaTime;
            if(timer > TIME_BETWEEN_Y_CHANGE)
            {
                timer = 0;
                if(yMod == 1)
                {
                    yMod = -1;
                } else
                {
                    yMod = 1;
                }
            }
        }

        //position is the upper middle spot of placeable item...
        public void Draw(SpriteBatch sb, Vector2 position)
        {
            if (itemStack.GetItem() != ItemDict.NONE)
            {
                Vector2 drawPos = new Vector2(position.X - (0.5f*GameplayInterface.hoveringItemBox.Width), position.Y - GameplayInterface.hoveringItemBox.Height - 4 + yMod);

                sb.Draw(GameplayInterface.hoveringItemBox, drawPos, Color.White * 0.9f);

                drawPos += new Vector2(2, 2);
                itemStack.GetItem().Draw(sb, drawPos, Color.White);
                if (itemStack.GetItem().GetStackCapacity() != 1 && itemStack.GetQuantity() > 1)
                {
                    Vector2 itemQuantityPosition = new Vector2(drawPos.X + 11, drawPos.Y + 9);
                    sb.Draw(GameplayInterface.numbers[itemStack.GetQuantity() % 10], itemQuantityPosition, Color.White);
                    if (itemStack.GetQuantity() >= 10)
                    {
                        itemQuantityPosition.X -= 4;
                        sb.Draw(GameplayInterface.numbers[itemStack.GetQuantity() / 10], itemQuantityPosition, Color.White);
                    }
                }
            }
        }

    }
}
