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
    public class PEntityExtractor : PlacedEntity, IInteract, ITick
    {
        private enum ExtractorState
        {
            IDLE, WORKING
        }

        private static int PROCESSING_TIME = 23 * 60;

        private static int MAX_CAPACITY = 25;

        private static Dictionary<Area.AreaEnum, List<Item>> ITEMS_PER_AREA = new Dictionary<Area.AreaEnum, List<Item>>()
        {
            {Area.AreaEnum.INTERIOR, new List<Item>() { null } },
            {Area.AreaEnum.FARM, new List<Item>() { ItemDict.CLAY, ItemDict.STONE, ItemDict.SCRAP_IRON, ItemDict.IRON_ORE, ItemDict.FOSSIL_SHARDS, ItemDict.COAL } },
            {Area.AreaEnum.BEACH, new List<Item>() { ItemDict.SALT_SHARDS, ItemDict.STONE, ItemDict.GOLD_ORE, ItemDict.FOSSIL_SHARDS }},
            {Area.AreaEnum.TOWN, new List<Item>() { ItemDict.STONE, ItemDict.CLAY, ItemDict.OPAL }},
            {Area.AreaEnum.S0, new List<Item>() { ItemDict.COAL, ItemDict.SCRAP_IRON, ItemDict.CLAY, ItemDict.STONE, ItemDict.QUARTZ }},
            {Area.AreaEnum.S1, new List<Item>() { ItemDict.IRON_ORE, ItemDict.STONE, ItemDict.CLAY }},
            {Area.AreaEnum.S2, new List<Item>() { ItemDict.MYTHRIL_ORE, ItemDict.STONE, ItemDict.IRON_ORE, ItemDict.AMETHYST, ItemDict.QUARTZ, ItemDict.TOPAZ, ItemDict.IRON_CHIP }},
            {Area.AreaEnum.S3, new List<Item>() { ItemDict.GOLD_ORE, ItemDict.STONE, ItemDict.FOSSIL_SHARDS, ItemDict.TRILOBITE, ItemDict.COAL, ItemDict.OLD_BONE }},
            {Area.AreaEnum.S4, new List<Item>() { ItemDict.ADAMANTITE_ORE, ItemDict.MYTHRIL_ORE, ItemDict.STONE, ItemDict.MYTHRIL_CHIP, ItemDict.GOLD_CHIP, ItemDict.IRON_CHIP }},
            {Area.AreaEnum.APEX, new List<Item>() { ItemDict.IRON_CHIP, ItemDict.MYTHRIL_CHIP, ItemDict.GOLD_CHIP }}
        };

        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private ExtractorState state;
        private ResultHoverBox resultHoverBox;

        public PEntityExtractor(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = ExtractorState.IDLE;
            this.timeRemaining = 0;
            this.resultHoverBox = new ResultHoverBox();
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y), layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("state", state.ToString());
            save.AddData("item", heldItem.GetItem().GetName());
            save.AddData("quantity", heldItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", ExtractorState.IDLE.ToString());
            if (stateStr.Equals(ExtractorState.IDLE.ToString()))
            {
                state = ExtractorState.IDLE;
            }
            else if (stateStr.Equals(ExtractorState.WORKING.ToString()))
            {
                state = ExtractorState.WORKING;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == ExtractorState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("idle");
                }
            }

            if (heldItem.GetItem() != ItemDict.NONE)
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
            if (state == ExtractorState.IDLE)
            {
                return "Fuel";
            } 
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                return "Collect";
            }
            return "";
        }

        private static List<Item> FUEL_ITEMS = new List<Item>() { ItemDict.COAL, ItemDict.OIL };

        private Item RollItemForArea(Area.AreaEnum area)
        {
            List<Item> table = ITEMS_PER_AREA[area];
            return table[Util.RandInt(0, table.Count - 1)];
        }

        private Item GetPrimaryItemForArea(Area.AreaEnum area)
        {
            return ITEMS_PER_AREA[area][0];
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                    if(i % 2 == 0)
                    {
                        Item roll = RollItemForArea(area.GetAreaEnum());
                        area.AddEntity(new EntityItem(roll, new Vector2(position.X, position.Y - 10)));
                    }
                }

                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = ExtractorState.IDLE;
            } 
            
        }

        private bool IsGroundExtractable(Area area)
        {
            Vector2 check1 = new Vector2(tilePosition.X, tilePosition.Y + 4);
            Vector2 check2 = new Vector2(tilePosition.X + 1, tilePosition.Y + 4);

            Area.GroundTileType type1 = area.GetGroundTileType((int)check1.X, (int)check1.Y);
            Area.GroundTileType type2 = area.GetGroundTileType((int)check2.X, (int)check2.Y);

            if(type1 == Area.GroundTileType.BRIDGE || type1 == Area.GroundTileType.SOLID || type2 == Area.GroundTileType.BRIDGE || type2 == Area.GroundTileType.SOLID)
            {
                return false;
            }

            return true;
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == ExtractorState.IDLE && heldItem.GetItem() == ItemDict.NONE)
            {
                Item addedItem = player.GetHeldItem().GetItem();

                if (FUEL_ITEMS.Contains(addedItem))
                {
                    if(GetPrimaryItemForArea(area.GetAreaEnum()) != null && IsGroundExtractable(area))
                    {
                        player.GetHeldItem().Subtract(1);
                        sprite.SetLoop("placement");
                        timeRemaining = PROCESSING_TIME;
                        state = ExtractorState.WORKING;
                    }
                    else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I can't use the Extractor here!", Color.Red));
                    }
                }
                else 
                {
                    player.AddNotification(new EntityPlayer.Notification("This is not usable as Extractor fuel!", Color.Red));
                }
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void Tick(int time, EntityPlayer player, Area area, World world)
        {
            timeRemaining -= time;
            if (timeRemaining <= 0 && state == ExtractorState.WORKING)
            {
                heldItem = new ItemStack(GetPrimaryItemForArea(area.GetAreaEnum()), Math.Min(MAX_CAPACITY, heldItem.GetQuantity()+1));
                sprite.SetLoop("placement");
                timeRemaining = PROCESSING_TIME;
                if(heldItem.GetQuantity() == MAX_CAPACITY)
                {
                    state = ExtractorState.IDLE;
                }
            }
        }
    }
}
