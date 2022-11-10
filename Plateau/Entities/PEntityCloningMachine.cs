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
    public class PEntityCloningMachine : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum ClonerState
        {
            IDLE, WORKING, FINISHED
        }

        private static int MINIMUM_PROCESSING_TIME = 1440;
        private ClonerState state;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack heldItem;

        public PEntityCloningMachine(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.state = ClonerState.IDLE;
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            sprite.AddLoop("working", 4, 11, true);
            this.timeRemaining = 0;
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
            string stateStr = saveState.TryGetData("state", ClonerState.IDLE.ToString());
            if (stateStr.Equals(ClonerState.IDLE.ToString()))
            {
                state = ClonerState.IDLE;
            }
            else if (stateStr.Equals(ClonerState.WORKING.ToString()))
            {
                state = ClonerState.WORKING;
            }
            else if (stateStr.Equals(ClonerState.FINISHED.ToString()))
            {
                state = ClonerState.FINISHED;
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
                if (state == ClonerState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else if (state == ClonerState.FINISHED)
                {
                    sprite.SetLoopIfNot("idle");
                }
                else
                {
                    sprite.SetLoopIfNot("idle");
                }
            }
            if (state == ClonerState.FINISHED)
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
            if (state == ClonerState.IDLE)
            {
                return "Seed";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == ClonerState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == ClonerState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = ClonerState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == ClonerState.IDLE)
            {
                ItemStack input = player.GetHeldItem();

                heldItem = new ItemStack(input.GetItem(), 1);
                input.Subtract(1);
                state = ClonerState.WORKING;
                timeRemaining = heldItem.GetItem().GetValue();
                if(timeRemaining < MINIMUM_PROCESSING_TIME)
                {
                    timeRemaining = MINIMUM_PROCESSING_TIME;
                }
                sprite.SetLoop("placement");
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
            if (state == ClonerState.WORKING)
            {
                timeRemaining -= minutesTicked;
                if (timeRemaining <= 0)
                {
                    heldItem.SetQuantity(Util.RandInt(2, 3));
                    sprite.SetLoop("placement");
                    state = ClonerState.FINISHED;
                }
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == ClonerState.IDLE)
            {
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
