using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Items;

namespace Plateau.Entities
{
    public class TEntityStoreCompost : TEntityVendor, ITickDaily
    {
        private static string DECAY_LOOP = "decay";
        private static string DEW_LOOP = "dew";
        private static string FROST_LOOP = "frost";
        private static string LOAMY_LOOP = "loamy";
        private static string QUALITY_LOOP = "quality";
        private static string SHINING_LOOP = "shining";
        private static string SWEET_LOOP = "sweet";
        private static string THICK_LOOP = "thick";

        public TEntityStoreCompost(Vector2 tilePosition, AnimatedSprite sprite) : base(tilePosition, sprite)
        {
            saleItems.Add(new SaleItem(ItemDict.DECAY_COMPOST, 8, Util.GetSaleValue(ItemDict.DECAY_COMPOST), new DialogueNode("decay comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.DEW_COMPOST, 8, Util.GetSaleValue(ItemDict.DEW_COMPOST), new DialogueNode("dew comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.FROST_COMPOST, 8, Util.GetSaleValue(ItemDict.FROST_COMPOST), new DialogueNode("frost comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.LOAMY_COMPOST, 8, Util.GetSaleValue(ItemDict.LOAMY_COMPOST), new DialogueNode("loamy comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.QUALITY_COMPOST, 8, Util.GetSaleValue(ItemDict.QUALITY_COMPOST), new DialogueNode("quality comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.SHINING_COMPOST, 8, Util.GetSaleValue(ItemDict.SHINING_COMPOST), new DialogueNode("shining comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.SWEET_COMPOST, 8, Util.GetSaleValue(ItemDict.SWEET_COMPOST), new DialogueNode("sweet comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.THICK_COMPOST, 8, Util.GetSaleValue(ItemDict.THICK_COMPOST), new DialogueNode("thick comp", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            sprite.AddLoop(DECAY_LOOP, 0, 0, false);
            sprite.AddLoop(DEW_LOOP, 1, 1, false);
            sprite.AddLoop(FROST_LOOP, 2, 2, false);
            sprite.AddLoop(LOAMY_LOOP, 3, 3, false);
            sprite.AddLoop(QUALITY_LOOP, 4, 4, false);
            sprite.AddLoop(SHINING_LOOP, 5, 5, false);
            sprite.AddLoop(SWEET_LOOP, 6, 6, false);
            sprite.AddLoop(THICK_LOOP, 7, 7, false);

            RandomizeStock();
        }

        private void RandomizeStock()
        {
            currentSaleItem = saleItems[Util.RandInt(0, saleItems.Count - 1)];
            quantityRemaining = currentSaleItem.quantity;
            ChangeAnimation();
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            RandomizeStock();
        }

        private void ChangeAnimation()
        {
            if (currentSaleItem.item == ItemDict.DECAY_COMPOST)
            {
                sprite.SetLoopIfNot(DECAY_LOOP);
            }
            else if (currentSaleItem.item == ItemDict.DEW_COMPOST)
            {
                sprite.SetLoopIfNot(FROST_LOOP);
            }
            else if (currentSaleItem.item == ItemDict.FROST_COMPOST)
            {
                sprite.SetLoopIfNot(FROST_LOOP);
            }
            else if (currentSaleItem.item == ItemDict.LOAMY_COMPOST)
            {
                sprite.SetLoopIfNot(LOAMY_LOOP);
            }
            else if (currentSaleItem.item == ItemDict.QUALITY_COMPOST)
            {
                sprite.SetLoopIfNot(QUALITY_LOOP);
            }
            else if (currentSaleItem.item == ItemDict.SHINING_COMPOST)
            {
                sprite.SetLoopIfNot(SHINING_LOOP);
            }
            else if (currentSaleItem.item == ItemDict.SWEET_COMPOST)
            {
                sprite.SetLoopIfNot(SWEET_LOOP);
            }
            else if (currentSaleItem.item == ItemDict.THICK_COMPOST)
            {
                sprite.SetLoopIfNot(THICK_LOOP);
            }
        }
    }
}
