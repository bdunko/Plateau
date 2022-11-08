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
    public class PEntitySeedMaker : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum SeedMakerState
        {
            IDLE, WORKING, FINISHED
        }

        private SeedMakerState state;
        private static int PROCESSING_TIME = 1 * 60;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack heldItem;

        public PEntitySeedMaker(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.state = SeedMakerState.IDLE;
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            sprite.AddLoop("anim", 4, 8, true);
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
            string stateStr = saveState.TryGetData("state", SeedMakerState.IDLE.ToString());
            if (stateStr.Equals(SeedMakerState.IDLE.ToString()))
            {
                state = SeedMakerState.IDLE;
            }
            else if (stateStr.Equals(SeedMakerState.WORKING.ToString()))
            {
                state = SeedMakerState.WORKING;
            }
            else if (stateStr.Equals(SeedMakerState.FINISHED.ToString()))
            {
                state = SeedMakerState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if(state == SeedMakerState.WORKING)
            {
                sprite.SetLoopIfNot("anim");
            }

            if (state == SeedMakerState.FINISHED)
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
                for(int i = 0; i < heldItem.GetQuantity(); i++)
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
            if(state == SeedMakerState.IDLE)
            {
                return "Add";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if(state == SeedMakerState.FINISHED)
            {
                return "Take";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if(state == SeedMakerState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = SeedMakerState.IDLE;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if(state == SeedMakerState.IDLE)
            {
                ItemStack input = player.GetHeldItem();
                if (input.GetItem().HasTag(Item.Tag.CROP) || input.GetItem().HasTag(Item.Tag.SILVER_CROP) ||
                    input.GetItem().HasTag(Item.Tag.GOLDEN_CROP) || input.GetItem().HasTag(Item.Tag.PHANTOM_CROP) ||
                    input.GetItem().HasTag(Item.Tag.MUSHROOM) || input.GetItem() == ItemDict.BIRDS_NEST ||
                    input.GetItem() == ItemDict.BLUEBERRY || input.GetItem() == ItemDict.RASPBERRY ||
                    input.GetItem() == ItemDict.BLACKBERRY || input.GetItem() == ItemDict.ELDERBERRY) {
                    heldItem = new ItemStack(input.GetItem(), 1);
                    input.Subtract(1);
                    state = SeedMakerState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                    sprite.SetLoop("placement");
                } else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't make seeds out of this.", Color.Red));
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
            if (state == SeedMakerState.WORKING)
            {
                timeRemaining -= minutesTicked;
                if(timeRemaining <= 0)
                {
                    if(heldItem.GetItem() == ItemDict.BIRDS_NEST)
                    {
                        Item seed = ItemDict.NONE;
                        bool shining = Util.RandInt(1, 10) == 1;
                        switch(Util.RandInt(1, 17))
                        {
                            case 1:
                                seed = ItemDict.BEET_SEEDS;
                                break;
                            case 2:
                                seed = ItemDict.BELLPEPPER_SEEDS;
                                break;
                            case 3:
                                seed = ItemDict.BROCCOLI_SEEDS;
                                break;
                            case 4:
                                seed = ItemDict.CABBAGE_SEEDS;
                                break;
                            case 5:
                                seed = ItemDict.CACTUS_SEEDS;
                                break;
                            case 6:
                                seed = ItemDict.CARROT_SEEDS;
                                break;
                            case 7:
                                seed = ItemDict.COTTON_SEEDS;
                                break;
                            case 8:
                                seed = ItemDict.CUCUMBER_SEEDS;
                                break;
                            case 9:
                                seed = ItemDict.EGGPLANT_SEEDS;
                                break;
                            case 10:
                                seed = ItemDict.FLAX_SEEDS;
                                break;
                            case 11:
                                seed = ItemDict.ONION_SEEDS;
                                break;
                            case 12:
                                seed = ItemDict.POTATO_SEEDS;
                                break;
                            case 13:
                                seed = ItemDict.PUMPKIN_SEEDS;
                                break;
                            case 14:
                                seed = ItemDict.SPINACH_SEEDS;
                                break;
                            case 15:
                                seed = ItemDict.STRAWBERRY_SEEDS;
                                break;
                            case 16:
                                seed = ItemDict.TOMATO_SEEDS;
                                break;
                            case 17:
                                seed = ItemDict.WATERMELON_SEEDS;
                                break;
                        }
                        if(shining)
                        {
                            seed = ItemDict.GetSeedShiningForm(seed);
                        }
                        heldItem = new ItemStack(seed, Util.RandInt(1, 3));
                    } else if (heldItem.GetItem() == ItemDict.BLUEBERRY || heldItem.GetItem() == ItemDict.RASPBERRY || heldItem.GetItem() == ItemDict.BLACKBERRY || heldItem.GetItem() == ItemDict.ELDERBERRY)
                    {
                        heldItem = new ItemStack(ItemDict.BERRY_BUSH_PLANTER, 1);
                    }
                    else if(heldItem.GetItem().HasTag(Item.Tag.MUSHROOM))
                    {
                        int quantitySpores = Util.RandInt(2, 4);
                        if(heldItem.GetItem() == ItemDict.TRUFFLE || heldItem.GetItem() == ItemDict.SHIITAKE)
                        {
                            quantitySpores += Util.RandInt(1, 3);
                        }
                        heldItem = new ItemStack(ItemDict.SPORES, quantitySpores);
                    } else
                    {
                        //regular: 2-3 seeds always
                        //silver: 0.33% 1 shining or 2-4 seeds
                        //golden: 1-2 shining
                        //phantom: 1-4 shining
                        bool shining = false;
                        int quantity = Util.RandInt(2, 3);
                        Item crop = heldItem.GetItem();
                        if(crop.HasTag(Item.Tag.SILVER_CROP))
                        {
                            crop = ItemDict.GetCropBaseForm(crop);
                            shining = Util.RandInt(0, 2) == 0;
                            quantity = Util.RandInt(2, 4);
                            if (shining)
                            {
                                quantity = 1;
                            }
                        } else if (crop.HasTag(Item.Tag.GOLDEN_CROP))
                        {
                            crop = ItemDict.GetCropBaseForm(crop);
                            shining = true;
                            quantity = Util.RandInt(1, 2);
                        } else if (crop.HasTag(Item.Tag.PHANTOM_CROP))
                        {
                            crop = ItemDict.GetCropBaseForm(crop);
                            shining = true;
                            quantity = Util.RandInt(1, 3);
                        }

                        Item seedForm = ItemDict.GetBaseCropSeedForm(crop);
                        if(shining)
                        {
                            seedForm = ItemDict.GetSeedShiningForm(seedForm);
                        }
                        heldItem = new ItemStack(seedForm, quantity);
                    }
                    sprite.SetLoop("placement");
                    state = SeedMakerState.FINISHED;
                }
            }
        }

        public HoveringInterface GetHoveringInterface()
        {
            if(state == SeedMakerState.IDLE)
            {
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return new HoveringInterface();
        }
    }
}
