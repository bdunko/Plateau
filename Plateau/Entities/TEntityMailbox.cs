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
    public class TEntityMailbox : TileEntity, IInteract, ITickDaily
    {
        public static int HEIGHT = 2;
        private static DialogueNode RECOLOR_DIALOGUE, EMPTY_DIALOGUE;
        private Texture2D exclaimation;
        private Texture2D boxGreyscale;
        private Texture2D boxRecolored;
        public Item boxDye;
        private Texture2D texture;
        private static List<Letter> LETTERS;
        private bool haveMail;
        private bool checkedMailToday;
        private static Item[] AIDEN_GIFTS = { ItemDict.SCRAP_IRON, ItemDict.STONE, ItemDict.GEARS, ItemDict.WEEDS, ItemDict.CLAY, ItemDict.PLANK};
        private static Item[] MEREDITH_GIFTS = { ItemDict.APPLE_MUFFIN, ItemDict.BUTTERED_ROLLS, ItemDict.ELDERBERRY_TART, ItemDict.SWEET_COCO_TREAT, ItemDict.LEMON_SHORTCAKE};
        private static Item[] OTIS_GIFTS = { ItemDict.SOUR_WINE, ItemDict.AUTUMNAL_WINE, ItemDict.REJUVENATION_TEA, ItemDict.DARK_TEA };
        private static Item[] CADE_GIFTS = { ItemDict.GOLD_BAR, ItemDict.MYTHRIL_BAR, ItemDict.ADAMANTITE_ORE, ItemDict.DIAMOND, ItemDict.SAPPHIRE, ItemDict.RUBY, ItemDict.EMERALD, ItemDict.GOLDEN_LEAF, ItemDict.FAIRY_DUST, ItemDict.ICE_NINE };
        private static Item[] HIMEKO_GIFTS = { ItemDict.IRON_ORE, ItemDict.HARDWOOD, ItemDict.PLANK, ItemDict.BOARD, ItemDict.BAMBOO, ItemDict.PAPER, ItemDict.BRICKS }; 
        private static Item[] CECILY_GIFTS = { ItemDict.WRAPPED_CABBAGE, ItemDict.COCONUT_BOAR, ItemDict.FARMERS_STEW, ItemDict.VEGGIE_SIDE_ROAST, ItemDict.BAKED_SNAPPER, ItemDict.HONEY_STIR_FRY }; 
        private static Item[] TROY_GIFTS = { ItemDict.BLUEGILL, ItemDict.CARP, ItemDict.CATFISH, ItemDict.LAKE_TROUT, ItemDict.SALMON, ItemDict.PIKE, ItemDict.TUNA, ItemDict.RED_SNAPPER, ItemDict.STRIPED_BASS, ItemDict.HERRING };
        private static Item[] RAUL_GIFTS = { ItemDict.COCONUT, ItemDict.GOLD_ORE, ItemDict.PEARL, ItemDict.OYSTER, ItemDict.HARDWOOD, ItemDict.IRON_ORE, ItemDict.CRAB, ItemDict.CLAM };
        private static Item[] ROCKWELL_GIFTS = { ItemDict.OLIVE, ItemDict.BANANA, ItemDict.CHERRY, ItemDict.LEMON, ItemDict.ORANGE, ItemDict.APPLE, ItemDict.HARDWOOD, ItemDict.ADAMANTITE_ORE, ItemDict.TRILOBITE };
        private static Item[] CAMUS_GIFTS = { ItemDict.IRON_BAR, ItemDict.MYTHRIL_BAR, ItemDict.GOLD_BAR, ItemDict.ADAMANTITE_BAR };
        private static Item[] THEO_GIFTS = { ItemDict.PAINTING_ACCEPTANCE, ItemDict.PAINTING_BALANCE, ItemDict.PAINTING_BLACK, ItemDict.PAINTING_COFFEE, ItemDict.PAINTING_DICHOTOMY, 
            ItemDict.PAINTING_FIREBALL, ItemDict.PAINTING_LION, ItemDict.PAINTING_MINTGREEN, ItemDict.PAINTING_GROVE, ItemDict.PAINTING_PUZZLE, ItemDict.PAINTING_TOXIN, ItemDict.PAINTING_CREAMPUFF, 
            ItemDict.PAINTING_SELF, ItemDict.PAINTING_RAIDER, ItemDict.PAINTING_VINEFLOWER, ItemDict.PAINTING_FJORD };
        private static Item[] PAIGE_GIFTS = { ItemDict.NAVY_DYE, ItemDict.BLUE_DYE, ItemDict.BLACK_DYE, ItemDict.RED_DYE, ItemDict.PINK_DYE, ItemDict.LIGHT_BROWN_DYE, ItemDict.DARK_BROWN_DYE, ItemDict.ORANGE_DYE,
            ItemDict.YELLOW_DYE, ItemDict.PURPLE_DYE, ItemDict.GREEN_DYE, ItemDict.OLIVE_DYE, ItemDict.WHITE_DYE, ItemDict.LIGHT_GREY_DYE, ItemDict.DARK_GREY_DYE, ItemDict.PAPER};
        private static Item[] CHARLOTTE_GIFTS = { ItemDict.AMETHYST, ItemDict.EGGPLANT, ItemDict.QUARTZ, ItemDict.TOPAZ, ItemDict.RUBY, ItemDict.WOOD, ItemDict.WEEDS };
        private static Item[] SKYE_GIFTS = { ItemDict.SWEATER, ItemDict.SCARF, ItemDict.STRIPED_SOCKS, ItemDict.SUPER_SHORTS, ItemDict.TIGHTIES, ItemDict.BUNNY_EARS, ItemDict.CAT_EARS, ItemDict.OVERALLS, ItemDict.STRIPED_SHIRT, ItemDict.PLAID_BUTTON, ItemDict.TURTLENECK};
        private static Item[] FINLEY_GIFTS = { ItemDict.SKY_ROSE, ItemDict.CHANTERELLE, ItemDict.SHIITAKE, ItemDict.CAVE_SOYBEAN, ItemDict.EMERALD_MOSS, ItemDict.CAVE_FUNGI, ItemDict.VANILLA_BEAN,
            ItemDict.CACAO_BEAN, ItemDict.MAIZE, ItemDict.PINEAPPLE, ItemDict.MOREL, ItemDict.CACTUS, ItemDict.BAMBOO};
        private static Item[] ELLE_GIFTS_SPRING = { ItemDict.SPINACH_SEEDS, ItemDict.POTATO_SEEDS, ItemDict.CARROT_SEEDS, ItemDict.QUALITY_COMPOST };
        private static Item[] ELLE_GIFTS_SUMMER = { ItemDict.ONION_SEEDS, ItemDict.CUCUMBER_SEEDS, ItemDict.TOMATO_SEEDS, ItemDict.QUALITY_COMPOST };
        private static Item[] ELLE_GIFTS_AUTUMN = { ItemDict.BROCCOLI_SEEDS, ItemDict.BEET_SEEDS, ItemDict.BELLPEPPER_SEEDS, ItemDict.QUALITY_COMPOST };
        private static Item[] ELLE_GIFTS_WINTER = { ItemDict.SWEET_COMPOST, ItemDict.DEW_COMPOST, ItemDict.QUALITY_COMPOST, ItemDict.THICK_COMPOST };
        private static Item[] PIPER_GIFTS_SPRING = { ItemDict.SPINACH, ItemDict.POTATO, ItemDict.CARROT, ItemDict.STRAWBERRY };
        private static Item[] PIPER_GIFTS_SUMMER = { ItemDict.ONION, ItemDict.CUCUMBER, ItemDict.TOMATO, ItemDict.WATERMELON_SLICE };
        private static Item[] PIPER_GIFTS_AUTUMN = { ItemDict.BELLPEPPER, ItemDict.BROCCOLI, ItemDict.BEET, ItemDict.PUMPKIN };
        private static Item[] PIPER_GIFTS_WINTER = { ItemDict.CHICORY_ROOT, ItemDict.WINTERGREEN };

        private static int[] GIFT_CHANCE_BY_HEARTS = { 0, 0, 0, 5, 7, 9, 10, 12, 14, 15, 18 };
        private static int WISH_FOR_LOVE_GIFT_BOOST = 10;

        private class Letter
        {
            public string flag;
            public DialogueNode dialogue;

            public Letter(string flag, DialogueNode dialogue)
            {
                this.flag = flag;
                this.dialogue = dialogue;
            }
        }

        public TEntityMailbox(Texture2D texture, Texture2D boxGreyscale, Texture2D exclaimation, Vector2 tilePosition, int tileWidth, int tileHeight) : base(tilePosition, tileWidth, tileHeight, DrawLayer.NORMAL)
        {
            this.texture = texture;
            this.boxGreyscale = boxGreyscale;
            this.exclaimation = exclaimation;
            this.haveMail = false;
            boxRecolored = null;
            boxDye = ItemDict.NONE;

            if (RECOLOR_DIALOGUE == null)
            {
                RECOLOR_DIALOGUE = new DialogueNode("Should I paint my mailbox?", DialogueNode.PORTRAIT_SYSTEM);
                RECOLOR_DIALOGUE.decisionUpText = "Yeah!";
                RECOLOR_DIALOGUE.decisionUpNode = new DialogueNode("Looking good!", DialogueNode.PORTRAIT_SYSTEM, (player, currentArea, world) =>
                {
                    TEntityMailbox mailbox = (TEntityMailbox)player.GetTargettedTileEntity();
                    mailbox.Dye(player.GetHeldItem().GetItem());
                    player.GetHeldItem().Subtract(1);

                });
                RECOLOR_DIALOGUE.decisionDownText = "Nevermind";
            }
            if(EMPTY_DIALOGUE == null)
            {
                EMPTY_DIALOGUE = new DialogueNode("No mail at the moment!", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (LETTERS == null)
            {
                LETTERS = new List<Letter>();
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_TEST, new DialogueNode("\"This is a letter in the mail. Read it!\"\n-Ben", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_LOST_ITEMS, new DialogueNode("\"<NAME>, Troy dropped off these items of yours at the Lost & Found last night.\nPlease be more careful with your belongings!\"\n-Otis", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    foreach(Item lostItem in GameState.LOST_ITEMS)
                    {
                        area.AddEntity(new EntityItem(lostItem, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                    GameState.LOST_ITEMS.Clear();
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_TUTORIAL_OTIS, new DialogueNode("\"Good morning <NAME>, are you starting to settle in to your new life? I'll deliver your mail to this mailbox every morning, so please check it often. Have a good day!\"\n-Otis", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_TUTORIAL_PIPER, new DialogueNode("\"<NAME>, read carefully, because this is your farming bootcamp.\nThis is how you grow crops:|1) Clear branches with your axe and rocks with your pickaxe.\n2) Hoe the ground to make farming patches.|3) Plant your seeds.\n4) Water the crops daily until they're ready for harvest.|I'll include some seeds to help you get started.\nIf you need a review, check your scrapbook.!\"\n-Piper", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    for (int i = 0; i < 10; i++)
                    {
                        area.AddEntity(new EntityItem(ItemDict.SPINACH_SEEDS, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_TUTORIAL_CAMUS, new DialogueNode("\"<NAME>, are you finding your tools to be a bit ineffective?\nCome talk to me about tool upgrades at the smithery, and we'll work something out.\"\n-Camus", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_TUTORIAL_CABLECAR, new DialogueNode("\"Notice: Nimbus Town Cablecar is back in action!\nSelf-service* open 24-hours a day, take a free 2-way trip to the base of Arc Mountain!\n*Nimbus Town assumses no liability for cablecar inflicted harm.\"", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    GameState.SetFlag(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL, 1);
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_TUTORIAL_TROY, new DialogueNode("\"<NAME>, stop by my workshop if you're getting tired of living in a tent. It's about time we get you a proper house built, right?\"", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_FESTIVAL_CYCLE, new DialogueNode("\"Attention all Nimbus Town residents:\nThe Cycle Festival will be held tommorrow at the town square, in commemoration of another passed cycle. We hope to see everyone there!\"\n-Otis", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_FESTIVAL_BEACH, new DialogueNode("\"Attention all Nimbus Town residents:\nBeach Day will be held tommorrow at the beach, to celebrate the start of summer! We hope to see everyone there!\"", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_FESTIVAL_HARVEST, new DialogueNode("\"Attention all Nimbus Town residents:\nThe Harvest Festival will be held tommorrow at Himeko's Inn! We hope to see everyone there!\"", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_FESTIVAL_CLEAR_NIGHT, new DialogueNode("\"Attention all Nimbus Town residents:\nClear Sky Night will occur tommorrow evening. Take this opportunity today to invite your special someone to an evening of stargazing!\"", DialogueNode.PORTRAIT_SYSTEM)));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_AIDEN, new DialogueNode("\"<NAME>, I found this junk in Piper's attic. It reminded me of you.\"\n-Aiden", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    Item gift = AIDEN_GIFTS[Util.RandInt(0, AIDEN_GIFTS.Length - 1)];
                    for(int i = 0; i < Util.RandInt(3, 5); i++)
                    {
                        area.AddEntity(new EntityItem(gift, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_MEREDITH, new DialogueNode("\"Dear <NAME>, I see you running around town all the time, so I know you're working hard. Here's a little something for you.\nBest wishes,\"\n-Meredith", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(MEREDITH_GIFTS[Util.RandInt(0, MEREDITH_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_OTIS, new DialogueNode("\"<NAME>, are you doing well? As mayor, it is my job to make sure that everyone in Nimbus Town prospers, so here is a small token of my appreciation for your efforts.\"\n-Otis", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(OTIS_GIFTS[Util.RandInt(0, OTIS_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_CADE, new DialogueNode("\"Hi <NAME>! Paige told me to write a letter to one of my friends, so I wrote this one for you! And here's a present too!\"\n-Cade", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(CADE_GIFTS[Util.RandInt(0, CADE_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_HIMEKO, new DialogueNode("\"Dear <NAME>, I was cleaning out the storeroom when I found something that you might find helpful. I hope this is useful to you.\"\n-Himeko", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    Item gift = HIMEKO_GIFTS[Util.RandInt(0, HIMEKO_GIFTS.Length - 1)];
                    for (int i = 0; i < Util.RandInt(8, 12); i++)
                    {
                        area.AddEntity(new EntityItem(gift, new Vector2(position.X, position.Y - 10)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_CECILY, new DialogueNode("\"<NAME>, I accidentally prepared a bit too much food yesterday, so here's some for you. These might be leftovers, but that doesn't mean they aren't delicious!\"\n-Cecily", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(CECILY_GIFTS[Util.RandInt(0, CECILY_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_TROY, new DialogueNode("\"Good morning <NAME>! The fish were biting up a storm yesterday, so I caught a bit more than I needed. Here's a little something for you. Don't worry, I refrigerated it overnight. Have a good one!\"\n-Troy", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    for (int i = 0; i < 3; i++)
                    {
                        area.AddEntity(new EntityItem(TROY_GIFTS[Util.RandInt(0, TROY_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_RAUL, new DialogueNode("\"<NAME>, guess what I found washed up on the beach yesterday? This!\"\n-Raul", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    Item gift = RAUL_GIFTS[Util.RandInt(0, RAUL_GIFTS.Length - 1)];
                    for (int i = 0; i < 3; i++)
                    {
                        area.AddEntity(new EntityItem(gift, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_ROCKWELL, new DialogueNode("\"<NAME>. Found something you might want while walking Ika. Here you go.\"\n-Rockwell", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(ROCKWELL_GIFTS[Util.RandInt(0, ROCKWELL_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_FINLEY, new DialogueNode("\"<NAME>, I found this priceless artifact during my recent expedition. Perhaps you can discern its purpose?\"\n-Finley", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    Item gift = FINLEY_GIFTS[Util.RandInt(0, FINLEY_GIFTS.Length - 1)];
                    for (int i = 0; i < Util.RandInt(3, 5); i++)
                    {
                        area.AddEntity(new EntityItem(gift, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_CAMUS, new DialogueNode("\"<NAME>, I ordered too many metal bars in my last shipment. I don't need this one, so please take it.\"\n-Camus", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(CAMUS_GIFTS[Util.RandInt(0, CAMUS_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_THEO, new DialogueNode("\"Dearest <NAME>, I have created this masterpiece of beauty most sublime. Feast your eyes upon it!\"\n-Theodore", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(THEO_GIFTS[Util.RandInt(0, THEO_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_ELLE, new DialogueNode("\"Valued customer <NAME>,\nNimbus General would like to offer you a free sample.\nNimbus General: For all your Nimbus needs!\n-Elle", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    Item gift;
                    switch(world.GetSeason())
                    {
                        case World.Season.SPRING:
                            gift = ELLE_GIFTS_SPRING[Util.RandInt(0, ELLE_GIFTS_SPRING.Length - 1)];
                            break;
                        case World.Season.SUMMER:
                            gift = ELLE_GIFTS_SUMMER[Util.RandInt(0, ELLE_GIFTS_SUMMER.Length - 1)];
                            break;
                        case World.Season.AUTUMN:
                            gift = ELLE_GIFTS_AUTUMN[Util.RandInt(0, ELLE_GIFTS_AUTUMN.Length - 1)];
                            break;
                        case World.Season.WINTER:
                        default:
                            gift = ELLE_GIFTS_WINTER[Util.RandInt(0, ELLE_GIFTS_WINTER.Length - 1)];
                            break;
                    }
                    for (int i = 0; i < Util.RandInt(12, 20); i++)
                    {
                        area.AddEntity(new EntityItem(gift, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_PIPER, new DialogueNode("\"<NAME>, here's some extra produce, fresh from Piper's Farm!\"\n-Piper", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    Item gift;
                    switch (world.GetSeason())
                    {
                        case World.Season.SPRING:
                            gift = PIPER_GIFTS_SPRING[Util.RandInt(0, PIPER_GIFTS_SPRING.Length - 1)];
                            break;
                        case World.Season.SUMMER:
                            gift = PIPER_GIFTS_SUMMER[Util.RandInt(0, PIPER_GIFTS_SUMMER.Length - 1)];
                            break;
                        case World.Season.AUTUMN:
                            gift = PIPER_GIFTS_AUTUMN[Util.RandInt(0, PIPER_GIFTS_AUTUMN.Length - 1)];
                            break;
                        case World.Season.WINTER:
                        default:
                            gift = PIPER_GIFTS_WINTER[Util.RandInt(0, PIPER_GIFTS_WINTER.Length - 1)];
                            break;
                    }
                    for (int i = 0; i < Util.RandInt(5, 8); i++)
                    {
                        area.AddEntity(new EntityItem(gift, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_PAIGE, new DialogueNode("\"Dear <NAME>, I hope this letter finds you healthy and hale. Here is a little something from your local librarian. Take care.\"\n-Piper", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    Item gift = PAIGE_GIFTS[Util.RandInt(0, PAIGE_GIFTS.Length - 1)];
                    for (int i = 0; i < 4; i++)
                    {
                        area.AddEntity(new EntityItem(gift, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                    }
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_CHARLOTTE, new DialogueNode("\"Dear <NAME>, I was thinking about you when I came across this item. I believe that it was a destined to be yours.\"\n-Charlotte", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(CHARLOTTE_GIFTS[Util.RandInt(0, CHARLOTTE_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
                LETTERS.Add(new Letter(GameState.FLAG_LETTER_GIFT_SKYE, new DialogueNode("\"<NAME>, I made this for you. I hope you like it.\"\n-Skye", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    area.AddEntity(new EntityItem(SKYE_GIFTS[Util.RandInt(0, SKYE_GIFTS.Length - 1)], new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y)));
                })));
            }
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sb.Draw(texture, position + new Vector2(0, 1), texture.Bounds, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            if (boxRecolored != null)
            {
                sb.Draw(boxRecolored, position + new Vector2(0, 1), texture.Bounds, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            }
            if(haveMail)
            {
                sb.Draw(exclaimation, position + new Vector2(texture.Width/2, -8), exclaimation.Bounds, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            }
        }

        public void NotifyRecolor()
        {
            if (boxDye == ItemDict.UN_DYE || boxDye == ItemDict.NONE)
            {
                boxDye = ItemDict.NONE;
                boxRecolored = null;
            }
            else
            {
                boxRecolored = Util.GenerateRecolor(boxGreyscale, ((DyeItem)boxDye).GetRecolorMap());
            }
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (player.GetHeldItem().GetItem().HasTag(Item.Tag.DYE))
            {
                return "Dye";
            }
            return "";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (player.GetHeldItem().GetItem().HasTag(Item.Tag.DYE))
            {
                player.SetCurrentDialogue(RECOLOR_DIALOGUE);
            }
        }

        public void Dye(Item dye)
        {
            boxDye = dye;
            NotifyRecolor();
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public override void Update(float deltaTime, Area area)
        {
            //if we have mail, set haveMail to true - this will cause exclaimation mark to be drawn
            if (!checkedMailToday)
            {
                foreach (Letter letter in LETTERS)
                {
                    if(GameState.GetFlagValue(letter.flag) != 0)
                    {
                        haveMail = true;
                        break;
                    }
                }
                checkedMailToday = true;
            }
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Read Mail";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (haveMail)
            {
                int numLetters = 0;
                haveMail = false;
                DialogueNode root = null;
                DialogueNode current = null;

                foreach (Letter letter in LETTERS)
                {
                    letter.dialogue.SetNext(null);
                    if (GameState.GetFlagValue(letter.flag) != 0)
                    {
                        GameState.SetFlag(letter.flag, 0);
                        if (root == null)
                        {
                            root = letter.dialogue;
                            current = root;
                        } else
                        {
                            current.SetNext(letter.dialogue);
                            current = letter.dialogue;
                        }
                        numLetters++;
                    }
                }

                DialogueNode header = new DialogueNode("The mailbox has " + numLetters + (numLetters == 1 ? "letter" : " letters")  + " today!", DialogueNode.PORTRAIT_SYSTEM);
                header.SetNext(root);
                player.SetCurrentDialogue(root);
            } else
            {
                player.SetCurrentDialogue(EMPTY_DIALOGUE);
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            GameState.SetFlag(GameState.FLAG_LETTER_GIFT_AIDEN, 1);
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            GameState.SetFlag(GameState.FLAG_LETTER_TEST, 1);
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            checkedMailToday = false;

            //try sending gifts
            foreach(EntityCharacter.CharacterEnum charEnum in Enum.GetValues(typeof(EntityCharacter.CharacterEnum)))
            {
                EntityCharacter character = world.GetCharacter(charEnum);
                if (character == null) //TODO: remove after adding all characters
                    continue;

                //odds of gift: scale by heart level, increased by a set amount if Wishboat is Love
                if (Util.RandInt(1, 100) <= (GIFT_CHANCE_BY_HEARTS[character.GetHeartLevel()] + (player.HasEffect(AppliedEffects.WISHBOAT_LOVE) ? WISH_FOR_LOVE_GIFT_BOOST : 0)))
                {
                    GameState.SetFlag(character.GetGiftFlag(), 1);
                }
            }
        }
    }
}