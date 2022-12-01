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
    public class PEntityVivarium : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum VivariumState
        {
            IDLE, PLANTED, FINISHED
        }

        private VivariumState state;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack heldItem;

        private static int CROP_PROCESSING_TIME = 3 * 23 * 60;
        private static int FORAGE_PROCESSING_TIME = 2 * 23 * 60;

        public PEntityVivarium(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.state = VivariumState.IDLE;
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            sprite.AddLoop("planted", 4, 4, true);
            sprite.AddLoop("grown", 5, 5, true);
            this.timeRemaining = CROP_PROCESSING_TIME;
            this.resultHoverBox = new ResultHoverBox();
        }
        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.Wheat, layerDepth);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y), layerDepth);
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
            string stateStr = saveState.TryGetData("state", VivariumState.IDLE.ToString());
            if (stateStr.Equals(VivariumState.IDLE.ToString()))
            {
                state = VivariumState.IDLE;
            }
            else if (stateStr.Equals(VivariumState.PLANTED.ToString()))
            {
                state = VivariumState.PLANTED;
            }
            else if (stateStr.Equals(VivariumState.FINISHED.ToString()))
            {
                state = VivariumState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            else if (state == VivariumState.PLANTED)
            {
                sprite.SetLoopIfNot("planted");
            }
            else if (state == VivariumState.FINISHED)
            {
                sprite.SetLoopIfNot("grown");
            }

            if (state == VivariumState.FINISHED)
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
            if (state == VivariumState.IDLE)
            {
                return "Plant";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == VivariumState.FINISHED)
            {
                return "Harvest";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == VivariumState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = VivariumState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == VivariumState.IDLE)
            {
                ItemStack input = player.GetHeldItem();
                if (input.GetItem().HasTag(Item.Tag.CROP) || input.GetItem().HasTag(Item.Tag.FORAGE) || input.GetItem().HasTag(Item.Tag.SEED))
                {
                    heldItem = new ItemStack(input.GetItem(), 1);
                    input.Subtract(1);
                    state = VivariumState.PLANTED;
                    if(input.GetItem().HasTag(Item.Tag.CROP))
                    {
                        timeRemaining = CROP_PROCESSING_TIME;
                    } else if (input.GetItem().HasTag(Item.Tag.SEED))
                    {
                        timeRemaining = (int)(TEntityFarmable.GetCropForSeed(heldItem.GetItem()).growthTime * 24 * 60);
                    } else
                    {
                        timeRemaining = FORAGE_PROCESSING_TIME;
                    }
                    
                    sprite.SetLoop("placement");
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't plant this in the vivarium.", Color.Red));
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
            if (state == VivariumState.PLANTED)
            {
                timeRemaining -= minutesTicked;
                if (timeRemaining <= 0)
                {
                    if (heldItem.GetItem().HasTag(Item.Tag.SEED))
                    {
                        TEntityFarmable.CropInfo info = TEntityFarmable.GetCropForSeed(heldItem.GetItem());
                        if (info != null)
                        {
                            int quantity = info.yieldAmount;
                            if(heldItem.GetItem().HasTag(Item.Tag.SHINING_SEED))
                            {
                                heldItem = new ItemStack(Util.RandInt(1, 3) == 1 ? info.goldenYield : info.silverYield, quantity);
                            } else
                            {
                                heldItem = new ItemStack(Util.RandInt(1, 3) == 1 ? info.silverYield : info.yield, quantity);
                            }
                        }
                    }
                    else
                    {
                        heldItem.SetQuantity(Util.RandInt(2, 4));
                        sprite.SetLoop("placement");
                        state = VivariumState.FINISHED;
                    }
                }
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == VivariumState.IDLE)
            {
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
