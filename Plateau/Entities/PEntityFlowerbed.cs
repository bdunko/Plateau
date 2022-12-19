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
    public class PEntityFlowerbed : PlacedEntity, IInteract, ITick, ITickDaily, IHaveHoveringInterface
    {
        private enum FlowerState
        {
            IDLE, PLANTED, FINISHED
        }

        private class FlowerData
        {
            public enum SeasonPair
            {
                SPRING_SUMMER, AUTUMN_WINTER
            }

            public string spriteLoop;
            public SeasonPair seasons;

            public FlowerData(string spriteLoop, SeasonPair seasons)
            {
                this.spriteLoop = spriteLoop;
                this.seasons = seasons;
            }
        }

        private Dictionary<Item, FlowerData> flowerDictionary;

        private FlowerState state;
        private static int PROCESSING_TIME = 3 * 23 * 60;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack heldItem;

        public PEntityFlowerbed(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.state = FlowerState.IDLE;
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            sprite.AddLoop("red_ginger", 4, 4, true);
            sprite.AddLoop("bluebell", 5, 5, true);
            sprite.AddLoop("sunflower", 6, 6, true);
            sprite.AddLoop("lavender", 7, 7, true);
            sprite.AddLoop("marigold", 8, 8, true);
            sprite.AddLoop("snowdrop", 9, 9, true);
            sprite.AddLoop("sky_rose", 10, 10, true);
            this.timeRemaining = PROCESSING_TIME;
            this.resultHoverBox = new ResultHoverBox();

            if(flowerDictionary == null)
            {
                flowerDictionary = new Dictionary<Item, FlowerData>()
                {
                    { ItemDict.RED_GINGER, new FlowerData("red_ginger", FlowerData.SeasonPair.SPRING_SUMMER) },
                    { ItemDict.SUNFLOWER, new FlowerData("sunflower", FlowerData.SeasonPair.SPRING_SUMMER) },
                    { ItemDict.MARIGOLD, new FlowerData("marigold", FlowerData.SeasonPair.SPRING_SUMMER) },
                    { ItemDict.LAVENDER, new FlowerData("lavender", FlowerData.SeasonPair.SPRING_SUMMER) },
                    { ItemDict.BLUEBELL, new FlowerData("bluebell", FlowerData.SeasonPair.SPRING_SUMMER) },
                    { ItemDict.SKY_ROSE, new FlowerData("sky_rose", FlowerData.SeasonPair.AUTUMN_WINTER) },
                    { ItemDict.SNOWDROP,  new FlowerData("snowdrop", FlowerData.SeasonPair.AUTUMN_WINTER) }
                };
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y));
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("item", heldItem.GetItem().GetName());
            save.AddData("quantity", heldItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            save.AddData("state", state.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", FlowerState.IDLE.ToString());
            if (stateStr.Equals(FlowerState.IDLE.ToString()))
            {
                state = FlowerState.IDLE;
            }
            else if (stateStr.Equals(FlowerState.PLANTED.ToString()))
            {
                state = FlowerState.PLANTED;
            }
            else if (stateStr.Equals(FlowerState.FINISHED.ToString()))
            {
                state = FlowerState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if (state == FlowerState.FINISHED)
            {
                sprite.SetLoopIfNot(flowerDictionary[heldItem.GetItem()].spriteLoop);
            }

            if (state == FlowerState.FINISHED)
            {
                resultHoverBox.AssignItemStack(heldItem);
            }
            else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
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
            return "";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (state == FlowerState.IDLE)
            {
                return "Plant";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == FlowerState.FINISHED)
            {
                return "Harvest";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == FlowerState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = FlowerState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == FlowerState.IDLE)
            {
                ItemStack input = player.GetHeldItem();
                if (flowerDictionary.ContainsKey(input.GetItem()))
                {
                    bool canPlant = true;
                    if((area.GetSeason() == World.Season.SPRING || area.GetSeason() == World.Season.SUMMER) && flowerDictionary[input.GetItem()].seasons == FlowerData.SeasonPair.AUTUMN_WINTER)
                    {
                        canPlant = false;
                    }
                    if ((area.GetSeason() == World.Season.AUTUMN || area.GetSeason() == World.Season.WINTER) && flowerDictionary[input.GetItem()].seasons == FlowerData.SeasonPair.SPRING_SUMMER)
                    {
                        canPlant = false;
                    }
                    

                    if (canPlant)
                    {
                        heldItem = new ItemStack(input.GetItem(), 1);
                        input.Subtract(1);
                        state = FlowerState.PLANTED;
                        timeRemaining = PROCESSING_TIME;
                        sprite.SetLoop("placement");
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I can't plant this flower outside in this season.", Color.Red));
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't plant this in a flowerbed.", Color.Red));
                }
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            if (state == FlowerState.PLANTED)
            {
                timeRemaining -= minutesTicked;
                if (timeRemaining <= 0)
                {
                    heldItem.SetQuantity(Util.RandInt(6, 8));
                    sprite.SetLoop("placement");
                    state = FlowerState.FINISHED;
                }
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state != FlowerState.FINISHED)
            {
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                if ((area.GetSeason() == World.Season.SPRING || area.GetSeason() == World.Season.SUMMER) && flowerDictionary[heldItem.GetItem()].seasons == FlowerData.SeasonPair.AUTUMN_WINTER)
                {
                    heldItem = new ItemStack(ItemDict.NONE, 0);
                    state = FlowerState.IDLE;
                    sprite.SetLoop("idle");

                }
                if ((area.GetSeason() == World.Season.AUTUMN || area.GetSeason() == World.Season.WINTER) && flowerDictionary[heldItem.GetItem()].seasons == FlowerData.SeasonPair.SPRING_SUMMER)
                {
                    heldItem = new ItemStack(ItemDict.NONE, 0);
                    state = FlowerState.IDLE;
                    sprite.SetLoop("idle");
                }
            }   
        }
    }
}
