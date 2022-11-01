using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class TEntityStoreShelf : TEntityVendor, ITickDaily
    {
        private static string LOOP_EMPTY = "empty";
        private static string LOOP_BEET = "beet";
        private static string LOOP_FLAX = "flax";
        private static string LOOP_BELLPEPPER = "bellpepper";
        private static string LOOP_BROCCOLI = "broccoli";
        private static string LOOP_CABBAGE = "cabbage";
        private static string LOOP_PUMPKIN = "pumpkin";
        private static string LOOP_SPINACH = "spinach";
        private static string LOOP_POTATO = "potato";
        private static string LOOP_STRAWBERRY = "strawberry";
        private static string LOOP_CARROT = "carrot";
        private static string LOOP_CACTUS = "cactus";
        private static string LOOP_ONION = "onion";
        private static string LOOP_COTTON = "cotton";
        private static string LOOP_CUCUMBER = "cucumber";
        private static string LOOP_EGGPLANT = "eggplant";
        private static string LOOP_TOMATO = "tomato";
        private static string LOOP_WATERMELON = "watermelon";

        private Item[] SPRING = { ItemDict.SPINACH_SEEDS, ItemDict.POTATO_SEEDS, ItemDict.STRAWBERRY_SEEDS, ItemDict.CARROT_SEEDS};
        private Item[] SUMMER = { ItemDict.CACTUS_SEEDS, ItemDict.ONION_SEEDS, ItemDict.COTTON_SEEDS, ItemDict.CUCUMBER_SEEDS, ItemDict.EGGPLANT_SEEDS, ItemDict.TOMATO_SEEDS, ItemDict.WATERMELON_SEEDS};
        private Item[] FALL = { ItemDict.BEET_SEEDS, ItemDict.FLAX_SEEDS, ItemDict.BELLPEPPER_SEEDS, ItemDict.BROCCOLI_SEEDS, ItemDict.CABBAGE_SEEDS, ItemDict.PUMPKIN_SEEDS};
        private bool updatedStock;
        public TEntityStoreShelf(Vector2 tilePosition, AnimatedSprite sprite, Area area) : base(tilePosition, sprite)
        {
            saleItems.Add(new SaleItem(ItemDict.BEET_SEEDS, 15, 70, new DialogueNode("beet dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.FLAX_SEEDS, 15, 70, new DialogueNode("flax dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.BELLPEPPER_SEEDS, 15, 70, new DialogueNode("bellpepper dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.BROCCOLI_SEEDS, 15, 70, new DialogueNode("broccoli dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.ONION_SEEDS, 15, 70, new DialogueNode("onion dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.COTTON_SEEDS, 15, 70, new DialogueNode("cotton dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.CUCUMBER_SEEDS, 15, 70, new DialogueNode("cucumber dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.TOMATO_SEEDS, 15, 70, new DialogueNode("tomato dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.SPINACH_SEEDS, 15, 40, new DialogueNode("spinach dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.POTATO_SEEDS, 15, 60, new DialogueNode("potato dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));
            saleItems.Add(new SaleItem(ItemDict.CARROT_SEEDS, 15, 70, new DialogueNode("carrot dialogue", PlateauMain.CONTENT.Load<Texture2D>(Paths.ITEM_COMPOST_BIN))));

            sprite.AddLoop(LOOP_EMPTY, 0, 0, false);
            sprite.AddLoop(LOOP_BEET, 1, 1, false);
            sprite.AddLoop(LOOP_FLAX, 2, 2, false);
            sprite.AddLoop(LOOP_BELLPEPPER, 3, 3, false);
            sprite.AddLoop(LOOP_BROCCOLI, 4, 4, false);
            sprite.AddLoop(LOOP_CABBAGE, 5, 5, false);
            sprite.AddLoop(LOOP_PUMPKIN, 6, 6, false);
            sprite.AddLoop(LOOP_SPINACH, 7, 7, false);
            sprite.AddLoop(LOOP_POTATO, 8, 8, false);
            sprite.AddLoop(LOOP_STRAWBERRY, 9, 9, false);
            sprite.AddLoop(LOOP_CARROT, 10, 10, false);
            sprite.AddLoop(LOOP_CACTUS, 11, 11, false);
            sprite.AddLoop(LOOP_ONION, 12, 12, false);
            sprite.AddLoop(LOOP_COTTON, 13, 13, false);
            sprite.AddLoop(LOOP_CUCUMBER, 14, 14, false);
            sprite.AddLoop(LOOP_EGGPLANT, 15, 15, false);
            sprite.AddLoop(LOOP_TOMATO, 16, 16, false);
            sprite.AddLoop(LOOP_WATERMELON, 17, 17, false);

            sprite.SetLoop(LOOP_EMPTY);

            updatedStock = false;
        }

        private void UpdateCurrentSaleItem(World.Season season)
        {
            bool found = false;
            while (!found)
            {
                currentSaleItem = saleItems[Util.RandInt(0, saleItems.Count - 1)];
                switch(season)
                {
                    case World.Season.SPRING:
                        found = Util.ArrayContains(SPRING, currentSaleItem.item);
                        break;
                    case World.Season.SUMMER:
                        found = Util.ArrayContains(SUMMER, currentSaleItem.item);
                        break;
                    case World.Season.AUTUMN:
                        found = Util.ArrayContains(FALL, currentSaleItem.item);
                        break;
                    case World.Season.WINTER:
                        found = true;
                        break;
                }
            }
            quantityRemaining = currentSaleItem.quantity;
            ChangeAnimation();
        }
        
        public override RectangleF GetCollisionRectangle()
        {
            RectangleF colRec = base.GetCollisionRectangle();
            colRec.X -= 4;
            colRec.Width += 4;
            return colRec;
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
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
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            if(currentSaleItem.item == ItemDict.NONE)
            {
                sprite.SetLoopIfNot(LOOP_EMPTY);
            }
            else if (currentSaleItem.item == ItemDict.BEET_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_BEET);
            }
            else if (currentSaleItem.item == ItemDict.FLAX_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_FLAX);
            }
            else if (currentSaleItem.item == ItemDict.BELLPEPPER_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_BELLPEPPER);
            }
            else if (currentSaleItem.item == ItemDict.BROCCOLI_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_BROCCOLI);
            }
            else if (currentSaleItem.item == ItemDict.CABBAGE_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_CABBAGE);
            }
            else if (currentSaleItem.item == ItemDict.PUMPKIN_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_PUMPKIN);
            }
            else if (currentSaleItem.item == ItemDict.SPINACH_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_SPINACH);
            }
            else if (currentSaleItem.item == ItemDict.POTATO_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_POTATO);
            }
            else if (currentSaleItem.item == ItemDict.STRAWBERRY_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_STRAWBERRY);
            }
            else if (currentSaleItem.item == ItemDict.CARROT_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_CARROT);
            }
            else if (currentSaleItem.item == ItemDict.CACTUS_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_CACTUS);
            }
            else if (currentSaleItem.item == ItemDict.ONION_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_ONION);
            }
            else if (currentSaleItem.item == ItemDict.COTTON_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_COTTON);
            }
            else if (currentSaleItem.item == ItemDict.CUCUMBER_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_CUCUMBER);
            }
            else if (currentSaleItem.item == ItemDict.EGGPLANT_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_EGGPLANT);
            }
            else if (currentSaleItem.item == ItemDict.TOMATO_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_TOMATO);
            }
            else if (currentSaleItem.item == ItemDict.WATERMELON_SEEDS)
            {
                sprite.SetLoopIfNot(LOOP_WATERMELON);
            }
        }

    }
}
