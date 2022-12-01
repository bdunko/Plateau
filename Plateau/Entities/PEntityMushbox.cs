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
    public class PEntityMushbox : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum MushboxState
        {
            IDLE, WORKING, FINISHED
        }

        private MushboxState state;
        private static int MIN = 2;
        private static int MAX = 4;
        private static int PROCESSING_TIME = 11 * 60;
        private static float TIME_BETWEEN_IDLE_ANIM = 8.0f;
        private float idleAnimTimer;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack heldItem;

        public PEntityMushbox(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.idleAnimTimer = 0;
            this.state = MushboxState.IDLE;
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            sprite.AddLoop("idle_anim", 4, 8, false);
            sprite.AddLoop("anim", 9, 13, true);
            this.timeRemaining = PROCESSING_TIME;
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
            string stateStr = saveState.TryGetData("state", MushboxState.IDLE.ToString());
            if (stateStr.Equals(MushboxState.IDLE.ToString()))
            {
                state = MushboxState.IDLE;
            }
            else if (stateStr.Equals(MushboxState.WORKING.ToString()))
            {
                state = MushboxState.WORKING;
            }
            else if (stateStr.Equals(MushboxState.FINISHED.ToString()))
            {
                state = MushboxState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
                idleAnimTimer = 0;
            }
            if (state == MushboxState.WORKING)
            {
                sprite.SetLoopIfNot("anim");
            }

            if(state == MushboxState.IDLE)
            {
                idleAnimTimer += deltaTime;
                if(idleAnimTimer >= TIME_BETWEEN_IDLE_ANIM)
                {
                    sprite.SetLoopIfNot("idle_anim");
                }
            }

            if (state == MushboxState.FINISHED)
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
            if (state == MushboxState.IDLE)
            {
                return "Spore";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == MushboxState.FINISHED)
            {
                return "Take";
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
                state = MushboxState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == MushboxState.IDLE)
            {
                ItemStack input = player.GetHeldItem();
                if (input.GetItem() == ItemDict.SPORES)
                {
                    heldItem = new ItemStack(input.GetItem(), 1);
                    input.Subtract(1);
                    state = MushboxState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                    sprite.SetLoop("placement");
                } else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can only add spores to a mushbox.", Color.Red));
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
            if (state == MushboxState.WORKING)
            {
                timeRemaining -= minutesTicked;
                if (timeRemaining <= 0)
                {
                    Item result = ItemDict.MOREL;
                    switch(Util.RandInt(0, 10))
                    {
                        case 0:
                            result = ItemDict.TRUFFLE;
                            break;
                        case 1:
                        case 2:
                        case 3:
                            result = ItemDict.OYSTER_MUSHROOM;
                            break;
                        case 4:
                        case 5:
                            result = ItemDict.CHANTERELLE;
                            break;
                        case 6:
                            result = ItemDict.SHIITAKE;
                            break;
                        case 7:
                        case 8:
                            result = ItemDict.CAVE_FUNGI;
                            break;
                        case 9:
                        case 10:
                            result = ItemDict.MOREL;
                            break;
                    }
                    heldItem = new ItemStack(result, Util.RandInt(MIN, MAX));
                    sprite.SetLoop("placement");
                    state = MushboxState.FINISHED;
                }
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == MushboxState.IDLE)
            {
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}
