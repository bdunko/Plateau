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
    public class PEntityAquarium : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum AquariumState
        {
            IDLE, WORKING, FINISHED
        }

        private static int FISH_CAPACITY_FEW = 4;
        private static int FISH_CAPACITY_MANY = 8;
        private static int FISH_CAPACITY_TONS = 16;
        private static int FISH_TIME_VERY_SHORT = 1 * 12 * 60;
        private static int FISH_TIME_SHORT = 1 * 23 * 60;
        private static int FISH_TIME_MEDIUM = 2 * 23 * 60;
        private static int FISH_TIME_LONG = 3 * 23 * 60;

        private Dictionary<Item, int> PROCESSING_TIME_BY_FISH = new Dictionary<Item, int>()
        {
            {ItemDict.CAVERN_TETRA, FISH_TIME_MEDIUM},
            {ItemDict.CAVEFISH, FISH_TIME_MEDIUM},
            {ItemDict.CATFISH, FISH_TIME_MEDIUM},
            {ItemDict.CARP, FISH_TIME_SHORT},
            {ItemDict.ONYX_EEL, FISH_TIME_MEDIUM},
            {ItemDict.MOLTEN_SQUID, FISH_TIME_MEDIUM},
            {ItemDict.MACKEREL, FISH_TIME_VERY_SHORT},
            {ItemDict.LUNAR_WHALE, FISH_TIME_LONG},
            {ItemDict.LAKE_TROUT, FISH_TIME_SHORT},
            {ItemDict.JUNGLE_PIRANHA, FISH_TIME_SHORT},
            {ItemDict.INKY_SQUID, FISH_TIME_SHORT},
            {ItemDict.INFERNAL_SHARK, FISH_TIME_LONG},
            {ItemDict.HERRING, FISH_TIME_VERY_SHORT},
            {ItemDict.GREAT_WHITE_SHARK, FISH_TIME_LONG},
            {ItemDict.EMPEROR_SALMON, FISH_TIME_LONG},
            {ItemDict.DARK_ANGLER, FISH_TIME_LONG},
            {ItemDict.CRAB, FISH_TIME_SHORT},
            {ItemDict.CLOUD_FLOUNDER, FISH_TIME_SHORT},
            {ItemDict.BLACKENED_OCTOPUS, FISH_TIME_MEDIUM},
            {ItemDict.BLUEGILL, FISH_TIME_SHORT},
            {ItemDict.BOXER_LOBSTER, FISH_TIME_MEDIUM},
            {ItemDict.WHITE_BLOWFISH, FISH_TIME_SHORT},
            {ItemDict.TUNA, FISH_TIME_SHORT},
            {ItemDict.SWORDFISH, FISH_TIME_MEDIUM},
            {ItemDict.SUNFISH, FISH_TIME_MEDIUM},
            {ItemDict.STRIPED_BASS, FISH_TIME_SHORT},
            {ItemDict.STORMBRINGER_KOI, FISH_TIME_LONG},
            {ItemDict.SKY_PIKE, FISH_TIME_LONG},
            {ItemDict.SHRIMP, FISH_TIME_VERY_SHORT},
            {ItemDict.SEA_TURTLE, FISH_TIME_LONG},
            {ItemDict.SARDINE, FISH_TIME_VERY_SHORT},
            {ItemDict.SALMON, FISH_TIME_SHORT},
            {ItemDict.RED_SNAPPER, FISH_TIME_SHORT},
            {ItemDict.QUEEN_AROWANA, FISH_TIME_MEDIUM},
            {ItemDict.PUFFERFISH, FISH_TIME_SHORT},
            {ItemDict.PIKE, FISH_TIME_MEDIUM},
        };

        private Dictionary<Item, int> MAX_QUANTITY_BY_FISH = new Dictionary<Item, int>()
        {
            {ItemDict.CAVERN_TETRA, FISH_CAPACITY_MANY},
            {ItemDict.CAVEFISH, FISH_CAPACITY_MANY},
            {ItemDict.CATFISH, FISH_CAPACITY_MANY},
            {ItemDict.CARP, FISH_CAPACITY_TONS},
            {ItemDict.ONYX_EEL, FISH_CAPACITY_FEW},
            {ItemDict.MOLTEN_SQUID, FISH_CAPACITY_FEW},
            {ItemDict.MACKEREL, FISH_CAPACITY_TONS},
            {ItemDict.LUNAR_WHALE, FISH_CAPACITY_FEW},
            {ItemDict.LAKE_TROUT, FISH_CAPACITY_MANY},
            {ItemDict.JUNGLE_PIRANHA, FISH_CAPACITY_FEW},
            {ItemDict.INKY_SQUID, FISH_CAPACITY_FEW},
            {ItemDict.INFERNAL_SHARK, FISH_CAPACITY_FEW},
            {ItemDict.HERRING, FISH_CAPACITY_TONS},
            {ItemDict.GREAT_WHITE_SHARK, FISH_CAPACITY_FEW},
            {ItemDict.EMPEROR_SALMON, FISH_CAPACITY_MANY},
            {ItemDict.DARK_ANGLER, FISH_CAPACITY_FEW},
            {ItemDict.CRAB, FISH_CAPACITY_TONS},
            {ItemDict.CLOUD_FLOUNDER, FISH_CAPACITY_MANY},
            {ItemDict.BLACKENED_OCTOPUS, FISH_CAPACITY_FEW},
            {ItemDict.BLUEGILL, FISH_CAPACITY_TONS},
            {ItemDict.BOXER_LOBSTER, FISH_CAPACITY_FEW},
            {ItemDict.WHITE_BLOWFISH, FISH_CAPACITY_MANY},
            {ItemDict.TUNA, FISH_CAPACITY_MANY},
            {ItemDict.SWORDFISH, FISH_CAPACITY_FEW},
            {ItemDict.SUNFISH, FISH_CAPACITY_MANY},
            {ItemDict.STRIPED_BASS, FISH_CAPACITY_TONS},
            {ItemDict.STORMBRINGER_KOI, FISH_CAPACITY_MANY},
            {ItemDict.SKY_PIKE, FISH_CAPACITY_FEW},
            {ItemDict.SHRIMP, FISH_CAPACITY_TONS},
            {ItemDict.SEA_TURTLE, FISH_CAPACITY_FEW},
            {ItemDict.SARDINE, FISH_CAPACITY_TONS},
            {ItemDict.SALMON, FISH_CAPACITY_MANY},
            {ItemDict.RED_SNAPPER, FISH_CAPACITY_MANY},
            {ItemDict.QUEEN_AROWANA, FISH_CAPACITY_FEW},
            {ItemDict.PUFFERFISH, FISH_CAPACITY_FEW},
            {ItemDict.PIKE, FISH_CAPACITY_MANY},
        };

        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private AquariumState state;
        private ResultHoverBox resultHoverBox;

        public PEntityAquarium(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 4, 7, true);
            sprite.AddLoop("working", 8, 11, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = AquariumState.IDLE;
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
            string stateStr = saveState.TryGetData("state", AquariumState.IDLE.ToString());
            if (stateStr.Equals(AquariumState.IDLE.ToString()))
            {
                state = AquariumState.IDLE;
            }
            else if (stateStr.Equals(AquariumState.WORKING.ToString()))
            {
                state = AquariumState.WORKING;
            }
            else if (stateStr.Equals(AquariumState.FINISHED.ToString()))
            {
                state = AquariumState.FINISHED;
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
                if (state == AquariumState.WORKING || state == AquariumState.FINISHED)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("idle");
                }
            }

            if (state == AquariumState.FINISHED)
            {
                resultHoverBox.AssignItemStack(new ItemStack(heldItem.GetItem(), heldItem.GetQuantity()));
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
            if (state == AquariumState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (heldItem.GetItem() != ItemDict.NONE && heldItem.GetQuantity() == 2)
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
            if (heldItem.GetItem() != ItemDict.NONE && heldItem.GetQuantity() != 2)
            {
                for (int i = 0; i < heldItem.GetQuantity()-2; i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(heldItem.GetItem(), 2);
                state = AquariumState.WORKING;
                timeRemaining = PROCESSING_TIME_BY_FISH[heldItem.GetItem()];
            }
            else if (heldItem.GetItem() != ItemDict.NONE && heldItem.GetQuantity() == 2)
            {
                for(int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = AquariumState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() == ItemDict.NONE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (PROCESSING_TIME_BY_FISH.ContainsKey(addedItem))
                {
                    if (player.HasItemStack(new ItemStack(addedItem, 2))) {
                        player.RemoveItemStackFromInventory(new ItemStack(addedItem, 2));
                        heldItem = new ItemStack(addedItem, 2);
                        sprite.SetLoop("placement");
                        state = AquariumState.WORKING;
                        timeRemaining = PROCESSING_TIME_BY_FISH[heldItem.GetItem()];
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I need to add a starting pair of two fish of the same kind.", Color.Red));
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't put this in the aquarium!", Color.Red));
                }
            }
            else
            {
                if (heldItem.GetItem() == player.GetHeldItem().GetItem())
                {
                    player.AddNotification(new EntityPlayer.Notification("Two fish is enough; I don't need to add more.", Color.Red));
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I should empty the tank before adding other fish.", Color.Red));
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
            if (timeRemaining <= 0 && state == AquariumState.WORKING)
            {
                heldItem = new ItemStack(heldItem.GetItem(), Math.Min(MAX_QUANTITY_BY_FISH[heldItem.GetItem()], heldItem.GetQuantity() + 1));
                sprite.SetLoop("placement");
                timeRemaining = PROCESSING_TIME_BY_FISH[heldItem.GetItem()];
                if (heldItem.GetQuantity() == MAX_QUANTITY_BY_FISH[heldItem.GetItem()])
                {
                    state = AquariumState.FINISHED;
                }
            }
        }

        public HoveringInterface GetHoveringInterface()
        {
            if (state == AquariumState.WORKING || state == AquariumState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
