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
    public class PEntityAlchemyCauldron : PlacedEntity, IInteract, IHaveHoveringInterface, ITick
    {
        private enum CauldronState
        {
            IDLE, WORKING, FINISHED
        }

        private CauldronState state;
        private PartialRecolorSprite sprite;
        private Item[] ingredients;
        private ItemStack result;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private static int WORKING_DURATION = 7 * 60;

        public PEntityAlchemyCauldron(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.state = CauldronState.IDLE;
            this.ingredients = new Item[3];
            result = new ItemStack(ItemDict.NONE, 0);
            ingredients[0] = ItemDict.NONE;
            ingredients[1] = ItemDict.NONE;
            ingredients[2] = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("animation", 4, 10, false);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
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
            save.AddData("item1", ingredients[0].GetName());
            save.AddData("item2", ingredients[1].GetName());
            save.AddData("item3", ingredients[2].GetName());
            save.AddData("resultItem", result.GetItem().GetName());
            save.AddData("resultQuantity", result.GetQuantity().ToString());
            save.AddData("state", state.ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            ingredients[0] = ItemDict.GetItemByName(saveState.TryGetData("item1", ItemDict.NONE.GetName()));
            ingredients[1] = ItemDict.GetItemByName(saveState.TryGetData("item2", ItemDict.NONE.GetName()));
            ingredients[2] = ItemDict.GetItemByName(saveState.TryGetData("item3", ItemDict.NONE.GetName()));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            result = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("resultItem", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("resultQuantity", "0")));
            string stateStr = saveState.TryGetData("state", CauldronState.IDLE.ToString());
            if (stateStr.Equals(CauldronState.IDLE.ToString()))
            {
                state = CauldronState.IDLE;
            }
            else if (stateStr.Equals(CauldronState.WORKING.ToString()))
            {
                state = CauldronState.WORKING;
            }
            else if (stateStr.Equals(CauldronState.FINISHED.ToString()))
            {
                state = CauldronState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }

            if (state == CauldronState.WORKING)
            {
                sprite.SetLoopIfNot("animation");
            }

            if (state == CauldronState.FINISHED)
            {
                resultHoverBox.AssignItemStack(result);
            }
            else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (state == CauldronState.IDLE || state == CauldronState.WORKING)
            {
                for (int i = 0; i < ingredients.Length; i++)
                {
                    if (ingredients[i] != ItemDict.NONE)
                    {
                        if(i == 0)
                        {
                            area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                            area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                        }
                        area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                    }
                }
            }
            else if (state == CauldronState.FINISHED)
            {
                for (int i = 0; i < result.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(result.GetItem(), new Vector2(position.X, position.Y - 10)));
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
            if (state == CauldronState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == CauldronState.FINISHED)
            {
                return "Collect";
            }
            else if (state == CauldronState.IDLE)
            {
                return "Empty";
            }
            return "";
        }


        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == CauldronState.FINISHED)
            {
                for (int i = 0; i < result.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(result.GetItem(), new Vector2(position.X, position.Y - 10)));
                }

                for (int i = 0; i < ingredients.Length; i++)
                {
                    if(ingredients[i] == ItemDict.PHILOSOPHERS_STONE)
                    {
                        area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                    }
                    ingredients[i] = ItemDict.NONE;
                }
                result = new ItemStack(ItemDict.NONE, 0);
                sprite.SetLoop("placement");
                state = CauldronState.IDLE;
            }
            else if (state == CauldronState.IDLE)
            {
                for (int i = 0; i < ingredients.Length; i++)
                {
                    if(i == 0)
                    {
                        area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                        area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                    }
                    area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                    ingredients[i] = ItemDict.NONE;
                    sprite.SetLoop("placement");
                }
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == CauldronState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH))
                {
                    for (int i = 0; i < ingredients.Length; i++)
                    {
                        if (ingredients[i] == ItemDict.NONE)
                        {
                            if (i == 0)
                            {
                                if (player.HasItemStack(new ItemStack(player.GetHeldItem().GetItem(), 3)))
                                {
                                    ingredients[i] = addedItem;
                                    player.RemoveItemStackFromInventory(new ItemStack(player.GetHeldItem().GetItem(), 3));
                                } else
                                {
                                    player.AddNotification(new EntityPlayer.Notification("I need 3 of the catalyst item.", Color.Red, EntityPlayer.Notification.Length.SHORT));
                                }
                            } else {
                                player.GetHeldItem().Subtract(1);
                                ingredients[i] = addedItem;
                            }
                            break;
                        }
                    }
                    sprite.SetLoop("placement");

                    if (ingredients[2] != ItemDict.NONE)
                    {
                        GameState.CookingRecipe recipe = GameState.GetAlchemyRecipe(ingredients[0], ingredients[1], ingredients[2]);
                        if (recipe != null)
                        {
                            timeRemaining = WORKING_DURATION;
                            state = CauldronState.WORKING;
                            sprite.SetLoop("animation");
                        }
                        else
                        {
                            for (int i = 0; i < ingredients.Length; i++)
                            {
                                if(i == 0)
                                {
                                    area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                                    area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                                }
                                area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                                ingredients[i] = ItemDict.NONE;
                            }
                            player.AddNotification(new EntityPlayer.Notification("This combination of items doesn't seem to work...", Color.Red));
                        }
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't add this!", Color.Red));
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
            if (timeRemaining <= 0 && state == CauldronState.WORKING)
            {
                result = new ItemStack(GameState.GetAlchemyRecipe(ingredients[0], ingredients[1], ingredients[2]).result, 1);
                sprite.SetLoop("placement");
                state = CauldronState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface()
        {
            if (state == CauldronState.IDLE || state == CauldronState.WORKING)
            {
                return new HoveringInterface(
                        new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(new ItemStack(ingredients[0], 3)),
                        new HoveringInterface.ItemStackElement(ingredients[1]),
                        new HoveringInterface.ItemStackElement(ingredients[2])));
            }
            return new HoveringInterface();
        }
    }
}