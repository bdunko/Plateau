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
    public class PEntityGemstoneReplicator : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum ReplicatorState
        {
            IDLE, WORKING
        }

        private static int GEM_TIME_VERY_SHORT = 1 * 23 * 60;
        private static int GEM_TIME_SHORT = 3 * 23 * 60;
        private static int GEM_TIME_MEDIUM = 5 * 23 * 60;
        private static int GEM_TIME_LONG = 7 * 23 * 60;

        private static int MAX_CAPACITY = 5;

        private Dictionary<Item, int> PROCESSING_TIME_BY_GEMSTONE = new Dictionary<Item, int>()
        {
            {ItemDict.AMETHYST, GEM_TIME_SHORT},
            {ItemDict.DIAMOND, GEM_TIME_LONG},
            {ItemDict.EARTH_CRYSTAL, GEM_TIME_LONG},
            {ItemDict.EMERALD, GEM_TIME_MEDIUM},
            {ItemDict.FIRE_CRYSTAL, GEM_TIME_LONG},
            {ItemDict.WIND_CRYSTAL, GEM_TIME_LONG},
            {ItemDict.WATER_CRYSTAL, GEM_TIME_LONG},
            {ItemDict.QUARTZ, GEM_TIME_SHORT},
            {ItemDict.RUBY, GEM_TIME_MEDIUM},
            {ItemDict.SAPPHIRE, GEM_TIME_MEDIUM},
            {ItemDict.OPAL, GEM_TIME_SHORT},
            {ItemDict.TOPAZ, GEM_TIME_SHORT},
            {ItemDict.AQUAMARINE, GEM_TIME_MEDIUM},
            {ItemDict.SNOW_CRYSTAL, GEM_TIME_VERY_SHORT},
            {ItemDict.ICE_NINE, GEM_TIME_LONG},
            {ItemDict.PEARL, GEM_TIME_LONG }
        };

        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private Item seed;
        private int timeRemaining;
        private ReplicatorState state;
        private ResultHoverBox resultHoverBox;

        public PEntityGemstoneReplicator(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.seed = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = ReplicatorState.IDLE;
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
            save.AddData("seed", seed.GetName());
            save.AddData("item", heldItem.GetItem().GetName());
            save.AddData("quantity", heldItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            seed = ItemDict.GetItemByName(saveState.TryGetData("seed", ItemDict.NONE.GetName()));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", ReplicatorState.IDLE.ToString());
            if (stateStr.Equals(ReplicatorState.IDLE.ToString()))
            {
                state = ReplicatorState.IDLE;
            }
            else if (stateStr.Equals(ReplicatorState.WORKING.ToString()))
            {
                state = ReplicatorState.WORKING;
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
                if (state == ReplicatorState.WORKING)
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
            if(seed != ItemDict.NONE)
            {
                area.AddEntity(new EntityItem(seed, new Vector2(position.X, position.Y - 10)));
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
            if (state == ReplicatorState.IDLE)
            {
                return "Seed";
            } else if (state == ReplicatorState.WORKING && heldItem.GetItem() == ItemDict.NONE)
            {
                return "Swap";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (heldItem.GetItem() == ItemDict.NONE && seed != ItemDict.NONE)
            {
                return "Empty";
            }
            else if (heldItem.GetItem() != ItemDict.NONE)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = ReplicatorState.WORKING;
                timeRemaining = PROCESSING_TIME_BY_GEMSTONE[seed];

            } else if (seed != ItemDict.NONE)
            {
                area.AddEntity(new EntityItem(seed, new Vector2(position.X, position.Y - 10)));
                seed = ItemDict.NONE;
                sprite.SetLoop("placement");
                state = ReplicatorState.IDLE;
            }
            
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() == ItemDict.NONE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if(PROCESSING_TIME_BY_GEMSTONE.ContainsKey(addedItem))
                {
                    if (addedItem != seed)
                    {
                        if (seed != ItemDict.NONE)
                        {
                            area.AddEntity(new EntityItem(seed, new Vector2(position.X, position.Y - 10)));
                        }
                        player.GetHeldItem().Subtract(1);
                        sprite.SetLoop("placement");
                        seed = addedItem;
                        timeRemaining = PROCESSING_TIME_BY_GEMSTONE[seed];
                        state = ReplicatorState.WORKING;
                    }
                } else
                {
                    if (seed == ItemDict.NONE)
                    {
                        player.AddNotification(new EntityPlayer.Notification("I need to seed the replicator with a gemstone.", Color.Red));
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I can't use this as a seed.", Color.Red));
                    }
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
            if (timeRemaining <= 0 && state == ReplicatorState.WORKING)
            {
                heldItem = new ItemStack(seed, Math.Min(MAX_CAPACITY, heldItem.GetQuantity()+1));
                sprite.SetLoop("placement");
                timeRemaining = PROCESSING_TIME_BY_GEMSTONE[seed];
                if(heldItem.GetQuantity() == MAX_CAPACITY)
                {
                    state = ReplicatorState.IDLE;
                }
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (heldItem.GetItem() == ItemDict.NONE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(seed)));
            }
            return null;
        }
    }
}
