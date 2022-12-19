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
    public class PEntityKeg : PlacedEntity, IInteract, ITick, IHaveHoveringInterface
    {
        private enum KegState
        {
            IDLE, WORKING, WORKING_ALCOHOL, FINISHED
        }

        private Dictionary<Item, Item> BASE_MAP = new Dictionary<Item, Item>()
        {
            { ItemDict.APPLE, ItemDict.APPLE_PRESERVES},
            { ItemDict.SILVER_APPLE, ItemDict.APPLE_PRESERVES},
            { ItemDict.GOLDEN_APPLE, ItemDict.APPLE_PRESERVES},
            { ItemDict.PHANTOM_APPLE, ItemDict.APPLE_PRESERVES},
            { ItemDict.BANANA, ItemDict.BANANA_JAM},
            { ItemDict.SILVER_BANANA, ItemDict.BANANA_JAM},
            { ItemDict.GOLDEN_BANANA, ItemDict.BANANA_JAM},
            { ItemDict.PHANTOM_BANANA, ItemDict.BANANA_JAM},
            { ItemDict.ORANGE, ItemDict.MARMALADE},
            { ItemDict.SILVER_ORANGE, ItemDict.MARMALADE},
            { ItemDict.GOLDEN_ORANGE, ItemDict.MARMALADE},
            { ItemDict.PHANTOM_ORANGE, ItemDict.MARMALADE},
            { ItemDict.LEMON, ItemDict.LEMONADE},
            { ItemDict.SILVER_LEMON, ItemDict.LEMONADE},
            { ItemDict.GOLDEN_LEMON, ItemDict.LEMONADE},
            { ItemDict.PHANTOM_LEMON, ItemDict.LEMONADE},
            { ItemDict.CHERRY, ItemDict.CHERRY_JELLY},
            { ItemDict.SILVER_CHERRY, ItemDict.CHERRY_JELLY},
            { ItemDict.GOLDEN_CHERRY, ItemDict.CHERRY_JELLY},
            { ItemDict.PHANTOM_CHERRY, ItemDict.CHERRY_JELLY},
            { ItemDict.OLIVE, ItemDict.MARINATED_OLIVE},
            { ItemDict.SILVER_OLIVE, ItemDict.MARINATED_OLIVE},
            { ItemDict.GOLDEN_OLIVE, ItemDict.MARINATED_OLIVE},
            { ItemDict.PHANTOM_OLIVE, ItemDict.MARINATED_OLIVE},
            { ItemDict.CARROT, ItemDict.PICKLED_CARROT},
            { ItemDict.SILVER_CARROT, ItemDict.PICKLED_CARROT},
            { ItemDict.GOLDEN_CARROT, ItemDict.PICKLED_CARROT},
            { ItemDict.PHANTOM_CARROT, ItemDict.PICKLED_CARROT},
            { ItemDict.CUCUMBER, ItemDict.GOOD_OL_PICKLES},
            { ItemDict.SILVER_CUCUMBER, ItemDict.GOOD_OL_PICKLES},
            { ItemDict.GOLDEN_CUCUMBER, ItemDict.GOOD_OL_PICKLES},
            { ItemDict.PHANTOM_CUCUMBER, ItemDict.GOOD_OL_PICKLES},
            { ItemDict.BEET, ItemDict.BRINY_BEET},
            { ItemDict.SILVER_BEET, ItemDict.BRINY_BEET},
            { ItemDict.GOLDEN_BEET, ItemDict.BRINY_BEET},
            { ItemDict.PHANTOM_BEET, ItemDict.BRINY_BEET},
            { ItemDict.EGG, ItemDict.SOUSE_EGG},
            { ItemDict.GOLDEN_EGG, ItemDict.SOUSE_EGG},
            { ItemDict.ONION, ItemDict.PICKLED_ONION},
            { ItemDict.SILVER_ONION, ItemDict.PICKLED_ONION},
            { ItemDict.GOLDEN_ONION, ItemDict.PICKLED_ONION},
            { ItemDict.PHANTOM_ONION, ItemDict.PICKLED_ONION},
            { ItemDict.PERSIMMON, ItemDict.PERSIMMON_JAM},
            { ItemDict.BLACKBERRY, ItemDict.BLACKBERRY_PRESERVES},
            { ItemDict.ELDERBERRY, ItemDict.ELDERBERRY_JAM},
            { ItemDict.BLUEBERRY, ItemDict.BLUEBERRY_JELLY},
            { ItemDict.STRAWBERRY, ItemDict.STRAWBERRY_BLAST_JAM},
            { ItemDict.SILVER_STRAWBERRY, ItemDict.STRAWBERRY_BLAST_JAM},
            { ItemDict.GOLDEN_STRAWBERRY, ItemDict.STRAWBERRY_BLAST_JAM},
            { ItemDict.PHANTOM_STRAWBERRY, ItemDict.STRAWBERRY_BLAST_JAM},
            { ItemDict.RASPBERRY, ItemDict.RASPBERRY_JELLY},
            { ItemDict.TOMATO, ItemDict.TOMATO_SALSA},
            { ItemDict.SILVER_TOMATO, ItemDict.TOMATO_SALSA},
            { ItemDict.GOLDEN_TOMATO, ItemDict.TOMATO_SALSA},
            { ItemDict.PHANTOM_TOMATO, ItemDict.TOMATO_SALSA},
            { ItemDict.PUMPKIN, ItemDict.AUTUMN_SALSA},
            { ItemDict.SILVER_PUMPKIN, ItemDict.AUTUMN_SALSA},
            { ItemDict.GOLDEN_PUMPKIN, ItemDict.AUTUMN_SALSA},
            { ItemDict.PHANTOM_PUMPKIN, ItemDict.AUTUMN_SALSA},
            { ItemDict.COCONUT, ItemDict.COCONUT_MILK},
            { ItemDict.SILVER_COCONUT, ItemDict.COCONUT_MILK},
            { ItemDict.GOLDEN_COCONUT, ItemDict.COCONUT_MILK},
            { ItemDict.PHANTOM_COCONUT, ItemDict.COCONUT_MILK},
            { ItemDict.WATERMELON_SLICE, ItemDict.WATERMELON_JELLY},
            { ItemDict.SILVER_WATERMELON_SLICE, ItemDict.WATERMELON_JELLY},
            { ItemDict.GOLDEN_WATERMELON_SLICE, ItemDict.WATERMELON_JELLY},
            { ItemDict.PHANTOM_WATERMELON_SLICE, ItemDict.WATERMELON_JELLY},
            { ItemDict.PINEAPPLE, ItemDict.PINEAPPLE_SALSA},
        };

        private Dictionary<Item, Item> ALCOHOL_MAP = new Dictionary<Item, Item>()
        {
            { ItemDict.APPLE_PRESERVES, ItemDict.APPLE_CIDER},
            { ItemDict.BANANA_JAM, ItemDict.BEERNANA},
            { ItemDict.MARMALADE, ItemDict.ALCORANGE},
            { ItemDict.LEMONADE, ItemDict.SOUR_WINE},
            { ItemDict.CHERRY_JELLY, ItemDict.CHERWINE},
            { ItemDict.BLACKBERRY_PRESERVES, ItemDict.BLACKBERRY_DIGESTIF},
            { ItemDict.ELDERBERRY_JAM, ItemDict.ELDERBERRY_APERITIF},
            { ItemDict.BLUEBERRY_JELLY, ItemDict.BLUEBERRY_CORDIAL},
            { ItemDict.STRAWBERRY_BLAST_JAM, ItemDict.STRAWBERRY_SPIRIT},
            { ItemDict.PERSIMMON_JAM, ItemDict.AUTUMNAL_WINE},
            { ItemDict.RASPBERRY_JELLY, ItemDict.RASPBERRY_LIQUEUR},
            { ItemDict.TOMATO_SALSA, ItemDict.BLOODY_MARIE},
            { ItemDict.AUTUMN_SALSA, ItemDict.PUMPKIN_CIDER},
            { ItemDict.COCONUT_MILK, ItemDict.COCONUT_RUM},
            { ItemDict.WATERMELON_JELLY, ItemDict.WATERMELON_WINE},
            { ItemDict.PINEAPPLE_SALSA, ItemDict.TROPICAL_RUM}
        };

        private static int PROCESSING_TIME_BASE = 1 * 23 * 60;
        private static int PROCESSING_TIME_ALCOHOL = 3 * 23 * 60;
        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private KegState state;
        private ResultHoverBox resultHoverBox;

        public PEntityKeg(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("working", 4, 7, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = KegState.IDLE;
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
            string stateStr = saveState.TryGetData("state", KegState.IDLE.ToString());
            if (stateStr.Equals(KegState.IDLE.ToString()))
            {
                state = KegState.IDLE;
            }
            else if (stateStr.Equals(KegState.WORKING.ToString()))
            {
                state = KegState.WORKING;
            }
            else if (stateStr.Equals(KegState.WORKING_ALCOHOL.ToString()))
            {
                state = KegState.WORKING_ALCOHOL;
            }
            else if (stateStr.Equals(KegState.FINISHED.ToString()))
            {
                state = KegState.FINISHED;
            } 
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
                if (state == KegState.WORKING || state == KegState.WORKING_ALCOHOL)
                {
                    sprite.SetLoopIfNot("working");
                } 
                else
                {
                    sprite.SetLoopIfNot("idle");
                }
            }
            if (state == KegState.FINISHED || state == KegState.WORKING_ALCOHOL)
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
            if (state == KegState.IDLE)
            {
                return "Add Item";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == KegState.FINISHED || state == KegState.WORKING_ALCOHOL)
            {
                return "Empty";
            }
            return "";
        }


        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == KegState.FINISHED || state == KegState.WORKING_ALCOHOL)
            {
                for(int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = KegState.IDLE;
                timeRemaining = 0;
                sprite.SetLoop("placement");
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (state == KegState.IDLE)
            {
                Item addedItem = player.GetHeldItem().GetItem();

                if (BASE_MAP.ContainsKey(addedItem))
                {
                    heldItem = new ItemStack(addedItem, 1);
                    player.GetHeldItem().Subtract(1);
                    sprite.SetLoop("placement");
                    state = KegState.WORKING;
                    timeRemaining = PROCESSING_TIME_BASE;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I can't add this to the Keg.", Color.Red));
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
            if (timeRemaining <= 0 && state == KegState.WORKING)
            {
                int quantity = 1;
                if(heldItem.GetItem().HasTag(Item.Tag.SILVER_CROP))
                {
                    quantity = 2;
                } else if (heldItem.GetItem().HasTag(Item.Tag.GOLDEN_CROP))
                {
                    quantity = 3;
                } else if (heldItem.GetItem().HasTag(Item.Tag.PHANTOM_CROP))
                {
                    quantity = 5;
                } else if (heldItem.GetItem() == ItemDict.GOLDEN_EGG)
                {
                    quantity = 3;
                }

                heldItem = new ItemStack(BASE_MAP[heldItem.GetItem()], quantity);
                sprite.SetLoop("placement");

                if (ALCOHOL_MAP.ContainsKey(heldItem.GetItem()))
                {
                    state = KegState.WORKING_ALCOHOL;
                    timeRemaining = PROCESSING_TIME_ALCOHOL;
                }
                else
                {
                    state = KegState.FINISHED;
                }
            } else if (timeRemaining <= 0 && state == KegState.WORKING_ALCOHOL)
            {
                heldItem = new ItemStack(ALCOHOL_MAP[heldItem.GetItem()], heldItem.GetQuantity());
                sprite.SetLoop("placement");
                state = KegState.FINISHED;
            }
        }

        public HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (state != KegState.FINISHED && state != KegState.WORKING_ALCOHOL)
            {
                return new HoveringInterface(
                     new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(heldItem)));
            }
            return null;
        }
    }
}