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
    public class PEntityChefsTable : PlacedEntity, IInteract, IHaveHoveringInterface
    {
        private PartialRecolorSprite sprite;
        private Item[] ingredients;
        private static float TIME_BETWEEN_ANIMATION = 5.0f;
        private float timeSinceAnimation = 0;

        public PEntityChefsTable(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.ingredients = new Item[3];
            ingredients[0] = ItemDict.NONE;
            ingredients[1] = ItemDict.NONE;
            ingredients[2] = ItemDict.NONE;
            this.timeSinceAnimation = 0;
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("animation", 4, 6, false);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("item1", ingredients[0].GetName());
            save.AddData("item2", ingredients[1].GetName());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            ingredients[0] = ItemDict.GetItemByName(saveState.TryGetData("item1", ItemDict.NONE.GetName()));
            ingredients[1] = ItemDict.GetItemByName(saveState.TryGetData("item2", ItemDict.NONE.GetName()));
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
                timeSinceAnimation += deltaTime;
                if(timeSinceAnimation >= TIME_BETWEEN_ANIMATION)
                {
                    sprite.SetLoop("animation");
                    timeSinceAnimation = 0;
                }
            }
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            for(int i = 0; i < ingredients.Length; i++)
            {
                if(ingredients[i] != ItemDict.NONE)
                {
                    area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
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
            return "Add";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Empty";
        }


        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            for(int i = 0; i < ingredients.Length; i++)
            {
                if(ingredients[i] != ItemDict.NONE)
                {
                    sprite.SetLoop("placement");
                    area.AddEntity(new EntityItem(ingredients[i], new Vector2(position.X, position.Y - 10)));
                }
                ingredients[i] = ItemDict.NONE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            Item addedItem = player.GetHeldItem().GetItem();
            if (!addedItem.HasTag(Item.Tag.NO_TRASH))
            {
                for(int i = 0; i < ingredients.Length; i++)
                {
                    if(ingredients[i] == ItemDict.NONE)
                    {
                        player.GetHeldItem().Subtract(1);
                        ingredients[i] = addedItem;
                        sprite.SetLoop("placement");
                        break;
                    }
                }

                if (ingredients[2] != ItemDict.NONE)
                {
                    Item[] originalIngredients = { ingredients[0], ingredients[1], ingredients[2] };

                    float resultMultiplier = 1.0f;
                    for(int i = 0; i < ingredients.Length; i++)
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

                    GameState.CookingRecipe recipe = GameState.GetTableCookingRecipe(ingredients[0], ingredients[1], ingredients[2]);
                    if (recipe != null)
                    {
                        while (resultMultiplier != 0)
                        {
                            if (resultMultiplier < 1)
                            {
                                if (resultMultiplier >= Util.RandInt(0, 100) / 100.0f)
                                {
                                    area.AddEntity(new EntityItem(recipe.result, new Vector2(position.X, position.Y - 10)));
                                }
                                resultMultiplier = 0;
                            }
                            else
                            {
                                area.AddEntity(new EntityItem(recipe.result, new Vector2(position.X, position.Y - 10)));
                                resultMultiplier--;
                            }
                        }
                        for (int i = 0; i < ingredients.Length; i++)
                        {
                            ingredients[i] = ItemDict.NONE;
                        }
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

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            return new HoveringInterface(
                    new HoveringInterface.Row(
                    new HoveringInterface.ItemStackElement(ingredients[0]),
                    new HoveringInterface.ItemStackElement(ingredients[1]),
                    new HoveringInterface.ItemStackElement(ingredients[2])));
        }
    }
}