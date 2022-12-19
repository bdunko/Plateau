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
    public class PEntityDryingRack : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum DryingRackState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 2 * 24 * 60;
        private PartialRecolorSprite sprite;
        private Item heldItem;
        private int timeRemaining;
        private DryingRackState state;
        private ResultHoverBox resultHoverBox;

        private Dictionary<Item, Item> DRYING_DICTIONARY = new Dictionary<Item, Item> {
            {ItemDict.WILD_MEAT, ItemDict.BOAR_JERKY},

            {ItemDict.SEAWEED, ItemDict.SEAWEED_SNACK },

            {ItemDict.POTATO, ItemDict.POTATO_CRISPS },

            {ItemDict.APPLE, ItemDict.DRIED_APPLE }, 
            {ItemDict.BANANA, ItemDict.BANANA_CHIPS }, 
            {ItemDict.COCONUT, ItemDict.COCONUT_CHIPS },
            {ItemDict.CHERRY, ItemDict.CHERRY_RAISINS },
            {ItemDict.LEMON, ItemDict.DRIED_CITRUS }, 
            {ItemDict.ORANGE, ItemDict.DRIED_CITRUS },
            {ItemDict.OLIVE, ItemDict.DRIED_OLIVES }, 
            {ItemDict.STRAWBERRY, ItemDict.DRIED_STRAWBERRY }, 
            {ItemDict.WATERMELON_SLICE, ItemDict.WATERLESS_MELON }, 

            {ItemDict.BEET, ItemDict.VEGGIE_CHIPS },
            {ItemDict.BELLPEPPER, ItemDict.VEGGIE_CHIPS },
            {ItemDict.BROCCOLI, ItemDict.VEGGIE_CHIPS },
            {ItemDict.CABBAGE, ItemDict.VEGGIE_CHIPS },
            {ItemDict.CARROT, ItemDict.VEGGIE_CHIPS },
            {ItemDict.CUCUMBER, ItemDict.VEGGIE_CHIPS },
            {ItemDict.EGGPLANT, ItemDict.VEGGIE_CHIPS },
            {ItemDict.ONION, ItemDict.VEGGIE_CHIPS },
            {ItemDict.PUMPKIN, ItemDict.VEGGIE_CHIPS },
            {ItemDict.SPINACH, ItemDict.VEGGIE_CHIPS },
            {ItemDict.TOMATO, ItemDict.VEGGIE_CHIPS },

            {ItemDict.BLUEGILL, ItemDict.KLIPPFISK },
            {ItemDict.CARP, ItemDict.KLIPPFISK },
            {ItemDict.CATFISH, ItemDict.KLIPPFISK },
            {ItemDict.CAVEFISH, ItemDict.KLIPPFISK },
            {ItemDict.CAVERN_TETRA, ItemDict.KLIPPFISK },
            {ItemDict.CLOUD_FLOUNDER, ItemDict.KLIPPFISK },
            {ItemDict.HERRING, ItemDict.KLIPPFISK },
            {ItemDict.JUNGLE_PIRANHA, ItemDict.KLIPPFISK },
            {ItemDict.LAKE_TROUT, ItemDict.KLIPPFISK },
            {ItemDict.MACKEREL, ItemDict.KLIPPFISK },
            {ItemDict.PIKE, ItemDict.KLIPPFISK },
            {ItemDict.RED_SNAPPER, ItemDict.KLIPPFISK },
            {ItemDict.SALMON, ItemDict.KLIPPFISK },
            {ItemDict.SARDINE, ItemDict.KLIPPFISK },
            {ItemDict.SKY_PIKE, ItemDict.KLIPPFISK },
            {ItemDict.STORMBRINGER_KOI, ItemDict.KLIPPFISK },
            {ItemDict.STRIPED_BASS, ItemDict.KLIPPFISK },
            {ItemDict.SUNFISH, ItemDict.KLIPPFISK },
            {ItemDict.SWORDFISH, ItemDict.KLIPPFISK },
            {ItemDict.TUNA, ItemDict.KLIPPFISK }
        };

        public PEntityDryingRack(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working_fish", 4, 4, true);
            sprite.AddLoop("working_meat", 5, 5, true);
            sprite.AddLoop("working_veggie", 6, 6, true);
            sprite.AddLoop("working_other", 7, 7, true);
            sprite.AddLoop("finished", 8, 8, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = DryingRackState.IDLE;
            this.timeRemaining = 0;
            this.resultHoverBox = new ResultHoverBox();
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y));
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("state", state.ToString());
            save.AddData("item", heldItem.GetName());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName()));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", DryingRackState.IDLE.ToString());
            if (stateStr.Equals(DryingRackState.IDLE.ToString()))
            {
                state = DryingRackState.IDLE;
            }
            else if (stateStr.Equals(DryingRackState.WORKING.ToString()))
            {
                state = DryingRackState.WORKING;
            }
            else if (stateStr.Equals(DryingRackState.FINISHED.ToString()))
            {
                state = DryingRackState.FINISHED;
            }
        }

        private void SetAnimationByItem()
        {
            if(heldItem.HasTag(Item.Tag.FISH))
            {
                sprite.SetLoopIfNot("working_fish");
            } else if (heldItem == ItemDict.WILD_MEAT)
            {
                sprite.SetLoopIfNot("working_meat");
            } else if (heldItem.HasTag(Item.Tag.VEGETABLE))
            {
                sprite.SetLoopIfNot("working_veggie");
            } else
            {
                sprite.SetLoopIfNot("working_other");
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("anim");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == DryingRackState.WORKING)
                {
                    SetAnimationByItem();
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == DryingRackState.FINISHED)
            {
                resultHoverBox.AssignItemStack(heldItem);
                sprite.SetLoopIfNot("finished");
            }
            else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (heldItem != ItemDict.NONE)
            {
                area.AddEntity(new EntityItem(heldItem, new Vector2(position.X, position.Y - 10)));
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
            if (state == DryingRackState.IDLE)
            {
                return "Dry";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == DryingRackState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == DryingRackState.FINISHED)
            {
                area.AddEntity(new EntityItem(heldItem, new Vector2(position.X, position.Y - 10)));
                sprite.SetLoop("placement");
                heldItem = ItemDict.NONE;
                state = DryingRackState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == DryingRackState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                Item trial = addedItem;
                if(trial.HasTag(Item.Tag.SILVER_CROP) || trial.HasTag(Item.Tag.GOLDEN_CROP) || trial.HasTag(Item.Tag.PHANTOM_CROP))
                {
                    trial = ItemDict.GetCropBaseForm(trial);
                }

                if (DRYING_DICTIONARY.ContainsKey(trial))
                {
                    heldItem = addedItem;
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = DryingRackState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't dry this on the Drying Rack.", Color.Red));
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
            if (timeRemaining <= 0 && state == DryingRackState.WORKING)
            {
                if (heldItem.HasTag(Item.Tag.SILVER_CROP) || heldItem.HasTag(Item.Tag.GOLDEN_CROP) || heldItem.HasTag(Item.Tag.PHANTOM_CROP))
                {
                    heldItem = ItemDict.GetCropBaseForm(heldItem);
                }

                heldItem = DRYING_DICTIONARY[heldItem];
                state = DryingRackState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == DryingRackState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
