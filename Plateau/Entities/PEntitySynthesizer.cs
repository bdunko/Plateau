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
    public class PEntitySynthesizer : PlacedEntity, IInteract, ITick
    {
        private enum ExtractorState
        {
            FINISHED, WORKING
        }

        private static Dictionary<Area.AreaEnum, List<Item>> SYNTHESIZED_ITEMS = new Dictionary<Area.AreaEnum, List<Item>>()
        {
            {
                Area.AreaEnum.INTERIOR,
                new List<Item>()
                {
                    ItemDict.BAMBOO_POT, ItemDict.BELL, ItemDict.BLACKBOARD, ItemDict.BOOMBOX, ItemDict.BOX, ItemDict.BUOY, ItemDict.CANVAS, ItemDict.CART, ItemDict.CLAY_BALL, ItemDict.CLAY_BOWL, ItemDict.CLAY_DOLL, ItemDict.CLAY_SLATE, ItemDict.CLOTHESLINE, ItemDict.CRATE, ItemDict.CUBE_STATUE, ItemDict.CYMBAL, ItemDict.DECORATIVE_BOULDER, ItemDict.DECORATIVE_LOG,
                    ItemDict.DRUM, ItemDict.FIRE_HYDRANT, ItemDict.FLAGPOLE, ItemDict.FROST_SCULPTURE, ItemDict.GARDEN_ARCH, ItemDict.GRANDFATHER_CLOCK, ItemDict.GUITAR_PLACEABLE, ItemDict.GYM_BENCH, ItemDict.HAMMOCK, ItemDict.HAYBALE, ItemDict.ICE_BLOCK, ItemDict.IGLOO, ItemDict.LATTICE, ItemDict.LIFEBUOY_SIGN, ItemDict.LIGHTNING_ROD, ItemDict.MAILBOX, ItemDict.MARKET_STALL, ItemDict.MILK_JUG,
                    ItemDict.MINECART, ItemDict.PET_BOWL, ItemDict.PIANO, ItemDict.ORNATE_MIRROR, ItemDict.PILE_OF_BRICKS, ItemDict.POSTBOX, ItemDict.POTTERY_JAR, ItemDict.POTTERY_PLATE, ItemDict.POTTERY_MUG, ItemDict.POTTERY_VASE, ItemDict.PYRAMID_STATUE, ItemDict.RECYCLING_BIN, ItemDict.SANDBOX, ItemDict.SANDCASTLE, ItemDict.SCARECROW, ItemDict.SEESAW, ItemDict.SIGNPOST,
                    ItemDict.SLIDE, ItemDict.SNOWMAN, ItemDict.SOFA, ItemDict.SOLAR_PANEL, ItemDict.SPHERE_STATUE, ItemDict.STATUE, ItemDict.STONE_COLUMN, ItemDict.SURFBOARD, ItemDict.SWINGS, ItemDict.TARGET, ItemDict.TELEVISION, ItemDict.TOOLBOX, ItemDict.TRAFFIC_CONE, ItemDict.TRAFFIC_LIGHT, ItemDict.TRASHCAN, ItemDict.UMBRELLA, ItemDict.UMBRELLA_TABLE, ItemDict.WAGON,
                    ItemDict.WATER_PUMP, ItemDict.WATERTOWER, ItemDict.WELL, ItemDict.WHEELBARROW, ItemDict.WHITEBOARD, ItemDict.WOODEN_BENCH, ItemDict.WOODEN_CHAIR, ItemDict.WOODEN_COLUMN, ItemDict.WOODEN_LONGTABLE, ItemDict.WOODEN_POST, ItemDict.WOODEN_ROUNDTABLE, ItemDict.WOODEN_SQUARETABLE, ItemDict.WOODEN_STOOL, ItemDict.DRUMSET, ItemDict.HARP,
                    ItemDict.XYLOPHONE, ItemDict.WALL_MIRROR, ItemDict.ANATOMICAL_POSTER, ItemDict.BANNER, ItemDict.CLOCK, ItemDict.FULL_THROTTLE_GRAFFITI, ItemDict.HEARTBREAK_GRAFFITI, ItemDict.HELIX_POSTER, ItemDict.HEROINE_GRAFFITI, ItemDict.HORIZONTAL_MIRROR, ItemDict.LEFTWARD_GRAFFITI, ItemDict.NOIZEBOYZ_GRAFFITI,
                    ItemDict.RAINBOW_GRAFFITI, ItemDict.RETRO_GRAFFITI, ItemDict.RIGHT_ARROW_GRAFFITI, ItemDict.SMILE_GRAFFITI, ItemDict.SOURCE_UNKNOWN_GRAFFITI, ItemDict.TOOLRACK, ItemDict.TRIPLE_MIRRORS, ItemDict.HERALDIC_SHIELD, ItemDict.DECORATIVE_SWORD, ItemDict.SUIT_OF_ARMOR, ItemDict.HORSESHOE,
                    ItemDict.TORCH, ItemDict.BRAZIER, ItemDict.CAMPFIRE, ItemDict.FIREPIT, ItemDict.FIREPLACE, ItemDict.LAMP, ItemDict.LANTERN, ItemDict.STREETLAMP, ItemDict.STREETLIGHT
                }
            },
            {
                Area.AreaEnum.FARM,
                new List<Item>()
                {
                    ItemDict.SHINING_BEET_SEEDS, ItemDict.SHINING_BELLPEPPER_SEEDS, ItemDict.SHINING_BROCCOLI_SEEDS, ItemDict.SHINING_CABBAGE_SEEDS,
                    ItemDict.SHINING_CACTUS_SEEDS, ItemDict.SHINING_CARROT_SEEDS, ItemDict.SHINING_COTTON_SEEDS, ItemDict.SHINING_CUCUMBER_SEEDS, ItemDict.SHINING_EGGPLANT_SEEDS, ItemDict.SHINING_FLAX_SEEDS, ItemDict.SHINING_ONION_SEEDS,
                    ItemDict.SHINING_POTATO_SEEDS, ItemDict.SHINING_PUMPKIN_SEEDS, ItemDict.SHINING_SPINACH_SEEDS, ItemDict.SHINING_STRAWBERRY_SEEDS, ItemDict.SHINING_TOMATO_SEEDS, ItemDict.SHINING_WATERMELON_SEEDS,
                    ItemDict.LOAMY_COMPOST, ItemDict.QUALITY_COMPOST, ItemDict.DEW_COMPOST, ItemDict.SWEET_COMPOST, ItemDict.DECAY_COMPOST, ItemDict.FROST_COMPOST, ItemDict.THICK_COMPOST, ItemDict.SHINING_COMPOST,
                    ItemDict.GOLDEN_EGG, ItemDict.MILK, ItemDict.GOLDEN_WOOL, ItemDict.BERRY_BUSH_PLANTER,
                    ItemDict.APPLE_SAPLING, ItemDict.BANANA_PALM_SAPLING, ItemDict.CHERRY_SAPLING, ItemDict.COCONUT_PALM_SAPLING, ItemDict.LEMON_SAPLING, ItemDict.OLIVE_SAPLING, ItemDict.ORANGE_SAPLING,
                }
            },
            {
                Area.AreaEnum.BEACH,
                new List<Item>()
                {
                    ItemDict.BLACKENED_OCTOPUS, ItemDict.BLUEGILL, ItemDict.BOXER_LOBSTER, ItemDict.CARP, ItemDict.CATFISH, ItemDict.CAVEFISH, ItemDict.CAVERN_TETRA, ItemDict.CLOUD_FLOUNDER, ItemDict.CRAB, ItemDict.DARK_ANGLER, ItemDict.EMPEROR_SALMON, ItemDict.GREAT_WHITE_SHARK,
                    ItemDict.HERRING, ItemDict.INFERNAL_SHARK, ItemDict.INKY_SQUID, ItemDict.JUNGLE_PIRANHA, ItemDict.LAKE_TROUT, ItemDict.LUNAR_WHALE, ItemDict.MACKEREL, ItemDict.MOLTEN_SQUID, ItemDict.ONYX_EEL, ItemDict.PIKE, ItemDict.PUFFERFISH, ItemDict.QUEEN_AROWANA, ItemDict.RED_SNAPPER, ItemDict.SALMON,
                    ItemDict.SARDINE, ItemDict.SEA_TURTLE, ItemDict.SHRIMP, ItemDict.SKY_PIKE, ItemDict.STORMBRINGER_KOI, ItemDict.STRIPED_BASS, ItemDict.SUNFISH, ItemDict.SWORDFISH, ItemDict.TUNA, ItemDict.WHITE_BLOWFISH,
                    ItemDict.OYSTER, ItemDict.CLAM, ItemDict.CRIMSON_CORAL, ItemDict.SEA_URCHIN, ItemDict.PEARL, ItemDict.FLAWLESS_CONCH
                }
            },
            {
                Area.AreaEnum.TOWN,
                new List<Item>()
                {
                    ItemDict.FRIED_EGG, ItemDict.EGG_SCRAMBLE, ItemDict.BREAKFAST_POTATOES, ItemDict.SPICY_BACON, ItemDict.BLUEBERRY_PANCAKES, ItemDict.APPLE_MUFFIN, ItemDict.ROASTED_PUMPKIN, ItemDict.VEGGIE_SIDE_ROAST, ItemDict.WRAPPED_CABBAGE,
                    ItemDict.SEAFOOD_PAELLA, ItemDict.SEASONAL_PIPERADE, ItemDict.SUPER_JUICE, ItemDict.POTATO_AND_BEET_FRIES, ItemDict.PICKLED_BEET_EGG, ItemDict.COCONUT_BOAR, ItemDict.SPRING_PIZZA, ItemDict.STRAWBERRY_SALAD, ItemDict.BOAR_STEW, ItemDict.BAKED_POTATO,
                    ItemDict.FRESH_SALAD, ItemDict.STEWED_VEGGIES, ItemDict.MEATY_PIZZA, ItemDict.WATERMELON_ICE, ItemDict.SUSHI_ROLL, ItemDict.SEARED_TUNA, ItemDict.DELUXE_SUSHI, ItemDict.MUSHROOM_STIR_FRY, ItemDict.MOUNTAIN_TERIYAKI, ItemDict.LETHAL_SASHIMI,
                    ItemDict.VANILLA_ICE_CREAM, ItemDict.BERRY_MILKSHAKE, ItemDict.MINT_CHOCO_BAR, ItemDict.MINTY_MELT, ItemDict.BANANA_SUNDAE, ItemDict.FRENCH_ONION_SOUP, ItemDict.TOMATO_SOUP, ItemDict.FARMERS_STEW, ItemDict.CREAM_OF_MUSHROOM, ItemDict.CREAMY_SPINACH,
                    ItemDict.SHRIMP_GUMBO, ItemDict.FRIED_CATFISH, ItemDict.GRILLED_SALMON, ItemDict.BAKED_SNAPPER, ItemDict.SWORDFISH_POT, ItemDict.HONEY_STIR_FRY, ItemDict.BUTTERED_ROLLS, ItemDict.SAVORY_ROAST, ItemDict.STUFFED_FLOUNDER, ItemDict.CLAM_LINGUINI, ItemDict.LEMON_SHORTCAKE,
                    ItemDict.CHERRY_CHEESECAKE, ItemDict.MOUNTAIN_BREAD, ItemDict.CHICKWEED_BLEND, ItemDict.CRISPY_GRASSHOPPER, ItemDict.REJUVENATION_TEA, ItemDict.HOMESTYLE_JELLY, ItemDict.SEAFOOD_BASKET, ItemDict.WILD_POPCORN, ItemDict.ELDERBERRY_TART,
                    ItemDict.DARK_TEA, ItemDict.LICHEN_JUICE, ItemDict.AUTUMN_MASH, ItemDict.CAMPFIRE_COFFEE, ItemDict.BLIND_DINNER, ItemDict.FRIED_FISH, ItemDict.SARDINE_SNACK, ItemDict.DWARVEN_STEW, ItemDict.SWEET_COCO_TREAT, ItemDict.FRIED_OYSTERS, ItemDict.SURVIVORS_SURPRISE,
                    ItemDict.EEL_ROLL, ItemDict.RAW_CALAMARI, ItemDict.STORMFISH, ItemDict.CREAMY_SQUID, ItemDict.ESCARGOT, ItemDict.LUMINOUS_STEW, ItemDict.COLESLAW, ItemDict.RATATOUILLE, ItemDict.EGGPLANT_PARMESAN,
                    ItemDict.SEAWEED_SNACK, ItemDict.KLIPPFISK, ItemDict.BOAR_JERKY, ItemDict.VEGGIE_CHIPS, ItemDict.POTATO_CRISPS, ItemDict.DRIED_APPLE, ItemDict.DRIED_STRAWBERRY, ItemDict.WATERLESS_MELON, ItemDict.DRIED_CITRUS, ItemDict.DRIED_OLIVES, ItemDict.COCONUT_CHIPS, ItemDict.CHERRY_RAISINS, ItemDict.BANANA_CHIPS,
                    ItemDict.APPLE_PRESERVES, ItemDict.APPLE_CIDER, ItemDict.BANANA_JAM,ItemDict. BEERNANA, ItemDict.MARMALADE, ItemDict.ALCORANGE, ItemDict.LEMONADE, ItemDict.SOUR_WINE, ItemDict.CHERRY_JELLY, ItemDict.CHERWINE, ItemDict.MARINATED_OLIVE,
                    ItemDict.PICKLED_CARROT, ItemDict.GOOD_OL_PICKLES, ItemDict.BRINY_BEET, ItemDict.SOUSE_EGG, ItemDict.PICKLED_ONION, ItemDict.PERSIMMON_JAM, ItemDict.AUTUMNAL_WINE, ItemDict.BLACKBERRY_PRESERVES, ItemDict.BLACKBERRY_DIGESTIF, ItemDict.BLUEBERRY_JELLY, ItemDict.BLUEBERRY_CORDIAL,
                    ItemDict.STRAWBERRY_BLAST_JAM, ItemDict.STRAWBERRY_SPIRIT, ItemDict.ELDERBERRY_APERITIF, ItemDict.ELDERBERRY_JAM, ItemDict.RASPBERRY_JELLY, ItemDict.RASPBERRY_LIQUEUR, ItemDict.TOMATO_SALSA, ItemDict.BLOODY_MARIE, ItemDict.AUTUMN_SALSA, ItemDict.PUMPKIN_CIDER,
                    ItemDict.COCONUT_RUM, ItemDict.WATERMELON_JELLY, ItemDict.WATERMELON_WINE, ItemDict.PINEAPPLE_SALSA, ItemDict.TROPICAL_RUM
                }
            },
            {
                Area.AreaEnum.S0,
                new List<Item>()
                {
                    ItemDict.BANDED_DRAGONFLY, ItemDict.BROWN_CICADA, ItemDict.CAVEWORM, ItemDict.EARTHWORM, ItemDict.EMPRESS_BUTTERFLY, ItemDict.FIREFLY, ItemDict.HONEY_BEE, ItemDict.JEWEL_SPIDER,
                    ItemDict.LANTERN_MOTH, ItemDict.PINK_LADYBUG, ItemDict.RICE_GRASSHOPPER, ItemDict.SNAIL, ItemDict.SOLDIER_ANT, ItemDict.STAG_BEETLE, ItemDict.STINGER_HORNET, ItemDict.YELLOW_BUTTERFLY,
                    ItemDict.BLUE_DYE, ItemDict.NAVY_DYE, ItemDict.BLACK_DYE, ItemDict.RED_DYE, ItemDict.PINK_DYE, ItemDict.LIGHT_BROWN_DYE, ItemDict.DARK_BROWN_DYE, ItemDict.ORANGE_DYE, ItemDict.YELLOW_DYE, ItemDict.PURPLE_DYE,
                    ItemDict.GREEN_DYE, ItemDict.OLIVE_DYE, ItemDict.WHITE_DYE, ItemDict.LIGHT_GREY_DYE, ItemDict.DARK_GREY_DYE, ItemDict.CYAN_DYE, ItemDict.CRIMSON_DYE, ItemDict.WHEAT_DYE, ItemDict.MINT_DYE, ItemDict.UN_DYE,
                    ItemDict.AUTUMNS_KISS, ItemDict.BIZARRE_PERFUME, ItemDict.BLISSFUL_SKY, ItemDict.FLORAL_PERFUME, ItemDict.OCEAN_GUST, ItemDict.RED_ANGEL, ItemDict.SUMMERS_GIFT, ItemDict.SWEET_BREEZE,
                    ItemDict.WARM_MEMORIES, ItemDict.VANILLA_EXTRACT, ItemDict.MINT_EXTRACT,
                    ItemDict.BIRDS_NEST, ItemDict.BLACK_FEATHER, ItemDict.BLUE_FEATHER, ItemDict.PRISMATIC_FEATHER, ItemDict.WHITE_FEATHER, ItemDict.RED_FEATHER,
                    ItemDict.GOLDEN_LEAF
    }
            },
            {
                Area.AreaEnum.S1,
                new List<Item>()
                {
                    ItemDict.BACKPACK, ItemDict.RUCKSACK, ItemDict.CAPE, ItemDict.WOLF_TAIL, ItemDict.FOX_TAIL, ItemDict.CAT_TAIL, ItemDict.CLOCKWORK, ItemDict.ROBO_ARMS, ItemDict.GUITAR,
                    ItemDict.SHORT_SOCKS, ItemDict.LONG_SOCKS, ItemDict.STRIPED_SOCKS, ItemDict.FESTIVE_SOCKS, ItemDict.MISMATTCHED,
                    ItemDict.EARRING_STUD, ItemDict.DANGLE_EARRING, ItemDict.PIERCING,
                    ItemDict.GLASSES, ItemDict.BLINDFOLD, ItemDict.EYEPATCH, ItemDict.GOGGLES, ItemDict.PROTECTIVE_VISOR, ItemDict.QUERADE_MASK, ItemDict.SNORKEL, ItemDict.SUNGLASSES,
                    ItemDict.WOOL_MITTENS, ItemDict.WORK_GLOVES, ItemDict.BOXING_MITTS,
                    ItemDict.BASEBALL_CAP, ItemDict.TEN_GALLON, ItemDict.BANDANA, ItemDict.BOWLER, ItemDict.BUNNY_EARS, ItemDict.BUTTERFLY_CLIP, ItemDict.CAMEL_HAT, ItemDict.CAT_EARS, ItemDict.CHEFS_HAT, ItemDict.CONICAL_FARMER, ItemDict.DINO_MASK, ItemDict.DOG_MASK, ItemDict.FACEMASK, ItemDict.FLAT_CAP,
                    ItemDict.HEADBAND, ItemDict.NIGHTCAP, ItemDict.NIGHTMARE_MASK, ItemDict.SNAPBACK, ItemDict.SQUARE_HAT, ItemDict.STRAW_HAT, ItemDict.TOP_HAT, ItemDict.TRACE_TATTOO, ItemDict.WHISKERS,
                    ItemDict.SCARF, ItemDict.ASCOT, ItemDict.MEDAL, ItemDict.SASH, ItemDict.NECKLACE, ItemDict.NECKWARMER, ItemDict.TIE,
                    ItemDict.ALL_SEASON_JACKET, ItemDict.APRON, ItemDict.BATHROBE, ItemDict.NOMAD_VEST, ItemDict.HOODED_SWEATSHIRT, ItemDict.ONESIE, ItemDict.OVERALLS, ItemDict.OVERCOAT, ItemDict.PUNK_JACKET, ItemDict.RAINCOAT, ItemDict.SPORTBALL_UNIFORM, ItemDict.SUIT_JACKET, ItemDict.WEDDING_DRESS,
                    ItemDict.JEANS, ItemDict.CHINO_SHORTS, ItemDict.JEAN_SHORTS, ItemDict.CHINOS, ItemDict.LONG_SKIRT, ItemDict.PUFF_SKIRT, ItemDict.SHORT_SKIRT, ItemDict.SUPER_SHORTS, ItemDict.TIGHTIES, ItemDict.TORN_JEANS,
                    ItemDict.SAILCLOTH,
                    ItemDict.BUTTON_DOWN, ItemDict.ISLANDER_TATTOO, ItemDict.LINEN_BUTTON, ItemDict.LONG_SLEEVE_TEE, ItemDict.PLAID_BUTTON, ItemDict.SHORT_SLEEVE_TEE, ItemDict.STRIPED_SHIRT, ItemDict.SWEATER, ItemDict.TANKER, ItemDict.TURTLENECK,
                    ItemDict.SNEAKERS, ItemDict.FLASH_HEELS, ItemDict.WING_SANDLES, ItemDict.HIGH_TOPS, ItemDict.TALL_BOOTS,
                    ItemDict.GOLDEN_SPINACH, ItemDict.GOLDEN_POTATO, ItemDict.GOLDEN_STRAWBERRY, ItemDict.GOLDEN_CARROT, ItemDict.GOLDEN_CHERRY, ItemDict.GOLDEN_OLIVE
                }
            },
            {
                Area.AreaEnum.S2,
                new List<Item>()
                {
                    ItemDict.ADAMANTITE_ORE, ItemDict.ADAMANTITE_BAR, ItemDict.AMETHYST, ItemDict.AQUAMARINE, ItemDict.DIAMOND, ItemDict.EARTH_CRYSTAL, ItemDict.EMERALD, ItemDict.FIRE_CRYSTAL, ItemDict.GOLD_BAR, ItemDict.GOLD_ORE,
                    ItemDict.IRON_BAR, ItemDict.IRON_ORE, ItemDict.MYTHRIL_BAR, ItemDict.MYTHRIL_ORE, ItemDict.QUARTZ, ItemDict.RUBY, ItemDict.SAPPHIRE, ItemDict.WATER_CRYSTAL, ItemDict.WIND_CRYSTAL, ItemDict.OPAL, ItemDict.TOPAZ,
                    ItemDict.TRILOBITE, ItemDict.FOSSIL_SHARDS, ItemDict.PRIMORDIAL_SHELL, ItemDict.OLD_BONE, ItemDict.ALBINO_WING,
                    ItemDict.GOLDEN_BEET, ItemDict.GOLDEN_BELLPEPPER, ItemDict.GOLDEN_BROCCOLI, ItemDict.GOLDEN_FLAX, ItemDict.GOLDEN_CABBAGE, ItemDict.GOLDEN_PUMPKIN, ItemDict.GOLDEN_APPLE, ItemDict.GOLDEN_LEMON
                }
            },
            {
                Area.AreaEnum.S3,
                new List<Item>()
                {
                    ItemDict.ANCIENT_KEY, ItemDict.IGNEOUS_KEY, ItemDict.CRYSTAL_KEY, ItemDict.SEDIMENTARY_KEY, ItemDict.METAMORPHIC_KEY, ItemDict.SKELETON_KEY,
                    ItemDict.ROYAL_JELLY, ItemDict.POLLEN_PUFF, ItemDict.QUEENS_STINGER,
                    ItemDict.LINEN_CLOTH, ItemDict.COTTON_CLOTH, ItemDict.WOOLEN_CLOTH, ItemDict.LUCKY_COIN,
                    ItemDict.GOLDEN_ONION, ItemDict.GOLDEN_CUCUMBER, ItemDict.GOLDEN_CACTUS, ItemDict.GOLDEN_EGGPLANT, ItemDict.GOLDEN_COTTON, ItemDict.GOLDEN_TOMATO, ItemDict.GOLDEN_WATERMELON_SLICE,
                    ItemDict.GOLDEN_COCONUT, ItemDict.GOLDEN_ORANGE, ItemDict.GOLDEN_BANANA,
                    ItemDict.FAIRY_DUST
                }
            },
            {
                Area.AreaEnum.S4,
                new List<Item>()
                {
                    ItemDict.BLACK_CANDLE, ItemDict.BURST_STONE, ItemDict.PRIMEVAL_ELEMENT, ItemDict.HEART_VESSEL, ItemDict.INVINCIROID, ItemDict.LAND_ELEMENT,
                    ItemDict.LAVENDER_INCENSE, ItemDict.MOSS_BOTTLE, ItemDict.PHILOSOPHERS_STONE, ItemDict.SALTED_CANDLE, ItemDict.SEA_ELEMENT, ItemDict.SKY_BOTTLE, ItemDict.SKY_ELEMENT, ItemDict.SOOTHE_CANDLE, ItemDict.SPICED_CANDLE, ItemDict.SUGAR_CANDLE, 
                    ItemDict.TROPICAL_BOTTLE, ItemDict.VOODOO_STEW, ItemDict.UNSTABLE_LIQUID, ItemDict.SHIMMERING_SALVE,
                    ItemDict. ROYAL_CREST, ItemDict.MIDIAN_SYMBOL, ItemDict.UNITY_CREST, ItemDict.COMPRESSION_CREST, ItemDict.POLYMORPH_CREST, ItemDict.PHILOSOPHERS_CREST, ItemDict.DASHING_CREST, ItemDict.FROZEN_CREST, ItemDict.MUTATING_CREST, ItemDict.MYTHICAL_CREST,
                    ItemDict.VAMPYRIC_CREST, ItemDict.BREWERY_CREST, ItemDict.CLOUD_CREST, ItemDict.BUTTERFLY_CHARM, ItemDict.DROPLET_CHARM, ItemDict.CHURN_CHARM, ItemDict.PRIMAL_CHARM, ItemDict.SNOUT_CHARM, ItemDict.SUNRISE_CHARM, ItemDict.SUNFLOWER_CHARM, ItemDict.SALTY_CHARM, ItemDict.VOLCANIC_CHARM,
                    ItemDict.SPINED_CHARM, ItemDict.MANTLE_CHARM, ItemDict.MUSHY_CHARM, ItemDict.DANDYLION_CHARM, ItemDict.LUMINOUS_RING, ItemDict.BLIND_RING, ItemDict.FLIGHT_RING, ItemDict.FLORAL_RING, ItemDict.GLIMMER_RING, ItemDict.MONOCULTURE_RING, ItemDict.LUMBER_RING, ItemDict.BAKERY_RING,
                    ItemDict.ROSE_RING, ItemDict.OCEANIC_RING, ItemDict.MUSICBOX_RING, ItemDict.SHELL_RING, ItemDict.FURNACE_RING, ItemDict.ACID_BRACER, ItemDict.URCHIN_BRACER, ItemDict.FLUFFY_BRACER, ItemDict.DRUID_BRACER, ItemDict.TRADITION_BRACER, ItemDict.SANDSTORM_BRACER, ItemDict.DWARVEN_CHILDS_BRACER,
                    ItemDict.STRIPED_BRACER, ItemDict.CARNIVORE_BRACER, ItemDict.PURIFICATION_BRACER, ItemDict.SCRAP_BRACER, ItemDict.PIN_BRACER, ItemDict.ESSENCE_BRACER, ItemDict.DISSECTION_PENDANT, ItemDict.SOUND_PENDANT, ItemDict.GAIA_PENDANT, ItemDict.CYCLE_PENDANT, ItemDict.EROSION_PENDANT, ItemDict.POLYCULTURE_PENDANT,
                    ItemDict.CONTRACT_PENDANT, ItemDict.LADYBUG_PENDANT, ItemDict.DYNAMITE_PENDANT, ItemDict.OILY_PENDANT, ItemDict.NEUTRALIZED_PENDANT, ItemDict.STREAMLINE_PENDANT, ItemDict.TORNADO_PENDANT,
                    ItemDict.ORIGAMI_AIRPLANE, ItemDict.ORIGAMI_BALL, ItemDict.ORIGAMI_BEETLE, ItemDict.ORIGAMI_BOX, ItemDict.ORIGAMI_DRAGON, ItemDict.ORIGAMI_FAN, ItemDict.ORIGAMI_FISH, ItemDict.ORIGAMI_FLOWER, ItemDict.ORIGAMI_FROG, ItemDict.ORIGAMI_LEAF, ItemDict.ORIGAMI_LION, ItemDict.ORIGAMI_SAILBOAT,
                    ItemDict.ORIGAMI_SWAN, ItemDict.ORIGAMI_TIGER, ItemDict.ORIGAMI_TURTLE, ItemDict.ORIGAMI_WHALE, ItemDict.ORIGAMI_KITE, ItemDict.ORIGAMI_HEART, ItemDict.ORIGAMI_CANDY, ItemDict.ORIGAMI_RABBIT,
                    ItemDict.ICE_NINE
                }
            },
            {
                Area.AreaEnum.APEX,
                new List<Item>()
                {
                    ItemDict.ADAMANTITE_ORE, ItemDict.GOLD_CHIP, ItemDict.IRON_CHIP, ItemDict.MYTHRIL_CHIP, 
                    ItemDict.SWEET_INCENSE, ItemDict.LAVENDER_INCENSE, ItemDict.COLD_INCENSE, ItemDict.IMPERIAL_INCENSE, ItemDict.FRESH_INCENSE
                }
            },
        };

        private static int PROCESSING_TIME = 20 * 60;

        private static int MAX_CAPACITY = 1;

        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private ExtractorState state;
        private ResultHoverBox resultHoverBox;

        public PEntitySynthesizer(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("finished", 0, 0, true);
            sprite.AddLoop("working", 4, 5, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = ExtractorState.WORKING;
            this.timeRemaining = PROCESSING_TIME;
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
            string stateStr = saveState.TryGetData("state", ExtractorState.WORKING.ToString());
            if (stateStr.Equals(ExtractorState.WORKING.ToString()))
            {
                state = ExtractorState.WORKING;
            }
            else if (stateStr.Equals(ExtractorState.FINISHED.ToString()))
            {
                state = ExtractorState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if(sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoopIfNot("working");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == ExtractorState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("finished");
                }
            }

            if (heldItem.GetItem() != ItemDict.NONE)
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
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }

                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = ExtractorState.WORKING;
                timeRemaining = PROCESSING_TIME;
            } 
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        private static Item GetSynthesizedItem(Area.AreaEnum areaEnum)
        {
            List<Item> areaItems = SYNTHESIZED_ITEMS[areaEnum];
            return areaItems[Util.RandInt(0, areaItems.Count - 1)];
        }

        public void Tick(int time, EntityPlayer player, Area area, World world)
        {
            timeRemaining -= time;
            if (timeRemaining <= 0 && state == ExtractorState.WORKING)
            {
                heldItem = new ItemStack(GetSynthesizedItem(area.GetAreaEnum()), 1);
                sprite.SetLoop("placement");
                if(heldItem.GetQuantity() == MAX_CAPACITY)
                {
                    state = ExtractorState.FINISHED;
                }
            }
        }
    }
}
