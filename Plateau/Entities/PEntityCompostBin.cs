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
    class PEntityCompostBin : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum CompostBinState
        {
            IDLE, FINISHED, WORKING
        }

        private static int PROCESSING_TIME = 44 * 60;
        private static int SIZE = 10;
        private PartialRecolorSprite sprite;
        private Item[] contents;
        private int timeRemaining;
        private CompostBinState state;
        private ResultHoverBox resultHoverBox;

        public PEntityCompostBin(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 0, true);
            sprite.AddLoop("working", 0, 4, true);
            sprite.AddLoop("placement", 5, 7, false);
            sprite.SetLoop("placement");
            this.state = CompostBinState.IDLE;
            this.timeRemaining = 0;
            this.contents = new Item[SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                contents[i] = ItemDict.NONE;
            }
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
            save.AddData("timeRemaining", timeRemaining.ToString());
            for (int i = 0; i < SIZE; i++)
            {
                save.AddData("inventory" + i, contents[i].GetName());
            }
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            for(int i = 0; i < SIZE; i++)
            {
                contents[i] = ItemDict.GetItemByName(saveState.TryGetData("inventory" + i, ItemDict.NONE.GetName()));
            }
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", CompostBinState.IDLE.ToString());
            if(stateStr.Equals(CompostBinState.IDLE.ToString()))
            {
                state = CompostBinState.IDLE;
            }
            else if (stateStr.Equals(CompostBinState.WORKING.ToString()))
            {
                state = CompostBinState.WORKING;
            }
            else if (stateStr.Equals(CompostBinState.FINISHED.ToString()))
            {
                state = CompostBinState.FINISHED;
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
                if (state == CompostBinState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("anim");
                }
            }
            if(state == CompostBinState.FINISHED)
            {
                resultHoverBox.AssignItemStack(new ItemStack(contents[0], 10));
            } else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            for (int i = 0; i < SIZE; i++)
            {
                if (contents[i] != ItemDict.NONE)
                {
                    area.AddEntity(new EntityItem(contents[i], new Vector2(position.X, position.Y - 10)));
                    contents[i] = ItemDict.NONE;
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
            if (state == CompostBinState.IDLE && contents[SIZE-1] == ItemDict.NONE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if(state == CompostBinState.FINISHED || state == CompostBinState.IDLE)
            {
                return "Empty";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            for (int i = 0; i < SIZE; i++)
            {
                if (contents[i] != ItemDict.NONE)
                {
                    area.AddEntity(new EntityItem(contents[i], new Vector2(position.X, position.Y - 10)));
                    contents[i] = ItemDict.NONE;
                    sprite.SetLoop("placement");
                }
            }
            state = CompostBinState.IDLE;
        }

        private bool IsValidCompostItem(Item item)
        {
            return (item.HasTag(Item.Tag.INSECT) || item.HasTag(Item.Tag.SEED) || item.HasTag(Item.Tag.CROP) || item.HasTag(Item.Tag.FORAGE) || item.HasTag(Item.Tag.VEGETABLE) ||
                item.HasTag(Item.Tag.FRUIT) || item.HasTag(Item.Tag.ANIMAL_PRODUCT) || item.HasTag(Item.Tag.FOOD) || item.HasTag(Item.Tag.FISH) || 
                item.HasTag(Item.Tag.MUSHROOM) || item.HasTag(Item.Tag.PERFUME) || item.HasTag(Item.Tag.GEM) ||
                item == ItemDict.WILD_HONEY || item == ItemDict.WEEDS || item == ItemDict.WILD_MEAT || item == ItemDict.CLAY || item == ItemDict.SNOW_CRYSTAL) &&
                item != ItemDict.GOLDEN_WOOL && item != ItemDict.WOOL;
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if(state == CompostBinState.IDLE && contents[SIZE-1] == ItemDict.NONE)
            {
                Item addedItem = player.GetHeldItem().GetItem();
                if (!addedItem.HasTag(Item.Tag.NO_TRASH) && IsValidCompostItem(addedItem))
                {
                    int i = 0;
                    while (i < SIZE)
                    {
                        if (contents[i] == ItemDict.NONE)
                        {
                            contents[i] = addedItem;
                            player.GetHeldItem().Subtract(1);
                            sprite.SetLoop("placement");
                            break;
                        }
                        i++;
                    }
                    if(contents[SIZE-1] != ItemDict.NONE)
                    {
                        state = CompostBinState.WORKING;
                        timeRemaining = PROCESSING_TIME;
                    }
                } else
                {
                    if (addedItem != ItemDict.NONE)
                    {
                        player.AddNotification(new EntityPlayer.Notification("I can't add that to the compost bin.", Color.Red));
                    } else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I should add biodegradable stuff to this compost bin.", Color.Red));
                    }
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
            if(timeRemaining <= 0 && state == CompostBinState.WORKING)
            {
                int loamyScore = 0, qualityScore = 0, dewScore = 0, sweetScore = 0, decayScore = 0, frostScore = 0, thickScore = 0, shiningScore = 0;
                Item result = ItemDict.LOAMY_COMPOST;
                for (int i = 0; i < SIZE; i++)
                {
                    Item item = contents[i];
                    loamyScore += (item == ItemDict.WEEDS || item.HasTag(Item.Tag.INSECT) || item.HasTag(Item.Tag.SEED)) ? 1 : 0;
                    qualityScore += (item.HasTag(Item.Tag.CROP) || item.HasTag(Item.Tag.VEGETABLE) || item.HasTag(Item.Tag.FORAGE)) ? 1 : 0;
                    dewScore += (item.HasTag(Item.Tag.FISH)) ? 1 : 0;
                    sweetScore += (item.HasTag(Item.Tag.PERFUME) || item.HasTag(Item.Tag.FRUIT) || item == ItemDict.WILD_HONEY) ? 1 : 0;
                    decayScore += (item.HasTag(Item.Tag.FOOD) || item == ItemDict.WILD_MEAT || item.HasTag(Item.Tag.ANIMAL_PRODUCT)) ? 1 : 0;
                    frostScore += (item == ItemDict.SNOW_CRYSTAL) ? 1 : 0;
                    thickScore += (item == ItemDict.CLAY || item.HasTag(Item.Tag.MUSHROOM)) ? 1 : 0;
                    shiningScore += (item.HasTag(Item.Tag.GEM) || item.HasTag(Item.Tag.GOLDEN_CROP) || item.HasTag(Item.Tag.SILVER_CROP) || item.HasTag(Item.Tag.PHANTOM_CROP) || item.HasTag(Item.Tag.RARE)) ? 1 : 0;

                    int largestScore = -1;
                    int[] scores = { loamyScore, qualityScore, dewScore, sweetScore, decayScore, frostScore, thickScore, shiningScore };
                    foreach(int score in scores)
                    {
                        largestScore = Math.Max(score, largestScore);
                    }
                    if (shiningScore == largestScore) result = ItemDict.SHINING_COMPOST;
                    else if (frostScore == largestScore) result = ItemDict.FROST_COMPOST;
                    else if (dewScore == largestScore) result = ItemDict.DEW_COMPOST;
                    else if (sweetScore == largestScore) result = ItemDict.SWEET_COMPOST;
                    else if (decayScore == largestScore) result = ItemDict.DECAY_COMPOST;
                    else if (thickScore == largestScore) result = ItemDict.THICK_COMPOST;
                    else if (qualityScore == largestScore) result = ItemDict.QUALITY_COMPOST;
                    else if (loamyScore == largestScore) result = ItemDict.LOAMY_COMPOST;
                }
                for (int i = 0; i < SIZE; i++)
                {
                    if(contents[i] != ItemDict.NONE)
                    {
                        contents[i] = result;
                    }
                }
                state = CompostBinState.FINISHED;
                sprite.SetLoop("placement");
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state == CompostBinState.IDLE)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(contents[0]),
                        new HoveringInterface.ItemStackElement(contents[1]),
                        new HoveringInterface.ItemStackElement(contents[2]),
                        new HoveringInterface.ItemStackElement(contents[3]),
                        new HoveringInterface.ItemStackElement(contents[4])),
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(contents[5]),
                        new HoveringInterface.ItemStackElement(contents[6]),
                        new HoveringInterface.ItemStackElement(contents[7]),
                        new HoveringInterface.ItemStackElement(contents[8]),
                        new HoveringInterface.ItemStackElement(contents[9])));
            }
            return null;
        }
    }
}
