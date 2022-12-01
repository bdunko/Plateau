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
    public class PEntityAlchemizer : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum AlchemizerState
        {
            IDLE, WORKING, FINISHED
        }

        private static float ALCHEMY_MULTIPLIER = 1.65f;
        private static int PROCESSING_TIME = 1 * 60;
        private PartialRecolorSprite sprite;
        private Item heldItem;
        private int timeRemaining;
        private AlchemizerState state;

        private Dictionary<Item, Item> ALCHEMY_DICTIONARY = new Dictionary<Item, Item> {
            {ItemDict.SCRAP_IRON, ItemDict.GOLD_ORE},
            {ItemDict.IRON_ORE, ItemDict.GOLD_ORE},
            {ItemDict.IRON_BAR, ItemDict.GOLD_BAR},
            {ItemDict.MYTHRIL_ORE, ItemDict.GOLD_ORE},
            {ItemDict.MYTHRIL_BAR, ItemDict.GOLD_BAR},
            {ItemDict.WEEDS, ItemDict.GOLDEN_LEAF},
            {ItemDict.METAL_FENCE, ItemDict.GOLDEN_FENCE},
            {ItemDict.BLOCK_METAL, ItemDict.BLOCK_GOLDEN},
            {ItemDict.PLATFORM_METAL, ItemDict.PLATFORM_GOLDEN},
            {ItemDict.PLATFORM_METAL_FARM, ItemDict.PLATFORM_GOLDEN_FARM},
            {ItemDict.SCAFFOLDING_METAL, ItemDict.SCAFFOLDING_GOLDEN},
            {ItemDict.EGG, ItemDict.GOLDEN_EGG},
            {ItemDict.WOOL, ItemDict.GOLDEN_WOOL},
            {ItemDict.SKY_BOTTLE, ItemDict.SHIMMERING_SALVE},
            {ItemDict.MOSS_BOTTLE, ItemDict.SHIMMERING_SALVE},
            {ItemDict.TROPICAL_BOTTLE, ItemDict.SHIMMERING_SALVE},
        };

        public PEntityAlchemizer(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 10, true);
            sprite.AddLoop("finished", 11, 11, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = AlchemizerState.IDLE;
            this.timeRemaining = 0;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
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
            string stateStr = saveState.TryGetData("state", AlchemizerState.IDLE.ToString());
            if (stateStr.Equals(AlchemizerState.IDLE.ToString()))
            {
                state = AlchemizerState.IDLE;
            }
            else if (stateStr.Equals(AlchemizerState.WORKING.ToString()))
            {
                state = AlchemizerState.WORKING;
            }
            else if (stateStr.Equals(AlchemizerState.FINISHED.ToString()))
            {
                state = AlchemizerState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished() && state != AlchemizerState.FINISHED)
            {
                sprite.SetLoop("anim");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == AlchemizerState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == AlchemizerState.FINISHED)
            {
                sprite.SetLoopIfNot("finished");
            }
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if(state == AlchemizerState.FINISHED)
            {
                InteractRight(player, area, world);
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
            if (state == AlchemizerState.IDLE)
            {
                return "Alch";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == AlchemizerState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == AlchemizerState.FINISHED)
            {
                Item dropped = ItemDict.NONE;
                if(ALCHEMY_DICTIONARY.ContainsKey(heldItem))
                {
                    dropped = ALCHEMY_DICTIONARY[heldItem];
                }
                else if(heldItem.HasTag(Item.Tag.CROP))
                {
                    dropped = ItemDict.GetCropGoldenForm(heldItem);
                } else if (heldItem.HasTag(Item.Tag.SEED))
                {
                    dropped = ItemDict.GetSeedShiningForm(heldItem);
                }

                if (dropped != ItemDict.NONE)
                {
                    area.AddEntity(new EntityItem(dropped, new Vector2(position.X, position.Y - 10)));
                } else
                {
                    player.GainGold((int)(heldItem.GetValue() * ALCHEMY_MULTIPLIER));
                    player.AddNotification(new EntityPlayer.Notification("Your " + heldItem.GetName() + " was converted into " + (int)(heldItem.GetValue() * ALCHEMY_MULTIPLIER) + " gold!", Color.Green, EntityPlayer.Notification.Length.SHORT));
                }

                sprite.SetLoop("placement");
                heldItem = ItemDict.NONE;
                state = AlchemizerState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == AlchemizerState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH))
                {
                    heldItem = addedItem;
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = AlchemizerState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I probably shouldn't try to alchemize this.", Color.Red));
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
            if (timeRemaining <= 0 && state == AlchemizerState.WORKING)
            {
                state = AlchemizerState.FINISHED;
                sprite.SetLoop("placement");
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == AlchemizerState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
