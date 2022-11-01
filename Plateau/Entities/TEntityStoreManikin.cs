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
    public class TEntityStoreManikin : TEntityVendor, ITickDaily
    {
        private AnimatedSprite clothesItem;
        private static Item[] SPRING_SUMMER = { ItemDict.HEADBAND, ItemDict.BASEBALL_CAP, ItemDict.WORK_GLOVES, ItemDict.GLASSES, ItemDict.JEANS,
        ItemDict.CHINO_SHORTS, ItemDict.SHORT_SKIRT, ItemDict.SHORT_SLEEVE_TEE, ItemDict.STRIPED_SHIRT, ItemDict.HOODED_SWEATSHIRT, ItemDict.ALL_SEASON_JACKET, ItemDict.SNEAKERS, ItemDict.HIGH_TOPS,
        ItemDict.SHORT_SOCKS};
        private static Item[] FALL_WINTER = { ItemDict.SCRAP_IRON, ItemDict.CAMEL_HAT, ItemDict.WORK_GLOVES, ItemDict.BASEBALL_CAP, ItemDict.GLASSES, ItemDict.JEANS,
        ItemDict.CHINOS, ItemDict.CHINO_SHORTS, ItemDict.LONG_SKIRT, ItemDict.LONG_SLEEVED_TEE, ItemDict.HOODED_SWEATSHIRT, ItemDict.RAINCOAT, ItemDict.HIGH_TOPS, ItemDict.LONG_SOCKS};
        private bool updatedStock;

        public TEntityStoreManikin(Vector2 tilePosition, AnimatedSprite sprite, Area area) : base(tilePosition, sprite)
        {
            saleItems.Add(new SaleItem(ItemDict.SCARF, 1, Util.GetSaleValue(ItemDict.SCARF), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.HEADBAND, 1, Util.GetSaleValue(ItemDict.HEADBAND), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.CAMEL_HAT, 1, Util.GetSaleValue(ItemDict.CAMEL_HAT), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.BASEBALL_CAP, 1, Util.GetSaleValue(ItemDict.BASEBALL_CAP), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.WORK_GLOVES, 1, Util.GetSaleValue(ItemDict.WORK_GLOVES), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.GLASSES, 1, Util.GetSaleValue(ItemDict.GLASSES), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.JEANS, 1, Util.GetSaleValue(ItemDict.JEANS), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.CHINOS, 1, Util.GetSaleValue(ItemDict.CHINOS), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.CHINO_SHORTS, 1, Util.GetSaleValue(ItemDict.CHINO_SHORTS), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.SHORT_SKIRT, 1, Util.GetSaleValue(ItemDict.SHORT_SKIRT), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.LONG_SKIRT, 1, Util.GetSaleValue(ItemDict.LONG_SKIRT), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.SHORT_SLEEVE_TEE, 1, Util.GetSaleValue(ItemDict.SHORT_SLEEVE_TEE), new DialogueNode("these are a white tee", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.LONG_SLEEVED_TEE, 1, Util.GetSaleValue(ItemDict.LONG_SLEEVED_TEE), new DialogueNode("these are a red tee", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.STRIPED_SHIRT, 1, Util.GetSaleValue(ItemDict.STRIPED_SHIRT), new DialogueNode("these are a light grey tee", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.HOODED_SWEATSHIRT, 1, 250, new DialogueNode("these are a blue tee", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.ALL_SEASON_JACKET, 1, 250, new DialogueNode("these are a olive tee", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.RAINCOAT, 1, 250, new DialogueNode("these are a dark grey tee", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.SNEAKERS, 1, Util.GetSaleValue(ItemDict.SNEAKERS), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.HIGH_TOPS, 1, Util.GetSaleValue(ItemDict.HIGH_TOPS), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            saleItems.Add(new SaleItem(ItemDict.SHORT_SOCKS, 1, Util.GetSaleValue(ItemDict.SHORT_SOCKS), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.LONG_SOCKS, 1, Util.GetSaleValue(ItemDict.LONG_SOCKS), new DialogueNode("these are a jeans", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            updatedStock = false;
        }

        public override void Update(float deltaTime, Area area)
        {
            if(!updatedStock)
            {
                UpdateCurrentSaleItem(area.GetWorldSeason());
                updatedStock = true;
            }
            base.Update(deltaTime, area);
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            updatedStock = false;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            base.Draw(sb, layerDepth);
            if (quantityRemaining != 0)
            {
                clothesItem.Draw(sb, this.position - new Vector2(24, 10), Color.White, layerDepth);
            }
        }

        private void UpdateCurrentSaleItem(World.Season season)
        {
            bool found = false;
            while (!found)
            {
                currentSaleItem = saleItems[Util.RandInt(0, saleItems.Count - 1)];
                if(season == World.Season.AUTUMN || season == World.Season.WINTER)
                {
                    found = Util.ArrayContains(FALL_WINTER, currentSaleItem.item);
                } else
                {
                    found = Util.ArrayContains(SPRING_SUMMER, currentSaleItem.item);
                }

            }
            quantityRemaining = currentSaleItem.quantity;
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            Texture2D spriteSheet = ((ClothingItem)currentSaleItem.item).GetSpritesheet();
            clothesItem = new AnimatedSprite(spriteSheet, 108, 11, 10, Util.CreateAndFillArray(108, 1000f));
            clothesItem.SetFrame(2);
        }
    }
}
