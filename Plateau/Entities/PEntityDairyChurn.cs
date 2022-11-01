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
    public class PEntityDairyChurn : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum DairyChurnState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 3 * 60;
        private PartialRecolorSprite sprite;
        private Item heldItem;
        private int timeRemaining;
        private DairyChurnState state;
        private ResultHoverBox resultHoverBox;

        public PEntityDairyChurn(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 8, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = DairyChurnState.IDLE;
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
            save.AddData("item", heldItem.GetName());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName()));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", DairyChurnState.IDLE.ToString());
            if (stateStr.Equals(DairyChurnState.IDLE.ToString()))
            {
                state = DairyChurnState.IDLE;
            }
            else if (stateStr.Equals(DairyChurnState.WORKING.ToString()))
            {
                state = DairyChurnState.WORKING;
            }
            else if (stateStr.Equals(DairyChurnState.FINISHED.ToString()))
            {
                state = DairyChurnState.FINISHED;
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
                if (state == DairyChurnState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == DairyChurnState.FINISHED)
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
            if(heldItem != ItemDict.NONE)
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
            if(state == DairyChurnState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if(state == DairyChurnState.FINISHED)
            {
                return "Empty";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if(state == DairyChurnState.FINISHED)
            {
                area.AddEntity(new EntityItem(heldItem, new Vector2(position.X, position.Y - 10)));
                sprite.SetLoop("placement");
                heldItem = ItemDict.NONE;
                state = DairyChurnState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == DairyChurnState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH) && (addedItem == ItemDict.MILK || addedItem == ItemDict.CREAM || addedItem == ItemDict.BUTTER))
                {
                    heldItem = addedItem;
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = DairyChurnState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can only add Milk, Cream, or Butter to the Dairy Churn.", Color.Red));
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
            if (timeRemaining <= 0 && state == DairyChurnState.WORKING)
            {
                if(heldItem == ItemDict.MILK)
                {
                    heldItem = ItemDict.CREAM;
                } else if (heldItem == ItemDict.CREAM)
                {
                    heldItem = ItemDict.BUTTER;
                } else if (heldItem == ItemDict.BUTTER)
                {
                    heldItem = ItemDict.CHEESE;
                }
                state = DairyChurnState.FINISHED;
                sprite.SetLoop("placement");
            }
        }

        public HoveringInterface GetHoveringInterface()
        {
            if (state == DairyChurnState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
