using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;

namespace Plateau.Entities
{
    public class HoveringInterface
    {
        public abstract class Element
        {
            public abstract void Draw(SpriteBatch sb, Vector2 position, float layerDepth, float opacity, RectangleF cameraBoundingBox);
            public abstract Vector2 GetSize();
        }

        public class ItemStackElement : Element
        {
            private ItemStack itemStack;

            public ItemStackElement(Item item)
            {
                this.itemStack = new ItemStack(item, 1);
            }

            public ItemStackElement(ItemStack itemStack)
            {
                this.itemStack = itemStack;
            }

            public override void Draw(SpriteBatch sb, Vector2 position, float layerDepth, float opacity, RectangleF cameraBoundingBox)
            {
                sb.Draw(GameplayInterface.itemBox, position, Color.White * 0.9f * opacity);
                position += new Vector2(2, 2);
                if (itemStack.GetItem() != ItemDict.NONE)
                {
                    itemStack.GetItem().Draw(sb, position, Color.White * opacity);
                    if (itemStack.GetItem().GetStackCapacity() != 1 && itemStack.GetQuantity() > 1)
                    {
                        Vector2 itemQuantityPosition = new Vector2(position.X + 11, position.Y + 9);
                        sb.Draw(GameplayInterface.numbers[itemStack.GetQuantity() % 10], itemQuantityPosition, Color.White * opacity);
                        if (itemStack.GetQuantity() >= 10)
                        {
                            itemQuantityPosition.X -= 4;
                            sb.Draw(GameplayInterface.numbers[itemStack.GetQuantity() / 10], itemQuantityPosition, Color.White * opacity);
                        }
                    }
                }
            }

            public override Vector2 GetSize()
            {
                return new Vector2(GameplayInterface.itemBox.Width, GameplayInterface.itemBox.Height+2);
            }
        }

        public class TextElement : Element
        {
            private string text;

            public TextElement(string text)
            {
                this.text = text;
            }

            public override void Draw(SpriteBatch sb, Vector2 position, float layerDepth, float opacity, RectangleF cameraBoundingBox)
            {
                GameplayInterface.WHITE_9SLICE.DrawString(sb, text, position, cameraBoundingBox, Color.White * opacity, Color.Black * opacity);
                //GameplayInterface.QUEUED_STRINGS.Add(new QueuedString(text, position, Color.Black * opacity));
            }

            public override Vector2 GetSize()
            {
                return PlateauMain.FONT.MeasureString(text) * PlateauMain.FONT_SCALE;
            }
        }

        public class Row
        {
            public List<Element> elementList;
            private static float ELEMENT_SPACING = 1f;

            public Row(params Element[] elements)
            {
                elementList = new List<Element>();
                foreach(Element element in elements)
                {
                    elementList.Add(element);
                }
            }

            public void AddElement(Element element)
            {
                elementList.Add(element);
            }

            public Vector2 GetSize()
            {
                float height = 0;
                float width = 0;
                foreach(Element element in elementList)
                {
                    Vector2 elementSize = element.GetSize();
                    if(elementSize.Y > height)
                    {
                        height = elementSize.Y;
                    }
                    width += elementSize.X;
                    width += ELEMENT_SPACING;
                }
                width -= ELEMENT_SPACING;
                return new Vector2(width, height);
            }

            public void Draw(SpriteBatch sb, Vector2 position, float layerDepth, float opacity, RectangleF cameraBoundingBox)
            {
                float xOffset = 0;
                Vector2 rowSize = GetSize();
                foreach(Element element in elementList)
                {
                    Vector2 elementSize = element.GetSize();

                    element.Draw(sb, position + new Vector2(xOffset, (rowSize.Y-elementSize.Y)/2), layerDepth, opacity, cameraBoundingBox);

                    xOffset += elementSize.X;
                    xOffset += ELEMENT_SPACING;
                }
            }
        }

        private static float ROW_SPACING = 0.55f;
        private List<Row> rowList;

        public HoveringInterface(params Row[] rows)
        {
            rowList = new List<Row>();
            foreach(Row row in rows)
            {
                rowList.Add(row);
            }
        }

        public Vector2 GetSize()
        {
            float width = 0;
            float height = 0;

            foreach(Row row in rowList)
            {
                Vector2 rowSize = row.GetSize();
                if(rowSize.X > width)
                {
                    width = rowSize.X;
                }

                height += rowSize.Y;
                height += ROW_SPACING;
            }

            height -= ROW_SPACING;

            return new Vector2(width, height);
        }

        public void AddRow(Row row)
        {
            rowList.Add(row);
        }

        public void Draw(SpriteBatch sb, Vector2 position, float opacity, RectangleF cameraBoundingBox)
        {
            float yOffset = 0;
            Vector2 interfaceSize = GetSize();
            foreach (Row row in rowList)
            {
                Vector2 rowSize = row.GetSize();

                row.Draw(sb, position + new Vector2((interfaceSize.X - rowSize.X) / 2, yOffset), 1.0f, opacity, cameraBoundingBox);

                yOffset += rowSize.Y;
                yOffset += ROW_SPACING;
            }
        }
    }
}
