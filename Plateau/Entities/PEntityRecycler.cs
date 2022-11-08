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
    public class PEntityRecycler : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum RecyclerState
        {
            IDLE, WORKING, FINISHED
        }

        private static int PROCESSING_TIME = 1 * 60 + 30;
        private PartialRecolorSprite sprite;
        private Item heldItem;
        private int timeRemaining;
        private RecyclerState state;

        public PEntityRecycler(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 4, 6, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = RecyclerState.IDLE;
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
            string stateStr = saveState.TryGetData("state", RecyclerState.IDLE.ToString());
            if (stateStr.Equals(RecyclerState.IDLE.ToString()))
            {
                state = RecyclerState.IDLE;
            }
            else if (stateStr.Equals(RecyclerState.WORKING.ToString()))
            {
                state = RecyclerState.WORKING;
            }
            else if (stateStr.Equals(RecyclerState.FINISHED.ToString()))
            {
                state = RecyclerState.FINISHED;
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
                if (state == RecyclerState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (state == RecyclerState.FINISHED)
            {
                InteractRight(player, area, world);
            }
            else if (heldItem != ItemDict.NONE)
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
            if (state == RecyclerState.IDLE)
            {
                return "Recycle";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == RecyclerState.FINISHED)
            {
                return "Take";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == RecyclerState.FINISHED)
            {
                GameState.CraftingRecipe cRecipe = GameState.GetCraftingRecipeForResult(heldItem);
                if(cRecipe == null)
                {
                    GameState.CookingRecipe recipe = GameState.GetCookingRecipeForResult(heldItem);
                    if (recipe == null)
                    {
                        recipe = GameState.GetAccessoryRecipeForResult(heldItem);
                        if (recipe == null)
                        {
                            recipe = GameState.GetAlchemyRecipeForResult(heldItem);
                        }
                        if(recipe == null)
                        {
                            throw new Exception("No recipe found for Recycler!");
                        }
                    }
                    area.AddEntity(new EntityItem(recipe.ingredient1, new Vector2(position.X, position.Y - 10)));
                    area.AddEntity(new EntityItem(recipe.ingredient2, new Vector2(position.X, position.Y - 10)));
                    area.AddEntity(new EntityItem(recipe.ingredient3, new Vector2(position.X, position.Y - 10)));
                } else
                {
                    for(int i = 0; i < cRecipe.components.Length; i++)
                    {
                        ItemStack toGive = cRecipe.components[i];
                        for(int j = 0; j < toGive.GetQuantity(); j ++)
                        {
                            area.AddEntity(new EntityItem(toGive.GetItem(), new Vector2(position.X, position.Y - 10)));
                        }
                    }
                }
                sprite.SetLoop("placement");
                heldItem = ItemDict.NONE;
                state = RecyclerState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == RecyclerState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                bool hasRecipe = false;

                GameState.CraftingRecipe cRecipe = GameState.GetCraftingRecipeForResult(addedItem);
                if (cRecipe == null)
                {
                    GameState.CookingRecipe recipe = GameState.GetCookingRecipeForResult(addedItem);
                    if (recipe == null)
                    {
                        recipe = GameState.GetAccessoryRecipeForResult(addedItem);
                        if (recipe == null)
                        {
                            recipe = GameState.GetAlchemyRecipeForResult(addedItem);
                        }
                    }

                    if(recipe != null)
                    {
                        hasRecipe = true;
                    }
                } else
                {
                    hasRecipe = true;
                }

                if (hasRecipe)
                {
                    heldItem = addedItem;
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = RecyclerState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("This item can't be recycled.", Color.Red));
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
            if (timeRemaining <= 0 && state == RecyclerState.WORKING)
            {
                state = RecyclerState.FINISHED;
                sprite.SetLoop("placement");
            }
        }

        public HoveringInterface GetHoveringInterface()
        {
            if (state == RecyclerState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
