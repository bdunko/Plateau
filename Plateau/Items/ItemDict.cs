using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Plateau.Components.Util;
using static Plateau.Items.Item;

namespace Plateau.Items
{
    public class ItemDict
    {
        private static bool SKIP_RECOLORS = false;
        
        private static int DEFAULT_STACK_SIZE = 99;

        private static float SILVER_MULTIPLIER = 1.5f;
        private static float GOLD_MULTIPLIER = 2.0f;
        private static float PHANTOM_MULTIPLIER = 3.0f;

        public static Item NONE;
        public static DamageDealingItem HOE, IRON_HOE, MITHRIL_HOE, ADAMANTITE_HOE, 
            WATERING_CAN, IRON_CAN, MITHRIL_CAN, ADAMANTITE_CAN, 
            AXE, IRON_AXE, MITHRIL_AXE, ADAMANTITE_AXE, 
            PICKAXE, IRON_PICKAXE, MITHRIL_PICKAXE, ADAMANTITE_PICKAXE, 
            FISHING_ROD, IRON_ROD, MITHRIL_ROD, ADAMANTITE_ROD;
        public static Item SHEARS, MILKING_PAIL, BASKET;
        public static Item BEET, BELLPEPPER, BROCCOLI, CABBAGE, CACTUS, CARROT, COTTON, CUCUMBER, EGGPLANT, FLAX, ONION, POTATO, PUMPKIN, SPINACH, STRAWBERRY, TOMATO, WATERMELON_SLICE,
            APPLE, BANANA, CHERRY, COCONUT, LEMON, OLIVE, ORANGE;
        public static Item SILVER_BEET, SILVER_BELLPEPPER, SILVER_BROCCOLI, SILVER_CABBAGE, SILVER_CACTUS, SILVER_CARROT, SILVER_COTTON, SILVER_CUCUMBER, SILVER_EGGPLANT, SILVER_FLAX, SILVER_ONION, SILVER_POTATO, SILVER_PUMPKIN, SILVER_SPINACH, SILVER_STRAWBERRY, SILVER_TOMATO, SILVER_WATERMELON_SLICE,
            SILVER_APPLE, SILVER_BANANA, SILVER_CHERRY, SILVER_COCONUT, SILVER_LEMON, SILVER_OLIVE, SILVER_ORANGE;
        public static Item GOLDEN_BEET, GOLDEN_BELLPEPPER, GOLDEN_BROCCOLI, GOLDEN_CABBAGE, GOLDEN_CACTUS, GOLDEN_CARROT, GOLDEN_COTTON, GOLDEN_CUCUMBER, GOLDEN_EGGPLANT, GOLDEN_FLAX, GOLDEN_ONION, GOLDEN_POTATO, GOLDEN_PUMPKIN, GOLDEN_SPINACH, GOLDEN_STRAWBERRY, GOLDEN_TOMATO, GOLDEN_WATERMELON_SLICE,
            GOLDEN_APPLE, GOLDEN_BANANA, GOLDEN_CHERRY, GOLDEN_COCONUT, GOLDEN_LEMON, GOLDEN_OLIVE, GOLDEN_ORANGE;
        public static Item PHANTOM_BEET, PHANTOM_BELLPEPPER, PHANTOM_BROCCOLI, PHANTOM_CABBAGE, PHANTOM_CACTUS, PHANTOM_CARROT, PHANTOM_COTTON, PHANTOM_CUCUMBER, PHANTOM_EGGPLANT, PHANTOM_FLAX, PHANTOM_ONION, PHANTOM_POTATO, PHANTOM_PUMPKIN, PHANTOM_SPINACH, PHANTOM_STRAWBERRY, PHANTOM_TOMATO, PHANTOM_WATERMELON_SLICE,
            PHANTOM_APPLE, PHANTOM_BANANA, PHANTOM_CHERRY, PHANTOM_COCONUT, PHANTOM_LEMON, PHANTOM_OLIVE, PHANTOM_ORANGE;
        public static Item BEET_SEEDS, BELLPEPPER_SEEDS, BROCCOLI_SEEDS, CABBAGE_SEEDS,
            CACTUS_SEEDS, CARROT_SEEDS, COTTON_SEEDS, CUCUMBER_SEEDS, EGGPLANT_SEEDS, FLAX_SEEDS, ONION_SEEDS,
            POTATO_SEEDS, PUMPKIN_SEEDS, SPINACH_SEEDS, STRAWBERRY_SEEDS, TOMATO_SEEDS, WATERMELON_SEEDS, SPORES;
        public static PlantableItem APPLE_SAPLING, BANANA_PALM_SAPLING, CHERRY_SAPLING, COCONUT_PALM_SAPLING, LEMON_SAPLING, OLIVE_SAPLING, ORANGE_SAPLING;
        public static PlantableItem BERRY_BUSH_PLANTER;
        public static Item SHINING_BEET_SEEDS, SHINING_BELLPEPPER_SEEDS, SHINING_BROCCOLI_SEEDS, SHINING_CABBAGE_SEEDS,
            SHINING_CACTUS_SEEDS, SHINING_CARROT_SEEDS, SHINING_COTTON_SEEDS, SHINING_CUCUMBER_SEEDS, SHINING_EGGPLANT_SEEDS, SHINING_FLAX_SEEDS, SHINING_ONION_SEEDS,
            SHINING_POTATO_SEEDS, SHINING_PUMPKIN_SEEDS, SHINING_SPINACH_SEEDS, SHINING_STRAWBERRY_SEEDS, SHINING_TOMATO_SEEDS, SHINING_WATERMELON_SEEDS;
        public static Item BUTTER, CHEESE, CREAM, EGG, MAYONNAISE, MILK, TRUFFLE, WOOL, GOLDEN_EGG, GOLDEN_WOOL;
        public static Item WILD_HONEY, BEESWAX, ROYAL_JELLY, POLLEN_PUFF, QUEENS_STINGER;
        public static Item FAIRY_DUST, GOLDEN_LEAF, HONEYCOMB, ICE_NINE, MOSSY_BARK, OYSTER_MUSHROOM, WOOD, HARDWOOD;
        public static Item LOAMY_COMPOST, QUALITY_COMPOST, DEW_COMPOST, SWEET_COMPOST, DECAY_COMPOST, FROST_COMPOST, THICK_COMPOST, SHINING_COMPOST;
        public static Item BLUE_DYE, NAVY_DYE, BLACK_DYE, RED_DYE, PINK_DYE, LIGHT_BROWN_DYE, DARK_BROWN_DYE, ORANGE_DYE, YELLOW_DYE, PURPLE_DYE, GREEN_DYE, OLIVE_DYE, WHITE_DYE, LIGHT_GREY_DYE, DARK_GREY_DYE, UN_DYE;
        public static Item COTTON_CLOTH, WOOLEN_CLOTH, LINEN_CLOTH;
        public static Item WILD_MEAT, BOAR_HIDE;
        public static Item BLACKENED_OCTOPUS, BLUEGILL, BOXER_LOBSTER, CARP, CATFISH, CAVEFISH, CAVERN_TETRA, CLOUD_FLOUNDER, CRAB, DARK_ANGLER, EMPEROR_SALMON, GREAT_WHITE_SHARK,
            HERRING, INFERNAL_SHARK, INKY_SQUID, JUNGLE_PIRANHA, LAKE_TROUT, LUNAR_WHALE, MACKEREL, MOLTEN_SQUID, ONYX_EEL, PIKE, PUFFERFISH, QUEEN_AROWANA, RED_SNAPPER, SALMON,
            SARDINE, SEA_TURTLE, SHRIMP, SKY_PIKE, STORMBRINGER_KOI, STRIPED_BASS, SUNFISH, SWORDFISH, TUNA, WHITE_BLOWFISH;
        public static Item BANDED_DRAGONFLY, BROWN_CICADA, CAVEWORM, EARTHWORM, EMPRESS_BUTTERFLY, FIREFLY, HONEY_BEE, JEWEL_SPIDER, 
            LANTERN_MOTH, PINK_LADYBUG, RICE_GRASSHOPPER, SNAIL, SOLDIER_ANT, STAG_BEETLE, STINGER_HORNET, YELLOW_BUTTERFLY;
        public static Item ALBINO_WING, BAT_WING, BIRDS_NEST, BLACK_FEATHER, BLUE_FEATHER, GUANO, PRISMATIC_FEATHER, PURE_FEATHER, WHITE_FEATHER, RED_FEATHER;
        public static Item CLAY, STONE, ADAMANTITE_ORE, ADAMANTITE_BAR, AMETHYST, AQUAMARINE, COAL, DIAMOND, EARTH_CRYSTAL, EMERALD, FIRE_CRYSTAL, GOLD_BAR, GOLD_ORE,
            IRON_BAR, IRON_ORE, MYTHRIL_BAR, MYTHRIL_ORE, MYTHRIL_CHIP, QUARTZ, RUBY, SALT_SHARDS, SAPPHIRE, SCRAP_IRON, WATER_CRYSTAL, WIND_CRYSTAL, OPAL, TOPAZ, TRILOBITE, IRON_CHIP, GOLD_CHIP, FOSSIL_SHARDS,
            PRIMORDIAL_SHELL, OLD_BONE;
        public static Item WEEDS, LUCKY_COIN,
            CLAM, CRIMSON_CORAL, FLAWLESS_CONCH, OYSTER, PEARL, SEA_URCHIN, SEAWEED, RED_GINGER,
            BLACKBERRY, SASSAFRAS, PERSIMMON, WILD_RICE,
            BLUEBELL, CHICKWEED, NETTLES, RASPBERRY, SUNFLOWER,
            BLUEBERRY, ELDERBERRY, LAVENDER, MARIGOLD,
            CHANTERELLE, CHICORY_ROOT, SNOW_CRYSTAL, SNOWDROP, WINTERGREEN,
            MOREL, MOUNTAIN_WHEAT, SPICY_LEAF,
            CAVE_FUNGI, CAVE_SOYBEAN, EMERALD_MOSS,
            MAIZE, PINEAPPLE, CACAO_BEAN, VANILLA_BEAN,
            BAMBOO, SKY_ROSE, SHIITAKE,
            CRYSTAL_KEY, SEDIMENTARY_KEY, METAMORPHIC_KEY, IGNEOUS_KEY, ANCIENT_KEY, SKELETON_KEY,
            PINECONE;
        //materials
        public static Item BOARD, PLANK, BRICKS, GEARS, PAPER;
        //glassblower
        public static PlaceableItem GLASS_SHEET;
        //perfume
        public static Item AUTUMNS_KISS, BIZARRE_PERFUME, BLISSFUL_SKY, FLORAL_PERFUME, OCEAN_GUST, OIL, RED_ANGEL, SUMMERS_GIFT, SWEET_BREEZE, WARM_MEMORIES, VANILLA_EXTRACT, MINT_EXTRACT;
        //alchemy
        public static Item BLACK_CANDLE, BURST_STONE, COLD_INCENSE, PRIMEVAL_ELEMENT, FRESH_INCENSE, HEART_VESSEL, IMPERIAL_INCENSE, INVINCIROID, LAND_ELEMENT,
            LAVENDER_INCENSE, MOSS_BOTTLE, PHILOSOPHERS_STONE, SALTED_CANDLE, SEA_ELEMENT, SKY_BOTTLE, SKY_ELEMENT, SOOTHE_CANDLE, SPICED_CANDLE, SUGAR_CANDLE, SWEET_INCENSE,
            TROPICAL_BOTTLE, VOODOO_STEW, UNSTABLE_LIQUID, SHIMMERING_SALVE;
        //machines
        public static PlaceableItem CHEST, COMPOST_BIN;
        public static PlaceableItem TOTEM_OF_THE_COW, TOTEM_OF_THE_PIG, TOTEM_OF_THE_SHEEP, TOTEM_OF_THE_CHICKEN, TOTEM_OF_THE_DOG, TOTEM_OF_THE_CAT, TOTEM_OF_THE_ROOSTER;
        public static PlaceableItem DAIRY_CHURN, MAYONNAISE_MAKER, LOOM, CHEFS_TABLE, CLAY_OVEN, PERFUMERY, BEEHIVE, BIRDHOUSE, SEED_MAKER, POTTERY_WHEEL, FURNACE, GEMSTONE_REPLICATOR, COMPRESSOR,
            MUSHBOX, SOULCHEST, FLOWERBED, GLASSBLOWER, AQUARIUM, ALCHEMY_CAULDRON, ANVIL, KEG, SKY_STATUE, DRACONIC_PILLAR, WORKBENCH, VIVARIUM, SYNTHESIZER, PAINTERS_PRESS, JEWELERS_BENCH, TERRARIUM, 
            BARBER_POLE, ENCHANTED_VANITY, ALCHEMIZER, ORIGAMI_STATION, CLONING_MACHINE, RECYCLER, DRYING_RACK, EXTRACTOR;
        //lights
        public static PlaceableItem TORCH, BRAZIER, CAMPFIRE, FIREPIT, FIREPLACE, LAMP, LANTERN, STREETLAMP, STREETLIGHT;
        //normal decor
        public static PlaceableItem BAMBOO_POT, BELL, BLACKBOARD, BOOMBOX, BOX, BUOY, CANVAS, CART, CLAY_BALL, CLAY_BOWL, CLAY_DOLL, CLAY_SLATE, CLOTHESLINE, CRATE, CUBE_STATUE, CYMBAL, DECORATIVE_BOULDER, DECORATIVE_LOG,
            DRUM, FIRE_HYDRANT, FLAGPOLE, FROST_SCULPTURE, GARDEN_ARCH, GRANDFATHER_CLOCK, GUITAR_PLACEABLE, GYM_BENCH, HAMMOCK, HAYBALE, ICE_BLOCK, IGLOO, LATTICE, LIFEBUOY_SIGN, LIGHTNING_ROD, MAILBOX, MARKET_STALL, MILK_JUG,
            MINECART, PET_BOWL, PIANO, ORNATE_MIRROR, PILE_OF_BRICKS, POSTBOX, POTTERY_JAR, POTTERY_PLATE, POTTERY_MUG, POTTERY_VASE, PYRAMID_STATUE, RECYCLING_BIN, SANDBOX, SANDCASTLE, SCARECROW, SEESAW, SIGNPOST,
            SLIDE, SNOWMAN, SOFA, SOLAR_PANEL, SPHERE_STATUE, STATUE, STONE_COLUMN, SURFBOARD, SWINGS, TARGET, TELEVISION, TOOLBOX, TRAFFIC_CONE, TRAFFIC_LIGHT, TRASHCAN, UMBRELLA, UMBRELLA_TABLE, WAGON,
            WATER_PUMP, WATERTOWER, WELL, WHEELBARROW, WHITEBOARD, WOODEN_BENCH, WOODEN_CHAIR, WOODEN_COLUMN, WOODEN_LONGTABLE, WOODEN_POST, WOODEN_ROUNDTABLE, WOODEN_SQUARETABLE, WOODEN_STOOL, DRUMSET, HARP,
            XYLOPHONE;
        //wall decor
        public static PlaceableItem WALL_MIRROR, ANATOMICAL_POSTER, BANNER, CLOCK, FULL_THROTTLE_GRAFFITI, HEARTBREAK_GRAFFITI, HELIX_POSTER, HEROINE_GRAFFITI, HORIZONTAL_MIRROR, LEFTWARD_GRAFFITI, NOIZEBOYZ_GRAFFITI, 
            RAINBOW_GRAFFITI, RETRO_GRAFFITI, RIGHT_ARROW_GRAFFITI, SMILE_GRAFFITI, SOURCE_UNKNOWN_GRAFFITI, TOOLRACK, TRIPLE_MIRRORS;
        public static PlaceableItem PAINTING_OASIS, PAINTING_FOUR, PAINTING_FUTURE,
            PAINTING_ARCTIC, PAINTING_FATE,
            PAINTING_ACCEPTANCE, PAINTING_BALANCE, PAINTING_BLACK, PAINTING_COFFEE, PAINTING_DICHOTOMY, PAINTING_FIREBALL, PAINTING_LION, PAINTING_MINTGREEN, PAINTING_GROVE, PAINTING_PUZZLE, PAINTING_TOXIN, PAINTING_CREAMPUFF, PAINTING_SELF, PAINTING_RAIDER, PAINTING_VINEFLOWER, PAINTING_FJORD,
            PAINTING_DITHER,
            PAINTING_BEACHDAY, PAINTING_RIVER,
            PAINTING_BEDTIME, PAINTING_ILOVEYOU, PAINTING_CHANGES, PAINTING_LIBERTY, PAINTING_INTERLUDE,
            PAINTING_SKYROSE, PAINTING_EARTH, PAINTING_CALCULATOR,
            PAINTING_CORAL, PAINTING_SEASONS, PAINTING_WHATEVER,
            PAINTING_SOLEMN, PAINTING_MOONSET, PAINTING_MONA, PAINTING_OVERHANG,
            PAINTING_SUNSET, PAINTING_SPICE, PAINTING_WINDOW, PAINTING_ET, PAINTING_LAVENDER, PAINTING_THREADS,
            PAINTING_LAUNCH, PAINTING_FABLE, PAINTING_GROWTH, PAINTING_SHADES, PAINTING_GENIUS,
            PAINTING_RESONANT;
        public static PlaceableItem HERALDIC_SHIELD, DECORATIVE_SWORD, SUIT_OF_ARMOR, HORSESHOE;
        //wallpapers
        public static WallpaperItem WAVE_WALLPAPER, STAR_WALLPAPER, BUBBLE_WALLPAPER, SOLID_WALLPAPER, VERTICAL_WALLPAPER, HORIZONTAL_WALLPAPER, DOT_WALLPAPER, POLKA_WALLPAPER, INVADER_WALLPAPER, ODD_WALLPAPER;
        //floor decor
        public static PlaceableItem CONCRETE_FLOOR, STREET_FLOOR, CARPET_FLOOR, BOARDWALK_FLOOR, STEPPING_STONE_FLOOR, 
            FOOTPRINT_FLOOR, MAT_FLOOR, SQUARE_FLOOR, TATAMI_FLOOR, THIN_TATAMI_FLOOR, TRIANGULATE_FLOOR, WOODEN_FLOOR;
        //fences
        public static PlaceableItem STONE_FENCE, WOODEN_FENCE, TALL_FENCE, BAMBOO_FENCE, METAL_FENCE, MYTHRIL_FENCE, GOLDEN_FENCE, GLASS_FENCE;
        //acc
        public static Item ROYAL_CREST, MIDIAN_SYMBOL, UNITY_CREST, COMPRESSION_CREST, POLYMORPH_CREST, PHILOSOPHERS_CREST, DASHING_CREST, FROZEN_CREST, MUTATING_CREST, MYTHICAL_CREST,
            VAMPYRIC_CREST, BREWERY_CREST, CLOUD_CREST, BUTTERFLY_CHARM, DROPLET_CHARM, CHURN_CHARM, PRIMAL_CHARM, SNOUT_CHARM, SUNRISE_CHARM, SUNFLOWER_CHARM, SALTY_CHARM, VOLCANIC_CHARM,
            SPINED_CHARM, MANTLE_CHARM, MUSHY_CHARM, DANDYLION_CHARM, LUMINOUS_RING, BLIND_RING, FLIGHT_RING, FLORAL_RING, GLIMMER_RING, MONOCULTURE_RING, LUMBER_RING, BAKERY_RING,
            ROSE_RING, OCEANIC_RING, MUSICBOX_RING, SHELL_RING, FURNACE_RING, ACID_BRACER, URCHIN_BRACER, FLUFFY_BRACER, DRUID_BRACER, TRADITION_BRACER, SANDSTORM_BRACER, DWARVEN_CHILDS_BRACER,
            STRIPED_BRACER, CARNIVORE_BRACER, PURIFICATION_BRACER, SCRAP_BRACER, PIN_BRACER, ESSENCE_BRACER, DISSECTION_PENDANT, SOUND_PENDANT, GAIA_PENDANT, CYCLE_PENDANT, EROSION_PENDANT, POLYCULTURE_PENDANT,
            CONTRACT_PENDANT, LADYBUG_PENDANT, DYNAMITE_PENDANT, OILY_PENDANT, NEUTRALIZED_PENDANT, STREAMLINE_PENDANT, TORNADO_PENDANT;
        public static Item ORIGAMI_AIRPLANE, ORIGAMI_BALL, ORIGAMI_BEETLE, ORIGAMI_BOX, ORIGAMI_DRAGON, ORIGAMI_FAN, ORIGAMI_FISH, ORIGAMI_FLOWER, ORIGAMI_FROG, ORIGAMI_LEAF, ORIGAMI_LION, ORIGAMI_SAILBOAT,
            ORIGAMI_SWAN, ORIGAMI_TIGER, ORIGAMI_TURTLE, ORIGAMI_WHALE;
        //clothing
        public static ClothingItem CLOTHING_NONE;
        public static ClothingItem BACKPACK, RUCKSACK, CAPE, WOLF_TAIL, FOX_TAIL, CAT_TAIL, CLOCKWORK, ROBO_ARMS, GUITAR;
        public static ClothingItem SHORT_SOCKS, LONG_SOCKS, STRIPED_SOCKS, FESTIVE_SOCKS, MISMATTCHED;
        public static ClothingItem EARRING_STUD, DANGLE_EARRING, PIERCING;
        public static ClothingItem GLASSES, BLINDFOLD, EYEPATCH, GOGGLES, PROTECTIVE_VISOR, QUERADE_MASK, SNORKEL, SUNGLASSES;
        public static ClothingItem WOOL_MITTENS, WORK_GLOVES, BOXING_MITTS;
        public static ClothingItem BASEBALL_CAP, TEN_GALLON, BANDANA, BOWLER, BUNNY_EARS, BUTTERFLY_CLIP, CAMEL_HAT, CAT_EARS, CHEFS_HAT, CONICAL_FARMER, DINO_MASK, DOG_MASK, FACEMASK, FLAT_CAP, HEADBAND, NIGHTCAP, NIGHTMARE_MASK, SNAPBACK, SQUARE_HAT, STRAW_HAT, TOP_HAT, TRACE_TATTOO, WHISKERS;
        public static ClothingItem SCARF, ASCOT, MEDAL, SASH, NECKLACE, NECKWARMER, TIE;
        public static ClothingItem ALL_SEASON_JACKET, APRON, BATHROBE, NOMAD_VEST, HOODED_SWEATSHIRT, ONESIE, OVERALLS, OVERCOAT, PUNK_JACKET, RAINCOAT, SPORTBALL_UNIFORM, SUIT_JACKET, WEDDING_DRESS;
        public static ClothingItem JEANS, CHINO_SHORTS, JEAN_SHORTS, CHINOS, LONG_SKIRT, PUFF_SKIRT, SHORT_SKIRT, SUPER_SHORTS, TIGHTIES, TORN_JEANS;
        public static ClothingItem SAILCLOTH;
        public static ClothingItem BUTTON_DOWN, ISLANDER_TATTOO, LINEN_BUTTON, LONG_SLEEVE_TEE, PLAID_BUTTON, SHORT_SLEEVE_TEE, STRIPED_SHIRT, SWEATER, TANKER, TURTLENECK;
        public static ClothingItem SNEAKERS, FLASH_HEELS, WING_SANDLES, HIGH_TOPS, TALL_BOOTS;
        public static ClothingItem HAIR_AFRO_ALFONSO, HAIR_BAREBONES_BRIAN, HAIR_BENNY_BOWLCUT, HAIR_CARLOS_COOL, HAIR_CLEAN_CONOR, HAIR_COMBED_CHRISTOPH, HAIR_COWLICK_COLTON,
            HAIR_DIRTY_JACK, HAIR_FREDDIE_FRINGE, HAIR_GABRIEL_PART, HAIR_LAZY_XAVIER, HAIR_MAXWELL_MOHAWK, HAIR_MR_BALD, HAIR_OVERHANG_OWEN, HAIR_PONYTAIL_TONYTALE, HAIR_SKULLCAP_STEVENS, HAIR_LUCKY_LUKE;
        public static ClothingItem HAIR_ALIENATED_ALICE, HAIR_BERTHA_BUN, HAIR_CLEANCUT_CHARLOTTE, HAIR_EARNEST_EMMA, HAIR_FLASHY_FRIZZLE, HAIR_FLUFFY_FELICIA, HAIR_GORGEOUS_GEORGEANN, HAIR_HANGOVER_HANNA,
            HAIR_INNOCENT_ILIA, HAIR_LUXURY_LARA, HAIR_MOUNTAIN_CLIMBER_MADELINE, HAIR_PADMA_PERFECTION, HAIR_PERSEPHONE_PUNK, HAIR_SOPHIA_SWING, HAIR_STRICT_SUSIE, HAIR_THE_ORIGINAL_OLIVIA, HAIR_ZAPPY_ZADIE;
        public static ClothingItem FACIALHAIR_BARON_MUSTACHE, FACIALHAIR_BEARD, FACIALHAIR_CAVEMAN, FACIALHAIR_DROOPY, FACIALHAIR_FLUFF, FACIALHAIR_FULLBEARD, FACIALHAIR_GOATEE, FACIALHAIR_GOATEEBACK,
            FACIALHAIR_MONK, FACIALHAIR_SHORTBEARD, FACIALHAIR_SIDEBURNS, FACIALHAIR_SOULPATCH;
        public static ClothingItem SKIN_PEACH, SKIN_ALIEN, SKIN_CHOCOLATE, SKIN_DRIFTER, SKIN_ECRU, SKIN_EXEMPLAR, SKIN_MERIDIAN, SKIN_MIDNIGHT, SKIN_OLIVE, SKIN_PHANTOM, SKIN_RUSSET, SKIN_SNOW, SKIN_KID;
        public static ClothingItem EYES_AMBER, EYES_BLANK, EYES_BLUSH, EYES_BROWN, EYES_DOT, EYES_EMERALD, EYES_MINT, EYES_OCEAN, EYES_SILVER, EYES_SOLAR, EYES_TEAK, EYES_FROST;
        public static BuildingBlockItem SCAFFOLDING_WOOD, SCAFFOLDING_METAL, SCAFFOLDING_GOLDEN, SCAFFOLDING_MYTHRIL;
        public static BuildingBlockItem PLATFORM_WOOD, PLATFORM_METAL, PLATFORM_STONE, PLATFORM_GOLDEN, PLATFORM_MYTHRIL, PLATFORM_PLANK;
        public static BuildingBlockItem PLATFORM_WOOD_FARM, PLATFORM_METAL_FARM, PLATFORM_STONE_FARM, PLATFORM_GOLDEN_FARM, PLATFORM_MYTHRIL_FARM;
        public static BuildingBlockItem BLOCK_WOOD, BLOCK_METAL, BLOCK_STONE, BLOCK_GOLDEN, BLOCK_MYTHRIL, BLOCK_PLANK, BLOCK_BARK;
        public static BuildingBlockItem WALL_PLANK, WALL_METAL, WALL_STONE;
        public static EdibleItem FRIED_EGG, EGG_SCRAMBLE, BREAKFAST_POTATOES, SPICY_BACON, BLUEBERRY_PANCAKES, APPLE_MUFFIN, ROASTED_PUMPKIN, VEGGIE_SIDE_ROAST, WRAPPED_CABBAGE,
            SEAFOOD_PAELLA, SEASONAL_PIPERADE, SUPER_JUICE, POTATO_AND_BEET_FRIES, PICKLED_BEET_EGG, COCONUT_BOAR, SPRING_PIZZA, STRAWBERRY_SALAD, BOAR_STEW, BAKED_POTATO,
            FRESH_SALAD, STEWED_VEGGIES, MEATY_PIZZA, WATERMELON_ICE, SUSHI_ROLL, SEARED_TUNA, DELUXE_SUSHI, MUSHROOM_STIR_FRY, MOUNTAIN_TERIYAKI, LETHAL_SASHIMI,
            VANILLA_ICE_CREAM, BERRY_MILKSHAKE, MINT_CHOCO_BAR, MINTY_MELT, BANANA_SUNDAE, FRENCH_ONION_SOUP, TOMATO_SOUP, FARMERS_STEW, CREAM_OF_MUSHROOM, CREAMY_SPINACH,
            SHRIMP_GUMBO, FRIED_CATFISH, GRILLED_SALMON, BAKED_SNAPPER, SWORDFISH_POT, HONEY_STIR_FRY, BUTTERED_ROLLS, SAVORY_ROAST, STUFFED_FLOUNDER, CLAM_LINGUINI, LEMON_SHORTCAKE,
            CHERRY_CHEESECAKE, MOUNTAIN_BREAD, CHICKWEED_BLEND, CRISPY_GRASSHOPPER, REJUVENATION_TEA, HOMESTYLE_JELLY, SEAFOOD_BASKET, WILD_POPCORN, ELDERBERRY_TART,
            DARK_TEA, LICHEN_JUICE, AUTUMN_MASH, CAMPFIRE_COFFEE, BLIND_DINNER, FRIED_FISH, SARDINE_SNACK, DWARVEN_STEW, SWEET_COCO_TREAT, FRIED_OYSTERS, SURVIVORS_SURPRISE,
            EEL_ROLL, RAW_CALAMARI, STORMFISH, CREAMY_SQUID, ESCARGOT, LUMINOUS_STEW, COLESLAW, RATATOUILLE, EGGPLANT_PARMESAN,
            SEAWEED_SNACK, KLIPPFISK, BOAR_JERKY, VEGGIE_CHIPS, POTATO_CRISPS, DRIED_APPLE, DRIED_STRAWBERRY, WATERLESS_MELON, DRIED_CITRUS, DRIED_OLIVES, COCONUT_CHIPS, CHERRY_RAISINS, BANANA_CHIPS;
        public static EdibleItem APPLE_PRESERVES, APPLE_CIDER, BANANA_JAM, BEERNANA, MARMALADE, ALCORANGE, LEMONADE, SOUR_WINE, CHERRY_JELLY, CHERWINE, MARINATED_OLIVE,
            PICKLED_CARROT, GOOD_OL_PICKLES, BRINY_BEET, SOUSE_EGG, PICKLED_ONION, PERSIMMON_JAM, AUTUMNAL_WINE, BLACKBERRY_PRESERVES, BLACKBERRY_DIGESTIF, BLUEBERRY_JELLY, BLUEBERRY_CORDIAL,
            STRAWBERRY_BLAST_JAM, STRAWBERRY_SPIRIT, ELDERBERRY_APERITIF, ELDERBERRY_JAM, RASPBERRY_JELLY, RASPBERRY_LIQUEUR, TOMATO_SALSA, BLOODY_MARIE, AUTUMN_SALSA, PUMPKIN_CIDER,
            COCONUT_RUM, WATERMELON_JELLY, WATERMELON_WINE, PINEAPPLE_SALSA, TROPICAL_RUM;
        public static UsableItem COCONUT_MILK;
        //shrine certs
        public static Item FRIENDSHIP_CERT, LOVER_CERT, READER_CERT, LIBRARY_CERT, FESTIVAL_CERT, COMMUNITY_CERT, SHIPMENT_CERT, BARON_CERT, SPIRIT_CERT;

        private static Dictionary<string, Item> itemDictionary;

        private ItemDict()
        {
            throw new Exception("itemdict constructor");
        }

        public static Item GetItemByName(string name)
        {
            name = name.ToLower();
            if(itemDictionary.ContainsKey(name))
            {
                return itemDictionary[name];
            }
            Console.WriteLine("Warning: GetItemByName failed: " + name + " not found.");
            return ItemDict.NONE;
        }


        public static string GetColoredItemBaseForm(Item item)
        {
            string name = item.GetName();
            if (name.IndexOf("(") == -1)
                return name;
            return name.Substring(0, name.IndexOf("(")).Trim();
        }

        public static Item GetColoredItem(Item item, RecolorMap color)
        {
            string name = item.GetName() + " (" + color.name + ")";
            return GetItemByName(name);
        }

        public static ClothingItem GetColoredItem(ClothingItem item, RecolorMap color)
        {
            string name = item.GetName() + " (" + color.name + ")";
            return (ClothingItem)GetItemByName(name);
        }

        private static void AddToDictionary(Item item)
        {
            itemDictionary[item.GetName().ToLower()] = item;
        }

        private static void LoadAllInDictionary()
        {
            foreach(string key in itemDictionary.Keys)
            {
                itemDictionary[key].Load();
            }
        }

        private static ClothingItem MakeColoredVersion(ClothingItem item, RecolorMap recolor, string greyscaleItemIcon, string greyscaleTexture)
        {
            string name = item.GetName() + (" (" + recolor.name + ")");
            return new ClothingItem(name, greyscaleItemIcon, item.GetStackCapacity(), item.GetRawDescription(), item.GetValue(), greyscaleTexture, recolor, item.GetTags());
        }

        private static PlaceableItem MakeColoredVersion(PlaceableItem item, RecolorMap recolor, string greyscaleItemRecolor, string greyscaleItemNonRecolor, string greyscaleSpritesheetRecolor)
        {
            string name = item.GetName() + (" (" + recolor.name + ")");
            return new PlaceableItem(name, greyscaleItemNonRecolor, greyscaleItemRecolor, 
                greyscaleSpritesheetRecolor, item.GetPlacedTexturePath(), 
                item.GetPlaceableWidth(), item.GetPlaceableHeight(), item.GetStackCapacity(), item.GetRawDescription(), item.GetValue(), item.GetPlacedEntityType(), item.GetPlacementType(), 
                recolor, item.GetTags());
        }

        private static BuildingBlockItem MakeColoredVersion(BuildingBlockItem item, RecolorMap recolor, string greyscaleItem, string greyscaleSpritesheet)
        {
            string name = item.GetName() + (" (" + recolor.name + ")");
            return new BuildingBlockItem(name, greyscaleItem, greyscaleSpritesheet, item.GetBlockType(), item.GetStackCapacity(), item.GetRawDescription(), item.GetValue(), recolor, item.GetTags());
        }

        private static WallpaperItem MakeColoredVersion(WallpaperItem item, RecolorMap recolor, string greyscaleItemRecolor, string greyscaleItemNonRecolor, string greyscaleSpritesheetRecolor, string greyscaleTop, string greyscaleBottom)
        {
            string name = item.GetName() + (" (" + recolor.name + ")");
            return new WallpaperItem(name, greyscaleItemNonRecolor, greyscaleItemRecolor, 
                greyscaleSpritesheetRecolor, item.GetPlacedTexturePath(), 
                greyscaleTop, item.GetPlacedTextureTopPath(),
                greyscaleBottom, item.GetPlacedTextureBottomPath(), 
                item.GetPlaceableWidth(), item.GetPlaceableHeight(), item.GetStackCapacity(), item.GetRawDescription(), item.GetValue(), item.GetPlacedEntityType(), item.GetPlacementType(), recolor, item.GetTags());
        }

        public static void LoadContent(ContentManager content)
        {
            itemDictionary = new Dictionary<string, Item>();
            UsableItem.Initialize();

            AddToDictionary(NONE = new Item("none", Paths.ITEM_NONE, 1,"nonedesc", 0, Tag.NO_TRASH));
            AddToDictionary(HOE = new DamageDealingItem("Hoe", Paths.ITEM_HOE, 1, 1, "The classic farming tool. Used to hoe the fields. This one's a little rusty...", 0, Tag.TOOL, Tag.NO_TRASH, Tag.HOE));
            AddToDictionary(IRON_HOE = new DamageDealingItem("Iron Hoe", Paths.ITEM_IRON_HOE, 1, 2, "The classic farming tool. Used to hoe the fields. Reinforced with iron!", 0, Tag.TOOL, Tag.NO_TRASH, Tag.HOE));
            AddToDictionary(MITHRIL_HOE = new DamageDealingItem("Mythril Hoe", Paths.ITEM_MYTHRIL_HOE, 1, 3, "The classic farming tool. Used to hoe the fields. The mithril is stainless!", 0, Tag.TOOL, Tag.NO_TRASH, Tag.HOE));
            AddToDictionary(ADAMANTITE_HOE = new DamageDealingItem("Adamantite Hoe", Paths.ITEM_ADAMANTITE_HOE, 1, 6, "The classic farming tool. Used to hoe the fields. The greatest hoe of all!", 0, Tag.TOOL, Tag.NO_TRASH, Tag.HOE));
            AddToDictionary(WATERING_CAN = new DamageDealingItem("Watering Can", Paths.ITEM_WATERING_CAN, 1, 1, "A tin watering can. It's used to water crops. Just ignore the holes in the bottom...", 0, Tag.TOOL, Tag.NO_TRASH, Tag.WATERING_CAN));
            AddToDictionary(IRON_CAN = new DamageDealingItem("Iron Can", Paths.ITEM_IRON_CAN, 1, 2, "An iron watering can. It's used to water crops. Now with 100% less leaking!", 0, Tag.TOOL, Tag.NO_TRASH, Tag.WATERING_CAN));
            AddToDictionary(MITHRIL_CAN = new DamageDealingItem("Mythril Can", Paths.ITEM_MYTHRIL_CAN, 1, 3, "A mithril watering can. It's used to water crops. Any farmer would be jealous of a tool of this quality!", 0, Tag.TOOL, Tag.NO_TRASH, Tag.WATERING_CAN));
            AddToDictionary(ADAMANTITE_CAN = new DamageDealingItem("Adamantite Can", Paths.ITEM_ADAMANTITE_CAN, 1, 6, "An adamantite watering can. It's used to water crops. This is a watering can without match!", 0, Tag.TOOL, Tag.NO_TRASH, Tag.WATERING_CAN));
            AddToDictionary(AXE = new DamageDealingItem("Axe", Paths.ITEM_AXE, 1,3, "It looks kinda sharp? Used to chop down stumps and trees.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.AXE));
            AddToDictionary(IRON_AXE = new DamageDealingItem("Iron Axe", Paths.ITEM_IRON_AXE, 1, 5, "It looks pretty sharp. Used to chop down stumps and trees.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.AXE));
            AddToDictionary(MITHRIL_AXE = new DamageDealingItem("Mythril Axe", Paths.ITEM_MYTHRIL_AXE, 1, 7, "It looks crazy sharp! Used to chop down stumps and trees.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.AXE));
            AddToDictionary(ADAMANTITE_AXE = new DamageDealingItem("Adamantite Axe", Paths.ITEM_ADAMANTITE_AXE, 1, 12, "So sharp, it might cut you if you look at it too hard. Used to chop down stumps and trees.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.AXE));
            AddToDictionary(PICKAXE = new DamageDealingItem("Pickaxe", Paths.ITEM_PICKAXE, 1,3, "This is sorta heavy. Used to break stones and mine for minerals.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.PICKAXE));
            AddToDictionary(IRON_PICKAXE = new DamageDealingItem("Iron Pickaxe", Paths.ITEM_IRON_PICKAXE, 1, 5, "This is really heavy. Used to break stones and mine for minerals.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.PICKAXE));
            AddToDictionary(MITHRIL_PICKAXE = new DamageDealingItem("Mythril Pickaxe", Paths.ITEM_MYTHRIL_PICKAXE, 1, 7, "This is... surprisingly light? Used to break stones and mine for minerals.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.PICKAXE));
            AddToDictionary(ADAMANTITE_PICKAXE = new DamageDealingItem("Adamantite Pickaxe", Paths.ITEM_ADAMANTITE_PICKAXE, 1, 12, "This pickaxe is of perfect weight! Used to break stones and mine for minerals.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.PICKAXE));
            AddToDictionary(FISHING_ROD = new DamageDealingItem("Fishing Rod", Paths.ITEM_FISHING_ROD, 1, 3, "A flexible wooden fishing rod. It's used to fish wherever there's water.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.FISHING_ROD));
            AddToDictionary(IRON_ROD = new DamageDealingItem("Iron Rod", Paths.ITEM_IRON_ROD, 1, 5, "An inflexible iron fishing rod. It's used to fish wherever there's water.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.FISHING_ROD));
            AddToDictionary(MITHRIL_ROD = new DamageDealingItem("Mythril Rod", Paths.ITEM_MYTHRIL_ROD, 1, 7, "A flexible mithril fishing rod. It's used to fish wherever there's water. This is a Good Rod.", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.FISHING_ROD));
            AddToDictionary(ADAMANTITE_ROD = new DamageDealingItem("Adamantite Rod", Paths.ITEM_ADAMANTITE_ROD, 1, 12, "A peerless adamantite fishing rod. It's used to fish wherever there's water. This is a Super Rod!", 0, 0, Tag.TOOL, Tag.NO_TRASH, Tag.FISHING_ROD));

            AddToDictionary(MILKING_PAIL = new Item("Milking Pail", Paths.ITEM_MILKING_PAIL, 1, "An iron bucket used to milk cows. Cows can be milked once a day.", 520));
            AddToDictionary(SHEARS = new Item("Shears", Paths.ITEM_SHEARS, 1, "A pair of shears used to trim wool from sheep.", 270));
            AddToDictionary(BASKET = new Item("Basket", Paths.ITEM_BASKET, 1, "A handcrafted basket used to collect eggs. Use it on a chicken to gather eggs once a day.", 200));

            //add certs
            AddToDictionary(SHIPMENT_CERT = new Item("Shipment Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of shipping 10,000 gold worth of product.", 0, Tag.NO_TRASH));
            AddToDictionary(BARON_CERT = new Item("Baron Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of shipping 1,000,000 gold worth of product.", 0, Tag.NO_TRASH));
            AddToDictionary(FESTIVAL_CERT = new Item("Festival Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of your first festival attendence.", 0, Tag.NO_TRASH));
            AddToDictionary(COMMUNITY_CERT = new Item("Community Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of your commitment to the Nimbus Town community.", 0, Tag.NO_TRASH));
            AddToDictionary(READER_CERT = new Item("Reader Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of collecting 10 different books.", 0, Tag.NO_TRASH));
            AddToDictionary(LIBRARY_CERT = new Item("Library Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of collecting 50 different books!", 0, Tag.NO_TRASH));
            AddToDictionary(FRIENDSHIP_CERT = new Item("Friendship Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of making your first new friend.", 0, Tag.NO_TRASH));
            AddToDictionary(LOVER_CERT = new Item("Lover Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of blossoming love.", 0, Tag.NO_TRASH));
            AddToDictionary(SPIRIT_CERT = new Item("Spirit Cert", Paths.ITEM_NONE, 1, "This certificate hereby given in commemoration of a long climb completed.", 0, Tag.NO_TRASH));

            AddToDictionary(APPLE_SAPLING = new PlantableItem("Apple Sapling", Paths.ITEM_APPLE_SAPLING, EntityType.APPLE_TREE, new Vector2(0, -8), DEFAULT_STACK_SIZE, "Johnny would be proud. After reaching maturity, this tree can be harvested daily during Autumn.", 350, Tag.SOIL_PLANT_ONLY));
            AddToDictionary(BANANA_PALM_SAPLING = new PlantableItem("Banana Palm Sapling", Paths.ITEM_BANANA_PALM_SAPLING, EntityType.BANANA_PALM, new Vector2(0, -7), DEFAULT_STACK_SIZE, "Going crazy? After reaching maturity, this tree can be harvested daily during Summer. Palm trees must be planted on sand.", 300, Tag.SAND_PLANT_ONLY));
            AddToDictionary(CHERRY_SAPLING = new PlantableItem("Cherry Sapling", Paths.ITEM_CHERRY_SAPLING, EntityType.CHERRY_TREE, new Vector2(0, -8), DEFAULT_STACK_SIZE, "How romantic! After reaching maturity, this tree can be harvested daily in Spring.", 175, Tag.SOIL_PLANT_ONLY));
            AddToDictionary(COCONUT_PALM_SAPLING = new PlantableItem("Coconut Palm Sapling", Paths.ITEM_COCONUT_PALM_SAPLING, EntityType.COCONUT_PALM, new Vector2(0, -7), DEFAULT_STACK_SIZE, "The cow of trees. After reaching maturity, this tree can be harvested daily in Summer. Palm trees must be planted on sand.", 125, Tag.SAND_PLANT_ONLY));
            AddToDictionary(LEMON_SAPLING = new PlantableItem("Lemon Sapling", Paths.ITEM_LEMON_SAPLING, EntityType.LEMON_TREE, new Vector2(0, -4), DEFAULT_STACK_SIZE, "Where's the lime? After reaching maturity, this tree can be harvested daily in Autumn.", 365, Tag.SOIL_PLANT_ONLY));
            AddToDictionary(OLIVE_SAPLING = new PlantableItem("Olive Sapling", Paths.ITEM_OLIVE_SAPLING, EntityType.OLIVE_TREE, new Vector2(0, -4), DEFAULT_STACK_SIZE, "Symbol of peace. After reaching maturity, this tree can be harvested daily in Spring.", 275, Tag.SOIL_PLANT_ONLY));
            AddToDictionary(ORANGE_SAPLING = new PlantableItem("Orange Sapling", Paths.ITEM_ORANGE_SAPLING, EntityType.ORANGE_TREE, new Vector2(0, -8), DEFAULT_STACK_SIZE, "Orange you glad? After reaching maturity, this tree can be harvested daily in Summer.", 320, Tag.SOIL_PLANT_ONLY));
            AddToDictionary(PINECONE = new PlantableItem("Pinecone", Paths.ITEM_PINECONE, EntityType.PINE_TREE, new Vector2(0, -8), DEFAULT_STACK_SIZE, "A prickly cone from the plateau's pine trees. Can be planted in any season, and grows slowly over time into a new pinetree.", 5, Tag.SOIL_PLANT_ONLY));
            AddToDictionary(BERRY_BUSH_PLANTER = new PlantableItem("Berry Bush Planter", Paths.ITEM_BERRY_BUSH_PLANTER, EntityType.BUSH, new Vector2(0, -1), DEFAULT_STACK_SIZE, "How berry interesting... Can be planted at anytime, and grows berries depending on the season.", 125));

            AddToDictionary(BEET = new EdibleItem("Beet", Paths.ITEM_BEET, DEFAULT_STACK_SIZE, "The vivid red juices are also suitable as a dye.", AppliedEffects.FORAGING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 95, Tag.VEGETABLE, Tag.CROP));
            AddToDictionary(BELLPEPPER = new EdibleItem("Bellpepper", Paths.ITEM_BELLPEPPER, DEFAULT_STACK_SIZE, "Named for its distinctive shape.", AppliedEffects.FORAGING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 80, Tag.VEGETABLE, Tag.CROP));
            AddToDictionary(BROCCOLI = new EdibleItem("Broccoli", Paths.ITEM_BROCCOLI, DEFAULT_STACK_SIZE, "Unpopular amoung children.", AppliedEffects.MINING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 90, Tag.VEGETABLE, Tag.CROP));
            AddToDictionary(CABBAGE = new EdibleItem("Cabbage", Paths.ITEM_CABBAGE, DEFAULT_STACK_SIZE, "Leafy greens.", AppliedEffects.MINING_II, AppliedEffects.LENGTH_SHORT, "Delicious!", 675, Tag.VEGETABLE, Tag.CROP));
            AddToDictionary(CACTUS = new Item("Cactus", Paths.ITEM_CACTUS, DEFAULT_STACK_SIZE, "Be careful handling this one!", 210, Tag.CROP));
            AddToDictionary(CARROT = new EdibleItem("Carrot", Paths.ITEM_CARROT, DEFAULT_STACK_SIZE, "Said to improve eyesight.", AppliedEffects.MINING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 160, Tag.VEGETABLE, Tag.CROP));
            AddToDictionary(COTTON = new Item("Cotton", Paths.ITEM_COTTON, DEFAULT_STACK_SIZE, "Very soft. This can be woven into cloth.", 150, Tag.CROP));
            AddToDictionary(CUCUMBER = new EdibleItem("Cucumber", Paths.ITEM_CUCUMBER, DEFAULT_STACK_SIZE, "Delicious cooked or raw.", AppliedEffects.FORAGING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 60, Tag.VEGETABLE, Tag.CROP));
            AddToDictionary(EGGPLANT = new EdibleItem("Eggplant", Paths.ITEM_EGGPLANT, DEFAULT_STACK_SIZE, "Putting the chickens out of business. The purple color is suitable as a dye.", AppliedEffects.GATHERING_CHICKEN, AppliedEffects.LENGTH_SHORT, "Delicious!", 145, Tag.CROP, Tag.VEGETABLE));
            AddToDictionary(FLAX = new Item("Flax", Paths.ITEM_FLAX, DEFAULT_STACK_SIZE, "This can be woven into linen cloth.", 190, Tag.CROP));
            AddToDictionary(ONION = new EdibleItem("Onion", Paths.ITEM_ONION, DEFAULT_STACK_SIZE, "Don't cry!", AppliedEffects.MINING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 80, Tag.CROP, Tag.VEGETABLE));
            AddToDictionary(POTATO = new EdibleItem("Potato", Paths.ITEM_POTATO, DEFAULT_STACK_SIZE, "A starchy staple.", AppliedEffects.CHOPPING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 60, Tag.CROP, Tag.VEGETABLE));
            AddToDictionary(PUMPKIN = new EdibleItem("Pumpkin", Paths.ITEM_PUMPKIN, DEFAULT_STACK_SIZE, "Fun to eat, funner to carve.", AppliedEffects.CHOPPING_II, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 875, Tag.CROP, Tag.FRUIT));
            AddToDictionary(SPINACH = new EdibleItem("Spinach", Paths.ITEM_SPINACH, DEFAULT_STACK_SIZE, "Tasty when cooked properly. Often cooked improperly.", AppliedEffects.FORAGING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 130, Tag.CROP, Tag.VEGETABLE));
            AddToDictionary(STRAWBERRY = new EdibleItem("Strawberry", Paths.ITEM_STRAWBERRY, DEFAULT_STACK_SIZE, "Gorgeous red berries.", AppliedEffects.SPEED_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 65, Tag.CROP, Tag.FRUIT));
            AddToDictionary(TOMATO = new EdibleItem("Tomato", Paths.ITEM_TOMATO, DEFAULT_STACK_SIZE, "Vegetable or fruit?", AppliedEffects.FORAGING_II, AppliedEffects.LENGTH_SHORT, "Delicious!", 140, Tag.CROP, Tag.FRUIT, Tag.VEGETABLE));
            AddToDictionary(WATERMELON_SLICE = new EdibleItem("Watermelon Slice", Paths.ITEM_WATERMELON, DEFAULT_STACK_SIZE, "A summer classic. Don't eat the black seeds!", AppliedEffects.BUG_CATCHING_II, AppliedEffects.LENGTH_SHORT, "Delicious!", 160, Tag.CROP, Tag.FRUIT));
            AddToDictionary(APPLE = new EdibleItem("Apple", Paths.ITEM_APPLE, DEFAULT_STACK_SIZE, "An apple a day keeps your health in check.", AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LENGTH_SHORT, "Delicious!", 250, Tag.FRUIT));
            AddToDictionary(BANANA = new EdibleItem("Banana", Paths.ITEM_BANANA, DEFAULT_STACK_SIZE, "Potassium power!", AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LENGTH_SHORT, "Delicious!", 220, Tag.FRUIT));
            AddToDictionary(CHERRY = new EdibleItem("Cherry", Paths.ITEM_CHERRY, DEFAULT_STACK_SIZE, "Cheerful springtime fruit.", AppliedEffects.SPEED_I_SPRING, AppliedEffects.LENGTH_SHORT, "Delicious!", 120, Tag.FRUIT));
            AddToDictionary(COCONUT = new EdibleItem("Coconut", Paths.ITEM_COCONUT, DEFAULT_STACK_SIZE, "Milk of the tropics.", AppliedEffects.FISHING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 85, Tag.FRUIT));
            AddToDictionary(LEMON = new EdibleItem("Lemon", Paths.ITEM_LEMON, DEFAULT_STACK_SIZE, "The more sour and acidic, the better.", AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LENGTH_SHORT, "Delicious!", 320, Tag.FRUIT));
            AddToDictionary(OLIVE = new EdibleItem("Olive", Paths.ITEM_OLIVE, DEFAULT_STACK_SIZE, "Revered as sacred in an ancient society.", AppliedEffects.CHOPPING_I, AppliedEffects.LENGTH_SHORT, "Delicious!", 195, Tag.FRUIT));
            AddToDictionary(ORANGE = new EdibleItem("Orange", Paths.ITEM_ORANGE, DEFAULT_STACK_SIZE, "Whoever named this wasn't particularly creative.", AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LENGTH_SHORT, "Delicious!", 165, Tag.FRUIT));

            AddToDictionary(SILVER_BEET = new EdibleItem("Silver Beet", Paths.ITEM_SILVER_BEET, DEFAULT_STACK_SIZE, "The vivid red juices are also suitable as a dye. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BEET.GetValue() * SILVER_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_BELLPEPPER = new EdibleItem("Silver Bellpepper", Paths.ITEM_SILVER_BELLPEPPER, DEFAULT_STACK_SIZE, "Named for its distinctive shape. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BELLPEPPER.GetValue() * SILVER_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_BROCCOLI = new EdibleItem("Silver Broccoli", Paths.ITEM_SILVER_BROCCOLI, DEFAULT_STACK_SIZE, "Unpopular amoung children. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.MINING_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BROCCOLI.GetValue() * SILVER_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_CABBAGE = new EdibleItem("Silver Cabbage", Paths.ITEM_SILVER_CABBAGE, DEFAULT_STACK_SIZE, "Leafy greens. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.MINING_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CABBAGE.GetValue() * SILVER_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_CACTUS = new Item("Silver Cactus", Paths.ITEM_SILVER_CACTUS, DEFAULT_STACK_SIZE, "Be careful handling this one! Silver crops have higher value than normal crops!", (int)(CACTUS.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_CARROT = new EdibleItem("Silver Carrot", Paths.ITEM_SILVER_CARROT, DEFAULT_STACK_SIZE, "Said to improve eyesight. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.MINING_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CARROT.GetValue() * SILVER_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_COTTON = new Item("Silver Cotton", Paths.ITEM_SILVER_COTTON, DEFAULT_STACK_SIZE, "Very soft. This can be woven into cloth. Silver crops have higher value than normal crops!", (int)(COTTON.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_CUCUMBER = new EdibleItem("Silver Cucumber", Paths.ITEM_SILVER_CUCUMBER, DEFAULT_STACK_SIZE, "Delicious cooked or raw. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CUCUMBER.GetValue() * SILVER_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_EGGPLANT = new EdibleItem("Silver Eggplant", Paths.ITEM_SILVER_EGGPLANT, DEFAULT_STACK_SIZE, "Looks nothing like an egg. The purple color is suitable as a dye. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.GATHERING_CHICKEN }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(EGGPLANT.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.SILVER_CROP));
            AddToDictionary(SILVER_FLAX = new Item("Silver Flax", Paths.ITEM_SILVER_FLAX, DEFAULT_STACK_SIZE, "This can be woven into linen. Silver crops have higher value than normal crops!", (int)(FLAX.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.SILVER_CROP));
            AddToDictionary(SILVER_ONION = new EdibleItem("Silver Onion", Paths.ITEM_SILVER_ONION, DEFAULT_STACK_SIZE, "Don't cry! Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.MINING_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(ONION.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.SILVER_CROP));
            AddToDictionary(SILVER_POTATO = new EdibleItem("Silver Potato", Paths.ITEM_SILVER_POTATO, DEFAULT_STACK_SIZE, "A starchy staple! Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(POTATO.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.SILVER_CROP));
            AddToDictionary(SILVER_PUMPKIN = new EdibleItem("Silver Pumpkin", Paths.ITEM_SILVER_PUMPKIN, DEFAULT_STACK_SIZE, "Fun to eat, funner to carve. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_II, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", (int)(PUMPKIN.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_SPINACH = new EdibleItem("Silver Spinach", Paths.ITEM_SILVER_SPINACH, DEFAULT_STACK_SIZE, "Tasty when cooked properly. Often cooked improperly. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(SPINACH.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.SILVER_CROP));
            AddToDictionary(SILVER_STRAWBERRY = new EdibleItem("Silver Strawberry", Paths.ITEM_SILVER_STRAWBERRY, DEFAULT_STACK_SIZE, "Every kid's summer favorite. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(STRAWBERRY.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_TOMATO = new EdibleItem("Silver Tomato", Paths.ITEM_SILVER_TOMATO, DEFAULT_STACK_SIZE, "Vegetable or fruit? Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_II, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(TOMATO.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.VEGETABLE, Tag.SILVER_CROP));
            AddToDictionary(SILVER_WATERMELON_SLICE = new EdibleItem("Silver Watermelon Slice", Paths.ITEM_SILVER_WATERMELON, DEFAULT_STACK_SIZE, "A summer classic. Don't eat the black seeds! Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_II, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(WATERMELON_SLICE.GetValue() * SILVER_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_APPLE = new EdibleItem("Silver Apple", Paths.ITEM_SILVER_APPLE, DEFAULT_STACK_SIZE, "An apple a day keeps your health in check. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(APPLE.GetValue() * SILVER_MULTIPLIER), Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_BANANA = new EdibleItem("Silver Banana", Paths.ITEM_SILVER_BANANA, DEFAULT_STACK_SIZE, "Potassium power! Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BANANA.GetValue() * SILVER_MULTIPLIER), Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_CHERRY = new EdibleItem("Silver Cherry", Paths.ITEM_SILVER_CHERRY, DEFAULT_STACK_SIZE, "Cheerful springtime fruit. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SPRING, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CHERRY.GetValue() * SILVER_MULTIPLIER), Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_COCONUT = new EdibleItem("Silver Coconut", Paths.ITEM_SILVER_COCONUT, DEFAULT_STACK_SIZE, "Milk of the tropics. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.FISHING_I, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(COCONUT.GetValue() * SILVER_MULTIPLIER), Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_LEMON = new EdibleItem("Silver Lemon", Paths.ITEM_SILVER_LEMON, DEFAULT_STACK_SIZE, "The more sour and acidic, the better. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(LEMON.GetValue() * SILVER_MULTIPLIER), Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_OLIVE = new EdibleItem("Silver Olive", Paths.ITEM_SILVER_OLIVE, DEFAULT_STACK_SIZE, "Revered as sacred in an ancient society. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_I, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(OLIVE.GetValue() * SILVER_MULTIPLIER), Tag.FRUIT, Tag.SILVER_CROP));
            AddToDictionary(SILVER_ORANGE = new EdibleItem("Silver Orange", Paths.ITEM_SILVER_ORANGE, DEFAULT_STACK_SIZE, "Whoever named this wasn't particularly creative. Silver crops have higher value than normal crops!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LUCK_I }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(ORANGE.GetValue() * SILVER_MULTIPLIER), Tag.FRUIT, Tag.SILVER_CROP));

            AddToDictionary(GOLDEN_BEET = new EdibleItem("Golden Beet", Paths.ITEM_GOLDEN_BEET, DEFAULT_STACK_SIZE, "The vivid red juices are also suitable as a dye. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BEET.GetValue() * GOLD_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_BELLPEPPER = new EdibleItem("Golden Bellpepper", Paths.ITEM_GOLDEN_BELLPEPPER, DEFAULT_STACK_SIZE, "Named for its distinctive shape. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BELLPEPPER.GetValue() * GOLD_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_BROCCOLI = new EdibleItem("Golden Broccoli", Paths.ITEM_GOLDEN_BROCCOLI, DEFAULT_STACK_SIZE, "Unpopular amoung children. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.MINING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BROCCOLI.GetValue() * GOLD_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_CABBAGE = new EdibleItem("Golden Cabbage", Paths.ITEM_GOLDEN_CABBAGE, DEFAULT_STACK_SIZE, "Leafy greens. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.MINING_II, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CABBAGE.GetValue() * GOLD_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_CACTUS = new Item("Golden Cactus", Paths.ITEM_GOLDEN_CACTUS, DEFAULT_STACK_SIZE, "Be careful handling this one! Golden crops are crops of the highest quality!", (int)(CACTUS.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_CARROT = new EdibleItem("Golden Carrot", Paths.ITEM_GOLDEN_CARROT, DEFAULT_STACK_SIZE, "Said to improve eyesight. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.MINING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CARROT.GetValue() * GOLD_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_COTTON = new Item("Golden Cotton", Paths.ITEM_GOLDEN_COTTON, DEFAULT_STACK_SIZE, "Very soft. This can be woven into cloth. Golden crops are crops of the highest quality!", (int)(COTTON.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_CUCUMBER = new EdibleItem("Golden Cucumber", Paths.ITEM_GOLDEN_CUCUMBER, DEFAULT_STACK_SIZE, "Delicious cooked or raw. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CUCUMBER.GetValue() * GOLD_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_EGGPLANT = new EdibleItem("Golden Eggplant", Paths.ITEM_GOLDEN_EGGPLANT, DEFAULT_STACK_SIZE, "Looks nothing like an egg. The purple color is suitable as a dye. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.GATHERING_CHICKEN, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(EGGPLANT.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_FLAX = new Item("Golden Flax", Paths.ITEM_GOLDEN_FLAX, DEFAULT_STACK_SIZE, "This can be woven into linen. Golden crops are crops of the highest quality!", (int)(FLAX.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_ONION = new EdibleItem("Golden Onion", Paths.ITEM_GOLDEN_ONION, DEFAULT_STACK_SIZE, "Don't cry! Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.MINING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(ONION.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_POTATO = new EdibleItem("Golden Potato", Paths.ITEM_GOLDEN_POTATO, DEFAULT_STACK_SIZE, "A starchy staple! Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(POTATO.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_PUMPKIN = new EdibleItem("Golden Pumpkin", Paths.ITEM_GOLDEN_PUMPKIN, DEFAULT_STACK_SIZE, "Fun to eat, funner to carve. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_II, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", (int)(PUMPKIN.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_SPINACH = new EdibleItem("Golden Spinach", Paths.ITEM_GOLDEN_SPINACH, DEFAULT_STACK_SIZE, "Tasty when cooked properly. Often cooked improperly. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(SPINACH.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_STRAWBERRY = new EdibleItem("Golden Strawberry", Paths.ITEM_GOLDEN_STRAWBERRY, DEFAULT_STACK_SIZE, "Every kid's summer favorite. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(STRAWBERRY.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_TOMATO = new EdibleItem("Golden Tomato", Paths.ITEM_GOLDEN_TOMATO, DEFAULT_STACK_SIZE, "Vegetable or fruit? Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_II, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(TOMATO.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.VEGETABLE, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_WATERMELON_SLICE = new EdibleItem("Golden Watermelon Slice", Paths.ITEM_GOLDEN_WATERMELON, DEFAULT_STACK_SIZE, "A summer classic. Don't eat the black seeds! Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_II, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(WATERMELON_SLICE.GetValue() * GOLD_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_APPLE = new EdibleItem("Golden Apple", Paths.ITEM_GOLDEN_APPLE, DEFAULT_STACK_SIZE, "An apple a day keeps your health in check. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(APPLE.GetValue() * GOLD_MULTIPLIER), Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_BANANA = new EdibleItem("Golden Banana", Paths.ITEM_GOLDEN_BANANA, DEFAULT_STACK_SIZE, "Potassium power! Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BANANA.GetValue() * GOLD_MULTIPLIER), Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_CHERRY = new EdibleItem("Golden Cherry", Paths.ITEM_GOLDEN_CHERRY, DEFAULT_STACK_SIZE, "Cheerful springtime fruit. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SPRING, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CHERRY.GetValue() * GOLD_MULTIPLIER), Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_COCONUT = new EdibleItem("Golden Coconut", Paths.ITEM_GOLDEN_COCONUT, DEFAULT_STACK_SIZE, "Milk of the tropics. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.FISHING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(COCONUT.GetValue() * GOLD_MULTIPLIER), Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_LEMON = new EdibleItem("Golden Lemon", Paths.ITEM_GOLDEN_LEMON, DEFAULT_STACK_SIZE, "The more sour and acidic, the better. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(LEMON.GetValue() * GOLD_MULTIPLIER), Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_OLIVE = new EdibleItem("Golden Olive", Paths.ITEM_GOLDEN_OLIVE, DEFAULT_STACK_SIZE, "Revered as sacred in an ancient society. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_I, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(OLIVE.GetValue() * GOLD_MULTIPLIER), Tag.FRUIT, Tag.GOLDEN_CROP));
            AddToDictionary(GOLDEN_ORANGE = new EdibleItem("Golden Orange", Paths.ITEM_GOLDEN_ORANGE, DEFAULT_STACK_SIZE, "Whoever named this wasn't particularly creative. Golden crops are crops of the highest quality!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LUCK_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(ORANGE.GetValue() * GOLD_MULTIPLIER), Tag.FRUIT, Tag.GOLDEN_CROP));

            AddToDictionary(PHANTOM_BEET = new EdibleItem("Phantom Beet", Paths.ITEM_PHANTOM_BEET, DEFAULT_STACK_SIZE, "The vivid sinister juices are also suitable as a dye. ", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BEET.GetValue() * PHANTOM_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_BELLPEPPER = new EdibleItem("Phantom Bellpepper", Paths.ITEM_PHANTOM_BELLPEPPER, DEFAULT_STACK_SIZE, "Sinister for its distinctive shape.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BELLPEPPER.GetValue() * PHANTOM_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_BROCCOLI = new EdibleItem("Phantom Broccoli", Paths.ITEM_PHANTOM_BROCCOLI, DEFAULT_STACK_SIZE, "Sinister amoung children.", new AppliedEffects.Effect[] { AppliedEffects.MINING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BROCCOLI.GetValue() * PHANTOM_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_CABBAGE = new EdibleItem("Phantom Cabbage", Paths.ITEM_PHANTOM_CABBAGE, DEFAULT_STACK_SIZE, "Leafy sinister.", new AppliedEffects.Effect[] { AppliedEffects.MINING_II, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CABBAGE.GetValue() * PHANTOM_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_CACTUS = new Item("Phantom Cactus", Paths.ITEM_PHANTOM_CACTUS, DEFAULT_STACK_SIZE, "Be sinister handling this one!", (int)(CACTUS.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_CARROT = new EdibleItem("Phantom Carrot", Paths.ITEM_PHANTOM_CARROT, DEFAULT_STACK_SIZE, "Said to sinister eyesight.", new AppliedEffects.Effect[] { AppliedEffects.MINING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CARROT.GetValue() * PHANTOM_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_COTTON = new Item("Phantom Cotton", Paths.ITEM_PHANTOM_COTTON, DEFAULT_STACK_SIZE, "Very sinister. This can be woven into cloth. ", (int)(COTTON.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_CUCUMBER = new EdibleItem("Phantom Cucumber", Paths.ITEM_PHANTOM_CUCUMBER, DEFAULT_STACK_SIZE, "Sinister cooked or raw.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CUCUMBER.GetValue() * PHANTOM_MULTIPLIER), Tag.VEGETABLE, Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_EGGPLANT = new EdibleItem("Phantom Eggplant", Paths.ITEM_PHANTOM_EGGPLANT, DEFAULT_STACK_SIZE, "Looks sinister like an egg. The purple color is suitable as a dye.", new AppliedEffects.Effect[] { AppliedEffects.GATHERING_CHICKEN, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(EGGPLANT.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_FLAX = new Item("Phantom Flax", Paths.ITEM_PHANTOM_FLAX, DEFAULT_STACK_SIZE, "This can be woven into sinister.", (int)(FLAX.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_ONION = new EdibleItem("Phantom Onion", Paths.ITEM_PHANTOM_ONION, DEFAULT_STACK_SIZE, "Sinister cry!", new AppliedEffects.Effect[] { AppliedEffects.MINING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(ONION.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_POTATO = new EdibleItem("Phantom Potato", Paths.ITEM_PHANTOM_POTATO, DEFAULT_STACK_SIZE, "A sinister staple!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(POTATO.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_PUMPKIN = new EdibleItem("Phantom Pumpkin", Paths.ITEM_PHANTOM_PUMPKIN, DEFAULT_STACK_SIZE, "Fun to eat, sinister to carve.", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_II, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", (int)(PUMPKIN.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_SPINACH = new EdibleItem("Phantom Spinach", Paths.ITEM_PHANTOM_SPINACH, DEFAULT_STACK_SIZE, "Sinister when cooked properly. Often cooked improperly.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(SPINACH.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_STRAWBERRY = new EdibleItem("Phantom Strawberry", Paths.ITEM_PHANTOM_STRAWBERRY, DEFAULT_STACK_SIZE, "Every kid's sinister favorite.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(STRAWBERRY.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.VEGETABLE, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_TOMATO = new EdibleItem("Phantom Tomato", Paths.ITEM_PHANTOM_TOMATO, DEFAULT_STACK_SIZE, "Vegetable or sinister?", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_II, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(TOMATO.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.VEGETABLE, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_WATERMELON_SLICE = new EdibleItem("Phantom Watermelon Slice", Paths.ITEM_PHANTOM_WATERMELON, DEFAULT_STACK_SIZE, "A summer classic. Don't eat the sinister seeds!", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_II, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(WATERMELON_SLICE.GetValue() * PHANTOM_MULTIPLIER), Tag.CROP, Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_APPLE = new EdibleItem("Phantom Apple", Paths.ITEM_PHANTOM_APPLE, DEFAULT_STACK_SIZE, "An apple a day keeps your sinister in check.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(APPLE.GetValue() * PHANTOM_MULTIPLIER), Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_BANANA = new EdibleItem("Phantom Banana", Paths.ITEM_PHANTOM_BANANA, DEFAULT_STACK_SIZE, "Sinister power!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(BANANA.GetValue() * PHANTOM_MULTIPLIER), Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_CHERRY = new EdibleItem("Phantom Cherry", Paths.ITEM_PHANTOM_CHERRY, DEFAULT_STACK_SIZE, "Sinister springtime fruit.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SPRING, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(CHERRY.GetValue() * PHANTOM_MULTIPLIER), Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_COCONUT = new EdibleItem("Phantom Coconut", Paths.ITEM_PHANTOM_COCONUT, DEFAULT_STACK_SIZE, "Milk of the sinister.", new AppliedEffects.Effect[] { AppliedEffects.FISHING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(COCONUT.GetValue() * PHANTOM_MULTIPLIER), Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_LEMON = new EdibleItem("Phantom Lemon", Paths.ITEM_PHANTOM_LEMON, DEFAULT_STACK_SIZE, "The more sour and sinister, the better.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(LEMON.GetValue() * PHANTOM_MULTIPLIER), Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_OLIVE = new EdibleItem("Phantom Olive", Paths.ITEM_PHANTOM_OLIVE, DEFAULT_STACK_SIZE, "Revered as sacred in a sinister society.", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_I, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(OLIVE.GetValue() * PHANTOM_MULTIPLIER), Tag.FRUIT, Tag.PHANTOM_CROP));
            AddToDictionary(PHANTOM_ORANGE = new EdibleItem("Phantom Orange", Paths.ITEM_PHANTOM_ORANGE, DEFAULT_STACK_SIZE, "Whoever named this wasn't particularly sinister.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LUCK_III }, AppliedEffects.LENGTH_SHORT, "Delicious!", (int)(ORANGE.GetValue() * PHANTOM_MULTIPLIER), Tag.FRUIT, Tag.PHANTOM_CROP));
            
            AddToDictionary(BEET_SEEDS = new Item("Beet Seeds", Paths.ITEM_BEET_SEEDS, DEFAULT_STACK_SIZE, "For laying down funky beets. Planted in Autumn, this crop requires 2 days to grow.", 90, Tag.SEED));
            AddToDictionary(BELLPEPPER_SEEDS = new Item("Bellpepper Seeds", Paths.ITEM_BELLPEPPER_SEEDS, DEFAULT_STACK_SIZE, "Ringing in the cool weather. Planted in Autumn, this crop requires 3 days to grow and can be harvested repeatedly.", 120, Tag.SEED));
            AddToDictionary(BROCCOLI_SEEDS = new Item("Broccoli Seeds", Paths.ITEM_BROCCOLI_SEEDS, DEFAULT_STACK_SIZE, "Sow the seeds of good health. Planted in Autumn, this crop requires 3 days to grow.", 50, Tag.SEED));
            AddToDictionary(CABBAGE_SEEDS = new Item("Cabbage Seeds", Paths.ITEM_CABBAGE_SEEDS, DEFAULT_STACK_SIZE, "Best placed in a patch. Planted in Autumn, this crop requires 6 days to grow. Make sure to plant it early in the season!", 180, Tag.SEED));
            AddToDictionary(CACTUS_SEEDS = new Item("Cactus Seeds", Paths.ITEM_CACTUS_SEEDS, DEFAULT_STACK_SIZE, "Even the seeds are prickly! Planted in Summer, this succulent requires 3 days to grow.", 85, Tag.SEED));
            AddToDictionary(CARROT_SEEDS = new Item("Carrot Seeds", Paths.ITEM_CARROT_SEEDS, DEFAULT_STACK_SIZE, "For rabbit-friendly farmers only. Planted in Spring, this crop requires 5 days to grow.", 70, Tag.SEED));
            AddToDictionary(COTTON_SEEDS = new Item("Cotton Seeds", Paths.ITEM_COTTON_SEEDS, DEFAULT_STACK_SIZE, "Sheep of the soil. Planted in Summer, this crop requires 5 days to grow.", 120, Tag.SEED));
            AddToDictionary(CUCUMBER_SEEDS = new Item("Cucumber Seeds", Paths.ITEM_CUCUMBER_SEEDS, DEFAULT_STACK_SIZE, "Looking cool, Farmer! Planted in Summer, this crop requries 2 days to grow and can be harvested repeatedly.", 140, Tag.SEED));
            AddToDictionary(EGGPLANT_SEEDS = new Item("Eggplant Seeds", Paths.ITEM_EGGPLANT_SEEDS, DEFAULT_STACK_SIZE, "Hens don't want you to learn his secret! Planted in Summer, this crop requires 4 days to grow an can be harvested repeatedly.", 80, Tag.SEED));
            AddToDictionary(FLAX_SEEDS = new Item("Flax Seeds", Paths.ITEM_FLAX_SEEDS, DEFAULT_STACK_SIZE, "Becomes a tropical textile. Planted in Autumn, this crop requires 5 days to grow.", 100, Tag.SEED));
            AddToDictionary(ONION_SEEDS = new Item("Onion Seeds", Paths.ITEM_ONION_SEEDS, DEFAULT_STACK_SIZE, "Sowing these makes you a bit sad. Planted in Summer, this crop requires 2 days to grow.", 40, Tag.SEED));
            AddToDictionary(POTATO_SEEDS = new Item("Potato Seeds", Paths.ITEM_POTATO_SEEDS, DEFAULT_STACK_SIZE, "A farming staple! Planted in Spring, this crop requires 3 days to grow.", 60, Tag.SEED));
            AddToDictionary(PUMPKIN_SEEDS = new Item("Pumpkin Seeds", Paths.ITEM_PUMPKIN_SEEDS, DEFAULT_STACK_SIZE, "Spooky... Planted in Autumn, this crop requires 7 days to grow. Make sure to plant it early in the season!", 220, Tag.SEED));
            AddToDictionary(SPINACH_SEEDS = new Item("Spinach Seeds", Paths.ITEM_SPINACH_SEEDS, DEFAULT_STACK_SIZE, "A humble beginning. Planted in Spring, this crop requires 2 days to grow. We all start somewhere.", 40, Tag.SEED));
            AddToDictionary(STRAWBERRY_SEEDS = new Item("Strawberry Seeds", Paths.ITEM_STRAWBERRY_SEEDS, DEFAULT_STACK_SIZE, "Berry bushes incoming! Planted in Spring, this crop requires 4 days to grow and can be harvested repeatedly.", 120, Tag.SEED));
            AddToDictionary(TOMATO_SEEDS = new Item("Tomato Seeds", Paths.ITEM_TOMATO_SEEDS, DEFAULT_STACK_SIZE, "Archetypical garden resident. Planted in Summer, this crop requires 5 days to grow and can be harvested repeatedly.", 100, Tag.SEED));
            AddToDictionary(WATERMELON_SEEDS = new Item("Watermelon Seeds", Paths.ITEM_WATERMELON_SEEDS, DEFAULT_STACK_SIZE, "It's a long wait, but well worth it! Planted in Summer, this crop requires 6 days to grow. Make sure to plant it early in the season!", 220, Tag.SEED));
            AddToDictionary(SPORES = new Item("Spores", Paths.ITEM_SPORES, DEFAULT_STACK_SIZE, "Spores of some kind of mushroom. It requires a specialized container to grow properly.", 25, Tag.SEED));

            AddToDictionary(SHINING_BEET_SEEDS = new Item("Shining Beet Seeds", Paths.ITEM_SHINING_BEET_SEEDS, DEFAULT_STACK_SIZE, "For laying fabulous funky beets. Planted in Autumn, this crop requires 2 days to grow. Shining seeds are higher quality!", (int)(BEET_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_BELLPEPPER_SEEDS = new Item("Shining Bellpepper Seeds", Paths.ITEM_SHINING_BELLPEPPER_SEEDS, DEFAULT_STACK_SIZE, "Ringing in the fabulous weather. Planted in Autumn, this crop requires 3 days to grow and can be harvested repeatedly. Shining seeds are higher quality!", (int)(BELLPEPPER_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_BROCCOLI_SEEDS = new Item("Shining Broccoli Seeds", Paths.ITEM_SHINING_BROCCOLI_SEEDS, DEFAULT_STACK_SIZE, "Sow the seeds of fabulous health. Planted in Autumn, this crop requires 3 days to grow. Shining seeds are higher quality!", (int)(BROCCOLI_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_CABBAGE_SEEDS = new Item("Shining Cabbage Seeds", Paths.ITEM_SHINING_CABBAGE_SEEDS, DEFAULT_STACK_SIZE, "Fabulous placed in a patch. Planted in Autumn, this crop requires 6 days to grow. Shining seeds are higher quality!", (int)(CABBAGE_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_CACTUS_SEEDS = new Item("Shining Cactus Seeds", Paths.ITEM_SHINING_CACTUS_SEEDS, DEFAULT_STACK_SIZE, "Even the seeds are fabuulous! Planted in Summer, this succulent requires 3 days to grow. Shining seeds are higher quality!", (int)(CACTUS_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_CARROT_SEEDS = new Item("Shining Carrot Seeds", Paths.ITEM_SHINING_CARROT_SEEDS, DEFAULT_STACK_SIZE, "For rabbit-fabulous farmers only. Planted in Spring, this crop requires 5 days to grow. Shining seeds are higher quality!", (int)(CARROT_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_COTTON_SEEDS = new Item("Shining Cotton Seeds", Paths.ITEM_SHINING_COTTON_SEEDS, DEFAULT_STACK_SIZE, "Sheep of fabulous soil. Planted in Summer, this crop requires 5 days to grow. Shining seeds are higher quality!", (int)(COTTON_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_CUCUMBER_SEEDS = new Item("Shining Cucumber Seeds", Paths.ITEM_SHINING_CUCUMBER_SEEDS, DEFAULT_STACK_SIZE, "Looking fabulous, Farmer! Planted in Summer, this crop requries 2 days to grow and can be harvested repeatedly. Shining seeds are higher quality!", (int)(CUCUMBER_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_EGGPLANT_SEEDS = new Item("Shining Eggplant Seeds", Paths.ITEM_SHINING_EGGPLANT_SEEDS, DEFAULT_STACK_SIZE, "Hens don't want you to learn his fabulous! Planted in Summer, this crop requires 4 days to grow an can be harvested repeatedly. Shining seeds are higher quality!", (int)(EGGPLANT_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_FLAX_SEEDS = new Item("Shining Flax Seeds", Paths.ITEM_SHINING_FLAX_SEEDS, DEFAULT_STACK_SIZE, "Becomes a fabulous textile. Planted in Autumn, this crop requires 5 days to grow. Shining seeds are higher quality!", (int)(FLAX_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_ONION_SEEDS = new Item("Shining Onion Seeds", Paths.ITEM_SHINING_ONION_SEEDS, DEFAULT_STACK_SIZE, "Sowing these makes you a bit fabulous. Planted in Summer, this crop requires 2 days to grow. Shining seeds are higher quality!", (int)(ONION_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_POTATO_SEEDS = new Item("Shining Potato Seeds", Paths.ITEM_SHINING_POTATO_SEEDS, DEFAULT_STACK_SIZE, "A fabulous staple! Planted in Spring, this crop requires 3 days to grow. Shining seeds are higher quality!", (int)(POTATO_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_PUMPKIN_SEEDS = new Item("Shining Pumpkin Seeds", Paths.ITEM_SHINING_PUMPKIN_SEEDS, DEFAULT_STACK_SIZE, "Fabulous... Planted in Autumn, this crop requires 7 days to grow. Shining seeds are higher quality!", (int)(PUMPKIN_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_SPINACH_SEEDS = new Item("Shining Spinach Seeds", Paths.ITEM_SHINING_SPINACH_SEEDS, DEFAULT_STACK_SIZE, "A fabulous beginning. Planted in Spring, this crop requires 2 days to grow. Shining seeds are higher quality!", (int)(SPINACH_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_STRAWBERRY_SEEDS = new Item("Shining Strawberry Seeds", Paths.ITEM_SHINING_STRAWBERRY_SEEDS, DEFAULT_STACK_SIZE, "Fabulous bushes incoming! Planted in Spring, this crop requires 4 days to grow and can be harvested repeatedly. Shining seeds are higher quality!", (int)(STRAWBERRY_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_TOMATO_SEEDS = new Item("Shining Tomato Seeds", Paths.ITEM_SHINING_TOMATO_SEEDS, DEFAULT_STACK_SIZE, "Archetypical garden fabulous. Planted in Summer, this crop requires 5 days to grow and can be harvested repeatedly. Shining seeds are higher quality!", (int)(TOMATO_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));
            AddToDictionary(SHINING_WATERMELON_SEEDS = new Item("Shining Watermelon Seeds", Paths.ITEM_SHINING_WATERMELON_SEEDS, DEFAULT_STACK_SIZE, "It's a fabulous wait, but well worth it! Planted in Summer, this crop requires 6 days to grow. Shining seeds are higher quality!", (int)(WATERMELON_SEEDS.GetValue() * GOLD_MULTIPLIER), Tag.SEED, Tag.SHINING_SEED));

            AddToDictionary(BOARD = new Item("Board", Paths.ITEM_BOARD, DEFAULT_STACK_SIZE, "Basic slice of wood. Used for crafting lots of wooden stuff.", 25, Tag.MATERIAL));
            AddToDictionary(PLANK = new Item("Plank", Paths.ITEM_PLANK, DEFAULT_STACK_SIZE, "Thick cut of hardwood. Used for crafting lots of fancy stuff.", 100, Tag.MATERIAL));
            AddToDictionary(BRICKS = new Item("Bricks", Paths.ITEM_BRICKS, DEFAULT_STACK_SIZE, "Set of cubical blocks. Used for crafting lots of stuff.", 35, Tag.MATERIAL));
            AddToDictionary(GEARS = new Item("Gears", Paths.ITEM_GEARS, DEFAULT_STACK_SIZE, "Handcrafted metal gears. Used for crafting machine stuff.", 250, Tag.MATERIAL));
            AddToDictionary(PAPER = new Item("Paper", Paths.ITEM_PAPER, DEFAULT_STACK_SIZE, "Sheet of bamboo paper. Used for crafting wallpaper stuff.", 45, Tag.MATERIAL));

            AddToDictionary(WILD_HONEY = new EdibleItem("Wild Honey", Paths.ITEM_WILD_HONEY, DEFAULT_STACK_SIZE, "Sweet and addictive, plus it's healthy to boot.", AppliedEffects.BUG_CATCHING_I, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 60, Tag.BEE_PRODUCT));
            AddToDictionary(BEESWAX = new Item("Beeswax", Paths.ITEM_BEESWAX, DEFAULT_STACK_SIZE, "Beeswax on, beeswax off.", 30, Tag.BEE_PRODUCT));
            AddToDictionary(QUEENS_STINGER = new Item("Queen's Stinger", Paths.ITEM_QUEENS_STINGER, DEFAULT_STACK_SIZE, "Royal stinger of the hive's queen. Revered amoung bee enthusiasts.", 280, Tag.BEE_PRODUCT));
            AddToDictionary(ROYAL_JELLY = new Item("Royal Jelly", Paths.ITEM_ROYAL_JELLY, DEFAULT_STACK_SIZE, "Nobel and purple. It can be crushed into a dye.", 100, Tag.BEE_PRODUCT));
            AddToDictionary(POLLEN_PUFF = new Item("Pollen Puff", Paths.ITEM_POLLEN_PUFF, DEFAULT_STACK_SIZE, "Pollen power!", 80, Tag.BEE_PRODUCT));

            AddToDictionary(BUTTER = new Item("Butter", Paths.ITEM_BUTTER, DEFAULT_STACK_SIZE, "Fatty goodness!", 160, Tag.FOOD, Tag.ANIMAL_PRODUCT));
            AddToDictionary(CREAM = new UsableItem("Cream", Paths.ITEM_CREAM, DEFAULT_STACK_SIZE, "Drink", UsableItem.MILK_CREAM_DIALOGUE, "A dish cooked with cream just screams \"fancy\". Drinking it heals all effects.", 135, Tag.FOOD, Tag.ANIMAL_PRODUCT));
            AddToDictionary(CHEESE = new EdibleItem("Cheese", Paths.ITEM_CHEESE, DEFAULT_STACK_SIZE, "This description is kinda cheesy.", AppliedEffects.GATHERING_COW, AppliedEffects.LENGTH_SHORT, "Delicious!", 190, Tag.FOOD, Tag.ANIMAL_PRODUCT));
            AddToDictionary(EGG = new Item("Egg", Paths.ITEM_EGG, DEFAULT_STACK_SIZE, "For some reason, it\'s satisfyingly round.", 60, Tag.FOOD, Tag.ANIMAL_PRODUCT));
            AddToDictionary(MAYONNAISE = new EdibleItem("Mayonnaise", Paths.ITEM_MAYONNAISE, DEFAULT_STACK_SIZE, "Thick and creamy.", AppliedEffects.GATHERING_CHICKEN, AppliedEffects.LENGTH_SHORT, "Delicious!", 120, Tag.FOOD, Tag.ANIMAL_PRODUCT));
            AddToDictionary(MILK = new UsableItem("Milk", Paths.ITEM_MILK, DEFAULT_STACK_SIZE, "Drink", UsableItem.MILK_CREAM_DIALOGUE, "Fresh milk! It has calcium for strong bones! Drinking it clears all effects.", 85, Tag.FOOD, Tag.ANIMAL_PRODUCT));
            AddToDictionary(GOLDEN_EGG = new Item("Golden Egg", Paths.ITEM_GOLDEN_EGG, DEFAULT_STACK_SIZE, "No beanstalks necessary.", 600, Tag.RARE, Tag.FOOD, Tag.ANIMAL_PRODUCT));
            AddToDictionary(GOLDEN_WOOL = new Item("Golden Wool", Paths.ITEM_GOLDEN_WOOL, DEFAULT_STACK_SIZE, "The Argonauts would've killed for this!", 750, Tag.RARE, Tag.ANIMAL_PRODUCT));
            AddToDictionary(WOOL = new Item("Wool", Paths.ITEM_WOOL, DEFAULT_STACK_SIZE, "Soft and warm. It can be woven into cloth with a Loom.", 75, Tag.ANIMAL_PRODUCT));
            AddToDictionary(TRUFFLE = new EdibleItem("Truffle", Paths.ITEM_TRUFFLE, DEFAULT_STACK_SIZE, "A fine delicacy. Adored by mushrooms lovers worldwide.", AppliedEffects.GATHERING_PIG, AppliedEffects.LENGTH_SHORT, "Delicious!", 150, Tag.MUSHROOM, Tag.ANIMAL_PRODUCT));

            AddToDictionary(BANDED_DRAGONFLY = new Item("Banded Dragonfly", Paths.ITEM_BANDED_DRAGONFLY, DEFAULT_STACK_SIZE, "Despite the name, it isn't much of a musician.", 30, Tag.INSECT));
            AddToDictionary(BROWN_CICADA = new Item("Brown Cicada", Paths.ITEM_BROWN_CICADA, DEFAULT_STACK_SIZE, "DJ of summer evenings.", 15, Tag.INSECT));
            AddToDictionary(CAVEWORM = new Item("Caveworm", Paths.ITEM_CAVEWORM, DEFAULT_STACK_SIZE, "These worms live their entire life in darkness. Fish adore them.", 55, Tag.INSECT));
            AddToDictionary(EARTHWORM = new Item("Earthworm", Paths.ITEM_EARTHWORM, DEFAULT_STACK_SIZE, "Fishermen are their biggest fans.", 20, Tag.INSECT));
            AddToDictionary(EMPRESS_BUTTERFLY = new Item("Empress Butterfly", Paths.ITEM_EMPRESS_BUTTERFLY, DEFAULT_STACK_SIZE, "The queen of butterflies. Quite rare!", 750, Tag.INSECT, Tag.RARE));
            AddToDictionary(FIREFLY = new Item("Firefly", Paths.ITEM_FIREFLY, DEFAULT_STACK_SIZE, "   Luminous yellow.\n      Peaceful light in the darkness.\n         Jealous of the moon.", 45, Tag.INSECT));
            AddToDictionary(HONEY_BEE = new Item("Honey Bee", Paths.ITEM_HONEY_BEE, DEFAULT_STACK_SIZE, "Messenger of flowers.", 25, Tag.INSECT));
            AddToDictionary(JEWEL_SPIDER = new Item("Jewel Spider", Paths.ITEM_JEWEL_SPIDER, DEFAULT_STACK_SIZE, "Its carapace solidfies into a prismatic gemlike structure over the course of its life.", 650, Tag.INSECT, Tag.RARE));
            AddToDictionary(LANTERN_MOTH = new Item("Lantern Moth", Paths.ITEM_LANTERN_MOTH, DEFAULT_STACK_SIZE, "Found whenever the light is. A firefly's bestie.", 10, Tag.INSECT));
            AddToDictionary(PINK_LADYBUG = new Item("Pink Ladybug", Paths.ITEM_PINK_LADYBUG, DEFAULT_STACK_SIZE, "These ladybugs camoflage themselves in cherry blossoms.", 77, Tag.INSECT));
            AddToDictionary(RICE_GRASSHOPPER = new Item("Rice Grasshopper", Paths.ITEM_RICE_GRASSHOPPER, DEFAULT_STACK_SIZE, "A common grasshopper. It's considered a pest by rice farmers.", 15, Tag.INSECT, Tag.ACCESSORY));
            AddToDictionary(SNAIL = new Item("Snail", Paths.ITEM_SNAIL, DEFAULT_STACK_SIZE, "Life is simply unfair, don't you think?", 20, Tag.INSECT));
            AddToDictionary(SOLDIER_ANT = new Item("Soldier Ant", Paths.ITEM_SOLDIER_ANT, DEFAULT_STACK_SIZE, "They travel in large battalions. Their bite isn't dangerous, but IS very painful.", 15, Tag.INSECT));
            AddToDictionary(STAG_BEETLE = new Item("Stag Beetle", Paths.ITEM_STAG_BEETLE, DEFAULT_STACK_SIZE, "Their pinch can crush small pebbles. Watch your fingers!", 450, Tag.INSECT, Tag.RARE));
            AddToDictionary(STINGER_HORNET = new Item("Stinger Hornet", Paths.ITEM_STINGER_HORNET, DEFAULT_STACK_SIZE, "Some people just want to watch the world burn. This hornet is the insect equivalent.", 35, Tag.INSECT));
            AddToDictionary(YELLOW_BUTTERFLY = new Item("Yellow Butterfly", Paths.ITEM_YELLOW_BUTTERFLY, DEFAULT_STACK_SIZE, "Doesn't actually like butter very much. She finds it to be too salty.", 25, Tag.INSECT));

            AddToDictionary(BLACKENED_OCTOPUS = new Item("Blackened Octopus", Paths.ITEM_BLACKENED_OCTOPUS, DEFAULT_STACK_SIZE, "It's bizarrely solid for a living creature.", 160, Tag.FISH));
            AddToDictionary(BLUEGILL = new Item("Bluegill", Paths.ITEM_BLUEGILL, DEFAULT_STACK_SIZE, "Creatively named.\nCan be crushed into oil.", 45, Tag.FISH));
            AddToDictionary(BOXER_LOBSTER = new Item("Boxer Lobster", Paths.ITEM_BOXER_LOBSTER, DEFAULT_STACK_SIZE, "This unique breed of lobster prefers to punch rather than pinch.", 190, Tag.FISH));
            AddToDictionary(CARP = new Item("Carp", Paths.ITEM_CARP, DEFAULT_STACK_SIZE, "A freshwater staple.\nCan be crushed into oil.", 60, Tag.FISH));
            AddToDictionary(CATFISH = new Item("Catfish", Paths.ITEM_CATFISH, DEFAULT_STACK_SIZE, "Meow? Woof!\nCan be crushed into oil.", 140, Tag.FISH));
            AddToDictionary(CAVEFISH = new Item("Cavefish", Paths.ITEM_CAVEFISH, DEFAULT_STACK_SIZE, "Strangely, it can survive in both freshwater and saltwater conditions.", 90, Tag.FISH));
            AddToDictionary(CAVERN_TETRA = new Item("Cavern Tetra", Paths.ITEM_CAVERN_TETRA, DEFAULT_STACK_SIZE, "It lives its life in darkness, so over the generations it has lost it's eyes.", 120, Tag.FISH));
            AddToDictionary(CLOUD_FLOUNDER = new Item("Cloud Flounder", Paths.ITEM_CLOUD_FLOUNDER, DEFAULT_STACK_SIZE, "From a distance, it looks like a very small cloud itself.", 111, Tag.FISH));
            AddToDictionary(CRAB = new Item("Crab", Paths.ITEM_CRAB, DEFAULT_STACK_SIZE, "Kinda in a bad mood right now. Makes good patties.", 100, Tag.FISH));
            AddToDictionary(DARK_ANGLER = new Item("Dark Angler", Paths.ITEM_DARK_ANGLER, DEFAULT_STACK_SIZE, "It hunts in the murky depths using the light attached to its head.", 550, Tag.FISH));
            AddToDictionary(EMPEROR_SALMON = new Item("Emperor Salmon", Paths.ITEM_EMPEROR_SALMON, DEFAULT_STACK_SIZE, "The lord of all salmon. King of the river!", 500, Tag.FISH));
            AddToDictionary(GREAT_WHITE_SHARK = new Item("Great White Shark", Paths.ITEM_GREAT_WHITE_SHARK, DEFAULT_STACK_SIZE, "Surprisingly nonaggressive towards humans. King of the seas!", 1100, Tag.FISH));
            AddToDictionary(HERRING = new Item("Herring", Paths.ITEM_HERRING, DEFAULT_STACK_SIZE, "Part of Tuna's band. Has a good ear for harmony.\nCan be crushed into oil.", 45, Tag.FISH));
            AddToDictionary(INFERNAL_SHARK = new Item("Infernal Shark", Paths.ITEM_INFERNAL_SHARK, DEFAULT_STACK_SIZE, "Since it is born in lava, it hardens quickly when exposed to cold air.", 1200, Tag.FISH));
            AddToDictionary(INKY_SQUID = new Item("Inky Squid", Paths.ITEM_INKY_SQUID, DEFAULT_STACK_SIZE, "There\'s still a lot of ink left. Try crushing this into dye.", 50, Tag.FISH));
            AddToDictionary(JUNGLE_PIRANHA = new Item("Jungle Piranha", Paths.ITEM_JUNGLE_PIRANHA, DEFAULT_STACK_SIZE, "It's got a mean bark. And an even meaner bite.", 70, Tag.FISH));
            AddToDictionary(LAKE_TROUT = new Item("Lake Trout", Paths.ITEM_LAKE_TROUT, DEFAULT_STACK_SIZE, "It's a trout that lives in lakes. There isn't much to say.", 85, Tag.FISH));
            AddToDictionary(LUNAR_WHALE = new Item("Lunar Whale", Paths.ITEM_LUNAR_WHALE, DEFAULT_STACK_SIZE, "It is rumored to make a migration to the moon yearly. It can survive without oxygen for extended periods.", 1212, Tag.FISH));
            AddToDictionary(MACKEREL = new Item("Mackerel", Paths.ITEM_MACKEREL, DEFAULT_STACK_SIZE, "Slick fish. Pretty low on the oceanic food chain.\nCan be crushed into oil.", 55, Tag.FISH));
            AddToDictionary(MOLTEN_SQUID = new Item("Molten Squid", Paths.ITEM_MOLTEN_SQUID, DEFAULT_STACK_SIZE, "This squid radiates heat. Handle carefully.", 190, Tag.FISH));
            AddToDictionary(ONYX_EEL = new Item("Onyx Eel", Paths.ITEM_ONYX_EEL, DEFAULT_STACK_SIZE, "It's covered in scales that resemble gemstones. Unforunately, they crumble once removed from the eel itself.", 130, Tag.FISH));
            AddToDictionary(PIKE = new Item("Pike", Paths.ITEM_PIKE, DEFAULT_STACK_SIZE, "Prefers to be called Lance.", 180, Tag.FISH));
            AddToDictionary(PUFFERFISH = new Item("Pufferfish", Paths.ITEM_PUFFERFISH, DEFAULT_STACK_SIZE, "Tends to bottle up its emotions, often to disastrous results.", 130, Tag.FISH));
            AddToDictionary(QUEEN_AROWANA = new Item("Queen Arowana", Paths.ITEM_QUEEN_AROWANA, DEFAULT_STACK_SIZE, "Queen of the jungle stream. It's very hard to catch one of these!", 800, Tag.FISH));
            AddToDictionary(RED_SNAPPER = new Item("Red Snapper", Paths.ITEM_RED_SNAPPER, DEFAULT_STACK_SIZE, "Supposedly the favored fish of cats worldwide.", 90, Tag.FISH));
            AddToDictionary(SALMON = new Item("Salmon", Paths.ITEM_SALMON, DEFAULT_STACK_SIZE, "A run of the mill salmon. They say that each season, the strongest is chosen to be their emperor.\nCan be crushed into oil.", 120, Tag.FISH));
            AddToDictionary(SARDINE = new Item("Sardine", Paths.ITEM_SARDINE, DEFAULT_STACK_SIZE, "It's small fry. Commonly canned.\nCan be crushed into oil.", 35, Tag.FISH));
            AddToDictionary(SEA_TURTLE = new Item("Sea Turtle", Paths.ITEM_SEA_TURTLE, DEFAULT_STACK_SIZE, "Knowing that this turtle will one day reach its destination fills you with determination.", 200, Tag.FISH));
            AddToDictionary(SHRIMP = new Item("Shrimp", Paths.ITEM_SHRIMP, DEFAULT_STACK_SIZE, "A crucial part of the food chain. Also a crucial part of any buffet.", 75, Tag.FISH));
            AddToDictionary(SKY_PIKE = new Item("Sky Pike", Paths.ITEM_SKY_PIKE, DEFAULT_STACK_SIZE, "He prefers to be called Sky Lance.", 222, Tag.FISH));
            AddToDictionary(STORMBRINGER_KOI = new Item("Stormbringer Koi", Paths.ITEM_STORMBRINGER_KOI, DEFAULT_STACK_SIZE, "Harbinger of thunder. It defends itself with strong electrical zaps.", 222, Tag.FISH));
            AddToDictionary(STRIPED_BASS = new Item("Striped Bass", Paths.ITEM_STRIPED_BASS, DEFAULT_STACK_SIZE, "Part of Tuna's band. He likes heavy metal.", 40, Tag.FISH));
            AddToDictionary(SUNFISH = new Item("Sunfish", Paths.ITEM_SUNFISH, DEFAULT_STACK_SIZE, "When it wakes up on the wrong side of the bed, is it called a moonfish?", 150, Tag.FISH));
            AddToDictionary(SWORDFISH = new Item("Swordfish", Paths.ITEM_SWORDFISH, DEFAULT_STACK_SIZE, "En garde!", 150, Tag.FISH));
            AddToDictionary(TUNA = new Item("Tuna", Paths.ITEM_TUNA, DEFAULT_STACK_SIZE, "Part of a band with Striped Bass and Herring. Prefers punk rock.\nCan be crushed into oil.", 80, Tag.FISH));
            AddToDictionary(WHITE_BLOWFISH = new Item("White Blowfish", Paths.ITEM_WHITE_BLOWFISH, DEFAULT_STACK_SIZE, "It eats holes in the clouds. Astonisingly aerodynamic.", 111, Tag.FISH));

            AddToDictionary(AUTUMNS_KISS = new UsableItem("Autumn\'s Kiss", Paths.ITEM_AUTUMNS_KISS, DEFAULT_STACK_SIZE, "Apply", UsableItem.AUTUMNS_KISS_DIALOGUE, "The slightest whiff of this perfume evokes feelings of fading leaves and windy evenings.", 350, Tag.PERFUME));
            AddToDictionary(BIZARRE_PERFUME = new UsableItem("Bizarre Perfume", Paths.ITEM_BIZARRE_PERFUME, DEFAULT_STACK_SIZE, "Apply", UsableItem.BIZARRE_PERFUME_DIALOGUE, "The slightest scent of this perfume evokes feelings of utter confusion and paradoxical expressions.", 165, Tag.PERFUME));
            AddToDictionary(BLISSFUL_SKY = new UsableItem("Blissful Sky", Paths.ITEM_BLISSFUL_SKY, DEFAULT_STACK_SIZE, "Apply", UsableItem.BLISSFUL_SKY_DIALOGUE, "The slightest smell of this perfume evokes feelings of free skys and fearless birds.", 350, Tag.PERFUME));
            AddToDictionary(FLORAL_PERFUME = new UsableItem("Floral Perfume", Paths.ITEM_FLORAL_PERFUME, DEFAULT_STACK_SIZE, "Apply", UsableItem.FLORAL_PERFUME_DIALOGUE, "The slightest sniff of this perfume evokes feelings of blossoming petals and newborn life.", 190, Tag.PERFUME));
            AddToDictionary(OCEAN_GUST = new UsableItem("Ocean Gust", Paths.ITEM_OCEAN_GUST, DEFAULT_STACK_SIZE, "Apply", UsableItem.OCEAN_GUST_DIALOGUE, "The slightest inhale of this perfume evokes feelings of abyssal salinity and encrusted bluffs.", 320, Tag.PERFUME));
            AddToDictionary(OIL = new Item("Oil", Paths.ITEM_OIL, DEFAULT_STACK_SIZE, "Slick and highly flamable. Used a lot for cooking.", 120, Tag.FOOD));
            AddToDictionary(RED_ANGEL = new UsableItem("Red Angel", Paths.ITEM_RED_ANGEL, DEFAULT_STACK_SIZE, "Apply", UsableItem.RED_ANGEL_DIALOGUE, "The slightest breath of this perfume evokes feelings of liberated worlds and unified purity.", 730, Tag.PERFUME, Tag.RARE));
            AddToDictionary(SUMMERS_GIFT = new UsableItem("Summer\'s Gift", Paths.ITEM_SUMMERS_GIFT, DEFAULT_STACK_SIZE, "Apply", UsableItem.SUMMERS_GIFT_DIALOGUE, "The slightest aroma of this perfume evokes feelings of sunburnt skin and grandma's cooking.", 380, Tag.PERFUME));
            AddToDictionary(SWEET_BREEZE = new UsableItem("Sweet Breeze", Paths.ITEM_SWEET_BREEZE, DEFAULT_STACK_SIZE, "Apply", UsableItem.SWEET_BREEZE_DIALOGUE, "The slightest reek of this perfume evokes feelings of sugary tarts and caked frosting.", 425, Tag.PERFUME));
            AddToDictionary(WARM_MEMORIES = new UsableItem("Warm Memories", Paths.ITEM_WARM_MEMORIES, DEFAULT_STACK_SIZE, "Apply", UsableItem.WARM_MEMORIES_DIALOGUE, "The slightest stench of this perfume evokes feelings of forgotten nostalgia and comforting regrets.", 510, Tag.PERFUME));
            AddToDictionary(VANILLA_EXTRACT = new Item("Vanilla Extract", Paths.ITEM_VANILLA_EXTRACT, DEFAULT_STACK_SIZE, "A little basic, honestly.", 150, Tag.FOOD));
            AddToDictionary(MINT_EXTRACT = new Item("Mint Extract", Paths.ITEM_MINT_EXTRACT, DEFAULT_STACK_SIZE, "Winterminty fresh!", 195, Tag.FOOD));
            
            AddToDictionary(PHILOSOPHERS_STONE = new Item("Philosopher\'s Stone", Paths.ITEM_PHILOSOPHERS_STONE, DEFAULT_STACK_SIZE, "Spoken of only in legends. Used (but not always consumed!) in alchemy recipes to convert iron into other metals. May also have another use...", 5700, Tag.ALCHEMY, Tag.RARE));
            AddToDictionary(HEART_VESSEL = new Item("Heart Vessel", Paths.ITEM_HEART_VESSEL, DEFAULT_STACK_SIZE, "When gifted with a pure heart, it transforms into the reciever's greatest desire.", 2400, Tag.ALCHEMY));
            AddToDictionary(INVINCIROID = new UsableItem("Invinciroid", Paths.ITEM_INVINCIROID, DEFAULT_STACK_SIZE, "Drink", UsableItem.INVINCIROID_DIALOGUE, "Concocted with power! Extends any active food boosts.", 900, Tag.ALCHEMY));
            AddToDictionary(SHIMMERING_SALVE = new EdibleItem("Shimmering Salve", Paths.ITEM_SHIMMERING_SALVE, DEFAULT_STACK_SIZE, "This vial is full of liquid luck. After you drink it, you are guaranteed to find a rare item!", AppliedEffects.BLESSED, AppliedEffects.LENGTH_INFINITE, "Delicious!", 3320, Tag.ALCHEMY));
            AddToDictionary(VOODOO_STEW = new EdibleItem("Voodoo Stew", Paths.ITEM_VOODOO_STEW, DEFAULT_STACK_SIZE, "It's probably for the best that you don't think too hard about what went into this. Who knows what kind of boost eating this will provide?", AppliedEffects.BEWITCHED, AppliedEffects.LENGTH_LONG, "Delicious!", 430, Tag.ALCHEMY, Tag.FOOD));
            AddToDictionary(MOSS_BOTTLE = new UsableItem("Moss Bottle", Paths.ITEM_MOSS_BOTTLE, DEFAULT_STACK_SIZE, "Use", UsableItem.MOSS_BOTTLE_DIALOGUE, "Brewed for plant consumption. Accelerates the growth of nearby plants by a day.", 210, Tag.ALCHEMY));
            AddToDictionary(TROPICAL_BOTTLE = new UsableItem("Tropical Bottle", Paths.ITEM_TROPICAL_BOTTLE, DEFAULT_STACK_SIZE, "Use", UsableItem.TROPICAL_BOTTLE_DIALOGUE, "Concocted for plant consumption. Accelerates the growth of nearby plants by 3 days.", 500, Tag.ALCHEMY));
            AddToDictionary(SKY_BOTTLE = new UsableItem("Sky Bottle", Paths.ITEM_SKY_BOTTLE, DEFAULT_STACK_SIZE, "Use", UsableItem.SKY_BOTTLE_DIALOGUE, "Synthesized for plant consumption. Instantly grows all nearby plants.", 520, Tag.ALCHEMY));
            AddToDictionary(PRIMEVAL_ELEMENT = new UsableItem("Primeval Element", Paths.ITEM_PRIMEVAL_ELEMENT, DEFAULT_STACK_SIZE, "Activate", UsableItem.PRIMEVAL_ELEMENT_DIALOGUE, "Quaking with energy. Try using it outside to accelerate forage growth...", 1970, Tag.ALCHEMY));
            AddToDictionary(LAND_ELEMENT = new UsableItem("Land Element", Paths.ITEM_LAND_ELEMENT, DEFAULT_STACK_SIZE, "Activate", UsableItem.LAND_ELEMENT_DIALOGUE, "Vibrating with heat. When first activated, your current location will be stored. Activate again to instantly warp to the stored location.", 1200, Tag.ALCHEMY));
            AddToDictionary(SKY_ELEMENT = new UsableItem("Sky Element", Paths.ITEM_SKY_ELEMENT, DEFAULT_STACK_SIZE, "Activate", UsableItem.SKY_ELEMENT_DIALOGUE, "Floating with emotion. Activating it on a rainy day summons the sun.", 1580, Tag.ALCHEMY));
            AddToDictionary(SEA_ELEMENT = new UsableItem("Sea Element", Paths.ITEM_SEA_ELEMENT, DEFAULT_STACK_SIZE, "Activate", UsableItem.SEA_ELEMENT_DIALOGUE, "Shimmering with strength. Activating it on a sunny day summons the rain.", 1600, Tag.ALCHEMY));
            AddToDictionary(UNSTABLE_LIQUID = new UsableItem("Unstable Liquid", Paths.ITEM_UNSTABLE_LIQUID, DEFAULT_STACK_SIZE, "Uncork", UsableItem.UNSTABLE_LIQUID_DIALOGUE, "The blast from this can water all crops nearby. Just don't uncork it carelessly.", 2200, Tag.ALCHEMY));
            AddToDictionary(BURST_STONE = new UsableItem("Burst Stone", Paths.ITEM_BURST_STONE, DEFAULT_STACK_SIZE, "Detonate", UsableItem.BURST_STONE_DIALOGUE, "The blast from this will clear all debris nearby. It feels dangerous to carry this around...", 1500, Tag.ALCHEMY));
            AddToDictionary(SOOTHE_CANDLE = new UsableItem("Soothe Candle", Paths.ITEM_SOOTHE_CANDLE, DEFAULT_STACK_SIZE, "Light", UsableItem.SOOTHE_CANDLE_DIALOGUE, "Contains moss from the cavern walls. This candle can warp you to the second stratum caves.", 260, Tag.ALCHEMY));
            AddToDictionary(SPICED_CANDLE = new UsableItem("Spiced Candle", Paths.ITEM_SPICED_CANDLE, DEFAULT_STACK_SIZE, "Light", UsableItem.SPICED_CANDLE_DIALOGUE, "Contains a feather from the mountain. This candle can warp you to the first stratum temple.", 330, Tag.ALCHEMY));
            AddToDictionary(SUGAR_CANDLE = new UsableItem("Sugar Candle", Paths.ITEM_SUGAR_CANDLE, DEFAULT_STACK_SIZE, "Light", UsableItem.SUGAR_CANDLE_DIALOGUE, "Contains a feather from the jungle. This candle can warp you to the third stratum.", 480, Tag.ALCHEMY));
            AddToDictionary(BLACK_CANDLE = new UsableItem("Black Candle", Paths.ITEM_BLACK_CANDLE, DEFAULT_STACK_SIZE, "Light", UsableItem.BLACK_CANDLE_DIALOGUE, "Contains a feather from close to home. This candle can warp you to the farm.", 350, Tag.ALCHEMY));
            AddToDictionary(SALTED_CANDLE = new UsableItem("Salted Candle", Paths.ITEM_SALTED_CANDLE, DEFAULT_STACK_SIZE, "Light", UsableItem.SALTED_CANDLE_DIALOGUE, "Contains a feather from the beach. This candle can warp you to the beach.", 330, Tag.ALCHEMY));

            AddToDictionary(SWEET_INCENSE = new Item("Sweet Incense", Paths.ITEM_SWEET_INCENSE, DEFAULT_STACK_SIZE, "Infused with marigold. Offer it to a shrine to trade for a few of an uncommon item.", 450, Tag.ALCHEMY));
            AddToDictionary(LAVENDER_INCENSE = new Item("Lavender Incense", Paths.ITEM_LAVENDER_INCENSE, DEFAULT_STACK_SIZE, "Infused with lavender. Offer it to a shrine and something good will happen..", 380, Tag.ALCHEMY));
            AddToDictionary(IMPERIAL_INCENSE = new Item("Imperial Incense", Paths.ITEM_IMPERIAL_INCENSE, DEFAULT_STACK_SIZE, "Infused with roses. Offer it to a shrine to trade for a rare item.", 1920, Tag.ALCHEMY));
            AddToDictionary(COLD_INCENSE = new Item("Cold Incense", Paths.ITEM_COLD_INCENSE, DEFAULT_STACK_SIZE, "Infused with mint. Offer it to a shrine to trade for a bunch of a common item.", 450, Tag.ALCHEMY));
            AddToDictionary(FRESH_INCENSE = new Item("Fresh Incense", Paths.ITEM_FRESH_INCENSE, DEFAULT_STACK_SIZE, "Infused with persimmon. Offer it to a shrine in exchange for a blessing.", 350, Tag.ALCHEMY));

            AddToDictionary(FAIRY_DUST = new Item("Fairy Dust", Paths.ITEM_FAIRY_DUST, DEFAULT_STACK_SIZE, "Very rare dust. Nobody has ever seen the faries it comes from. The pink color would be suitable for a dye.", 800, Tag.RARE));
            AddToDictionary(GOLDEN_LEAF = new Item("Golden Leaf", Paths.ITEM_GOLDEN_LEAF, DEFAULT_STACK_SIZE, "This shining leaf is used in high-end crafting.", 800, Tag.RARE));
            AddToDictionary(HONEYCOMB = new Item("Honeycomb", Paths.ITEM_HONEYCOMB, DEFAULT_STACK_SIZE, "Empty, unfortunately. Used to build beehives.", 50));
            AddToDictionary(ICE_NINE = new Item("Ice Nine", Paths.ITEM_ICE_NINE, DEFAULT_STACK_SIZE, "Legends say that a single crystal of this is enough to freeze an ocean.", 999, Tag.RARE));
            AddToDictionary(MOSSY_BARK = new Item("Mossy Bark", Paths.ITEM_MOSSY_BARK, DEFAULT_STACK_SIZE, "Covered in lichens. It\'s pretty soft, try crushing this in a painter\'s press.", 10, Tag.WOOD));
            AddToDictionary(OYSTER_MUSHROOM = new Item("Oyster Mushroom", Paths.ITEM_OYSTER_MUSHROOM, DEFAULT_STACK_SIZE, "An uncommon mushroom that grows on treetrunks. ", 40, Tag.MUSHROOM));
            AddToDictionary(WOOD = new Item("Wood", Paths.ITEM_WOOD, DEFAULT_STACK_SIZE, "A thick log of wood. Very useful for crafting.", 15, Tag.WOOD));
            AddToDictionary(HARDWOOD = new Item("Hardwood", Paths.ITEM_HARDWOOD, DEFAULT_STACK_SIZE, "Extra tough piece of wood. It's less common than normal wood.", 80, Tag.WOOD));
            AddToDictionary(BAMBOO = new Item("Bamboo", Paths.ITEM_BAMBOO, DEFAULT_STACK_SIZE, "Stronger than it looks. Useful crafting material.", 30, Tag.WOOD));

            AddToDictionary(CLAY = new Item("Clay", Paths.ITEM_CLAY, DEFAULT_STACK_SIZE, "Soft chunk of earth. Useful for crafting and pottery.", 15));
            AddToDictionary(STONE = new Item("Stone", Paths.ITEM_STONE, DEFAULT_STACK_SIZE, "Hard chunk of earth. Used for crafting.", 25));
            AddToDictionary(ADAMANTITE_BAR = new Item("Adamantite Bar", Paths.ITEM_ADAMANTITE_BAR, DEFAULT_STACK_SIZE, "The strongest material unknown to man!", 1400, Tag.RARE, Tag.BAR));
            AddToDictionary(ADAMANTITE_ORE = new Item("Adamantite Ore", Paths.ITEM_ADAMANTITE_ORE, DEFAULT_STACK_SIZE, "A bit of some kind of ore? ", 300, Tag.RARE, Tag.ORE));
            AddToDictionary(AMETHYST = new Item("Amethyst", Paths.ITEM_AMETHYST, DEFAULT_STACK_SIZE, "A simple gemstone. It represents balance.", 150, Tag.GEM));
            AddToDictionary(AQUAMARINE = new Item("Aquamarine", Paths.ITEM_AQUAMARINE, DEFAULT_STACK_SIZE, "An uncommon gemstone. It represents truth.", 240, Tag.GEM));
            AddToDictionary(COAL = new Item("Coal", Paths.ITEM_COAL, DEFAULT_STACK_SIZE, "Compressed carbon. Used to smelt ores in a furnace and for crafting. You could also crush it into a dye.", 80, Tag.ORE));
            AddToDictionary(DIAMOND = new Item("Diamond", Paths.ITEM_DIAMOND, DEFAULT_STACK_SIZE, "Now this is a rare find! Represents purity.", 1000, Tag.GEM, Tag.RARE));
            AddToDictionary(EARTH_CRYSTAL = new Item("Earth Crystal", Paths.ITEM_EARTH_CRYSTAL, DEFAULT_STACK_SIZE, "A mysterious gemstone. Somehow, it feels shifting to the touch.", 500, Tag.GEM));
            AddToDictionary(EMERALD = new Item("Emerald", Paths.ITEM_EMERALD, DEFAULT_STACK_SIZE, "A valuable gemstone. Represents growth.", 800, Tag.GEM));
            AddToDictionary(FIRE_CRYSTAL = new Item("Fire Crystal", Paths.ITEM_FIRE_CRYSTAL, DEFAULT_STACK_SIZE, "An irreconcilable gemstone. Somehow, it feels engulfing to the touch. ", 500, Tag.GEM));
            AddToDictionary(GOLD_BAR = new Item("Gold Bar", Paths.ITEM_GOLD_BAR, DEFAULT_STACK_SIZE, "Smelted bar of gold. It is quite soft, so be careful.", 520, Tag.BAR));
            AddToDictionary(GOLD_ORE = new Item("Gold Ore", Paths.ITEM_GOLD_ORE, DEFAULT_STACK_SIZE, "You\'ve struck it rich! Go smelt this in a furnace!", 100, Tag.ORE));
            AddToDictionary(IRON_BAR = new Item("Iron Bar", Paths.ITEM_IRON_BAR, DEFAULT_STACK_SIZE, "Smelted bar of iron. Used for crafting all kinds of things.", 240,Tag.BAR));
            AddToDictionary(IRON_ORE = new Item("Iron Ore", Paths.ITEM_IRON_ORE, DEFAULT_STACK_SIZE, "Raw hunk of iron. Try smelting it in a furnace.", 40, Tag.ORE));
            AddToDictionary(MYTHRIL_BAR = new Item("Mythril Bar", Paths.ITEM_MYTHRIL_BAR, DEFAULT_STACK_SIZE, "Bar of some blue mineral. It\'s extraordinarily strong yet pliable.", 440, Tag.BAR));
            AddToDictionary(MYTHRIL_ORE = new Item("Mythril Ore", Paths.ITEM_MYTHRIL_ORE, DEFAULT_STACK_SIZE, "Chunk of some strange ore. Try smelting it in a furnace.", 80, Tag.ORE));
            AddToDictionary(MYTHRIL_CHIP = new Item("Mythril Chip", Paths.ITEM_MYTHRIL_CHIP, DEFAULT_STACK_SIZE, "Electronic part made of some unknown metal. Try smelting it in a furnace?", 55));
            AddToDictionary(QUARTZ = new Item("Quartz", Paths.ITEM_QUARTZ, DEFAULT_STACK_SIZE, "A common gemstone. Represents healing.", 120, Tag.GEM));
            AddToDictionary(RUBY = new Item("Ruby", Paths.ITEM_RUBY, DEFAULT_STACK_SIZE, "An exquisite gemstone. Represents zest.", 700, Tag.GEM));
            AddToDictionary(SALT_SHARDS = new Item("Salt Shards", Paths.ITEM_SALT_SHARDS, DEFAULT_STACK_SIZE, "Now all you need are pepper shards.", 10));
            AddToDictionary(SAPPHIRE = new Item("Sapphire", Paths.ITEM_SAPPHIRE, DEFAULT_STACK_SIZE, "A beautiful gemstone. Represents virtue.", 600, Tag.GEM));
            AddToDictionary(SCRAP_IRON = new Item("Scrap Iron", Paths.ITEM_SCRAP_IRON, DEFAULT_STACK_SIZE, "Chunk of low quality iron. Pretty much junk, honestly.", 5, Tag.ORE));
            AddToDictionary(WATER_CRYSTAL = new Item("Water Crystal", Paths.ITEM_WATER_CRYSTAL, DEFAULT_STACK_SIZE, "An unrecognized gemstone. Somehow, it feels cascading to the touch.", 500, Tag.GEM));
            AddToDictionary(WIND_CRYSTAL = new Item("Wind Crystal", Paths.ITEM_WIND_CRYSTAL, DEFAULT_STACK_SIZE, "An enigmatic gemstone. Somehow, it feels fleeting to the touch.", 500, Tag.GEM));
            AddToDictionary(OPAL = new Item("Opal", Paths.ITEM_OPAL, DEFAULT_STACK_SIZE, "A serene gemstone. Represents loyalty.", 200, Tag.GEM));
            AddToDictionary(TOPAZ = new Item("Topaz", Paths.ITEM_TOPAZ, DEFAULT_STACK_SIZE, "A dazzling gemstone. It represents nobility of spirit.", 180, Tag.GEM));
            AddToDictionary(GOLD_CHIP = new Item("Gold Chip", Paths.ITEM_GOLD_CHIP, DEFAULT_STACK_SIZE, "Valuable electronics made of gold. Try smelting it in a furnace?", 85));
            AddToDictionary(IRON_CHIP = new Item("Iron Chip", Paths.ITEM_IRON_CHIP, DEFAULT_STACK_SIZE, "Busted machinery made of iron. Try smelting it in a furnace?", 35));
            AddToDictionary(TRILOBITE = new Item("Trilobite", Paths.ITEM_TRILOBITE, DEFAULT_STACK_SIZE, "A fossilized anthropod. It's used as currency, somewhere.", 33, Tag.ORE));
            AddToDictionary(PRIMORDIAL_SHELL = new Item("Primordial Shell", Paths.ITEM_PRIMORDIAL_SHELL, DEFAULT_STACK_SIZE, "Who knows how long the turtle this shell came from live for? Such a perfectly preserved specimen is quite a rare find!", 250, Tag.ORE));
            AddToDictionary(OLD_BONE = new Item("Old Bone", Paths.ITEM_OLD_BONE, DEFAULT_STACK_SIZE, "Go fetch!", 75, Tag.ORE));
            AddToDictionary(FOSSIL_SHARDS = new Item("Fossil Shards", Paths.ITEM_FOSSIL_SHARDS, DEFAULT_STACK_SIZE, "Unfortunately impossible to repair, but maybe you can find someone to take them off your hands.", 5, Tag.ORE));

            AddToDictionary(WEEDS = new Item("Weeds", Paths.ITEM_WEEDS, DEFAULT_STACK_SIZE, "A clump of stubborn weeds. Useful for crafting and composting.", 2));
            AddToDictionary(CLAM = new Item("Clam", Paths.ITEM_CLAM, DEFAULT_STACK_SIZE, "A bit shy. Very tasty type of shellfish.", 60, Tag.FISH));
            AddToDictionary(CRIMSON_CORAL = new Item("Crimson Coral", Paths.ITEM_CRIMSON_CORAL, DEFAULT_STACK_SIZE, "This vibrant chunk of coral can be pressed into dye.", 30, Tag.FORAGE));
            AddToDictionary(SEA_URCHIN = new Item("Sea Urchin", Paths.ITEM_SEA_URCHIN, DEFAULT_STACK_SIZE, "Spiky! Can be pressed into a dark blue dye.", 40, Tag.FORAGE));
            AddToDictionary(OYSTER = new Item("Oyster", Paths.ITEM_OYSTER, DEFAULT_STACK_SIZE, "The rich briny taste and texture of this mollusk makes oysters a popular type of seafood.", 80, Tag.FORAGE));
            AddToDictionary(PEARL = new Item("Pearl", Paths.ITEM_PEARL, DEFAULT_STACK_SIZE, "A beautiful specimen. Extremely rare!", 1200, Tag.RARE));
            AddToDictionary(SEAWEED = new Item("Seaweed", Paths.ITEM_SEAWEED, DEFAULT_STACK_SIZE, "Clumps of this oceanic weed grow all across the seafloor.", 5, Tag.FORAGE));
            AddToDictionary(FLAWLESS_CONCH = new Item("Flawless Conch", Paths.ITEM_FLAWLESS_CONCH, DEFAULT_STACK_SIZE, "Perfectly pristine in every way. This is an uncommon find.", 500, Tag.FORAGE, Tag.RARE));
            AddToDictionary(LUCKY_COIN = new Item("Lucky Coin", Paths.ITEM_LUCKY_COIN, DEFAULT_STACK_SIZE, "Turns out money DOES grow on trees. This is sure to fetch a pretty penny.", 250, Tag.RARE));
            AddToDictionary(RED_GINGER = new Item("Red Ginger", Paths.ITEM_RED_GINGER, DEFAULT_STACK_SIZE, "Brilliant tropical flower. The pacific red is suitable for dyes.", 30, Tag.FORAGE, Tag.FLOWER));
            AddToDictionary(BLACKBERRY = new EdibleItem("Blackberry", Paths.ITEM_BLACKBERRY, DEFAULT_STACK_SIZE, "Infamous for staining the clothes of many a child.", AppliedEffects.BUG_CATCHING_I_AUTUMN, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 20, Tag.FORAGE, Tag.FRUIT));
            AddToDictionary(PERSIMMON = new EdibleItem("Persimmon", Paths.ITEM_PERSIMMON, DEFAULT_STACK_SIZE, "A sweet autumnal fruit. Popular in jams.", AppliedEffects.FORAGING_I_AUTUMN, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 25, Tag.FORAGE, Tag.FRUIT));
            AddToDictionary(SASSAFRAS = new Item("Sassafras", Paths.ITEM_SASSAFRAS, DEFAULT_STACK_SIZE, "Needs to watch it\'s attitude.", 15, Tag.FORAGE));
            AddToDictionary(WILD_RICE = new Item("Wild Rice", Paths.ITEM_WILD_RICE, DEFAULT_STACK_SIZE, "Very similar to brown rice when cooked.", 15, Tag.FORAGE));
            AddToDictionary(BLUEBELL = new Item("Bluebell", Paths.ITEM_BLUEBELL, DEFAULT_STACK_SIZE, "The natural instrument of spring. Can be crushed into a blue dye.", 30, Tag.FORAGE, Tag.FLOWER));
            AddToDictionary(CHICKWEED = new EdibleItem("Chickweed", Paths.ITEM_CHICKWEED, DEFAULT_STACK_SIZE, "Chickens actually aren\'t overly fond of these. Doesn\'t get them high, either.", AppliedEffects.FORAGING_I_SPRING, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 15, Tag.FORAGE));
            AddToDictionary(NETTLES = new Item("Nettles", Paths.ITEM_NETTLES, DEFAULT_STACK_SIZE, "Commonly brewed into a natural tea.", 15, Tag.FORAGE));
            AddToDictionary(RASPBERRY = new EdibleItem("Raspberry", Paths.ITEM_RASPBERRY, DEFAULT_STACK_SIZE, "Smoked one too many cigars.", AppliedEffects.BUG_CATCHING_I_SPRING, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 10, Tag.FORAGE, Tag.FRUIT));
            AddToDictionary(SUNFLOWER = new Item("Sunflower", Paths.ITEM_SUNFLOWER, DEFAULT_STACK_SIZE, "Praise the sun! This can be crushed into yellow dye.", 50, Tag.FORAGE, Tag.FLOWER));
            AddToDictionary(BLUEBERRY = new EdibleItem("Blueberry", Paths.ITEM_BLUEBERRY, DEFAULT_STACK_SIZE, "Accurately named. Alternatively: Deliciousberry.", AppliedEffects.BUG_CATCHING_I_SUMMER, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 30, Tag.FORAGE, Tag.FRUIT));
            AddToDictionary(ELDERBERRY = new EdibleItem("Elderberry", Paths.ITEM_ELDERBERRY, DEFAULT_STACK_SIZE, "Respect it.", AppliedEffects.CHOPPING_I_AUTUMN, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 25, Tag.FORAGE, Tag.FRUIT));
            AddToDictionary(LAVENDER = new Item("Lavender", Paths.ITEM_LAVENDER, DEFAULT_STACK_SIZE, "Lovingly fragrant. Purple dyes are often made from these.", 55, Tag.FORAGE, Tag.FLOWER));
            AddToDictionary(MARIGOLD = new Item("Marigold", Paths.ITEM_MARIGOLD, DEFAULT_STACK_SIZE, "Ashen blossom of a joyous summer. Try crushing it into dye.", 70, Tag.FORAGE, Tag.FLOWER));
            AddToDictionary(CHANTERELLE = new EdibleItem("Chanterelle", Paths.ITEM_CHANTERELLE, DEFAULT_STACK_SIZE, "This edible mushroom is coveted by survivalists.", AppliedEffects.FORAGING_I_MUSHROOMS, AppliedEffects.LENGTH_SHORT, "Delicious!", 90, Tag.FORAGE, Tag.MUSHROOM));
            AddToDictionary(CHICORY_ROOT = new Item("Chicory Root", Paths.ITEM_CHICORY_ROOT, DEFAULT_STACK_SIZE, "The definition of farmer chic.", 20, Tag.FORAGE));
            AddToDictionary(SNOW_CRYSTAL = new Item("Snow Crystal", Paths.ITEM_SNOW_CRYSTAL, DEFAULT_STACK_SIZE, "Cold to the touch. Has color removing properties when crushed into dye.", 15, Tag.FORAGE));
            AddToDictionary(SNOWDROP = new Item("Snowdrop", Paths.ITEM_SNOWDROP, DEFAULT_STACK_SIZE, "Legends say that this flower grows only where the first snowflakes of winter land. Try crushing it into dye.", 60, Tag.FORAGE, Tag.FLOWER));
            AddToDictionary(WINTERGREEN = new EdibleItem("Wintergreen", Paths.ITEM_WINTERGREEN, DEFAULT_STACK_SIZE, "Minty fresh!", AppliedEffects.FORAGING_I_WINTER, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 40, Tag.FORAGE));
            AddToDictionary(CAVE_FUNGI = new EdibleItem("Cave Fungi", Paths.ITEM_CAVE_FUNGI, DEFAULT_STACK_SIZE, "A suspicious unnamed fungus.", AppliedEffects.FORAGING_II_MUSHROOMS, AppliedEffects.LENGTH_SHORT, "Delicious!", 30, Tag.FORAGE, Tag.MUSHROOM));
            AddToDictionary(CAVE_SOYBEAN = new Item("Cave Soybean", Paths.ITEM_CAVE_SOYBEAN, DEFAULT_STACK_SIZE, "This unique type of soybean grows underground instead of in pods.", 20, Tag.FORAGE));
            AddToDictionary(CACAO_BEAN = new Item("Cacao Bean", Paths.ITEM_CACAO_BEAN, DEFAULT_STACK_SIZE, "Hot chocolate, anyone? Could probably be crushed into a dye.", 30, Tag.FORAGE));
            AddToDictionary(EMERALD_MOSS = new Item("Emerald Moss", Paths.ITEM_EMERALD_MOSS, DEFAULT_STACK_SIZE, "Despite zero exposure to the sun, this moss glows in the dark. It can be crushed into dye.", 15, Tag.FORAGE));
            AddToDictionary(MAIZE = new EdibleItem("Maize", Paths.ITEM_MAIZE, DEFAULT_STACK_SIZE, "Don't get lost!", AppliedEffects.FORAGING_II_SUMMER, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 40, Tag.FORAGE, Tag.VEGETABLE));
            AddToDictionary(MOREL = new EdibleItem("Morel", Paths.ITEM_MOREL, DEFAULT_STACK_SIZE, "This morel is a delicious morsel.", AppliedEffects.FORAGING_I_MUSHROOMS, AppliedEffects.LENGTH_SHORT, "Delicious!", 80, Tag.FORAGE, Tag.MUSHROOM));
            AddToDictionary(MOUNTAIN_WHEAT = new Item("Mountain Wheat", Paths.ITEM_MOUNTAIN_WHEAT, DEFAULT_STACK_SIZE, "A form of wild grain that can be ground into a hardy flour.", 30, Tag.FORAGE));
            AddToDictionary(PINEAPPLE = new EdibleItem("Pineapple", Paths.ITEM_PINEAPPLE, DEFAULT_STACK_SIZE, "Paradoxically resembles neither apples nor pine.", AppliedEffects.BUG_CATCHING_II, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 120, Tag.FORAGE, Tag.FRUIT));
            AddToDictionary(SHIITAKE = new EdibleItem("Shiitake", Paths.ITEM_SHIITAKE, DEFAULT_STACK_SIZE, "Sometimes known as the emperor of mushrooms.", AppliedEffects.FORAGING_III_MUSHROOMS, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 120, Tag.FORAGE, Tag.MUSHROOM));
            AddToDictionary(SKY_ROSE = new Item("Sky Rose", Paths.ITEM_SKY_ROSE, DEFAULT_STACK_SIZE, "This delicate flower gathers moisture/nfrom passing clouds.", 150, Tag.FORAGE,Tag.FLOWER));
            AddToDictionary(SPICY_LEAF = new Item("Spicy Leaf", Paths.ITEM_SPICY_LEAF, DEFAULT_STACK_SIZE, "This otherwise nondescript weed releases a surprisingly spicy oil when cooked.", 10, Tag.FORAGE));
            AddToDictionary(VANILLA_BEAN = new Item("Vanilla Bean", Paths.ITEM_VANILLA_BEAN, DEFAULT_STACK_SIZE, "Don\'t try eating it raw. That\'s a mistake only made once.", 4, Tag.FORAGE));

            AddToDictionary(CRYSTAL_KEY = new Item("Crystal Key", Paths.ITEM_CRYSTAL_KEY, DEFAULT_STACK_SIZE, "A crystaline key. Perhaps there is a chest somewhere that this key could unlock?", 150));
            AddToDictionary(SEDIMENTARY_KEY = new Item("Sedimentary Key", Paths.ITEM_SEDIMENTARY_KEY, DEFAULT_STACK_SIZE, "A static key. Perhaps there is a chest somewhere that this key could unlock?", 50));
            AddToDictionary(METAMORPHIC_KEY = new Item("Metamorphic Key", Paths.ITEM_METAMORPHIC_KEY, DEFAULT_STACK_SIZE, "A transformed key. Perhaps there is a chest somewhere that this key could unlock?", 50));
            AddToDictionary(IGNEOUS_KEY = new Item("Igneous Key", Paths.ITEM_IGNEOUS_KEY, DEFAULT_STACK_SIZE, "A warm key. Perhaps there is a chest somewhere that this key could unlock?", 50));
            AddToDictionary(ANCIENT_KEY = new Item("Ancient Key", Paths.ITEM_ANCIENT_KEY, DEFAULT_STACK_SIZE, "An old key. Perhaps there are some chests nearby that this key could unlock?", 50));
            AddToDictionary(SKELETON_KEY = new Item("Skeleton Key", Paths.ITEM_SKELETON_KEY, DEFAULT_STACK_SIZE, "A calcium key. This can probably unlock any chests you find.", 75));

            AddToDictionary(LOAMY_COMPOST = new Item("Loamy Compost", Paths.ITEM_LOAMY_COMPOST, DEFAULT_STACK_SIZE, "Compost made with decomposing weeds or insects. Apply to your fields for a chance to get seeds back when harvesting.", 20, Tag.COMPOST));
            AddToDictionary(QUALITY_COMPOST = new Item("Quality Compost", Paths.ITEM_QUALITY_COMPOST, DEFAULT_STACK_SIZE, "Compost made with rotten vegetables or vegetation. Apply to your fields to increase crop quality.", 80, Tag.COMPOST));
            AddToDictionary(DEW_COMPOST = new Item("Dew Compost", Paths.ITEM_DEW_COMPOST, DEFAULT_STACK_SIZE, "Compost made with decayed fish. Apply to your fields to accelerate crop growth.", 65, Tag.COMPOST));
            AddToDictionary(SWEET_COMPOST = new Item("Sweet Compost", Paths.ITEM_SWEET_COMPOST, DEFAULT_STACK_SIZE, "Compost made with rotting fruit. Apply to your fields and you\'ll be able to gather some insects in addition to crops while harvesting.", 55, Tag.COMPOST));
            AddToDictionary(DECAY_COMPOST = new Item("Decay Compost", Paths.ITEM_DECAY_COMPOST, DEFAULT_STACK_SIZE, "Compost made with festering meat or eggs. Apply to your fields and something weird might happen?", 100, Tag.COMPOST));
            AddToDictionary(FROST_COMPOST = new Item("Frost Compost", Paths.ITEM_FROST_COMPOST, DEFAULT_STACK_SIZE, "Compost made from melted snow. Apply to your fields to prevent crops from wilting when the seasons shift.", 30, Tag.COMPOST));
            AddToDictionary(THICK_COMPOST = new Item("Thick Compost", Paths.ITEM_THICK_COMPOST, DEFAULT_STACK_SIZE, "Compost made with festering mushrooms or clay. Apply to your fields and the soil will retain water better.", 55, Tag.COMPOST));
            AddToDictionary(SHINING_COMPOST = new Item("Shining Compost", Paths.ITEM_SHINING_COMPOST, DEFAULT_STACK_SIZE, "Compost made from valuables. Apply to your fields and something good will happen.", 250, Tag.COMPOST, Tag.RARE));

            AddToDictionary(BLUE_DYE = new DyeItem("Blue Dye", Paths.ITEM_BLUE_DYE, DEFAULT_STACK_SIZE, Util.BLUE, Util.HOUSE_BLUE, "A liberating vial of blue colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 100, Tag.DYE));
            AddToDictionary(NAVY_DYE = new DyeItem("Navy Dye", Paths.ITEM_NAVY_DYE, DEFAULT_STACK_SIZE, Util.NAVY, Util.HOUSE_NAVY, "An elegant vial of navy colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 110, Tag.DYE));
            AddToDictionary(BLACK_DYE = new DyeItem("Black Dye", Paths.ITEM_BLACK_DYE, DEFAULT_STACK_SIZE, Util.BLACK, Util.HOUSE_BLACK, "A grievous vial of black colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 120, Tag.DYE));
            AddToDictionary(RED_DYE = new DyeItem("Red Dye", Paths.ITEM_RED_DYE, DEFAULT_STACK_SIZE, Util.RED, Util.HOUSE_RED, "A sanguine vial of red colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 100, Tag.DYE));
            AddToDictionary(PINK_DYE = new DyeItem("Pink Dye", Paths.ITEM_PINK_DYE, DEFAULT_STACK_SIZE, Util.PINK, Util.HOUSE_PINK, "An ecstatic vial of pink colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 1200, Tag.DYE, Tag.RARE));
            AddToDictionary(LIGHT_BROWN_DYE = new DyeItem("Light Brown Dye", Paths.ITEM_LIGHT_BROWN_DYE, DEFAULT_STACK_SIZE, Util.LIGHT_BROWN, Util.HOUSE_LIGHT_BROWN, "A steady vial of light brown colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 100, Tag.DYE));
            AddToDictionary(DARK_BROWN_DYE = new DyeItem("Dark Brown Dye", Paths.ITEM_DARK_BROWN_DYE, DEFAULT_STACK_SIZE, Util.DARK_BROWN, Util.HOUSE_DARK_BROWN, "A tectonic vial of dark brown colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 100, Tag.DYE));
            AddToDictionary(ORANGE_DYE = new DyeItem("Orange Dye", Paths.ITEM_ORANGE_DYE, DEFAULT_STACK_SIZE, Util.ORANGE, Util.HOUSE_ORANGE, "An artistic vial of orange colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 150, Tag.DYE));
            AddToDictionary(YELLOW_DYE = new DyeItem("Yellow Dye", Paths.ITEM_YELLOW_DYE, DEFAULT_STACK_SIZE, Util.YELLOW, Util.HOUSE_YELLOW, "A regal vial of yellow colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 120, Tag.DYE));
            AddToDictionary(PURPLE_DYE = new DyeItem("Purple Dye", Paths.ITEM_PURPLE_DYE, DEFAULT_STACK_SIZE, Util.PURPLE, Util.HOUSE_PURPLE, "An imperial vial of purple colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 125, Tag.DYE));
            AddToDictionary(GREEN_DYE = new DyeItem("Green Dye", Paths.ITEM_GREEN_DYE, DEFAULT_STACK_SIZE, Util.GREEN, Util.HOUSE_GREEN, "A vibrant vial of green colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 75, Tag.DYE));
            AddToDictionary(OLIVE_DYE = new DyeItem("Olive Dye", Paths.ITEM_OLIVE_DYE, DEFAULT_STACK_SIZE, Util.OLIVE, Util.HOUSE_OLIVE, "A distinguished vial of olive colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 70, Tag.DYE));
            AddToDictionary(WHITE_DYE = new DyeItem("White Dye", Paths.ITEM_WHITE_DYE, DEFAULT_STACK_SIZE, Util.WHITE, Util.HOUSE_WHITE, "A classic vial of white colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 130, Tag.DYE));
            AddToDictionary(LIGHT_GREY_DYE = new DyeItem("Light Grey Dye", Paths.ITEM_LIGHT_GREY_DYE, DEFAULT_STACK_SIZE, Util.LIGHT_GREY, Util.HOUSE_LIGHT_GREY, "A timeless vial of light grey colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 100, Tag.DYE));
            AddToDictionary(DARK_GREY_DYE = new DyeItem("Dark Grey Dye", Paths.ITEM_DARK_GREY_DYE, DEFAULT_STACK_SIZE, Util.DARK_GREY, Util.HOUSE_DARK_GREY, "A futuristic vial of dark grey colored dye. In your inventory, hold this and right click on clothes or furniture to apply it.", 160, Tag.DYE));
            AddToDictionary(UN_DYE = new DyeItem("Un-Dye", Paths.ITEM_UN_DYE, DEFAULT_STACK_SIZE, null, null, "A strange vial of dye-nullifying dye. In your inventory, hold this and right click on clothes or furniture to remove dye.", 80, Tag.DYE));
            
            AddToDictionary(ALBINO_WING = new Item("Albino Wing", Paths.ITEM_ALBINO_WING, DEFAULT_STACK_SIZE, "This type of wing is very rare. Unfortunately, it is said that finding one is a bad omen.", 800, Item.Tag.RARE, Tag.BAT_PRODUCT));
            AddToDictionary(BAT_WING = new Item("Bat Wing", Paths.ITEM_BAT_WING, DEFAULT_STACK_SIZE, "Looks like something other than birds used your birdhouse last night. The pigment can be utilized in a dye.", 35, Tag.BAT_PRODUCT));
            AddToDictionary(BIRDS_NEST = new Item("Bird\'s Nest", Paths.ITEM_BIRDS_NEST, DEFAULT_STACK_SIZE, "Represents the accumulated wealth of at least one bird. Perhaps crushing it in a Seed Maker would yield interesting results?", 25, Tag.BIRD_PRODUCT));
            AddToDictionary(BLACK_FEATHER = new Item("Black Feather", Paths.ITEM_BLACK_FEATHER, DEFAULT_STACK_SIZE, "Black feather of a bird. It represents a connection between the world of the living and the dead.", 75, Tag.BIRD_PRODUCT));
            AddToDictionary(BLUE_FEATHER = new Item("Blue Feather", Paths.ITEM_BLUE_FEATHER, DEFAULT_STACK_SIZE, "Blue feather of a bird. It represents a connection between the earth and sky.", 85, Tag.BIRD_PRODUCT));
            AddToDictionary(GUANO = new Item("Guano", Paths.ITEM_GUANO, DEFAULT_STACK_SIZE, "Yuck! Might make for useful fertilizer.", 1, Tag.BAT_PRODUCT, Tag.COMPOST));
            AddToDictionary(PRISMATIC_FEATHER = new Item("Prismatic Feather", Paths.ITEM_PRISMATIC_FEATHER, DEFAULT_STACK_SIZE, "This type of feather is very rare. It represents the connection between man and nature.", 1005, Item.Tag.RARE, Tag.BIRD_PRODUCT));
            AddToDictionary(PURE_FEATHER = new Item("Pure Feather", Paths.ITEM_PURE_FEATHER, DEFAULT_STACK_SIZE, "Often used as a symbol of proposal. It traditionally represents the connection between lovers.", 10000, Item.Tag.RARE, Item.Tag.NO_TRASH));
            AddToDictionary(WHITE_FEATHER = new Item("White Feather", Paths.ITEM_WHITE_FEATHER, DEFAULT_STACK_SIZE, "White feather of a bird. It represents a connection between imagination and reality.", 65, Tag.BIRD_PRODUCT));
            AddToDictionary(RED_FEATHER = new Item("Red Feather", Paths.ITEM_RED_FEATHER, DEFAULT_STACK_SIZE, "Scarlet feather of a bird. It represents a connection between creation and destruction.", 95, Tag.BIRD_PRODUCT));

            AddToDictionary(FRIED_EGG = new EdibleItem("Fried Egg", Paths.ITEM_FRIED_EGG, DEFAULT_STACK_SIZE, "Sunny side up!", AppliedEffects.GATHERING_CHICKEN, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 360, Tag.FOOD, Tag.SALTY, Tag.BREAKFAST, Tag.CUISINE));
            AddToDictionary(EGG_SCRAMBLE = new EdibleItem("Egg Scramble", Paths.ITEM_EGG_SCRAMBLE, DEFAULT_STACK_SIZE, "They're all mixed up.", AppliedEffects.GATHERING_SHEEP, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 520, Tag.FOOD, Tag.SALTY, Tag.BREAKFAST, Tag.CUISINE));
            AddToDictionary(BREAKFAST_POTATOES = new EdibleItem("Breakfast Potatoes", Paths.ITEM_BREAKFAST_POTATOES, DEFAULT_STACK_SIZE, "Cubed, not hashed.", AppliedEffects.SPEED_II_MORNING, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 300, Tag.FOOD, Tag.SALTY, Tag.BREAKFAST, Tag.CUISINE));
            AddToDictionary(SPICY_BACON = new EdibleItem("Spicy Bacon", Paths.ITEM_SPICY_BACON, DEFAULT_STACK_SIZE, "Sizzling spicy!", AppliedEffects.SPEED_III_MORNING, AppliedEffects.LENGTH_SHORT, "Delicious!", 330, Tag.FOOD, Tag.SPICY, Tag.SALTY, Tag.BREAKFAST, Tag.CUISINE));
            AddToDictionary(BLUEBERRY_PANCAKES = new EdibleItem("Blueberry Pancakes", Paths.ITEM_BLUEBERRY_PANCAKES, DEFAULT_STACK_SIZE, "A classic pancake-fruit pairing.", AppliedEffects.SPEED_IV_MORNING, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 240, Tag.FOOD, Tag.FRUITY, Tag.BREAKFAST, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(APPLE_MUFFIN = new EdibleItem("Apple Muffin", Paths.ITEM_APPLE_MUFFIN, DEFAULT_STACK_SIZE, "Freshly baked!", AppliedEffects.SPEED_IV_AUTUMN, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 750, Tag.FOOD, Tag.BREAKFAST, Tag.SWEET, Tag.BREAKFAST, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(ROASTED_PUMPKIN = new EdibleItem("Roasted Pumpkin", Paths.ITEM_ROASTED_PUMPKIN, DEFAULT_STACK_SIZE, "Crispy out the outside, yet tender in the middle.", AppliedEffects.CHOPPING_III, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 2000, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(VEGGIE_SIDE_ROAST = new EdibleItem("Veggie Side Roast", Paths.ITEM_VEGGIE_SIDE_ROAST, DEFAULT_STACK_SIZE, "A variety of veggies to go with your meal!", AppliedEffects.FORAGING_III_AUTUMN, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 435, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(WRAPPED_CABBAGE = new EdibleItem("Wrapped Cabbage", Paths.ITEM_WRAPPED_CABBAGE, DEFAULT_STACK_SIZE, "These cabbage rolls combine leafy greens with savory meat.", AppliedEffects.MINING_IV, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 1350, Tag.FOOD, Tag.MEATY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(SEAFOOD_PAELLA = new EdibleItem("Seafood Paella", Paths.ITEM_SEAFOOD_PAELLA, DEFAULT_STACK_SIZE, "A delicious collection of the ocean's greatest hits, all over rice.", AppliedEffects.MINING_IV, AppliedEffects.LENGTH_LONG, "Delicious!", 400, Tag.FOOD, Tag.SEAFOOD, Tag.MEATY, Tag.CUISINE));
            AddToDictionary(SEASONAL_PIPERADE = new EdibleItem("Seasonal Piperade", Paths.ITEM_SEASONAL_PIPERADE, DEFAULT_STACK_SIZE, "Do you think Piper would appreciate this?", AppliedEffects.CHOPPING_IV, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 480, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(SUPER_JUICE = new EdibleItem("Super Juice", Paths.ITEM_SUPER_JUICE, DEFAULT_STACK_SIZE, "Contains all 14 essential vitamins!", AppliedEffects.CHOPPING_VI, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 460, Tag.FOOD, Tag.VEGGIE, Tag.DRINK, Tag.BITTER, Tag.CUISINE));
            AddToDictionary(POTATO_AND_BEET_FRIES = new EdibleItem("Potato and Beet Fries", Paths.ITEM_POTATO_AND_BEET_FRIES, DEFAULT_STACK_SIZE, "The mixture of fried beets and potatoes provides some nice variety to typical potato fries.", AppliedEffects.FORAGING_III, AppliedEffects.LENGTH_SHORT, "Delicious!", 410, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.BREAKFAST, Tag.CUISINE));
            AddToDictionary(PICKLED_BEET_EGG = new EdibleItem("Pickled Beet Egg", Paths.ITEM_PICKLED_BEET_EGG, DEFAULT_STACK_SIZE, "\"Pickled\" using an unconventional speed-brine technique. The secret is in the oils of crushed up spicy leaf.", AppliedEffects.GATHERING_CHICKEN, AppliedEffects.LENGTH_SHORT, "Delicious!", 270, Tag.FOOD, Tag.SPICY, Tag.BREAKFAST, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(COCONUT_BOAR = new EdibleItem("Coconut Boar", Paths.ITEM_COCONUT_BOAR, DEFAULT_STACK_SIZE, "A unique combination of tropical seasoning and mountainous meat. And it's huge!", AppliedEffects.MINING_V, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 1560, Tag.FOOD, Tag.MEATY, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(SPRING_PIZZA = new EdibleItem("Spring Pizza", Paths.ITEM_SPRING_PIZZA, DEFAULT_STACK_SIZE, "Thin-crusted pizza made from springtime plants.", AppliedEffects.FORAGING_II, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 640, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(STRAWBERRY_SALAD = new EdibleItem("Strawberry Salad", Paths.ITEM_STRAWBERRY_SALAD, DEFAULT_STACK_SIZE, "The sweet strawberries help spruce up this salad!", AppliedEffects.FORAGING_IV_SPRING, AppliedEffects.LENGTH_LONG, "Delicious!", 500, Tag.FOOD, Tag.SWEET, Tag.VEGGIE, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(BOAR_STEW = new EdibleItem("Boar Stew", Paths.ITEM_BOAR_STEW, DEFAULT_STACK_SIZE, "A hearty stew made from boar, potatoes, and carrots. Extremely filling.", AppliedEffects.GATHERING_BOAR, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 585, Tag.FOOD, Tag.MEATY, Tag.CUISINE));
            AddToDictionary(BAKED_POTATO = new EdibleItem("Baked Potato", Paths.ITEM_BAKED_POTATO, DEFAULT_STACK_SIZE, "A classic! Stuffed with melted butter.", AppliedEffects.MINING_II, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 625, Tag.FOOD, Tag.VEGGIE, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(FRESH_SALAD = new EdibleItem("Fresh Salad", Paths.ITEM_FRESH_SALAD, DEFAULT_STACK_SIZE, "The combination of diced watermelon, cucumber, and tomatoes make this an incredibly refreshing dish.", AppliedEffects.FORAGING_V_FLOWERS, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 550, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(STEWED_VEGGIES = new EdibleItem("Stewed Veggies", Paths.ITEM_STEWED_VEGGIES, DEFAULT_STACK_SIZE, "A combination of stewed summer vegetables. Stewed cucumber is surprisingly delicious!", AppliedEffects.CHOPPING_III, AppliedEffects.LENGTH_LONG, "Delicious!", 455, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(MEATY_PIZZA = new EdibleItem("Meaty Pizza", Paths.ITEM_MEATY_PIZZA, DEFAULT_STACK_SIZE, "This pizza is covered in savory meats!", AppliedEffects.MINING_III, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 500, Tag.FOOD, Tag.MEATY, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(WATERMELON_ICE = new EdibleItem("Watermelon Ice", Paths.ITEM_WATERMELON_ICE, DEFAULT_STACK_SIZE, "Shaved ice flavored with watermelon juice. Nothing can hit the spot better on a sunny day than this!", AppliedEffects.SPEED_III, AppliedEffects.LENGTH_SHORT, "Delicious!", 350, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(SUSHI_ROLL = new EdibleItem("Sushi Roll", Paths.ITEM_SUSHI_ROLL, DEFAULT_STACK_SIZE, "A custom roll of maki sushi. Chef's choice of fish.", AppliedEffects.FISHING_II, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 140, Tag.FOOD, Tag.SALTY, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(SEARED_TUNA = new EdibleItem("Seared Tuna", Paths.ITEM_SEARED_TUNA, DEFAULT_STACK_SIZE, "Although the outside of this tuna is seared, the inside is still rare.", AppliedEffects.FISHING_V_OCEAN, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 360, Tag.FOOD, Tag.SEAFOOD, Tag.SPICY, Tag.CUISINE));
            AddToDictionary(DELUXE_SUSHI = new EdibleItem("Deluxe Sushi", Paths.ITEM_DELUXE_SUSHI, DEFAULT_STACK_SIZE, "A deluxe roll of maki sushi. Made with high quality fish!", AppliedEffects.FISHING_III, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 250, Tag.FOOD, Tag.SEAFOOD, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(MUSHROOM_STIR_FRY = new EdibleItem("Mushroom Stir Fry", Paths.ITEM_MUSHROOM_STIR_FRY, DEFAULT_STACK_SIZE, "The copious amount of mushrooms added to this stir fry contribute their earthy flavor to the dish.", AppliedEffects.FORAGING_IV_MUSHROOMS, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 230, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(MOUNTAIN_TERIYAKI = new EdibleItem("Mountain Teriyaki", Paths.ITEM_MOUNTAIN_TERIYAKI, DEFAULT_STACK_SIZE, "Boar meat glazed with a sweet sauce. It's a Nimbus-Fusion classic!", AppliedEffects.MINING_VI, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 410, Tag.FOOD, Tag.MEATY, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(LETHAL_SASHIMI = new EdibleItem("Lethal Sashimi", Paths.ITEM_LETHAL_SASHIMI, DEFAULT_STACK_SIZE, "Supposedly, the pufferfish venom was removed before rolling. Supposedly.", AppliedEffects.SPEED_V, AppliedEffects.LENGTH_VERY_SHORT, "Delicious!", 555, Tag.FOOD, Tag.MEATY, Tag.SEAFOOD, Tag.SWEET, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(VANILLA_ICE_CREAM = new EdibleItem("Vanilla Ice Cream", Paths.ITEM_VANILLA_ICE_CREAM, DEFAULT_STACK_SIZE, "Sweet, sugarly, and cold.", AppliedEffects.FORAGING_V_WINTER, AppliedEffects.LENGTH_SHORT, "Delicious!", 240, Tag.FOOD, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(BERRY_MILKSHAKE = new EdibleItem("Berry Milkshake", Paths.ITEM_BERRY_MILKSHAKE, DEFAULT_STACK_SIZE, "The blended wild berries add a fruity touch.", AppliedEffects.SPEED_III, AppliedEffects.LENGTH_SHORT, "Delicious!", 210, Tag.FOOD, Tag.SWEET, Tag.DRINK, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(MINT_CHOCO_BAR = new EdibleItem("Mint-Choco Bar", Paths.ITEM_MINT_CHOCO_BAR, DEFAULT_STACK_SIZE, "A classic combination of mint and chocolate, wrapped up into a conveniently pocket-sized ice cream bar.", AppliedEffects.SPEED_III, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 385, Tag.FOOD, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(MINTY_MELT = new EdibleItem("Minty Melt", Paths.ITEM_MINTY_MELT, DEFAULT_STACK_SIZE, "Eat it! Quickly, before it melts any further!", AppliedEffects.SPEED_III, AppliedEffects.LENGTH_SHORT, "Delicious!", 640, Tag.FOOD, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(BANANA_SUNDAE = new EdibleItem("Banana Sundae", Paths.ITEM_BANANA_SUNDAE, DEFAULT_STACK_SIZE, "A sunday classic!", AppliedEffects.SPEED_V_WINTER, AppliedEffects.LENGTH_SHORT, "Delicious!",3609, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(FRENCH_ONION_SOUP = new EdibleItem("French Onion Soup", Paths.ITEM_FRENCH_ONION_SOUP, DEFAULT_STACK_SIZE, "An exquisite soup topped with cheese.", AppliedEffects.FORAGING_III_FLOWERS, AppliedEffects.LENGTH_SHORT, "Delicious!", 750, Tag.FOOD, Tag.VEGGIE, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(TOMATO_SOUP = new EdibleItem("Tomato Soup", Paths.ITEM_TOMATO_SOUP, DEFAULT_STACK_SIZE, "Probably the easiest soup ever.", AppliedEffects.CHOPPING_III, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 650, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(FARMERS_STEW = new EdibleItem("Farmer's Stew", Paths.ITEM_FARMERS_STEW, DEFAULT_STACK_SIZE, "This standard Nimbus Town recipe uses three different vegetables!", AppliedEffects.FORAGING_III, AppliedEffects.LENGTH_LONG, "Delicious!", 1700, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(CREAM_OF_MUSHROOM = new EdibleItem("Cream of Mushroom", Paths.ITEM_CREAM_OF_MUSHROOM, DEFAULT_STACK_SIZE, "It's as close to liquid mushrooms as you can get.", AppliedEffects.FORAGING_VI_MUSHROOMS, AppliedEffects.LENGTH_SHORT, "Delicious!", 580, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(CREAMY_SPINACH = new EdibleItem("Creamy Spinach", Paths.ITEM_CREAMY_SPINACH, DEFAULT_STACK_SIZE, "The spinach adds an earthy flavor to this creamy soup.", AppliedEffects.GATHERING_COW, AppliedEffects.LENGTH_SHORT, "Delicious!", 600, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(SHRIMP_GUMBO = new EdibleItem("Shrimp Gumbo", Paths.ITEM_SHRIMP_GUMBO, DEFAULT_STACK_SIZE, "Spicy but delicious!", AppliedEffects.CHOPPING_V, AppliedEffects.LENGTH_LONG, "Delicious!", 440, Tag.FOOD, Tag.SEAFOOD, Tag.SPICY, Tag.CUISINE));
            AddToDictionary(FRIED_CATFISH = new EdibleItem("Fried Catfish", Paths.ITEM_FRIED_CATFISH, DEFAULT_STACK_SIZE, "Is there any other way to prepare Catfish?", AppliedEffects.FISHING_IV_FRESHWATER, AppliedEffects.LENGTH_LONG, "Delicious!", 620, Tag.FOOD, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(GRILLED_SALMON = new EdibleItem("Grilled Salmon", Paths.ITEM_GRILLED_SALMON, DEFAULT_STACK_SIZE, "Perfectly grilled with a sauce of lemon butter on top.", AppliedEffects.FISHING_IV, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 1260, Tag.FOOD, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(BAKED_SNAPPER = new EdibleItem("Baked Snapper", Paths.ITEM_BAKED_SNAPPER, DEFAULT_STACK_SIZE, "It's stuffed with healthy spinach!", AppliedEffects.FISHING_IV_OCEAN, AppliedEffects.LENGTH_LONG, "Delicious!", 700, Tag.FOOD, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(SWORDFISH_POT = new EdibleItem("Swordfish Pot", Paths.ITEM_SWORDFISH_POT, DEFAULT_STACK_SIZE, "It's hotpot style with an eye-catching swordfish taking center stage.", AppliedEffects.FISHING_V, AppliedEffects.LENGTH_LONG, "Delicious!", 620, Tag.FOOD, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(HONEY_STIR_FRY = new EdibleItem("Honey Stir Fry", Paths.ITEM_HONEY_STIR_FRY, DEFAULT_STACK_SIZE, "This peppers & meat stirfry includes honey for sweetness.", AppliedEffects.GATHERING_BOAR, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 300, Tag.FOOD, Tag.SWEET, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(BUTTERED_ROLLS = new EdibleItem("Buttered Rolls", Paths.ITEM_BUTTERED_ROLLS, DEFAULT_STACK_SIZE, "Traditional rolls, baked fresh and ready to eat!", AppliedEffects.MINING_III, AppliedEffects.LENGTH_SHORT, "Delicious!", 500, Tag.FOOD, Tag.BREAKFAST, Tag.CUISINE));
            AddToDictionary(SAVORY_ROAST = new EdibleItem("Savory Roast", Paths.ITEM_SAVORY_ROAST, DEFAULT_STACK_SIZE, "This took a long time to roast. It's worth taking the time to savor it.", AppliedEffects.MINING_IV, AppliedEffects.LENGTH_LONG, "Delicious!", 1020, Tag.FOOD, Tag.MEATY, Tag.CUISINE));
            AddToDictionary(STUFFED_FLOUNDER = new EdibleItem("Stuffed Flounder", Paths.ITEM_STUFFED_FLOUNDER, DEFAULT_STACK_SIZE, "This flouder is packed full of lobster and crab!", AppliedEffects.FISHING_IV_CLOUD, AppliedEffects.LENGTH_LONG, "Delicious!", 800, Tag.FOOD, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(CLAM_LINGUINI = new EdibleItem("Clam Linguini", Paths.ITEM_CLAM_LINGUINI, DEFAULT_STACK_SIZE, "The definitive seafood pasta dish. Nothing else comes close.", AppliedEffects.FORAGING_III_BEACH, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 300, Tag.FOOD, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(LEMON_SHORTCAKE = new EdibleItem("Lemon Shortcake", Paths.ITEM_LEMON_SHORTCAKE, DEFAULT_STACK_SIZE, "The sweet and sour notes of lemon permeate this dense cake.", AppliedEffects.SPEED_IV_AUTUMN, AppliedEffects.LENGTH_SHORT, "Delicious!", 1020, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(CHERRY_CHEESECAKE = new EdibleItem("Cherry Cheesecake", Paths.ITEM_CHERRY_CHEESECAKE, DEFAULT_STACK_SIZE, "A creamy cheesecake topped with fresh cherries.", AppliedEffects.SPEED_III_SPRING, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 620, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(MOUNTAIN_BREAD = new EdibleItem("Mountain Bread", Paths.ITEM_MOUNTAIN_BREAD, DEFAULT_STACK_SIZE, "This hearty bread is dense and filling, though somewhat devoid of flavor.", AppliedEffects.CHOPPING_I, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 200, Tag.FOOD, Tag.BREAKFAST, Tag.CUISINE));
            AddToDictionary(CHICKWEED_BLEND = new EdibleItem("Chickweed Blend", Paths.ITEM_CHICKWEED_BLEND, DEFAULT_STACK_SIZE, "This salad is made from wild chickweed and spinach leaves.", AppliedEffects.FORAGING_III_SPRING, AppliedEffects.LENGTH_SHORT, "Delicious!", 220, Tag.FOOD, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(CRISPY_GRASSHOPPER = new EdibleItem("Crispy Grasshopper", Paths.ITEM_CRISPY_GRASSHOPPER, DEFAULT_STACK_SIZE, "Fried grasshopper has a taste that can primarily be described as \"crunchy\".", AppliedEffects.BUG_CATCHING_II, AppliedEffects.LENGTH_SHORT, "Delicious!", 250, Tag.FOOD, Tag.SALTY, Tag.SPICY, Tag.CUISINE));
            AddToDictionary(REJUVENATION_TEA = new EdibleItem("Rejuvenation Tea", Paths.ITEM_REJUVENATION_TEA, DEFAULT_STACK_SIZE, "This healthy tea includes a surprisingly spicy bite.", AppliedEffects.SPEED_II, AppliedEffects.LENGTH_SHORT, "Delicious!", 100, Tag.FOOD, Tag.DRINK, Tag.BITTER, Tag.SPICY, Tag.CUISINE));
            AddToDictionary(HOMESTYLE_JELLY = new EdibleItem("\"Homestyle\" Jelly", Paths.ITEM_HOMESTYLE_JELLY, DEFAULT_STACK_SIZE, "It's basically what would happen if you made mashed potatoes with fruit.", AppliedEffects.SPEED_IV_SPRING, AppliedEffects.LENGTH_SHORT, "Delicious!", 140, Tag.FOOD, Tag.SWEET, Tag.BREAKFAST, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(SEAFOOD_BASKET = new EdibleItem("Seafood Basket", Paths.ITEM_SEAFOOD_BASKET, DEFAULT_STACK_SIZE, "A basket full of fried seafood! Bad for your weight, good for your soul!", AppliedEffects.FORAGING_IV_BEACH, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 340, Tag.FOOD, Tag.SEAFOOD, Tag.SALTY, Tag.SPICY, Tag.CUISINE));
            AddToDictionary(WILD_POPCORN = new EdibleItem("Wild Popcorn", Paths.ITEM_WILD_POPCORN, DEFAULT_STACK_SIZE, "Made from wild maize, then coated with butter and chunks of salt.", AppliedEffects.CHOPPING_II, AppliedEffects.LENGTH_SHORT, "Delicious!", 200, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(ELDERBERRY_TART = new EdibleItem("Elderberry Tart", Paths.ITEM_ELDERBERRY_TART, DEFAULT_STACK_SIZE, "A tart tart made from elderberries.", AppliedEffects.FORAGING_III_AUTUMN, AppliedEffects.LENGTH_SHORT, "Delicious!", 170, Tag.FOOD, Tag.SWEET, Tag.BREAKFAST, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(DARK_TEA = new EdibleItem("Dark Tea", Paths.ITEM_DARK_TEA, DEFAULT_STACK_SIZE, "Tea made from seeping sassafras leaves. The flavor is exceedingly strong.", AppliedEffects.FORAGING_II_WINTER, AppliedEffects.LENGTH_SHORT, "Delicious!", 90, Tag.FOOD, Tag.BITTER, Tag.DRINK, Tag.CUISINE));
            AddToDictionary(LICHEN_JUICE = new EdibleItem("Lichen Juice", Paths.ITEM_LICHEN_JUICE, DEFAULT_STACK_SIZE, "Earthy juice concocted from moss. Looks better than it smells, smells better than it tastes.", AppliedEffects.MINING_II, AppliedEffects.LENGTH_SHORT, "Delicious!", 120, Tag.FOOD, Tag.BITTER, Tag.DRINK, Tag.CUISINE));
            AddToDictionary(AUTUMN_MASH = new EdibleItem("Autumn Mash", Paths.ITEM_AUTUMN_MASH, DEFAULT_STACK_SIZE, "An extremely sweet variant of mashed potatoes composed of oranges and persimmons.", AppliedEffects.SPEED_III_AUTUMN, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 340, Tag.FOOD, Tag.VEGGIE, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(CAMPFIRE_COFFEE = new EdibleItem("Campfire Coffee", Paths.ITEM_CAMPFIRE_COFFEE, DEFAULT_STACK_SIZE, "Brewed from chicory root. A standard survivalist drink.", AppliedEffects.SPEED_III_WINTER, AppliedEffects.LENGTH_SHORT, "Delicious!", 100, Tag.FOOD, Tag.DRINK, Tag.BITTER, Tag.CUISINE));
            AddToDictionary(BLIND_DINNER = new EdibleItem("Blind Dinner", Paths.ITEM_BLIND_DINNER, DEFAULT_STACK_SIZE, "A dish starring blind fish from caves where the sun doesn't shine.", AppliedEffects.FISHING_VI_CAVE, AppliedEffects.LENGTH_LONG, "Delicious!", 567, Tag.FOOD, Tag.SEAFOOD, Tag.SALTY, Tag.BITTER, Tag.CUISINE));
            AddToDictionary(FRIED_FISH = new EdibleItem("Fried Fish", Paths.ITEM_FRIED_FISH, DEFAULT_STACK_SIZE, "First you fry it, then you fry it some more.", AppliedEffects.FISHING_III_FRESHWATER, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 300, Tag.FOOD, Tag.SEAFOOD, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(SARDINE_SNACK = new EdibleItem("Sardine Snack", Paths.ITEM_SARDINE_SNACK, DEFAULT_STACK_SIZE, "Also contains herring and striped bass.", AppliedEffects.FISHING_III_OCEAN, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 210, Tag.FOOD, Tag.SEAFOOD, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(DWARVEN_STEW = new EdibleItem("Dwarven Stew", Paths.ITEM_DWARVEN_STEW, DEFAULT_STACK_SIZE, "This stew contains onions and rare mountain mushrooms.", AppliedEffects.GATHERING_PIG, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 600, Tag.FOOD, Tag.MEATY, Tag.CUISINE));
            AddToDictionary(SWEET_COCO_TREAT = new EdibleItem("Sweet Coco Treat", Paths.ITEM_SWEET_COCO_TREAT, DEFAULT_STACK_SIZE, "A homemade snackbar made of coconut, vanilla, and honey.", AppliedEffects.SPEED_III, AppliedEffects.LENGTH_SHORT, "Delicious!", 490, Tag.FOOD, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(FRIED_OYSTERS = new EdibleItem("Fried Oysters", Paths.ITEM_FRIED_OYSTERS, DEFAULT_STACK_SIZE, "...on the half shell.", AppliedEffects.FISHING_III_OCEAN, AppliedEffects.LENGTH_SHORT, "Delicious!", 415, Tag.FOOD, Tag.SALTY, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(SURVIVORS_SURPRISE = new EdibleItem("Survivor's Surprise", Paths.ITEM_SURVIVORS_SURPRISE, DEFAULT_STACK_SIZE, "The surprise is that energy bars made of ants and worms don't actually taste half bad.", AppliedEffects.BUG_CATCHING_III, AppliedEffects.LENGTH_LONG, "Delicious!", 130, Tag.FOOD, Tag.BITTER, Tag.CUISINE));
            AddToDictionary(EEL_ROLL = new EdibleItem("Eel Roll", Paths.ITEM_EEL_ROLL, DEFAULT_STACK_SIZE, "An exotic piece of eel nigiri.", AppliedEffects.FISHING_IV, AppliedEffects.LENGTH_SHORT, "Delicious!", 210, Tag.FOOD, Tag.SWEET, Tag.SALTY, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(RAW_CALAMARI = new EdibleItem("\"Raw\" Calamari", Paths.ITEM_RAW_CALAMARI, DEFAULT_STACK_SIZE, "There's no need to cook an octopus that lived its whole life in lava. Somehow the texture raw is already crunchy.", AppliedEffects.FISHING_III_LAVA, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 440, Tag.FOOD, Tag.SALTY, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(STORMFISH = new EdibleItem("Stormfish", Paths.ITEM_STORMFISH, DEFAULT_STACK_SIZE, "Fried sky koi coated with large salt crystals. Very spicy.", AppliedEffects.FISHING_VI_CLOUD, AppliedEffects.LENGTH_LONG, "Delicious!", 560, Tag.FOOD, Tag.SALTY, Tag.SPICY, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(CREAMY_SQUID = new EdibleItem("Creamy Squid", Paths.ITEM_CREAMY_SQUID, DEFAULT_STACK_SIZE, "A unique dish of magma squid in cream sauce.", AppliedEffects.FISHING_VI_LAVA, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 810, Tag.FOOD, Tag.SWEET, Tag.SEAFOOD, Tag.CUISINE));
            AddToDictionary(ESCARGOT = new EdibleItem("Escargot", Paths.ITEM_ESCARGOT, DEFAULT_STACK_SIZE, "Cooked snails served in shell, with a buttery sauce.", AppliedEffects.BUG_CATCHING_II, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 460, Tag.FOOD, Tag.CUISINE));
            AddToDictionary(LUMINOUS_STEW = new EdibleItem("Luminous Stew", Paths.ITEM_LUMINOUS_STEW, DEFAULT_STACK_SIZE, "The liquid itself is glowing...", AppliedEffects.BUG_CATCHING_VI_NIGHT, AppliedEffects.LENGTH_LONG, "Delicious!", 200, Tag.FOOD, Tag.BITTER, Tag.CUISINE));
            AddToDictionary(COLESLAW = new EdibleItem("Coleslaw", Paths.ITEM_COLESLAW, DEFAULT_STACK_SIZE, "Traditional slaw made from cabbage, carrots, and mayo.", AppliedEffects.CHOPPING_IV, AppliedEffects.LENGTH_SHORT, "Delicious!", 1200, Tag.FOOD, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(RATATOUILLE = new EdibleItem("Ratatouille", Paths.ITEM_RATATOUILLE, DEFAULT_STACK_SIZE, "A dish made from stacked eggplant, tomatoes, and peppers. Rats can make it too.", AppliedEffects.BUG_CATCHING_IV, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 580, Tag.FOOD, Tag.SAVORY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(EGGPLANT_PARMESAN = new EdibleItem("Eggplant Parmesan", Paths.ITEM_EGGPLANT_PARMESAN, DEFAULT_STACK_SIZE, "Eggplant baked with cheese. Rich and flavorful.", AppliedEffects.FORAGING_IV_SUMMER, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 980, Tag.FOOD, Tag.SAVORY, Tag.VEGGIE, Tag.CUISINE));

            AddToDictionary(BOAR_JERKY = new EdibleItem("Boar Jerky", Paths.ITEM_BOAR_JERKY, DEFAULT_STACK_SIZE, "This jerky was made from mountain boars. It's tougher and more gamey than traditional beef jerky.", AppliedEffects.MINING_I, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 220, Tag.FOOD, Tag.SALTY, Tag.MEATY, Tag.CUISINE));
            AddToDictionary(KLIPPFISK = new EdibleItem("Klippfisk", Paths.ITEM_KLIPPFISK, DEFAULT_STACK_SIZE, "Fish preserved in open air with salt and sun. The smell is strong, but the flavor is stronger. Can be reconstituted with water.", AppliedEffects.FISHING_I, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 250, Tag.FOOD, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(DRIED_APPLE = new EdibleItem("Dried Apple", Paths.ITEM_DRIED_APPLE, DEFAULT_STACK_SIZE, "These slices of dried apple are perfect for a quick snack.", AppliedEffects.FORAGING_I, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 520, Tag.FOOD, Tag.FRUITY, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(BANANA_CHIPS = new EdibleItem("Banana Chips", Paths.ITEM_BANANA_CHIPS, DEFAULT_STACK_SIZE, "Chips made from dried banana slices. Commonly used in trail mix.", AppliedEffects.BUG_CATCHING_I, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 460, Tag.FOOD, Tag.FRUITY, Tag.SALTY, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(COCONUT_CHIPS = new EdibleItem("Coconut Chips", Paths.ITEM_COCONUT_CHIPS, DEFAULT_STACK_SIZE, "A unique chip made from compressing coconut flesh into discs, then drying.", AppliedEffects.SPEED_I, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 200, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(CHERRY_RAISINS = new EdibleItem("Cherry Raisins", Paths.ITEM_CHERRY_RAISINS, DEFAULT_STACK_SIZE, "These raisens use cherries instead of grapes.", AppliedEffects.SPEED_I_SPRING, AppliedEffects.LENGTH_LONG, "Delicious!", 270, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(DRIED_CITRUS = new EdibleItem("Dried Citrus", Paths.ITEM_DRIED_CITRUS, DEFAULT_STACK_SIZE, "Slices of dried citrus. Popular as a snack in tropical places.", AppliedEffects.SPEED_I_SUMMER, AppliedEffects.LENGTH_LONG, "Delicious!", 390, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(DRIED_OLIVES = new EdibleItem("Dried Olives", Paths.ITEM_DRIED_OLIVES, DEFAULT_STACK_SIZE, "These olives have been preserved by the sun. The flavor is rich and full.", AppliedEffects.SPEED_I_AUTUMN, AppliedEffects.LENGTH_LONG, "Delicious!", 400, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(DRIED_STRAWBERRY = new EdibleItem("Dried Strawberry", Paths.ITEM_DRIED_STRAWBERRY, DEFAULT_STACK_SIZE, "Dried strawberries! Like normal strawberries, but drier!", AppliedEffects.SPEED_III_SPRING, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 220, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(WATERLESS_MELON = new EdibleItem("Waterless Melon", Paths.ITEM_WATERLESS_MELON, DEFAULT_STACK_SIZE, "Haha, very funny... ", AppliedEffects.SPEED_III_SUMMER, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 330, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(POTATO_CRISPS = new EdibleItem("Potato Crisps", Paths.ITEM_POTATO_CRISPS, DEFAULT_STACK_SIZE, "Addictive potato chips! Snacktime has arrived!", AppliedEffects.CHOPPING_II, AppliedEffects.LENGTH_VERY_LONG, "Delicious!", 230, Tag.FOOD, Tag.SWEET, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(SEAWEED_SNACK = new EdibleItem("Seaweed Snack", Paths.ITEM_SEAWEED_SNACK, DEFAULT_STACK_SIZE, "These dried sheets of seaweed have a strong salty flavor.", AppliedEffects.FISHING_II_OCEAN, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 100, Tag.FOOD, Tag.SEAFOOD, Tag.SALTY, Tag.SPICY, Tag.CUISINE));
            AddToDictionary(VEGGIE_CHIPS = new EdibleItem("Veggie Chips", Paths.ITEM_VEGGIE_CHIPS, DEFAULT_STACK_SIZE, "Chips of sun-dried vegetables. Eating healthy never tasted so good!", AppliedEffects.FORAGING_II, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 300, Tag.FOOD, Tag.VEGGIE, Tag.SALTY, Tag.CUISINE));

            AddToDictionary(APPLE_PRESERVES = new EdibleItem("Apple Preserves", Paths.ITEM_APPLE_PRESERVES, DEFAULT_STACK_SIZE, "These delicious apple preserves are perfect over toast.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_I, AppliedEffects.SPEED_I }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 420, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(APPLE_CIDER = new EdibleItem("Apple Cider", Paths.ITEM_APPLE_CIDER, DEFAULT_STACK_SIZE, "This is a non-alcoholic cider, so it's suitable for children too!", new AppliedEffects.Effect[] { AppliedEffects.GATHERING_CHICKEN, AppliedEffects.GATHERING_COW, AppliedEffects.GATHERING_SHEEP }, AppliedEffects.LENGTH_LONG, "Delicious!", 660, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(BANANA_JAM = new EdibleItem("Banana Jam", Paths.ITEM_BANANA_JAM, DEFAULT_STACK_SIZE, "Come on and slam?", new AppliedEffects.Effect[] { AppliedEffects.SPEED_I, AppliedEffects.BUG_CATCHING_I }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 375, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(BEERNANA = new EdibleItem("Beernana", Paths.ITEM_BEERNANA, DEFAULT_STACK_SIZE, "Beer with a topical flavor from bananas. It's kind of odd, honestly.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_V, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_LONG, "Delicious!", 600, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(MARMALADE = new EdibleItem("Marmalade", Paths.ITEM_MARMALADE, DEFAULT_STACK_SIZE, "Royal preserves made from bitter oranges.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_II_SUMMER, AppliedEffects.BUG_CATCHING_II_SUMMER }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 300, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(ALCORANGE = new EdibleItem("Alcorange", Paths.ITEM_ALCORANGE, DEFAULT_STACK_SIZE, "Alcohol + Orange = Alcorange? Get it?", new AppliedEffects.Effect[] { AppliedEffects.MINING_V, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 500, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(LEMONADE = new EdibleItem("Lemonade", Paths.ITEM_LEMONADE, DEFAULT_STACK_SIZE, "Sweet summer lemonade! Now you just need a stand!", new AppliedEffects.Effect[] { AppliedEffects.SPEED_II_AUTUMN, AppliedEffects.FORAGING_II_AUTUMN }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 525, Tag.FOOD, Tag.BITTER, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(SOUR_WINE = new EdibleItem("Sour Wine", Paths.ITEM_SOUR_WINE, DEFAULT_STACK_SIZE, "Wine this sour may not be to everyone's tastes...", new AppliedEffects.Effect[] { AppliedEffects.SPEED_VI, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 800, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(CHERRY_JELLY = new EdibleItem("Cherry Jelly", Paths.ITEM_CHERRY_JELLY, DEFAULT_STACK_SIZE, "Jelly made from cherries. Slightly sour, but still sweet.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_II_SPRING, AppliedEffects.CHOPPING_II_SPRING }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 225, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(CHERWINE = new EdibleItem("Cherwine", Paths.ITEM_CHERWINE, DEFAULT_STACK_SIZE, "It's not actually alcoholic.", new AppliedEffects.Effect[] { AppliedEffects.FISHING_V, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 400, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(MARINATED_OLIVE = new EdibleItem("Marinated Olive", Paths.ITEM_MARINATED_OLIVE, DEFAULT_STACK_SIZE, "These olives are ready to hit the snack platter!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_III_AUTUMN, AppliedEffects.MINING_III_AUTUMN }, AppliedEffects.LENGTH_SHORT, "Delicious!", 430, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(PICKLED_CARROT = new EdibleItem("Pickled Carrot", Paths.ITEM_PICKLED_CARROT, DEFAULT_STACK_SIZE, "The pickling process added a bite of vinegar to this carrot.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_II, AppliedEffects.MINING_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", 370, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(GOOD_OL_PICKLES = new EdibleItem("Good Ol' Pickles", Paths.ITEM_GOOD_OL_PICKLES, DEFAULT_STACK_SIZE, "Traditional, real, homestyle pickles!", new AppliedEffects.Effect[] { AppliedEffects.MINING_II, AppliedEffects.CHOPPING_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", 300, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(BRINY_BEET = new EdibleItem("Briny Beet", Paths.ITEM_BRINY_BEET, DEFAULT_STACK_SIZE, "Can't beat pickled beets!", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_II, AppliedEffects.FORAGING_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", 250, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(SOUSE_EGG = new EdibleItem("Souse Egg", Paths.ITEM_SOUSE_EGG, DEFAULT_STACK_SIZE, "A pickled egg with an excessively vinegary flavor.", new AppliedEffects.Effect[] { AppliedEffects.GATHERING_CHICKEN, AppliedEffects.GATHERING_SHEEP }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 190, Tag.FOOD, Tag.BREAKFAST, Tag.SALTY, Tag.CUISINE));
            AddToDictionary(PICKLED_ONION = new EdibleItem("Pickled Onion", Paths.ITEM_PICKLED_ONION, DEFAULT_STACK_SIZE, "An onion pickled with vinegar and salt. Very strong.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_II, AppliedEffects.FORAGING_II }, AppliedEffects.LENGTH_SHORT, "Delicious!", 230, Tag.FOOD, Tag.SALTY, Tag.VEGGIE, Tag.CUISINE));
            AddToDictionary(PERSIMMON_JAM = new EdibleItem("Persimmon Jam", Paths.ITEM_PERSIMMON_JAM, DEFAULT_STACK_SIZE, "A sticky jam made of wild persimmons. It tends to clump together in the jar.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_III_AUTUMN, AppliedEffects.MINING_III_AUTUMN }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 80, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(AUTUMNAL_WINE = new EdibleItem("Autumnal Wine", Paths.ITEM_AUTUMNAL_WINE, DEFAULT_STACK_SIZE, "Wine made with fresh persimmon. Great for entertaining during autumn evenings.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_VI_AUTUMN, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_LONG, "Delicious!", 210, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(BLACKBERRY_PRESERVES = new EdibleItem("Blackberry Preserves", Paths.ITEM_BLACKBERRY_PRESERVES, DEFAULT_STACK_SIZE, "Preserves made from wild blackberries! Watch out for stains!", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_III }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 75, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(BLACKBERRY_DIGESTIF = new EdibleItem("Blackberry Digestif", Paths.ITEM_BLACKBERRY_DIGESTIF, DEFAULT_STACK_SIZE, "Blackberries contribute their fruity flavour to this dark liqueur.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_VI_AUTUMN, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_LONG, "Delicious!", 200, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(BLUEBERRY_JELLY = new EdibleItem("Blueberry Jelly", Paths.ITEM_BLUEBERRY_JELLY, DEFAULT_STACK_SIZE, "This jelly is made with fresh wild blueberries!", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_III_SUMMER }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 90, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(BLUEBERRY_CORDIAL = new EdibleItem("Blueberry Cordial", Paths.ITEM_BLUEBERRY_CORDIAL, DEFAULT_STACK_SIZE, "A specialty cordial with an aftertaste of blueberries.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_VI_SUMMER, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_LONG, "Delicious!", 220, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(STRAWBERRY_BLAST_JAM = new EdibleItem("Strawberry Blast! Jam", Paths.ITEM_STRAWBERRY_BLAST_JAM, DEFAULT_STACK_SIZE, "Strawberry! Blast! Strawberry! Blast!", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_IV_SPRING }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 140, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(STRAWBERRY_SPIRIT = new EdibleItem("Strawberry Spirit", Paths.ITEM_STRAWBERRY_SPIRIT, DEFAULT_STACK_SIZE, "This alcohol contains the rich fruity taste of strawberries.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_VI_SPRING, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_LONG, "Delicious!", 290, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(ELDERBERRY_JAM = new EdibleItem("Elderberry Jam", Paths.ITEM_ELDERBERRY_JAM, DEFAULT_STACK_SIZE, "Jam made from mashed elderberries. Some of the elderberries were small enough to survive the jamming process.", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_III_AUTUMN, AppliedEffects.FISHING_III_AUTUMN }, AppliedEffects.LENGTH_LONG, "Delicious!", 80, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(ELDERBERRY_APERITIF = new EdibleItem("Elderberry Aperitif", Paths.ITEM_ELDERBERRY_APERITIF, DEFAULT_STACK_SIZE, "Made from elderberries. Drier than you might expect.", new AppliedEffects.Effect[] { AppliedEffects.MINING_VI_AUTUMN, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_LONG, "Delicious!", 210, Tag.FOOD, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(RASPBERRY_JELLY = new EdibleItem("Raspberry Jelly", Paths.ITEM_RASPBERRY_JELLY, DEFAULT_STACK_SIZE, "Jelly made from wild raspberries! Commonly spread on toast or crackers.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_III_SPRING }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 60, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(RASPBERRY_LIQUEUR = new EdibleItem("Raspberry Liqueur", Paths.ITEM_RASPBERRY_LIQUEUR, DEFAULT_STACK_SIZE, "A sweet liqueur made of raspberries. It tastes sour yet tangy.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_VI_SPRING, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 180, Tag.FOOD, Tag.ALCOHOL, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(TOMATO_SALSA = new EdibleItem("Tomato Salsa", Paths.ITEM_TOMATO_SALSA, DEFAULT_STACK_SIZE, "Spicy salsa made from tomatos, ready for a fiesta!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_II, AppliedEffects.FISHING_II }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 250, Tag.FOOD, Tag.SPICY, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(BLOODY_MARIE = new EdibleItem("Bloody Marie", Paths.ITEM_BLOODY_MARIE, DEFAULT_STACK_SIZE, "A spicy cocktail made with tomato. Not for the faint of heart.", new AppliedEffects.Effect[] { AppliedEffects.BEWITCHED, AppliedEffects.SPEED_V, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 440, Tag.FOOD, Tag.ALCOHOL, Tag.SPICY, Tag.CUISINE));
            AddToDictionary(AUTUMN_SALSA = new EdibleItem("Autumn Salsa", Paths.ITEM_AUTUMN_SALSA, DEFAULT_STACK_SIZE, "Specialty salsa made with pumpkin. It's more dense than most salsas.", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_III, AppliedEffects.FISHING_III}, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 1350, Tag.FOOD, Tag.SWEET, Tag.CUISINE));
            AddToDictionary(PUMPKIN_CIDER = new EdibleItem("Pumpkin Cider", Paths.ITEM_PUMPKIN_CIDER, DEFAULT_STACK_SIZE, "Cider brewed from pumpkins. It has a distinctively earthy flavor.", new AppliedEffects.Effect[] { AppliedEffects.CHOPPING_V, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 1900, Tag.FOOD, Tag.BITTER, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(COCONUT_MILK = new UsableItem("Coconut Milk", Paths.ITEM_COCONUT_MILK, DEFAULT_STACK_SIZE, "Drink", UsableItem.MILK_CREAM_DIALOGUE, "Sweet milk made from coconuts! Clears all effects when drunk.", 170, Tag.FOOD, Tag.FRUITY, Tag.SWEET));
            AddToDictionary(COCONUT_RUM = new EdibleItem("Coconut Rum", Paths.ITEM_COCONUT_RUM, DEFAULT_STACK_SIZE, "Rum flavored with coconut. A quintessential part of any tropical vacation.", new AppliedEffects.Effect[] { AppliedEffects.FISHING_V, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 330, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.ALCOHOL, Tag.CUISINE));
            AddToDictionary(WATERMELON_JELLY = new EdibleItem("Watermelon Jelly", Paths.ITEM_WATERMELON_JELLY, DEFAULT_STACK_SIZE, "Jelly made from the flesh of watermelons. The watermelon flavoring is subtle, yet sweet.", new AppliedEffects.Effect[] { AppliedEffects.BUG_CATCHING_IV_SUMMER }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 400, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(WATERMELON_WINE = new EdibleItem("Watermelon Wine", Paths.ITEM_WATERMELON_WINE, DEFAULT_STACK_SIZE, "A fruity wine flavored with watermelon juice. It's like bottled Summer!", new AppliedEffects.Effect[] { AppliedEffects.FORAGING_VI_SUMMER, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 480, Tag.FOOD, Tag.ALCOHOL, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(PINEAPPLE_SALSA = new EdibleItem("Pineapple Salsa", Paths.ITEM_PINEAPPLE_SALSA, DEFAULT_STACK_SIZE, "Sweet tropical salsa made from pineapple. Good over fish.", new AppliedEffects.Effect[] { AppliedEffects.SPEED_II_SUMMER, AppliedEffects.FISHING_II_SUMMER }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 225, Tag.FOOD, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));
            AddToDictionary(TROPICAL_RUM = new EdibleItem("Tropical Rum", Paths.ITEM_TROPICAL_RUM, DEFAULT_STACK_SIZE, "A rum made from pineapple. It has a very bold pineapple flavor.", new AppliedEffects.Effect[] { AppliedEffects.FISHING_VI_SUMMER, AppliedEffects.DIZZY }, AppliedEffects.LENGTH_MEDIUM, "Delicious!", 400, Tag.FOOD, Tag.ALCOHOL, Tag.SWEET, Tag.FRUITY, Tag.CUISINE));

            AddToDictionary(GLASS_SHEET = new PlaceableItem("Glass Sheet", Paths.ITEM_GLASS_SHEET, Paths.ITEM_GLASS_SHEET, Paths.SPRITE_GLASS_SHEET_SPRITESHEET, Paths.SPRITE_GLASS_SHEET_SPRITESHEET, 1, 1, DEFAULT_STACK_SIZE, "A pristine sheet of clear glass. It's stronger than it looks in movies. Can be placed on walls as window panels. Also used for crafting.", 110, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WALL_MIRROR = new PlaceableItem("Wall Mirror", Paths.ITEM_WALL_MIRROR, Paths.ITEM_WALL_MIRROR, Paths.SPRITE_WALL_MIRROR_SPRITESHEET, Paths.SPRITE_WALL_MIRROR_SPRITESHEET, 2, 3, DEFAULT_STACK_SIZE, "A nice mirror really spices up otherwise boring walls.", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));


            AddToDictionary(COTTON_CLOTH = new Item("Cotton Cloth", Paths.ITEM_COTTON_CLOTH, DEFAULT_STACK_SIZE, "A thick reel of cloth woven from cotton. Used to craft clothing and some furniture.", 250, Tag.TEXTILE));
            AddToDictionary(WOOLEN_CLOTH = new Item("Woolen Cloth", Paths.ITEM_WOOLEN_CLOTH, DEFAULT_STACK_SIZE, "Cloth woven out of the wool from sheep. Tougher than cotton. Used to craft clothing and some furniture.", 200, Tag.TEXTILE));
            AddToDictionary(LINEN_CLOTH = new Item("Linen Cloth", Paths.ITEM_LINEN_CLOTH, DEFAULT_STACK_SIZE, "Lightweight textile popular in tropical climates. Used to craft clothing.", 300, Tag.TEXTILE));

            AddToDictionary(WILD_MEAT = new Item("Wild Meat", Paths.ITEM_WILD_MEAT, DEFAULT_STACK_SIZE, "A hearty hunk of boar steak. The staple local meat of Nimbus Town.", 65));
            AddToDictionary(BOAR_HIDE = new Item("Boar Hide", Paths.ITEM_BOAR_HIDE, DEFAULT_STACK_SIZE, "Waste not, want not. This piece of hide can be used for crafting.", 35));

            AddToDictionary(CHEST = new PlaceableItem("Chest", Paths.ITEM_CHEST, Paths.ITEM_CHEST, Paths.SPRITE_CHEST_SPRITESHEET, Paths.SPRITE_CHEST_SPRITESHEET, 2, 1, DEFAULT_STACK_SIZE, "A sturdy wooden chest. You can store your excess items in here.", 500, EntityType.CHEST, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TORCH = new PlaceableItem("Torch", Paths.ITEM_TORCH, Paths.ITEM_TORCH, Paths.SPRITE_TORCH_SPRITESHEET, Paths.SPRITE_TORCH_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "A light-bearing torch. Don't worry, it's 100% safe to use anywhere.", 50, EntityType.LIGHT_DECOR_ANIM, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(COMPOST_BIN = new PlaceableItem("Compost Bin", Paths.ITEM_COMPOST_BIN, Paths.ITEM_COMPOST_BIN, Paths.SPRITE_COMPOST_BIN_SPRITESHEET, Paths.SPRITE_COMPOST_BIN_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "Throw organic material into this and let it spin for a few days to make compost for your crops.", 400, EntityType.COMPOST_BIN, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TERRARIUM = new PlaceableItem("Terrarium", Paths.ITEM_TERRARIUM, Paths.ITEM_TERRARIUM, Paths.SPRITE_TERRARIUM_SPRITESHEET, Paths.SPRITE_TERRARIUM_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "A home where all insects can thrive! And reproduce, of course.", 400, EntityType.TERRARIUM, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DAIRY_CHURN = new PlaceableItem("Dairy Churn", Paths.ITEM_DAIRY_CHURN, Paths.ITEM_DAIRY_CHURN, Paths.SPRITE_DIARY_CHURN_SPRITESHEET, Paths.SPRITE_DIARY_CHURN_SPRITESHEET, 1, 3, DEFAULT_STACK_SIZE, "Churns milk into cream, cream into butter, and butter into cheese.\nDon't ask how the last one works.", 800, EntityType.DAIRY_CHURN, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(MAYONNAISE_MAKER = new PlaceableItem("Mayonnaise Maker", Paths.ITEM_MAYONNAISE_MAKER, Paths.ITEM_MAYONNAISE_MAKER, Paths.SPRITE_MAYONNAISE_MAKER_SPRITESHEET, Paths.SPRITE_MAYONNAISE_MAKER_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "In goes the egg, out comes the mayonnaise.", 800, EntityType.MAYONNAISE_MAKER, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(LOOM = new PlaceableItem("Loom", Paths.ITEM_LOOM, Paths.ITEM_LOOM, Paths.SPRITE_LOOM_SPRITESHEET, Paths.SPRITE_LOOM_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "With this I can weave wool, cotton, and flax into cloth!", 1000, EntityType.LOOM, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CHEFS_TABLE = new PlaceableItem("Chef's Table", Paths.ITEM_CHEFS_TABLE, Paths.ITEM_CHEFS_TABLE, Paths.SPRITE_CHEFS_TABLE_SPRITESHEET, Paths.SPRITE_CHEFS_TABLE_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "A handy preparation table. I can make all kinds of food with this!", 550, EntityType.CHEFS_TABLE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLAY_OVEN = new PlaceableItem("Clay Oven", Paths.ITEM_CLAY_OVEN, Paths.ITEM_CLAY_OVEN, Paths.SPRITE_CLAY_OVEN_SPRITESHEET, Paths.SPRITE_CLAY_OVEN_SPRITESHEET, 2, 3, DEFAULT_STACK_SIZE, "A custom-built clay oven! With this I can cook even more recipes that the Chef's Table can't handle.", 1100, EntityType.CLAY_OVEN, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PERFUMERY = new PlaceableItem("Perfumery", Paths.ITEM_PERFUMERY, Paths.ITEM_PERFUMERY, Paths.SPRITE_PERFUMERY_SPRITESHEET, Paths.SPRITE_PERFUMERY_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "Turns a pair of flowers into lovely perfume. Different pairs yield different results!", 1150, EntityType.PERFUMERY, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BEEHIVE = new PlaceableItem("Beehive", Paths.ITEM_BEEHIVE, Paths.ITEM_BEEHIVE, Paths.SPRITE_BEEHIVE_SPRITESHEET, Paths.SPRITE_BEEHIVE_SPRITESHEET, 1, 3, DEFAULT_STACK_SIZE, "Attracts honey-making bees! Leave it somewhere outside and check back in a few days.", 500, EntityType.BEEHIVE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BIRDHOUSE = new PlaceableItem("Birdhouse", Paths.ITEM_BIRDHOUSE, Paths.ITEM_BIRDHOUSE, Paths.SPRITE_BIRDHOUSE_SPRITESHEET, Paths.SPRITE_BIRDHOUSE_SPRITESHEET, 1, 3, DEFAULT_STACK_SIZE, "Birds need a home too! Leave it outside and this'll fill up with all kinds of stuff.", 550, EntityType.BIRDHOUSE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SEED_MAKER = new PlaceableItem("Seed Maker", Paths.ITEM_SEED_MAKER, Paths.ITEM_SEED_MAKER, Paths.SPRITE_SEED_MAKER_SPRITESHEET, Paths.SPRITE_SEED_MAKER_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Turns crops into seeds. Use it to multiply your yields. Maybe higher quality crops will become something special?", 2300, EntityType.SEED_MAKER, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(POTTERY_WHEEL = new PlaceableItem("Pottery Wheel", Paths.ITEM_POTTERY_WHEEL, Paths.ITEM_POTTERY_WHEEL, Paths.SPRITE_POTTERY_WHEEL_SPRITESHEET, Paths.SPRITE_POTTERY_WHEEL_SPRITESHEET, 1, 1, DEFAULT_STACK_SIZE, "Transforms clay into pottery. The amount of clay used determines the result.", 625, EntityType.POTTERY_WHEEL, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FURNACE = new PlaceableItem("Furnace", Paths.ITEM_FURNACE, Paths.ITEM_FURNACE, Paths.SPRITE_FURNACE_SPRITESHEET, Paths.SPRITE_FURNACE_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Use this to smelt 3 ores of any type and 1 coal into a metal bar.", 1100, EntityType.FURNACE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GEMSTONE_REPLICATOR = new PlaceableItem("Gemstone Replicator", Paths.ITEM_GEMSTONE_REPLICATOR, Paths.ITEM_GEMSTONE_REPLICATOR, Paths.SPRITE_GEMSTONE_REPLICATOR_SPRITESHEET, Paths.SPRITE_GEMSTONE_REPLICATOR_SPRITESHEET, 1, 1, DEFAULT_STACK_SIZE, "This special machine can duplicate any gemstone placed within it. Simply seed it with a gemstone, and wait. The resultant gems are indistinguishable from the real deal!", 2600, EntityType.GEMSTONE_REPLICATOR, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(COMPRESSOR = new PlaceableItem("Compressor", Paths.ITEM_COMPRESSOR, Paths.ITEM_COMPRESSOR, Paths.SPRITE_COMPRESSOR_SPRITESHEET, Paths.SPRITE_COMPRESSOR_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Compresses stuff. Try different things for interesting results?", 1750, EntityType.COMPRESSOR, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(MUSHBOX = new PlaceableItem("Mushbox", Paths.ITEM_MUSHBOX, Paths.ITEM_MUSHBOX, Paths.SPRITE_MUSHBOX_SPRITESHEET, Paths.SPRITE_MUSHBOX_SPRITESHEET, 1, 1, DEFAULT_STACK_SIZE, "Mushbox + Spores = Mushrooms!", 600, EntityType.MUSH_BOX, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SOULCHEST = new PlaceableItem("Soulchest", Paths.ITEM_SOULCHEST, Paths.ITEM_SOULCHEST, Paths.SPRITE_SOULCHEST_SPRITESHEET, Paths.SPRITE_SOULCHEST_SPRITESHEET, 2, 1, DEFAULT_STACK_SIZE, "A resonant chest. All soulchests share the same contents, no matter where they're placed.", 5500, EntityType.SOULCHEST, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FLOWERBED = new PlaceableItem("Flowerbed", Paths.ITEM_FLOWERBED, Paths.ITEM_FLOWERBED, Paths.SPRITE_FLOWERBED_SPRITESHEET, Paths.SPRITE_FLOWERBED_SPRITESHEET, 3, 2, DEFAULT_STACK_SIZE, "Specially designed to grow flowers. Place any flower in here and it'll eventually sprout into many more.", 500, EntityType.FLOWERBED, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GLASSBLOWER = new PlaceableItem("Glassblower", Paths.ITEM_GLASSBLOWER, Paths.ITEM_GLASSBLOWER, Paths.SPRITE_GLASSBLOWER_SPRITESHEET, Paths.SPRITE_GLASSBLOWER_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Sucks up sand and blows it into glass. Place this on sandy terrain.", 1000, EntityType.GLASSBLOWER, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(AQUARIUM = new PlaceableItem("Aquarium", Paths.ITEM_AQUARIUM, Paths.ITEM_AQUARIUM, Paths.SPRITE_AQUARIUM_SPRITESHEET, Paths.SPRITE_AQUARIUM_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "A luxury fishbowl. Add two fish of the same kind and they'll start producing more.", 1500, EntityType.AQUARIUM, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ALCHEMY_CAULDRON = new PlaceableItem("Alchemy Cauldron", Paths.ITEM_ALCHEMY_CAULDRON, Paths.ITEM_ALCHEMY_CAULDRON, Paths.SPRITE_ALCHEMY_CAULDRON_SPRITESHEET, Paths.SPRITE_ALCHEMY_CAULDRON_SPRITESHEET, 2, 1, DEFAULT_STACK_SIZE, "This blackened cauldron can be used to create all sorts of mythical concoctions...", 1600, EntityType.ALCHEMY_CAULDRON, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ANVIL = new PlaceableItem("Anvil", Paths.ITEM_ANVIL, Paths.ITEM_ANVIL, Paths.SPRITE_ANVIL_SPRITESHEET, Paths.SPRITE_ANVIL_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "Used to hammer bars into metal goods. It's also a very, very good paperweight.", 3200, EntityType.ANVIL, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(KEG = new PlaceableItem("Keg", Paths.ITEM_KEG, Paths.ITEM_KEG, Paths.SPRITE_KEG_SPRITESHEET, Paths.SPRITE_KEG_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Used to turn fruit into alcohol and pickle some vegetables. Unfortunately lacks a suitable spout to drink directly from.", 1600, EntityType.KEG, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SKY_STATUE = new PlaceableItem("Sky Statue", Paths.ITEM_SKY_STATUE, Paths.ITEM_SKY_STATUE, Paths.SPRITE_SKY_STATUE_SPRITESHEET, Paths.SPRITE_SKY_STATUE_SPRITESHEET, 2, 3, DEFAULT_STACK_SIZE, "This mythical statue allows you to teleport to other Sky Statues in the same area.", 2200, EntityType.SKY_STATUE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DRACONIC_PILLAR = new PlaceableItem("Draconic Pillar", Paths.ITEM_DRACONIC_PILLAR, Paths.ITEM_DRACONIC_PILLAR, Paths.SPRITE_DRACONIC_PILLAR_SPRITESHEET, Paths.SPRITE_DRACONIC_PILLAR_SPRITESHEET, 2, 4, DEFAULT_STACK_SIZE, "An imposing structure. You can warp between placed Draconic Pillars regardless of distance.", 4800, EntityType.DRACONIC_PILLAR, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WORKBENCH = new PlaceableItem("Workbench", Paths.ITEM_WORKBENCH, Paths.ITEM_WORKBENCH, Paths.SPRITE_WORKBENCH_SPRITESHEET, Paths.SPRITE_WORKBENCH_SPRITESHEET, 3, 2, DEFAULT_STACK_SIZE, "The built in saw can be used to turn wood into planks or stone into bricks. You can even turn iron into gears!", 1100, EntityType.WORKBENCH, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(VIVARIUM = new PlaceableItem("Vivarium", Paths.ITEM_VIVARIUM, Paths.ITEM_VIVARIUM, Paths.SPRITE_VIVARIUM_SPRITESHEET, Paths.SPRITE_VIVARIUM_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "This miniature greenhouse can be used to grow small quantities of any plant or forage, even out of season.", 1100, EntityType.VIVARIUM, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SYNTHESIZER = new PlaceableItem("Synthesizer", Paths.ITEM_SYNTHESIZER, Paths.ITEM_SYNTHESIZER, Paths.SPRITE_SYNTHESIZER_SPRITESHEET, Paths.SPRITE_SYNTHESIZER_SPRITESHEET, 2, 3, DEFAULT_STACK_SIZE, "This strange machine can synthesize random objects from thin air!\nThe outcome is deterministic and based on the Synthesizer's placement.", 4300, EntityType.SYNTHESIZER, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTERS_PRESS = new PlaceableItem("Painter's Press", Paths.ITEM_PAINTERS_PRESS, Paths.ITEM_PAINTERS_PRESS, Paths.SPRITE_PAINTERS_PRESS_SPRITESHEET, Paths.SPRITE_PAINTERS_PRESS_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Put something colorful in and it will be compressed into a dye used to color clothing and furniture. Splat!", 460, EntityType.PAINTERS_PRESS, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(JEWELERS_BENCH = new PlaceableItem("Jeweler's Bench", Paths.ITEM_JEWELERS_BENCH, Paths.ITEM_JEWELERS_BENCH, Paths.SPRITE_JEWELERS_BENCH_SPRITESHEET, Paths.SPRITE_JEWELERS_BENCH_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "This table has the tools required to craft any style of accessory. From rings to necklaces, now you can have it all.", 1100, EntityType.JEWELERS_BENCH, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BARBER_POLE = new PlaceableItem("Barber Pole", Paths.ITEM_BARBER_POLE, Paths.ITEM_BARBER_POLE, Paths.SPRITE_BARBER_POLE_SPRITESHEET, Paths.SPRITE_BARBER_POLE_SPRITESHEET, 1, 3, DEFAULT_STACK_SIZE, "Time for a haircut? This barber's pole allows you to change your hair style or color freely. Just be careful with the scissors!", 1350, EntityType.BARBER_POLE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ENCHANTED_VANITY = new PlaceableItem("Enchanted Vanity", Paths.ITEM_ENCHANTED_VANITY, Paths.ITEM_ENCHANTED_VANITY, Paths.SPRITE_ENCHANTED_VANITY_SPRITESHEET, Paths.SPRITE_ENCHANTED_VANITY_SPRITESHEET, 2, 3, DEFAULT_STACK_SIZE, "This magical vanity can change aspects of your physical appearance in an instant. Nobody knows how it works, and investigations have found no scientific explaination.", 2250, EntityType.ENCHANTED_VANITY, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DRYING_RACK = new PlaceableItem("Drying Rack", Paths.ITEM_DRYING_RACK, Paths.ITEM_DRYING_RACK, Paths.SPRITE_DRYING_RACK_SPRITESHEET, Paths.SPRITE_DRYING_RACK_SPRITESHEET, 3, 3, DEFAULT_STACK_SIZE, "A traditional wooden drying rack. Utilize the power of the sun to dry fish, meat, and more!", 750, EntityType.DRYING_RACK, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLONING_MACHINE = new PlaceableItem("Cloning Machine", Paths.ITEM_CLONING_MACHINE, Paths.ITEM_CLONING_MACHINE, Paths.SPRITE_CLONING_MACHINE_SPRITESHEET, Paths.SPRITE_CLONING_MACHINE_SPRITESHEET, 3, 2, DEFAULT_STACK_SIZE, "The pinnacle of technological innovation! Finally, a working cloning machine! (The creators of CLONING MACHINE (tm) take no responsibility for clone related mishaps)", 10300, EntityType.CLONING_MACHINE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ORIGAMI_STATION = new PlaceableItem("Origami Station", Paths.ITEM_ORIGAMI_STATION, Paths.ITEM_ORIGAMI_STATION, Paths.SPRITE_ORIGAMI_STATION_SPRITESHEET, Paths.SPRITE_ORIGAMI_STATION_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "A beautiful table for beautiful papercrafts. Combine a piece of paper and some dye to get started.", 1200, EntityType.ORIGAMI_STATION, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(RECYCLER = new PlaceableItem("Recycler", Paths.ITEM_RECYCLER, Paths.ITEM_RECYCLER, Paths.SPRITE_RECYCLER_SPRITESHEET, Paths.SPRITE_RECYCLER_SPRITESHEET, 2, 2, DEFAULT_STACK_SIZE, "This machine can decompose anything into its base components. Even cooked meals can be broken down into their parts using this!", 1600, EntityType.RECYCLER, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ALCHEMIZER = new PlaceableItem("Alchemizer", Paths.ITEM_ALCHEMIZER, Paths.ITEM_ALCHEMIZER, Paths.SPRITE_ALCHEMIZER_SPRITESHEET, Paths.SPRITE_ALCHEMIZER_SPRITESHEET, 4, 2, DEFAULT_STACK_SIZE, "Put something in this cauldron and it will miraculously become gold!", 9700, EntityType.ALCHEMIZER, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(EXTRACTOR = new PlaceableItem("Extractor", Paths.ITEM_EXTRACTOR, Paths.ITEM_EXTRACTOR, Paths.SPRITE_EXTRACTOR_SPRITESHEET, Paths.SPRITE_EXTRACTOR_SPRITESHEET, 2, 4, DEFAULT_STACK_SIZE, "This autonomous mining probe can excavate minerals and ore from deep below the ground.\nPlace it and fuel it with some Coal to get started.", 1600, EntityType.EXTRACTOR, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(TOTEM_OF_THE_CHICKEN = new PlaceableItem("Totem of the Chicken", Paths.ITEM_TOTEM_OF_THE_CHICKEN, Paths.ITEM_TOTEM_OF_THE_CHICKEN, Paths.SPRITE_TOTEM_OF_THE_CHICKEN_SPRITESHEET, Paths.SPRITE_TOTEM_OF_THE_CHICKEN_SPRITESHEET, 1, 4, DEFAULT_STACK_SIZE, "Chickens love this totem. Place it on your farm and chickens will start appearing the next day!", 4000, EntityType.TOTEM_CHICKEN, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOTEM_OF_THE_COW = new PlaceableItem("Totem of the Cow", Paths.ITEM_TOTEM_OF_THE_COW, Paths.ITEM_TOTEM_OF_THE_COW, Paths.SPRITE_TOTEM_OF_THE_COW_SPRITESHEET, Paths.SPRITE_TOTEM_OF_THE_COW_SPRITESHEET, 1, 4, DEFAULT_STACK_SIZE, "Bovines love this totem. Place it on your farm and cows will start appearing the next day!", 4500, EntityType.TOTEM_COW, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOTEM_OF_THE_PIG = new PlaceableItem("Totem of the Pig", Paths.ITEM_TOTEM_OF_THE_PIG, Paths.ITEM_TOTEM_OF_THE_PIG, Paths.SPRITE_TOTEM_OF_THE_PIG_SPRITESHEET, Paths.SPRITE_TOTEM_OF_THE_PIG_SPRITESHEET, 1, 4, DEFAULT_STACK_SIZE, "Pigs love this totem. Place it on your farm and pigs will start appearing the next day!", 4300, EntityType.TOTEM_PIG, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOTEM_OF_THE_SHEEP = new PlaceableItem("Totem of the Sheep", Paths.ITEM_TOTEM_OF_THE_SHEEP, Paths.ITEM_TOTEM_OF_THE_SHEEP, Paths.SPRITE_TOTEM_OF_THE_SHEEP_SPRITESHEET, Paths.SPRITE_TOTEM_OF_THE_SHEEP_SPRITESHEET, 1, 4, DEFAULT_STACK_SIZE, "Sheep love this totem. Place it on your farm and sheep will start appearing the next day!", 4600, EntityType.TOTEM_SHEEP, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOTEM_OF_THE_DOG = new PlaceableItem("Totem of the Dog", Paths.ITEM_TOTEM_OF_THE_DOG, Paths.ITEM_TOTEM_OF_THE_DOG, Paths.SPRITE_TOTEM_OF_THE_DOG_SPRITESHEET, Paths.SPRITE_TOTEM_OF_THE_DOG_SPRITESHEET, 1, 4, DEFAULT_STACK_SIZE, "Dogs love this totem. Place it on your farm and friends will start appearing the next day! The dog will help you with farmwork.", 5800, EntityType.TOTEM_DOG, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOTEM_OF_THE_CAT = new PlaceableItem("Totem of the Cat", Paths.ITEM_TOTEM_OF_THE_CAT, Paths.ITEM_TOTEM_OF_THE_CAT, Paths.SPRITE_TOTEM_OF_THE_CAT_SPRITESHEET, Paths.SPRITE_TOTEM_OF_THE_CAT_SPRITESHEET, 1, 4, DEFAULT_STACK_SIZE, "Cats love this totem. Place it on your farm and friends will start appearing the next day! The cat will help you with farmwork.", 5200, EntityType.TOTEM_CAT, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOTEM_OF_THE_ROOSTER = new PlaceableItem("Totem of the Rooster", Paths.ITEM_TOTEM_OF_THE_ROOSTER, Paths.ITEM_TOTEM_OF_THE_ROOSTER, Paths.SPRITE_TOTEM_OF_THE_ROOSTER_SPRITESHEET, Paths.SPRITE_TOTEM_OF_THE_ROOSTER_SPRITESHEET, 1, 4, DEFAULT_STACK_SIZE, "Roosters love this totem. Place it on your farm and you'll have a natural alarm clock!", 4600, EntityType.TOTEM_ROOSTER, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(CONCRETE_FLOOR = new PlaceableItem("Concrete Floor", Paths.ITEM_FLOOR_CONCRETE, Paths.ITEM_FLOOR_CONCRETE, Paths.SPRITE_FLOOR_CONCRETE, Paths.SPRITE_FLOOR_CONCRETE, 1,1, DEFAULT_STACK_SIZE, "Basic floor made of concrete.", 65, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STREET_FLOOR = new PlaceableItem("Street Floor", Paths.ITEM_FLOOR_STREET, Paths.ITEM_FLOOR_STREET, Paths.SPRITE_FLOOR_STREET, Paths.SPRITE_FLOOR_STREET, 1,1, DEFAULT_STACK_SIZE, "But where're the cars?", 60, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CARPET_FLOOR = new PlaceableItem("Carpet Floor", Paths.ITEM_FLOOR_CARPET, Paths.ITEM_FLOOR_CARPET, Paths.SPRITE_FLOOR_CARPET, Paths.SPRITE_FLOOR_CARPET, 1,1, DEFAULT_STACK_SIZE, "Some carpet for your floors. Try dying it!", 60, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BOARDWALK_FLOOR = new PlaceableItem("Boardwalk Floor", Paths.ITEM_FLOOR_BOARDWALK, Paths.ITEM_FLOOR_BOARDWALK, Paths.SPRITE_FLOOR_BOARDWALK, Paths.SPRITE_FLOOR_BOARDWALK, 1,1, DEFAULT_STACK_SIZE, "Not just for the beach.", 20, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STEPPING_STONE_FLOOR = new PlaceableItem("Stepping Stone Floor", Paths.ITEM_FLOOR_STEPPING_STONE, Paths.ITEM_FLOOR_STEPPING_STONE, Paths.SPRITE_FLOOR_STEPPING_STONE, Paths.SPRITE_FLOOR_STEPPING_STONE, 1,1, DEFAULT_STACK_SIZE, "Commonly used to decorate gardens.", 25, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_FLOOR = new PlaceableItem("Wooden Floor", Paths.ITEM_FLOOR_WOODEN, Paths.ITEM_FLOOR_WOODEN, Paths.SPRITE_FLOOR_WOODEN, Paths.SPRITE_FLOOR_WOODEN, 1, 1, DEFAULT_STACK_SIZE, "Simple flooring made from wood.", 30, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TATAMI_FLOOR = new PlaceableItem("Tatami Floor", Paths.ITEM_FLOOR_TATAMI, Paths.ITEM_FLOOR_TATAMI, Paths.SPRITE_FLOOR_TATAMI, Paths.SPRITE_FLOOR_TATAMI, 1, 1, DEFAULT_STACK_SIZE, "A flooring mat woven from bamboo.", 20, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(THIN_TATAMI_FLOOR = new PlaceableItem("Thin Tatami Floor", Paths.ITEM_FLOOR_THIN_TATAMI, Paths.ITEM_FLOOR_THIN_TATAMI, Paths.SPRITE_FLOOR_THIN_TATAMI, Paths.SPRITE_FLOOR_THIN_TATAMI, 1, 1, DEFAULT_STACK_SIZE, "Like normal tatami, but thin.", 15, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(MAT_FLOOR = new PlaceableItem("Mat Floor", Paths.ITEM_FLOOR_MAT, Paths.ITEM_FLOOR_MAT, Paths.SPRITE_FLOOR_MAT, Paths.SPRITE_FLOOR_MAT, 1, 1, DEFAULT_STACK_SIZE, "A thin cloth mat that can be used to decorate floors.", 75, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FOOTPRINT_FLOOR = new PlaceableItem("Footprint Floor", Paths.ITEM_FLOOR_FOOTPRINT, Paths.ITEM_FLOOR_FOOTPRINT, Paths.SPRITE_FLOOR_FOOTPRINT, Paths.SPRITE_FLOOR_FOOTPRINT, 1, 1, DEFAULT_STACK_SIZE, "Oops...", 5, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TRIANGULATE_FLOOR = new PlaceableItem("Triangulate Floor", Paths.ITEM_FLOOR_TRIANGULATE, Paths.ITEM_FLOOR_TRIANGULATE, Paths.SPRITE_FLOOR_TRIANGULATE, Paths.SPRITE_FLOOR_TRIANGULATE, 1, 1, DEFAULT_STACK_SIZE, "A strange flooring pattern, perhaps used to signal to aliens?", 75, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SQUARE_FLOOR = new PlaceableItem("Square Floor", Paths.ITEM_FLOOR_SQUARE, Paths.ITEM_FLOOR_SQUARE, Paths.SPRITE_FLOOR_SQUARE, Paths.SPRITE_FLOOR_SQUARE, 1, 1, DEFAULT_STACK_SIZE, "Fancy stepping stones roughly cut into square parts.", 50, EntityType.FLOOR_DECOR, PlaceableItem.PlacementType.FLOOR, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(MYTHRIL_FENCE = new PlaceableItem("Mythril Fence", Paths.ITEM_MYTHRIL_FENCE, Paths.ITEM_MYTHRIL_FENCE, Paths.SPRITE_MYTHRIL_FENCE_SPRITESHEET, Paths.SPRITE_MYTHRIL_FENCE_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Quintessential fencing made of mythril. Place fences adjacent to each other for best results.", 
                100, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(METAL_FENCE = new PlaceableItem("Metal Fence", Paths.ITEM_METAL_FENCE, Paths.ITEM_METAL_FENCE, Paths.SPRITE_METAL_FENCE_SPRITESHEET, Paths.SPRITE_METAL_FENCE_SPRITESHEET, 1, 1, DEFAULT_STACK_SIZE, "Inelastic fencing made of iron. If you place them side by side they'll expand.",
                60, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STONE_FENCE = new PlaceableItem("Stone Fence", Paths.ITEM_STONE_FENCE, Paths.ITEM_STONE_FENCE, Paths.SPRITE_STONE_FENCE_SPRITESHEET, Paths.SPRITE_STONE_FENCE_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Gorgonic fencing made of stone. Good fences make good neighbors.",
                20, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_FENCE = new PlaceableItem("Wooden Fence", Paths.ITEM_WOODEN_FENCE, Paths.ITEM_WOODEN_FENCE, Paths.SPRITE_WOODEN_FENCE_SPRITESHEET, Paths.SPRITE_WOODEN_FENCE_SPRITESHEET, 1, 1, DEFAULT_STACK_SIZE, "Rural fencing made of wood. Place multiple!",
                15, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TALL_FENCE = new PlaceableItem("Tall Fence", Paths.ITEM_TALL_FENCE, Paths.ITEM_TALL_FENCE, Paths.SPRITE_TALL_FENCE_SPRITESHEET, Paths.SPRITE_TALL_FENCE_SPRITESHEET, 1, 3, DEFAULT_STACK_SIZE, "Towering fencing made of wood. Privacy restored!",
                25, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GOLDEN_FENCE = new PlaceableItem("Golden Fence", Paths.ITEM_GOLDEN_FENCE, Paths.ITEM_GOLDEN_FENCE, Paths.SPRITE_GOLDEN_FENCE_SPRITESHEET, Paths.SPRITE_GOLDEN_FENCE_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Midasian fencing made of gold. A bit gaudy, perhaps?",
                120, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BAMBOO_FENCE = new PlaceableItem("Bamboo Fence", Paths.ITEM_BAMBOO_FENCE, Paths.ITEM_BAMBOO_FENCE, Paths.SPRITE_BAMBOO_FENCE_SPRITESHEET, Paths.SPRITE_BAMBOO_FENCE_SPRITESHEET, 1, 2, DEFAULT_STACK_SIZE, "Grassy fencing made of bamboo. Provides a feeling of far away places.",
                25, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GLASS_FENCE = new PlaceableItem("Glass Fence", Paths.ITEM_GLASS_FENCE, Paths.ITEM_GLASS_FENCE, Paths.SPRITE_GLASS_FENCE_SPRITESHEET, Paths.SPRITE_GLASS_FENCE_SPRITESHEET, 1, 1, DEFAULT_STACK_SIZE, "Brawny fencing made of glass. Unyielding, unbreakable, and unstoppable.",
                30, EntityType.FENCE, PlaceableItem.PlacementType.NORMAL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(WALL_MIRROR = new PlaceableItem("Wall Mirror", Paths.ITEM_WALL_MIRROR, Paths.ITEM_WALL_MIRROR, Paths.SPRITE_WALL_MIRROR_SPRITESHEET, Paths.SPRITE_WALL_MIRROR_SPRITESHEET, 2, 3, DEFAULT_STACK_SIZE, "A nice mirror really spices up otherwise boring walls.", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(HORSESHOE = new PlaceableItem("Horseshoe", Paths.ITEM_HORSESHOE, Paths.ITEM_HORSESHOE, Paths.SPRITE_HORSESHOE_SPRITESHEET, Paths.SPRITE_HORSESHOE_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "Hooves up!", 1600, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HERALDIC_SHIELD = new PlaceableItem("Heraldic Shield", Paths.ITEM_HERALDIC_SHIELD, Paths.ITEM_HERALDIC_SHIELD, Paths.SPRITE_HERALDIC_SHIELD_SPRITESHEET, Paths.SPRITE_HERALDIC_SHIELD_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "Shields up!", 3500, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DECORATIVE_SWORD = new PlaceableItem("Decorative Sword", Paths.ITEM_DECORATIVE_SWORD, Paths.ITEM_DECORATIVE_SWORD, Paths.SPRITE_DECORATIVE_SWORD_SPRITESHEET, Paths.SPRITE_DECORATIVE_SWORD_SPRITESHEET,
                3, 1, DEFAULT_STACK_SIZE, "Swords up!", 4100, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SUIT_OF_ARMOR = new PlaceableItem("Suit of Armor", Paths.ITEM_SUIT_OF_ARMOR, Paths.ITEM_SUIT_OF_ARMOR, Paths.SPRITE_SUIT_OF_ARMOR_SPRITESHEET, Paths.SPRITE_SUIT_OF_ARMOR_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "Armor up!", 12800, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(ANATOMICAL_POSTER = new PlaceableItem("Anatomical Poster", Paths.ITEM_ANATOMICAL_POSTER, Paths.ITEM_ANATOMICAL_POSTER, Paths.SPRITE_ANATOMICAL_POSTER_SPRITESHEET, Paths.SPRITE_ANATOMICAL_POSTER_SPRITESHEET, 
                2, 4, DEFAULT_STACK_SIZE, "Something you might find in a doctor's office. Probably a bit weird to hang in your house though.", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BAMBOO_POT = new PlaceableItem("Bamboo Pot", Paths.ITEM_BAMBOO_POT, Paths.ITEM_BAMBOO_POT, Paths.SPRITE_BAMBOO_POT_SPRITESHEET, Paths.SPRITE_BAMBOO_POT_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "It adds a slight eastern touch to any room.", 195, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BANNER = new PlaceableItem("Banner", Paths.ITEM_BANNER, Paths.ITEM_BANNER, Paths.SPRITE_BANNER_SPRITESHEET, Paths.SPRITE_BANNER_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "A nice cloth banner to hang on the wall.", 750, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BELL = new PlaceableItem("Bell", Paths.ITEM_BELL, Paths.ITEM_BELL, Paths.SPRITE_BELL_SPRITESHEET, Paths.SPRITE_BELL_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "Ring a ding! (Alternatively: School is in session!)", 700, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLACKBOARD = new PlaceableItem("Blackboard", Paths.ITEM_BLACKBOARD, Paths.ITEM_BLACKBOARD, Paths.SPRITE_BLACKBOARD_SPRITESHEET, Paths.SPRITE_BLACKBOARD_SPRITESHEET,
                4, 3, DEFAULT_STACK_SIZE, "Whiteboard's old-fashioned aunt. Useless without chalk.", 375, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BOOMBOX = new PlaceableItem("Boombox", Paths.ITEM_BOOMBOX, Paths.ITEM_BOOMBOX, Paths.SPRITE_BOOMBOX_SPRITESHEET, Paths.SPRITE_BOOMBOX_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "Kaboom-box!", 650, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BOX = new PlaceableItem("Box", Paths.ITEM_BOX, Paths.ITEM_BOX, Paths.SPRITE_BOX_SPRITESHEET, Paths.SPRITE_BOX_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "It looks like cardboard, but it's actually made of wood.", 140, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BRAZIER = new PlaceableItem("Brazier", Paths.ITEM_BRAZIER, Paths.ITEM_BRAZIER, Paths.SPRITE_BRAZIER_SPRITESHEET, Paths.SPRITE_BRAZIER_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "A ritualistic brazier. Or perhaps medieval?", 260, EntityType.LIGHT_DECOR_ANIM, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BUOY = new PlaceableItem("Buoy", Paths.ITEM_BUOY, Paths.ITEM_BUOY, Paths.SPRITE_BUOY_SPRITESHEET, Paths.SPRITE_BUOY_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "A wooden buoy. It reminds you of the beach.", 180, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CAMPFIRE = new PlaceableItem("Campfire", Paths.ITEM_CAMPFIRE, Paths.ITEM_CAMPFIRE, Paths.SPRITE_CAMPFIRE_SPRITESHEET, Paths.SPRITE_CAMPFIRE_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "Let's gather 'round the campfire, and sing our campfire song. Our C A M P F I R E S O N G song!", 180, EntityType.LIGHT_DECOR_ANIM, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CANVAS = new PlaceableItem("Canvas", Paths.ITEM_CANVAS, Paths.ITEM_CANVAS, Paths.SPRITE_CANVAS_SPRITESHEET, Paths.SPRITE_CANVAS_SPRITESHEET,
                2, 3, DEFAULT_STACK_SIZE, "The blankest slate of any artist. The possibilities are limitless.", 300, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CART = new PlaceableItem("Cart", Paths.ITEM_CART, Paths.ITEM_CART, Paths.SPRITE_CART_SPRITESHEET, Paths.SPRITE_CART_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "Not the kind used for racing, sadly.", 400, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLAY_BALL = new PlaceableItem("Clay Ball", Paths.ITEM_CLAY_BALL, Paths.ITEM_CLAY_BALL, Paths.SPRITE_CLAY_BALL_SPRITESHEET, Paths.SPRITE_CLAY_BALL_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "It's literally a hardened ball of clay. The most elementary pottery you can make.", 115, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLAY_BOWL = new PlaceableItem("Clay Bowl", Paths.ITEM_CLAY_BOWL, Paths.ITEM_CLAY_BOWL, Paths.SPRITE_CLAY_BOWL_SPRITESHEET, Paths.SPRITE_CLAY_BOWL_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "A nice clay bowl. Masterfully crafted.", 270, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLAY_DOLL = new PlaceableItem("Clay Doll", Paths.ITEM_CLAY_DOLL, Paths.ITEM_CLAY_DOLL, Paths.SPRITE_CLAY_DOLL_SPRITESHEET, Paths.SPRITE_CLAY_DOLL_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "Evolved from a ball toy.", 235, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLAY_SLATE = new PlaceableItem("Clay Slate", Paths.ITEM_CLAY_SLATE, Paths.ITEM_CLAY_SLATE, Paths.SPRITE_CLAY_SLATE_SPRITESHEET, Paths.SPRITE_CLAY_SLATE_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "Definitely doesn\'t resemble a tombstone in any way.", 315, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLOCK = new PlaceableItem("Clock", Paths.ITEM_CLOCK, Paths.ITEM_CLOCK, Paths.SPRITE_CLOCK_SPRITESHEET, Paths.SPRITE_CLOCK_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "Now it's time!", 1400, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CLOTHESLINE = new PlaceableItem("Clothesline", Paths.ITEM_CLOTHESLINE, Paths.ITEM_CLOTHESLINE, Paths.SPRITE_CLOTHESLINE_SPRITESHEET, Paths.SPRITE_CLOTHESLINE_SPRITESHEET,
                3, 2, DEFAULT_STACK_SIZE, "Either a type of punch OR a nature-assisted dryer.", 1050, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CRATE = new PlaceableItem("Crate", Paths.ITEM_CRATE, Paths.ITEM_CRATE, Paths.SPRITE_CRATE_SPRITESHEET, Paths.SPRITE_CRATE_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "It's crate!", 280, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CUBE_STATUE = new PlaceableItem("Cube Statue", Paths.ITEM_CUBE_STATUE, Paths.ITEM_CUBE_STATUE, Paths.SPRITE_CUBE_STATUE_SPRITESHEET, Paths.SPRITE_CUBE_STATUE_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "An abstract statue of a cube. It is actually a reference to the cubical nature of humanity's suffering.", 900, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(CYMBAL = new PlaceableItem("Cymbal", Paths.ITEM_CYMBAL, Paths.ITEM_CYMBAL, Paths.SPRITE_CYMBAL_SPRITESHEET, Paths.SPRITE_CYMBAL_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "You could also call it a gong.", 1000, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DECORATIVE_BOULDER = new PlaceableItem("Decorative Boulder", Paths.ITEM_DECORATIVE_BOULDER, Paths.ITEM_DECORATIVE_BOULDER, Paths.SPRITE_DECORATIVE_BOULDER_SPRITESHEET, Paths.SPRITE_DECORATIVE_BOULDER_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "The boulder is artificial. The middle is hollow, so it's much easier to carry than normal rocks..", 400, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DECORATIVE_LOG = new PlaceableItem("Decorative Log", Paths.ITEM_DECORATIVE_LOG, Paths.ITEM_DECORATIVE_LOG, Paths.SPRITE_DECORATIVE_LOG_SPRITESHEET, Paths.SPRITE_DECORATIVE_LOG_SPRITESHEET,
                3, 1, DEFAULT_STACK_SIZE, "This log is artificial. It's much lighter than a normal log.", 500, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DRUM = new PlaceableItem("Drum", Paths.ITEM_DRUM, Paths.ITEM_DRUM, Paths.SPRITE_DRUM_SPRITESHEET, Paths.SPRITE_DRUM_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "A tribal standing drum. Perhaps not as versatile as a drumset.", 450, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FIRE_HYDRANT = new PlaceableItem("Fire Hydrant", Paths.ITEM_FIRE_HYDRANT, Paths.ITEM_FIRE_HYDRANT, Paths.SPRITE_FIRE_HYDRANT_SPRITESHEET, Paths.SPRITE_FIRE_HYDRANT_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "It isn't actually connected to a source of water.", 1200, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FIREPIT = new PlaceableItem("Firepit", Paths.ITEM_FIREPIT, Paths.ITEM_FIREPIT, Paths.SPRITE_FIREPIT_SPRITESHEET, Paths.SPRITE_FIREPIT_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "A simple campfire surrounded by stones.", 180, EntityType.LIGHT_DECOR_ANIM, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FIREPLACE = new PlaceableItem("Fireplace", Paths.ITEM_FIREPLACE, Paths.ITEM_FIREPLACE, Paths.SPRITE_FIREPLACE_SPRITESHEET, Paths.SPRITE_FIREPLACE_SPRITESHEET,
                3, 2, DEFAULT_STACK_SIZE, "This is a good place to light fires.", 2150, EntityType.LIGHT_DECOR_ANIM, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FLAGPOLE = new PlaceableItem("Flagpole", Paths.ITEM_FLAGPOLE, Paths.ITEM_FLAGPOLE, Paths.SPRITE_FLAGPOLE_SPRITESHEET, Paths.SPRITE_FLAGPOLE_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "Strangely, the flag seems to blow in the wind even in places without wind. How enigmatic!", 950, EntityType.ANIMATED_DECOR_6F, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FROST_SCULPTURE = new PlaceableItem("Frost Sculpture", Paths.ITEM_FROST_SCULPTURE, Paths.ITEM_FROST_SCULPTURE, Paths.SPRITE_FROST_SCULPTURE_SPRITESHEET, Paths.SPRITE_FROST_SCULPTURE_SPRITESHEET,
                2, 3, DEFAULT_STACK_SIZE, "The structure of this sculpture is beyond the capabilities of normal ice.", 1650, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(FULL_THROTTLE_GRAFFITI = new PlaceableItem("Full Throttle Graffiti", Paths.ITEM_FULL_THROTTLE_GRAFFITI, Paths.ITEM_FULL_THROTTLE_GRAFFITI, Paths.SPRITE_FULL_THROTTLE_GRAFFITI_SPRITESHEET, Paths.SPRITE_FULL_THROTTLE_GRAFFITI_SPRITESHEET,
                3, 1, DEFAULT_STACK_SIZE, "As the saying goes: you only live a single time.", 1100, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GARDEN_ARCH = new PlaceableItem("Garden Arch", Paths.ITEM_GARDEN_ARCH, Paths.ITEM_GARDEN_ARCH, Paths.SPRITE_GARDEN_ARCH_SPRITESHEET, Paths.SPRITE_GARDEN_ARCH_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "A decorative arch commonly found in gardens. Also a frequent guest at weddings.", 520, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GRANDFATHER_CLOCK = new PlaceableItem("Grandfather Clock", Paths.ITEM_GRANDFATHER_CLOCK, Paths.ITEM_GRANDFATHER_CLOCK, Paths.SPRITE_GRANDFATHER_CLOCK_SPRITESHEET, Paths.SPRITE_GRANDFATHER_CLOCK_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "Elderly timepiece. It's seen much in its time.", 2000, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GUITAR_PLACEABLE = new PlaceableItem("Guitar Stand", Paths.ITEM_GUITAR_PLACEABLE, Paths.ITEM_GUITAR_PLACEABLE, Paths.SPRITE_GUITAR_PLACEABLE_SPRITESHEET, Paths.SPRITE_GUITAR_PLACEABLE_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "A quality acoustic guitar. Whether campfire or concert, it's the star of the show.", 1400, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(GYM_BENCH = new PlaceableItem("Gym Bench", Paths.ITEM_GYM_BENCH, Paths.ITEM_GYM_BENCH, Paths.SPRITE_GYM_BENCH_SPRITESHEET, Paths.SPRITE_GYM_BENCH_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "The kind of metal bench one might find in a locker room.", 600, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HAMMOCK = new PlaceableItem("Hammock", Paths.ITEM_HAMMOCK, Paths.ITEM_HAMMOCK, Paths.SPRITE_HAMMOCK_SPRITESHEET, Paths.SPRITE_HAMMOCK_SPRITESHEET,
                4, 2, DEFAULT_STACK_SIZE, "Maybe a little nap wouldn't hurt...", 1250, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HAYBALE = new PlaceableItem("Haybale", Paths.ITEM_HAYBALE, Paths.ITEM_HAYBALE, Paths.SPRITE_HAYBALE_SPRITESHEET, Paths.SPRITE_HAYBALE_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "Found on any respectable farm.", 200, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HEARTBREAK_GRAFFITI = new PlaceableItem("Heartbreak Graffiti", Paths.ITEM_HEARTBREAK_GRAFFITI, Paths.ITEM_HEARTBREAK_GRAFFITI, Paths.SPRITE_HEARTBREAK_GRAFFITI_SPRITESHEET, Paths.SPRITE_HEARTBREAK_GRAFFITI_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "A heart united is now divided. Is all meaning faded forever?", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HELIX_POSTER = new PlaceableItem("Helix Poster", Paths.ITEM_HELIX_POSTER, Paths.ITEM_HELIX_POSTER, Paths.SPRITE_HELIX_POSTER_SPRITESHEET, Paths.SPRITE_HELIX_POSTER_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "It's a picture of deoxyribonucleic acid.", 1200, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HEROINE_GRAFFITI = new PlaceableItem("Heroine Graffiti", Paths.ITEM_HEROINE_GRAFFITI, Paths.ITEM_HEROINE_GRAFFITI, Paths.SPRITE_HEROINE_GRAFFITI_SPRITESHEET, Paths.SPRITE_HEROINE_GRAFFITI_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "This world needs more heroines.", 5600, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HORIZONTAL_MIRROR = new PlaceableItem("Horizontal Mirror", Paths.ITEM_HORIZONTAL_MIRROR, Paths.ITEM_HORIZONTAL_MIRROR, Paths.SPRITE_HORIZONTAL_MIRROR_SPRITESHEET, Paths.SPRITE_HORIZONTAL_MIRROR_SPRITESHEET,
                3, 2, DEFAULT_STACK_SIZE, "The mirror is hung horizontally.", 1300, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ICE_BLOCK = new PlaceableItem("Ice Block", Paths.ITEM_ICE_BLOCK, Paths.ITEM_ICE_BLOCK, Paths.SPRITE_ICE_BLOCK_SPRITESHEET, Paths.SPRITE_ICE_BLOCK_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "Secret: When your hero takes fatal damage, prevent it and become Immune this turn.", 150, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(IGLOO = new PlaceableItem("Igloo", Paths.ITEM_IGLOO, Paths.ITEM_IGLOO, Paths.SPRITE_IGLOO_SPRITESHEET, Paths.SPRITE_IGLOO_SPRITESHEET,
                5, 3, DEFAULT_STACK_SIZE, "A penguin's abode. The structure is reinforced with iron.", 900, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(LAMP = new PlaceableItem("Lamp", Paths.ITEM_LAMP, Paths.ITEM_LAMP, Paths.SPRITE_LAMP_SPRITESHEET, Paths.SPRITE_LAMP_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "An artificial and flawed representation of the sun.", 1300, EntityType.LIGHT_DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(LANTERN = new PlaceableItem("Lantern", Paths.ITEM_LANTERN, Paths.ITEM_LANTERN, Paths.SPRITE_LANTERN_SPRITESHEET, Paths.SPRITE_LANTERN_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "A quiet lantern made of stone. Despite being manmade, it blends well with nature.", 750, EntityType.LIGHT_DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(LATTICE = new PlaceableItem("Lattice", Paths.ITEM_LATTICE, Paths.ITEM_LATTICE, Paths.SPRITE_LATTICE_SPRITESHEET, Paths.SPRITE_LATTICE_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "Not to be confused for lettice.", 250, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(LEFTWARD_GRAFFITI = new PlaceableItem("Leftward Graffiti", Paths.ITEM_LEFTWARD_GRAFFITI, Paths.ITEM_LEFTWARD_GRAFFITI, Paths.SPRITE_LEFTWARD_GRAFFITI_SPRITESHEET, Paths.SPRITE_LEFTWARD_GRAFFITI_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "Move it to the left! Left! Left! Left!", 900, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(LIFEBUOY_SIGN = new PlaceableItem("Lifebuoy Sign", Paths.ITEM_LIFEBUOY_SIGN, Paths.ITEM_LIFEBUOY_SIGN, Paths.SPRITE_LIFEBUOY_SIGN_SPRITESHEET, Paths.SPRITE_LIFEBUOY_SIGN_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "Law-abiding citizen of the beach. The lifebuoy isn't functional on this one.", 850, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(LIGHTNING_ROD = new PlaceableItem("Lightning Rod", Paths.ITEM_LIGHTNING_ROD, Paths.ITEM_LIGHTNING_ROD, Paths.SPRITE_LIGHTNING_ROD_SPRITESHEET, Paths.SPRITE_LIGHTNING_ROD_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "It's alive!", 1600, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(MAILBOX = new PlaceableItem("Mailbox", Paths.ITEM_MAILBOX, Paths.ITEM_MAILBOX, Paths.SPRITE_MAILBOX_SPRITESHEET, Paths.SPRITE_MAILBOX_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "A box in which mail is placed.", 750, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(MARKET_STALL = new PlaceableItem("Market Stall", Paths.ITEM_MARKET_STALL, Paths.ITEM_MARKET_STALL, Paths.SPRITE_MARKET_STALL_SPRITESHEET, Paths.SPRITE_MARKET_STALL_SPRITESHEET,
                4, 4, DEFAULT_STACK_SIZE, "Pity the Nimbus Town farmer's market is already occupied.", 3250, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(MILK_JUG = new PlaceableItem("Milk Jug", Paths.ITEM_MILK_JUG, Paths.ITEM_MILK_JUG, Paths.SPRITE_MILK_JUG_SPRITESHEET, Paths.SPRITE_MILK_JUG_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "Moo!", 460, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(MINECART = new PlaceableItem("Minecart", Paths.ITEM_MINECART, Paths.ITEM_MINECART, Paths.SPRITE_MINECART_SPRITESHEET, Paths.SPRITE_MINECART_SPRITESHEET,
                3, 2, DEFAULT_STACK_SIZE, "Not very useful without powered rails.", 1400, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(NOIZEBOYZ_GRAFFITI = new PlaceableItem("Noizeboyz Graffiti", Paths.ITEM_NOIZEBOYZ_GRAFFITI, Paths.ITEM_NOIZEBOYZ_GRAFFITI, Paths.SPRITE_NOIZEBOYZ_GRAFFITI_SPRITESHEET, Paths.SPRITE_NOIZEBOYZ_GRAFFITI_SPRITESHEET,
                3, 2, DEFAULT_STACK_SIZE, "LET'Z\n   MAKE\n      ZOME\n         NOIZE!", 1300, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ORNATE_MIRROR = new PlaceableItem("Ornate Mirror", Paths.ITEM_ORNATE_MIRROR, Paths.ITEM_ORNATE_MIRROR, Paths.SPRITE_ORNATE_MIRROR_SPRITESHEET, Paths.SPRITE_ORNATE_MIRROR_SPRITESHEET,
                5, 4, DEFAULT_STACK_SIZE, "It's hard to believe it's over, isn't it? Funny how we get attached to the struggle.", 5200, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PET_BOWL = new PlaceableItem("Pet Bowl", Paths.ITEM_PET_BOWL, Paths.ITEM_PET_BOWL, Paths.SPRITE_PET_BOWL_SPRITESHEET, Paths.SPRITE_PET_BOWL_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "A little bowl for little Fido.", 560, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PIANO = new PlaceableItem("Piano", Paths.ITEM_PIANO, Paths.ITEM_PIANO, Paths.SPRITE_PIANO_SPRITESHEET, Paths.SPRITE_PIANO_SPRITESHEET,
                3, 3, DEFAULT_STACK_SIZE, "The most solemn of instruments.", 2950, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PILE_OF_BRICKS = new PlaceableItem("Pile of Bricks", Paths.ITEM_PILE_OF_BRICKS, Paths.ITEM_PILE_OF_BRICKS, Paths.SPRITE_PILE_OF_BRICKS_SPRITESHEET, Paths.SPRITE_PILE_OF_BRICKS_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "Made of clay. Ancient tradition dictates their characteristic red hue.", 250, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(POSTBOX = new PlaceableItem("Postbox", Paths.ITEM_POSTBOX, Paths.ITEM_POSTBOX, Paths.SPRITE_POSTBOX_SPRITESHEET, Paths.SPRITE_POSTBOX_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "POST is a method used to send data to the server.", 1800, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(POTTERY_JAR = new PlaceableItem("Pottery Jar", Paths.ITEM_POTTERY_JAR, Paths.ITEM_POTTERY_JAR, Paths.SPRITE_POTTERY_JAR_SPRITESHEET, Paths.SPRITE_POTTERY_JAR_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "Clay coffee archive.", 160, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(POTTERY_MUG = new PlaceableItem("Pottery Mug", Paths.ITEM_POTTERY_MUG, Paths.ITEM_POTTERY_MUG, Paths.SPRITE_POTTERY_MUG_SPRITESHEET, Paths.SPRITE_POTTERY_MUG_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "Some mugs are ugly, but not this one.", 130, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(POTTERY_PLATE = new PlaceableItem("Pottery Plate", Paths.ITEM_POTTERY_PLATE, Paths.ITEM_POTTERY_PLATE, Paths.SPRITE_POTTERY_PLATE_SPRITESHEET, Paths.SPRITE_POTTERY_PLATE_SPRITESHEET,
                1, 1, DEFAULT_STACK_SIZE, "A decorative plate. You could eat on it too, maybe.", 200, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(POTTERY_VASE = new PlaceableItem("Pottery Vase", Paths.ITEM_POTTERY_VASE, Paths.ITEM_POTTERY_VASE, Paths.SPRITE_POTTERY_VASE_SPRITESHEET, Paths.SPRITE_POTTERY_VASE_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "There's no rupees in here, so don't break it.", 360, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PYRAMID_STATUE = new PlaceableItem("Pyramid Statue", Paths.ITEM_PYRAMID_STATUE, Paths.ITEM_PYRAMID_STATUE, Paths.SPRITE_PYRAMID_STATUE_SPRITESHEET, Paths.SPRITE_PYRAMID_STATUE_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "An abstract statue of a pyramid. It is, of course, a subtle reference to the juxtaposition of loyalty and betrayal.", 1400, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(RAINBOW_GRAFFITI = new PlaceableItem("Rainbow Graffiti", Paths.ITEM_RAINBOW_GRAFFITI, Paths.ITEM_RAINBOW_GRAFFITI, Paths.SPRITE_RAINBOW_GRAFFITI_SPRITESHEET, Paths.SPRITE_RAINBOW_GRAFFITI_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "Painted with hope for a world united.", 1300, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(RECYCLING_BIN = new PlaceableItem("Recycling Bin", Paths.ITEM_RECYCLING_BIN, Paths.ITEM_RECYCLING_BIN, Paths.SPRITE_RECYCLING_BIN_SPRITESHEET, Paths.SPRITE_RECYCLING_BIN_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "Reduce & reuse.", 750, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(RETRO_GRAFFITI = new PlaceableItem("Retro Graffiti", Paths.ITEM_RETRO_GRAFFITI, Paths.ITEM_RETRO_GRAFFITI, Paths.SPRITE_RETRO_GRAFFITI_SPRITESHEET, Paths.SPRITE_RETRO_GRAFFITI_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "buying gf 1k", 900, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(RIGHT_ARROW_GRAFFITI = new PlaceableItem("Right Arrow Graffiti", Paths.ITEM_RIGHT_ARROW_GRAFFITI, Paths.ITEM_RIGHT_ARROW_GRAFFITI, Paths.SPRITE_RIGHT_ARROW_GRAFFITI_SPRITESHEET, Paths.SPRITE_RIGHT_ARROW_GRAFFITI_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "Take it to the right! Right! Right! Right!", 700, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SANDBOX = new PlaceableItem("Sandbox", Paths.ITEM_SANDBOX, Paths.ITEM_SANDBOX, Paths.SPRITE_SANDBOX_SPRITESHEET, Paths.SPRITE_SANDBOX_SPRITESHEET,
                3, 1, DEFAULT_STACK_SIZE, "I love this genre!", 800, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SANDCASTLE = new PlaceableItem("Sandcastle", Paths.ITEM_SANDCASTLE, Paths.ITEM_SANDCASTLE, Paths.SPRITE_SANDCASTLE_SPRITESHEET, Paths.SPRITE_SANDCASTLE_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "It's huge! To an ant.", 2600, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SCARECROW = new PlaceableItem("Scarecrow", Paths.ITEM_SCARECROW, Paths.ITEM_SCARECROW, Paths.SPRITE_SCARECROW_SPRITESHEET, Paths.SPRITE_SCARECROW_SPRITESHEET,
                1, 4, DEFAULT_STACK_SIZE, "Frightening! To crows, at least.", 260, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SEESAW = new PlaceableItem("Seesaw", Paths.ITEM_SEESAW, Paths.ITEM_SEESAW, Paths.SPRITE_SEESAW_SPRITESHEET, Paths.SPRITE_SEESAW_SPRITESHEET,
                4, 1, DEFAULT_STACK_SIZE, "See! Saw! Saw! See!", 780, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SIGNPOST = new PlaceableItem("Signpost", Paths.ITEM_SIGNPOST, Paths.ITEM_SIGNPOST, Paths.SPRITE_SIGNPOST_SPRITESHEET, Paths.SPRITE_SIGNPOST_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "No directions provided.", 250, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SLIDE = new PlaceableItem("Slide", Paths.ITEM_SLIDE, Paths.ITEM_SLIDE, Paths.SPRITE_SLIDE_SPRITESHEET, Paths.SPRITE_SLIDE_SPRITESHEET,
                4, 4, DEFAULT_STACK_SIZE, "It gets a bit hot in the sun.", 2100, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SMILE_GRAFFITI = new PlaceableItem("Smile Graffiti", Paths.ITEM_SMILE_GRAFFITI, Paths.ITEM_SMILE_GRAFFITI, Paths.SPRITE_SMILE_GRAFFITI_SPRITESHEET, Paths.SPRITE_SMILE_GRAFFITI_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "It's just a smile, see? Nothing to see here.", 900, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SNOWMAN = new PlaceableItem("Snowman", Paths.ITEM_SNOWMAN, Paths.ITEM_SNOWMAN, Paths.SPRITE_SNOWMAN_SPRITESHEET, Paths.SPRITE_SNOWMAN_SPRITESHEET,
                3, 5, DEFAULT_STACK_SIZE, "Not built to scale.", 2250, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SOFA = new PlaceableItem("Sofa", Paths.ITEM_SOFA, Paths.ITEM_SOFA, Paths.SPRITE_SOFA_SPRITESHEET, Paths.SPRITE_SOFA_SPRITESHEET,
                3, 2, DEFAULT_STACK_SIZE, "Comfy!", 1600, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SOLAR_PANEL = new PlaceableItem("Solar Panel", Paths.ITEM_SOLAR_PANEL, Paths.ITEM_SOLAR_PANEL, Paths.SPRITE_SOLAR_PANEL_SPRITESHEET, Paths.SPRITE_SOLAR_PANEL_SPRITESHEET,
                2, 1, DEFAULT_STACK_SIZE, "Finally, a renewable source of energy!", 1850, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SOURCE_UNKNOWN_GRAFFITI = new PlaceableItem("Source Unknown Graffiti", Paths.ITEM_SOURCE_UNKNOWN_GRAFFITI, Paths.ITEM_SOURCE_UNKNOWN_GRAFFITI, Paths.SPRITE_SOURCE_UNKNOWN_GRAFFITI_SPRITESHEET, Paths.SPRITE_SOURCE_UNKNOWN_GRAFFITI_SPRITESHEET,
                3, 1, DEFAULT_STACK_SIZE, "Who could have done this?", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SPHERE_STATUE = new PlaceableItem("Sphere Statue", Paths.ITEM_SPHERE_STATUE, Paths.ITEM_SPHERE_STATUE, Paths.SPRITE_SPHERE_STATUE_SPRITESHEET, Paths.SPRITE_SPHERE_STATUE_SPRITESHEET,
                1, 3, DEFAULT_STACK_SIZE, "An abstract statue of a sphere. It is secretly a cleverly concealed allusion to the meaninglessness of conflict.", 1700, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STATUE = new PlaceableItem("Statue", Paths.ITEM_STATUE, Paths.ITEM_STATUE, Paths.SPRITE_STATUE_SPRITESHEET, Paths.SPRITE_STATUE_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "Most statues are modeled after specific people. This statue is not like most statues.", 6200, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STONE_COLUMN = new PlaceableItem("Stone Column", Paths.ITEM_STONE_COLUMN, Paths.ITEM_STONE_COLUMN, Paths.SPRITE_STONE_COLUMN_SPRITESHEET, Paths.SPRITE_STONE_COLUMN_SPRITESHEET,
                1, 4, DEFAULT_STACK_SIZE, "Solid and strong. Made to last generations.", 800, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STREETLAMP = new PlaceableItem("Streetlamp", Paths.ITEM_STREETLAMP, Paths.ITEM_STREETLAMP, Paths.SPRITE_STREETLAMP_SPRITESHEET, Paths.SPRITE_STREETLAMP_SPRITESHEET,
                1, 5, DEFAULT_STACK_SIZE, "A rustic-styled light.", 625, EntityType.LIGHT_DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STREETLIGHT = new PlaceableItem("Streetlight", Paths.ITEM_STREETLIGHT, Paths.ITEM_STREETLIGHT, Paths.SPRITE_STREETLIGHT_SPRITESHEET, Paths.SPRITE_STREETLIGHT_SPRITESHEET,
                3, 5, DEFAULT_STACK_SIZE, "An urban-styled light.", 2500, EntityType.LIGHT_DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SURFBOARD = new PlaceableItem("Surfboard", Paths.ITEM_SURFBOARD, Paths.ITEM_SURFBOARD, Paths.SPRITE_SURFBOARD_SPRITESHEET, Paths.SPRITE_SURFBOARD_SPRITESHEET,
                2, 4, DEFAULT_STACK_SIZE, "Surf's up, dude!", 350, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SWINGS = new PlaceableItem("Swings", Paths.ITEM_SWINGS, Paths.ITEM_SWINGS, Paths.SPRITE_SWINGS_SPRITESHEET, Paths.SPRITE_SWINGS_SPRITESHEET,
                5, 4, DEFAULT_STACK_SIZE, "A swing full of memories.", 1700, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TARGET = new PlaceableItem("Target", Paths.ITEM_TARGET, Paths.ITEM_TARGET, Paths.SPRITE_TARGET_SPRITESHEET, Paths.SPRITE_TARGET_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "Aim, shoot, fire!", 425, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TELEVISION = new PlaceableItem("Television", Paths.ITEM_TELEVISION, Paths.ITEM_TELEVISION, Paths.SPRITE_TELEVISION_SPRITESHEET, Paths.SPRITE_TELEVISION_SPRITESHEET,
                2, 3, DEFAULT_STACK_SIZE, "Not terribly useful without cable...", 2800, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOOLBOX = new PlaceableItem("Toolbox", Paths.ITEM_TOOLBOX, Paths.ITEM_TOOLBOX, Paths.SPRITE_TOOLBOX_SPRITESHEET, Paths.SPRITE_TOOLBOX_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "It's got all the nuts and bolts.", 850, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TOOLRACK = new PlaceableItem("Toolrack", Paths.ITEM_TOOLRACK, Paths.ITEM_TOOLRACK, Paths.SPRITE_TOOLRACK_SPRITESHEET, Paths.SPRITE_TOOLRACK_SPRITESHEET,
                2, 2, DEFAULT_STACK_SIZE, "The tools are decorative.", 720, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TRAFFIC_CONE = new PlaceableItem("Traffic Cone", Paths.ITEM_TRAFFIC_CONE, Paths.ITEM_TRAFFIC_CONE, Paths.SPRITE_TRAFFIC_CONE_SPRITESHEET, Paths.SPRITE_TRAFFIC_CONE_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "Also a great hat!", 1100, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TRAFFIC_LIGHT = new PlaceableItem("Traffic Light", Paths.ITEM_TRAFFIC_LIGHT, Paths.ITEM_TRAFFIC_LIGHT, Paths.SPRITE_TRAFFIC_LIGHT_SPRITESHEET, Paths.SPRITE_TRAFFIC_LIGHT_SPRITESHEET,
                2, 5, DEFAULT_STACK_SIZE, "It seems to be a bit broken.", 2000, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TRASHCAN = new PlaceableItem("Trashcan", Paths.ITEM_TRASHCAN, Paths.ITEM_TRASHCAN, Paths.SPRITE_TRASHCAN_SPRITESHEET, Paths.SPRITE_TRASHCAN_SPRITESHEET,
                1, 2, DEFAULT_STACK_SIZE, "No littering!", 625, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(TRIPLE_MIRRORS = new PlaceableItem("Triple Mirrors", Paths.ITEM_TRIPLE_MIRRORS, Paths.ITEM_TRIPLE_MIRRORS, Paths.SPRITE_TRIPLE_MIRRORS_SPRITESHEET, Paths.SPRITE_TRIPLE_MIRRORS_SPRITESHEET,
                3, 5, DEFAULT_STACK_SIZE, "Reflection in triplicate.", 3300, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(UMBRELLA = new PlaceableItem("Umbrella", Paths.ITEM_UMBRELLA, Paths.ITEM_UMBRELLA, Paths.SPRITE_UMBRELLA_SPRITESHEET, Paths.SPRITE_UMBRELLA_SPRITESHEET,
                3, 4, DEFAULT_STACK_SIZE, "It's an umbrella without a table.", 1500, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(UMBRELLA_TABLE = new PlaceableItem("Umbrella Table", Paths.ITEM_UMBRELLA_TABLE, Paths.ITEM_UMBRELLA_TABLE, Paths.SPRITE_UMBRELLA_TABLE_SPRITESHEET, Paths.SPRITE_UMBRELLA_TABLE_SPRITESHEET,
                3, 4, DEFAULT_STACK_SIZE, "It's an umbrella with a table.", 2100, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WAGON = new PlaceableItem("Wagon", Paths.ITEM_WAGON, Paths.ITEM_WAGON, Paths.SPRITE_WAGON_SPRITESHEET, Paths.SPRITE_WAGON_SPRITESHEET,
               4, 3, DEFAULT_STACK_SIZE, "The pioneers used to ride these babies for miles!", 3000, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WATER_PUMP = new PlaceableItem("Water Pump", Paths.ITEM_WATER_PUMP, Paths.ITEM_WATER_PUMP, Paths.SPRITE_WATER_PUMP_SPRITESHEET, Paths.SPRITE_WATER_PUMP_SPRITESHEET,
               1, 2, DEFAULT_STACK_SIZE, "It's even usable by the blind!", 1100, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WATERTOWER = new PlaceableItem("Watertower", Paths.ITEM_WATERTOWER, Paths.ITEM_WATERTOWER, Paths.SPRITE_WATERTOWER_SPRITESHEET, Paths.SPRITE_WATERTOWER_SPRITESHEET,
               3, 6, DEFAULT_STACK_SIZE, "Made to hold water. More sturdy than it looks.", 4100, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WELL = new PlaceableItem("Well", Paths.ITEM_WELL, Paths.ITEM_WELL, Paths.SPRITE_WELL_SPRITESHEET, Paths.SPRITE_WELL_SPRITESHEET,
               2, 4, DEFAULT_STACK_SIZE, "well well...", 2200, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WHEELBARROW = new PlaceableItem("Wheelbarrow", Paths.ITEM_WHEELBARROW, Paths.ITEM_WHEELBARROW, Paths.SPRITE_WHEELBARROW_SPRITESHEET, Paths.SPRITE_WHEELBARROW_SPRITESHEET,
               2, 2, DEFAULT_STACK_SIZE, "The forgotten 7th brother, Wheel the Exhausted.", 750, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WHITEBOARD = new PlaceableItem("Whiteboard", Paths.ITEM_WHITEBOARD, Paths.ITEM_WHITEBOARD, Paths.SPRITE_WHITEBOARD_SPRITESHEET, Paths.SPRITE_WHITEBOARD_SPRITESHEET,
               3, 3, DEFAULT_STACK_SIZE, "A modern classroom staple. Just like the stapler.", 1400, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_BENCH = new PlaceableItem("Wooden Bench", Paths.ITEM_WOODEN_BENCH, Paths.ITEM_WOODEN_BENCH, Paths.SPRITE_WOODEN_BENCH_SPRITESHEET, Paths.SPRITE_WOODEN_BENCH_SPRITESHEET,
               3, 2, DEFAULT_STACK_SIZE, "Made for parks.", 350, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_CHAIR = new PlaceableItem("Wooden Chair", Paths.ITEM_WOODEN_CHAIR, Paths.ITEM_WOODEN_CHAIR, Paths.SPRITE_WOODEN_CHAIR_SPRITESHEET, Paths.SPRITE_WOODEN_CHAIR_SPRITESHEET,
               1, 2, DEFAULT_STACK_SIZE, "Mysteriously musical. You'll always be one short.", 120, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_COLUMN = new PlaceableItem("Wooden Column", Paths.ITEM_WOODEN_COLUMN, Paths.ITEM_WOODEN_COLUMN, Paths.SPRITE_WOODEN_COLUMN_SPRITESHEET, Paths.SPRITE_WOODEN_COLUMN_SPRITESHEET,
               1, 4, DEFAULT_STACK_SIZE, "Popular remix of the hit single \"Wooden Row\"", 320, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_LONGTABLE = new PlaceableItem("Wooden Longtable", Paths.ITEM_WOODEN_LONGTABLE, Paths.ITEM_WOODEN_LONGTABLE, Paths.SPRITE_WOODEN_LONGTABLE_SPRITESHEET, Paths.SPRITE_WOODEN_LONGTABLE_SPRITESHEET,
               4, 2, DEFAULT_STACK_SIZE, "Ready for a banquet!", 550, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_POST = new PlaceableItem("Wooden Post", Paths.ITEM_WOODEN_POST, Paths.ITEM_WOODEN_POST, Paths.SPRITE_WOODEN_POST_SPRITESHEET, Paths.SPRITE_WOODEN_POST_SPRITESHEET,
               1, 2, DEFAULT_STACK_SIZE, "As simple as they come.", 75, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_ROUNDTABLE = new PlaceableItem("Wooden Roundtable", Paths.ITEM_WOODEN_ROUNDTABLE, Paths.ITEM_WOODEN_ROUNDTABLE, Paths.SPRITE_WOODEN_ROUNDTABLE_SPRITESHEET, Paths.SPRITE_WOODEN_ROUNDTABLE_SPRITESHEET,
               2, 2, DEFAULT_STACK_SIZE, "Call the knights!", 300, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_SQUARETABLE = new PlaceableItem("Wooden Squaretable", Paths.ITEM_WOODEN_SQUARETABLE, Paths.ITEM_WOODEN_SQUARETABLE, Paths.SPRITE_WOODEN_SQUARETABLE_SPRITESHEET, Paths.SPRITE_WOODEN_SQUARETABLE_SPRITESHEET,
               2, 2, DEFAULT_STACK_SIZE, "Running into the edges is pretty painful.", 180, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WOODEN_STOOL = new PlaceableItem("Wooden Stool", Paths.ITEM_WOODEN_STOOL, Paths.ITEM_WOODEN_STOOL, Paths.SPRITE_WOODEN_STOOL_SPRITESHEET, Paths.SPRITE_WOODEN_STOOL_SPRITESHEET,
               1, 2, DEFAULT_STACK_SIZE, "It's like a chair, but not.", 100, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DRUMSET = new PlaceableItem("Drumset", Paths.ITEM_DRUMSET, Paths.ITEM_DRUMSET, Paths.SPRITE_DRUMSET_SPRITESHEET, Paths.SPRITE_DRUMSET_SPRITESHEET,
               4, 3, DEFAULT_STACK_SIZE, "Badum tsssss?", 3520, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HARP = new PlaceableItem("Harp", Paths.ITEM_HARP, Paths.ITEM_HARP, Paths.SPRITE_HARP_SPRITESHEET, Paths.SPRITE_HARP_SPRITESHEET,
               2, 3, DEFAULT_STACK_SIZE, "Pure and melodious.", 3570, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(XYLOPHONE = new PlaceableItem("Xylophone", Paths.ITEM_XYLOPHONE, Paths.ITEM_XYLOPHONE, Paths.SPRITE_XYLOPHONE_SPRITESHEET, Paths.SPRITE_XYLOPHONE_SPRITESHEET,
               3, 2, DEFAULT_STACK_SIZE, "Operator?", 3180, EntityType.DECOR, PlaceableItem.PlacementType.NORMAL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(PAINTING_OASIS = new PlaceableItem("\"Oasis\"", Paths.ITEM_PAINTING_OASIS, Paths.ITEM_PAINTING_OASIS, Paths.SPRITE_PAINTING_OASIS_SPRITESHEET, Paths.SPRITE_PAINTING_OASIS_SPRITESHEET,
                2, 3, 1, "Painting #1 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_FOUR = new PlaceableItem("\"Four\"", Paths.ITEM_PAINTING_FOUR, Paths.ITEM_PAINTING_FOUR, Paths.SPRITE_PAINTING_FOUR_SPRITESHEET, Paths.SPRITE_PAINTING_FOUR_SPRITESHEET,
                2, 3, 1, "Painting #2 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_FUTURE = new PlaceableItem("\"Future\"", Paths.ITEM_PAINTING_FUTURE, Paths.ITEM_PAINTING_FUTURE, Paths.SPRITE_PAINTING_FUTURE_SPRITESHEET, Paths.SPRITE_PAINTING_FUTURE_SPRITESHEET,
                2, 3, 1, "Painting #3 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_ARCTIC = new PlaceableItem("\"Arctic\"", Paths.ITEM_PAINTING_ARCTIC, Paths.ITEM_PAINTING_ARCTIC, Paths.SPRITE_PAINTING_ARCTIC_SPRITESHEET, Paths.SPRITE_PAINTING_ARCTIC_SPRITESHEET,
                3, 2, 1, "Painting #4 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_FATE = new PlaceableItem("\"Fate\"", Paths.ITEM_PAINTING_FATE, Paths.ITEM_PAINTING_FATE, Paths.SPRITE_PAINTING_FATE_SPRITESHEET, Paths.SPRITE_PAINTING_FATE_SPRITESHEET,
                3, 2, 1, "Painting #5 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_ACCEPTANCE = new PlaceableItem("\"Acceptance\"", Paths.ITEM_PAINTING_ACCEPTANCE, Paths.ITEM_PAINTING_ACCEPTANCE, Paths.SPRITE_PAINTING_ACCEPTANCE_SPRITESHEET, Paths.SPRITE_PAINTING_ACCEPTANCE_SPRITESHEET,
                1, 1, 1, "Painting #6 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_BALANCE = new PlaceableItem("\"Balance\"", Paths.ITEM_PAINTING_BALANCE, Paths.ITEM_PAINTING_BALANCE, Paths.SPRITE_PAINTING_BALANCE_SPRITESHEET, Paths.SPRITE_PAINTING_BALANCE_SPRITESHEET,
                1, 1, 1, "Painting #7 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_BLACK = new PlaceableItem("\"Black\"", Paths.ITEM_PAINTING_BLACK, Paths.ITEM_PAINTING_BLACK, Paths.SPRITE_PAINTING_BLACK_SPRITESHEET, Paths.SPRITE_PAINTING_BLACK_SPRITESHEET,
                1, 1, 1, "Painting #8 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_COFFEE = new PlaceableItem("\"Coffee\"", Paths.ITEM_PAINTING_COFFEE, Paths.ITEM_PAINTING_COFFEE, Paths.SPRITE_PAINTING_COFFEE_SPRITESHEET, Paths.SPRITE_PAINTING_COFFEE_SPRITESHEET,
                1, 1, 1, "Painting #9 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_DICHOTOMY = new PlaceableItem("\"Dichotomy\"", Paths.ITEM_PAINTING_DICHOTOMY, Paths.ITEM_PAINTING_DICHOTOMY, Paths.SPRITE_PAINTING_DICHOTOMY_SPRITESHEET, Paths.SPRITE_PAINTING_DICHOTOMY_SPRITESHEET,
                1, 1, 1, "Painting #10 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_FIREBALL = new PlaceableItem("\"Fireball\"", Paths.ITEM_PAINTING_FIREBALL, Paths.ITEM_PAINTING_FIREBALL, Paths.SPRITE_PAINTING_FIREBALL_SPRITESHEET, Paths.SPRITE_PAINTING_FIREBALL_SPRITESHEET,
                1, 1, 1, "Painting #11 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_LION = new PlaceableItem("\"Lion\"", Paths.ITEM_PAINTING_LION, Paths.ITEM_PAINTING_LION, Paths.SPRITE_PAINTING_LION_SPRITESHEET, Paths.SPRITE_PAINTING_LION_SPRITESHEET,
                1, 1, 1, "Painting #12 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_MINTGREEN = new PlaceableItem("\"Mintgreen\"", Paths.ITEM_PAINTING_MINTGREEN, Paths.ITEM_PAINTING_MINTGREEN, Paths.SPRITE_PAINTING_MINTGREEN_SPRITESHEET, Paths.SPRITE_PAINTING_MINTGREEN_SPRITESHEET,
                1, 1, 1, "Painting #13 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_GROVE = new PlaceableItem("\"Grove\"", Paths.ITEM_PAINTING_GROVE, Paths.ITEM_PAINTING_GROVE, Paths.SPRITE_PAINTING_GROVE_SPRITESHEET, Paths.SPRITE_PAINTING_GROVE_SPRITESHEET,
                1, 1, 1, "Painting #14 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_PUZZLE = new PlaceableItem("\"Puzzle\"", Paths.ITEM_PAINTING_PUZZLE, Paths.ITEM_PAINTING_PUZZLE, Paths.SPRITE_PAINTING_PUZZLE_SPRITESHEET, Paths.SPRITE_PAINTING_PUZZLE_SPRITESHEET,
                1, 1, 1, "Painting #15 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_TOXIN = new PlaceableItem("\"Toxin\"", Paths.ITEM_PAINTING_TOXIN, Paths.ITEM_PAINTING_TOXIN, Paths.SPRITE_PAINTING_TOXIN_SPRITESHEET, Paths.SPRITE_PAINTING_TOXIN_SPRITESHEET,
                1, 1, 1, "Painting #16 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_CREAMPUFF = new PlaceableItem("\"Creampuff\"", Paths.ITEM_PAINTING_CREAMPUFF, Paths.ITEM_PAINTING_CREAMPUFF, Paths.SPRITE_PAINTING_CREAMPUFF_SPRITESHEET, Paths.SPRITE_PAINTING_CREAMPUFF_SPRITESHEET,
                1, 1, 1, "Painting #17 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_SELF = new PlaceableItem("\"Self\"", Paths.ITEM_PAINTING_SELF, Paths.ITEM_PAINTING_SELF, Paths.SPRITE_PAINTING_SELF_SPRITESHEET, Paths.SPRITE_PAINTING_SELF_SPRITESHEET,
                1, 1, 1, "Painting #18 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_RAIDER = new PlaceableItem("\"Raider\"", Paths.ITEM_PAINTING_RAIDER, Paths.ITEM_PAINTING_RAIDER, Paths.SPRITE_PAINTING_RAIDER_SPRITESHEET, Paths.SPRITE_PAINTING_RAIDER_SPRITESHEET,
                1, 1, 1, "Painting #19 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_VINEFLOWER = new PlaceableItem("\"Vineflower\"", Paths.ITEM_PAINTING_VINEFLOWER, Paths.ITEM_PAINTING_VINEFLOWER, Paths.SPRITE_PAINTING_VINEFLOWER_SPRITESHEET, Paths.SPRITE_PAINTING_VINEFLOWER_SPRITESHEET,
                1, 1, 1, "Painting #20 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_FJORD = new PlaceableItem("\"Fjord\"", Paths.ITEM_PAINTING_FJORD, Paths.ITEM_PAINTING_FJORD, Paths.SPRITE_PAINTING_FJORD_SPRITESHEET, Paths.SPRITE_PAINTING_FJORD_SPRITESHEET,
                1, 1, 1, "Painting #21 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_DITHER = new PlaceableItem("\"Dither\"", Paths.ITEM_PAINTING_DITHER, Paths.ITEM_PAINTING_DITHER, Paths.SPRITE_PAINTING_DITHER_SPRITESHEET, Paths.SPRITE_PAINTING_DITHER_SPRITESHEET,
                5, 1, 1, "Painting #22 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_BEACHDAY = new PlaceableItem("\"Beach Day\"", Paths.ITEM_PAINTING_BEACHDAY, Paths.ITEM_PAINTING_BEACHDAY, Paths.SPRITE_PAINTING_BEACHDAY_SPRITESHEET, Paths.SPRITE_PAINTING_BEACHDAY_SPRITESHEET,
                4, 1, 1, "Painting #23 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_RIVER = new PlaceableItem("\"River\"", Paths.ITEM_PAINTING_RIVER, Paths.ITEM_PAINTING_RIVER, Paths.SPRITE_PAINTING_RIVER_SPRITESHEET, Paths.SPRITE_PAINTING_RIVER_SPRITESHEET,
                4, 1, 1, "Painting #24 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_BEDTIME = new PlaceableItem("\"Bedtime\"", Paths.ITEM_PAINTING_BEDTIME, Paths.ITEM_PAINTING_BEDTIME, Paths.SPRITE_PAINTING_BEDTIME_SPRITESHEET, Paths.SPRITE_PAINTING_BEDTIME_SPRITESHEET,
                2, 2, 1, "Painting #25 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_ILOVEYOU = new PlaceableItem("\"iloveyou\"", Paths.ITEM_PAINTING_ILOVEYOU, Paths.ITEM_PAINTING_ILOVEYOU, Paths.SPRITE_PAINTING_ILOVEYOU_SPRITESHEET, Paths.SPRITE_PAINTING_ILOVEYOU_SPRITESHEET,
                2, 2, 1, "Painting #26 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_CHANGES = new PlaceableItem("\"Changes\"", Paths.ITEM_PAINTING_CHANGES, Paths.ITEM_PAINTING_CHANGES, Paths.SPRITE_PAINTING_CHANGES_SPRITESHEET, Paths.SPRITE_PAINTING_CHANGES_SPRITESHEET,
                2, 2, 1, "Painting #27 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_LIBERTY = new PlaceableItem("\"Liberty\"", Paths.ITEM_PAINTING_LIBERTY, Paths.ITEM_PAINTING_LIBERTY, Paths.SPRITE_PAINTING_LIBERTY_SPRITESHEET, Paths.SPRITE_PAINTING_LIBERTY_SPRITESHEET,
                2, 2, 1, "Painting #28 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_INTERLUDE = new PlaceableItem("\"Interlude\"", Paths.ITEM_PAINTING_INTERLUDE, Paths.ITEM_PAINTING_INTERLUDE, Paths.SPRITE_PAINTING_INTERLUDE_SPRITESHEET, Paths.SPRITE_PAINTING_INTERLUDE_SPRITESHEET,
                2, 2, 1, "Painting #29 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_SKYROSE = new PlaceableItem("\"Skyrose\"", Paths.ITEM_PAINTING_SKYROSE, Paths.ITEM_PAINTING_SKYROSE, Paths.SPRITE_PAINTING_SKYROSE_SPRITESHEET, Paths.SPRITE_PAINTING_SKYROSE_SPRITESHEET,
                2, 2, 1, "Painting #30 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_EARTH = new PlaceableItem("\"Earth\"", Paths.ITEM_PAINTING_EARTH, Paths.ITEM_PAINTING_EARTH, Paths.SPRITE_PAINTING_EARTH_SPRITESHEET, Paths.SPRITE_PAINTING_EARTH_SPRITESHEET,
                2, 2, 1, "Painting #31 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_CALCULATOR = new PlaceableItem("\"Calculator\"", Paths.ITEM_PAINTING_CALCULATOR, Paths.ITEM_PAINTING_CALCULATOR, Paths.SPRITE_PAINTING_CALCULATOR_SPRITESHEET, Paths.SPRITE_PAINTING_CALCULATOR_SPRITESHEET,
                2, 2, 1, "Painting #32 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_CORAL = new PlaceableItem("\"Coral\"", Paths.ITEM_PAINTING_CORAL, Paths.ITEM_PAINTING_CORAL, Paths.SPRITE_PAINTING_CORAL_SPRITESHEET, Paths.SPRITE_PAINTING_CORAL_SPRITESHEET,
                2, 2, 1, "Painting #33 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_SEASONS = new PlaceableItem("\"Seasons\"", Paths.ITEM_PAINTING_SEASONS, Paths.ITEM_PAINTING_SEASONS, Paths.SPRITE_PAINTING_SEASONS_SPRITESHEET, Paths.SPRITE_PAINTING_SEASONS_SPRITESHEET,
                2, 2, 1, "Painting #34 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_WHATEVER = new PlaceableItem("\"whatever...\"", Paths.ITEM_PAINTING_WHATEVER, Paths.ITEM_PAINTING_WHATEVER, Paths.SPRITE_PAINTING_WHATEVER_SPRITESHEET, Paths.SPRITE_PAINTING_WHATEVER_SPRITESHEET,
                2, 2, 1, "Painting #35 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_SOLEMN = new PlaceableItem("\"Solemn\"", Paths.ITEM_PAINTING_SOLEMN, Paths.ITEM_PAINTING_SOLEMN, Paths.SPRITE_PAINTING_SOLEMN_SPRITESHEET, Paths.SPRITE_PAINTING_SOLEMN_SPRITESHEET,
                2, 2, 1, "Painting #36 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_MOONSET = new PlaceableItem("\"Moonset\"", Paths.ITEM_PAINTING_MOONSET, Paths.ITEM_PAINTING_MOONSET, Paths.SPRITE_PAINTING_MOONSET_SPRITESHEET, Paths.SPRITE_PAINTING_MOONSET_SPRITESHEET,
                2, 2, 1, "Painting #37 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_MONA = new PlaceableItem("\"Mona\"", Paths.ITEM_PAINTING_SEASONS, Paths.ITEM_PAINTING_MONA, Paths.SPRITE_PAINTING_MONA_SPRITESHEET, Paths.SPRITE_PAINTING_MONA_SPRITESHEET,
                2, 2, 1, "Painting #38 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_OVERHANG = new PlaceableItem("\"Overhang\"", Paths.ITEM_PAINTING_OVERHANG, Paths.ITEM_PAINTING_OVERHANG, Paths.SPRITE_PAINTING_OVERHANG_SPRITESHEET, Paths.SPRITE_PAINTING_OVERHANG_SPRITESHEET,
                2, 2, 1, "Painting #39 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_SUNSET = new PlaceableItem("\"Sunset\"", Paths.ITEM_PAINTING_SUNSET, Paths.ITEM_PAINTING_SUNSET, Paths.SPRITE_PAINTING_SUNSET_SPRITESHEET, Paths.SPRITE_PAINTING_SUNSET_SPRITESHEET,
                2, 2, 1, "Painting #40 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_SPICE = new PlaceableItem("\"Spice\"", Paths.ITEM_PAINTING_SPICE, Paths.ITEM_PAINTING_SPICE, Paths.SPRITE_PAINTING_SPICE_SPRITESHEET, Paths.SPRITE_PAINTING_SPICE_SPRITESHEET,
                2, 2, 1, "Painting #41 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_WINDOW = new PlaceableItem("\"Window\"", Paths.ITEM_PAINTING_WINDOW, Paths.ITEM_PAINTING_WINDOW, Paths.SPRITE_PAINTING_WINDOW_SPRITESHEET, Paths.SPRITE_PAINTING_WINDOW_SPRITESHEET,
                2, 2, 1, "Painting #42 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_ET = new PlaceableItem("\"E.T.\"", Paths.ITEM_PAINTING_ET, Paths.ITEM_PAINTING_ET, Paths.SPRITE_PAINTING_ET_SPRITESHEET, Paths.SPRITE_PAINTING_ET_SPRITESHEET,
                2, 2, 1, "Painting #43 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_LAVENDER = new PlaceableItem("\"Lavender\"", Paths.ITEM_PAINTING_LAVENDER, Paths.ITEM_PAINTING_LAVENDER, Paths.SPRITE_PAINTING_LAVENDER_SPRITESHEET, Paths.SPRITE_PAINTING_LAVENDER_SPRITESHEET,
                2, 2, 1, "Painting #44 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_THREADS = new PlaceableItem("\"Threads\"", Paths.ITEM_PAINTING_THREADS, Paths.ITEM_PAINTING_THREADS, Paths.SPRITE_PAINTING_THREADS_SPRITESHEET, Paths.SPRITE_PAINTING_THREADS_SPRITESHEET,
                2, 2, 1, "Painting #45 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_LAUNCH = new PlaceableItem("\"Launch\"", Paths.ITEM_PAINTING_LAUNCH, Paths.ITEM_PAINTING_LAUNCH, Paths.SPRITE_PAINTING_LAUNCH_SPRITESHEET, Paths.SPRITE_PAINTING_LAUNCH_SPRITESHEET,
                1, 2, 1, "Painting #46 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_FABLE = new PlaceableItem("\"Fable\"", Paths.ITEM_PAINTING_FABLE, Paths.ITEM_PAINTING_FABLE, Paths.SPRITE_PAINTING_FABLE_SPRITESHEET, Paths.SPRITE_PAINTING_FABLE_SPRITESHEET,
                1, 2, 1, "Painting #47 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_GROWTH = new PlaceableItem("\"Growth\"", Paths.ITEM_PAINTING_GROWTH, Paths.ITEM_PAINTING_GROWTH, Paths.SPRITE_PAINTING_GROWTH_SPRITESHEET, Paths.SPRITE_PAINTING_GROWTH_SPRITESHEET,
                1, 2, 1, "Painting #48 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_SHADES = new PlaceableItem("\"Shades\"", Paths.ITEM_PAINTING_SHADES, Paths.ITEM_PAINTING_SHADES, Paths.SPRITE_PAINTING_SHADES_SPRITESHEET, Paths.SPRITE_PAINTING_SHADES_SPRITESHEET,
                1, 2, 1, "Painting #49 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_GENIUS = new PlaceableItem("\"Genius\"", Paths.ITEM_PAINTING_GENIUS, Paths.ITEM_PAINTING_GENIUS, Paths.SPRITE_PAINTING_GENIUS_SPRITESHEET, Paths.SPRITE_PAINTING_GENIUS_SPRITESHEET,
                1, 2, 1, "Painting #50 of 50", 1000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PAINTING_RESONANT = new PlaceableItem("\"Resonant\"", Paths.ITEM_PAINTING_RESONANT, Paths.ITEM_PAINTING_RESONANT, Paths.SPRITE_PAINTING_RESONANT_SPRITESHEET, Paths.SPRITE_PAINTING_RESONANT_SPRITESHEET,
                6, 7, 1, "Painting #51 of 50\nHow does it feel to be on the top, looking down?\nThank you for playing Plateau.\n-Ben", 800000000, EntityType.WALL_DECOR, PlaceableItem.PlacementType.WALL, null, Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(WAVE_WALLPAPER = new WallpaperItem("Wave Wallpaper", Paths.ITEM_WALLPAPER_WAVE, Paths.ITEM_WALLPAPER_WAVE,
                Paths.SPRITE_WALLPAPER_WAVE, Paths.SPRITE_WALLPAPER_WAVE,
                Paths.SPRITE_WALLPAPER_WAVE_TOP, Paths.SPRITE_WALLPAPER_WAVE_TOP,
                Paths.SPRITE_WALLPAPER_WAVE_BOTTOM, Paths.SPRITE_WALLPAPER_WAVE_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "Do the wave!", 75, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(STAR_WALLPAPER = new WallpaperItem("Star Wallpaper", Paths.ITEM_WALLPAPER_STAR, Paths.ITEM_WALLPAPER_STAR,
                Paths.SPRITE_WALLPAPER_STAR, Paths.SPRITE_WALLPAPER_STAR,
                Paths.SPRITE_WALLPAPER_STAR, Paths.SPRITE_WALLPAPER_STAR,
                Paths.SPRITE_WALLPAPER_STAR, Paths.SPRITE_WALLPAPER_STAR,
                1, 1, DEFAULT_STACK_SIZE, "One small step for man, one giant leap for your walls.", 95, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BUBBLE_WALLPAPER = new WallpaperItem("Bubble Wallpaper", Paths.ITEM_WALLPAPER_BUBBLE, Paths.ITEM_WALLPAPER_BUBBLE, 
                Paths.SPRITE_WALLPAPER_BUBBLE, Paths.SPRITE_WALLPAPER_BUBBLE,
                Paths.SPRITE_WALLPAPER_BUBBLE, Paths.SPRITE_WALLPAPER_BUBBLE,
                Paths.SPRITE_WALLPAPER_BUBBLE, Paths.SPRITE_WALLPAPER_BUBBLE,
                1, 1, DEFAULT_STACK_SIZE, "It makes you feel like you're underwater!", 80, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SOLID_WALLPAPER = new WallpaperItem("Solid Wallpaper", Paths.ITEM_WALLPAPER_SOLID, Paths.ITEM_WALLPAPER_SOLID,
                Paths.SPRITE_WALLPAPER_SOLID, Paths.SPRITE_WALLPAPER_SOLID,
                Paths.SPRITE_WALLPAPER_SOLID_TOP, Paths.SPRITE_WALLPAPER_SOLID_TOP,
                Paths.SPRITE_WALLPAPER_SOLID_BOTTOM, Paths.SPRITE_WALLPAPER_SOLID_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "A roll of basic solid colored wallpaper.", 50, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(VERTICAL_WALLPAPER = new WallpaperItem("Vertical Wallpaper", Paths.ITEM_WALLPAPER_VERTICAL, Paths.ITEM_WALLPAPER_VERTICAL,
                Paths.SPRITE_WALLPAPER_VERTICAL, Paths.SPRITE_WALLPAPER_BUBBLE,
                Paths.SPRITE_WALLPAPER_VERTICAL_TOP, Paths.SPRITE_WALLPAPER_VERTICAL_TOP,
                Paths.SPRITE_WALLPAPER_VERTICAL_BOTTOM, Paths.SPRITE_WALLPAPER_VERTICAL_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "A roll of vertically striped wallpaper.", 75, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(HORIZONTAL_WALLPAPER = new WallpaperItem("Horizontal Wallpaper", Paths.ITEM_WALLPAPER_HORIZONTAL, Paths.ITEM_WALLPAPER_HORIZONTAL,
                Paths.SPRITE_WALLPAPER_HORIZONTAL, Paths.SPRITE_WALLPAPER_HORIZONTAL,
                Paths.SPRITE_WALLPAPER_HORIZONTAL_TOP, Paths.SPRITE_WALLPAPER_HORIZONTAL_TOP,
                Paths.SPRITE_WALLPAPER_HORIZONTAL_BOTTOM, Paths.SPRITE_WALLPAPER_HORIZONTAL_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "It's exactly what it says on the tin.", 90, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(DOT_WALLPAPER = new WallpaperItem("Dot Wallpaper", Paths.ITEM_WALLPAPER_DOT, Paths.ITEM_WALLPAPER_DOT,
                Paths.SPRITE_WALLPAPER_DOT, Paths.SPRITE_WALLPAPER_DOT,
                Paths.SPRITE_WALLPAPER_DOT_TOP, Paths.SPRITE_WALLPAPER_DOT_TOP,
                Paths.SPRITE_WALLPAPER_DOT_BOTTOM, Paths.SPRITE_WALLPAPER_DOT_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "This wallpaper doesn't take Damage Over Time in any way. It's long lasting!", 95, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(POLKA_WALLPAPER = new WallpaperItem("Polka Wallpaper", Paths.ITEM_WALLPAPER_POLKA, Paths.ITEM_WALLPAPER_POLKA,
                Paths.SPRITE_WALLPAPER_POLKA, Paths.SPRITE_WALLPAPER_POLKA,
                Paths.SPRITE_WALLPAPER_POLKA_TOP, Paths.SPRITE_WALLPAPER_POLKA_TOP,
                Paths.SPRITE_WALLPAPER_POLKA_BOTTOM, Paths.SPRITE_WALLPAPER_POLKA_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "Do the dance!", 90, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(INVADER_WALLPAPER = new WallpaperItem("Invader Wallpaper", Paths.ITEM_WALLPAPER_INVADER, Paths.ITEM_WALLPAPER_INVADER,
                Paths.SPRITE_WALLPAPER_INVADER, Paths.SPRITE_WALLPAPER_INVADER,
                Paths.SPRITE_WALLPAPER_INVADER_TOP, Paths.SPRITE_WALLPAPER_INVADER_TOP,
                Paths.SPRITE_WALLPAPER_INVADER_BOTTOM, Paths.SPRITE_WALLPAPER_INVADER_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "Modeled after retro arcade games.", 85, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(ODD_WALLPAPER = new WallpaperItem("Odd Wallpaper", Paths.ITEM_WALLPAPER_ODD, Paths.ITEM_WALLPAPER_ODD,
                Paths.SPRITE_WALLPAPER_ODD, Paths.SPRITE_WALLPAPER_ODD,
                Paths.SPRITE_WALLPAPER_ODD_TOP, Paths.SPRITE_WALLPAPER_ODD_TOP,
                Paths.SPRITE_WALLPAPER_ODD_BOTTOM, Paths.SPRITE_WALLPAPER_ODD_BOTTOM,
                1, 1, DEFAULT_STACK_SIZE, "What is this pattern even supposed to be?", 70, EntityType.WALLPAPER, PlaceableItem.PlacementType.WALLPAPER, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));

            AddToDictionary(ROYAL_CREST = new Item("Royal Crest", Paths.ITEM_ROYAL_CREST, 1,"", 3700, Tag.ACCESSORY));
            AddToDictionary(MIDIAN_SYMBOL = new Item("Midian Symbol", Paths.ITEM_MIDIAN_SYMBOL, 1,"", 4860, Tag.ACCESSORY));
            AddToDictionary(UNITY_CREST = new Item("Unity Crest", Paths.ITEM_UNITY_CREST, 1,"", 5700, Tag.ACCESSORY));
            AddToDictionary(COMPRESSION_CREST = new Item("Compression Crest", Paths.ITEM_COMPRESSION_CREST, 1,"", 4700, Tag.ACCESSORY));
            AddToDictionary(POLYMORPH_CREST = new Item("Polymorph Crest", Paths.ITEM_POLYMORPH_CREST, 1,"", 5800, Tag.ACCESSORY));
            AddToDictionary(PHILOSOPHERS_CREST = new Item("Philosopher\'s crest", Paths.ITEM_PHILOSOPHERS_CREST, 1,"", 10100, Tag.ACCESSORY));
            AddToDictionary(DASHING_CREST = new Item("Dashing Crest", Paths.ITEM_DASHING_CREST, 1,"", 5170, Tag.ACCESSORY));
            AddToDictionary(FROZEN_CREST = new Item("Frozen Crest", Paths.ITEM_FROZEN_CREST, 1,"", 5000, Tag.ACCESSORY));
            AddToDictionary(MUTATING_CREST = new Item("Mutating Crest", Paths.ITEM_MUTATING_CREST, 1,"", 6500, Tag.ACCESSORY));
            AddToDictionary(MYTHICAL_CREST = new Item("Mythical Crest", Paths.ITEM_MYTHICAL_CREST, 1,"", 5550, Tag.ACCESSORY));
            AddToDictionary(VAMPYRIC_CREST = new Item("Vampyric Crest", Paths.ITEM_VAMPYRIC_CREST, 1,"", 4750, Tag.ACCESSORY));
            AddToDictionary(BREWERY_CREST = new Item("Brewery Crest", Paths.ITEM_BREWERY_CREST, 1,"", 4800, Tag.ACCESSORY));
            AddToDictionary(CLOUD_CREST = new Item("Cloud Crest", Paths.ITEM_CLOUD_CREST, 1,"", 4000, Tag.ACCESSORY));
            AddToDictionary(BUTTERFLY_CHARM = new Item("Butterfly Charm", Paths.ITEM_BUTTERFLY_CHARM, 1,"", 310, Tag.ACCESSORY));
            AddToDictionary(DROPLET_CHARM = new Item("Droplet Charm", Paths.ITEM_DROPLET_CHARM, 1,"", 460, Tag.ACCESSORY));
            AddToDictionary(CHURN_CHARM = new Item("Churn Charm", Paths.ITEM_CHURN_CHARM, 1,"", 1300, Tag.ACCESSORY));
            AddToDictionary(PRIMAL_CHARM = new Item("Primal Charm", Paths.ITEM_PRIMAL_CHARM, 1,"", 675, Tag.ACCESSORY));
            AddToDictionary(SNOUT_CHARM = new Item("Snout Charm", Paths.ITEM_SNOUT_CHARM, 1,"", 1050, Tag.ACCESSORY));
            AddToDictionary(SUNRISE_CHARM = new Item("Sunrise Charm", Paths.ITEM_SUNRISE_CHARM, 1,"", 650, Tag.ACCESSORY));
            AddToDictionary(SUNFLOWER_CHARM = new Item("Sunflower Charm", Paths.ITEM_SUNFLOWER_CHARM, 1,"", 400, Tag.ACCESSORY));
            AddToDictionary(SALTY_CHARM = new Item("Salty Charm", Paths.ITEM_SALTY_CHARM, 1,"", 260, Tag.ACCESSORY));
            AddToDictionary(VOLCANIC_CHARM = new Item("Volcanic Charm", Paths.ITEM_VOLCANIC_CHARM, 1,"", 1330, Tag.ACCESSORY));
            AddToDictionary(SPINED_CHARM = new Item("Spined Charm", Paths.ITEM_SPINED_CHARM, 1,"", 945, Tag.ACCESSORY));
            AddToDictionary(MANTLE_CHARM = new Item("Mantle Charm", Paths.ITEM_MANTLE_CHARM, 1,"", 400, Tag.ACCESSORY));
            AddToDictionary(MUSHY_CHARM = new Item("Mushy Charm", Paths.ITEM_MUSHY_CHARM, 1,"", 750, Tag.ACCESSORY));
            AddToDictionary(DANDYLION_CHARM = new Item("Dandylion Charm", Paths.ITEM_DANDYLION_CHARM, 1,"", 640, Tag.ACCESSORY));
            AddToDictionary(LUMINOUS_RING = new Item("Luminous Ring", Paths.ITEM_LUMINOUS_RING, 1,"", 1350, Tag.ACCESSORY));
            AddToDictionary(BLIND_RING = new Item("Blind Ring", Paths.ITEM_BLIND_RING, 1,"", 1600, Tag.ACCESSORY));
            AddToDictionary(FLIGHT_RING = new Item("Flight Ring", Paths.ITEM_FLIGHT_RING, 1,"", 2770, Tag.ACCESSORY));
            AddToDictionary(FLORAL_RING = new Item("Floral Ring", Paths.ITEM_FLORAL_RING, 1,"", 2060, Tag.ACCESSORY));
            AddToDictionary(GLIMMER_RING = new Item("Glimmer Ring", Paths.ITEM_GLIMMER_RING, 1,"", 3350, Tag.ACCESSORY));
            AddToDictionary(MONOCULTURE_RING = new Item("Monoculture Ring", Paths.ITEM_MONOCULTURE_RING, 1,"", 1820, Tag.ACCESSORY));
            AddToDictionary(LUMBER_RING = new Item("Lumber Ring", Paths.ITEM_LUMBER_RING, 1,"", 1470, Tag.ACCESSORY));
            AddToDictionary(BAKERY_RING = new Item("Bakery Ring", Paths.ITEM_BAKERY_RING, 1,"", 1250, Tag.ACCESSORY));
            AddToDictionary(ROSE_RING = new Item("Rose Ring", Paths.ITEM_ROSE_RING, 1,"", 2400, Tag.ACCESSORY));
            AddToDictionary(OCEANIC_RING = new Item("Oceanic Ring", Paths.ITEM_OCEANIC_RING, 1,"", 3750, Tag.ACCESSORY));
            AddToDictionary(MUSICBOX_RING = new Item("Musicbox Ring", Paths.ITEM_MUSICBOX_RING, 1,"", 1330, Tag.ACCESSORY));
            AddToDictionary(SHELL_RING = new Item("Shell Ring", Paths.ITEM_SHELL_RING, 1,"", 2150, Tag.ACCESSORY));
            AddToDictionary(FURNACE_RING = new Item("Furnace Ring", Paths.ITEM_FURNACE_RING, 1,"", 2200, Tag.ACCESSORY));
            AddToDictionary(ACID_BRACER = new Item("Acid Bracer", Paths.ITEM_ACID_BRACER, 1,"", 1005, Tag.ACCESSORY));
            AddToDictionary(URCHIN_BRACER = new Item("Urchin Bracer", Paths.ITEM_URCHIN_BRACER, 1,"", 800, Tag.ACCESSORY));
            AddToDictionary(FLUFFY_BRACER = new Item("Fluffy Bracer", Paths.ITEM_FLUFFY_BRACER, 1,"", 1700, Tag.ACCESSORY));
            AddToDictionary(DRUID_BRACER = new Item("Druid Bracer", Paths.ITEM_DRUID_BRACER, 1,"", 1340, Tag.ACCESSORY));
            AddToDictionary(TRADITION_BRACER = new Item("Tradition Bracer", Paths.ITEM_TRADITION_BRACER, 1,"", 1450, Tag.ACCESSORY));
            AddToDictionary(SANDSTORM_BRACER = new Item("Sandstorm Bracer", Paths.ITEM_SANDSTORM_BRACER, 1,"", 1300, Tag.ACCESSORY));
            AddToDictionary(DWARVEN_CHILDS_BRACER = new Item("Dwarven Child\'s Bracer", Paths.ITEM_DWARVEN_CHILDS_BRACER, 1,"", 975, Tag.ACCESSORY));
            AddToDictionary(STRIPED_BRACER = new Item("Striped Bracer", Paths.ITEM_STRIPED_BRACER, 1,"", 580, Tag.ACCESSORY));
            AddToDictionary(CARNIVORE_BRACER = new Item("Carnivore Bracer", Paths.ITEM_CARNIVORE_BRACER, 1,"", 1350, Tag.ACCESSORY));
            AddToDictionary(PURIFICATION_BRACER = new Item("Purification Bracer", Paths.ITEM_PURIFICATION_BRACER, 1,"", 1225, Tag.ACCESSORY));
            AddToDictionary(SCRAP_BRACER = new Item("Scrap Bracer", Paths.ITEM_SCRAP_BRACER, 1,"", 650, Tag.ACCESSORY));
            AddToDictionary(PIN_BRACER = new Item("Pin Bracer", Paths.ITEM_PIN_BRACER, 1,"", 1200, Tag.ACCESSORY));
            AddToDictionary(ESSENCE_BRACER = new Item("Lunar Bracer", Paths.ITEM_ESSENCE_BRACER, 1,"", 1100, Tag.ACCESSORY));
            AddToDictionary(DISSECTION_PENDANT = new Item("Dissection Pendant", Paths.ITEM_DISSECTION_PENDANT, 1,"", 1020, Tag.ACCESSORY));
            AddToDictionary(SOUND_PENDANT = new Item("Sound Pendant", Paths.ITEM_SOUND_PENDANT, 1,"", 1220, Tag.ACCESSORY));
            AddToDictionary(GAIA_PENDANT = new Item("Gaia Pendant", Paths.ITEM_GAIA_PENDANT, 1,"", 3000, Tag.ACCESSORY));
            AddToDictionary(CYCLE_PENDANT = new Item("Cycle Pendant", Paths.ITEM_CYCLE_PENDANT, 1,"", 3200, Tag.ACCESSORY));
            AddToDictionary(EROSION_PENDANT = new Item("Erosion Pendant", Paths.ITEM_EROSION_PENDANT, 1,"", 3600, Tag.ACCESSORY));
            AddToDictionary(POLYCULTURE_PENDANT = new Item("Polyculture Pendant", Paths.ITEM_POLYCULTURE_PENDANT, 1,"", 1500, Tag.ACCESSORY));
            AddToDictionary(CONTRACT_PENDANT = new Item("Contract Pendant", Paths.ITEM_CONTRACT_PENDANT, 1,"", 1100, Tag.ACCESSORY));
            AddToDictionary(LADYBUG_PENDANT = new Item("Ladybug Pendant", Paths.ITEM_LADYBUG_PENDANT, 1,"", 1060, Tag.ACCESSORY));
            AddToDictionary(DYNAMITE_PENDANT = new Item("Dynamite Pendant", Paths.ITEM_DYNAMITE_PENDANT, 1,"", 1880, Tag.ACCESSORY));
            AddToDictionary(OILY_PENDANT = new Item("Oily Pendant", Paths.ITEM_OILY_PENDANT, 1,"", 1670, Tag.ACCESSORY));
            AddToDictionary(NEUTRALIZED_PENDANT = new Item("Neutralized Pendant", Paths.ITEM_NEUTRALIZED_PENDANT, 1,"", 1110, Tag.ACCESSORY));
            AddToDictionary(STREAMLINE_PENDANT = new Item("Streamline Pendant", Paths.ITEM_STREAMLINE_PENDANT, 1,"", 1610, Tag.ACCESSORY));
            AddToDictionary(TORNADO_PENDANT = new Item("Tornado Pendant", Paths.ITEM_TORNADO_PENDANT, 1,"", 1800, Tag.ACCESSORY));
           
            AddToDictionary(ORIGAMI_AIRPLANE = new Item("Origami Airplane", Paths.ITEM_ORIGAMI_AIRPLANE, 1,"", 400, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_BALL = new Item("Origami Ball", Paths.ITEM_ORIGAMI_BALL, 1,"", 350, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_BEETLE = new Item("Origami Beetle", Paths.ITEM_ORIGAMI_BEETLE, 1,"", 400, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_BOX = new Item("Origami Box", Paths.ITEM_ORIGAMI_BOX, 1,"", 420, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_DRAGON = new Item("Origami Dragon", Paths.ITEM_ORIGAMI_DRAGON, 1,"", 450, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_FAN = new Item("Origami Fan", Paths.ITEM_ORIGAMI_FAN, 1,"", 2600, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_FISH = new Item("Origami Fish", Paths.ITEM_ORIGAMI_FISH, 1,"", 390, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_FLOWER = new Item("Origami Flower", Paths.ITEM_ORIGAMI_FLOWER, 1,"", 440, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_FROG = new Item("Origami Frog", Paths.ITEM_ORIGAMI_FROG, 1,"", 390, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_LEAF = new Item("Origami Leaf", Paths.ITEM_ORIGAMI_LEAF, 1,"", 360, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_LION = new Item("Origami Lion", Paths.ITEM_ORIGAMI_LION, 1,"", 400, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_SAILBOAT = new Item("Origami Sailboat", Paths.ITEM_ORIGAMI_SAILBOAT, 1,"", 500, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_SWAN = new Item("Origami Swan", Paths.ITEM_ORIGAMI_SWAN, 1,"", 450, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_TIGER = new Item("Origami Tiger", Paths.ITEM_ORIGAMI_TIGER, 1,"", 490, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_TURTLE = new Item("Origami Turtle", Paths.ITEM_ORIGAMI_TURTLE, 1,"", 350, Tag.ACCESSORY));
            AddToDictionary(ORIGAMI_WHALE = new Item("Origami Whale", Paths.ITEM_ORIGAMI_WHALE, 1,"", 400, Tag.ACCESSORY));
            
            AddToDictionary(CLOTHING_NONE = new ClothingItem("clothingnone", Paths.ITEM_NONE, 1,"nodesc", 0, Paths.CLOTHING_NONE_SPRITESHEET, null));

            AddToDictionary(BACKPACK = new ClothingItem("Backpack", Paths.ITEM_BACKPACK, 1,"A convenient backpack. Favorite of hikers and students alike.", 1000, Paths.CLOTHING_BACKPACK_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(RUCKSACK = new ClothingItem("Rucksack", Paths.ITEM_RUCKSACK, 1,"The small size makes it popular for shorter outings.", 450, Paths.CLOTHING_RUCKSACK_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(CAPE = new ClothingItem("Cape", Paths.ITEM_CAPE, 1,"For superheroes only.", 1250, Paths.CLOTHING_CAPE_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(GUITAR = new ClothingItem("Guitar", Paths.ITEM_GUITAR, 1,"Worn on the back. Now all you need is a campfire.", 1700, Paths.CLOTHING_GUITAR_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(WOLF_TAIL = new ClothingItem("Wolf Tail", Paths.ITEM_WOLF_TAIL, 1,"A tail for those who howl at the moon.", 800, Paths.CLOTHING_WOLF_TAIL_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(FOX_TAIL = new ClothingItem("Fox Tail", Paths.ITEM_FOX_TAIL, 1,"Also usable as a paintbrush.", 1000, Paths.CLOTHING_FOX_TAIL_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(CAT_TAIL = new ClothingItem("Cat Tail", Paths.ITEM_CAT_TAIL, 1,"A decorative tail which resembles that of a feline.", 800, Paths.CLOTHING_CAT_TAIL_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(CLOCKWORK = new ClothingItem("Clockwork", Paths.ITEM_CLOCKWORK, 1,"Tick... tock... tick... tock...", 2200, Paths.CLOTHING_CLOCKWORK_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(ROBO_ARMS = new ClothingItem("Robo-Arms", Paths.ITEM_ROBO_ARMS, 1,"They called me crazy, but who's the mad scientist now?", 3600, Paths.CLOTHING_ROBO_ARMS_SPRITESHEET_DEFAULT, null, Item.Tag.BACK, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(EARRING_STUD = new ClothingItem("Earring Stud", Paths.ITEM_EARRING_STUD, 1,"It's been working out.", 300, Paths.CLOTHING_EARRING_STUD_SPRITESHEET_DEFAULT, null, Item.Tag.EARRINGS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(DANGLE_EARRING = new ClothingItem("Dangle Earring", Paths.ITEM_DANGLE_EARRING, 1,"It dangles a little, see?", 620, Paths.CLOTHING_DANGLE_EARRING_SPRITESHEET_DEFAULT, null, Item.Tag.EARRINGS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(PIERCING = new ClothingItem("Piercing", Paths.ITEM_PIERCING, 1,"Straight through the heart.", 530, Paths.CLOTHING_PIERCING_SPRITESHEET_DEFAULT, null, Item.Tag.EARRINGS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(GLASSES = new ClothingItem("Glasses", Paths.ITEM_GLASSES, 1,"These'll help you see!", 500, Paths.CLOTHING_GLASSES_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(BLINDFOLD = new ClothingItem("Blindfold", Paths.ITEM_BLINDFOLD, 1,"There is nothing outside, and nothing within.", 550, Paths.CLOTHING_BLINDFOLD_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(EYEPATCH = new ClothingItem("Eyepatch", Paths.ITEM_EYEPATCH, 1,"This is what happens when you run with shears.", 450, Paths.CLOTHING_EYEPATCH_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(GOGGLES = new ClothingItem("Goggles", Paths.ITEM_GOGGLES, 1,"Gogo Goggles!", 900, Paths.CLOTHING_GOGGLES_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(PROTECTIVE_VISOR = new ClothingItem("Protective Visor", Paths.ITEM_PROTECTIVE_VISOR, 1,"Just in case...", 950, Paths.CLOTHING_PROTECTIVE_VISOR_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(QUERADE_MASK = new ClothingItem("Querade Mask", Paths.ITEM_QUERADE_MASK, 1,"Exquisitely suited for a midnight ball.", 950, Paths.CLOTHING_QUERADE_MASK_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SNORKEL = new ClothingItem("Snorkel", Paths.ITEM_SNORKEL, 1,"Doesn't work without an oxygen tank...", 2300, Paths.CLOTHING_SNORKEL_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SUNGLASSES = new ClothingItem("Sunglasses", Paths.ITEM_SUNGLASSES, 1,"Too cool for school.", 1000, Paths.CLOTHING_SUNGLASSES_SPRITESHEET_DEFAULT, null, Item.Tag.GLASSES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(WOOL_MITTENS = new ClothingItem("Wool Mittens", Paths.ITEM_WOOL_MITTENS, 1,"Woven with love. This will keep your hands warm.", 330, Paths.CLOTHING_WOOL_MITTENS_SPRITESHEET_DEFAULT, null, Item.Tag.GLOVES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(WORK_GLOVES = new ClothingItem("Work Gloves", Paths.ITEM_WORK_GLOVES, 1,"Rough and tough. Built to last.", 430, Paths.CLOTHING_WORK_GLOVES_SPRITESHEET_DEFAULT, null, Item.Tag.GLOVES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(BOXING_MITTS = new ClothingItem("Boxing Mitts", Paths.ITEM_BOXING_MITTS, 1,"You ain't no air fighter, Mac!", 900, Paths.CLOTHING_BOXING_MITTS_SPRITESHEET_DEFAULT, null, Item.Tag.GLOVES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            
            AddToDictionary(BASEBALL_CAP = new ClothingItem("Baseball Cap", Paths.ITEM_BASEBALL_CAP, 1,"Great for casual wear.", 500, Paths.CLOTHING_BASEBALL_CAP_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TEN_GALLON = new ClothingItem("10 Gallon", Paths.ITEM_10_GALLON, 1,"This one has a mild defect. It's actually 11 gallons.", 350, Paths.CLOTHING_10_GALLON_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(BANDANA = new ClothingItem("Bandana", Paths.ITEM_BANDANA, 1,"Aye matey, I be doing the yardwork now!", 650, Paths.CLOTHING_BANDANA_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_HIDE_HAIR));
            AddToDictionary(BOWLER = new ClothingItem("Bowler", Paths.ITEM_BOWLER, 1,"What do you think, detective?", 900, Paths.CLOTHING_BOWLER_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(BUNNY_EARS = new ClothingItem("Bunny Ears", Paths.ITEM_BUNNY_EARS, 1,"You feel a strange desire for carrots...", 3200, Paths.CLOTHING_BUNNY_EARS_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_SHOW_HAIR));
            AddToDictionary(CAT_EARS = new ClothingItem("Cat Ears", Paths.ITEM_CAT_EARS, 1,"Meow!", 1700, Paths.CLOTHING_CAT_EARS_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_SHOW_HAIR));
            AddToDictionary(BUTTERFLY_CLIP = new ClothingItem("Butterfly Clip", Paths.ITEM_BUTTERFLY_CLIP, 1,"It resembles a butterfly. How quaint!", 100, Paths.CLOTHING_BUTTERFLY_CLIP_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_SHOW_HAIR));
            AddToDictionary(CAMEL_HAT = new ClothingItem("Camel Hat", Paths.ITEM_CAMEL_HAT, 1,"Named for the distinctive hump.", 600, Paths.CLOTHING_CAMEL_HAT_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(CHEFS_HAT = new ClothingItem("Chefs Hat", Paths.ITEM_CHEFS_HAT, 1,"Required for entry into any respectable cooking guild.", 2950, Paths.CLOTHING_CHEFS_HAT_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(CONICAL_FARMER = new ClothingItem("Conical Farmer", Paths.ITEM_CONICAL_FARMER, 1,"Complete protection from the blistering sun!", 400, Paths.CLOTHING_CONICAL_FARMER_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(DINO_MASK = new ClothingItem("Dino Mask", Paths.ITEM_DINO_MASK, 1,"That belongs in a museum!", 3200, Paths.CLOTHING_DINO_MASK_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_HIDE_HAIR));
            AddToDictionary(DOG_MASK = new ClothingItem("Dog Mask", Paths.ITEM_DOG_MASK, 1,"Who let the dogs out?", 1600, Paths.CLOTHING_DOG_MASK_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_HIDE_HAIR));
            AddToDictionary(FACEMASK = new ClothingItem("Facemask", Paths.ITEM_FACEMASK, 1,"You never know when you might be the lone survivor. Or when you'll need to do field surgery.", 900, Paths.CLOTHING_FACEMASK_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_SHOW_HAIR));
            AddToDictionary(FLAT_CAP = new ClothingItem("Flat Cap", Paths.ITEM_FLAT_CAP, 1,"You know Professor, this reminds me of a perculiar puzzle...", 1000, Paths.CLOTHING_FLAT_CAP_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(HEADBAND = new ClothingItem("Headband", Paths.ITEM_HEADBAND, 1,"Burn those calories!", 500, Paths.CLOTHING_HEADBAND_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_SHOW_HAIR));
            AddToDictionary(NIGHTCAP = new ClothingItem("Nightcap", Paths.ITEM_NIGHTCAP, 1,"Spin attack, go! Sleep attack, activate!", 1000, Paths.CLOTHING_NIGHTCAP_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(NIGHTMARE_MASK = new ClothingItem("Nightmare Mask", Paths.ITEM_NIGHTMARE_MASK, 1,"Not recommended for wear near lakes or elm trees.", 950, Paths.CLOTHING_NIGHTMARE_MASK_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SNAPBACK = new ClothingItem("Snapback", Paths.ITEM_SNAPBACK, 1,"Take that, and snap it back!", 750, Paths.CLOTHING_SNAPBACK_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SQUARE_HAT = new ClothingItem("Square Hat", Paths.ITEM_SQUARE_HAT, 1,"Doesn't make the wearer better at dancing, sadly.", 900, Paths.CLOTHING_SQUARE_HAT_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(STRAW_HAT = new ClothingItem("Straw Hat", Paths.ITEM_STRAW_HAT, 1,"The hat that broke the camel's back.", 580, Paths.CLOTHING_STRAW_HAT_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TOP_HAT = new ClothingItem("Top Hat", Paths.ITEM_TOP_HAT, 1,"Fit for a magically refined gentleman.", 1250, Paths.CLOTHING_TOP_HAT_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TRACE_TATTOO = new ClothingItem("Trace Tattoo", Paths.ITEM_TRACE_TATTOO, 1,"A strange removable tattoo. It may hold an unknown power.", 1800, Paths.CLOTHING_TRACE_TATTOO_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_SHOW_HAIR));
            AddToDictionary(WHISKERS = new ClothingItem("Whiskers", Paths.ITEM_WHISKERS, 1,"Meeeoooow!", 2000, Paths.CLOTHING_WHISKERS_SPRITESHEET_DEFAULT, null, Item.Tag.HAT, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.ALWAYS_SHOW_HAIR));

            AddToDictionary(SCARF = new ClothingItem("Scarf", Paths.ITEM_SCARF, 1,"A shield against the winter cold.", 620, Paths.CLOTHING_SCARF_SPRITESHEET_DEFAULT, null, Item.Tag.SCARF, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(ASCOT = new ClothingItem("Ascot", Paths.ITEM_ASCOT, 1,"Fancy!", 675, Paths.CLOTHING_ASCOT_SPRITESHEET_DEFAULT, null, Item.Tag.SCARF, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(MEDAL = new ClothingItem("Medal", Paths.ITEM_MEDAL, 1,"Sweet, sweet victory. (Yeah!)", 1500, Paths.CLOTHING_MEDAL_SPRITESHEET_DEFAULT, null, Item.Tag.SCARF, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(NECKWARMER = new ClothingItem("Neckwarmer", Paths.ITEM_NECKWARMER, 1,"Practically named itself.", 410, Paths.CLOTHING_NECKWARMER_SPRITESHEET_DEFAULT, null, Item.Tag.SCARF, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SASH = new ClothingItem("Sash", Paths.ITEM_SASH, 1,"Feeling sashy?", 420, Paths.CLOTHING_SASH_SPRITESHEET_DEFAULT, null, Item.Tag.SCARF, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TIE = new ClothingItem("Tie", Paths.ITEM_TIE, 1,"Looking sharp!", 600, Paths.CLOTHING_TIE_SPRITESHEET_DEFAULT, null, Item.Tag.SCARF, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(NECKLACE = new ClothingItem("Necklace", Paths.ITEM_NECKLACE, 1,"Made out of real pearls! This one is very valuable.", 4400, Paths.CLOTHING_NECKLACE_SPRITESHEET_DEFAULT, null, Item.Tag.SCARF, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(ALL_SEASON_JACKET = new ClothingItem("All-Season Jacket", Paths.ITEM_ALL_SEASON_JACKET, 1,"It's water AND windproof.", 1400, Paths.CLOTHING_ALL_SEASON_JACKET_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(APRON = new ClothingItem("Apron", Paths.ITEM_APRON, 1,"Flame retardant, but still suitable for the kitchen too.", 3200, Paths.CLOTHING_APRON_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(BATHROBE = new ClothingItem("Bathrobe", Paths.ITEM_BATHROBE, 1,"Like wearing a towel, but socially acceptable.", 4100, Paths.CLOTHING_BATHROBE_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(NOMAD_VEST = new ClothingItem("Nomad Vest", Paths.ITEM_NOMAD_VEST, 1,"Lightweight and made to move.", 3800, Paths.CLOTHING_NOMAD_VEST_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(HOODED_SWEATSHIRT = new ClothingItem("Hooded Sweatshirt", Paths.ITEM_HOODED_SWEATSHIRT, 1, "Perfect for lounging about on a lazy day.", 1300, Paths.CLOTHING_HOODED_SWEATSHIRT_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(ONESIE = new ClothingItem("Onesie", Paths.ITEM_ONESIE, 1,"But what would a twosie look like?", 2200, Paths.CLOTHING_ONESIE_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(OVERALLS = new ClothingItem("Overalls", Paths.ITEM_OVERALLS, 1,"A farming classic. Made for work!", 2200, Paths.CLOTHING_OVERALLS_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(OVERCOAT = new ClothingItem("Overcoat", Paths.ITEM_OVERCOAT, 1,"Smooth and suave.", 1900, Paths.CLOTHING_OVERCOAT_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(PUNK_JACKET = new ClothingItem("Punk Jacket", Paths.ITEM_PUNK_JACKET, 1, "Welcome to the Salty Spitoon, how tough are ya?", 1100, Paths.CLOTHING_PUNK_JACKET_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(RAINCOAT = new ClothingItem("Raincoat", Paths.ITEM_RAINCOAT, 1,"The beeswax repels rain. Classically yellow.", 1750, Paths.CLOTHING_RAINCOAT_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SPORTBALL_UNIFORM = new ClothingItem("Sportball Uniform", Paths.ITEM_SPORTBALL_UNIFORM, 1, "Batter up!/nPower play!/nPenalty kick!", 4600, Paths.CLOTHING_SPORTBALL_UNIFORM_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SUIT_JACKET = new ClothingItem("Suit Jacket", Paths.ITEM_SUIT_JACKET, 1,"Fit for any ballroom.", 1900, Paths.CLOTHING_SUIT_JACKET_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(WEDDING_DRESS = new ClothingItem("Wedding Dress", Paths.ITEM_WEDDING_DRESS, 1,"For her special day.", 5200, Paths.CLOTHING_WEDDING_DRESS_SPRITESHEET_DEFAULT, null, Item.Tag.OUTERWEAR, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(JEANS = new ClothingItem("Jeans", Paths.ITEM_JEANS, 1,"Pants made of thick denim. The working man's standby.", 750, Paths.CLOTHING_JEANS_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(CHINO_SHORTS = new ClothingItem("Chino Shorts", Paths.ITEM_CHINO_SHORTS, 1,"Comfy and easy to wear!", 540, Paths.CLOTHING_CHINO_SHORTS_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(JEAN_SHORTS = new ClothingItem("Jean Shorts", Paths.ITEM_JEAN_SHORTS, 1,"Jorts?", 450, Paths.CLOTHING_JEAN_SHORTS_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(CHINOS = new ClothingItem("Chinos", Paths.ITEM_CHINOS, 1,"AKA Khakis. But Khaki is a color. Chino is the technical name for this type of pant.", 750, Paths.CLOTHING_CHINOS_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(LONG_SKIRT = new ClothingItem("Long Skirt", Paths.ITEM_LONG_SKIRT, 1,"Surprisingly mobile.", 800, Paths.CLOTHING_LONG_SKIRT_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.DRAW_OVER_SHOES));
            AddToDictionary(PUFF_SKIRT = new ClothingItem("Puff Skirt", Paths.ITEM_PUFF_SKIRT, 1,"Jiggly!", 1850, Paths.CLOTHING_PUFF_SKIRT_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.DRAW_OVER_SHOES));
            AddToDictionary(SHORT_SKIRT = new ClothingItem("Short Skirt", Paths.ITEM_SHORT_SKIRT, 1,"Just the right length!", 470, Paths.CLOTHING_SHORT_SKIRT_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING, Item.Tag.DRAW_OVER_SHOES));
            AddToDictionary(SUPER_SHORTS = new ClothingItem("Super Shorts", Paths.ITEM_SUPER_SHORTS, 1,"These super shorts are super short.", 710, Paths.CLOTHING_SUPER_SHORTS_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TIGHTIES = new ClothingItem("Tighties", Paths.ITEM_TIGHTIES, 1,"And white-ies!", 820, Paths.CLOTHING_TIGHTIES_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TORN_JEANS = new ClothingItem("Torn Jeans", Paths.ITEM_TORN_JEANS, 1,"Just a touch of rebellion...", 950, Paths.CLOTHING_TORN_JEANS_SPRITESHEET_DEFAULT, null, Item.Tag.PANTS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(SAILCLOTH = new ClothingItem("Sailcloth", Paths.ITEM_SAILCLOTH, 1,"A traditional woven cloth of the mountains. Somehow, it lets you glide. Physicists are baffled by this enigma.", 1300, Paths.CLOTHING_SAILCLOTH_SPRITESHEET_DEFAULT, null, Item.Tag.SAILCLOTH, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(SHORT_SLEEVE_TEE = new ClothingItem("Short-sleeve Tee", Paths.ITEM_SHORT_SLEEVE_TEE, 1,"The classic!", 320, Paths.CLOTHING_SHORT_SLEEVE_TEE_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(BUTTON_DOWN = new ClothingItem("Button-Down", Paths.ITEM_BUTTON_DOWN, 1,"The term \"button-down\" refers to the collar. This type of shirt is actually called a button-up.", 2800, Paths.CLOTHING_BUTTON_DOWN_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(ISLANDER_TATTOO = new ClothingItem("Islander Tattoo", Paths.ITEM_ISLANDER_TATTOO, 1,"The glyph seems to hold some strange power...", 1200, Paths.CLOTHING_ISLANDER_TATTOO_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(LINEN_BUTTON = new ClothingItem("Linen Button", Paths.ITEM_LINEN_BUTTON, 1,"Perfect for a day on the beach.", 2750, Paths.CLOTHING_LINEN_BUTTON_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(LONG_SLEEVE_TEE = new ClothingItem("Long-sleeve Tee", Paths.ITEM_LONG_SLEEVE_TEE, 1,"A bit more heavyweight than a typical tee, too.", 600, Paths.CLOTHING_LONG_SLEEVE_TEE_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(PLAID_BUTTON = new ClothingItem("Plaid Button", Paths.ITEM_PLAID_BUTTON, 1,"Lumberjack made urban.", 3000, Paths.CLOTHING_PLAID_BUTTON_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(STRIPED_SHIRT = new ClothingItem("Striped Shirt", Paths.ITEM_STRIPED_SHIRT, 1,"Navel in nature.", 940, Paths.CLOTHING_STRIPED_SHIRT_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(SWEATER = new ClothingItem("Sweater", Paths.ITEM_SWEATER, 1,"Woven with warmth.", 1100, Paths.CLOTHING_SWEATER_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TANKER = new ClothingItem("Tanker", Paths.ITEM_TANKER, 1,"Run & gun.", 520, Paths.CLOTHING_TANKER_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TURTLENECK = new ClothingItem("Turtleneck", Paths.ITEM_TURTLENECK, 1,"Named after tortoises.", 900,Paths.CLOTHING_TURTLENECK_SPRITESHEET_DEFAULT, null, Item.Tag.SHIRT, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(SNEAKERS = new ClothingItem("Sneakers", Paths.ITEM_SNEAKERS, 1,"Made for running!", 500, Paths.CLOTHING_SNEAKERS_SPRITESHEET_DEFAULT, null, Item.Tag.SHOES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(FLASH_HEELS = new ClothingItem("Flash Heels", Paths.ITEM_FLASH_HEELS, 1,"Hit the climax!", 1350, Paths.CLOTHING_FLASH_HEELS_SPRITESHEET_DEFAULT, null, Item.Tag.SHOES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(WING_SANDLES = new ClothingItem("Wing Sandles", Paths.ITEM_WING_SANDLES, 1,"Footwear of choice for a long-forgotten civilization.", 1800, Paths.CLOTHING_WING_SANDLES_SPRITESHEET_DEFAULT, null, Item.Tag.SHOES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(HIGH_TOPS = new ClothingItem("High Tops", Paths.ITEM_HIGH_TOPS, 1,"Some pumped up kicks.", 800, Paths.CLOTHING_HIGH_TOPS_SPRITESHEET_DEFAULT, null, Item.Tag.SHOES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(TALL_BOOTS = new ClothingItem("Tall Boots", Paths.ITEM_TALL_BOOTS, 1,"Made for mountaineers.", 550, Paths.CLOTHING_TALL_BOOTS_SPRITESHEET_DEFAULT, null, Item.Tag.SHOES, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(SHORT_SOCKS = new ClothingItem("Short Socks", Paths.ITEM_SHORT_SOCKS, 1,"A basic pair of socks. Like, really basic.", 320, Paths.CLOTHING_SHORT_SOCKS_SPRITESHEET_DEFAULT, null, Item.Tag.SOCKS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(LONG_SOCKS = new ClothingItem("Long Socks", Paths.ITEM_LONG_SOCKS, 1,"Could also be called leggings.", 650, Paths.CLOTHING_LONG_SOCKS_SPRITESHEET_DEFAULT, null, Item.Tag.SOCKS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(STRIPED_SOCKS = new ClothingItem("Striped Socks", Paths.ITEM_STRIPED_SOCKS, 1,"The stripes add a little more pizazz.", 380, Paths.CLOTHING_STRIPED_SOCKS_SPRITESHEET_DEFAULT, null, Item.Tag.SOCKS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(FESTIVE_SOCKS = new ClothingItem("Festive Socks", Paths.ITEM_FESTIVE_SOCKS, 1,"Made for elves. Made by elves.", 520, Paths.CLOTHING_FESTIVE_SOCKS_SPRITESHEET_DEFAULT, null, Item.Tag.SOCKS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));
            AddToDictionary(MISMATTCHED = new ClothingItem("Mismattched", Paths.ITEM_MISMATTCHED, 1,"A laundry nightmare made material.", 1560, Paths.CLOTHING_MISMATTCHED_SPRITESHEET_DEFAULT, null, Item.Tag.SOCKS, Item.Tag.DYEABLE, Item.Tag.CLOTHING));

            AddToDictionary(HAIR_AFRO_ALFONSO = new ClothingItem("Afro Alfonso", Paths.ITEM_WOOD, 1,"mh1desc", 0, Paths.HAIR_AFRO_ALFONSO_SPRITESHEET, null, Item.Tag.HAIR, Item.Tag.HIDE_WHEN_HAT));
            AddToDictionary(HAIR_BAREBONES_BRIAN = new ClothingItem("Barebones Brian", Paths.ITEM_WOOD, 1,"mh2desc", 0, Paths.HAIR_BAREBONES_BRIAN_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_BENNY_BOWLCUT = new ClothingItem("Benny Bowlcut", Paths.ITEM_WOOD, 1,"mh3desc", 0, Paths.HAIR_BENNY_BOWLCUT_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_CARLOS_COOL = new ClothingItem("Carlos Cool", Paths.ITEM_WOOD, 1,"mh4desc", 0, Paths.HAIR_CARLOS_COOL_SPRITESHEET, null, Item.Tag.HAIR, Item.Tag.HIDE_WHEN_HAT));
            AddToDictionary(HAIR_CLEAN_CONOR = new ClothingItem("Clean Conor", Paths.ITEM_WOOD, 1,"mh5desc", 0, Paths.HAIR_CLEAN_CONOR_SPRITESHEET, null, Item.Tag.HAIR, Item.Tag.HIDE_WHEN_HAT));
            AddToDictionary(HAIR_COMBED_CHRISTOPH = new ClothingItem("Combed Chrisotph", Paths.ITEM_WOOD, 1,"mh6desc", 0, Paths.HAIR_COMBED_CHRISTOPH_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_COWLICK_COLTON = new ClothingItem("Cowlick Colton", Paths.ITEM_WOOD, 1,"mh7desc", 0, Paths.HAIR_COWLICK_COLTON_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_DIRTY_JACK = new ClothingItem("Dirty Jack", Paths.ITEM_WOOD, 1,"mh8desc", 0, Paths.HAIR_DIRTY_JACK_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_FREDDIE_FRINGE = new ClothingItem("Freddie Fringe", Paths.ITEM_WOOD, 1,"mh9desc", 0, Paths.HAIR_FREDDIE_FRINGE_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_GABRIEL_PART = new ClothingItem("Gabriel Part", Paths.ITEM_WOOD, 1,"mh10desc", 0, Paths.HAIR_GABRIEL_PART_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_LAZY_XAVIER = new ClothingItem("Lazy Xavier", Paths.ITEM_WOOD, 1,"mh11desc", 0, Paths.HAIR_LAZY_XAVIER_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_LUCKY_LUKE = new ClothingItem("Lucky Luke", Paths.ITEM_WOOD, 1,"mh12desc", 0, Paths.HAIR_LUCKY_LUKE_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_MAXWELL_MOHAWK = new ClothingItem("Maxwell Mohawk", Paths.ITEM_WOOD, 1,"mh13desc", 0, Paths.HAIR_MAXWELL_MOHAWK_SPRITESHEET, null, Item.Tag.HAIR, Item.Tag.HIDE_WHEN_HAT));
            AddToDictionary(HAIR_MR_BALD = new ClothingItem("Mr Bald", Paths.ITEM_WOOD, 1,"mh14desc", 0, Paths.HAIR_MR_BALD_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_OVERHANG_OWEN = new ClothingItem("Overhang Owen", Paths.ITEM_WOOD, 1,"mh15desc", 0, Paths.HAIR_OVERHANG_OWEN_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_PONYTAIL_TONYTALE = new ClothingItem("Ponytail Tonytale", Paths.ITEM_WOOD, 1,"mh16desc", 0, Paths.HAIR_PONYTAIL_TONYTALE_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_SKULLCAP_STEVENS = new ClothingItem("Skullcap Stevens", Paths.ITEM_WOOD, 1,"mh17desc", 0, Paths.HAIR_SKULLCAP_STEVENS_SPRITESHEET, null, Item.Tag.HAIR));

            AddToDictionary(HAIR_ALIENATED_ALICE = new ClothingItem("Alienated Alice", Paths.ITEM_WOOD, 1,"fh1desc", 0, Paths.HAIR_ALIENATED_ALICE_SPRITESHEET, null, Item.Tag.HAIR, Item.Tag.HIDE_WHEN_HAT));
            AddToDictionary(HAIR_BERTHA_BUN = new ClothingItem("Bertha Bun", Paths.ITEM_WOOD, 1,"fh2desc", 0, Paths.HAIR_BERTHA_BUN_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_CLEANCUT_CHARLOTTE = new ClothingItem("Cleancut Charlotte", Paths.ITEM_WOOD, 1,"fh3desc", 0, Paths.HAIR_CLEANCUT_CHARLOTTE_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_EARNEST_EMMA = new ClothingItem("Earnest Emma", Paths.ITEM_WOOD, 1,"fh4desc", 0, Paths.HAIR_EARNEST_EMMA_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_FLASHY_FRIZZLE = new ClothingItem("Flashy Frizzle", Paths.ITEM_WOOD, 1,"fh5desc", 0, Paths.HAIR_FLASHY_FRIZZLE_SPRITESHEET, null, Item.Tag.HAIR, Item.Tag.HIDE_WHEN_HAT));
            AddToDictionary(HAIR_FLUFFY_FELICIA = new ClothingItem("Fluffy Felicia", Paths.ITEM_WOOD, 1,"fh6desc", 0, Paths.HAIR_FLUFFY_FELICIA_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_GORGEOUS_GEORGEANN = new ClothingItem("Gorgeous Georgeann", Paths.ITEM_WOOD, 1,"fh7desc", 0, Paths.HAIR_GORGEOUS_GEORGEANN_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_HANGOVER_HANNA = new ClothingItem("Hangover Hanna", Paths.ITEM_WOOD, 1,"fh9desc", 0, Paths.HAIR_HANGOVER_HANNA_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_INNOCENT_ILIA = new ClothingItem("Innocent Ilia", Paths.ITEM_WOOD, 1,"fh10desc", 0, Paths.HAIR_INNOCENT_ILIA_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_LUXURY_LARA = new ClothingItem("Luxury Lara", Paths.ITEM_WOOD, 1,"fh11desc", 0, Paths.HAIR_LUXURY_LARA_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_MOUNTAIN_CLIMBER_MADELINE = new ClothingItem("Mountain Climber Madeline", Paths.ITEM_WOOD, 1,"fh12desc", 0, Paths.HAIR_MOUNTAIN_CLIMBER_MADELINE_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_PADMA_PERFECTION = new ClothingItem("Padma Perfection", Paths.ITEM_WOOD, 1,"fh13desc", 0, Paths.HAIR_PADMA_PERFECTION_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_PERSEPHONE_PUNK = new ClothingItem("Persephone Punk", Paths.ITEM_WOOD, 1,"fh14desc", 0, Paths.HAIR_PERSEPHONE_PUNK_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_SOPHIA_SWING = new ClothingItem("Sophia Swing", Paths.ITEM_WOOD, 1,"fh15desc", 0, Paths.HAIR_SOPHIA_SWING_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_STRICT_SUSIE = new ClothingItem("Strict Susie", Paths.ITEM_WOOD, 1,"fh16desc", 0, Paths.HAIR_STRICT_SUSIE_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_THE_ORIGINAL_OLIVIA = new ClothingItem("The Original Olivia", Paths.ITEM_WOOD, 1,"fh17desc", 0, Paths.HAIR_THE_ORIGINAL_OLIVIA_SPRITESHEET, null, Item.Tag.HAIR));
            AddToDictionary(HAIR_ZAPPY_ZADIE = new ClothingItem("Zappy Zadie", Paths.ITEM_WOOD, 1,"fh18desc", 0, Paths.HAIR_ZAPPY_ZADIE_SPRITESHEET, null, Item.Tag.HAIR));

            AddToDictionary(FACIALHAIR_BARON_MUSTACHE = new ClothingItem("Baron Mustache", Paths.ITEM_WOOD, 1, "faha1", 0, Paths.FACIALHAIR_BARON_MUSTACHE_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_BEARD = new ClothingItem("Beard", Paths.ITEM_WOOD, 1, "faha2", 0, Paths.FACIALHAIR_BEARD_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_CAVEMAN = new ClothingItem("Caveman", Paths.ITEM_WOOD, 1, "faha3", 0, Paths.FACIALHAIR_CAVEMAN_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_DROOPY = new ClothingItem("Droopy", Paths.ITEM_WOOD, 1, "faha4", 0, Paths.FACIALHAIR_DROOPY_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_FLUFF = new ClothingItem("Fluff", Paths.ITEM_WOOD, 1, "faha5", 0, Paths.FACIALHAIR_FLUFF_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_FULLBEARD = new ClothingItem("Fullbeard", Paths.ITEM_WOOD, 1, "faha6", 0, Paths.FACIALHAIR_FULLBEARD_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_GOATEE = new ClothingItem("Goatee", Paths.ITEM_WOOD, 1, "faha7", 0, Paths.FACIALHAIR_GOATEE_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_GOATEEBACK = new ClothingItem("Goatee Back", Paths.ITEM_WOOD, 1, "faha8", 0, Paths.FACIALHAIR_GOATEEBACK_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_MONK = new ClothingItem("Monk", Paths.ITEM_WOOD, 1, "faha9", 0, Paths.FACIALHAIR_MONK_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_SHORTBEARD = new ClothingItem("Shortbeard", Paths.ITEM_WOOD, 1, "faha10", 0, Paths.FACIALHAIR_SHORTBEARD_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_SIDEBURNS = new ClothingItem("Sideburns", Paths.ITEM_WOOD, 1, "faha11", 0, Paths.FACIALHAIR_SIDEBURNS_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));
            AddToDictionary(FACIALHAIR_SOULPATCH = new ClothingItem("Soulpatch", Paths.ITEM_WOOD, 1, "faha12", 0, Paths.FACIALHAIR_SOULPATCH_SPRITESHEET, null, Item.Tag.FACIAL_HAIR));

            AddToDictionary(SKIN_PEACH = new ClothingItem("Skin Peach", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_PEACH_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_ALIEN = new ClothingItem("Skin Alien", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_ALIEN_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_CHOCOLATE = new ClothingItem("Skin Chocolate", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_CHOCOLATE_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_DRIFTER = new ClothingItem("Skin Drifter", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_DRIFTER_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_ECRU = new ClothingItem("Skin Ecru", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_ECRU_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_EXEMPLAR = new ClothingItem("Skin Exemplar", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_EXEMPLAR_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_MERIDIAN = new ClothingItem("Skin Meridian", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_MERIDIAN_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_MIDNIGHT = new ClothingItem("Skin Midnight", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_MIDNIGHT_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_OLIVE = new ClothingItem("Skin Olive", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_OLIVE_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_PHANTOM = new ClothingItem("Skin Phantom", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_PHANTOM_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_RUSSET = new ClothingItem("Skin Russet", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_RUSSET_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_SNOW = new ClothingItem("Skin Snow", Paths.ITEM_WOOD, 1,"skpedesc", 0, Paths.SKIN_SNOW_SPRITESHEET, null, Item.Tag.SKIN));
            AddToDictionary(SKIN_KID = new ClothingItem("Skin Kid", Paths.ITEM_WOOD, 1, "skpedesc", 0, Paths.SKIN_KID_SPRITESHEET, null, Item.Tag.SKIN)); //skin used exclusively for Cade

            AddToDictionary(EYES_AMBER = new ClothingItem("Eyes Amber", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_AMBER_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_BLANK = new ClothingItem("Eyes Blank", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_BLANK_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_BLUSH = new ClothingItem("Eyes Blush", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_BLUSH_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_BROWN = new ClothingItem("Eyes Brown", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_BROWN_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_DOT = new ClothingItem("Eyes Dot", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_DOT_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_EMERALD = new ClothingItem("Eyes Emerald", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_EMERALD_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_MINT = new ClothingItem("Eyes Mint", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_MINT_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_OCEAN = new ClothingItem("Eyes Ocean", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_OCEAN_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_SILVER = new ClothingItem("Eyes Silver", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_SILVER_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_SOLAR = new ClothingItem("Eyes Solar", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_SOLAR_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_TEAK = new ClothingItem("Eyes Teak", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_TEAK_SPRITESHEET, null, Item.Tag.EYES));
            AddToDictionary(EYES_FROST = new ClothingItem("Eyes Frost", Paths.ITEM_WOOD, 1,"eyedesc", 0, Paths.EYES_FROST_SPRITESHEET, null, Item.Tag.EYES));

            AddToDictionary(SCAFFOLDING_WOOD = new BuildingBlockItem("Wooden Scaffolding", Paths.ITEM_SCAFFOLDING_WOOD, Paths.SPRITE_SCAFFOLDING_WOOD, BlockType.SCAFFOLDING, DEFAULT_STACK_SIZE, "Scaffolding made of wood. I can place this on my farm to build structures.", 10, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SCAFFOLDING_METAL = new BuildingBlockItem("Metal Scaffolding", Paths.ITEM_SCAFFOLDING_METAL, Paths.SPRITE_SCAFFOLDING_METAL, BlockType.SCAFFOLDING, DEFAULT_STACK_SIZE, "Chainlink made of iron. I can place this on my farm to build structures.", 50, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SCAFFOLDING_GOLDEN = new BuildingBlockItem("Golden Scaffolding", Paths.ITEM_SCAFFOLDING_GOLDEN, Paths.SPRITE_SCAFFOLDING_GOLDEN, BlockType.SCAFFOLDING, DEFAULT_STACK_SIZE, "An ornate pattern made of gold. I can place this on my farm to build structures.", 120, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(SCAFFOLDING_MYTHRIL = new BuildingBlockItem("Mythril Scaffolding", Paths.ITEM_SCAFFOLDING_MYTHRIL, Paths.SPRITE_SCAFFOLDING_MYTHRIL, BlockType.SCAFFOLDING, DEFAULT_STACK_SIZE, "Bars made of mythril. I can place this on my farm to build structures.", 110, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_WOOD = new BuildingBlockItem("Wooden Platform", Paths.ITEM_PLATFORM_WOOD, Paths.SPRITE_PLATFORM_WOOD, BlockType.PLATFORM, DEFAULT_STACK_SIZE, "Platform made of wood. I can place this on my farm to build structures. Platforms are solid on top.", 15, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_STONE = new BuildingBlockItem("Stone Platform", Paths.ITEM_PLATFORM_STONE, Paths.SPRITE_PLATFORM_STONE, BlockType.PLATFORM, DEFAULT_STACK_SIZE, "Platform made of stone. I can place this on my farm to build structures. Platforms are solid on top.", 30, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_METAL = new BuildingBlockItem("Metal Platform", Paths.ITEM_PLATFORM_METAL, Paths.SPRITE_PLATFORM_METAL, BlockType.PLATFORM, DEFAULT_STACK_SIZE, "Platform made of iron. I can place this on my farm to build structures. Platforms are solid on top.", 100, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_GOLDEN = new BuildingBlockItem("Golden Platform", Paths.ITEM_PLATFORM_GOLDEN, Paths.SPRITE_PLATFORM_GOLDEN, BlockType.PLATFORM, DEFAULT_STACK_SIZE, "Platform made of gold. I can place this on my farm to build structures. Platforms are solid on top.", 240, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_MYTHRIL = new BuildingBlockItem("Mythril Platform", Paths.ITEM_PLATFORM_MYTHRIL, Paths.SPRITE_PLATFORM_MYTHRIL, BlockType.PLATFORM, DEFAULT_STACK_SIZE, "Platform made of mythril. I can place on my farm this to build structures. Platforms are solid on top.", 210, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_WOOD_FARM = new BuildingBlockItem("Wooden Farming Platform", Paths.ITEM_PLATFORM_WOOD_FARM, Paths.SPRITE_PLATFORM_WOOD_FARM, BlockType.PLATFORM_FARM, DEFAULT_STACK_SIZE, "Platform made of wood. I can place this on my farm to build structures. The dirt on the top is tillable soil.", 30, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_METAL_FARM = new BuildingBlockItem("Metal Farming Platform", Paths.ITEM_PLATFORM_METAL_FARM, Paths.SPRITE_PLATFORM_METAL_FARM, BlockType.PLATFORM_FARM, DEFAULT_STACK_SIZE, "Platform made of iron. I can place this on my farm to build structures. The dirt on the top is tillable soil.", 150, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_STONE_FARM = new BuildingBlockItem("Stone Farming Platform", Paths.ITEM_PLATFORM_STONE_FARM, Paths.SPRITE_PLATFORM_STONE_FARM, BlockType.PLATFORM_FARM, DEFAULT_STACK_SIZE, "Platform made of stone. I can place this on my farm to build structures. The dirt on the top is tillable soil.", 40, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_GOLDEN_FARM = new BuildingBlockItem("Golden Farming Platform", Paths.ITEM_PLATFORM_GOLDEN_FARM, Paths.SPRITE_PLATFORM_GOLDEN_FARM, BlockType.PLATFORM_FARM, DEFAULT_STACK_SIZE, "Platform made of gold. I can place this on my farm to build structures. The dirt on the top is tillable soil.", 280, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_MYTHRIL_FARM = new BuildingBlockItem("Mythril Farming Platform", Paths.ITEM_PLATFORM_MYTHRIL_FARM, Paths.SPRITE_PLATFORM_MYTHRIL_FARM, BlockType.PLATFORM_FARM, DEFAULT_STACK_SIZE, "Platform made of mythril. I can place this on my farm to build structures. The dirt on the top is tillable soil.", 250, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLOCK_WOOD = new BuildingBlockItem("Wooden Block", Paths.ITEM_BLOCK_WOOD, Paths.SPRITE_BLOCK_WOOD, BlockType.BLOCK, DEFAULT_STACK_SIZE, "A square block made of wood. I can place this on my farm to build structures.", 25, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLOCK_METAL = new BuildingBlockItem("Metal Block", Paths.ITEM_BLOCK_METAL, Paths.SPRITE_BLOCK_METAL, BlockType.BLOCK, DEFAULT_STACK_SIZE, "A metal block made of iron. I can place this on my farm to build structures.", 140, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLOCK_STONE = new BuildingBlockItem("Stone Block", Paths.ITEM_BLOCK_STONE, Paths.SPRITE_BLOCK_STONE, BlockType.BLOCK, DEFAULT_STACK_SIZE, "A solid block made of stone. I can place this on my farm to build structures.", 45, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLOCK_GOLDEN = new BuildingBlockItem("Golden Block", Paths.ITEM_BLOCK_GOLDEN, Paths.SPRITE_BLOCK_GOLDEN, BlockType.BLOCK, DEFAULT_STACK_SIZE, "A shining block made of gold. I can place this on my farm to build structures.", 360, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLOCK_MYTHRIL = new BuildingBlockItem("Mythril Block", Paths.ITEM_BLOCK_MYTHRIL, Paths.SPRITE_BLOCK_MYTHRIL, BlockType.BLOCK, DEFAULT_STACK_SIZE, "A blue block made of mythril. I can place on my farm this to build structures.", 310, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WALL_STONE = new BuildingBlockItem("Stone Wall", Paths.ITEM_WALL_STONE, Paths.SPRITE_WALL_STONE, BlockType.SCAFFOLDING, DEFAULT_STACK_SIZE, "Walls made of stone. I can place this on my farm to build structures.", 15, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WALL_METAL = new BuildingBlockItem("Metal Wall", Paths.ITEM_WALL_METAL, Paths.SPRITE_WALL_METAL, BlockType.SCAFFOLDING, DEFAULT_STACK_SIZE, "Walls made of metal. I can place this on my farm to build structures.", 10, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(WALL_PLANK = new BuildingBlockItem("Plank Wall", Paths.ITEM_WALL_PLANK, Paths.SPRITE_WALL_PLANK, BlockType.SCAFFOLDING, DEFAULT_STACK_SIZE, "Walls made from planks. I can place this on my farm to build structures.", 25, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(PLATFORM_PLANK = new BuildingBlockItem("Plank Platform", Paths.ITEM_PLATFORM_PLANK, Paths.SPRITE_PLATFORM_PLANK, BlockType.PLATFORM, DEFAULT_STACK_SIZE, "Platform made of planks. I can place this on my farm to build structures. Platforms are solid on top.", 35, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLOCK_PLANK = new BuildingBlockItem("Plank Block", Paths.ITEM_BLOCK_PLANK, Paths.SPRITE_BLOCK_PLANK, BlockType.BLOCK, DEFAULT_STACK_SIZE, "A crafty block made of planks. I can place this on my farm to build structures.", 50, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));
            AddToDictionary(BLOCK_BARK = new BuildingBlockItem("Bark Block", Paths.ITEM_BLOCK_BARK, Paths.SPRITE_BLOCK_BARK, BlockType.BLOCK, DEFAULT_STACK_SIZE, "A natural block made of bark. I can place this on my farm to build structures.", 25, null, Item.Tag.DYEABLE, Item.Tag.PLACEABLE));


            //preload all but recolors
            LoadAllInDictionary();

            if (!SKIP_RECOLORS)
            {
                foreach (Util.RecolorMap color in Util.DYE_COLORS)
                {
                    AddToDictionary(MakeColoredVersion(CHEST, color, Paths.ITEM_CHEST_GREYSCALE, Paths.ITEM_CHEST, Paths.SPRITE_CHEST_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(COMPOST_BIN, color, Paths.ITEM_COMPOST_BIN_GREYSCALE, Paths.ITEM_COMPOST_BIN, Paths.SPRITE_COMPOST_BIN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TERRARIUM, color, Paths.ITEM_TERRARIUM_GREYSCALE, Paths.ITEM_TERRARIUM, Paths.SPRITE_TERRARIUM_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DAIRY_CHURN, color, Paths.ITEM_DAIRY_CHURN_GREYSCALE, Paths.ITEM_DAIRY_CHURN, Paths.SPRITE_DAIRY_CHURN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MAYONNAISE_MAKER, color, Paths.ITEM_MAYONNAISE_MAKER_GREYSCALE, Paths.ITEM_MAYONNAISE_MAKER, Paths.SPRITE_MAYONNAISE_MAKER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LOOM, color, Paths.ITEM_LOOM_GREYSCALE, Paths.ITEM_LOOM, Paths.SPRITE_LOOM_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CHEFS_TABLE, color, Paths.ITEM_CHEFS_TABLE_GREYSCALE, Paths.ITEM_CHEFS_TABLE, Paths.SPRITE_CHEFS_TABLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLAY_OVEN, color, Paths.ITEM_CLAY_OVEN_GREYSCALE, Paths.ITEM_CLAY_OVEN, Paths.SPRITE_CLAY_OVEN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PERFUMERY, color, Paths.ITEM_PERFUMERY_GREYSCALE, Paths.ITEM_PERFUMERY, Paths.SPRITE_PERFUMERY_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BEEHIVE, color, Paths.ITEM_BEEHIVE_GREYSCALE, Paths.ITEM_BEEHIVE, Paths.SPRITE_BEEHIVE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BIRDHOUSE, color, Paths.ITEM_BIRDHOUSE_GREYSCALE, Paths.ITEM_BIRDHOUSE, Paths.SPRITE_BIRDHOUSE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SEED_MAKER, color, Paths.ITEM_SEED_MAKER_GREYSCALE, Paths.ITEM_SEED_MAKER, Paths.SPRITE_SEED_MAKER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(POTTERY_WHEEL, color, Paths.ITEM_POTTERY_WHEEL_GREYSCALE, Paths.ITEM_POTTERY_WHEEL, Paths.SPRITE_POTTERY_WHEEL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FURNACE, color, Paths.ITEM_FURNACE_GREYSCALE, Paths.ITEM_FURNACE, Paths.SPRITE_FURNACE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GEMSTONE_REPLICATOR, color, Paths.ITEM_GEMSTONE_REPLICATOR_GREYSCALE, Paths.ITEM_GEMSTONE_REPLICATOR, Paths.SPRITE_GEMSTONE_REPLICATOR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(COMPRESSOR, color, Paths.ITEM_COMPRESSOR_GREYSCALE, Paths.ITEM_COMPRESSOR, Paths.SPRITE_COMPRESSOR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MUSHBOX, color, Paths.ITEM_MUSHBOX_GREYSCALE, Paths.ITEM_MUSHBOX, Paths.SPRITE_MUSHBOX_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SOULCHEST, color, Paths.ITEM_SOULCHEST_GREYSCALE, Paths.ITEM_SOULCHEST, Paths.SPRITE_SOULCHEST_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FLOWERBED, color, Paths.ITEM_FLOWERBED_GREYSCALE, Paths.ITEM_FLOWERBED, Paths.SPRITE_FLOWERBED_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GLASSBLOWER, color, Paths.ITEM_GLASSBLOWER_GREYSCALE, Paths.ITEM_GLASSBLOWER, Paths.SPRITE_GLASSBLOWER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(AQUARIUM, color, Paths.ITEM_AQUARIUM_GREYSCALE, Paths.ITEM_AQUARIUM, Paths.SPRITE_AQUARIUM_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ALCHEMY_CAULDRON, color, Paths.ITEM_ALCHEMY_CAULDRON_GREYSCALE, Paths.ITEM_ALCHEMY_CAULDRON, Paths.SPRITE_ALCHEMY_CAULDRON_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ANVIL, color, Paths.ITEM_ANVIL_GREYSCALE, Paths.ITEM_ANVIL, Paths.SPRITE_ANVIL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(KEG, color, Paths.ITEM_KEG_GREYSCALE, Paths.ITEM_KEG, Paths.SPRITE_KEG_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SKY_STATUE, color, Paths.ITEM_SKY_STATUE_GREYSCALE, Paths.ITEM_SKY_STATUE, Paths.SPRITE_SKY_STATUE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DRACONIC_PILLAR, color, Paths.ITEM_DRACONIC_PILLAR_GREYSCALE, Paths.ITEM_DRACONIC_PILLAR, Paths.SPRITE_DRACONIC_PILLAR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WORKBENCH, color, Paths.ITEM_WORKBENCH_GREYSCALE, Paths.ITEM_WORKBENCH, Paths.SPRITE_WORKBENCH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(VIVARIUM, color, Paths.ITEM_VIVARIUM_GREYSCALE, Paths.ITEM_VIVARIUM, Paths.SPRITE_VIVARIUM_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SYNTHESIZER, color, Paths.ITEM_SYNTHESIZER_GREYSCALE, Paths.ITEM_SYNTHESIZER, Paths.SPRITE_SYNTHESIZER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTERS_PRESS, color, Paths.ITEM_PAINTERS_PRESS_GREYSCALE, Paths.ITEM_PAINTERS_PRESS, Paths.SPRITE_PAINTERS_PRESS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(JEWELERS_BENCH, color, Paths.ITEM_JEWELERS_BENCH_GREYSCALE, Paths.ITEM_JEWELERS_BENCH, Paths.SPRITE_JEWELERS_BENCH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BARBER_POLE, color, Paths.ITEM_BARBER_POLE_GREYSCALE, Paths.ITEM_BARBER_POLE, Paths.SPRITE_BARBER_POLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ENCHANTED_VANITY, color, Paths.ITEM_ENCHANTED_VANITY_GREYSCALE, Paths.ITEM_ENCHANTED_VANITY, Paths.SPRITE_ENCHANTED_VANITY_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DRYING_RACK, color, Paths.ITEM_DRYING_RACK_GREYSCALE, Paths.ITEM_DRYING_RACK, Paths.SPRITE_DRYING_RACK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLONING_MACHINE, color, Paths.ITEM_CLONING_MACHINE_GREYSCALE, Paths.ITEM_CLONING_MACHINE, Paths.SPRITE_CLONING_MACHINE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ORIGAMI_STATION, color, Paths.ITEM_ORIGAMI_STATION_GREYSCALE, Paths.ITEM_ORIGAMI_STATION, Paths.SPRITE_ORIGAMI_STATION_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(RECYCLER, color, Paths.ITEM_RECYCLER_GREYSCALE, Paths.ITEM_RECYCLER, Paths.SPRITE_RECYCLER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ALCHEMIZER, color, Paths.ITEM_ALCHEMIZER_GREYSCALE, Paths.ITEM_ALCHEMIZER, Paths.SPRITE_ALCHEMIZER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(EXTRACTOR, color, Paths.ITEM_EXTRACTOR_GREYSCALE, Paths.ITEM_EXTRACTOR, Paths.SPRITE_EXTRACTOR_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(HORSESHOE, color, Paths.ITEM_HORSESHOE_GREYSCALE, Paths.ITEM_HORSESHOE, Paths.SPRITE_HORSESHOE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HERALDIC_SHIELD, color, Paths.ITEM_HERALIDIC_SHIELD_GREYSCALE, Paths.ITEM_HERALDIC_SHIELD, Paths.SPRITE_HERALDIC_SHIELD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DECORATIVE_SWORD, color, Paths.ITEM_DECORATIVE_SWORD_GREYSCALE, Paths.ITEM_DECORATIVE_SWORD, Paths.SPRITE_DECORATIVE_SWORD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SUIT_OF_ARMOR, color, Paths.ITEM_SUIT_OF_ARMOR_GREYSCALE, Paths.ITEM_SUIT_OF_ARMOR, Paths.SPRITE_SUIT_OF_ARMOR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TORCH, color, Paths.ITEM_TORCH_GREYSCALE, Paths.ITEM_TORCH, Paths.SPRITE_TORCH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ANATOMICAL_POSTER, color, Paths.ITEM_ANATOMICAL_POSTER_GREYSCALE, Paths.ITEM_ANATOMICAL_POSTER, Paths.SPRITE_ANATOMICAL_POSTER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BAMBOO_POT, color, Paths.ITEM_BAMBOO_POT_GREYSCALE, Paths.ITEM_BAMBOO, Paths.SPRITE_BAMBOO_POT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BANNER, color, Paths.ITEM_BANNER_GREYSCALE, Paths.ITEM_BANNER, Paths.SPRITE_BANNER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BELL, color, Paths.ITEM_BELL_GREYSCALE, Paths.ITEM_BELL, Paths.SPRITE_BELL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLACKBOARD, color, Paths.ITEM_BLACKBOARD_GREYSCALE, Paths.ITEM_BLACKBOARD, Paths.SPRITE_BLACKBOARD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BOOMBOX, color, Paths.ITEM_BOOMBOX_GREYSCALE, Paths.ITEM_BOOMBOX, Paths.SPRITE_BOOMBOX_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BOX, color, Paths.ITEM_BOX_GREYSCALE, Paths.ITEM_BOX, Paths.SPRITE_BOX_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BRAZIER, color, Paths.ITEM_BRAZIER_GREYSCALE, Paths.ITEM_BRAZIER, Paths.SPRITE_BRAZIER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BUOY, color, Paths.ITEM_BUOY_GREYSCALE, Paths.ITEM_BUOY, Paths.SPRITE_BUOY_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CAMPFIRE, color, Paths.ITEM_CAMPFIRE_GREYSCALE, Paths.ITEM_CAMPFIRE, Paths.SPRITE_CAMPFIRE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CANVAS, color, Paths.ITEM_CANVAS_GREYSCALE, Paths.ITEM_CANVAS, Paths.SPRITE_CANVAS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CART, color, Paths.ITEM_CART_GREYSCALE, Paths.ITEM_CART, Paths.SPRITE_CART_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLAY_BALL, color, Paths.ITEM_CLAY_BALL_GREYSCALE, Paths.ITEM_CLAY_BALL, Paths.SPRITE_CLAY_BALL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLAY_BOWL, color, Paths.ITEM_CLAY_BOWL_GREYSCALE, Paths.ITEM_CLAY_BOWL, Paths.SPRITE_CLAY_BOWL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLAY_DOLL, color, Paths.ITEM_CLAY_DOLL_GREYSCALE, Paths.ITEM_CLAY_DOLL, Paths.SPRITE_CLAY_DOLL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLAY_SLATE, color, Paths.ITEM_CLAY_SLATE_GREYSCALE, Paths.ITEM_CLAY_SLATE, Paths.SPRITE_CLAY_SLATE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLOCK, color, Paths.ITEM_CLOCK_GREYSCALE, Paths.ITEM_CLOCK, Paths.SPRITE_CLOCK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLOTHESLINE, color, Paths.ITEM_CLOTHESLINE_GREYSCALE, Paths.ITEM_CLOTHESLINE, Paths.SPRITE_CLOTHESLINE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CRATE, color, Paths.ITEM_CRATE_GREYSCALE, Paths.ITEM_CRATE, Paths.SPRITE_CRATE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CUBE_STATUE, color, Paths.ITEM_CUBE_STATUE_GREYSCALE, Paths.ITEM_CUBE_STATUE, Paths.SPRITE_CUBE_STATUE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CYMBAL, color, Paths.ITEM_CYMBAL_GREYSCALE, Paths.ITEM_CYMBAL, Paths.SPRITE_CYMBAL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DECORATIVE_BOULDER, color, Paths.ITEM_DECORATIVE_BOULDER_GREYSCALE, Paths.ITEM_DECORATIVE_BOULDER, Paths.SPRITE_DECORATIVE_BOULDER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DECORATIVE_LOG, color, Paths.ITEM_DECORATIVE_LOG_GREYSCALE, Paths.ITEM_DECORATIVE_LOG, Paths.SPRITE_DECORATIVE_LOG_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DRUM, color, Paths.ITEM_DRUM_GREYSCALE, Paths.ITEM_DRUM, Paths.SPRITE_DRUM_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FIRE_HYDRANT, color, Paths.ITEM_FIRE_HYDRANT_GREYSCALE, Paths.ITEM_FIRE_HYDRANT, Paths.SPRITE_FIRE_HYDRANT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FIREPIT, color, Paths.ITEM_FIREPIT_GREYSCALE, Paths.ITEM_FIREPIT, Paths.SPRITE_FIREPIT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FIREPLACE, color, Paths.ITEM_FIREPLACE_GREYSCALE, Paths.ITEM_FIREPLACE, Paths.SPRITE_FIREPLACE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FLAGPOLE, color, Paths.ITEM_FLAGPOLE_GREYSCALE, Paths.ITEM_FLAGPOLE, Paths.SPRITE_FLAGPOLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FROST_SCULPTURE, color, Paths.ITEM_FROST_SCULPTURE_GREYSCALE, Paths.ITEM_FROST_SCULPTURE, Paths.SPRITE_FROST_SCULPTURE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FULL_THROTTLE_GRAFFITI, color, Paths.ITEM_FULL_THROTTLE_GRAFFITI_GREYSCALE, Paths.ITEM_FULL_THROTTLE_GRAFFITI, Paths.SPRITE_FULL_THROTTLE_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GARDEN_ARCH, color, Paths.ITEM_GARDEN_ARCH_GREYSCALE, Paths.ITEM_GARDEN_ARCH, Paths.SPRITE_GARDEN_ARCH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GRANDFATHER_CLOCK, color, Paths.ITEM_GRANDFATHER_CLOCK_GREYSCALE, Paths.ITEM_GRANDFATHER_CLOCK, Paths.SPRITE_GRANDFATHER_CLOCK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GUITAR_PLACEABLE, color, Paths.ITEM_GUITAR_PLACEABLE_GREYSCALE, Paths.ITEM_GUITAR_PLACEABLE, Paths.SPRITE_GUITAR_PLACEABLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GYM_BENCH, color, Paths.ITEM_GYM_BENCH_GREYSCALE, Paths.ITEM_GYM_BENCH, Paths.SPRITE_GYM_BENCH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HAMMOCK, color, Paths.ITEM_HAMMOCK_GREYSCALE, Paths.ITEM_HAMMOCK, Paths.SPRITE_HAMMOCK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HAYBALE, color, Paths.ITEM_HAYBALE_GREYSCALE, Paths.ITEM_HAYBALE, Paths.SPRITE_HAYBALE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HEARTBREAK_GRAFFITI, color, Paths.ITEM_HEARTBREAK_GRAFFITI_GREYSCALE, Paths.ITEM_HEARTBREAK_GRAFFITI, Paths.SPRITE_HEARTBREAK_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HELIX_POSTER, color, Paths.ITEM_HELIX_POSTER_GREYSCALE, Paths.ITEM_HELIX_POSTER, Paths.SPRITE_HELIX_POSTER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HEROINE_GRAFFITI, color, Paths.ITEM_HEROINE_GRAFFITI_GREYSCALE, Paths.ITEM_HEROINE_GRAFFITI, Paths.SPRITE_HEROINE_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HORIZONTAL_MIRROR, color, Paths.ITEM_HORIZONTAL_MIRROR_GREYSCALE, Paths.ITEM_HORIZONTAL_MIRROR, Paths.SPRITE_HORIZONTAL_MIRROR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ICE_BLOCK, color, Paths.ITEM_ICE_BLOCK_GREYSCALE, Paths.ITEM_ICE_BLOCK, Paths.SPRITE_ICE_BLOCK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(IGLOO, color, Paths.ITEM_IGLOO_GREYSCALE, Paths.ITEM_IGLOO, Paths.SPRITE_IGLOO_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LAMP, color, Paths.ITEM_LAMP_GREYSCALE, Paths.ITEM_LAMP, Paths.SPRITE_LAMP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LANTERN, color, Paths.ITEM_LANTERN_GREYSCALE, Paths.ITEM_LANTERN, Paths.SPRITE_LANTERN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LATTICE, color, Paths.ITEM_LATTICE_GREYSCALE, Paths.ITEM_LATTICE, Paths.SPRITE_LATTICE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LEFTWARD_GRAFFITI, color, Paths.ITEM_LEFTWARD_GRAFFITI_GREYSCALE, Paths.ITEM_LEFTWARD_GRAFFITI, Paths.SPRITE_LEFTWARD_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LIFEBUOY_SIGN, color, Paths.ITEM_LIFEBUOY_SIGN_GREYSCALE, Paths.ITEM_LIFEBUOY_SIGN, Paths.SPRITE_LIFEBUOY_SIGN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LIGHTNING_ROD, color, Paths.ITEM_LIGHTNING_ROD_GREYSCALE, Paths.ITEM_LIGHTNING_ROD, Paths.SPRITE_LIGHTNING_ROD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MAILBOX, color, Paths.ITEM_MAILBOX_GREYSCALE, Paths.ITEM_MAILBOX, Paths.SPRITE_MAILBOX_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MARKET_STALL, color, Paths.ITEM_MARKET_STALL_GREYSCALE, Paths.ITEM_MARKET_STALL, Paths.SPRITE_MARKET_STALL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MILK_JUG, color, Paths.ITEM_MILK_JUG_GREYSCALE, Paths.ITEM_MILK_JUG, Paths.SPRITE_MILK_JUG_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MINECART, color, Paths.ITEM_MINECART_GREYSCALE, Paths.ITEM_MINECART, Paths.SPRITE_MINECART_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(NOIZEBOYZ_GRAFFITI, color, Paths.ITEM_NOIZEBOYZ_GRAFFITI_GREYSCALE, Paths.ITEM_NOIZEBOYZ_GRAFFITI, Paths.SPRITE_NOIZEBOYZ_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ORNATE_MIRROR, color, Paths.ITEM_ORNATE_MIRROR_GREYSCALE, Paths.ITEM_ORNATE_MIRROR, Paths.SPRITE_ORNATE_MIRROR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PET_BOWL, color, Paths.ITEM_PET_BOWL_GREYSCALE, Paths.ITEM_PET_BOWL, Paths.SPRITE_PET_BOWL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PIANO, color, Paths.ITEM_PIANO_GREYSCALE, Paths.ITEM_PIANO, Paths.SPRITE_PIANO_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PILE_OF_BRICKS, color, Paths.ITEM_PILE_OF_BRICKS_GREYSCALE, Paths.ITEM_PILE_OF_BRICKS, Paths.SPRITE_PILE_OF_BRICKS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(POSTBOX, color, Paths.ITEM_POSTBOX_GREYSCALE, Paths.ITEM_POSTBOX, Paths.SPRITE_POSTBOX_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(POTTERY_JAR, color, Paths.ITEM_POTTERY_JAR_GREYSCALE, Paths.ITEM_POTTERY_JAR, Paths.SPRITE_POTTERY_JAR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(POTTERY_MUG, color, Paths.ITEM_POTTERY_MUG_GREYSCALE, Paths.ITEM_POTTERY_MUG, Paths.SPRITE_POTTERY_MUG_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(POTTERY_PLATE, color, Paths.ITEM_POTTERY_PLATE_GREYSCALE, Paths.ITEM_POTTERY_PLATE, Paths.SPRITE_POTTERY_PLATE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(POTTERY_VASE, color, Paths.ITEM_POTTERY_VASE_GREYSCALE, Paths.ITEM_POTTERY_VASE, Paths.SPRITE_POTTERY_VASE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PYRAMID_STATUE, color, Paths.ITEM_PYRAMID_STATUE_GREYSCALE, Paths.ITEM_PYRAMID_STATUE, Paths.SPRITE_PYRAMID_STATUE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(RAINBOW_GRAFFITI, color, Paths.ITEM_RAINBOW_GRAFFITI_GREYSCALE, Paths.ITEM_RAINBOW_GRAFFITI, Paths.SPRITE_RAINBOW_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(RECYCLING_BIN, color, Paths.ITEM_RECYCLING_BIN_GREYSCALE, Paths.ITEM_RECYCLING_BIN, Paths.SPRITE_RECYCLING_BIN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(RETRO_GRAFFITI, color, Paths.ITEM_RETRO_GRAFFITI_GREYSCALE, Paths.ITEM_RETRO_GRAFFITI, Paths.SPRITE_RETRO_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(RIGHT_ARROW_GRAFFITI, color, Paths.ITEM_RIGHT_ARROW_GRAFFITI_GREYSCALE, Paths.ITEM_RIGHT_ARROW_GRAFFITI, Paths.SPRITE_RIGHT_ARROW_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SANDBOX, color, Paths.ITEM_SANDBOX_GREYSCALE, Paths.ITEM_SANDBOX, Paths.SPRITE_SANDBOX_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SANDCASTLE, color, Paths.ITEM_SANDCASTLE_GREYSCALE, Paths.ITEM_SANDCASTLE, Paths.SPRITE_SANDCASTLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SCARECROW, color, Paths.ITEM_SCARECROW_GREYSCALE, Paths.ITEM_SCARECROW, Paths.SPRITE_SCARECROW_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SEESAW, color, Paths.ITEM_SEESAW_GREYSCALE, Paths.ITEM_SEESAW, Paths.SPRITE_SEESAW_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SIGNPOST, color, Paths.ITEM_SIGNPOST_GREYSCALE, Paths.ITEM_SIGNPOST, Paths.SPRITE_SIGNPOST_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SLIDE, color, Paths.ITEM_SLIDE_GREYSCALE, Paths.ITEM_SLIDE, Paths.SPRITE_SLIDE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SMILE_GRAFFITI, color, Paths.ITEM_SMILE_GRAFFITI_GREYSCALE, Paths.ITEM_SMILE_GRAFFITI, Paths.SPRITE_SMILE_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SNOWMAN, color, Paths.ITEM_SNOWMAN_GREYSCALE, Paths.ITEM_SNOWMAN, Paths.SPRITE_SNOWMAN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SOFA, color, Paths.ITEM_SOFA_GREYSCALE, Paths.ITEM_SOFA, Paths.SPRITE_SOFA_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SOLAR_PANEL, color, Paths.ITEM_SOLAR_PANEL_GREYSCALE, Paths.ITEM_SOLAR_PANEL, Paths.SPRITE_SOLAR_PANEL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SOURCE_UNKNOWN_GRAFFITI, color, Paths.ITEM_SOURCE_UNKNOWN_GRAFFITI_GREYSCALE, Paths.ITEM_SOURCE_UNKNOWN_GRAFFITI, Paths.SPRITE_SOURCE_UNKNOWN_GRAFFITI_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SPHERE_STATUE, color, Paths.ITEM_SPHERE_STATUE_GREYSCALE, Paths.ITEM_SPHERE_STATUE, Paths.SPRITE_SPHERE_STATUE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STATUE, color, Paths.ITEM_STATUE_GREYSCALE, Paths.ITEM_STATUE, Paths.SPRITE_STATUE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STONE_COLUMN, color, Paths.ITEM_STONE_COLUMN_GREYSCALE, Paths.ITEM_STONE_COLUMN, Paths.SPRITE_STONE_COLUMN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STREETLAMP, color, Paths.ITEM_STREETLAMP_GREYSCALE, Paths.ITEM_STREETLAMP, Paths.SPRITE_STREETLAMP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STREETLIGHT, color, Paths.ITEM_STREETLIGHT_GREYSCALE, Paths.ITEM_STREETLIGHT, Paths.SPRITE_STREETLIGHT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SURFBOARD, color, Paths.ITEM_SURFBOARD_GREYSCALE, Paths.ITEM_SURFBOARD, Paths.SPRITE_SURFBOARD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SWINGS, color, Paths.ITEM_SWINGS_GREYSCALE, Paths.ITEM_SWINGS, Paths.SPRITE_SWINGS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TARGET, color, Paths.ITEM_TARGET_GREYSCALE, Paths.ITEM_TARGET, Paths.SPRITE_TARGET_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TELEVISION, color, Paths.ITEM_TELEVISION_GREYSCALE, Paths.ITEM_TELEVISION, Paths.SPRITE_TELEVISION_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOOLBOX, color, Paths.ITEM_TOOLBOX_GREYSCALE, Paths.ITEM_TOOLBOX, Paths.SPRITE_TOOLBOX_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOOLRACK, color, Paths.ITEM_TOOLRACK_GREYSCALE, Paths.ITEM_TOOLRACK, Paths.SPRITE_TOOLRACK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TRAFFIC_CONE, color, Paths.ITEM_TRAFFIC_CONE_GREYSCALE, Paths.ITEM_TRAFFIC_CONE, Paths.SPRITE_TRAFFIC_CONE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TRAFFIC_LIGHT, color, Paths.ITEM_TRAFFIC_LIGHT_GREYSCALE, Paths.ITEM_TRAFFIC_LIGHT, Paths.SPRITE_TRAFFIC_LIGHT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TRASHCAN, color, Paths.ITEM_TRASHCAN_GREYSCALE, Paths.ITEM_TRASHCAN, Paths.SPRITE_TRASHCAN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TRIPLE_MIRRORS, color, Paths.ITEM_TRIPLE_MIRRORS_GREYSCALE, Paths.ITEM_TRIPLE_MIRRORS, Paths.SPRITE_TRIPLE_MIRRORS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(UMBRELLA, color, Paths.ITEM_UMBRELLA_GREYSCALE, Paths.ITEM_UMBRELLA, Paths.SPRITE_UMBRELLA_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(UMBRELLA_TABLE, color, Paths.ITEM_UMBRELLA_TABLE_GREYSCALE, Paths.ITEM_UMBRELLA_TABLE, Paths.SPRITE_UMBRELLA_TABLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WAGON, color, Paths.ITEM_WAGON_GREYSCALE, Paths.ITEM_WAGON, Paths.SPRITE_WAGON_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WALL_MIRROR, color, Paths.ITEM_WALL_MIRROR_GREYSCALE, Paths.ITEM_WALL_MIRROR, Paths.SPRITE_WALL_MIRROR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WATER_PUMP, color, Paths.ITEM_WATER_PUMP_GREYSCALE, Paths.ITEM_WATER_PUMP, Paths.SPRITE_WATER_PUMP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WATERTOWER, color, Paths.ITEM_WATERTOWER_GREYSCALE, Paths.ITEM_WATERTOWER, Paths.SPRITE_WATERTOWER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WATER_PUMP, color, Paths.ITEM_WATER_PUMP_GREYSCALE, Paths.ITEM_WATER_PUMP, Paths.SPRITE_WATER_PUMP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WELL, color, Paths.ITEM_WELL_GREYSCALE, Paths.ITEM_WELL, Paths.SPRITE_WELL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WHEELBARROW, color, Paths.ITEM_WHEELBARROW_GREYSCALE, Paths.ITEM_WHEELBARROW, Paths.SPRITE_WHEELBARROW_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WHITEBOARD, color, Paths.ITEM_WHITEBOARD_GREYSCALE, Paths.ITEM_WHITEBOARD, Paths.SPRITE_WHITEBOARD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_BENCH, color, Paths.ITEM_WOODEN_BENCH_GREYSCALE, Paths.ITEM_WOODEN_BENCH, Paths.SPRITE_WOODEN_BENCH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_CHAIR, color, Paths.ITEM_WOODEN_CHAIR_GREYSCALE, Paths.ITEM_WOODEN_CHAIR, Paths.SPRITE_WOODEN_CHAIR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_COLUMN, color, Paths.ITEM_WOODEN_COLUMN_GREYSCALE, Paths.ITEM_WOODEN_COLUMN, Paths.SPRITE_WOODEN_COLUMN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_LONGTABLE, color, Paths.ITEM_WOODEN_LONGTABLE_GREYSCALE, Paths.ITEM_WOODEN_LONGTABLE, Paths.SPRITE_WOODEN_LONGTABLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_POST, color, Paths.ITEM_WOODEN_POST_GREYSCALE, Paths.ITEM_WOODEN_POST, Paths.SPRITE_WOODEN_POST_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_ROUNDTABLE, color, Paths.ITEM_WOODEN_ROUNDTABLE_GREYSCALE, Paths.ITEM_WOODEN_ROUNDTABLE, Paths.SPRITE_WOODEN_ROUNDTABLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_SQUARETABLE, color, Paths.ITEM_WOODEN_SQUARETABLE_GREYSCALE, Paths.ITEM_WOODEN_SQUARETABLE, Paths.SPRITE_WOODEN_SQUARETABLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_STOOL, color, Paths.ITEM_WOODEN_STOOL_GREYSCALE, Paths.ITEM_WOODEN_STOOL, Paths.SPRITE_WOODEN_STOOL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DRUMSET, color, Paths.ITEM_DRUMSET_GREYSCALE, Paths.ITEM_DRUMSET, Paths.SPRITE_DRUMSET_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HARP, color, Paths.ITEM_HARP_GREYSCALE, Paths.ITEM_HARP, Paths.SPRITE_HARP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(XYLOPHONE, color, Paths.ITEM_XYLOPHONE_GREYSCALE, Paths.ITEM_XYLOPHONE, Paths.SPRITE_XYLOPHONE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GLASS_SHEET, color, Paths.ITEM_GLASS_SHEET_GREYSCALE, Paths.ITEM_GLASS_SHEET, Paths.SPRITE_GLASS_SHEET_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(PAINTING_OASIS, color, Paths.ITEM_PAINTINGFRAME_POSTER_GREYSCALE, Paths.ITEM_PAINTING_OASIS, Paths.SPRITE_PAINTINGFRAME_POSTER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_FOUR, color, Paths.ITEM_PAINTINGFRAME_POSTER_GREYSCALE, Paths.ITEM_PAINTING_FOUR, Paths.SPRITE_PAINTINGFRAME_POSTER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_FUTURE, color, Paths.ITEM_PAINTINGFRAME_POSTER_GREYSCALE, Paths.ITEM_PAINTING_FUTURE, Paths.SPRITE_PAINTINGFRAME_POSTER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_ARCTIC, color, Paths.ITEM_PAINTINGFRAME_LANDSCAPE_GREYSCALE, Paths.ITEM_PAINTING_ARCTIC, Paths.SPRITE_PAINTINGFRAME_LANDSCAPE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_ARCTIC, color, Paths.ITEM_PAINTINGFRAME_LANDSCAPE_GREYSCALE, Paths.ITEM_PAINTING_ARCTIC, Paths.SPRITE_PAINTINGFRAME_LANDSCAPE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_ACCEPTANCE, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_ACCEPTANCE, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_BALANCE, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_BALANCE, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_BLACK, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_BLACK, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_COFFEE, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_COFFEE, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_DICHOTOMY, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_DICHOTOMY, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_FIREBALL, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_FIREBALL, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_LION, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_LION, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_MINTGREEN, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_MINTGREEN, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_GROVE, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_GROVE, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_PUZZLE, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_PUZZLE, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_TOXIN, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_TOXIN, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_CREAMPUFF, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_CREAMPUFF, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_SELF, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_SELF, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_RAIDER, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_RAIDER, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_VINEFLOWER, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_VINEFLOWER, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_FJORD, color, Paths.ITEM_PAINTINGFRAME_CIRCLE_GREYSCALE, Paths.ITEM_PAINTING_FJORD, Paths.SPRITE_PAINTINGFRAME_CIRCLE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_DITHER, color, Paths.ITEM_PAINTINGFRAME_LONGFLAT_GREYSCALE, Paths.ITEM_PAINTING_DITHER, Paths.SPRITE_PAINTINGFRAME_LONGFLAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_BEACHDAY, color, Paths.ITEM_PAINTINGFRAME_SCROLL_GREYSCALE, Paths.ITEM_PAINTING_BEACHDAY, Paths.SPRITE_PAINTINGFRAME_SCROLL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_RIVER, color, Paths.ITEM_PAINTINGFRAME_SCROLL_GREYSCALE, Paths.ITEM_PAINTING_RIVER, Paths.SPRITE_PAINTINGFRAME_SCROLL_SPRITESHEET_GREYSCALE)); 
                    AddToDictionary(MakeColoredVersion(PAINTING_BEDTIME, color, Paths.ITEM_PAINTINGFRAME_HEART_GREYSCALE, Paths.ITEM_PAINTING_BEDTIME, Paths.SPRITE_PAINTINGFRAME_HEART_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_ILOVEYOU, color, Paths.ITEM_PAINTINGFRAME_HEART_GREYSCALE, Paths.ITEM_PAINTING_ILOVEYOU, Paths.SPRITE_PAINTINGFRAME_HEART_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_CHANGES, color, Paths.ITEM_PAINTINGFRAME_HEART_GREYSCALE, Paths.ITEM_PAINTING_CHANGES, Paths.SPRITE_PAINTINGFRAME_HEART_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_LIBERTY, color, Paths.ITEM_PAINTINGFRAME_HEART_GREYSCALE, Paths.ITEM_PAINTING_LIBERTY, Paths.SPRITE_PAINTINGFRAME_HEART_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_INTERLUDE, color, Paths.ITEM_PAINTINGFRAME_HEART_GREYSCALE, Paths.ITEM_PAINTING_INTERLUDE, Paths.SPRITE_PAINTINGFRAME_HEART_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_SKYROSE, color, Paths.ITEM_PAINTINGFRAME_STANDARD_GREYSCALE, Paths.ITEM_PAINTING_SKYROSE, Paths.SPRITE_PAINTINGFRAME_STANDARD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_EARTH, color, Paths.ITEM_PAINTINGFRAME_STANDARD_GREYSCALE, Paths.ITEM_PAINTING_EARTH, Paths.SPRITE_PAINTINGFRAME_STANDARD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_CALCULATOR, color, Paths.ITEM_PAINTINGFRAME_STANDARD_GREYSCALE, Paths.ITEM_PAINTING_CALCULATOR, Paths.SPRITE_PAINTINGFRAME_STANDARD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_CORAL, color, Paths.ITEM_PAINTINGFRAME_SMALLSQUARE_GREYSCALE, Paths.ITEM_PAINTING_CORAL, Paths.SPRITE_PAINTINGFRAME_SMALLSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_SEASONS, color, Paths.ITEM_PAINTINGFRAME_SMALLSQUARE_GREYSCALE, Paths.ITEM_PAINTING_SEASONS, Paths.SPRITE_PAINTINGFRAME_SMALLSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_WHATEVER, color, Paths.ITEM_PAINTINGFRAME_SMALLSQUARE_GREYSCALE, Paths.ITEM_PAINTING_WHATEVER, Paths.SPRITE_PAINTINGFRAME_SMALLSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_SOLEMN, color, Paths.ITEM_PAINTINGFRAME_SQUARE_GREYSCALE, Paths.ITEM_PAINTING_SOLEMN, Paths.SPRITE_PAINTINGFRAME_SQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_MOONSET, color, Paths.ITEM_PAINTINGFRAME_SQUARE_GREYSCALE, Paths.ITEM_PAINTING_MOONSET, Paths.SPRITE_PAINTINGFRAME_SQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_MONA, color, Paths.ITEM_PAINTINGFRAME_SQUARE_GREYSCALE, Paths.ITEM_PAINTING_MONA, Paths.SPRITE_PAINTINGFRAME_SQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_OVERHANG, color, Paths.ITEM_PAINTINGFRAME_SQUARE_GREYSCALE, Paths.ITEM_PAINTING_OVERHANG, Paths.SPRITE_PAINTINGFRAME_SQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_SUNSET, color, Paths.ITEM_PAINTINGFRAME_HANGSQUARE_GREYSCALE, Paths.ITEM_PAINTING_SUNSET, Paths.SPRITE_PAINTINGFRAME_HANGSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_SPICE, color, Paths.ITEM_PAINTINGFRAME_HANGSQUARE_GREYSCALE, Paths.ITEM_PAINTING_SPICE, Paths.SPRITE_PAINTINGFRAME_HANGSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_WINDOW, color, Paths.ITEM_PAINTINGFRAME_HANGSQUARE_GREYSCALE, Paths.ITEM_PAINTING_WINDOW, Paths.SPRITE_PAINTINGFRAME_HANGSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_ET, color, Paths.ITEM_PAINTINGFRAME_HANGSQUARE_GREYSCALE, Paths.ITEM_PAINTING_ET, Paths.SPRITE_PAINTINGFRAME_HANGSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_LAVENDER, color, Paths.ITEM_PAINTINGFRAME_HANGSQUARE_GREYSCALE, Paths.ITEM_PAINTING_LAVENDER, Paths.SPRITE_PAINTINGFRAME_HANGSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_THREADS, color, Paths.ITEM_PAINTINGFRAME_HANGSQUARE_GREYSCALE, Paths.ITEM_PAINTING_THREADS, Paths.SPRITE_PAINTINGFRAME_HANGSQUARE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_LAUNCH, color, Paths.ITEM_PAINTINGFRAME_PORTRAIT_GREYSCALE, Paths.ITEM_PAINTING_LAUNCH, Paths.SPRITE_PAINTINGFRAME_PORTRAIT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_FABLE, color, Paths.ITEM_PAINTINGFRAME_PORTRAIT_GREYSCALE, Paths.ITEM_PAINTING_FABLE, Paths.SPRITE_PAINTINGFRAME_PORTRAIT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_GROWTH, color, Paths.ITEM_PAINTINGFRAME_PORTRAIT_GREYSCALE, Paths.ITEM_PAINTING_GROWTH, Paths.SPRITE_PAINTINGFRAME_PORTRAIT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_SHADES, color, Paths.ITEM_PAINTINGFRAME_PORTRAIT_GREYSCALE, Paths.ITEM_PAINTING_SHADES, Paths.SPRITE_PAINTINGFRAME_PORTRAIT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_GENIUS, color, Paths.ITEM_PAINTINGFRAME_PORTRAIT_GREYSCALE, Paths.ITEM_PAINTING_GENIUS, Paths.SPRITE_PAINTINGFRAME_PORTRAIT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PAINTING_RESONANT, color, Paths.ITEM_PAINTINGFRAME_HUGE_GREYSCALE, Paths.ITEM_PAINTING_RESONANT, Paths.SPRITE_PAINTINGFRAME_HUGE_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(TOTEM_OF_THE_CHICKEN, color, Paths.ITEM_TOTEM_OF_THE_CHICKEN_GREYSCALE, Paths.ITEM_TOTEM_OF_THE_CHICKEN, Paths.SPRITE_TOTEM_OF_THE_CHICKEN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOTEM_OF_THE_COW, color, Paths.ITEM_TOTEM_OF_THE_COW_GREYSCALE, Paths.ITEM_TOTEM_OF_THE_COW, Paths.SPRITE_TOTEM_OF_THE_COW_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOTEM_OF_THE_PIG, color, Paths.ITEM_TOTEM_OF_THE_PIG_GREYSCALE, Paths.ITEM_TOTEM_OF_THE_PIG, Paths.SPRITE_TOTEM_OF_THE_PIG_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOTEM_OF_THE_SHEEP, color, Paths.ITEM_TOTEM_OF_THE_SHEEP_GREYSCALE, Paths.ITEM_TOTEM_OF_THE_SHEEP, Paths.SPRITE_TOTEM_OF_THE_SHEEP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOTEM_OF_THE_ROOSTER, color, Paths.ITEM_TOTEM_OF_THE_ROOSTER_GREYSCALE, Paths.ITEM_TOTEM_OF_THE_ROOSTER, Paths.SPRITE_TOTEM_OF_THE_ROOSTER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOTEM_OF_THE_DOG, color, Paths.ITEM_TOTEM_OF_THE_DOG_GREYSCALE, Paths.ITEM_TOTEM_OF_THE_DOG, Paths.SPRITE_TOTEM_OF_THE_DOG_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOTEM_OF_THE_CAT, color, Paths.ITEM_TOTEM_OF_THE_CAT_GREYSCALE, Paths.ITEM_TOTEM_OF_THE_CAT, Paths.SPRITE_TOTEM_OF_THE_CAT_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(CONCRETE_FLOOR, color, Paths.ITEM_FLOOR_CONCRETE_GREYSCALE, Paths.ITEM_FLOOR_CONCRETE, Paths.SPRITE_FLOOR_CONCRETE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STREET_FLOOR, color, Paths.ITEM_FLOOR_STREET_GREYSCALE, Paths.ITEM_FLOOR_STREET, Paths.SPRITE_FLOOR_STREET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CARPET_FLOOR, color, Paths.ITEM_FLOOR_CARPET_GREYSCALE, Paths.ITEM_FLOOR_CARPET, Paths.SPRITE_FLOOR_CARPET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BOARDWALK_FLOOR, color, Paths.ITEM_FLOOR_BOARDWALK_GREYSCALE, Paths.ITEM_FLOOR_BOARDWALK, Paths.SPRITE_FLOOR_BOARDWALK_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STEPPING_STONE_FLOOR, color, Paths.ITEM_FLOOR_STEPPING_STONE_GREYSCALE, Paths.ITEM_FLOOR_STEPPING_STONE, Paths.SPRITE_FLOOR_STEPPING_STONE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FOOTPRINT_FLOOR, color, Paths.ITEM_FLOOR_FOOTPRINT_GREYSCALE, Paths.ITEM_FLOOR_FOOTPRINT, Paths.SPRITE_FLOOR_FOOTPRINT_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MAT_FLOOR, color, Paths.ITEM_FLOOR_MAT_GREYSCALE, Paths.ITEM_FLOOR_MAT, Paths.SPRITE_FLOOR_MAT_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SQUARE_FLOOR, color, Paths.ITEM_FLOOR_SQUARE_GREYSCALE, Paths.ITEM_FLOOR_SQUARE, Paths.SPRITE_FLOOR_SQUARE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TATAMI_FLOOR, color, Paths.ITEM_FLOOR_TATAMI_GREYSCALE, Paths.ITEM_FLOOR_TATAMI, Paths.SPRITE_FLOOR_TATAMI_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(THIN_TATAMI_FLOOR, color, Paths.ITEM_FLOOR_THIN_TATAMI_GREYSCALE, Paths.ITEM_FLOOR_THIN_TATAMI, Paths.SPRITE_FLOOR_THIN_TATAMI_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TRIANGULATE_FLOOR, color, Paths.ITEM_FLOOR_TRIANGULATE_GREYSCALE, Paths.ITEM_FLOOR_TRIANGULATE, Paths.SPRITE_FLOOR_TRIANGULATE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_FLOOR, color, Paths.ITEM_FLOOR_WOODEN_GREYSCALE, Paths.ITEM_FLOOR_WOODEN, Paths.SPRITE_FLOOR_WOODEN_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(WAVE_WALLPAPER, color, Paths.ITEM_WALLPAPER_WAVE_GREYSCALE, Paths.ITEM_WALLPAPER_WAVE, Paths.SPRITE_WALLPAPER_WAVE_GREYSCALE, Paths.SPRITE_WALLPAPER_WAVE_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_WAVE_GREYSCALE_BOTTOM));
                    AddToDictionary(MakeColoredVersion(STAR_WALLPAPER, color, Paths.ITEM_WALLPAPER_STAR_GREYSCALE, Paths.ITEM_WALLPAPER_STAR, Paths.SPRITE_WALLPAPER_STAR_GREYSCALE, Paths.SPRITE_WALLPAPER_STAR_GREYSCALE, Paths.SPRITE_WALLPAPER_STAR_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BUBBLE_WALLPAPER, color, Paths.ITEM_WALLPAPER_BUBBLE_GREYSCALE, Paths.ITEM_WALLPAPER_BUBBLE, Paths.SPRITE_WALLPAPER_BUBBLE_GREYSCALE, Paths.SPRITE_WALLPAPER_BUBBLE_GREYSCALE, Paths.SPRITE_WALLPAPER_BUBBLE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SOLID_WALLPAPER, color, Paths.ITEM_WALLPAPER_SOLID_GREYSCALE, Paths.ITEM_WALLPAPER_SOLID, Paths.SPRITE_WALLPAPER_SOLID_GREYSCALE, Paths.SPRITE_WALLPAPER_SOLID_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_SOLID_GREYSCALE_BOTTOM));
                    AddToDictionary(MakeColoredVersion(VERTICAL_WALLPAPER, color, Paths.ITEM_WALLPAPER_VERTICAL_GREYSCALE, Paths.ITEM_WALLPAPER_VERTICAL, Paths.SPRITE_WALLPAPER_VERTICAL_GREYSCALE, Paths.SPRITE_WALLPAPER_VERTICAL_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_VERTICAL_GREYSCALE_BOTTOM));
                    AddToDictionary(MakeColoredVersion(HORIZONTAL_WALLPAPER, color, Paths.ITEM_WALLPAPER_HORIZONTAL_GREYSCALE, Paths.ITEM_WALLPAPER_HORIZONTAL, Paths.SPRITE_WALLPAPER_HORIZONTAL_GREYSCALE, Paths.SPRITE_WALLPAPER_HORIZONTAL_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_HORIZONTAL_GREYSCALE_BOTTOM));
                    AddToDictionary(MakeColoredVersion(DOT_WALLPAPER, color, Paths.ITEM_WALLPAPER_DOT_GREYSCALE, Paths.ITEM_WALLPAPER_DOT, Paths.SPRITE_WALLPAPER_DOT_GREYSCALE, Paths.SPRITE_WALLPAPER_DOT_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_DOT_GREYSCALE_BOTTOM));
                    AddToDictionary(MakeColoredVersion(POLKA_WALLPAPER, color, Paths.ITEM_WALLPAPER_POLKA_GREYSCALE, Paths.ITEM_WALLPAPER_POLKA, Paths.SPRITE_WALLPAPER_POLKA_GREYSCALE, Paths.SPRITE_WALLPAPER_POLKA_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_POLKA_GREYSCALE_BOTTOM));
                    AddToDictionary(MakeColoredVersion(INVADER_WALLPAPER, color, Paths.ITEM_WALLPAPER_INVADER_GREYSCALE, Paths.ITEM_WALLPAPER_INVADER, Paths.SPRITE_WALLPAPER_INVADER_GREYSCALE, Paths.SPRITE_WALLPAPER_INVADER_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_INVADER_GREYSCALE_BOTTOM));
                    AddToDictionary(MakeColoredVersion(ODD_WALLPAPER, color, Paths.ITEM_WALLPAPER_ODD_GREYSCALE, Paths.ITEM_WALLPAPER_ODD, Paths.SPRITE_WALLPAPER_ODD_GREYSCALE, Paths.SPRITE_WALLPAPER_ODD_GREYSCALE_TOP, Paths.SPRITE_WALLPAPER_ODD_GREYSCALE_BOTTOM));

                    AddToDictionary(MakeColoredVersion(BACKPACK, color, Paths.ITEM_BACKPACK_GREYSCALE, Paths.CLOTHING_BACKPACK_SPRITESHEET_GREYSCALE)); 
                    AddToDictionary(MakeColoredVersion(RUCKSACK, color, Paths.ITEM_RUCKSACK_GREYSCALE, Paths.CLOTHING_RUCKSACK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CAPE, color, Paths.ITEM_CAPE_GREYSCALE, Paths.CLOTHING_CAPE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GUITAR, color, Paths.ITEM_GUITAR_GREYSCALE, Paths.CLOTHING_GUITAR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOLF_TAIL, color, Paths.ITEM_WOLF_TAIL_GREYSCALE, Paths.CLOTHING_WOLF_TAIL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FOX_TAIL, color, Paths.ITEM_FOX_TAIL_GREYSCALE, Paths.CLOTHING_FOX_TAIL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CAT_TAIL, color, Paths.ITEM_CAT_TAIL_GREYSCALE, Paths.CLOTHING_CAT_TAIL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CLOCKWORK, color, Paths.ITEM_CLOCKWORK_GREYSCALE, Paths.CLOTHING_CLOCKWORK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ROBO_ARMS, color, Paths.ITEM_ROBO_ARMS_GREYSCALE, Paths.CLOTHING_ROBO_ARMS_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(EARRING_STUD, color, Paths.ITEM_EARRING_STUD_GREYSCALE, Paths.CLOTHING_EARRING_STUD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DANGLE_EARRING, color, Paths.ITEM_DANGLE_EARRING_GREYSCALE, Paths.CLOTHING_DANGLE_EARRING_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PIERCING, color, Paths.ITEM_PIERCING_GREYSCALE, Paths.CLOTHING_PIERCING_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(GLASSES, color, Paths.ITEM_GLASSES_GREYSCALE, Paths.CLOTHING_GLASSES_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLINDFOLD, color, Paths.ITEM_BLINDFOLD_GREYSCALE, Paths.CLOTHING_BLINDFOLD_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(EYEPATCH, color, Paths.ITEM_EYEPATCH_GREYSCALE, Paths.CLOTHING_EYEPATCH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(QUERADE_MASK, color, Paths.ITEM_QUERADE_MASK_GREYSCALE, Paths.CLOTHING_QUERADE_MASK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PROTECTIVE_VISOR, color, Paths.ITEM_PROTECTIVE_VISOR_GREYSCALE, Paths.CLOTHING_PROTECTIVE_VISOR_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SNORKEL, color, Paths.ITEM_SNORKEL_GREYSCALE, Paths.CLOTHING_SNORKEL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SUNGLASSES, color, Paths.ITEM_SUNGLASSES_GREYSCALE, Paths.CLOTHING_SUNGLASSES_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GOGGLES, color, Paths.ITEM_GOGGLES_GREYSCALE, Paths.CLOTHING_GOGGLES_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(WOOL_MITTENS, color, Paths.ITEM_WOOL_MITTENS_GREYSCALE, Paths.CLOTHING_WOOL_MITTENS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WORK_GLOVES, color, Paths.ITEM_WORK_GLOVES_GREYSCALE, Paths.CLOTHING_WORK_GLOVES_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BOXING_MITTS, color, Paths.ITEM_BOXING_MITTS_GREYSCALE, Paths.CLOTHING_BOXING_MITTS_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(BASEBALL_CAP, color, Paths.ITEM_BASEBALL_CAP_GREYSCALE, Paths.CLOTHING_BASEBALL_CAP_SPRITESHEET_GREYSCALE)); //
                    AddToDictionary(MakeColoredVersion(TEN_GALLON, color, Paths.ITEM_10_GALLON_GREYSCALE, Paths.CLOTHING_10_GALLON_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BANDANA, color, Paths.ITEM_BANDANA_GREYSCALE, Paths.CLOTHING_BANDANA_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BOWLER, color, Paths.ITEM_BOWLER_GREYSCALE, Paths.CLOTHING_BOWLER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BUNNY_EARS, color, Paths.ITEM_BUNNY_EARS_GREYSCALE, Paths.CLOTHING_BUNNY_EARS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BUTTERFLY_CLIP, color, Paths.ITEM_BUTTERFLY_CLIP_GREYSCALE, Paths.CLOTHING_BUTTERFLY_CLIP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CAMEL_HAT, color, Paths.ITEM_CAMEL_HAT_GREYSCALE, Paths.CLOTHING_CAMEL_HAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CAT_EARS, color, Paths.ITEM_CAT_EARS_GREYSCALE, Paths.CLOTHING_CAT_EARS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CHEFS_HAT, color, Paths.ITEM_CHEFS_HAT_GREYSCALE, Paths.CLOTHING_CHEFS_HAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CONICAL_FARMER, color, Paths.ITEM_CONICAL_FARMER_GREYSCALE, Paths.CLOTHING_CONICAL_FARMER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DINO_MASK, color, Paths.ITEM_DINO_MASK_GREYSCALE, Paths.CLOTHING_DINO_MASK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(DOG_MASK, color, Paths.ITEM_DOG_MASK_GREYSCALE, Paths.CLOTHING_DOG_MASK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FACEMASK, color, Paths.ITEM_FACEMASK_GREYSCALE, Paths.CLOTHING_FACEMASK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FLAT_CAP, color, Paths.ITEM_FLAT_CAP_GREYSCALE, Paths.CLOTHING_FLAT_CAP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HEADBAND, color, Paths.ITEM_HEADBAND_GREYSCALE, Paths.CLOTHING_HEADBAND_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(NIGHTCAP, color, Paths.ITEM_NIGHTCAP_GREYSCALE, Paths.CLOTHING_NIGHTCAP_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(NIGHTMARE_MASK, color, Paths.ITEM_NIGHTMARE_MASK_GREYSCALE, Paths.CLOTHING_NIGHTMARE_MASK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SNAPBACK, color, Paths.ITEM_SNAPBACK_GREYSCALE, Paths.CLOTHING_SNAPBACK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SQUARE_HAT, color, Paths.ITEM_SQUARE_HAT_GREYSCALE, Paths.CLOTHING_SQUARE_HAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STRAW_HAT, color, Paths.ITEM_STRAW_HAT_GREYSCALE, Paths.CLOTHING_STRAW_HAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TOP_HAT, color, Paths.ITEM_TOP_HAT_GREYSCALE, Paths.CLOTHING_TOP_HAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TRACE_TATTOO, color, Paths.ITEM_TRACE_TATTOO_GREYSCALE, Paths.CLOTHING_TRACE_TATTOO_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WHISKERS, color, Paths.ITEM_WHISKERS_GREYSCALE, Paths.CLOTHING_WHISKERS_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(SCARF, color, Paths.ITEM_SCARF_GREYSCALE, Paths.CLOTHING_SCARF_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ASCOT, color, Paths.ITEM_ASCOT_GREYSCALE, Paths.CLOTHING_ASCOT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MEDAL, color, Paths.ITEM_MEDAL_GREYSCALE, Paths.CLOTHING_MEDAL_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(NECKWARMER, color, Paths.ITEM_NECKWARMER_GREYSCALE, Paths.CLOTHING_NECKWARMER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(NECKLACE, color, Paths.ITEM_NECKLACE_GREYSCALE, Paths.CLOTHING_NECKLACE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SASH, color, Paths.ITEM_SASH_GREYSCALE, Paths.CLOTHING_SASH_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TIE, color, Paths.ITEM_TIE_GREYSCALE, Paths.CLOTHING_TIE_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(ALL_SEASON_JACKET, color, Paths.ITEM_ALL_SEASON_JACKET_GREYSCALE, Paths.CLOTHING_ALL_SEASON_JACKET_SPRITESHEET_GREYSCALE)); //
                    AddToDictionary(MakeColoredVersion(APRON, color, Paths.ITEM_APRON_GREYSCALE, Paths.CLOTHING_APRON_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BATHROBE, color, Paths.ITEM_BATHROBE_GREYSCALE, Paths.CLOTHING_BATHROBE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(NOMAD_VEST, color, Paths.ITEM_NOMAD_VEST_GREYSCALE, Paths.CLOTHING_NOMAD_VEST_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HOODED_SWEATSHIRT, color, Paths.ITEM_HOODED_SWEATSHIRT_GREYSCALE, Paths.CLOTHING_HOODED_SWEATSHIRT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ONESIE, color, Paths.ITEM_ONESIE_GREYSCALE, Paths.CLOTHING_ONESIE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(OVERALLS, color, Paths.ITEM_OVERALLS_GREYSCALE, Paths.CLOTHING_OVERALLS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(OVERCOAT, color, Paths.ITEM_OVERCOAT_GREYSCALE, Paths.CLOTHING_OVERCOAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PUNK_JACKET, color, Paths.ITEM_PUNK_JACKET_GREYSCALE, Paths.CLOTHING_PUNK_JACKET_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(RAINCOAT, color, Paths.ITEM_RAINCOAT_GREYSCALE, Paths.CLOTHING_RAINCOAT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SPORTBALL_UNIFORM, color, Paths.ITEM_SPORTBALL_UNIFORM_GREYSCALE, Paths.CLOTHING_SPORTBALL_UNIFORM_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SUIT_JACKET, color, Paths.ITEM_SUIT_JACKET_GREYSCALE, Paths.CLOTHING_SUIT_JACKET_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WEDDING_DRESS, color, Paths.ITEM_WEDDING_DRESS_GREYSCALE, Paths.CLOTHING_WEDDING_DRESS_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(JEANS, color, Paths.ITEM_JEANS_GREYSCALE, Paths.CLOTHING_JEANS_SPRITESHEET_GREYSCALE)); //
                    AddToDictionary(MakeColoredVersion(CHINO_SHORTS, color, Paths.ITEM_CHINO_SHORTS_GREYSCALE, Paths.CLOTHING_CHINO_SHORTS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(JEAN_SHORTS, color, Paths.ITEM_JEAN_SHORTS_GREYSCALE, Paths.CLOTHING_JEAN_SHORTS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(CHINOS, color, Paths.ITEM_CHINOS_GREYSCALE, Paths.CLOTHING_CHINOS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PUFF_SKIRT, color, Paths.ITEM_PUFF_SKIRT_GREYSCALE, Paths.CLOTHING_PUFF_SKIRT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LONG_SKIRT, color, Paths.ITEM_LONG_SKIRT_GREYSCALE, Paths.CLOTHING_LONG_SKIRT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SHORT_SKIRT, color, Paths.ITEM_SHORT_SKIRT_GREYSCALE, Paths.CLOTHING_SHORT_SKIRT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TIGHTIES, color, Paths.ITEM_TIGHTIES_GREYSCALE, Paths.CLOTHING_TIGHTIES_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SUPER_SHORTS, color, Paths.ITEM_SUPER_SHORTS_GREYSCALE, Paths.CLOTHING_SUPER_SHORTS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TORN_JEANS, color, Paths.ITEM_TORN_JEANS_GREYSCALE, Paths.CLOTHING_TORN_JEANS_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(SAILCLOTH, color, Paths.ITEM_SAILCLOTH_GREYSCALE, Paths.CLOTHING_SAILCLOTH_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(SHORT_SLEEVE_TEE, color, Paths.ITEM_SHORT_SLEEVE_TEE_GREYSCALE, Paths.CLOTHING_SHORT_SLEEVE_TEE_SPRITESHEET_GREYSCALE)); //
                    AddToDictionary(MakeColoredVersion(BUTTON_DOWN, color, Paths.ITEM_BUTTON_DOWN_GREYSCALE, Paths.CLOTHING_BUTTON_DOWN_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(ISLANDER_TATTOO, color, Paths.ITEM_ISLANDER_TATTOO_GREYSCALE, Paths.CLOTHING_ISLANDER_TATTOO_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LINEN_BUTTON, color, Paths.ITEM_LINEN_BUTTON_GREYSCALE, Paths.CLOTHING_LINEN_BUTTON_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(LONG_SLEEVE_TEE, color, Paths.ITEM_LONG_SLEEVED_TEE_GREYSCALE, Paths.CLOTHING_LONG_SLEEVED_TEE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLAID_BUTTON, color, Paths.ITEM_PLAID_BUTTON_GREYSCALE, Paths.CLOTHING_PLAID_BUTTON_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TURTLENECK, color, Paths.ITEM_TURTLENECK_GREYSCALE, Paths.CLOTHING_TURTLENECK_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STRIPED_SHIRT, color, Paths.ITEM_STRIPED_SHIRT_GREYSCALE, Paths.CLOTHING_STRIPED_SHIRT_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SWEATER, color, Paths.ITEM_SWEATER_GREYSCALE, Paths.CLOTHING_SWEATER_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TANKER, color, Paths.ITEM_TANKER_GREYSCALE, Paths.CLOTHING_TANKER_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(SNEAKERS, color, Paths.ITEM_SNEAKERS_GREYSCALE, Paths.CLOTHING_SNEAKERS_SPRITESHEET_GREYSCALE)); //
                    AddToDictionary(MakeColoredVersion(FLASH_HEELS, color, Paths.ITEM_FLASH_HEELS_GREYSCALE, Paths.CLOTHING_FLASH_HEELS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WING_SANDLES, color, Paths.ITEM_WING_SANDLES_GREYSCALE, Paths.CLOTHING_WING_SANDLES_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(HIGH_TOPS, color, Paths.ITEM_HIGH_TOPS_GREYSCALE, Paths.CLOTHING_HIGH_TOPS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TALL_BOOTS, color, Paths.ITEM_TALL_BOOTS_GREYSCALE, Paths.CLOTHING_TALL_BOOTS_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(SHORT_SOCKS, color, Paths.ITEM_SHORT_SOCKS_GREYSCALE, Paths.CLOTHING_SHORT_SOCKS_SPRITESHEET_GREYSCALE)); //
                    AddToDictionary(MakeColoredVersion(LONG_SOCKS, color, Paths.ITEM_LONG_SOCKS_GREYSCALE, Paths.CLOTHING_LONG_SOCKS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STRIPED_SOCKS, color, Paths.ITEM_STRIPED_SOCKS_GREYSCALE, Paths.CLOTHING_STRIPED_SOCKS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(FESTIVE_SOCKS, color, Paths.ITEM_FESTIVE_SOCKS_GREYSCALE, Paths.CLOTHING_FESTIVE_SOCKS_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MISMATTCHED, color, Paths.ITEM_MISMATTCHED_GREYSCALE, Paths.CLOTHING_MISMATTCHED_SPRITESHEET_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(BLOCK_METAL, color, Paths.ITEM_BLOCK_METAL_GREYSCALE, Paths.SPRITE_BLOCK_METAL_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLOCK_WOOD, color, Paths.ITEM_BLOCK_WOOD_GREYSCALE, Paths.SPRITE_BLOCK_WOOD_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLOCK_STONE, color, Paths.ITEM_BLOCK_STONE_GREYSCALE, Paths.SPRITE_BLOCK_STONE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLOCK_MYTHRIL, color, Paths.ITEM_BLOCK_MYTHRIL_GREYSCALE, Paths.SPRITE_BLOCK_MYTHRIL_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLOCK_GOLDEN, color, Paths.ITEM_BLOCK_GOLDEN_GREYSCALE, Paths.SPRITE_BLOCK_GOLDEN_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SCAFFOLDING_METAL, color, Paths.ITEM_SCAFFOLDING_METAL_GREYSCALE, Paths.SPRITE_SCAFFOLDING_METAL_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WALL_STONE, color, Paths.ITEM_WALL_STONE_GREYSCALE, Paths.SPRITE_WALL_STONE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SCAFFOLDING_WOOD, color, Paths.ITEM_SCAFFOLDING_WOOD_GREYSCALE, Paths.SPRITE_SCAFFOLDING_WOOD_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SCAFFOLDING_GOLDEN, color, Paths.ITEM_SCAFFOLDING_GOLDEN_GREYSCALE, Paths.SPRITE_SCAFFOLDING_GOLDEN_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(SCAFFOLDING_MYTHRIL, color, Paths.ITEM_SCAFFOLDING_MYTHRIL_GREYSCALE, Paths.SPRITE_SCAFFOLDING_MYTHRIL_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_METAL_FARM, color, Paths.ITEM_PLATFORM_METAL_FARM_GREYSCALE, Paths.SPRITE_PLATFORM_METAL_FARM_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_STONE_FARM, color, Paths.ITEM_PLATFORM_STONE_FARM_GREYSCALE, Paths.SPRITE_PLATFORM_STONE_FARM_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_WOOD_FARM, color, Paths.ITEM_PLATFORM_WOOD_FARM_GREYSCALE, Paths.SPRITE_PLATFORM_WOOD_FARM_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_GOLDEN_FARM, color, Paths.ITEM_PLATFORM_GOLDEN_FARM_GREYSCALE, Paths.SPRITE_PLATFORM_GOLDEN_FARM_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_MYTHRIL_FARM, color, Paths.ITEM_PLATFORM_MYTHRIL_FARM_GREYSCALE, Paths.SPRITE_PLATFORM_MYTHRIL_FARM_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_METAL, color, Paths.ITEM_PLATFORM_METAL_GREYSCALE, Paths.SPRITE_PLATFORM_METAL_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_WOOD, color, Paths.ITEM_PLATFORM_WOOD_GREYSCALE, Paths.SPRITE_PLATFORM_WOOD_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_STONE, color, Paths.ITEM_PLATFORM_STONE_GREYSCALE, Paths.SPRITE_PLATFORM_STONE_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_MYTHRIL, color, Paths.ITEM_PLATFORM_MYTHRIL_GREYSCALE, Paths.SPRITE_PLATFORM_MYTHRIL_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_GOLDEN, color, Paths.ITEM_PLATFORM_GOLDEN_GREYSCALE, Paths.SPRITE_PLATFORM_GOLDEN_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WALL_PLANK, color, Paths.ITEM_WALL_PLANK_GREYSCALE, Paths.SPRITE_WALL_PLANK_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WALL_METAL, color, Paths.ITEM_WALL_METAL_GREYSCALE, Paths.SPRITE_WALL_METAL_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(PLATFORM_PLANK, color, Paths.ITEM_PLATFORM_PLANK_GREYSCALE, Paths.SPRITE_PLATFORM_PLANK_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLOCK_PLANK, color, Paths.ITEM_BLOCK_PLANK_GREYSCALE, Paths.SPRITE_BLOCK_PLANK_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(BLOCK_BARK, color, Paths.ITEM_BLOCK_BARK_GREYSCALE, Paths.SPRITE_BLOCK_BARK_GREYSCALE));

                    AddToDictionary(MakeColoredVersion(BAMBOO_FENCE, color, Paths.ITEM_BAMBOO_FENCE_GREYSCALE, Paths.ITEM_BAMBOO_FENCE, Paths.SPRITE_BAMBOO_FENCE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GLASS_FENCE, color, Paths.ITEM_GLASS_FENCE_GREYSCALE, Paths.ITEM_GLASS_FENCE, Paths.SPRITE_GLASS_FENCE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(GOLDEN_FENCE, color, Paths.ITEM_GOLDEN_FENCE_GREYSCALE, Paths.ITEM_GOLDEN_FENCE, Paths.SPRITE_GOLDEN_FENCE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(METAL_FENCE, color, Paths.ITEM_METAL_FENCE_GREYSCALE, Paths.ITEM_METAL_FENCE, Paths.SPRITE_METAL_FENCE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(MYTHRIL_FENCE, color, Paths.ITEM_MYTHRIL_FENCE_GREYSCALE, Paths.ITEM_MYTHRIL_FENCE, Paths.SPRITE_MYTHRIL_FENCE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(STONE_FENCE, color, Paths.ITEM_STONE_FENCE_GREYSCALE, Paths.ITEM_STONE_FENCE, Paths.SPRITE_STONE_FENCE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(TALL_FENCE, color, Paths.ITEM_TALL_FENCE_GREYSCALE, Paths.ITEM_TALL_FENCE, Paths.SPRITE_TALL_FENCE_SPRITESHEET_GREYSCALE));
                    AddToDictionary(MakeColoredVersion(WOODEN_FENCE, color, Paths.ITEM_WOODEN_FENCE_GREYSCALE, Paths.ITEM_WOODEN_FENCE, Paths.SPRITE_WOODEN_FENCE_SPRITESHEET_GREYSCALE));
                }

                foreach (Util.RecolorMap hairColor in Util.HAIR_COLORS)
                {
                    AddToDictionary(MakeColoredVersion(HAIR_AFRO_ALFONSO, hairColor, Paths.ITEM_PEARL, Paths.HAIR_AFRO_ALFONSO_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_ALIENATED_ALICE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_ALIENATED_ALICE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_BAREBONES_BRIAN, hairColor, Paths.ITEM_PEARL, Paths.HAIR_BAREBONES_BRIAN_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_BENNY_BOWLCUT, hairColor, Paths.ITEM_PEARL, Paths.HAIR_BENNY_BOWLCUT_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_BERTHA_BUN, hairColor, Paths.ITEM_PEARL, Paths.HAIR_BERTHA_BUN_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_CARLOS_COOL, hairColor, Paths.ITEM_PEARL, Paths.HAIR_CARLOS_COOL_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_CLEANCUT_CHARLOTTE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_CLEANCUT_CHARLOTTE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_CLEAN_CONOR, hairColor, Paths.ITEM_PEARL, Paths.HAIR_CLEAN_CONOR_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_COMBED_CHRISTOPH, hairColor, Paths.ITEM_PEARL, Paths.HAIR_COMBED_CHRISTOPH_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_COWLICK_COLTON, hairColor, Paths.ITEM_PEARL, Paths.HAIR_COWLICK_COLTON_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_DIRTY_JACK, hairColor, Paths.ITEM_PEARL, Paths.HAIR_DIRTY_JACK_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_EARNEST_EMMA, hairColor, Paths.ITEM_PEARL, Paths.HAIR_EARNEST_EMMA_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_FLASHY_FRIZZLE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_FLASHY_FRIZZLE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_FLUFFY_FELICIA, hairColor, Paths.ITEM_PEARL, Paths.HAIR_FLUFFY_FELICIA_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_FREDDIE_FRINGE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_FREDDIE_FRINGE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_GABRIEL_PART, hairColor, Paths.ITEM_PEARL, Paths.HAIR_GABRIEL_PART_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_GORGEOUS_GEORGEANN, hairColor, Paths.ITEM_PEARL, Paths.HAIR_GORGEOUS_GEORGEANN_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_HANGOVER_HANNA, hairColor, Paths.ITEM_PEARL, Paths.HAIR_HANGOVER_HANNA_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_INNOCENT_ILIA, hairColor, Paths.ITEM_PEARL, Paths.HAIR_INNOCENT_ILIA_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_LAZY_XAVIER, hairColor, Paths.ITEM_PEARL, Paths.HAIR_LAZY_XAVIER_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_LUCKY_LUKE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_LUCKY_LUKE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_LUXURY_LARA, hairColor, Paths.ITEM_PEARL, Paths.HAIR_LUXURY_LARA_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_MAXWELL_MOHAWK, hairColor, Paths.ITEM_PEARL, Paths.HAIR_MAXWELL_MOHAWK_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_MOUNTAIN_CLIMBER_MADELINE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_MOUNTAIN_CLIMBER_MADELINE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_MR_BALD, hairColor, Paths.ITEM_PEARL, Paths.HAIR_MR_BALD_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_OVERHANG_OWEN, hairColor, Paths.ITEM_PEARL, Paths.HAIR_OVERHANG_OWEN_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_PADMA_PERFECTION, hairColor, Paths.ITEM_PEARL, Paths.HAIR_PADMA_PERFECTION_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_PERSEPHONE_PUNK, hairColor, Paths.ITEM_PEARL, Paths.HAIR_PERSEPHONE_PUNK_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_PONYTAIL_TONYTALE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_PONYTAIL_TONYTALE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_SKULLCAP_STEVENS, hairColor, Paths.ITEM_PEARL, Paths.HAIR_SKULLCAP_STEVENS_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_SOPHIA_SWING, hairColor, Paths.ITEM_PEARL, Paths.HAIR_SOPHIA_SWING_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_STRICT_SUSIE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_STRICT_SUSIE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_THE_ORIGINAL_OLIVIA, hairColor, Paths.ITEM_PEARL, Paths.HAIR_THE_ORIGINAL_OLIVIA_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(HAIR_ZAPPY_ZADIE, hairColor, Paths.ITEM_PEARL, Paths.HAIR_ZAPPY_ZADIE_SPRITESHEET));

                    AddToDictionary(MakeColoredVersion(FACIALHAIR_BARON_MUSTACHE, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_BARON_MUSTACHE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_BEARD, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_BEARD_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_CAVEMAN, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_CAVEMAN_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_DROOPY, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_DROOPY_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_FLUFF, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_FLUFF_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_FULLBEARD, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_FULLBEARD_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_GOATEE, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_GOATEE_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_GOATEEBACK, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_GOATEEBACK_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_MONK, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_MONK_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_SHORTBEARD, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_SHORTBEARD_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_SIDEBURNS, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_SIDEBURNS_SPRITESHEET));
                    AddToDictionary(MakeColoredVersion(FACIALHAIR_SOULPATCH, hairColor, Paths.ITEM_PEARL, Paths.FACIALHAIR_SOULPATCH_SPRITESHEET));
                }
            }
        }

        public static List<Item> GetAllOfTag(Tag tag)
        {
            List<Item> toReturn = new List<Item>();

            foreach(Item item in itemDictionary.Values)
            {
                if(item.HasTag(tag))
                {
                    if (item is ClothingItem && item.GetName().Contains('('))
                        continue;
                    toReturn.Add(item);
                }
            }

            return toReturn;
        }

        public static Item GetCropBaseForm(Item crop) {
            if (crop == GOLDEN_WATERMELON_SLICE || crop == SILVER_WATERMELON_SLICE || crop == PHANTOM_WATERMELON_SLICE || crop == WATERMELON_SLICE)
            {
                return WATERMELON_SLICE;
            }
            return GetItemByName(crop.GetName().Split(' ')[1]);
        }

        public static Item GetCropGoldenForm(Item crop)
        {
            return GetItemByName("Golden " + GetCropBaseForm(crop));
        }

        public static Item GetBaseCropSeedForm(Item crop)
        {
            if (crop == GOLDEN_WATERMELON_SLICE || crop == SILVER_WATERMELON_SLICE || crop == PHANTOM_WATERMELON_SLICE || crop == WATERMELON_SLICE)
            {
                return WATERMELON_SEEDS;
            }
            return GetItemByName(crop.GetName() + " Seeds");
        }

        public static Item GetSeedShiningForm(Item seed)
        {
            return GetItemByName("Shining " + seed.GetName());
        }
    }
}
