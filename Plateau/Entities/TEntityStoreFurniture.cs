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
    public class TEntityStoreFurniture : TEntityVendor, ITickDaily
    {
        private AnimatedSprite furnitureItem;

        public TEntityStoreFurniture(Vector2 tilePosition, AnimatedSprite sprite) : base(tilePosition, sprite)
        {
            saleItems.Add(new SaleItem(ItemDict.BAMBOO_POT, 1, Util.GetSaleValue(ItemDict.BAMBOO_POT), new DialogueNode("these are a bamboo pot", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.SURFBOARD, 1, Util.GetSaleValue(ItemDict.SURFBOARD), new DialogueNode("these are a surfboard", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.TARGET, 1, Util.GetSaleValue(ItemDict.TARGET), new DialogueNode("these are a target", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.CANVAS, 1, Util.GetSaleValue(ItemDict.CANVAS), new DialogueNode("these are a canvas", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.GRANDFATHER_CLOCK, 1, Util.GetSaleValue(ItemDict.GRANDFATHER_CLOCK), new DialogueNode("these are a grandfather clock", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.CART, 1, Util.GetSaleValue(ItemDict.CART), new DialogueNode("these are a cart", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.LATTICE, 1, Util.GetSaleValue(ItemDict.LATTICE), new DialogueNode("these are a lattice", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.TARGET, 1, Util.GetSaleValue(ItemDict.TARGET), new DialogueNode("these are a target", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.TELEVISION, 1, Util.GetSaleValue(ItemDict.TELEVISION), new DialogueNode("these are a television", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.SIGNPOST, 1, Util.GetSaleValue(ItemDict.SIGNPOST), new DialogueNode("these are a signpost", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.WOODEN_CHAIR, 1, Util.GetSaleValue(ItemDict.WOODEN_CHAIR), new DialogueNode("these are a woodenchair", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.WOODEN_SQUARETABLE, 1, Util.GetSaleValue(ItemDict.WOODEN_SQUARETABLE), new DialogueNode("these are a wooden squaretable", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.WOODEN_STOOL, 1, Util.GetSaleValue(ItemDict.WOODEN_STOOL), new DialogueNode("these are a wooden stool", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.PIANO, 1, Util.GetSaleValue(ItemDict.PIANO), new DialogueNode("these are a piano", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.GUITAR_PLACEABLE, 1, Util.GetSaleValue(ItemDict.GUITAR_PLACEABLE), new DialogueNode("these are a guitar", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.LANTERN, 1, Util.GetSaleValue(ItemDict.LANTERN), new DialogueNode("these are a lantern", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.WHEELBARROW, 1, Util.GetSaleValue(ItemDict.WHEELBARROW), new DialogueNode("these are a wheelbarrow", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.DECORATIVE_BOULDER, 1, Util.GetSaleValue(ItemDict.DECORATIVE_BOULDER), new DialogueNode("these are a boulder", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.WATER_PUMP, 1, Util.GetSaleValue(ItemDict.WATER_PUMP), new DialogueNode("these are a water pump", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));


            UpdateCurrentSaleItem();
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            UpdateCurrentSaleItem();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if (quantityRemaining != 0)
            {
                Vector2 pos = this.position;
                pos.Y += sprite.GetFrameHeight();
                pos.Y -= furnitureItem.GetFrameHeight();
                    
                furnitureItem.Draw(sb, pos, Color.White);
            }
        }

        private void UpdateCurrentSaleItem()
        {
            currentSaleItem = saleItems[Util.RandInt(0, saleItems.Count - 1)];
            quantityRemaining = currentSaleItem.quantity;
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            Texture2D previewSS = ((PlaceableItem)currentSaleItem.item).GetPreviewTexture();
            furnitureItem = new AnimatedSprite(previewSS, 1, 1, 1, Util.CreateAndFillArray(1, 1000f));
            furnitureItem.SetFrame(0);
        }
    }
}
