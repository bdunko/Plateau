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
    public class PEntityBeehive : PlacedEntity, IInteract, ITick, IHaveHoveringInterface, ITickDaily
    {
        private static Area.AreaEnum[] INCLUDED_AREAS = { Area.AreaEnum.BEACH, Area.AreaEnum.FARM, Area.AreaEnum.TOWN, Area.AreaEnum.S0, Area.AreaEnum.S1, Area.AreaEnum.S3};

        private static int MAX_CAPACITY = 6;
        private static int PROCESSING_TIME = 23 * 60;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack producedItem;
        private static float TIME_BETWEEN_ANIMATION = 3.0f;
        private static int MAX_BEES = 5;
        private static float MULTIPLIER_PER_BEE = 0.2f;
        private static float MULTIPLIER_PER_HORNET = 0.33f;
        private Item beeType;
        private float animationTimer;
        private int beesAdded;

        public PEntityBeehive(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.producedItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.AddLoop("animation", 4, 9, false);
            sprite.SetLoop("placement");
            this.timeRemaining = PROCESSING_TIME;
            this.resultHoverBox = new ResultHoverBox();
            this.animationTimer = TIME_BETWEEN_ANIMATION;
            this.beesAdded = 0;
            this.beeType = ItemDict.NONE;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.Wheat, layerDepth);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y), layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("item", producedItem.GetItem().GetName());
            save.AddData("beeType", beeType.GetName());
            save.AddData("quantity", producedItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            save.AddData("beesAdded", beesAdded.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            producedItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            beeType = ItemDict.GetItemByName(saveState.TryGetData("beeType", ItemDict.NONE.GetName()));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            beesAdded = Int32.Parse(saveState.TryGetData("beesAdded", "0"));
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if(sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if(!sprite.IsCurrentLoop("placement"))
            {
                animationTimer -= deltaTime;
                if(animationTimer <= 0)
                {
                    animationTimer = TIME_BETWEEN_ANIMATION;
                    sprite.SetLoop("animation");
                }
            }

            if(producedItem.GetItem() != ItemDict.NONE)
            {
                resultHoverBox.AssignItemStack(producedItem);
            } else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (producedItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < producedItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(producedItem.GetItem(), new Vector2(position.X, position.Y - 10)));
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
            if (beeType != ItemDict.NONE && producedItem.GetItem() == ItemDict.NONE)
            {
                return "Empty";
            }
            return "";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (producedItem.GetItem() == ItemDict.NONE && beesAdded != MAX_BEES)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if(producedItem.GetItem() != ItemDict.NONE)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (producedItem.GetItem() != ItemDict.NONE)
            {
                List<Item> result = new List<Item>();
                for(int j = 0; j < producedItem.GetQuantity(); j++)
                {
                    result.Add(producedItem.GetItem());
                }
                    
                switch(area.GetAreaEnum())
                {
                    case Area.AreaEnum.FARM:
                    case Area.AreaEnum.TOWN:
                    case Area.AreaEnum.S0:
                        for(int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (Util.RandInt(1, 100) <= 40) {result.Add(ItemDict.BEESWAX); }
                            if (Util.RandInt(1, 100) <= 10) { result.Add(ItemDict.HONEYCOMB); }
                        }
                        break;
                    case Area.AreaEnum.BEACH:
                        for(int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.HONEYCOMB); }
                        }
                        break;
                    case Area.AreaEnum.S1:
                        for (int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.ROYAL_JELLY); }
                            if (Util.RandInt(1, 100) <= 10) { result.Add(ItemDict.BEESWAX); }
                        }
                        break;
                    case Area.AreaEnum.S3:
                        for (int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.POLLEN_PUFF); }
                            if (Util.RandInt(1, 100) <= 10) { result.Add(ItemDict.BEESWAX); }
                        }
                        break;
                }
                if(Util.RandInt(1, 100) == 1) { result.Add(ItemDict.QUEENS_STINGER);  }

                foreach(Item resultItem in result)
                {
                    area.AddEntity(new EntityItem(resultItem, new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                producedItem = new ItemStack(ItemDict.NONE, 0);
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            Item held = player.GetHeldItem().GetItem();
            if(held == ItemDict.HONEY_BEE || held == ItemDict.STINGER_HORNET)
            {
                if(producedItem.GetItem() != ItemDict.NONE)
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't add bees after honey is being produced.", Color.Black));
                } else if (area.GetSeason() == World.Season.WINTER)
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't add bees to the Beehive in Winter.", Color.Black));
                } else if (beeType != ItemDict.NONE && held != beeType)
                {
                    player.AddNotification(new EntityPlayer.Notification("I need to empty the hive if I want to change types,\nI can't mix Bees and Hornets!", Color.Black));
                } else if (!INCLUDED_AREAS.Contains(area.GetAreaEnum()))
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't beekeep in this area!", Color.Black));
                }
                else if (beesAdded < MAX_BEES)
                {
                    beeType = held;
                    player.GetHeldItem().Subtract(1);
                    beesAdded++;
                    sprite.SetLoop("placement");
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I've added enough bees for now.", Color.Black));
                }
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            if (beesAdded != 0)
            {
                beesAdded = 0;
                beeType = ItemDict.NONE;
                sprite.SetLoop("placement");
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {

        }

        public void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            int adjustedTime = minutesTicked;
            if(beeType == ItemDict.HONEY_BEE)
            {
                adjustedTime = (int)(minutesTicked * (1 + (MULTIPLIER_PER_BEE * beesAdded)));
            } else if (beeType == ItemDict.STINGER_HORNET)
            {
                adjustedTime = (int)(minutesTicked * (1 + (MULTIPLIER_PER_HORNET * beesAdded)));
            }
            timeRemaining -= adjustedTime;
            if(timeRemaining <= 0)
            {
                if (INCLUDED_AREAS.Contains(area.GetAreaEnum()) && area.GetSeason() != World.Season.WINTER)
                {
                    if (producedItem.GetItem() == ItemDict.NONE)
                    {
                        producedItem = new ItemStack(ItemDict.WILD_HONEY, 1);
                    }
                    else
                    {
                        if (producedItem.GetQuantity() < MAX_CAPACITY)
                        {
                            producedItem.Add(1);
                        }
                    }
                }
                timeRemaining = PROCESSING_TIME;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (producedItem.GetItem() == ItemDict.NONE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(new ItemStack(beesAdded != 0 ? beeType : ItemDict.NONE, beesAdded))));
            }
            return new HoveringInterface();
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            if(area.GetSeason() == World.Season.WINTER)
            {
                beesAdded = 0;
                beeType = ItemDict.NONE;
            }
        }
    }
}
