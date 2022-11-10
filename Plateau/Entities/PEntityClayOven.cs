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
    public class PEntityClayOven : PlacedEntity, IInteract, IHaveHoveringInterface, ITick
    {
        private enum OvenState
        {
            IDLE, WORKING, FINISHED
        }

        private OvenState state;
        private PartialRecolorSprite sprite;
        private Item[] ingredients;
        private ItemStack result;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private static int SHORT_COOK_DURATION = 1 * 60;
        private static int MEDIUM_COOK_DURATION = 3 * 60;
        private static int LONG_COOK_DURATION = 6 * 60;

        public PEntityClayOven(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.state = OvenState.IDLE;
            this.ingredients = new Item[3];
            result = new ItemStack(ItemDict.NONE, 0);
            ingredients[0] = ItemDict.NONE;
            ingredients[1] = ItemDict.NONE;
            ingredients[2] = ItemDict.NONE;
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("animation", 4, 7, false);
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
            string stateStr = saveState.TryGetData("state", OvenState.IDLE.ToString());
            if (stateStr.Equals(OvenState.IDLE.ToString()))
            {
                state = OvenState.IDLE;
            }
            else if (stateStr.Equals(OvenState.WORKING.ToString()))
            {
                state = OvenState.WORKING;
            }
            else if (stateStr.Equals(OvenState.FINISHED.ToString()))
            {
                state = OvenState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            
            if (state == OvenState.WORKING)
            {
                sprite.SetLoopIfNot("animation");
            }

            if(state == OvenState.FINISHED)
            {
                resultHoverBox.AssignItemStack(result);
            } else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (state == OvenState.IDLE || state == OvenState.WORKING)
            {
                for (int i = 0; i < ingredients.Length; i++)
                {
                    if (ingredients[i] != ItemDict.NONE)
                    {
                        area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                    }
                }
            } else if (state == OvenState.FINISHED)
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
            if (state == OvenState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if(state == OvenState.FINISHED)
            {
                return "Collect";
            } else if (state == OvenState.IDLE)
            {
                return "Empty";
            }
            return "";
        }


        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == OvenState.FINISHED)
            {
                for (int i = 0; i < result.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(result.GetItem(), new Vector2(position.X, position.Y - 10)));
                }

                for (int i = 0; i < ingredients.Length; i++)
                {
                    ingredients[i] = ItemDict.NONE;
                }
                result = new ItemStack(ItemDict.NONE, 0);
                sprite.SetLoop("placement");
                state = OvenState.IDLE;
                GameState.STATISTICS[GameState.STAT_TIMES_COOKED] += 1;
            } else if (state == OvenState.IDLE)
            {
                for (int i = 0; i < ingredients.Length; i++)
                {
                    if (ingredients[i] != ItemDict.NONE)
                    {
                        area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                        ingredients[i] = ItemDict.NONE;
                        sprite.SetLoop("placement");
                    }
                }
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == OvenState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH))
                {
                    for (int i = 0; i < ingredients.Length; i++)
                    {
                        if (ingredients[i] == ItemDict.NONE)
                        {
                            player.GetHeldItem().Subtract(1);
                            ingredients[i] = addedItem;
                            break;
                        }
                    }
                    sprite.SetLoop("placement");

                    if (ingredients[2] != ItemDict.NONE)
                    {
                        Item[] originalIngredients = { ingredients[0], ingredients[1], ingredients[2] };

                        for (int i = 0; i < ingredients.Length; i++)
                        {
                            Item ingr = ingredients[i];

                            if (ingr == ItemDict.GOLDEN_EGG)
                            {
                                ingredients[i] = ItemDict.EGG;
                            }
                            else if (ingr.HasTag(Item.Tag.SILVER_CROP))
                            {
                                ingredients[i] = ItemDict.GetCropBaseForm(ingr);
                            }
                            else if (ingr.HasTag(Item.Tag.GOLDEN_CROP))
                            {
                                ingredients[i] = ItemDict.GetCropBaseForm(ingr);
                            }
                            else if (ingr.HasTag(Item.Tag.PHANTOM_CROP))
                            {
                                ingredients[i] = ItemDict.GetCropBaseForm(ingr);
                            }
                        }

                        GameState.CookingRecipe recipe = GameState.GetOvenCookingRecipe(ingredients[0], ingredients[1], ingredients[2]);
                        if (recipe != null)
                        {
                            switch(recipe.length)
                            {
                                case GameState.CookingRecipe.LengthEnum.SHORT:
                                    timeRemaining = SHORT_COOK_DURATION;
                                    break;
                                case GameState.CookingRecipe.LengthEnum.MEDIUM:
                                    timeRemaining = MEDIUM_COOK_DURATION;
                                    break;
                                case GameState.CookingRecipe.LengthEnum.LONG:
                                    timeRemaining = LONG_COOK_DURATION;
                                    break;
                                case GameState.CookingRecipe.LengthEnum.NONE:
                                    throw new Exception("OVEN RECIPE WITH NONE LENGTH!");
                            }
                            
                            for(int i = 0; i < ingredients.Length; i++)
                            {
                                ingredients[i] = originalIngredients[i];
                            }
                            state = OvenState.WORKING;
                            sprite.SetLoop("animation");
                        }
                        else
                        {
                            for (int i = 0; i < originalIngredients.Length; i++)
                            {
                                area.AddEntity(new EntityItem(originalIngredients[i], new Vector2(position.X, position.Y - 10)));
                                ingredients[i] = ItemDict.NONE;
                            }
                            player.AddNotification(new EntityPlayer.Notification("This combination of ingredients doesn't seem to be a recipe...", Color.Red));
                        }
                    }
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't cook with this!", Color.Red));
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
            if (timeRemaining <= 0 && state == OvenState.WORKING)
            {
                Item[] originalIngredients = { ingredients[0], ingredients[1], ingredients[2] };

                float resultMultiplier = 1.0f;
                for (int i = 0; i < ingredients.Length; i++)
                {
                    Item ingr = ingredients[i];

                    if (ingr == ItemDict.GOLDEN_EGG)
                    {
                        ingredients[i] = ItemDict.EGG;
                        resultMultiplier += 0.5f;
                    }
                    else if (ingr.HasTag(Item.Tag.SILVER_CROP))
                    {
                        ingredients[i] = ItemDict.GetCropBaseForm(ingr);
                        resultMultiplier += 0.5f;
                    }
                    else if (ingr.HasTag(Item.Tag.GOLDEN_CROP))
                    {
                        ingredients[i] = ItemDict.GetCropBaseForm(ingr);
                        resultMultiplier += 1f;
                    }
                    else if (ingr.HasTag(Item.Tag.PHANTOM_CROP))
                    {
                        ingredients[i] = ItemDict.GetCropBaseForm(ingr);
                        resultMultiplier += 1.5f;
                    }
                }

                int q = 0;
                GameState.CookingRecipe recipe = GameState.GetOvenCookingRecipe(ingredients[0], ingredients[1], ingredients[2]);
                while (resultMultiplier != 0)
                {
                    if (resultMultiplier < 1)
                    {
                        if (resultMultiplier >= Util.RandInt(0, 100) / 100.0f)
                        {
                            q++;
                            area.AddEntity(new EntityItem(recipe.result, new Vector2(position.X, position.Y - 10)));
                        }
                        resultMultiplier = 0;
                    }
                    else
                    {
                        q++;
                        resultMultiplier--;
                    }
                }

                result = new ItemStack(recipe.result, q);
                sprite.SetLoop("placement");
                state = OvenState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == OvenState.IDLE || state == OvenState.WORKING)
            {
                return new HoveringInterface(
                        new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(ingredients[0]),
                        new HoveringInterface.ItemStackElement(ingredients[1]),
                        new HoveringInterface.ItemStackElement(ingredients[2])));
            }
            return new HoveringInterface();
        }
    }
}