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
    public class PEntityOrigamiStation : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum OrigamiState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 4 * 60;
        private PartialRecolorSprite sprite;
        private Item item1, item2;
        private int timeRemaining;
        private OrigamiState state;
        private ResultHoverBox resultHoverBox;

        private static Dictionary<Item, Item> ORIGAMI_DICTIONARY = new Dictionary<Item, Item> {
            {ItemDict.NAVY_DYE, ItemDict.ORIGAMI_HEART},
            {ItemDict.BLUE_DYE, ItemDict.ORIGAMI_WHALE},
            {ItemDict.BLACK_DYE, ItemDict.ORIGAMI_BOX},
            {ItemDict.RED_DYE, ItemDict.ORIGAMI_KITE},
            {ItemDict.PINK_DYE, ItemDict.ORIGAMI_FAN},
            {ItemDict.LIGHT_BROWN_DYE, ItemDict.ORIGAMI_BEETLE},
            {ItemDict.DARK_BROWN_DYE, ItemDict.ORIGAMI_FROG},
            {ItemDict.ORANGE_DYE, ItemDict.ORIGAMI_TIGER},
            {ItemDict.YELLOW_DYE, ItemDict.ORIGAMI_LION},
            {ItemDict.PURPLE_DYE, ItemDict.ORIGAMI_FLOWER},
            {ItemDict.GREEN_DYE, ItemDict.ORIGAMI_LEAF},
            {ItemDict.OLIVE_DYE, ItemDict.ORIGAMI_TURTLE},
            {ItemDict.WHITE_DYE, ItemDict.ORIGAMI_SWAN},
            {ItemDict.LIGHT_GREY_DYE, ItemDict.ORIGAMI_AIRPLANE},
            {ItemDict.DARK_GREY_DYE, ItemDict.ORIGAMI_SAILBOAT},
            {ItemDict.UN_DYE, ItemDict.ORIGAMI_BALL},
            {ItemDict.CYAN_DYE, ItemDict.ORIGAMI_FISH},
            {ItemDict.CRIMSON_DYE, ItemDict.ORIGAMI_DRAGON},
            {ItemDict.WHEAT_DYE, ItemDict.ORIGAMI_RABBIT},
            {ItemDict.MINT_DYE, ItemDict.ORIGAMI_CANDY},

        };

        public PEntityOrigamiStation(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.item1 = ItemDict.NONE;
            this.item2 = ItemDict.NONE;
            this.sprite = sprite;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = OrigamiState.IDLE;
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
            save.AddData("item1", item1.GetName());
            save.AddData("item2", item2.GetName());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            item1 = ItemDict.GetItemByName(saveState.TryGetData("item1", ItemDict.NONE.GetName().ToString()));
            item2 = ItemDict.GetItemByName(saveState.TryGetData("item2", ItemDict.NONE.GetName().ToString()));
            string stateStr = saveState.TryGetData("state", OrigamiState.IDLE.ToString());
            if (stateStr.Equals(OrigamiState.IDLE.ToString()))
            {
                state = OrigamiState.IDLE;
            }
            else if (stateStr.Equals(OrigamiState.WORKING.ToString()))
            {
                state = OrigamiState.WORKING;
            }
            else if (stateStr.Equals(OrigamiState.FINISHED.ToString()))
            {
                state = OrigamiState.FINISHED;
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
                if (state == OrigamiState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if (state == OrigamiState.FINISHED)
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
            if (state == OrigamiState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == OrigamiState.IDLE)
            {
                return "Empty";
            }
            else if (state == OrigamiState.FINISHED)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == OrigamiState.FINISHED)
            {
                sprite.SetLoop("placement");
                area.AddEntity(new EntityItem(item1, new Vector2(position.X, position.Y - 10)));
                item2 = ItemDict.NONE;
                item1 = ItemDict.NONE;
                state = OrigamiState.IDLE;
                sprite.SetLoop("placement");
            }
            else if (state == OrigamiState.IDLE)
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
            if (state == OrigamiState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if(item1 == ItemDict.NONE)
                {
                    if(addedItem == ItemDict.PAPER)
                    {
                        player.GetHeldItem().Subtract(1);
                        item1 = addedItem;
                        sprite.SetLoop("placement");
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I should start by adding some paper.", Color.Red));
                    }
                } else //item 2
                {
                    if(ORIGAMI_DICTIONARY.ContainsKey(addedItem))
                    {
                        item2 = addedItem;
                        state = OrigamiState.WORKING;
                        timeRemaining = PROCESSING_TIME;
                        sprite.SetLoop("placement");
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I should choose a dye to use.", Color.Red));
                    }
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
            if (timeRemaining <= 0 && state == OrigamiState.WORKING)
            {
                item1 = ORIGAMI_DICTIONARY[item2];
                item2 = ItemDict.NONE;
                sprite.SetLoop("placement");
                state = OrigamiState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == OrigamiState.IDLE || state == OrigamiState.WORKING)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(item1),
                        new HoveringInterface.ItemStackElement(item2)));
            }
            return new HoveringInterface();
        }
    }
}

