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
    public class PEntityTerrarium : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum TerrariumState
        {
            IDLE, WORKING, FINISHED
        }

        private static int INSECT_CAPACITY_RARE = 5;
        private static int INSECT_CAPACITY_NORMAL = 10;
        private static int INSECT_TIME_RARE = 2 * 23 * 60;
        private static int INSECt_TIME_NORMAL = 12 * 60;

        private Item[] RARE_INSECTS = { ItemDict.STAG_BEETLE, ItemDict.JEWEL_SPIDER, ItemDict.EMPRESS_BUTTERFLY };

        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private static float TIME_BETWEEN_IDLE_ANIM = 8.0f;
        private float idleAnimTimer;
        private TerrariumState state;
        private ResultHoverBox resultHoverBox;

        public PEntityTerrarium(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.idleAnimTimer = 0;
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("anim", 4, 10, false);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = TerrariumState.IDLE;
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
            string stateStr = saveState.TryGetData("state", TerrariumState.IDLE.ToString());
            if (stateStr.Equals(TerrariumState.IDLE.ToString()))
            {
                state = TerrariumState.IDLE;
            }
            else if (stateStr.Equals(TerrariumState.WORKING.ToString()))
            {
                state = TerrariumState.WORKING;
            }
            else if (stateStr.Equals(TerrariumState.FINISHED.ToString()))
            {
                state = TerrariumState.FINISHED;
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

            if (state == TerrariumState.IDLE)
            {
                idleAnimTimer += deltaTime;
                if (idleAnimTimer >= TIME_BETWEEN_IDLE_ANIM)
                {
                    sprite.SetLoopIfNot("anim");
                }
            }

            if (state == TerrariumState.FINISHED)
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
            if (state == TerrariumState.IDLE)
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
                state = TerrariumState.WORKING;
                timeRemaining = RARE_INSECTS.Contains(heldItem.GetItem()) ? INSECT_TIME_RARE : INSECt_TIME_NORMAL;
            }
            else if (heldItem.GetItem() != ItemDict.NONE && heldItem.GetQuantity() == 2)
            {
                for(int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = TerrariumState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() == ItemDict.NONE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (addedItem.HasTag(Item.Tag.INSECT))
                {
                    if (player.HasItemStack(new ItemStack(addedItem, 2))) {
                        player.RemoveItemStackFromInventory(new ItemStack(addedItem, 2));
                        heldItem = new ItemStack(addedItem, 2);
                        sprite.SetLoop("placement");
                        state = TerrariumState.WORKING;
                        timeRemaining = RARE_INSECTS.Contains(heldItem.GetItem()) ? INSECT_TIME_RARE : INSECt_TIME_NORMAL;
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I need to add a starting pair of two insects of the same kind.", Color.Red));
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't put non-insects into the terrarium!", Color.Red));
                }
            }
            else
            {
                if (heldItem.GetItem() == player.GetHeldItem().GetItem())
                {
                    player.AddNotification(new EntityPlayer.Notification("Two insects is enough; I don't need to add more.", Color.Red));
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I should empty it before adding other insects.", Color.Red));
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
            if (timeRemaining <= 0 && state == TerrariumState.WORKING)
            {
                heldItem = new ItemStack(heldItem.GetItem(), heldItem.GetQuantity() + 1);
                sprite.SetLoop("placement");
                timeRemaining = RARE_INSECTS.Contains(heldItem.GetItem()) ? INSECT_TIME_RARE : INSECt_TIME_NORMAL;
                if (heldItem.GetQuantity() == (RARE_INSECTS.Contains(heldItem.GetItem()) ? INSECT_CAPACITY_RARE : INSECT_CAPACITY_NORMAL))
                {
                    state = TerrariumState.FINISHED;
                }
            }
        }

        public HoveringInterface GetHoveringInterface()
        {
            if (state == TerrariumState.WORKING || state == TerrariumState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
