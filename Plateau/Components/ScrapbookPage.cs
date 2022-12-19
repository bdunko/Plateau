using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class ScrapbookPage
    {
        public static Vector2 PAGE_STARTING_POSITION = GameplayInterface.SCRAPBOOK_POSITION + new Vector2(89, 15);

        public abstract class Component
        {
            protected bool visible;
            protected Vector2 position;

            public abstract void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, List<QueuedString> stringsToDraw);

            public void SetVisible(bool visible)
            {
                this.visible = visible;
            }
            public void SetPosition(Vector2 position)
            {
                this.position = position + PAGE_STARTING_POSITION;
            }
        }

        public class CookingRecipeComponent : Component
        {
            public enum RecipeType
            {
                COOKING, ALCHEMY, ACCESSORY
            }

            public GameState.CookingRecipe recipe;
            private static int NEXT_PIECE_X_OFFSET = 29;
            public static Texture2D[] NUMBERS;
            private RecipeType recipeType;

            public CookingRecipeComponent(Vector2 position, GameState.CookingRecipe recipe, Texture2D[] numbers, RecipeType recipeType)
            {
                this.visible = true;
                this.position = PAGE_STARTING_POSITION + position;
                this.recipe = recipe;
                this.recipeType = recipeType;
                NUMBERS = numbers;
            }

            public override void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, List<QueuedString> stringsToDraw)
            {
                if(visible)
                {
                    sb.Draw(recipe.ingredient1.GetTexture(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position), Color.White);
                    if(recipeType == RecipeType.ALCHEMY)
                    {
                        Vector2 itemQuantityPosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(12, 10);
                        sb.Draw(NUMBERS[3], itemQuantityPosition, Color.White);
                    }
                    sb.Draw(recipe.ingredient2.GetTexture(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET, 0), Color.White);
                    sb.Draw(recipe.ingredient3.GetTexture(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET*2, 0), Color.White);
                    switch(recipeType)
                    {
                        case RecipeType.COOKING:
                            sb.Draw(recipe.length == GameState.CookingRecipe.LengthEnum.NONE ? ItemDict.CHEFS_TABLE.GetTexture() : ItemDict.CLAY_OVEN.GetTexture(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET * 3, 0), Color.White);
                            break;
                        case RecipeType.ALCHEMY:
                            sb.Draw(ItemDict.ALCHEMY_CAULDRON.GetTexture(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET * 3, 0), Color.White);
                            break;
                        case RecipeType.ACCESSORY:
                            sb.Draw(ItemDict.JEWELERS_BENCH.GetTexture(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET * 3, 0), Color.White);
                            break;
                    }
                    sb.Draw(recipe.result.GetTexture(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET*4, 0), Color.White);
                }
            }
        }

        public class BlueprintComponent : Component
        {
            public Item item;
            public Texture2D bgTex;
            private static Vector2 OFFSET = new Vector2(3, 3);

            public BlueprintComponent(Vector2 position, Texture2D blueprintTex, Item item)
            {
                this.position = PAGE_STARTING_POSITION + position;
                this.item = item;
                this.bgTex = blueprintTex;
                this.visible = true;
            }

            public override void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, List<QueuedString> stringsToDraw)
            {
                if(visible)
                {
                    sb.Draw(bgTex, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position), Color.White);
                    item.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + OFFSET, Color.White);
                }
            }
        }

        public class CraftingRecipeComponent : Component
        {
            public GameState.CraftingRecipe recipe;
            private static int NEXT_PIECE_X_OFFSET = 25;
            private static Vector2 CRAFTING_BUTTON_OFFSET = new Vector2(0.5f, 0.5f);
            public static Texture2D[] NUMBERS;
            public static Texture2D CRAFTING_BUTTON;

            public CraftingRecipeComponent(Vector2 position, GameState.CraftingRecipe recipe, Texture2D[] numbers, Texture2D craftingButton)
            {
                this.recipe = recipe;
                this.visible = true;
                this.position = PAGE_STARTING_POSITION + position;
                NUMBERS = numbers;
                CRAFTING_BUTTON = craftingButton;
            }

            public override void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, List<QueuedString> stringsToDraw)
            {
                if (visible)
                {
                    int i = 0;
                    while (i < recipe.components.Length)
                    {
                        Vector2 drawPosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET * i, 0);
                        sb.Draw(recipe.components[i].GetItem().GetTexture(), drawPosition, Color.White);
                        //DRAW NUMBERS...
                        if (recipe.components[i].GetItem().GetStackCapacity() != 1 && recipe.components[i].GetQuantity() != 0)
                        {
                            Vector2 itemQuantityPosition = new Vector2(drawPosition.X + 12, drawPosition.Y + 10);
                            sb.Draw(NUMBERS[recipe.components[i].GetQuantity() % 10], itemQuantityPosition, Color.White);
                            if (recipe.components[i].GetQuantity() >= 10)
                            {
                                itemQuantityPosition.X -= 4;
                                sb.Draw(NUMBERS[recipe.components[i].GetQuantity() / 10], itemQuantityPosition, Color.White);
                            }
                        }
                        i++;
                    }
                    sb.Draw(CRAFTING_BUTTON, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET * 4, 0) + CRAFTING_BUTTON_OFFSET, Color.White);
                    Vector2 resultPosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position) + new Vector2(NEXT_PIECE_X_OFFSET * 5, 0);
                    sb.Draw(recipe.result.GetItem().GetTexture(), resultPosition, Color.White);
                    if (recipe.result.GetItem().GetStackCapacity() != 1 && recipe.result.GetQuantity() != 0)
                    {
                        Vector2 itemQuantityPosition = new Vector2(resultPosition.X + 12, resultPosition.Y + 10);
                        sb.Draw(NUMBERS[recipe.result.GetQuantity() % 10], itemQuantityPosition, Color.White);
                        if (recipe.result.GetQuantity() >= 10)
                        {
                            itemQuantityPosition.X -= 4;
                            sb.Draw(NUMBERS[recipe.result.GetQuantity() / 10], itemQuantityPosition, Color.White);
                        }
                    }
                }
            }
        }

        public class ImageComponent : Component
        {
            public Texture2D image;
            public Color color;

            public ImageComponent(Vector2 position, Texture2D image, Color color)
            {
                this.visible = true;
                this.position = PAGE_STARTING_POSITION + position;
                this.image = image;
                this.color = color;
            }

            public override void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, List<QueuedString> stringsToDraw)
            {
                if(visible)
                {
                    sb.Draw(image, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position), color);
                }
            }
        }

        public class TextComponent : Component
        {
            public string text;
            public Color color;

            public TextComponent(Vector2 position, string text, Color color)
            {
                this.position = PAGE_STARTING_POSITION + position;
                this.text = text;
                this.color = color;
                this.visible = true;
            }

            public override void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, List<QueuedString> stringsToDraw)
            {
                if(visible)
                {
                    stringsToDraw.Add(new QueuedString(text, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, position), color, PlateauMain.FONT_SCALE));
                }
            }
        }

        private List<Component> components;
        private string name;
        private bool unlocked;

        public ScrapbookPage(string name, params Component[] components)
        {
            this.unlocked = false;
            this.name = name;
            this.components = new List<Component>();
            foreach(Component component in components)
            {
                this.components.Add(component);
            }
        }

        public void IsUnlocked(bool isUnlocked)
        {
            this.unlocked = isUnlocked;
        }

        public string GetName()
        {
            if (unlocked)
            {
                return name;
            }
            return "???";
        }

        public void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, List<QueuedString> stringsToDraw)
        {
            if (unlocked)
            {
                foreach (Component component in components)
                {
                    component.Draw(sb, cameraBoundingBox, stringsToDraw);
                }
            }
        }
    }
}
