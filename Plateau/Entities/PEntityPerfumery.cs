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
    public class PEntityPerfumery : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum PerfumeryState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 18 * 60;
        private PartialRecolorSprite sprite;
        private Item item1, item2;
        private int timeRemaining;
        private PerfumeryState state;
        private ResultHoverBox resultHoverBox;

        public PEntityPerfumery(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.item1 = ItemDict.NONE;
            this.item2 = ItemDict.NONE;
            this.sprite = sprite;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 11, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = PerfumeryState.IDLE;
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
            save.AddData("item1", item1.GetName());
            save.AddData("item2", item2.GetName());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            item1 = ItemDict.GetItemByName(saveState.TryGetData("item1", ItemDict.NONE.GetName().ToString()));
            item2 = ItemDict.GetItemByName(saveState.TryGetData("item2", ItemDict.NONE.GetName().ToString()));
            string stateStr = saveState.TryGetData("state", PerfumeryState.IDLE.ToString());
            if (stateStr.Equals(PerfumeryState.IDLE.ToString()))
            {
                state = PerfumeryState.IDLE;
            }
            else if (stateStr.Equals(PerfumeryState.WORKING.ToString()))
            {
                state = PerfumeryState.WORKING;
            }
            else if (stateStr.Equals(PerfumeryState.FINISHED.ToString()))
            {
                state = PerfumeryState.FINISHED;
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
                if (state == PerfumeryState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == PerfumeryState.FINISHED)
            {
                resultHoverBox.AssignItemStack(item1);
            }
            else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (item1 != ItemDict.NONE)
            {
                area.AddEntity(new EntityItem(item1, new Vector2(position.X, position.Y - 10)));
            }
            if (item2 != ItemDict.NONE)
            {
                area.AddEntity(new EntityItem(item2, new Vector2(position.X, position.Y - 10)));
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
            if(state == PerfumeryState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if(state == PerfumeryState.IDLE)
            {
                return "Empty";
            }
            else if(state == PerfumeryState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == PerfumeryState.FINISHED)
            {
                sprite.SetLoop("placement");
                area.AddEntity(new EntityItem(item1, new Vector2(position.X, position.Y - 10)));
                item2 = ItemDict.NONE;
                item1 = ItemDict.NONE;
                state = PerfumeryState.IDLE;
                sprite.SetLoop("placement");
            } else if (state == PerfumeryState.IDLE)
            {
                if (item1 != ItemDict.NONE)
                {
                    area.AddEntity(new EntityItem(item1, new Vector2(position.X, position.Y - 10)));
                    sprite.SetLoop("placement");
                }
                if (item2 != ItemDict.NONE)
                {
                    area.AddEntity(new EntityItem(item2, new Vector2(position.X, position.Y - 10)));
                    sprite.SetLoop("placement");
                }
                item1 = ItemDict.NONE;
                item2 = ItemDict.NONE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == PerfumeryState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH) && (addedItem == ItemDict.VANILLA_BEAN || addedItem == ItemDict.WINTERGREEN 
                    || addedItem == ItemDict.WILD_HONEY || addedItem == ItemDict.SASSAFRAS || addedItem.HasTag(Item.Tag.FLOWER)))
                {
                    player.GetHeldItem().Subtract(1);
                    if(item1 == ItemDict.NONE)
                    {
                        item1 = addedItem;
                    } else if (item2 == ItemDict.NONE)
                    {
                        item2 = addedItem;
                        state = PerfumeryState.WORKING;
                        timeRemaining = PROCESSING_TIME;
                    }
                    sprite.SetLoop("placement");
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't make perfume using this!", Color.Red));
                }
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            
        }

        public void Tick(int time, EntityPlayer player, Area area, World world)
        {
            timeRemaining -= time;
            if (timeRemaining <= 0 && state == PerfumeryState.WORKING)
            {
                item1 = GameState.PERFUMERY_RECIPE_DICT[item1][item2];
                item2 = ItemDict.NONE;
                sprite.SetLoop("placement");
                state = PerfumeryState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == PerfumeryState.IDLE || state == PerfumeryState.WORKING)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(item1),
                        new HoveringInterface.ItemStackElement(item2)));
            }
            return null;
        }
    }
}

