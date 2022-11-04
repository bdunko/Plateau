using Microsoft.Xna.Framework;
using Plateau.Entities;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class GameState
    {
        public class CraftingRecipe
        {
            public bool haveBlueprint;
            public ItemStack result;
            public ItemStack[] components;

            public CraftingRecipe(ItemStack result, params ItemStack[] components)
            {
                this.haveBlueprint = false;
                this.result = result;
                this.components = components;
            }
        }
        public class CookingRecipe
        {
            public enum LengthEnum
            {
                NONE, SHORT, MEDIUM, LONG
            }

            public Item ingredient1, ingredient2, ingredient3;
            public Item result;
            public LengthEnum length;

            public CookingRecipe(Item result, Item ingredient1, Item ingredient2, Item ingredient3, LengthEnum length)
            {
                this.result = result;
                this.ingredient1 = ingredient1;
                this.ingredient2 = ingredient2;
                this.ingredient3 = ingredient3;
                this.length = length;
            }
        }

        public class ShrineStatus
        {
            public class RequiredItem
            {
                public Item item;
                public int amountNeeded;
                public int amountHave;

                public RequiredItem(Item item, int amountNeeded)
                {
                    this.item = item;
                    this.amountNeeded = amountNeeded;
                    this.amountHave = 0;
                }
            }

            private Action<EntityPlayer, Area> onCompletion;
            private RequiredItem[] requiredItems;
            private string id;

            public ShrineStatus(string id, Action<EntityPlayer, Area> onCompletion, params RequiredItem[] requiredItems)
            {
                this.id = id;
                this.onCompletion = onCompletion;
                this.requiredItems = requiredItems;
            }

            public RequiredItem[] GetRequiredItems()
            {
                return requiredItems;
            }

            public bool TryGiveItem(Item item)
            {
                bool accepted = false;
                foreach (RequiredItem ri in requiredItems)
                {
                    if (ri.item == item)
                    {
                        if (ri.amountHave < ri.amountNeeded)
                        {
                            ri.amountHave++;
                            accepted = true;
                            break;
                        }
                    }
                }
                return accepted;
            }

            public void Complete(EntityPlayer player, Area area)
            {
                this.onCompletion(player, area);
            }

            public string GetID()
            {
                return this.id;
            }

            public bool IsComplete()
            {
                foreach(RequiredItem ri in requiredItems)
                {
                    if(ri.amountHave != ri.amountNeeded)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        //public static string FLAG_....
        public static string FLAG_SETTINGS_HIDE_CONTROLS = "flagSettings_HideControls";
        public static string FLAG_SETTINGS_HIDE_GRID = "flagSettings_HideGrid";
        public static string FLAG_SETTINGS_HIDE_RETICLE = "flagSettings_HideReticle";
        public static string FLAG_SETTINGS_WINDOWED = "flagSettings_Windowed";
        public static string FLAG_SETTINGS_SFX_VOLUME = "flagSettings_SFX";
        public static string FLAG_SETTINGS_MUSIC_VOLUME = "flagSettings_Music";
        public static string FLAG_SETTINGS_RESOLUTION_SCALE = "flagSettings_Scale";

        public static string FLAG_HOUSE_UPGRADE_LEVEL = "flag_houseUpgradeLevel"; //the current house upgrade; starts at 0
        public static string FLAG_CELLAR_UPGRADE_LEVEL = "flag_cellarUpgradeLevel"; //the cellar level, starts at 0 (locked), then gains 1 for each level unlocked to a max of 7
        public static string FLAG_MOUNTAIN_STRATUM_LEVEL = "flag_mountainStratumLevel"; //current maximum stratum unlocked, starts at 0 and maxes at 5
        public static string FLAG_NUM_SHRINES_FIRST_STAGE_COMPLETED = "flag_numShrinesFirstStageCompleted"; //number of shrines where first stage is complete
        public static string FLAG_NUM_SHRINES_FULLY_COMPLETED = "flag_numShrinesFullyCompleted"; //number of shrines where second stage is complete

        public static float LAND_ELEMENT_X = -1000;
        public static float LAND_ELEMENT_Y = -1000;
        public static string LAND_ELEMENT_AREA = Area.AreaEnum.APEX.ToString();
        public static bool LAND_ELEMENT_PRIMED = false;

        public static string FLAG_LETTER_TEST = "flag_letterTest";
        public static string FLAG_LETTER_LOST_ITEMS = "flag_letterLostItems";
        public static string FLAG_LETTER_TUTORIAL_OTIS = "flag_letterTutorialOtis";
        public static string FLAG_LETTER_TUTORIAL_PIPER = "flag_letterTutorialPiper";
        public static string FLAG_LETTER_TUTORIAL_CAMUS = "flag_letterTutorialCamus";
        public static string FLAG_LETTER_TUTORIAL_CABLECAR = "flag_letterTutorialCablecar";
        public static string FLAG_LETTER_TUTORIAL_TROY = "flag_letterTutorialTroy";
        public static string FLAG_LETTER_FESTIVAL_CYCLE = "flag_letterFestivalCycle";
        public static string FLAG_LETTER_FESTIVAL_BEACH = "flag_letterFestivalBeach";
        public static string FLAG_LETTER_FESTIVAL_HARVEST = "flag_letterFestivalHarvest";
        public static string FLAG_LETTER_FESTIVAL_CLEAR_NIGHT = "flag_letterFestivalClearNight";
        public static string FLAG_LETTER_GIFT_AIDEN = "flag_letterGiftAiden";
        public static string FLAG_LETTER_GIFT_MEREDITH = "flag_letterGiftMeredith";
        public static string FLAG_LETTER_GIFT_OTIS = "flag_letterGiftOtis";
        public static string FLAG_LETTER_GIFT_CADE = "flag_letterGiftCade";
        public static string FLAG_LETTER_GIFT_HIMEKO = "flag_letterGiftHimeko";
        public static string FLAG_LETTER_GIFT_CECILY = "flag_letterGiftCecily";
        public static string FLAG_LETTER_GIFT_TROY = "flag_letterGiftTroy";
        public static string FLAG_LETTER_GIFT_RAUL = "flag_letterGiftRaul";
        public static string FLAG_LETTER_GIFT_ROCKWELL = "flag_letterGiftRockwell";
        public static string FLAG_LETTER_GIFT_FINLEY = "flag_letterGiftFinley";
        public static string FLAG_LETTER_GIFT_CAMUS = "flag_letterGiftCamus";
        public static string FLAG_LETTER_GIFT_THEO = "flag_letterGiftTheo";
        public static string FLAG_LETTER_GIFT_ELLE = "flag_letterGiftElle";
        public static string FLAG_LETTER_GIFT_PIPER = "flag_letterGiftPiper";
        public static string FLAG_LETTER_GIFT_PAIGE = "flag_letterGiftPaige";
        public static string FLAG_LETTER_GIFT_CHARLOTTE = "flag_letterGiftCharlotte";
        public static string FLAG_LETTER_GIFT_SKYE = "flag_letterGiftSkye";

        public static string FLAG_BOOK_THE_FARMERS_HANDBOOK_ANIMALS = "flagBook_TheFarmersHandbookAnimals";
        public static string FLAG_BOOK_THE_FARMERS_HANDBOOK_COMPOST = "flagBook_TheFarmersHandbookCompost";
        public static string FLAG_BOOK_THE_FARMERS_HANDBOOK_SEED_MAKERS = "flagBook_TheFarmersHandbookSeedMakers";
        public static string FLAG_BOOK_BASIC_PATTERNS_SS = "flagBook_BasicPatternsSS";
        public static string FLAG_BOOK_FABULOUS_FARMSTEADS = "flagBook_FabulousFarmsteads";
        public static string FLAG_BOOK_BASIC_PATTERNS_FW = "flagBook_BasicPatternsFW";
        public static string FLAG_BOOK_CHILLING_CONFECTIONS = "flagBook_ChillingConfections";
        public static string FLAG_BOOK_DATING_FOR_DUMMIES = "flagBook_DatingForDummies";
        public static string FLAG_BOOK_COUNTRY_PATTERNS = "flagBook_CountryPatterns";
        public static string FLAG_BOOK_A_STUDY_OF_COLOR_IN_NATURE = "flagBook_AStudyOfColorInNature";
        public static string FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_1 = "flagBook_TheForagersCookbookVol1";
        public static string FLAG_BOOK_COOKBOOK_SPRINGS_GIFT = "flagBook_CookbookSpringsGift";
        public static string FLAG_BOOK_STRAIGHTFORWARD_SMELTING = "flagBook_StraightforwardSmelting";
        public static string FLAG_BOOK_MASTERFUL_METALWORKING = "flagBook_MasterfulMetalworking";
        public static string FLAG_BOOK_SIMPLY_WOODWORKING = "flagBook_SimplyWoodworking";
        public static string FLAG_BOOK_A_TOUCH_OF_NATURE = "flagBook_ATouchOfNature";
        public static string FLAG_BOOK_THE_TRADERS_ATLAS = "flagBook_TheTradersAtlas";
        public static string FLAG_BOOK_GREAT_GLASSBLOWING = "flagBook_GreatGlassblowing";
        public static string FLAG_BOOK_AN_ARTISTS_REFLECTION = "flagBook_AnArtistsReflection";
        public static string FLAG_BOOK_FISHING_THROUGH_THE_SEASONS = "flagBook_FishingThroughTheSeasons";
        public static string FLAG_BOOK_ANCIENT_MARINERS_SCROLL = "flagBook_AncientMarinersScroll";
        public static string FLAG_BOOK_COOKBOOK_FOUR_SEASONS = "flagBook_CookbookFourSeasons";
        public static string FLAG_BOOK_JUICY_JAMS_PRECIOUS_PRESERVES_AND_WORTHWHILE_WINES = "flagBook_JuicyJamsPreciousPreservesAndWorthwhileWines";
        public static string FLAG_BOOK_ARTISTIC_SCENT = "flagBook_ArtisticScent";
        public static string FLAG_BOOK_THE_FARMERS_HANDBOOK_ADVANCED = "flagBook_TheFarmersHandbookAdvanced";
        public static string FLAG_BOOK_HOMEMADE_ACCESSORIES = "flagBook_HomemadeAccessories";
        public static string FLAG_BOOK_NATURAL_CRAFTS = "flagBook_NaturalCrafts";
        public static string FLAG_BOOK_SUPPER_WITH_GRANDMA_NINE = "flagBook_SupperWithGrandmaNine";
        public static string FLAG_BOOK_BEEKEEPERS_MANUAL = "flagBook_BeekeepersManual";
        public static string FLAG_BOOK_BREAKFAST_WITH_GRANDMA_NINE = "flagBook_BreakfastWithGrandmaNine";
        public static string FLAG_BOOK_BEEMASTERS_MANUAL = "flagBook_BeemastersManual";
        public static string FLAG_BOOK_BIRDWATCHERS_ISSUE_I = "flagBook_BirdwatchersIssue1";
        public static string FLAG_BOOK_BIRDWATCHERS_ISSUE_II = "flagBook_BirdwatchersIssue2";
        public static string FLAG_BOOK_COOKBOOK_SUMMERS_BOUNTY = "flagBook_CookbookSummersBounty";
        public static string FLAG_BOOK_COOKBOOK_AUTUMNS_HARVEST = "flagBook_CookbookAutumnsHarvest";
        public static string FLAG_BOOK_ESSENTIAL_ENGINEERING = "flagBook_EssentialEngineering";
        public static string FLAG_BOOK_COMPRESSOR_USER_MANUAL = "flagBook_CompressorUserManual";
        public static string FLAG_BOOK_SOUPER_SOUPS = "flagBook_SouperSoups";
        public static string FLAG_BOOK_DUSTY_TOME = "flagBook_DustyTome";
        public static string FLAG_BOOK_UNIQUE_FLAVORS = "flagBook_UniqueFlavors";
        public static string FLAG_BOOK_POTTERY_FOR_FUN_AND_PROFIT = "flagBook_PotteryForFunAndProfit";
        public static string FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_2 = "flagBook_TheForagersCookbookVol2";
        public static string FLAG_BOOK_ELEMENTAL_MYSTICA = "flagBook_ElementalMystica";
        public static string FLAG_BOOK_JEWELERS_HANDBOOK = "flagBook_JewelersHandbook";
        public static string FLAG_BOOK_CRAVING_STONECARVING = "flagBook_CravingStonecarving";
        public static string FLAG_BOOK_MUSTY_TOME = "flagBook_MustyTome";
        public static string FLAG_BOOK_URBAN_DESIGN_BIBLE = "flagBook_UrbanDesignBible";
        public static string FLAG_BOOK_URBAN_PATTERNS = "flagBook_UrbanPatterns";
        public static string FLAG_BOOK_FISH_BEYOND = "flagBook_FishBeyond";
        public static string FLAG_BOOK_TROPICAL_PATTERNS = "flagBook_TropicalPatterns";
        public static string FLAG_BOOK_THE_FARMERS_HANDBOOK_MASTERY = "flagBook_TheFarmersHandbookMastery";
        public static string FLAG_BOOK_THE_FARMERS_HANDBOOK_MYTHS_AND_LEGENDS = "flagBook_TheFarmersHandbookMythsAndLegends";
        public static string FLAG_BOOK_COSTUME_PATTERNS = "flagBook_CostumePatterns";
        public static string FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_3 = "flagBook_TheForagersCookbookVol3";
        public static string FLAG_BOOK_INTENSE_INCENSE = "flagBook_IntenseIncense";
        public static string FLAG_BOOK_CHANNELING_THE_ELEMENTS = "flagBook_ChannelingTheElements";
        public static string FLAG_BOOK_ICE_A_TREATISE = "flagBook_IceATreatise";
        public static string FLAG_BOOK_MUSIC_AT_HOME = "flagBook_MusicAtHome";
        public static string FLAG_BOOK_PLAYGROUND_PREP = "flagBook_PlaygroundPrep";
        public static string FLAG_BOOK_EASTERN_CUISINE = "flagBook_EasternCuisine";
        public static string FLAG_BOOK_FOCI_OF_THE_SHAMAN = "flagBook_FociOfTheShaman";
        public static string FLAG_BOOK_FASHION_PRIMER = "flagBook_FashionPrimer";
        public static string FLAG_BOOK_DECOR_PRIMER = "flagBook_DecorPrimer";
        public static string FLAG_BOOK_WORKING_WITH_YOUR_WORKBENCH = "flagBook_WorkingWithYourWorkbench";

        public static List<Item> LOST_ITEMS = new List<Item>();

        private static Dictionary<string, int> FLAGS = new Dictionary<string, int>();
        private static List<CraftingRecipe> MACHINE_RECIPES = new List<CraftingRecipe>();
        private static List<CraftingRecipe> SCAFFOLDING_RECIPES = new List<CraftingRecipe>();
        private static List<CraftingRecipe> FURNITURE_RECIPES = new List<CraftingRecipe>();
        private static List<CraftingRecipe> WALL_FLOOR_RECIPES = new List<CraftingRecipe>();
        private static List<CraftingRecipe> CLOTHING_RECIPES = new List<CraftingRecipe>();

        private static List<CookingRecipe> TABLE_RECIPES = new List<CookingRecipe>();
        private static List<CookingRecipe> OVEN_RECIPES = new List<CookingRecipe>();

        private static List<CookingRecipe> ALCHEMY_RECIPES = new List<CookingRecipe>();
        private static List<CookingRecipe> ACCESSORY_RECIPES = new List<CookingRecipe>();

        public static Dictionary<Item, Dictionary<Item, Item>> PERFUMERY_RECIPE_DICT;

        public static ShrineStatus SHRINE_SEASON_SPRING_1, SHRINE_SEASON_SPRING_2;
        public static ShrineStatus SHRINE_SEASON_SUMMER_1, SHRINE_SEASON_SUMMER_2;
        public static ShrineStatus SHRINE_SEASON_AUTUMN_1, SHRINE_SEASON_AUTUMN_2;
        public static ShrineStatus SHRINE_SEASON_WINTER_1, SHRINE_SEASON_WINTER_2;
        public static ShrineStatus SHRINE_CHICKEN_1, SHRINE_CHICKEN_2;
        public static ShrineStatus SHRINE_SHEEP_1, SHRINE_SHEEP_2;
        public static ShrineStatus SHRINE_COW_1, SHRINE_COW_2;
        public static ShrineStatus SHRINE_FRIENDSHIP_1, SHRINE_FRIENDSHIP_2;
        public static ShrineStatus SHRINE_LIBRARIAN_1, SHRINE_LIBRARIAN_2;
        public static ShrineStatus SHRINE_PAINTER_1, SHRINE_PAINTER_2;
        public static ShrineStatus SHRINE_CHEF_1, SHRINE_CHEF_2;
        public static ShrineStatus SHRINE_BLACKSMITH_1, SHRINE_BLACKSMITH_2;
        public static ShrineStatus SHRINE_BUILDER_1, SHRINE_BUILDER_2;
        public static ShrineStatus SHRINE_MERCHANT_1, SHRINE_MERCHANT_2;
        public static ShrineStatus SHRINE_SEABIRD_1, SHRINE_SEABIRD_2;
        public static ShrineStatus SHRINE_OLD_MAN_OF_THE_SEA_1, SHRINE_OLD_MAN_OF_THE_SEA_2;
        public static ShrineStatus SHRINE_ARBORIST_1, SHRINE_ARBORIST_2;
        public static ShrineStatus SHRINE_CELEBRATION_1, SHRINE_CELEBRATION_2;
        public static ShrineStatus SHRINE_FRAGRANT_1, SHRINE_FRAGRANT_2;
        public static ShrineStatus SHRINE_INSECT_1, SHRINE_INSECT_2;
        public static ShrineStatus SHRINE_MOUNTAIN_1, SHRINE_MOUNTAIN_2;
        public static ShrineStatus SHRINE_BOAR_1, SHRINE_BOAR_2;
        public static ShrineStatus SHRINE_BEE_1, SHRINE_BEE_2;
        public static ShrineStatus SHRINE_BIRD_1, SHRINE_BIRD_2;
        public static ShrineStatus SHRINE_LABOURER_1, SHRINE_LABOURER_2;
        public static ShrineStatus SHRINE_IRON_1, SHRINE_IRON_2;
        public static ShrineStatus SHRINE_POND_1, SHRINE_POND_2;
        public static ShrineStatus SHRINE_ALCHEMIST_1, SHRINE_ALCHEMIST_2;
        public static ShrineStatus SHRINE_POTTER_1, SHRINE_POTTER_2;
        public static ShrineStatus SHRINE_BAT_1, SHRINE_BAT_2;
        public static ShrineStatus SHRINE_GROUNDWATER_1, SHRINE_GROUNDWATER_2;
        public static ShrineStatus SHRINE_SHINING_1, SHRINE_SHINING_2;
        public static ShrineStatus SHRINE_MUSHROOM_1, SHRINE_MUSHROOM_2;
        public static ShrineStatus SHRINE_CAVER_1, SHRINE_CAVER_2;
        public static ShrineStatus SHRINE_CONCRETE_1, SHRINE_CONCRETE_2;
        public static ShrineStatus SHRINE_JUNGLE_1, SHRINE_JUNGLE_2;
        public static ShrineStatus SHRINE_VOLCANO_1, SHRINE_VOLCANO_2;
        public static ShrineStatus SHRINE_WEAVER_1, SHRINE_WEAVER_2;
        public static ShrineStatus SHRINE_GOLDEN_1, SHRINE_GOLDEN_2;
        public static ShrineStatus SHRINE_LEGENDARY_1, SHRINE_LEGENDARY_2;
        public static ShrineStatus SHRINE_ARCHAEOLOGIST_1, SHRINE_ARCHAEOLOGIST_2;
        public static ShrineStatus SHRINE_MYTH_1, SHRINE_MYTH_2;
        public static ShrineStatus SHRINE_DEPARTED_1, SHRINE_DEPARTED_2;
        public static ShrineStatus SHRINE_SHRUBBERY_1, SHRINE_SHRUBBERY_2;
        public static ShrineStatus SHRINE_CRYSTAL_1, SHRINE_CRYSTAL_2;
        public static ShrineStatus SHRINE_ICE_1, SHRINE_ICE_2;
        public static ShrineStatus SHRINE_LIFE_1, SHRINE_LIFE_2;
        public static ShrineStatus SHRINE_MINER_1, SHRINE_MINER_2;
        public static ShrineStatus SHRINE_SKY_1, SHRINE_SKY_2;
        public static ShrineStatus SHRINE_CREATION_1, SHRINE_CREATION_2;
        public static ShrineTradeSets STS_SEASON_SPRING, STS_SEASON_SUMMER, STS_SEASON_AUTUMN, STS_SEASON_WINTER, STS_CHICKEN, STS_SHEEP, STS_COW, STS_FRIENDSHIP, STS_LIBRARIAN, STS_PAINTER, STS_CHEF,
            STS_BLACKSMITH, STS_BUILDER, STS_MERCHANT, STS_SEABIRD, STS_OLD_MAN_OF_THE_SEA, STS_ARBORIST, STS_CELEBRATION, STS_FRAGRANT, STS_INSECT, STS_MOUNTAIN, STS_BOAR, STS_BEE, STS_BIRD, STS_LABOURER,
            STS_IRON, STS_POND, STS_ALCHEMIST, STS_POTTER, STS_BAT, STS_GROUNDWATER, STS_SHINING, STS_MUSHROOM, STS_CAVER, STS_CONCRETE, STS_JUNGLE, STS_VOLCANO, STS_WEAVER, STS_GOLDEN, STS_LEGENDARY,
            STS_ARCHAEOLOGIST, STS_MYTH, STS_DEPARTED, STS_SHRUBBERY, STS_CRYSTAL, STS_ICE, STS_LIFE, STS_MINER, STS_SKY, STS_CREATION;

        private static List<ShrineStatus> shrineList;

        public static Item[] A_TOUCH_OF_NATURE_UNLOCKS, AN_ARTISTS_REFLECTION_UNLOCKS, URBAN_DESIGN_BIBLE_UNLOCKS, CRAVING_STONECARVING_UNLOCKS, ESSENTIAL_ENGINEERING_UNLOCKS, FABULOUS_FARMSTEADS_UNLOCKS, ICE_A_TREATISE_UNLOCKS,
            MUSIC_AT_HOME_UNLOCKS, PLAYGROUND_PREP_UNLOCKS, SIMPLY_WOODWORKING_UNLOCKS;
        public static Item[] BASIC_PATTERNS_SS_UNLOCKS, BASIC_PATTERNS_FW_UNLOCKS, TROPICAL_PATTERNS_UNLOCKS, URBAN_PATTERNS_UNLOCKS, COSTUME_PATTERNS_UNLOCKS, COUNTRY_PATTERNS_UNLOCKS;

        public static string STAT_SHIPPED_VALUE = "stats_shippedvalue";
        public static string STAT_TIMES_JUMPED = "stats_timesjumped";
        public static string STAT_DISTANCE_TRAVELED = "stats_distancetraveled";
        public static string STAT_ROCKS_MINED = "stats_rocksmined";
        public static string STAT_WOOD_CHOPPED = "stats_woodchopped"; 
        public static string STAT_FORAGE_COLLECTED = "stats_foragecollected";
        public static string STAT_CROPS_HARVESTED = "stats_cropsharvested";
        public static string STAT_ITEMS_CRAFTED = "stats_itemscrafted";
        public static string STAT_TIMES_COOKED = "stats_timescooked";
        public static string STAT_CHESTS_OPENED = "stats_chestsopened";

        public static Dictionary<string, double> STATISTICS;

        public class ShrineTradeSets
        {
            private List<Item> commonTrade;
            private List<Item> uncommonTrade;
            private List<Item> rareTrade;
            private AppliedEffects.Effect effectTrade;

            public ShrineTradeSets(List<Item> common, List<Item> uncommon, List<Item> rare, AppliedEffects.Effect effect)
            {
                this.commonTrade = common;
                this.uncommonTrade = uncommon;
                this.rareTrade = rare;
                this.effectTrade = effect;
            }

            public Item GetCommonItem()
            {
                return commonTrade[Util.RandInt(0, commonTrade.Count - 1)];
            }

            public Item GetUncommonItem()
            {
                return uncommonTrade[Util.RandInt(0, uncommonTrade.Count - 1)];
            }

            public Item GetRareItem()
            {
                return rareTrade[Util.RandInt(0, rareTrade.Count - 1)];
            }

            public AppliedEffects.Effect GetEffect()
            {
                return effectTrade;
            }
        }

        public static void Initialize()
        {
            //MAX OF 24 PER CATEGORY, OR SCRAPBOOK WONT DISPLAY THE OVERFLOW!

            A_TOUCH_OF_NATURE_UNLOCKS = new Item[] { ItemDict.BAMBOO_POT, ItemDict.BUOY, ItemDict.CAMPFIRE, ItemDict.DECORATIVE_BOULDER, ItemDict.DECORATIVE_LOG, ItemDict.FIREPIT, ItemDict.HAMMOCK, ItemDict.LIFEBUOY_SIGN,
            ItemDict.SANDCASTLE, ItemDict.SURFBOARD, ItemDict.UMBRELLA, ItemDict.UMBRELLA_TABLE, 
                ItemDict.STAR_WALLPAPER, ItemDict.BUBBLE_WALLPAPER, ItemDict.WAVE_WALLPAPER, 
                ItemDict.TATAMI_FLOOR, ItemDict.THIN_TATAMI_FLOOR, 
                ItemDict.WALL_PLANK, ItemDict.PLATFORM_PLANK, ItemDict.BLOCK_PLANK, ItemDict.BLOCK_BARK,
                ItemDict.BAMBOO_FENCE,}; //23

            AN_ARTISTS_REFLECTION_UNLOCKS = new Item[] { ItemDict.BANNER, ItemDict.CANVAS, ItemDict.ANATOMICAL_POSTER, ItemDict.HELIX_POSTER, ItemDict.HORIZONTAL_MIRROR, 
                ItemDict.ORNATE_MIRROR, ItemDict.WALL_MIRROR, ItemDict.TRIPLE_MIRRORS, ItemDict.RAINBOW_GRAFFITI, ItemDict.HEARTBREAK_GRAFFITI, 
                ItemDict.TRIANGULATE_FLOOR, ItemDict.ODD_WALLPAPER, 
                ItemDict.GLASS_FENCE}; //13

            URBAN_DESIGN_BIBLE_UNLOCKS = new Item[] { ItemDict.BOOMBOX, ItemDict.FIRE_HYDRANT, ItemDict.GYM_BENCH, ItemDict.POSTBOX, ItemDict.RECYCLING_BIN, ItemDict.SOFA, ItemDict.TRAFFIC_CONE, ItemDict.TRAFFIC_LIGHT, ItemDict.TRASHCAN, 
                ItemDict.FULL_THROTTLE_GRAFFITI, ItemDict.HEROINE_GRAFFITI, ItemDict.LEFTWARD_GRAFFITI,ItemDict.NOIZEBOYZ_GRAFFITI, ItemDict.RETRO_GRAFFITI, ItemDict.RIGHT_ARROW_GRAFFITI, ItemDict.SMILE_GRAFFITI, ItemDict.SOURCE_UNKNOWN_GRAFFITI,
                ItemDict.INVADER_WALLPAPER, 
                ItemDict.STREET_FLOOR, 
                ItemDict.SCAFFOLDING_GOLDEN, ItemDict.BLOCK_GOLDEN, ItemDict.PLATFORM_GOLDEN, ItemDict.PLATFORM_GOLDEN_FARM,
                ItemDict.GOLDEN_FENCE}; //24
            
            CRAVING_STONECARVING_UNLOCKS = new Item[] { ItemDict.CUBE_STATUE, ItemDict.PYRAMID_STATUE, ItemDict.SPHERE_STATUE, ItemDict.STATUE, ItemDict.STONE_COLUMN, ItemDict.FIREPLACE, ItemDict.WELL,
            ItemDict.CONCRETE_FLOOR, ItemDict.SQUARE_FLOOR, 
                ItemDict.WALL_STONE, ItemDict.BLOCK_STONE, ItemDict.PLATFORM_STONE, ItemDict.PLATFORM_STONE_FARM, 
                ItemDict.STONE_FENCE}; //14

            ESSENTIAL_ENGINEERING_UNLOCKS = new Item[] { ItemDict.CLOCK, ItemDict.GRANDFATHER_CLOCK, ItemDict.STREETLAMP, ItemDict.STREETLIGHT, ItemDict.TELEVISION, ItemDict.LAMP, ItemDict.LANTERN, ItemDict.LIGHTNING_ROD,
            ItemDict.SOLAR_PANEL, ItemDict.MINECART, 
                ItemDict.SCAFFOLDING_METAL,  ItemDict.WALL_METAL, ItemDict.BLOCK_METAL, ItemDict.PLATFORM_METAL, ItemDict.PLATFORM_METAL_FARM}; //15

            FABULOUS_FARMSTEADS_UNLOCKS = new Item[] { ItemDict.CART, ItemDict.CLOTHESLINE, ItemDict.FLAGPOLE, ItemDict.HAYBALE, ItemDict.MAILBOX, ItemDict.MARKET_STALL, ItemDict.MILK_JUG,
                ItemDict.PILE_OF_BRICKS, ItemDict.SCARECROW, ItemDict.SIGNPOST, ItemDict.TOOLBOX, ItemDict.TOOLRACK, ItemDict.WAGON, ItemDict.WATER_PUMP, ItemDict.WHEELBARROW,  
                ItemDict.POLKA_WALLPAPER, ItemDict.DOT_WALLPAPER,
                ItemDict.STEPPING_STONE_FLOOR, ItemDict.CARPET_FLOOR,
                ItemDict.SCAFFOLDING_WOOD, ItemDict.BLOCK_WOOD, ItemDict.PLATFORM_WOOD, ItemDict.PLATFORM_WOOD_FARM,
                ItemDict.TALL_FENCE}; //24
            
            ICE_A_TREATISE_UNLOCKS = new Item[] { ItemDict.FROST_SCULPTURE, ItemDict.ICE_BLOCK, ItemDict.SNOWMAN, ItemDict.IGLOO, 
                ItemDict.SCAFFOLDING_MYTHRIL, ItemDict.BLOCK_MYTHRIL, ItemDict.PLATFORM_MYTHRIL, ItemDict.PLATFORM_MYTHRIL_FARM, 
                ItemDict.MYTHRIL_FENCE }; //8
            
            PLAYGROUND_PREP_UNLOCKS = new Item[] { ItemDict.BLACKBOARD, ItemDict.DRUM, ItemDict.SANDBOX, ItemDict.SEESAW, ItemDict.SLIDE, ItemDict.SWINGS, ItemDict.TARGET, ItemDict.WHITEBOARD, ItemDict.PET_BOWL, ItemDict.BELL, ItemDict.WATERTOWER,
                ItemDict.BOARDWALK_FLOOR, ItemDict.FOOTPRINT_FLOOR, ItemDict.MAT_FLOOR,
                ItemDict.METAL_FENCE}; //15
            
            SIMPLY_WOODWORKING_UNLOCKS = new Item[] { ItemDict.BOX, ItemDict.BRAZIER, ItemDict.CRATE, ItemDict.GARDEN_ARCH, ItemDict.LATTICE, ItemDict.WOODEN_BENCH, ItemDict.WOODEN_CHAIR, ItemDict.WOODEN_COLUMN,
                ItemDict.WOODEN_LONGTABLE, ItemDict.WOODEN_ROUNDTABLE, ItemDict.WOODEN_SQUARETABLE, ItemDict.WOODEN_STOOL, ItemDict.WOODEN_POST,
                ItemDict.HORIZONTAL_WALLPAPER, ItemDict.VERTICAL_WALLPAPER, ItemDict.SOLID_WALLPAPER, 
                ItemDict.WOODEN_FLOOR, 
                ItemDict.WOODEN_FENCE }; //12

            MUSIC_AT_HOME_UNLOCKS = new Item[] { ItemDict.DRUMSET, ItemDict.HARP, ItemDict.CYMBAL, ItemDict.PIANO, ItemDict.GUITAR_PLACEABLE, ItemDict.XYLOPHONE }; //6

            BASIC_PATTERNS_SS_UNLOCKS = new Item[] { ItemDict.RUCKSACK, ItemDict.GUITAR, ItemDict.GLASSES, ItemDict.GOGGLES, ItemDict.BASEBALL_CAP, ItemDict.HEADBAND, ItemDict.BUTTERFLY_CLIP, ItemDict.ALL_SEASON_JACKET, ItemDict.JEANS,
            ItemDict.CHINO_SHORTS, ItemDict.SHORT_SKIRT, ItemDict.PUFF_SKIRT, ItemDict.SAILCLOTH, ItemDict.SHORT_SLEEVE_TEE, ItemDict.STRIPED_SHIRT, ItemDict.TANKER, ItemDict.SNEAKERS, ItemDict.SHORT_SOCKS}; //18

            BASIC_PATTERNS_FW_UNLOCKS = new Item[] { ItemDict.WOOL_MITTENS, ItemDict.BOWLER, ItemDict.CAMEL_HAT, ItemDict.SQUARE_HAT, ItemDict.FLAT_CAP, ItemDict.NECKWARMER, ItemDict.SCRAP_BRACER, ItemDict.HOODED_SWEATSHIRT, ItemDict.OVERCOAT,
            ItemDict.CHINOS, ItemDict.LONG_SKIRT, ItemDict.LONG_SLEEVE_TEE, ItemDict.BUTTON_DOWN, ItemDict.PLAID_BUTTON, ItemDict.TURTLENECK, ItemDict.SWEATER, ItemDict.HIGH_TOPS, ItemDict.LONG_SOCKS, ItemDict.STRIPED_SOCKS,
            ItemDict.FESTIVE_SOCKS}; //20

            COSTUME_PATTERNS_UNLOCKS = new Item[] { ItemDict.CAPE, ItemDict.WOLF_TAIL, ItemDict.CLOCKWORK, ItemDict.ROBO_ARMS, ItemDict.DANGLE_EARRING, ItemDict.BLINDFOLD, ItemDict.BOXING_MITTS, ItemDict.DINO_MASK, ItemDict.DOG_MASK,
            ItemDict.NIGHTCAP, ItemDict.CHEFS_HAT, ItemDict.NIGHTMARE_MASK, ItemDict.MEDAL, ItemDict.SPORTBALL_UNIFORM, ItemDict.MISMATTCHED, ItemDict.FOX_TAIL}; //16

            COUNTRY_PATTERNS_UNLOCKS = new Item[] { ItemDict.BACKPACK, ItemDict.PROTECTIVE_VISOR, ItemDict.WORK_GLOVES, ItemDict.FACEMASK, ItemDict.CONICAL_FARMER, ItemDict.STRAW_HAT, ItemDict.BANDANA, ItemDict.TEN_GALLON,
            ItemDict.SASH, ItemDict.APRON, ItemDict.OVERALLS, ItemDict.TALL_BOOTS}; //12

            TROPICAL_PATTERNS_UNLOCKS = new Item[] {ItemDict.CAT_TAIL, ItemDict.SNORKEL, ItemDict.EYEPATCH, ItemDict.TRACE_TATTOO, ItemDict.WHISKERS, ItemDict.CAT_EARS, ItemDict.BUNNY_EARS, ItemDict.NOMAD_VEST, ItemDict.BATHROBE, ItemDict.SUPER_SHORTS,
            ItemDict.TIGHTIES, ItemDict.ISLANDER_TATTOO, ItemDict.LINEN_BUTTON, ItemDict.WING_SANDLES}; //14

            URBAN_PATTERNS_UNLOCKS = new Item[] { ItemDict.EARRING_STUD, ItemDict.PIERCING, ItemDict.SUNGLASSES, ItemDict.QUERADE_MASK, ItemDict.TOP_HAT, ItemDict.SNAPBACK, ItemDict.ASCOT, ItemDict.NECKLACE, ItemDict.TIE, ItemDict.RAINCOAT,
            ItemDict.PUNK_JACKET, ItemDict.ONESIE, ItemDict.WEDDING_DRESS, ItemDict.SUIT_JACKET, ItemDict.TORN_JEANS, ItemDict.JEAN_SHORTS, ItemDict.FLASH_HEELS}; //17

            STATISTICS = new Dictionary<string, double>();
            STATISTICS[STAT_SHIPPED_VALUE] = 0;
            STATISTICS[STAT_TIMES_JUMPED] = 0;
            STATISTICS[STAT_DISTANCE_TRAVELED] = 0;
            STATISTICS[STAT_ROCKS_MINED] = 0;
            STATISTICS[STAT_WOOD_CHOPPED] = 0;
            STATISTICS[STAT_FORAGE_COLLECTED] = 0;
            STATISTICS[STAT_CROPS_HARVESTED] = 0;
            STATISTICS[STAT_ITEMS_CRAFTED] = 0;
            STATISTICS[STAT_TIMES_COOKED] = 0;
            STATISTICS[STAT_CHESTS_OPENED] = 0;

            //FURNITURE_RECIPES.Add(new Recipe(new ItemStack(ItemDict., 1), new ItemStack(ItemDict.,), new ItemStack(ItemDict.,), new ItemStack(ItemDict.,), new ItemStack(ItemDict.,)));
            FLAGS[FLAG_SETTINGS_HIDE_CONTROLS] = 0;
            FLAGS[FLAG_SETTINGS_HIDE_GRID] = 0;
            FLAGS[FLAG_SETTINGS_HIDE_RETICLE] = 0;
            FLAGS[FLAG_SETTINGS_WINDOWED] = 0;
            FLAGS[FLAG_SETTINGS_SFX_VOLUME] = 5;
            FLAGS[FLAG_SETTINGS_MUSIC_VOLUME] = 5;
            FLAGS[FLAG_SETTINGS_RESOLUTION_SCALE] = PlateauMain.CURRENT_RESOLUTION.scale;

            FLAGS[FLAG_HOUSE_UPGRADE_LEVEL] = 0;
            FLAGS[FLAG_MOUNTAIN_STRATUM_LEVEL] = 0;
            FLAGS[FLAG_CELLAR_UPGRADE_LEVEL] = 0;
            FLAGS[FLAG_NUM_SHRINES_FIRST_STAGE_COMPLETED] = 0;
            FLAGS[FLAG_NUM_SHRINES_FULLY_COMPLETED] = 0;

            LAND_ELEMENT_AREA = Area.AreaEnum.APEX.ToString();
            LAND_ELEMENT_X = 0;
            LAND_ELEMENT_Y = 0;
            LAND_ELEMENT_PRIMED = false;

            FLAGS[FLAG_LETTER_TEST] = 0;
            FLAGS[FLAG_LETTER_GIFT_AIDEN] = 0;
            FLAGS[FLAG_LETTER_LOST_ITEMS] = 0;
            FLAGS[FLAG_LETTER_TUTORIAL_OTIS] = 0;
            FLAGS[FLAG_LETTER_TUTORIAL_PIPER] = 0;
            FLAGS[FLAG_LETTER_TUTORIAL_CAMUS] = 0;
            FLAGS[FLAG_LETTER_TUTORIAL_CABLECAR] = 0;
            FLAGS[FLAG_LETTER_TUTORIAL_TROY] = 0;
            FLAGS[FLAG_LETTER_FESTIVAL_CYCLE] = 0;
            FLAGS[FLAG_LETTER_FESTIVAL_BEACH] = 0;
            FLAGS[FLAG_LETTER_FESTIVAL_HARVEST] = 0;
            FLAGS[FLAG_LETTER_FESTIVAL_CLEAR_NIGHT] = 0;
            FLAGS[FLAG_LETTER_GIFT_AIDEN] = 0;
            FLAGS[FLAG_LETTER_GIFT_MEREDITH] = 0;
            FLAGS[FLAG_LETTER_GIFT_OTIS] = 0;
            FLAGS[FLAG_LETTER_GIFT_CADE] = 0;
            FLAGS[FLAG_LETTER_GIFT_HIMEKO] = 0;
            FLAGS[FLAG_LETTER_GIFT_CECILY] = 0;
            FLAGS[FLAG_LETTER_GIFT_TROY] = 0;
            FLAGS[FLAG_LETTER_GIFT_RAUL] = 0;
            FLAGS[FLAG_LETTER_GIFT_ROCKWELL] = 0;
            FLAGS[FLAG_LETTER_GIFT_FINLEY] = 0;
            FLAGS[FLAG_LETTER_GIFT_CAMUS] = 0;
            FLAGS[FLAG_LETTER_GIFT_THEO] = 0;
            FLAGS[FLAG_LETTER_GIFT_ELLE] = 0;
            FLAGS[FLAG_LETTER_GIFT_PIPER] = 0;
            FLAGS[FLAG_LETTER_GIFT_PAIGE] = 0;
            FLAGS[FLAG_LETTER_GIFT_CHARLOTTE] = 0;
            FLAGS[FLAG_LETTER_GIFT_SKYE] = 0;

            FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_ANIMALS] = 0;
            FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_COMPOST] = 0;
            FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_SEED_MAKERS] = 0;
            FLAGS[FLAG_BOOK_BASIC_PATTERNS_SS] = 0;
            FLAGS[FLAG_BOOK_FABULOUS_FARMSTEADS] = 0;
            FLAGS[FLAG_BOOK_BASIC_PATTERNS_FW] = 0;
            FLAGS[FLAG_BOOK_CHILLING_CONFECTIONS] = 0;
            FLAGS[FLAG_BOOK_DATING_FOR_DUMMIES] = 0;
            FLAGS[FLAG_BOOK_COUNTRY_PATTERNS] = 0;
            FLAGS[FLAG_BOOK_A_STUDY_OF_COLOR_IN_NATURE] = 0;
            FLAGS[FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_1] = 0;
            FLAGS[FLAG_BOOK_COOKBOOK_SPRINGS_GIFT] = 0;
            FLAGS[FLAG_BOOK_STRAIGHTFORWARD_SMELTING] = 0;
            FLAGS[FLAG_BOOK_MASTERFUL_METALWORKING] = 0;
            FLAGS[FLAG_BOOK_SIMPLY_WOODWORKING] = 0;
            FLAGS[FLAG_BOOK_A_TOUCH_OF_NATURE] = 0;
            FLAGS[FLAG_BOOK_THE_TRADERS_ATLAS] = 0;
            FLAGS[FLAG_BOOK_GREAT_GLASSBLOWING] = 0;
            FLAGS[FLAG_BOOK_AN_ARTISTS_REFLECTION] = 0;
            FLAGS[FLAG_BOOK_FISHING_THROUGH_THE_SEASONS] = 0;
            FLAGS[FLAG_BOOK_ANCIENT_MARINERS_SCROLL] = 0;
            FLAGS[FLAG_BOOK_COOKBOOK_FOUR_SEASONS] = 0;
            FLAGS[FLAG_BOOK_JUICY_JAMS_PRECIOUS_PRESERVES_AND_WORTHWHILE_WINES] = 0;
            FLAGS[FLAG_BOOK_ARTISTIC_SCENT] = 0;
            FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_ADVANCED] = 0;
            FLAGS[FLAG_BOOK_HOMEMADE_ACCESSORIES] = 0;
            FLAGS[FLAG_BOOK_NATURAL_CRAFTS] = 0;
            FLAGS[FLAG_BOOK_SUPPER_WITH_GRANDMA_NINE] = 0;
            FLAGS[FLAG_BOOK_BEEKEEPERS_MANUAL] = 0;
            FLAGS[FLAG_BOOK_BREAKFAST_WITH_GRANDMA_NINE] = 0;
            FLAGS[FLAG_BOOK_BEEMASTERS_MANUAL] = 0;
            FLAGS[FLAG_BOOK_BIRDWATCHERS_ISSUE_I] = 0;
            FLAGS[FLAG_BOOK_BIRDWATCHERS_ISSUE_II] = 0;
            FLAGS[FLAG_BOOK_COOKBOOK_SUMMERS_BOUNTY] = 0;
            FLAGS[FLAG_BOOK_COOKBOOK_AUTUMNS_HARVEST] = 0;
            FLAGS[FLAG_BOOK_ESSENTIAL_ENGINEERING] = 0;
            FLAGS[FLAG_BOOK_COMPRESSOR_USER_MANUAL] = 0;
            FLAGS[FLAG_BOOK_SOUPER_SOUPS] = 0;
            FLAGS[FLAG_BOOK_DUSTY_TOME] = 0;
            FLAGS[FLAG_BOOK_UNIQUE_FLAVORS] = 0;
            FLAGS[FLAG_BOOK_POTTERY_FOR_FUN_AND_PROFIT] = 0;
            FLAGS[FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_2] = 0;
            FLAGS[FLAG_BOOK_ELEMENTAL_MYSTICA] = 0;
            FLAGS[FLAG_BOOK_JEWELERS_HANDBOOK] = 0;
            FLAGS[FLAG_BOOK_CRAVING_STONECARVING] = 0;
            FLAGS[FLAG_BOOK_MUSTY_TOME] = 0;
            FLAGS[FLAG_BOOK_URBAN_DESIGN_BIBLE] = 0;
            FLAGS[FLAG_BOOK_URBAN_PATTERNS] = 0;
            FLAGS[FLAG_BOOK_FISH_BEYOND] = 0;
            FLAGS[FLAG_BOOK_TROPICAL_PATTERNS] = 0;
            FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_MASTERY] = 0;
            FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_MYTHS_AND_LEGENDS] = 0;
            FLAGS[FLAG_BOOK_COSTUME_PATTERNS] = 0;
            FLAGS[FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_3] = 0;
            FLAGS[FLAG_BOOK_INTENSE_INCENSE] = 0;
            FLAGS[FLAG_BOOK_CHANNELING_THE_ELEMENTS] = 0;
            FLAGS[FLAG_BOOK_ICE_A_TREATISE] = 0;
            FLAGS[FLAG_BOOK_MUSIC_AT_HOME] = 0;
            FLAGS[FLAG_BOOK_PLAYGROUND_PREP] = 0;
            FLAGS[FLAG_BOOK_EASTERN_CUISINE] = 0;
            FLAGS[FLAG_BOOK_FOCI_OF_THE_SHAMAN] = 0;
            FLAGS[FLAG_BOOK_FASHION_PRIMER] = 0;
            FLAGS[FLAG_BOOK_DECOR_PRIMER] = 0;
            FLAGS[FLAG_BOOK_WORKING_WITH_YOUR_WORKBENCH] = 0;

            STS_SEASON_SPRING = new ShrineTradeSets(new List<Item> {ItemDict.BLUEBELL, ItemDict.CHICKWEED, ItemDict.NETTLES }, 
                new List<Item> {ItemDict.RASPBERRY, ItemDict.SPINACH_SEEDS, ItemDict.CARROT_SEEDS, ItemDict.POTATO_SEEDS }, 
                new List<Item> {ItemDict.STRAWBERRY_SEEDS}, 
                AppliedEffects.FORAGING_III_SPRING);
            STS_SEASON_SUMMER = new ShrineTradeSets(new List<Item> { ItemDict.MARIGOLD, ItemDict.LAVENDER, ItemDict.SUNFLOWER },
                new List<Item> { ItemDict.BLUEBERRY, ItemDict.CACTUS_SEEDS, ItemDict.TOMATO_SEEDS, ItemDict.CUCUMBER_SEEDS, ItemDict.ONION_SEEDS },
                new List<Item> { ItemDict.EGGPLANT_SEEDS },
                AppliedEffects.FORAGING_III_SUMMER);
            STS_SEASON_AUTUMN = new ShrineTradeSets(new List<Item> { ItemDict.WILD_RICE, ItemDict.PERSIMMON, ItemDict.SASSAFRAS },
                new List<Item> { ItemDict.BLACKBERRY, ItemDict.ELDERBERRY, ItemDict.BEET_SEEDS, ItemDict.BELLPEPPER_SEEDS, ItemDict.BROCCOLI_SEEDS },
                new List<Item> { ItemDict.PUMPKIN_SEEDS },
                AppliedEffects.FORAGING_III_AUTUMN);
            STS_SEASON_WINTER = new ShrineTradeSets(new List<Item> { ItemDict.WINTERGREEN, ItemDict.CHICORY_ROOT, ItemDict.SNOWDROP },
                new List<Item> { ItemDict.FROST_COMPOST},
                new List<Item> { ItemDict.CABBAGE_SEEDS },
                AppliedEffects.FORAGING_II_WINTER);
            STS_CHICKEN = new ShrineTradeSets(new List<Item> { ItemDict.EGG, ItemDict.WHITE_FEATHER },
                new List<Item> { ItemDict.MAYONNAISE, ItemDict.RED_FEATHER },
                new List<Item> { ItemDict.GOLDEN_EGG },
                AppliedEffects.GATHERING_CHICKEN);
            STS_SHEEP = new ShrineTradeSets(new List<Item> { ItemDict.WOOL },
                new List<Item> { ItemDict.WOOLEN_CLOTH },
                new List<Item> { ItemDict.GOLDEN_WOOL },
                AppliedEffects.GATHERING_SHEEP);
            STS_COW = new ShrineTradeSets(new List<Item> { ItemDict.MILK},
                new List<Item> { ItemDict.CREAM, ItemDict.BUTTER, ItemDict.CHEESE },
                new List<Item> { ItemDict.WATERMELON_SLICE },
                AppliedEffects.GATHERING_COW);
            STS_FRIENDSHIP = new ShrineTradeSets(new List<Item> { ItemDict.ANCIENT_KEY },
                new List<Item> { ItemDict.ANCIENT_KEY, ItemDict.METAMORPHIC_KEY, ItemDict.SEDIMENTARY_KEY, ItemDict.IGNEOUS_KEY, ItemDict.CRYSTAL_KEY },
                new List<Item> { ItemDict.HEART_VESSEL },
                AppliedEffects.SPEED_II);
            STS_LIBRARIAN = new ShrineTradeSets(new List<Item> { ItemDict.PAPER },
                new List<Item> { ItemDict.BLACK_DYE, ItemDict.BLUE_DYE, ItemDict.DARK_BROWN_DYE, ItemDict.DARK_GREY_DYE, ItemDict.GREEN_DYE, ItemDict.LIGHT_BROWN_DYE, ItemDict.LIGHT_GREY_DYE, ItemDict.NAVY_DYE, ItemDict.OLIVE_DYE, ItemDict.ORANGE_DYE, ItemDict.RED_DYE, ItemDict.PURPLE_DYE, ItemDict.WHITE_DYE, ItemDict.YELLOW_DYE },
                new List<Item> { ItemDict.SKY_ROSE }, 
                AppliedEffects.SPEED_II);
            STS_PAINTER = new ShrineTradeSets(new List<Item> { ItemDict.SEA_URCHIN, ItemDict.INKY_SQUID, ItemDict.MOSSY_BARK, ItemDict.EMERALD_MOSS },
                new List<Item> { ItemDict.ROYAL_JELLY, ItemDict.POLLEN_PUFF, ItemDict.RED_GINGER, ItemDict.CACAO_BEAN, ItemDict.CAVE_SOYBEAN },
                new List<Item> { ItemDict.PEARL },
                AppliedEffects.BUG_CATCHING_II);
            STS_CHEF = new ShrineTradeSets(new List<Item> { ItemDict.REJUVENATION_TEA, ItemDict.BAKED_POTATO, ItemDict.ESCARGOT, ItemDict.SPRING_PIZZA, ItemDict.WILD_POPCORN, ItemDict.VANILLA_BEAN },
                new List<Item> { ItemDict.SEASONAL_PIPERADE, ItemDict.GRILLED_SALMON, ItemDict.SEAFOOD_BASKET, ItemDict.RATATOUILLE, ItemDict.SEAFOOD_PAELLA, ItemDict.MINT_CHOCO_BAR },
                new List<Item> { ItemDict.SOUR_WINE },
                AppliedEffects.FORAGING_II);
            STS_BLACKSMITH = new ShrineTradeSets(new List<Item> { ItemDict.SCRAP_IRON },
                new List<Item> { ItemDict.IRON_ORE, ItemDict.COAL, ItemDict.SCRAP_IRON },
                new List<Item> { ItemDict.MYTHRIL_BAR },
                AppliedEffects.MINING_II);
            STS_BUILDER = new ShrineTradeSets(new List<Item> { ItemDict.WOOD, ItemDict.HARDWOOD, ItemDict.BAMBOO, ItemDict.CLAY },
                new List<Item> { ItemDict.PLANK, ItemDict.BOARD, ItemDict.GEARS, ItemDict.BRICKS },
                new List<Item> { ItemDict.GOLDEN_LEAF },
                AppliedEffects.CHOPPING_II);
            STS_MERCHANT = new ShrineTradeSets(new List<Item> { ItemDict.LUCKY_COIN },
                new List<Item> { ItemDict.GOLD_ORE, ItemDict.STONE },
                new List<Item> { ItemDict.GOLD_BAR },
                AppliedEffects.SPEED_II);
            STS_SEABIRD = new ShrineTradeSets(new List<Item> { ItemDict.SEAWEED, ItemDict.CRIMSON_CORAL, ItemDict.SEA_URCHIN, ItemDict.COCONUT },
                new List<Item> { ItemDict.CRAB, ItemDict.WHITE_FEATHER, ItemDict.RED_SNAPPER },
                new List<Item> { ItemDict.FLAWLESS_CONCH },
                AppliedEffects.FORAGING_III_BEACH);
            STS_OLD_MAN_OF_THE_SEA = new ShrineTradeSets(new List<Item> { ItemDict.CLAM, ItemDict.OYSTER, ItemDict.SARDINE, ItemDict.HERRING, ItemDict.STRIPED_BASS },
                new List<Item> { ItemDict.MACKEREL, ItemDict.TUNA, ItemDict.PUFFERFISH, ItemDict.SHRIMP, ItemDict.SWORDFISH },
                new List<Item> { ItemDict.GREAT_WHITE_SHARK },
                AppliedEffects.FISHING_III_OCEAN);
            STS_CELEBRATION = new ShrineTradeSets(new List<Item> { ItemDict.LOAMY_COMPOST, ItemDict.DECAY_COMPOST, ItemDict.THICK_COMPOST },
                new List<Item> { ItemDict.QUALITY_COMPOST, ItemDict.SWEET_COMPOST, ItemDict.DECAY_COMPOST },
                new List<Item> { ItemDict.SHINING_COMPOST },
                AppliedEffects.BUG_CATCHING_II);
            STS_ARBORIST = new ShrineTradeSets(new List<Item> { ItemDict.WOOD, ItemDict.CHERRY_SAPLING, ItemDict.APPLE_SAPLING },
                new List<Item> { ItemDict.HARDWOOD, ItemDict.LEMON, ItemDict.ORANGE_SAPLING, ItemDict.BANANA_PALM_SAPLING },
                new List<Item> { ItemDict.OLIVE_SAPLING },
                AppliedEffects.CHOPPING_III);
            STS_FRAGRANT = new ShrineTradeSets(new List<Item> { ItemDict.FLORAL_PERFUME, ItemDict.BIZARRE_PERFUME, ItemDict.SWEET_BREEZE },
                new List<Item> { ItemDict.SUMMERS_GIFT, ItemDict.OCEAN_GUST, ItemDict.BLISSFUL_SKY, ItemDict.WARM_MEMORIES, ItemDict.AUTUMNS_KISS },
                new List<Item> { ItemDict.RED_ANGEL },
                AppliedEffects.FORAGING_III_FLOWERS);
            STS_INSECT = new ShrineTradeSets(new List<Item> { ItemDict.SNAIL, ItemDict.RICE_GRASSHOPPER, ItemDict.SOLDIER_ANT, ItemDict.BROWN_CICADA },
                new List<Item> { ItemDict.BANDED_DRAGONFLY, ItemDict.YELLOW_BUTTERFLY, ItemDict.STINGER_HORNET, ItemDict.PINK_LADYBUG },
                new List<Item> { ItemDict.STAG_BEETLE },
                AppliedEffects.BUG_CATCHING_III);
            STS_MOUNTAIN = new ShrineTradeSets(new List<Item> { ItemDict.BAMBOO },
                new List<Item> { ItemDict.SPICY_LEAF, ItemDict.MOREL, ItemDict.MOUNTAIN_WHEAT },
                new List<Item> { ItemDict.WIND_CRYSTAL },
                AppliedEffects.FORAGING_III);
            STS_BOAR = new ShrineTradeSets(new List<Item> { ItemDict.WILD_MEAT, ItemDict.BOAR_HIDE },
                new List<Item> { ItemDict.BOAR_STEW, ItemDict.BOAR_JERKY, ItemDict.MEATY_PIZZA },
                new List<Item> { ItemDict.COCONUT_BOAR },
                AppliedEffects.GATHERING_BOAR);
            STS_BEE = new ShrineTradeSets(new List<Item> { ItemDict.WILD_HONEY, ItemDict.HONEYCOMB, ItemDict.HONEY_BEE },
                new List<Item> { ItemDict.BEESWAX, ItemDict.ROYAL_JELLY, ItemDict.POLLEN_PUFF },
                new List<Item> { ItemDict.QUEENS_STINGER },
                AppliedEffects.BUG_CATCHING_III);
            STS_BIRD = new ShrineTradeSets(new List<Item> { ItemDict.BIRDS_NEST, ItemDict.BLACK_FEATHER },
                new List<Item> { ItemDict.BLUE_FEATHER, ItemDict.RED_FEATHER },
                new List<Item> { ItemDict.PRISMATIC_FEATHER },
                AppliedEffects.FORAGING_II);
            STS_LABOURER = new ShrineTradeSets(new List<Item> { ItemDict.WOOD, ItemDict.STONE, ItemDict.CLAY },
                new List<Item> { ItemDict.HARDWOOD, ItemDict.BAMBOO, ItemDict.IRON_ORE },
                new List<Item> { ItemDict.ADAMANTITE_ORE },
                AppliedEffects.CHOPPING_II);
            STS_IRON = new ShrineTradeSets(new List<Item> { ItemDict.SCRAP_IRON, ItemDict.IRON_ORE, ItemDict.IRON_CHIP },
                new List<Item> { ItemDict.IRON_ORE },
                new List<Item> { ItemDict.FURNACE },
                AppliedEffects.MINING_II);
            STS_POND = new ShrineTradeSets(new List<Item> { ItemDict.BLUEGILL, ItemDict.CARP, ItemDict.CATFISH },
                new List<Item> { ItemDict.LAKE_TROUT, ItemDict.JUNGLE_PIRANHA, ItemDict.SALMON, ItemDict.PIKE, ItemDict.SUNFISH },
                new List<Item> { ItemDict.EMPEROR_SALMON },
                AppliedEffects.FISHING_II);
            STS_ALCHEMIST = new ShrineTradeSets(new List<Item> { ItemDict.BLACK_CANDLE, ItemDict.SALTED_CANDLE, ItemDict.SPICED_CANDLE, ItemDict.MOSS_BOTTLE },
                new List<Item> { ItemDict.TROPICAL_BOTTLE, ItemDict.VOODOO_STEW, ItemDict.BURST_STONE },
                new List<Item> { ItemDict.UNSTABLE_LIQUID },
                AppliedEffects.SPEED_II);
            STS_POTTER = new ShrineTradeSets(new List<Item> { ItemDict.CLAY },
                new List<Item> { ItemDict.EARTHWORM, ItemDict.CAVEWORM },
                new List<Item> { ItemDict.POTTERY_WHEEL },
                AppliedEffects.MINING_II);
            STS_BAT = new ShrineTradeSets(new List<Item> { ItemDict.GUANO },
                new List<Item> { ItemDict.BAT_WING },
                new List<Item> { ItemDict.ALBINO_WING },
                AppliedEffects.MINING_II);
            STS_GROUNDWATER = new ShrineTradeSets(new List<Item> { ItemDict.CAVEFISH },
                new List<Item> { ItemDict.CAVERN_TETRA },
                new List<Item> { ItemDict.DARK_ANGLER },
                AppliedEffects.FISHING_IV_CAVE);
            STS_SHINING = new ShrineTradeSets(new List<Item> { ItemDict.QUARTZ },
                new List<Item> { ItemDict.AMETHYST, ItemDict.AQUAMARINE, ItemDict.TOPAZ },
                new List<Item> { ItemDict.DIAMOND, ItemDict.EMERALD, ItemDict.SAPPHIRE, ItemDict.RUBY },
                AppliedEffects.MINING_III);
            STS_MUSHROOM = new ShrineTradeSets(new List<Item> { ItemDict.SPORES },
                new List<Item> { ItemDict.CAVE_FUNGI, ItemDict.OYSTER_MUSHROOM, ItemDict.CHANTERELLE, ItemDict.MOREL, ItemDict.TRUFFLE },
                new List<Item> { ItemDict.SHIITAKE },
                AppliedEffects.FORAGING_III_MUSHROOMS);
            STS_CAVER = new ShrineTradeSets(new List<Item> { ItemDict.STONE },
                new List<Item> { ItemDict.SCRAP_IRON, ItemDict.IRON_ORE, ItemDict.MYTHRIL_ORE, ItemDict.GOLD_ORE },
                new List<Item> { ItemDict.JEWEL_SPIDER },
                AppliedEffects.MINING_III);
            STS_CONCRETE = new ShrineTradeSets(new List<Item> { ItemDict.IRON_CHIP, ItemDict.SCRAP_IRON, ItemDict.IRON_ORE },
                new List<Item> { ItemDict.IRON_CHIP, ItemDict.MYTHRIL_CHIP, ItemDict.GOLD_CHIP, ItemDict.GLASS_SHEET },
                new List<Item> { ItemDict.BOOMBOX },
                AppliedEffects.MINING_III);
            STS_JUNGLE = new ShrineTradeSets(new List<Item> { ItemDict.VANILLA_BEAN, ItemDict.CACAO_BEAN },
                new List<Item> { ItemDict.MAIZE, ItemDict.PINEAPPLE },
                new List<Item> { ItemDict.FIRE_CRYSTAL },
                AppliedEffects.BUG_CATCHING_III);
            STS_VOLCANO = new ShrineTradeSets(new List<Item> { ItemDict.ONYX_EEL, ItemDict.BLACKENED_OCTOPUS },
                new List<Item> { ItemDict.MOLTEN_SQUID, ItemDict.BOXER_LOBSTER },
                new List<Item> { ItemDict.INFERNAL_SHARK },
                AppliedEffects.FISHING_III_LAVA);
            STS_WEAVER = new ShrineTradeSets(new List<Item> { ItemDict.COTTON, ItemDict.FLAX },
                new List<Item> { ItemDict.LINEN_CLOTH, ItemDict.COTTON_CLOTH },
                new List<Item> { ItemDict.WATER_CRYSTAL },
                AppliedEffects.SPEED_III);
            STS_GOLDEN = new ShrineTradeSets(new List<Item> { ItemDict.PINEAPPLE, ItemDict.LUCKY_COIN, ItemDict.MARIGOLD, ItemDict.GOLD_CHIP },
                new List<Item> { ItemDict.GOLD_ORE, ItemDict.GOLD_BAR},
                new List<Item> { ItemDict.SHIMMERING_SALVE },
                AppliedEffects.SPEED_IV_MORNING);
            STS_ARCHAEOLOGIST = new ShrineTradeSets(new List<Item> { ItemDict.FOSSIL_SHARDS, ItemDict.TRILOBITE, ItemDict.OLD_BONE },
                new List<Item> { ItemDict.TRILOBITE },
                new List<Item> { ItemDict.PRIMORDIAL_SHELL },
                AppliedEffects.MINING_IV);
            STS_MYTH = new ShrineTradeSets(new List<Item> { ItemDict.STRIPED_BASS, ItemDict.WHITE_BLOWFISH, ItemDict.CLOUD_FLOUNDER },
                new List<Item> { ItemDict.SKY_PIKE, ItemDict.SEA_TURTLE, ItemDict.STORMBRINGER_KOI },
                new List<Item> { ItemDict.CRYSTAL_KEY },
                AppliedEffects.CHOPPING_IV);
            STS_SHRUBBERY = new ShrineTradeSets(new List<Item> { ItemDict.RASPBERRY, ItemDict.BLUEBELL, ItemDict.BLACKBERRY, ItemDict.ELDERBERRY },
                new List<Item> { ItemDict.BERRY_BUSH_PLANTER },
                new List<Item> { ItemDict.EARTH_CRYSTAL },
                AppliedEffects.FORAGING_IV);
            STS_CRYSTAL = new ShrineTradeSets(new List<Item> { ItemDict.SALT_SHARDS },
                new List<Item> { ItemDict.AQUAMARINE, ItemDict.QUARTZ, ItemDict.TOPAZ, ItemDict.WATER_CRYSTAL, ItemDict.FIRE_CRYSTAL, ItemDict.WIND_CRYSTAL, ItemDict.EARTH_CRYSTAL, ItemDict.GLASS_SHEET, ItemDict.SNOW_CRYSTAL },
                new List<Item> { ItemDict.SKY_ELEMENT, ItemDict.SEA_ELEMENT, ItemDict.PRIMEVAL_ELEMENT, ItemDict.SKY_ELEMENT },
                AppliedEffects.MINING_III);
            STS_ICE = new ShrineTradeSets(new List<Item> { ItemDict.SNOW_CRYSTAL},
                new List<Item> { ItemDict.MINT_EXTRACT, ItemDict.VANILLA_EXTRACT },
                new List<Item> { ItemDict.ICE_NINE },
                AppliedEffects.FORAGING_IV_WINTER);
            STS_LIFE = new ShrineTradeSets(new List<Item> { ItemDict.BAMBOO, ItemDict.SKY_ROSE, ItemDict.SHIITAKE },
                new List<Item> { ItemDict.BROWN_CICADA },
                new List<Item> { ItemDict.EMPRESS_BUTTERFLY },
                AppliedEffects.BUG_CATCHING_IV);
            STS_MINER = new ShrineTradeSets(new List<Item> { ItemDict.COAL, ItemDict.IRON_ORE, ItemDict.IRON_BAR },
                new List<Item> { ItemDict.IRON_ORE, ItemDict.GOLD_ORE, ItemDict.MYTHRIL_ORE, ItemDict.ADAMANTITE_ORE },
                new List<Item> { ItemDict.ADAMANTITE_ORE, ItemDict.ADAMANTITE_BAR },
                AppliedEffects.MINING_IV);
            STS_SKY = new ShrineTradeSets(new List<Item> { ItemDict.WHITE_BLOWFISH, ItemDict.CLOUD_FLOUNDER },
                new List<Item> { ItemDict.STORMBRINGER_KOI, ItemDict.SKY_PIKE },
                new List<Item> { ItemDict.LUNAR_WHALE },
                AppliedEffects.FISHING_IV_CLOUD);
            STS_DEPARTED = new ShrineTradeSets(new List<Item> { ItemDict.INVINCIROID },
                new List<Item> { ItemDict.SOOTHE_CANDLE, ItemDict.SUGAR_CANDLE },
                new List<Item> { ItemDict.SKY_BOTTLE },
                AppliedEffects.SPEED_IV);
            STS_CREATION = new ShrineTradeSets(new List<Item> { ItemDict.UN_DYE },
                new List<Item> { ItemDict.PINK_DYE, ItemDict.RED_DYE, ItemDict.ORANGE_DYE, ItemDict.YELLOW_DYE },
                new List<Item> { ItemDict.HOE, ItemDict.WATERING_CAN, ItemDict.PICKAXE, ItemDict.AXE, ItemDict.FISHING_ROD },
                AppliedEffects.LUCK_VI);

            //add trading sets to shrines
            //add a new graphic for shrines with incense - incense burners
            //add a particle effect as well

            shrineList = new List<ShrineStatus>();
            shrineList.Add(SHRINE_SEASON_SPRING_1 = new ShrineStatus("SHRINE_SEASON_SPRING_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_ANIMALS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"The Farmer's Handbook: Animals\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BLUEBELL, 1), new ShrineStatus.RequiredItem(ItemDict.NETTLES, 1), new ShrineStatus.RequiredItem(ItemDict.CHICKWEED, 1)));
            shrineList.Add(SHRINE_SEASON_SPRING_2 = new ShrineStatus("SHRINE_SEASON_SPRING_2", (player, area) => {
                FLAGS[FLAG_BOOK_BASIC_PATTERNS_SS] = 1;
                UnlockRecipe(BASIC_PATTERNS_SS_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Basic Patterns (S/S)\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new spring & summer clothing!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SPINACH, 1), new ShrineStatus.RequiredItem(ItemDict.POTATO, 1), new ShrineStatus.RequiredItem(ItemDict.CARROT, 1)));
            shrineList.Add(SHRINE_SEASON_SUMMER_1 = new ShrineStatus("SHRINE_SEASON_SUMMER_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_COMPOST] = 1;
                UnlockRecipe(ItemDict.COMPOST_BIN);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"The Farmer's Handbook: Animals\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Compost Bin!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.MARIGOLD, 1), new ShrineStatus.RequiredItem(ItemDict.SUNFLOWER, 1), new ShrineStatus.RequiredItem(ItemDict.LAVENDER, 1)));
            shrineList.Add(SHRINE_SEASON_SUMMER_2 = new ShrineStatus("SHRINE_SEASON_SUMMER_2", (player, area) => {
                FLAGS[FLAG_BOOK_FABULOUS_FARMSTEADS] = 1;
                UnlockRecipe(FABULOUS_FARMSTEADS_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Fabulous Farmsteads\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new farm-themed items!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.TOMATO, 1), new ShrineStatus.RequiredItem(ItemDict.CUCUMBER, 1), new ShrineStatus.RequiredItem(ItemDict.ONION, 1)));
            shrineList.Add(SHRINE_SEASON_AUTUMN_1 = new ShrineStatus("SHRINE_SEASON_AUTUMN_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_SEED_MAKERS] = 1;
                UnlockRecipe(ItemDict.SEED_MAKER);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"The Farmer's Handbook: Seed Makers\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Seed Maker!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.WILD_RICE, 1), new ShrineStatus.RequiredItem(ItemDict.PERSIMMON, 1), new ShrineStatus.RequiredItem(ItemDict.SASSAFRAS, 1)));
            shrineList.Add(SHRINE_SEASON_AUTUMN_2 = new ShrineStatus("SHRINE_SEASON_AUTUMN_2", (player, area) => {
                FLAGS[FLAG_BOOK_BASIC_PATTERNS_FW] = 1;
                UnlockRecipe(BASIC_PATTERNS_FW_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Basic Patterns (F/W)\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new fall & winter clothing!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BEET, 1), new ShrineStatus.RequiredItem(ItemDict.BROCCOLI, 1), new ShrineStatus.RequiredItem(ItemDict.BELLPEPPER, 1)));
            shrineList.Add(SHRINE_SEASON_WINTER_1 = new ShrineStatus("SHRINE_SEASON_WINTER_1", (player, area) => {
                FLAGS[FLAG_BOOK_A_TOUCH_OF_NATURE] = 1;
                UnlockRecipe(A_TOUCH_OF_NATURE_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"A Touch of Nature\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new natural decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.WINTERGREEN, 1), new ShrineStatus.RequiredItem(ItemDict.CHICORY_ROOT, 1), new ShrineStatus.RequiredItem(ItemDict.CHANTERELLE, 1), new ShrineStatus.RequiredItem(ItemDict.SNOWDROP, 1)));
            shrineList.Add(SHRINE_SEASON_WINTER_2 = new ShrineStatus("SHRINE_SEASON_WINTER_2", (player, area) => {
                FLAGS[FLAG_BOOK_CHILLING_CONFECTIONS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Chilling Confections\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SNOW_CRYSTAL, 10)));

            shrineList.Add(SHRINE_CHICKEN_1 = new ShrineStatus("SHRINE_CHICKEN_1", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_CHICKEN, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.BASKET, 1)));
            shrineList.Add(SHRINE_CHICKEN_2 = new ShrineStatus("SHRINE_CHICKEN_2", (player, area) => {
                UnlockRecipe(ItemDict.MAYONNAISE_MAKER);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Mayonaise Maker!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.EGG, 1)));
            shrineList.Add(SHRINE_SHEEP_1 = new ShrineStatus("SHRINE_SHEEP_1", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_SHEEP, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.SHEARS, 1)));
            shrineList.Add(SHRINE_SHEEP_2 = new ShrineStatus("SHRINE_SHEEP_2", (player, area) => {
                UnlockRecipe(ItemDict.LOOM);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Loom!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.WOOL, 1)));
            shrineList.Add(SHRINE_COW_1 = new ShrineStatus("SHRINE_COW_1", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_COW, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.MILKING_PAIL, 1)));
            shrineList.Add(SHRINE_COW_2 = new ShrineStatus("SHRINE_COW_2", (player, area) => {
                UnlockRecipe(ItemDict.DAIRY_CHURN);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Dairy Churn!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.MILK, 1)));

            shrineList.Add(SHRINE_FRIENDSHIP_1 = new ShrineStatus("SHRINE_FRIENDSHIP_1", (player, area) => {
                FLAGS[FLAG_BOOK_DATING_FOR_DUMMIES] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Dating For Dummies\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                UnlockRecipe(ItemDict.BARBER_POLE);
                UnlockRecipe(ItemDict.ENCHANTED_VANITY);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Barber Pole and an Enchanted Vanity!", Color.Green, EntityPlayer.Notification.Length.LONG));

            },
                new ShrineStatus.RequiredItem(ItemDict.FRIENDSHIP_CERT, 1)));
            shrineList.Add(SHRINE_FRIENDSHIP_2 = new ShrineStatus("SHRINE_FRIENDSHIP_2", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.PURE_FEATHER, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.LOVER_CERT, 1)));
            shrineList.Add(SHRINE_LIBRARIAN_1 = new ShrineStatus("SHRINE_LIBRARIAN_1", (player, area) => {
                FLAGS[FLAG_BOOK_COUNTRY_PATTERNS] = 1;
                UnlockRecipe(COUNTRY_PATTERNS_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Country Patterns\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new rustic clothing!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.READER_CERT, 1)));
            shrineList.Add(SHRINE_LIBRARIAN_2 = new ShrineStatus("SHRINE_LIBRARIAN_2", (player, area) => {

                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_SHEEP, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.LIBRARY_CERT, 1)));

            shrineList.Add(SHRINE_PAINTER_1 = new ShrineStatus("SHRINE_PAINTER_1", (player, area) => {
                UnlockRecipe(ItemDict.PAINTERS_PRESS);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Painter's Press!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SEA_URCHIN, 1), new ShrineStatus.RequiredItem(ItemDict.BLUEBELL, 1), new ShrineStatus.RequiredItem(ItemDict.CRIMSON_CORAL, 1)));
            shrineList.Add(SHRINE_PAINTER_2 = new ShrineStatus("SHRINE_PAINTER_2", (player, area) => {
                FLAGS[FLAG_BOOK_A_STUDY_OF_COLOR_IN_NATURE] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"A Study of Color in Nature\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.RED_DYE, 1), new ShrineStatus.RequiredItem(ItemDict.ORANGE_DYE, 1), new ShrineStatus.RequiredItem(ItemDict.YELLOW_DYE, 1), new ShrineStatus.RequiredItem(ItemDict.BLUE_DYE, 1), new ShrineStatus.RequiredItem(ItemDict.NAVY_DYE, 1)));

            shrineList.Add(SHRINE_CHEF_1 = new ShrineStatus("SHRINE_CHEF_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_1] = 1;
                UnlockRecipe(ItemDict.CHEFS_TABLE);
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"The Forager's Cookbook Vol I\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Chef's Table!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.NETTLES, 1), new ShrineStatus.RequiredItem(ItemDict.SPICY_LEAF, 1), new ShrineStatus.RequiredItem(ItemDict.MOREL, 1), new ShrineStatus.RequiredItem(ItemDict.CHICKWEED, 1)));
            shrineList.Add(SHRINE_CHEF_2 = new ShrineStatus("SHRINE_CHEF_2", (player, area) => {
                FLAGS[FLAG_BOOK_COOKBOOK_SPRINGS_GIFT] = 1;
                UnlockRecipe(ItemDict.CLAY_OVEN);
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Cookbook: Spring's Gift\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Clay Oven!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.REJUVENATION_TEA, 1), new ShrineStatus.RequiredItem(ItemDict.CHICKWEED_BLEND, 1)));

            shrineList.Add(SHRINE_BLACKSMITH_1 = new ShrineStatus("SHRINE_BLACKSMITH_1", (player, area) => {
                FLAGS[FLAG_BOOK_STRAIGHTFORWARD_SMELTING] = 1;
                UnlockRecipe(ItemDict.FURNACE);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Straightforward Smelting\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Furnace!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SCRAP_IRON, 1), new ShrineStatus.RequiredItem(ItemDict.IRON_ORE, 1), new ShrineStatus.RequiredItem(ItemDict.COAL, 1)));
            shrineList.Add(SHRINE_BLACKSMITH_2 = new ShrineStatus("SHRINE_BLACKSMITH_2", (player, area) => {
                FLAGS[FLAG_BOOK_MASTERFUL_METALWORKING] = 1;
                UnlockRecipe(ItemDict.ANVIL);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Masterful Metalworking\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make an Anvil!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.IRON_BAR, 3)));

            shrineList.Add(SHRINE_BUILDER_1 = new ShrineStatus("SHRINE_BUILDER_1", (player, area) => {
                FLAGS[FLAG_BOOK_WORKING_WITH_YOUR_WORKBENCH] = 1;
                UnlockRecipe(ItemDict.WORKBENCH);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Working With Your Workbench\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Workbench!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.WOOD, 10)));
            shrineList.Add(SHRINE_BUILDER_2 = new ShrineStatus("SHRINE_BUILDER_2", (player, area) => {
                FLAGS[FLAG_BOOK_SIMPLY_WOODWORKING] = 1;
                UnlockRecipe(SIMPLY_WOODWORKING_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Simply Woodworking\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new wooden furniture!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BOARD, 3), new ShrineStatus.RequiredItem(ItemDict.BRICKS, 3)));
            shrineList.Add(SHRINE_MERCHANT_1 = new ShrineStatus("SHRINE_MERCHANT_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_TRADERS_ATLAS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"The Trader's Atlas\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SHIPMENT_CERT, 1)));
            shrineList.Add(SHRINE_MERCHANT_2 = new ShrineStatus("SHRINE_MERCHANT_2", (player, area) => {
                //TODO TOTEM OF THE ROOSTER
            },
                new ShrineStatus.RequiredItem(ItemDict.BARON_CERT, 1)));
            shrineList.Add(SHRINE_SEABIRD_1 = new ShrineStatus("SHRINE_SEABIRD_1", (player, area) => {
                FLAGS[FLAG_BOOK_GREAT_GLASSBLOWING] = 1;
                UnlockRecipe(ItemDict.GLASSBLOWER);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Great Glassblowing\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Glassblower!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.MOUNTAIN_BREAD, 1), new ShrineStatus.RequiredItem(ItemDict.OYSTER, 1)));
            shrineList.Add(SHRINE_SEABIRD_2 = new ShrineStatus("SHRINE_SEABIRD_2", (player, area) => {
                FLAGS[FLAG_BOOK_AN_ARTISTS_REFLECTION] = 1;
                UnlockRecipe(AN_ARTISTS_REFLECTION_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"An Artist's Reflection\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new reflective decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.WHITE_FEATHER, 1), new ShrineStatus.RequiredItem(ItemDict.SWORDFISH, 1), new ShrineStatus.RequiredItem(ItemDict.GLASS_SHEET, 1)));
            shrineList.Add(SHRINE_OLD_MAN_OF_THE_SEA_1 = new ShrineStatus("SHRINE_OLD_MAN_OF_THE_SEA_1", (player, area) => {
                FLAGS[FLAG_BOOK_FISHING_THROUGH_THE_SEASONS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Fishing Through the Seasons\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SEAWEED, 1), new ShrineStatus.RequiredItem(ItemDict.HERRING, 1), new ShrineStatus.RequiredItem(ItemDict.SARDINE, 1)));
            shrineList.Add(SHRINE_OLD_MAN_OF_THE_SEA_2 = new ShrineStatus("SHRINE_OLD_MAN_OF_THE_SEA_2", (player, area) => {
                FLAGS[FLAG_BOOK_ANCIENT_MARINERS_SCROLL] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Ancient Mariner's Scroll\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.MACKEREL, 1), new ShrineStatus.RequiredItem(ItemDict.TUNA, 1), new ShrineStatus.RequiredItem(ItemDict.PUFFERFISH, 1), new ShrineStatus.RequiredItem(ItemDict.SEA_TURTLE, 1)));
            shrineList.Add(SHRINE_CELEBRATION_1 = new ShrineStatus("SHRINE_CELEBRATION_1", (player, area) => {
                //TODO TOTEM OF THE DOG
            },
                new ShrineStatus.RequiredItem(ItemDict.FESTIVAL_CERT, 1)));
            shrineList.Add(SHRINE_CELEBRATION_2 = new ShrineStatus("SHRINE_CELEBRATION_2", (player, area) => {
                FLAGS[FLAG_BOOK_COOKBOOK_FOUR_SEASONS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Cookbook: Four Seasons\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.COMMUNITY_CERT, 1)));

            shrineList.Add(SHRINE_ARBORIST_1 = new ShrineStatus("SHRINE_ARBORIST_1", (player, area) => {
                FLAGS[FLAG_BOOK_JUICY_JAMS_PRECIOUS_PRESERVES_AND_WORTHWHILE_WINES] = 1;
                UnlockRecipe(ItemDict.KEG);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Juicy Jams, Precious Preserves, & Worthwhile Wines\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make Kegs!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.CHERRY, 1)));
            shrineList.Add(SHRINE_ARBORIST_2 = new ShrineStatus("SHRINE_ARBORIST_2", (player, area) => {
                UnlockRecipe(ItemDict.VIVARIUM);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Vivarium!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.APPLE, 1), new ShrineStatus.RequiredItem(ItemDict.ORANGE, 1), new ShrineStatus.RequiredItem(ItemDict.LEMON, 1)));
            shrineList.Add(SHRINE_FRAGRANT_1 = new ShrineStatus("SHRINE_FRAGRANT_1", (player, area) => {
                FLAGS[FLAG_BOOK_ARTISTIC_SCENT] = 1;
                UnlockRecipe(ItemDict.PERFUMERY);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Artistic Scent\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Perfumery!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BLUEBELL, 1), new ShrineStatus.RequiredItem(ItemDict.LAVENDER, 1), new ShrineStatus.RequiredItem(ItemDict.RASPBERRY, 1)));
            shrineList.Add(SHRINE_FRAGRANT_2 = new ShrineStatus("SHRINE_FRAGRANT_2", (player, area) => {
                UnlockRecipe(ItemDict.FLOWERBED);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make Flowerbeds!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.FLORAL_PERFUME, 1), new ShrineStatus.RequiredItem(ItemDict.SWEET_BREEZE, 1), new ShrineStatus.RequiredItem(ItemDict.SUMMERS_GIFT, 1)));
            shrineList.Add(SHRINE_INSECT_1 = new ShrineStatus("SHRINE_INSECT_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_ADVANCED] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"The Farmer's Handbook: Intermediate\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BANDED_DRAGONFLY, 1), new ShrineStatus.RequiredItem(ItemDict.RICE_GRASSHOPPER, 1), new ShrineStatus.RequiredItem(ItemDict.YELLOW_BUTTERFLY, 1), new ShrineStatus.RequiredItem(ItemDict.FIREFLY, 1)));
            shrineList.Add(SHRINE_INSECT_2 = new ShrineStatus("SHRINE_INSECT_2", (player, area) => {
                FLAGS[FLAG_BOOK_NATURAL_CRAFTS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Natural Crafts\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.LANTERN_MOTH, 1), new ShrineStatus.RequiredItem(ItemDict.PINK_LADYBUG, 1), new ShrineStatus.RequiredItem(ItemDict.SNAIL, 1), new ShrineStatus.RequiredItem(ItemDict.EARTHWORM, 1)));
            shrineList.Add(SHRINE_MOUNTAIN_1 = new ShrineStatus("SHRINE_MOUNTAIN_1", (player, area) => {
                FLAGS[FLAG_BOOK_HOMEMADE_ACCESSORIES] = 1;
                UnlockRecipe(ItemDict.JEWELERS_BENCH);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Homemade Accessories\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Jeweler's Bench!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.MOREL, 1), new ShrineStatus.RequiredItem(ItemDict.MOUNTAIN_WHEAT, 1), new ShrineStatus.RequiredItem(ItemDict.SPICY_LEAF, 1)));
            shrineList.Add(SHRINE_MOUNTAIN_2 = new ShrineStatus("SHRINE_MOUNTAIN_2", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_PIG, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.WIND_CRYSTAL, 1)));
            shrineList.Add(SHRINE_BOAR_1 = new ShrineStatus("SHRINE_BOAR_1", (player, area) => {
                FLAGS[FLAG_BOOK_SUPPER_WITH_GRANDMA_NINE] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Supper With Grandma Nine\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
              new ShrineStatus.RequiredItem(ItemDict.BOAR_HIDE, 1), new ShrineStatus.RequiredItem(ItemDict.WILD_MEAT, 1)));
            shrineList.Add(SHRINE_BOAR_2 = new ShrineStatus("SHRINE_BOAR_2", (player, area) => {
                FLAGS[FLAG_BOOK_BREAKFAST_WITH_GRANDMA_NINE] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Breakfast With Grandma Nine\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SAVORY_ROAST, 1), new ShrineStatus.RequiredItem(ItemDict.BOAR_STEW, 1)));
            shrineList.Add(SHRINE_BEE_1 = new ShrineStatus("SHRINE_BEE_1", (player, area) => {
                FLAGS[FLAG_BOOK_BEEKEEPERS_MANUAL] = 1;
                UnlockRecipe(ItemDict.BEEHIVE);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Beekeeper's Manual\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make Beehives!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
               new ShrineStatus.RequiredItem(ItemDict.HONEY_BEE, 3), new ShrineStatus.RequiredItem(ItemDict.HONEYCOMB, 1)));
            shrineList.Add(SHRINE_BEE_2 = new ShrineStatus("SHRINE_BEE_2", (player, area) => {
                FLAGS[FLAG_BOOK_BEEMASTERS_MANUAL] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Beemaster's Manual\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.WILD_HONEY, 3), new ShrineStatus.RequiredItem(ItemDict.ROYAL_JELLY, 1)));
            shrineList.Add(SHRINE_BIRD_1 = new ShrineStatus("SHRINE_BIRD_1", (player, area) => {
                FLAGS[FLAG_BOOK_BIRDWATCHERS_ISSUE_I] = 1;
                UnlockRecipe(ItemDict.BIRDHOUSE);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Birdwatchers: Issue I\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make Birdhouses!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
              new ShrineStatus.RequiredItem(ItemDict.BIRDS_NEST, 1)));
            shrineList.Add(SHRINE_BIRD_2 = new ShrineStatus("SHRINE_BIRD_2", (player, area) => {
                FLAGS[FLAG_BOOK_BIRDWATCHERS_ISSUE_II] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Birdwatchers: Issue II\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BLACK_FEATHER, 1), new ShrineStatus.RequiredItem(ItemDict.BLUE_FEATHER, 1)));
            shrineList.Add(SHRINE_LABOURER_1 = new ShrineStatus("SHRINE_LABOURER_1", (player, area) => {
                FLAGS[FLAG_BOOK_COOKBOOK_SUMMERS_BOUNTY] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Cookbook: Summer's Bounty\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
               new ShrineStatus.RequiredItem(ItemDict.WOOD, 10), new ShrineStatus.RequiredItem(ItemDict.MOSSY_BARK, 3)));
            shrineList.Add(SHRINE_LABOURER_2 = new ShrineStatus("SHRINE_LABOURER_2", (player, area) => {
                FLAGS[FLAG_BOOK_COOKBOOK_AUTUMNS_HARVEST] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Cookbook: Autumn's Harvest\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BAMBOO, 5), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_LEAF, 1)));
            shrineList.Add(SHRINE_IRON_1 = new ShrineStatus("SHRINE_IRON_1", (player, area) => {
                FLAGS[FLAG_BOOK_ESSENTIAL_ENGINEERING] = 1;
                UnlockRecipe(ESSENTIAL_ENGINEERING_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Essential Engineering\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new technological decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
               new ShrineStatus.RequiredItem(ItemDict.SCRAP_IRON, 10)));
            shrineList.Add(SHRINE_IRON_2 = new ShrineStatus("SHRINE_IRON_2", (player, area) => {
                FLAGS[FLAG_BOOK_COMPRESSOR_USER_MANUAL] = 1;
                UnlockRecipe(ItemDict.COMPRESSOR);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Compressor User Manual\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Compressor!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SCRAP_IRON, 99)));
            shrineList.Add(SHRINE_POND_1 = new ShrineStatus("SHRINE_POND_1", (player, area) => {
                //TOTEM OF THE CAT
            },
               new ShrineStatus.RequiredItem(ItemDict.BLUEGILL, 1), new ShrineStatus.RequiredItem(ItemDict.CARP, 1), new ShrineStatus.RequiredItem(ItemDict.LAKE_TROUT, 1)));
            shrineList.Add(SHRINE_POND_2 = new ShrineStatus("SHRINE_POND_2", (player, area) => {
                FLAGS[FLAG_BOOK_SOUPER_SOUPS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the ookbook: \"Souper Soups\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SALMON, 1), new ShrineStatus.RequiredItem(ItemDict.CATFISH, 1)));
            shrineList.Add(SHRINE_ALCHEMIST_1 = new ShrineStatus("SHRINE_ALCHEMIST_1", (player, area) => {
                FLAGS[FLAG_BOOK_DUSTY_TOME] = 1;
                UnlockRecipe(ItemDict.ALCHEMY_CAULDRON);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Dusty Tome\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make an Alchemy Cauldron!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
              new ShrineStatus.RequiredItem(ItemDict.BEESWAX, 1), new ShrineStatus.RequiredItem(ItemDict.RASPBERRY, 3)));
            shrineList.Add(SHRINE_ALCHEMIST_2 = new ShrineStatus("SHRINE_ALCHEMIST_2", (player, area) => {
                FLAGS[FLAG_BOOK_INTENSE_INCENSE] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Intense Incense\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BLACK_CANDLE, 1), new ShrineStatus.RequiredItem(ItemDict.MOSS_BOTTLE, 1)));
            shrineList.Add(SHRINE_POTTER_1 = new ShrineStatus("SHRINE_POTTER_1", (player, area) => {
                FLAGS[FLAG_BOOK_POTTERY_FOR_FUN_AND_PROFIT] = 1;
                UnlockRecipe(ItemDict.POTTERY_WHEEL);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Pottery for Fun and Profit\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Pottery Wheel!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
              new ShrineStatus.RequiredItem(ItemDict.CLAY, 3)));
            shrineList.Add(SHRINE_POTTER_2 = new ShrineStatus("SHRINE_POTTER_2", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_COW, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.POTTERY_MUG, 1), new ShrineStatus.RequiredItem(ItemDict.POTTERY_JAR, 1), new ShrineStatus.RequiredItem(ItemDict.POTTERY_PLATE, 1)));

            shrineList.Add(SHRINE_BAT_1 = new ShrineStatus("SHRINE_BAT_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_2] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"The Forager's Cookbook Vol II\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
               new ShrineStatus.RequiredItem(ItemDict.GUANO, 5), new ShrineStatus.RequiredItem(ItemDict.BAT_WING, 1)));
            shrineList.Add(SHRINE_BAT_2 = new ShrineStatus("SHRINE_BAT_2", (player, area) => {
                //TOTEM OF THE CAT
            },
                new ShrineStatus.RequiredItem(ItemDict.GUANO, 25), new ShrineStatus.RequiredItem(ItemDict.ALBINO_WING, 1)));
            shrineList.Add(SHRINE_GROUNDWATER_1 = new ShrineStatus("SHRINE_GROUNDWATER_1", (player, area) => {
                UnlockRecipe(ItemDict.AQUARIUM);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make an Aquarium!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
               new ShrineStatus.RequiredItem(ItemDict.CAVEFISH, 3), new ShrineStatus.RequiredItem(ItemDict.SALT_SHARDS, 3)));
            shrineList.Add(SHRINE_GROUNDWATER_2 = new ShrineStatus("SHRINE_GROUNDWATER_2", (player, area) => {
                FLAGS[FLAG_BOOK_ELEMENTAL_MYSTICA] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Elemental Mystica\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.CAVERN_TETRA, 3), new ShrineStatus.RequiredItem(ItemDict.AQUAMARINE, 1)));
            shrineList.Add(SHRINE_SHINING_1 = new ShrineStatus("SHRINE_SHINING_1", (player, area) => {
                FLAGS[FLAG_BOOK_JEWELERS_HANDBOOK] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Jeweler's Handbook\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
               new ShrineStatus.RequiredItem(ItemDict.QUARTZ, 1), new ShrineStatus.RequiredItem(ItemDict.AMETHYST, 1), new ShrineStatus.RequiredItem(ItemDict.TOPAZ, 1)));
            shrineList.Add(SHRINE_SHINING_2 = new ShrineStatus("SHRINE_SHINING_2", (player, area) => {
                UnlockRecipe(ItemDict.GEMSTONE_REPLICATOR);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Gemstone Replicator!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.DIAMOND, 1), new ShrineStatus.RequiredItem(ItemDict.RUBY, 1), new ShrineStatus.RequiredItem(ItemDict.EMERALD, 1), new ShrineStatus.RequiredItem(ItemDict.SAPPHIRE, 1)));
            shrineList.Add(SHRINE_MUSHROOM_1 = new ShrineStatus("SHRINE_MUSHROOM_1", (player, area) => {
                UnlockRecipe(ItemDict.MUSHBOX);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Mushbox!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
             new ShrineStatus.RequiredItem(ItemDict.CAVE_FUNGI, 3), new ShrineStatus.RequiredItem(ItemDict.OYSTER_MUSHROOM, 3)));
            shrineList.Add(SHRINE_MUSHROOM_2 = new ShrineStatus("SHRINE_MUSHROOM_2", (player, area) => {
                //TOTEM OF THE DOG
            },
                new ShrineStatus.RequiredItem(ItemDict.TRUFFLE, 1), new ShrineStatus.RequiredItem(ItemDict.SHIITAKE, 1)));
            shrineList.Add(SHRINE_CAVER_1 = new ShrineStatus("SHRINE_CAVER_1", (player, area) => {
                FLAGS[FLAG_BOOK_CRAVING_STONECARVING] = 1;
                UnlockRecipe(CRAVING_STONECARVING_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Craving Stonecarving\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new carved decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.STONE, 50)));
            shrineList.Add(SHRINE_CAVER_2 = new ShrineStatus("SHRINE_CAVER_2", (player, area) => {
                FLAGS[FLAG_BOOK_MUSTY_TOME] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Musty Tome\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
               new ShrineStatus.RequiredItem(ItemDict.GOLD_ORE, 5), new ShrineStatus.RequiredItem(ItemDict.MYTHRIL_ORE, 5)));
            shrineList.Add(SHRINE_CONCRETE_1 = new ShrineStatus("SHRINE_CONCRETE_1", (player, area) => {
                FLAGS[FLAG_BOOK_URBAN_DESIGN_BIBLE] = 1;
                UnlockRecipe(URBAN_DESIGN_BIBLE_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Urban Design Bible\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make urban decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BLACK_DYE, 3), new ShrineStatus.RequiredItem(ItemDict.COAL, 5)));
            shrineList.Add(SHRINE_CONCRETE_2 = new ShrineStatus("SHRINE_CONCRETE_2", (player, area) => {
                FLAGS[FLAG_BOOK_URBAN_PATTERNS] = 1;
                UnlockRecipe(URBAN_PATTERNS_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Urban Patterns\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new city clothing!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.BOOMBOX, 1), new ShrineStatus.RequiredItem(ItemDict.TRAFFIC_CONE, 1)));

            shrineList.Add(SHRINE_JUNGLE_1 = new ShrineStatus("SHRINE_JUNGLE_1", (player, area) => {
                UnlockRecipe(ItemDict.TERRARIUM);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Terrarium!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.COCONUT, 5), new ShrineStatus.RequiredItem(ItemDict.MAIZE, 3), new ShrineStatus.RequiredItem(ItemDict.PINEAPPLE, 3)));
            shrineList.Add(SHRINE_JUNGLE_2 = new ShrineStatus("SHRINE_JUNGLE_2", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_COW, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.FIRE_CRYSTAL, 1)));
            shrineList.Add(SHRINE_VOLCANO_1 = new ShrineStatus("SHRINE_VOLCANO_1", (player, area) => {
                FLAGS[FLAG_BOOK_FISH_BEYOND] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Fish Beyond\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.ONYX_EEL, 1), new ShrineStatus.RequiredItem(ItemDict.BLACKENED_OCTOPUS, 1), new ShrineStatus.RequiredItem(ItemDict.MOLTEN_SQUID, 1)));
            shrineList.Add(SHRINE_VOLCANO_2 = new ShrineStatus("SHRINE_VOLCANO_2", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_PIG, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.INFERNAL_SHARK, 1)));
            shrineList.Add(SHRINE_WEAVER_1 = new ShrineStatus("SHRINE_WEAVER_1", (player, area) => {
                FLAGS[FLAG_BOOK_TROPICAL_PATTERNS] = 1;
                UnlockRecipe(TROPICAL_PATTERNS_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Tropical Patterns\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new tropical clothing!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.LONG_SLEEVE_TEE, 1), new ShrineStatus.RequiredItem(ItemDict.BUTTON_DOWN, 1), new ShrineStatus.RequiredItem(ItemDict.STRIPED_SHIRT, 1)));
            shrineList.Add(SHRINE_WEAVER_2 = new ShrineStatus("SHRINE_WEAVER_2", (player, area) => {
                FLAGS[FLAG_BOOK_COSTUME_PATTERNS] = 1;
                UnlockRecipe(COSTUME_PATTERNS_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Costume Patterns\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new costumes!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.APRON, 1), new ShrineStatus.RequiredItem(ItemDict.OVERALLS, 1), new ShrineStatus.RequiredItem(ItemDict.BATHROBE, 1)));
            shrineList.Add(SHRINE_GOLDEN_1 = new ShrineStatus("SHRINE_GOLDEN_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_MASTERY] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"The Farmer's Handbook: Mastery\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.GOLD_ORE, 10)));
            shrineList.Add(SHRINE_GOLDEN_2 = new ShrineStatus("SHRINE_GOLDEN_2", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_SHEEP, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_EGG, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_WOOL, 1)));
            shrineList.Add(SHRINE_LEGENDARY_1 = new ShrineStatus("SHRINE_LEGENDARY_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FARMERS_HANDBOOK_MYTHS_AND_LEGENDS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"The Farmer's Handbook: Myths & Legends\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_SPINACH, 1)));
            shrineList.Add(SHRINE_LEGENDARY_2 = new ShrineStatus("SHRINE_LEGENDARY_2", (player, area) => {
                UnlockRecipe(ItemDict.TOTEM_OF_THE_SHEEP);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Totem of the Sheep!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_POTATO, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_STRAWBERRY, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_CARROT, 1),
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_CACTUS, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_ONION, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_COTTON, 1),
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_CUCUMBER, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_EGGPLANT, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_TOMATO, 1),
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_WATERMELON_SLICE, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_BEET, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_FLAX, 1),
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_BELLPEPPER, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_BROCCOLI, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_CABBAGE, 1),
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_PUMPKIN, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_CHERRY, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_OLIVE, 1),
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_COCONUT, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_ORANGE, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_BANANA, 1),
                new ShrineStatus.RequiredItem(ItemDict.GOLDEN_APPLE, 1), new ShrineStatus.RequiredItem(ItemDict.GOLDEN_LEMON, 1)));
            shrineList.Add(SHRINE_ARCHAEOLOGIST_1 = new ShrineStatus("SHRINE_EXPLORATION_1", (player, area) => {
                UnlockRecipe(ItemDict.SYNTHESIZER);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Splicer!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.TRILOBITE, 5))); 
            shrineList.Add(SHRINE_ARCHAEOLOGIST_2 = new ShrineStatus("SHRINE_EXPLORATION_2", (player, area) => {
                UnlockRecipe(ItemDict.TOTEM_OF_THE_PIG);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Totem of the Pig!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.TRILOBITE, 99), new ShrineStatus.RequiredItem(ItemDict.FOSSIL_SHARDS, 99), new ShrineStatus.RequiredItem(ItemDict.OLD_BONE, 10), new ShrineStatus.RequiredItem(ItemDict.PRIMORDIAL_SHELL, 1))); 
            shrineList.Add(SHRINE_MYTH_1 = new ShrineStatus("SHRINE_MYTH_1", (player, area) => {
                UnlockRecipe(ItemDict.SOULCHEST);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Soulchest!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.QUEEN_AROWANA, 1)));
            shrineList.Add(SHRINE_MYTH_2 = new ShrineStatus("SHRINE_MYTH_2", (player, area) => {
                FLAGS[FLAG_BOOK_BASIC_PATTERNS_FW] = 1;
                UnlockRecipe(BASIC_PATTERNS_FW_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Totem of the Chicken!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.GREAT_WHITE_SHARK, 1), new ShrineStatus.RequiredItem(ItemDict.EMPEROR_SALMON, 1), new ShrineStatus.RequiredItem(ItemDict.DARK_ANGLER, 1)));
            shrineList.Add(SHRINE_SHRUBBERY_1 = new ShrineStatus("SHRINE_SHRUBBERY_1", (player, area) => {
                FLAGS[FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_3] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"The Forager's Cookbook Vol III\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.RASPBERRY, 10)));
            shrineList.Add(SHRINE_SHRUBBERY_2 = new ShrineStatus("SHRINE_SHRUBBERY_2", (player, area) => {
                area.AddEntity(new EntityItem(ItemDict.TOTEM_OF_THE_CHICKEN, new Vector2(player.GetAdjustedPosition().X, player.GetAdjustedPosition().Y - 10)));
            },
                new ShrineStatus.RequiredItem(ItemDict.BLUEBELL, 10), new ShrineStatus.RequiredItem(ItemDict.ELDERBERRY, 25), new ShrineStatus.RequiredItem(ItemDict.BLACKBERRY, 25)));

            shrineList.Add(SHRINE_CRYSTAL_1 = new ShrineStatus("SHRINE_CRYSTAL_1", (player, area) => {
                FLAGS[FLAG_BOOK_CHANNELING_THE_ELEMENTS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Channeling the Elements\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.EARTH_CRYSTAL, 1), new ShrineStatus.RequiredItem(ItemDict.WATER_CRYSTAL, 1), new ShrineStatus.RequiredItem(ItemDict.WIND_CRYSTAL, 1), new ShrineStatus.RequiredItem(ItemDict.FIRE_CRYSTAL, 1)));
            shrineList.Add(SHRINE_CRYSTAL_2 = new ShrineStatus("SHRINE_CRYSTAL_2", (player, area) => {
                FLAGS[FLAG_BOOK_UNIQUE_FLAVORS] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Unique Flavors\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.LAND_ELEMENT, 1), new ShrineStatus.RequiredItem(ItemDict.SEA_ELEMENT, 1), new ShrineStatus.RequiredItem(ItemDict.SKY_ELEMENT, 1), new ShrineStatus.RequiredItem(ItemDict.PRIMEVAL_ELEMENT, 1)));
            shrineList.Add(SHRINE_ICE_1 = new ShrineStatus("SHRINE_ICE_1", (player, area) => {
                FLAGS[FLAG_BOOK_ICE_A_TREATISE] = 1;
                UnlockRecipe(ICE_A_TREATISE_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Ice: A Treatise\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new icy decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SNOW_CRYSTAL, 99)));
            shrineList.Add(SHRINE_ICE_2 = new ShrineStatus("SHRINE_ICE_2", (player, area) => {
                FLAGS[FLAG_BOOK_FASHION_PRIMER] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Fashion Primer\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.ICE_NINE, 1)));
            shrineList.Add(SHRINE_LIFE_1 = new ShrineStatus("SHRINE_LIFE_1", (player, area) => {
                FLAGS[FLAG_BOOK_MUSIC_AT_HOME] = 1;
                UnlockRecipe(MUSIC_AT_HOME_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Music At Home\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make new musical decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.STINGER_HORNET, 1), new ShrineStatus.RequiredItem(ItemDict.BROWN_CICADA, 1), new ShrineStatus.RequiredItem(ItemDict.SOLDIER_ANT, 1), new ShrineStatus.RequiredItem(ItemDict.POLLEN_PUFF, 1)));
            shrineList.Add(SHRINE_LIFE_2 = new ShrineStatus("SHRINE_LIFE_2", (player, area) => {
                UnlockRecipe(ItemDict.TOTEM_OF_THE_COW);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Totem of the Cow!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.EMPRESS_BUTTERFLY, 1), new ShrineStatus.RequiredItem(ItemDict.JEWEL_SPIDER, 1), new ShrineStatus.RequiredItem(ItemDict.STAG_BEETLE, 1), new ShrineStatus.RequiredItem(ItemDict.QUEENS_STINGER, 1)));
            shrineList.Add(SHRINE_MINER_1 = new ShrineStatus("SHRINE_MINER_1", (player, area) => {
                FLAGS[FLAG_BOOK_PLAYGROUND_PREP] = 1;
                UnlockRecipe(PLAYGROUND_PREP_UNLOCKS);
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Playground Prep\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
                player.AddNotification(new EntityPlayer.Notification("I learned how to make playful decorations!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.MYTHRIL_BAR, 5)));
            shrineList.Add(SHRINE_MINER_2 = new ShrineStatus("SHRINE_MINER_2", (player, area) => {
                FLAGS[FLAG_BOOK_DECOR_PRIMER] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Decor Primer\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.ADAMANTITE_BAR, 1)));
            shrineList.Add(SHRINE_SKY_1 = new ShrineStatus("SHRINE_SKY_1", (player, area) => {
                FLAGS[FLAG_BOOK_EASTERN_CUISINE] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the cookbook: \"Eastern Cuisine\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.WHITE_BLOWFISH, 1), new ShrineStatus.RequiredItem(ItemDict.CLOUD_FLOUNDER, 1), new ShrineStatus.RequiredItem(ItemDict.SKY_PIKE, 1)));
            shrineList.Add(SHRINE_SKY_2 = new ShrineStatus("SHRINE_SKY_2", (player, area) => {
                UnlockRecipe(ItemDict.SKY_STATUE);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Sky Statue!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.STORMBRINGER_KOI, 1), new ShrineStatus.RequiredItem(ItemDict.LUNAR_WHALE, 1)));
            shrineList.Add(SHRINE_DEPARTED_1 = new ShrineStatus("SHRINE_DEPARTED_1", (player, area) => {
                FLAGS[FLAG_BOOK_FOCI_OF_THE_SHAMAN] = 1;
                player.AddNotification(new EntityPlayer.Notification("I obtained the book: \"Foci of the Shaman\"!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_SPINACH, 1)));
            shrineList.Add(SHRINE_DEPARTED_2 = new ShrineStatus("SHRINE_DEPARTED_2", (player, area) => {
                //TODO CAT, DOG, ROOSTER TOTEM UNLOCK!
            },
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_POTATO, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_STRAWBERRY, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_CARROT, 1),
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_CACTUS, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_ONION, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_COTTON, 1),
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_CUCUMBER, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_EGGPLANT, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_TOMATO, 1),
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_WATERMELON_SLICE, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_BEET, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_FLAX, 1),
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_BELLPEPPER, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_BROCCOLI, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_CABBAGE, 1),
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_PUMPKIN, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_CHERRY, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_OLIVE, 1),
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_COCONUT, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_ORANGE, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_BANANA, 1),
                new ShrineStatus.RequiredItem(ItemDict.PHANTOM_APPLE, 1), new ShrineStatus.RequiredItem(ItemDict.PHANTOM_LEMON, 1)));

            shrineList.Add(SHRINE_CREATION_1 = new ShrineStatus("SHRINE_CREATION_1", (player, area) => {
                //TODO CREATE A VILLAGER... flip a flag for this l8r
            },
                new ShrineStatus.RequiredItem(ItemDict.HOE, 1), new ShrineStatus.RequiredItem(ItemDict.WATERING_CAN, 1)));
            shrineList.Add(SHRINE_CREATION_2 = new ShrineStatus("SHRINE_CREATION_2", (player, area) => {
                UnlockRecipe(ItemDict.DRACONIC_PILLAR);
                player.AddNotification(new EntityPlayer.Notification("I learned how to make a Draconic Pillar!", Color.Green, EntityPlayer.Notification.Length.LONG));
            },
                new ShrineStatus.RequiredItem(ItemDict.SPIRIT_CERT, 1)));

            //machines

            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CHEST, 1), new ItemStack(ItemDict.WOOD, 30)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BASKET, 1), new ItemStack(ItemDict.BAMBOO, 6)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SHEARS, 1), new ItemStack(ItemDict.IRON_BAR, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MILKING_PAIL, 1), new ItemStack(ItemDict.IRON_BAR, 2)));

            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WORKBENCH, 1), new ItemStack(ItemDict.WOOD, 50), new ItemStack(ItemDict.IRON_BAR, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BARBER_POLE, 1), new ItemStack(ItemDict.HARDWOOD, 3), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.RED_DYE, 2), new ItemStack(ItemDict.BLUE_DYE, 2)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ENCHANTED_VANITY, 1), new ItemStack(ItemDict.HARDWOOD, 5), new ItemStack(ItemDict.GLASS_SHEET, 1), new ItemStack(ItemDict.GOLD_BAR, 1), new ItemStack(ItemDict.FAIRY_DUST, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.COMPOST_BIN, 1), new ItemStack(ItemDict.WOOD, 20), new ItemStack(ItemDict.IRON_BAR, 1), new ItemStack(ItemDict.WEEDS, 10)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SEED_MAKER, 1), new ItemStack(ItemDict.BOARD, 20), new ItemStack(ItemDict.GEARS, 3), new ItemStack(ItemDict.MYTHRIL_BAR, 2)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MAYONNAISE_MAKER, 1), new ItemStack(ItemDict.BOARD, 10), new ItemStack(ItemDict.GEARS, 2)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LOOM, 1), new ItemStack(ItemDict.PLANK, 8), new ItemStack(ItemDict.WOOL, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DAIRY_CHURN, 1), new ItemStack(ItemDict.WOOD, 20), new ItemStack(ItemDict.HARDWOOD, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.KEG, 1), new ItemStack(ItemDict.BOARD, 60)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CHEFS_TABLE, 1), new ItemStack(ItemDict.WOOD, 15), new ItemStack(ItemDict.IRON_BAR, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CLAY_OVEN, 1), new ItemStack(ItemDict.STONE, 30), new ItemStack(ItemDict.CLAY, 15)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DRYING_RACK, 1), new ItemStack(ItemDict.BOARD, 25), new ItemStack(ItemDict.SALT_SHARDS, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PERFUMERY, 1), new ItemStack(ItemDict.BOARD, 10), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GLASSBLOWER, 1), new ItemStack(ItemDict.STONE, 15), new ItemStack(ItemDict.COAL, 1), new ItemStack(ItemDict.WIND_CRYSTAL, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PAINTERS_PRESS, 1), new ItemStack(ItemDict.STONE, 10), new ItemStack(ItemDict.HARDWOOD, 2)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.POTTERY_WHEEL, 1), new ItemStack(ItemDict.STONE, 15), new ItemStack(ItemDict.CLAY, 10)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ORIGAMI_STATION, 1), new ItemStack(ItemDict.HARDWOOD, 4), new ItemStack(ItemDict.PINK_LADYBUG, 1), new ItemStack(ItemDict.PAPER, 7)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FURNACE, 1), new ItemStack(ItemDict.STONE, 40), new ItemStack(ItemDict.SCRAP_IRON, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ANVIL, 1), new ItemStack(ItemDict.IRON_BAR, 12)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ALCHEMY_CAULDRON, 1), new ItemStack(ItemDict.IRON_BAR, 6)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.JEWELERS_BENCH, 1), new ItemStack(ItemDict.PLANK, 8), new ItemStack(ItemDict.BOAR_HIDE, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SYNTHESIZER, 1), new ItemStack(ItemDict.GLASS_SHEET, 20), new ItemStack(ItemDict.ADAMANTITE_BAR, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BEEHIVE, 1), new ItemStack(ItemDict.WOOD, 20), new ItemStack(ItemDict.HONEYCOMB, 3)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BIRDHOUSE, 1), new ItemStack(ItemDict.HARDWOOD, 5), new ItemStack(ItemDict.MOSSY_BARK, 5), new ItemStack(ItemDict.BIRDS_NEST, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FLOWERBED, 1), new ItemStack(ItemDict.WOOD, 20), new ItemStack(ItemDict.WEEDS, 10), new ItemStack(ItemDict.CLAY, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TERRARIUM, 1), new ItemStack(ItemDict.GLASS_SHEET, 3), new ItemStack(ItemDict.WOOD, 5), new ItemStack(ItemDict.STONE, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.AQUARIUM, 1), new ItemStack(ItemDict.GLASS_SHEET, 10), new ItemStack(ItemDict.WATER_CRYSTAL, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MUSHBOX, 1), new ItemStack(ItemDict.HARDWOOD, 5), new ItemStack(ItemDict.WEEDS, 20), new ItemStack(ItemDict.SPORES, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GEMSTONE_REPLICATOR, 1), new ItemStack(ItemDict.STONE, 40), new ItemStack(ItemDict.GLASS_SHEET, 3), new ItemStack(ItemDict.EARTH_CRYSTAL, 1), new ItemStack(ItemDict.FIRE_CRYSTAL, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.COMPRESSOR, 1), new ItemStack(ItemDict.STONE, 15), new ItemStack(ItemDict.GLASS_SHEET, 3), new ItemStack(ItemDict.COAL, 10)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.RECYCLER, 1), new ItemStack(ItemDict.IRON_BAR, 3), new ItemStack(ItemDict.GEARS, 2), new ItemStack(ItemDict.WEEDS, 99)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOTEM_OF_THE_PIG, 1), new ItemStack(ItemDict.HARDWOOD, 30), new ItemStack(ItemDict.GOLDEN_LEAF, 1), new ItemStack(ItemDict.TRUFFLE, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOTEM_OF_THE_COW, 1), new ItemStack(ItemDict.HARDWOOD, 30), new ItemStack(ItemDict.FAIRY_DUST, 1), new ItemStack(ItemDict.MILK, 10)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOTEM_OF_THE_SHEEP, 1), new ItemStack(ItemDict.HARDWOOD, 30), new ItemStack(ItemDict.GOLDEN_WOOL, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOTEM_OF_THE_CHICKEN, 1), new ItemStack(ItemDict.HARDWOOD, 30), new ItemStack(ItemDict.GOLDEN_EGG, 1), new ItemStack(ItemDict.EGG, 10)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOTEM_OF_THE_DOG, 1), new ItemStack(ItemDict.HARDWOOD, 30), new ItemStack(ItemDict.LUCKY_COIN, 4), new ItemStack(ItemDict.SAVORY_ROAST, 10)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOTEM_OF_THE_CAT, 1), new ItemStack(ItemDict.HARDWOOD, 30), new ItemStack(ItemDict.LUCKY_COIN, 4), new ItemStack(ItemDict.STORMFISH, 10)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOTEM_OF_THE_ROOSTER, 1), new ItemStack(ItemDict.HARDWOOD, 30), new ItemStack(ItemDict.GOLDEN_EGG, 2)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SOULCHEST, 1), new ItemStack(ItemDict.MYTHRIL_BAR, 8), new ItemStack(ItemDict.FIRE_CRYSTAL, 3)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SKY_STATUE, 1), new ItemStack(ItemDict.STONE, 40), new ItemStack(ItemDict.PRISMATIC_FEATHER, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DRACONIC_PILLAR, 1), new ItemStack(ItemDict.COAL, 20), new ItemStack(ItemDict.ADAMANTITE_BAR, 2)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ALCHEMIZER, 1), new ItemStack(ItemDict.GOLD_BAR, 4), new ItemStack(ItemDict.PHILOSOPHERS_STONE, 1), new ItemStack(ItemDict.PRISMATIC_FEATHER, 1)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.EXTRACTOR, 1), new ItemStack(ItemDict.IRON_BAR, 3), new ItemStack(ItemDict.IRON_CHIP, 5), new ItemStack(ItemDict.MYTHRIL_CHIP, 5), new ItemStack(ItemDict.GOLD_CHIP, 5)));
            MACHINE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CLONING_MACHINE, 1), new ItemStack(ItemDict.ADAMANTITE_BAR, 2), new ItemStack(ItemDict.GEARS, 25), new ItemStack(ItemDict.GLASS_SHEET, 3)));

            //scaffolding...
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SCAFFOLDING_WOOD, 5), new ItemStack(ItemDict.BOARD, 3)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_WOOD, 5), new ItemStack(ItemDict.BOARD, 5)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_WOOD_FARM, 5), new ItemStack(ItemDict.BOARD, 5), new ItemStack(ItemDict.LOAMY_COMPOST, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLOCK_WOOD, 5), new ItemStack(ItemDict.BOARD, 8)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WALL_PLANK, 5), new ItemStack(ItemDict.PLANK, 1)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_PLANK, 5), new ItemStack(ItemDict.PLANK, 1), new ItemStack(ItemDict.BOARD, 1)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLOCK_PLANK, 5), new ItemStack(ItemDict.PLANK, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLOCK_BARK, 5), new ItemStack(ItemDict.MOSSY_BARK, 5)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WALL_STONE, 5), new ItemStack(ItemDict.BRICKS, 3)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_STONE, 5), new ItemStack(ItemDict.BRICKS, 5)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_STONE_FARM, 5), new ItemStack(ItemDict.BRICKS, 5), new ItemStack(ItemDict.LOAMY_COMPOST, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLOCK_STONE, 5), new ItemStack(ItemDict.BRICKS, 8)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SCAFFOLDING_METAL, 5), new ItemStack(ItemDict.IRON_BAR, 1)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WALL_METAL, 5), new ItemStack(ItemDict.SCRAP_IRON, 5)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_METAL, 5), new ItemStack(ItemDict.IRON_BAR, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_METAL_FARM, 5), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.QUALITY_COMPOST, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLOCK_METAL, 5), new ItemStack(ItemDict.IRON_BAR, 3)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SCAFFOLDING_MYTHRIL, 5), new ItemStack(ItemDict.MYTHRIL_BAR, 1)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_MYTHRIL, 5), new ItemStack(ItemDict.MYTHRIL_BAR, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_MYTHRIL_FARM, 5), new ItemStack(ItemDict.MYTHRIL_BAR, 2), new ItemStack(ItemDict.QUALITY_COMPOST, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLOCK_MYTHRIL, 5), new ItemStack(ItemDict.MYTHRIL_BAR, 3)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SCAFFOLDING_GOLDEN, 5), new ItemStack(ItemDict.GOLD_BAR, 1)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_GOLDEN, 5), new ItemStack(ItemDict.GOLD_BAR, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLATFORM_GOLDEN_FARM, 5), new ItemStack(ItemDict.GOLD_BAR, 2), new ItemStack(ItemDict.QUALITY_COMPOST, 2)));
            SCAFFOLDING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLOCK_GOLDEN, 5), new ItemStack(ItemDict.GOLD_BAR, 3)));

            //simply woodworking
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BOX, 1), new ItemStack(ItemDict.HARDWOOD, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BRAZIER, 1), new ItemStack(ItemDict.HARDWOOD, 2), new ItemStack(ItemDict.WEEDS, 5)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CRATE, 1), new ItemStack(ItemDict.PLANK, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GARDEN_ARCH, 1), new ItemStack(ItemDict.BOARD, 6), new ItemStack(ItemDict.PLANK, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LATTICE, 1), new ItemStack(ItemDict.BOARD, 8)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_BENCH, 1), new ItemStack(ItemDict.BOARD, 3), new ItemStack(ItemDict.PLANK, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_CHAIR, 1), new ItemStack(ItemDict.BOARD, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_COLUMN, 1), new ItemStack(ItemDict.WOOD, 4), new ItemStack(ItemDict.HARDWOOD, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_LONGTABLE, 1), new ItemStack(ItemDict.BOARD, 8), new ItemStack(ItemDict.PLANK, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_ROUNDTABLE, 1), new ItemStack(ItemDict.BOARD, 5), new ItemStack(ItemDict.PLANK, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_SQUARETABLE, 1), new ItemStack(ItemDict.BOARD, 6)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_STOOL, 1), new ItemStack(ItemDict.BOARD, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_POST, 1), new ItemStack(ItemDict.WOOD, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TORCH, 1), new ItemStack(ItemDict.WOOD, 1), new ItemStack(ItemDict.WEEDS, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_FENCE, 5), new ItemStack(ItemDict.WOOD, 5)));
            //nature touch
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BAMBOO_POT, 1), new ItemStack(ItemDict.CLAY, 2), new ItemStack(ItemDict.BAMBOO, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BUOY, 1), new ItemStack(ItemDict.WOOD, 3), new ItemStack(ItemDict.OYSTER, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CAMPFIRE, 1), new ItemStack(ItemDict.WOOD, 6), new ItemStack(ItemDict.WEEDS, 5)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DECORATIVE_BOULDER, 1), new ItemStack(ItemDict.STONE, 12), new ItemStack(ItemDict.WEEDS, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DECORATIVE_LOG, 1), new ItemStack(ItemDict.HARDWOOD, 4), new ItemStack(ItemDict.WEEDS, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FIREPIT, 1), new ItemStack(ItemDict.BRICKS, 2), new ItemStack(ItemDict.WOOD, 2), new ItemStack(ItemDict.WEEDS, 5)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HAMMOCK, 1), new ItemStack(ItemDict.WOOD, 3), new ItemStack(ItemDict.LINEN_CLOTH, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LIFEBUOY_SIGN, 1), new ItemStack(ItemDict.WOOD, 5), new ItemStack(ItemDict.FLAWLESS_CONCH, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SANDCASTLE, 1), new ItemStack(ItemDict.PEARL, 1), new ItemStack(ItemDict.EARTH_CRYSTAL, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.UMBRELLA, 1), new ItemStack(ItemDict.IRON_BAR, 1), new ItemStack(ItemDict.LINEN_CLOTH, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.UMBRELLA_TABLE, 1), new ItemStack(ItemDict.UMBRELLA, 1), new ItemStack(ItemDict.WOODEN_ROUNDTABLE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SURFBOARD, 1), new ItemStack(ItemDict.WOOD, 3), new ItemStack(ItemDict.BLUE_DYE, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BAMBOO_FENCE, 5), new ItemStack(ItemDict.BAMBOO, 3)));
            //fab farm
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CART, 1), new ItemStack(ItemDict.BOARD, 3), new ItemStack(ItemDict.IRON_BAR, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CLOTHESLINE, 1), new ItemStack(ItemDict.WOOD, 2), new ItemStack(ItemDict.COTTON_CLOTH, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FLAGPOLE, 1), new ItemStack(ItemDict.PLANK, 3), new ItemStack(ItemDict.WOOLEN_CLOTH, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HAYBALE, 1), new ItemStack(ItemDict.MOUNTAIN_WHEAT, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MAILBOX, 1), new ItemStack(ItemDict.BOARD, 1), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.RED_DYE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MARKET_STALL, 1), new ItemStack(ItemDict.PLANK, 8), new ItemStack(ItemDict.LINEN_CLOTH, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MILK_JUG, 1), new ItemStack(ItemDict.IRON_BAR, 1), new ItemStack(ItemDict.MILK, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PET_BOWL, 1), new ItemStack(ItemDict.IRON_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PILE_OF_BRICKS, 1), new ItemStack(ItemDict.BRICKS, 3), new ItemStack(ItemDict.RED_DYE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SCARECROW, 1), new ItemStack(ItemDict.MOUNTAIN_WHEAT, 3), new ItemStack(ItemDict.WEEDS, 2), new ItemStack(ItemDict.HARDWOOD, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SIGNPOST, 1), new ItemStack(ItemDict.BOARD, 3), new ItemStack(ItemDict.BLACK_DYE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOOLBOX, 1), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.RED_DYE, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOOLRACK, 1), new ItemStack(ItemDict.BOARD, 4), new ItemStack(ItemDict.IRON_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WAGON, 1), new ItemStack(ItemDict.HARDWOOD, 6), new ItemStack(ItemDict.LINEN_CLOTH, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WATER_PUMP, 1), new ItemStack(ItemDict.IRON_BAR, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WATERTOWER, 1), new ItemStack(ItemDict.BOARD, 16), new ItemStack(ItemDict.WATER_CRYSTAL, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WHEELBARROW, 1), new ItemStack(ItemDict.BOARD, 4), new ItemStack(ItemDict.IRON_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TALL_FENCE, 5), new ItemStack(ItemDict.BOARD, 5)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BELL, 1), new ItemStack(ItemDict.WOOD, 2), new ItemStack(ItemDict.GOLD_BAR, 1)));
            //reflec
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ANATOMICAL_POSTER, 1), new ItemStack(ItemDict.RED_DYE, 4), new ItemStack(ItemDict.ORANGE_DYE, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BANNER, 1), new ItemStack(ItemDict.LINEN_CLOTH, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CANVAS, 1), new ItemStack(ItemDict.BOARD, 2), new ItemStack(ItemDict.PAPER, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HELIX_POSTER, 1), new ItemStack(ItemDict.BLUE_DYE, 4), new ItemStack(ItemDict.NAVY_DYE, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HORIZONTAL_MIRROR, 1), new ItemStack(ItemDict.GOLD_BAR, 1), new ItemStack(ItemDict.GLASS_SHEET, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ORNATE_MIRROR, 1), new ItemStack(ItemDict.GOLDEN_LEAF, 3), new ItemStack(ItemDict.GLASS_SHEET, 6)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.RAINBOW_GRAFFITI, 1), new ItemStack(ItemDict.ORANGE_DYE, 3), new ItemStack(ItemDict.BLUE_DYE, 2), new ItemStack(ItemDict.OLIVE_DYE, 1), new ItemStack(ItemDict.PURPLE_DYE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TRIPLE_MIRRORS, 1), new ItemStack(ItemDict.WALL_MIRROR, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WALL_MIRROR, 1), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.GLASS_SHEET, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GLASS_FENCE, 5), new ItemStack(ItemDict.GLASS_SHEET, 1)));
            //ess eng
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CLOCK, 1), new ItemStack(ItemDict.GEARS, 1), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GRANDFATHER_CLOCK, 1), new ItemStack(ItemDict.HARDWOOD, 4), new ItemStack(ItemDict.GEARS, 2), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STREETLAMP, 1), new ItemStack(ItemDict.BOARD, 2), new ItemStack(ItemDict.PLANK, 2), new ItemStack(ItemDict.FIREFLY, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STREETLIGHT, 1), new ItemStack(ItemDict.IRON_BAR, 4), new ItemStack(ItemDict.WOOLEN_CLOTH, 2), new ItemStack(ItemDict.FIRE_CRYSTAL, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TELEVISION, 1), new ItemStack(ItemDict.HARDWOOD, 2), new ItemStack(ItemDict.MYTHRIL_BAR, 2), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LAMP, 1), new ItemStack(ItemDict.HARDWOOD, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.FIRE_CRYSTAL, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LANTERN, 1), new ItemStack(ItemDict.STONE, 4), new ItemStack(ItemDict.FIREFLY, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MINECART, 1), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.BOARD, 3), new ItemStack(ItemDict.GOLD_ORE, 4)));
            //crv stone
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CUBE_STATUE, 1), new ItemStack(ItemDict.STONE, 8), new ItemStack(ItemDict.IRON_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PYRAMID_STATUE, 1), new ItemStack(ItemDict.STONE, 8), new ItemStack(ItemDict.MYTHRIL_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SPHERE_STATUE, 1), new ItemStack(ItemDict.STONE, 8), new ItemStack(ItemDict.GOLD_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STATUE, 1), new ItemStack(ItemDict.STONE, 12), new ItemStack(ItemDict.BATHROBE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STONE_COLUMN, 1), new ItemStack(ItemDict.STONE, 6), new ItemStack(ItemDict.IRON_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FIREPLACE, 1), new ItemStack(ItemDict.BRICKS, 10), new ItemStack(ItemDict.CLAY, 4), new ItemStack(ItemDict.COAL, 4), new ItemStack(ItemDict.FIRE_CRYSTAL, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WELL, 1), new ItemStack(ItemDict.BRICKS, 8), new ItemStack(ItemDict.BOARD, 2), new ItemStack(ItemDict.WATER_CRYSTAL, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STONE_FENCE, 5), new ItemStack(ItemDict.BRICKS, 3)));
            //urban design
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BOOMBOX, 1), new ItemStack(ItemDict.WOOD, 2), new ItemStack(ItemDict.GEARS, 1), new ItemStack(ItemDict.GLASS_SHEET, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FIRE_HYDRANT, 1), new ItemStack(ItemDict.IRON_BAR, 3), new ItemStack(ItemDict.RED_DYE, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GYM_BENCH, 1), new ItemStack(ItemDict.IRON_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LIGHTNING_ROD, 1), new ItemStack(ItemDict.IRON_BAR, 3), new ItemStack(ItemDict.MYTHRIL_BAR, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.POSTBOX, 1), new ItemStack(ItemDict.IRON_BAR, 4), new ItemStack(ItemDict.BLUE_DYE, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.RECYCLING_BIN, 1), new ItemStack(ItemDict.BOARD, 3), new ItemStack(ItemDict.GREEN_DYE, 4), new ItemStack(ItemDict.WHITE_DYE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SOFA, 1), new ItemStack(ItemDict.HARDWOOD, 4), new ItemStack(ItemDict.WOOLEN_CLOTH, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SOLAR_PANEL, 1), new ItemStack(ItemDict.IRON_BAR, 1), new ItemStack(ItemDict.GOLD_BAR, 1), new ItemStack(ItemDict.MYTHRIL_BAR, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TRAFFIC_CONE, 1), new ItemStack(ItemDict.WOOD, 3), new ItemStack(ItemDict.ORANGE_DYE, 3), new ItemStack(ItemDict.WHITE_DYE, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TRAFFIC_LIGHT, 1), new ItemStack(ItemDict.IRON_BAR, 4), new ItemStack(ItemDict.RED_DYE, 1), new ItemStack(ItemDict.YELLOW_DYE, 1), new ItemStack(ItemDict.GREEN_DYE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TRASHCAN, 1), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.WEEDS, 6)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FULL_THROTTLE_GRAFFITI, 1), new ItemStack(ItemDict.ORANGE_DYE, 3), new ItemStack(ItemDict.RED_DYE, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HEARTBREAK_GRAFFITI, 1), new ItemStack(ItemDict.RED_DYE, 3), new ItemStack(ItemDict.PURPLE_DYE, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HEROINE_GRAFFITI, 1), new ItemStack(ItemDict.PINK_DYE, 3), new ItemStack(ItemDict.PURPLE_DYE, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LEFTWARD_GRAFFITI, 1), new ItemStack(ItemDict.RED_DYE, 7)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.NOIZEBOYZ_GRAFFITI, 1), new ItemStack(ItemDict.YELLOW_DYE, 3), new ItemStack(ItemDict.GREEN_DYE, 3), new ItemStack(ItemDict.WHITE_DYE, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.RETRO_GRAFFITI, 1), new ItemStack(ItemDict.YELLOW_DYE, 6)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.RIGHT_ARROW_GRAFFITI, 1), new ItemStack(ItemDict.GREEN_DYE, 3), new ItemStack(ItemDict.OLIVE_DYE, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SMILE_GRAFFITI, 1), new ItemStack(ItemDict.RED_DYE, 2), new ItemStack(ItemDict.BLUE_DYE, 2), new ItemStack(ItemDict.GREEN_DYE, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SOURCE_UNKNOWN_GRAFFITI, 1), new ItemStack(ItemDict.BLUE_DYE, 3), new ItemStack(ItemDict.NAVY_DYE, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.METAL_FENCE, 5), new ItemStack(ItemDict.IRON_BAR, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MYTHRIL_FENCE, 5), new ItemStack(ItemDict.MYTHRIL_BAR, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GOLDEN_FENCE, 5), new ItemStack(ItemDict.GOLD_BAR, 1)));
            //ice
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FROST_SCULPTURE, 1), new ItemStack(ItemDict.SNOW_CRYSTAL, 4), new ItemStack(ItemDict.ICE_NINE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ICE_BLOCK, 1), new ItemStack(ItemDict.SNOW_CRYSTAL, 6)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.IGLOO, 1), new ItemStack(ItemDict.SNOW_CRYSTAL, 12), new ItemStack(ItemDict.IRON_BAR, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SNOWMAN, 1), new ItemStack(ItemDict.SNOW_CRYSTAL, 5), new ItemStack(ItemDict.ICE_NINE, 1), new ItemStack(ItemDict.CARROT, 1), new ItemStack(ItemDict.WOOD, 2)));
            //playgr
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLACKBOARD, 1), new ItemStack(ItemDict.BOARD, 4), new ItemStack(ItemDict.COAL, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SANDBOX, 1), new ItemStack(ItemDict.WOOD, 2), new ItemStack(ItemDict.EARTH_CRYSTAL, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SEESAW, 1), new ItemStack(ItemDict.PLANK, 2), new ItemStack(ItemDict.RED_DYE, 4)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SLIDE, 1), new ItemStack(ItemDict.HARDWOOD, 2), new ItemStack(ItemDict.IRON_BAR, 6)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SWINGS, 1), new ItemStack(ItemDict.WOOD, 8), new ItemStack(ItemDict.COTTON_CLOTH, 3), new ItemStack(ItemDict.IRON_BAR, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TARGET, 1), new ItemStack(ItemDict.BOARD, 3), new ItemStack(ItemDict.RED_DYE, 1), new ItemStack(ItemDict.WHITE_DYE, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WHITEBOARD, 1), new ItemStack(ItemDict.BOARD, 4), new ItemStack(ItemDict.IRON_BAR, 1), new ItemStack(ItemDict.WHITE_DYE, 5)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DRUM, 1), new ItemStack(ItemDict.WOOD, 4), new ItemStack(ItemDict.COTTON_CLOTH, 1)));
            //music
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CYMBAL, 1), new ItemStack(ItemDict.HARDWOOD, 1), new ItemStack(ItemDict.IRON_BAR, 3)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GUITAR_PLACEABLE, 1), new ItemStack(ItemDict.HARDWOOD, 3), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PIANO, 1), new ItemStack(ItemDict.HARDWOOD, 4), new ItemStack(ItemDict.GOLDEN_LEAF, 2)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DRUMSET, 1), new ItemStack(ItemDict.IRON_BAR, 5), new ItemStack(ItemDict.GOLD_BAR, 2), new ItemStack(ItemDict.BOAR_HIDE, 7)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HARP, 1), new ItemStack(ItemDict.HARDWOOD, 4), new ItemStack(ItemDict.LINEN_CLOTH, 2), new ItemStack(ItemDict.GOLDEN_LEAF, 1), new ItemStack(ItemDict.FAIRY_DUST, 1)));
            FURNITURE_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.XYLOPHONE, 1), new ItemStack(ItemDict.HARDWOOD, 3), new ItemStack(ItemDict.PLANK, 8), new ItemStack(ItemDict.PEARL, 1)));

            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STEPPING_STONE_FLOOR, 5), new ItemStack(ItemDict.STONE, 1), new ItemStack(ItemDict.IRON_ORE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CARPET_FLOOR, 5), new ItemStack(ItemDict.WOOLEN_CLOTH, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CONCRETE_FLOOR, 5), new ItemStack(ItemDict.CLAY, 4), new ItemStack(ItemDict.DARK_GREY_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STREET_FLOOR, 5), new ItemStack(ItemDict.CLAY, 2), new ItemStack(ItemDict.BRICKS, 2), new ItemStack(ItemDict.YELLOW_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BOARDWALK_FLOOR, 5), new ItemStack(ItemDict.BOARD, 2), new ItemStack(ItemDict.CRIMSON_CORAL, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOODEN_FLOOR, 5), new ItemStack(ItemDict.PLANK, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TATAMI_FLOOR, 5), new ItemStack(ItemDict.BAMBOO, 3)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.THIN_TATAMI_FLOOR, 5), new ItemStack(ItemDict.BAMBOO, 2)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MAT_FLOOR, 5), new ItemStack(ItemDict.COTTON_CLOTH, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FOOTPRINT_FLOOR, 5), new ItemStack(ItemDict.CLAY, 2)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TRIANGULATE_FLOOR, 5), new ItemStack(ItemDict.BLUE_DYE, 1), new ItemStack(ItemDict.RED_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SQUARE_FLOOR, 5), new ItemStack(ItemDict.STONE, 2)));

            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SOLID_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.HARDWOOD, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HORIZONTAL_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.BLUE_DYE, 1), new ItemStack(ItemDict.WHITE_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.VERTICAL_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.LIGHT_BROWN_DYE, 2)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WAVE_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.BLUE_DYE, 2)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STAR_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.BLACK_DYE, 1), new ItemStack(ItemDict.YELLOW_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BUBBLE_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.BLUE_DYE, 1), new ItemStack(ItemDict.NAVY_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DOT_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.WHITE_DYE, 1), new ItemStack(ItemDict.BLACK_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.POLKA_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.LIGHT_BROWN_DYE, 1), new ItemStack(ItemDict.YELLOW_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.INVADER_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.RED_DYE, 1), new ItemStack(ItemDict.BLACK_DYE, 1)));
            WALL_FLOOR_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ODD_WALLPAPER, 5), new ItemStack(ItemDict.PAPER, 2), new ItemStack(ItemDict.GREEN_DYE, 1), new ItemStack(ItemDict.OLIVE_DYE, 1)));

            // (S/S) (F/W) (COUNTRY) (URBAN) (TROPICAL) (COSTUME)
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SHORT_SLEEVE_TEE, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STRIPED_SHIRT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.BLACK_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TANKER, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.BLUE_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LONG_SLEEVE_TEE, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BUTTON_DOWN, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.BLUE_DYE, 1), new ItemStack(ItemDict.PEARL, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PLAID_BUTTON, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 2), new ItemStack(ItemDict.BLACK_DYE, 1), new ItemStack(ItemDict.RED_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TURTLENECK, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 3), new ItemStack(ItemDict.OLIVE_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SWEATER, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 3), new ItemStack(ItemDict.ORANGE_DYE, 1), new ItemStack(ItemDict.RED_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LINEN_BUTTON, 1), new ItemStack(ItemDict.LINEN_CLOTH, 2), new ItemStack(ItemDict.PEARL, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ISLANDER_TATTOO, 1), new ItemStack(ItemDict.GREEN_DYE, 4), new ItemStack(ItemDict.FLAWLESS_CONCH, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.JEANS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.BLUE_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CHINO_SHORTS, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.NAVY_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SHORT_SKIRT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.DARK_BROWN_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PUFF_SKIRT, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.PINK_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CHINOS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.LIGHT_BROWN_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LONG_SKIRT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.PURPLE_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TORN_JEANS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.BLACK_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.JEAN_SHORTS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.LIGHT_GREY_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SUPER_SHORTS, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.PURPLE_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TIGHTIES, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.WHITE_DYE, 3)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ALL_SEASON_JACKET, 1), new ItemStack(ItemDict.COTTON_CLOTH, 4), new ItemStack(ItemDict.OLIVE_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HOODED_SWEATSHIRT, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 4), new ItemStack(ItemDict.OLIVE_DYE, 3)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.OVERCOAT, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 6), new ItemStack(ItemDict.LIGHT_BROWN_DYE, 3)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.APRON, 1), new ItemStack(ItemDict.COTTON_CLOTH, 5), new ItemStack(ItemDict.PINK_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.OVERALLS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 6), new ItemStack(ItemDict.NAVY_DYE, 4)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.RAINCOAT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 4), new ItemStack(ItemDict.BEESWAX, 6))); //add beewax
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PUNK_JACKET, 1), new ItemStack(ItemDict.BOAR_HIDE, 6), new ItemStack(ItemDict.RED_DYE, 6)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ONESIE, 1), new ItemStack(ItemDict.COTTON_CLOTH, 6), new ItemStack(ItemDict.BLUE_DYE, 3)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WEDDING_DRESS, 1), new ItemStack(ItemDict.LINEN_CLOTH, 8), new ItemStack(ItemDict.DIAMOND, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SUIT_JACKET, 1), new ItemStack(ItemDict.COTTON_CLOTH, 5), new ItemStack(ItemDict.SKY_ROSE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.NOMAD_VEST, 1), new ItemStack(ItemDict.LINEN_CLOTH, 4), new ItemStack(ItemDict.ORANGE_DYE, 3), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BATHROBE, 1), new ItemStack(ItemDict.LINEN_CLOTH, 5), new ItemStack(ItemDict.WHITE_DYE, 3), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SPORTBALL_UNIFORM, 1), new ItemStack(ItemDict.COTTON_CLOTH, 5), new ItemStack(ItemDict.ADAMANTITE_BAR, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SNEAKERS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.BLUE_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HIGH_TOPS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.RED_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TALL_BOOTS, 1), new ItemStack(ItemDict.BOAR_HIDE, 4), new ItemStack(ItemDict.WOOLEN_CLOTH, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WING_SANDLES, 1), new ItemStack(ItemDict.HARDWOOD, 2), new ItemStack(ItemDict.PRISMATIC_FEATHER, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FLASH_HEELS, 1), new ItemStack(ItemDict.BOAR_HIDE, 1), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SHORT_SOCKS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.LONG_SOCKS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.NAVY_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STRIPED_SOCKS, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FESTIVE_SOCKS, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 1), new ItemStack(ItemDict.WINTERGREEN, 4)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MISMATTCHED, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 1), new ItemStack(ItemDict.ALBINO_WING, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.NECKWARMER, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 1), new ItemStack(ItemDict.LIGHT_BROWN_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SCARF, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 2), new ItemStack(ItemDict.GREEN_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SASH, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 1), new ItemStack(ItemDict.RED_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ASCOT, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 2), new ItemStack(ItemDict.NAVY_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.NECKLACE, 1), new ItemStack(ItemDict.PEARL, 3)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TIE, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.RED_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.MEDAL, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOOL_MITTENS, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 1), new ItemStack(ItemDict.SNOW_CRYSTAL, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WORK_GLOVES, 1), new ItemStack(ItemDict.BOAR_HIDE, 2), new ItemStack(ItemDict.WOOLEN_CLOTH, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BOXING_MITTS, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.RED_DYE, 2)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SAILCLOTH, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.WIND_CRYSTAL, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.RUCKSACK, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.WOOD, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GUITAR, 1), new ItemStack(ItemDict.HARDWOOD, 8), new ItemStack(ItemDict.MYTHRIL_BAR, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BACKPACK, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.IRON_BAR, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CAT_TAIL, 1), new ItemStack(ItemDict.BOAR_HIDE, 2), new ItemStack(ItemDict.BLACK_DYE, 3), new ItemStack(ItemDict.WHITE_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FOX_TAIL, 1), new ItemStack(ItemDict.BOAR_HIDE, 2), new ItemStack(ItemDict.ORANGE_DYE, 5)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WOLF_TAIL, 1), new ItemStack(ItemDict.BOAR_HIDE, 2), new ItemStack(ItemDict.DARK_GREY_DYE, 2), new ItemStack(ItemDict.LIGHT_GREY_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CAPE, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 3), new ItemStack(ItemDict.RED_DYE, 4)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CLOCKWORK, 1), new ItemStack(ItemDict.GEARS, 3), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.ROBO_ARMS, 1), new ItemStack(ItemDict.ADAMANTITE_BAR, 1), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GLASSES, 1), new ItemStack(ItemDict.IRON_BAR, 1), new ItemStack(ItemDict.GLASS_SHEET, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.GOGGLES, 1), new ItemStack(ItemDict.MYTHRIL_BAR, 1), new ItemStack(ItemDict.NAVY_DYE, 3), new ItemStack(ItemDict.GLASS_SHEET, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PROTECTIVE_VISOR, 1), new ItemStack(ItemDict.IRON_BAR, 3), new ItemStack(ItemDict.SCRAP_IRON, 5)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SUNGLASSES, 1), new ItemStack(ItemDict.IRON_BAR, 1), new ItemStack(ItemDict.BLACK_DYE, 3), new ItemStack(ItemDict.GLASS_SHEET, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.QUERADE_MASK, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.BLACK_DYE, 2), new ItemStack(ItemDict.BLACK_FEATHER, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SNORKEL, 1), new ItemStack(ItemDict.MYTHRIL_BAR, 2), new ItemStack(ItemDict.GLASS_SHEET, 1), new ItemStack(ItemDict.FLAWLESS_CONCH, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.EYEPATCH, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.BOAR_HIDE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BLINDFOLD, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.BLACK_DYE, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.EARRING_STUD, 1), new ItemStack(ItemDict.IRON_BAR, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.PIERCING, 1), new ItemStack(ItemDict.MYTHRIL_BAR, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DANGLE_EARRING, 1), new ItemStack(ItemDict.GOLD_BAR, 1)));

            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BASEBALL_CAP, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.GREEN_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.HEADBAND, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.GREEN_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BUTTERFLY_CLIP, 1), new ItemStack(ItemDict.WOOD, 1), new ItemStack(ItemDict.YELLOW_BUTTERFLY, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BOWLER, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.DARK_BROWN_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CAMEL_HAT, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 2), new ItemStack(ItemDict.BOAR_HIDE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SQUARE_HAT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.DARK_BROWN_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FLAT_CAP, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.BLUE_DYE, 3)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.STRAW_HAT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 1), new ItemStack(ItemDict.MOUNTAIN_WHEAT, 5)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TEN_GALLON, 1), new ItemStack(ItemDict.BOAR_HIDE, 8)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BANDANA, 1), new ItemStack(ItemDict.LINEN_CLOTH, 1), new ItemStack(ItemDict.BLUE_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CONICAL_FARMER, 1), new ItemStack(ItemDict.BAMBOO, 6), new ItemStack(ItemDict.WILD_RICE, 3)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.FACEMASK, 1), new ItemStack(ItemDict.LINEN_CLOTH, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.SNAPBACK, 1), new ItemStack(ItemDict.COTTON_CLOTH, 2), new ItemStack(ItemDict.LIGHT_GREY_DYE, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TOP_HAT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 3), new ItemStack(ItemDict.BLACK_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.TRACE_TATTOO, 1), new ItemStack(ItemDict.GREEN_DYE, 5), new ItemStack(ItemDict.ALBINO_WING, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.WHISKERS, 1), new ItemStack(ItemDict.MYTHRIL_BAR, 1), new ItemStack(ItemDict.GOLDEN_LEAF, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.BUNNY_EARS, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 3), new ItemStack(ItemDict.CARROT, 10)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CAT_EARS, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 2), new ItemStack(ItemDict.EMPEROR_SALMON, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DOG_MASK, 1), new ItemStack(ItemDict.WOOLEN_CLOTH, 5), new ItemStack(ItemDict.BOAR_HIDE, 4)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.DINO_MASK, 1), new ItemStack(ItemDict.BOAR_HIDE, 4), new ItemStack(ItemDict.GREEN_DYE, 4), new ItemStack(ItemDict.ADAMANTITE_BAR, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.NIGHTCAP, 1), new ItemStack(ItemDict.LINEN_CLOTH, 2), new ItemStack(ItemDict.OLIVE_DYE, 2)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.CHEFS_HAT, 1), new ItemStack(ItemDict.COTTON_CLOTH, 3), new ItemStack(ItemDict.SAVORY_ROAST, 1)));
            CLOTHING_RECIPES.Add(new CraftingRecipe(new ItemStack(ItemDict.NIGHTMARE_MASK, 1), new ItemStack(ItemDict.IRON_BAR, 2), new ItemStack(ItemDict.WHITE_DYE, 2)));

            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.MOUNTAIN_BREAD, ItemDict.MOUNTAIN_WHEAT, ItemDict.MOUNTAIN_WHEAT, ItemDict.MOUNTAIN_WHEAT, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.WILD_POPCORN, ItemDict.MAIZE, ItemDict.MAIZE, ItemDict.SALT_SHARDS, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.ROASTED_PUMPKIN, ItemDict.PUMPKIN, ItemDict.BUTTER, ItemDict.OIL, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SEASONAL_PIPERADE, ItemDict.ONION, ItemDict.BELLPEPPER, ItemDict.TOMATO, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.TOMATO_SOUP, ItemDict.TOMATO, ItemDict.TOMATO, ItemDict.CREAM, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SHRIMP_GUMBO, ItemDict.SHRIMP, ItemDict.ONION, ItemDict.BELLPEPPER, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.STUFFED_FLOUNDER, ItemDict.CLOUD_FLOUNDER, ItemDict.BOXER_LOBSTER, ItemDict.CRAB, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.FRIED_CATFISH, ItemDict.CATFISH, ItemDict.BUTTER, ItemDict.OIL, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.FRIED_FISH, ItemDict.BLUEGILL, ItemDict.CARP, ItemDict.OIL, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.FRIED_OYSTERS, ItemDict.OYSTER, ItemDict.OIL, ItemDict.BUTTER, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.BLIND_DINNER, ItemDict.CAVEFISH, ItemDict.CAVEFISH, ItemDict.CAVERN_TETRA, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.STORMFISH, ItemDict.STORMBRINGER_KOI, ItemDict.SPICY_LEAF, ItemDict.SALT_SHARDS, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.GRILLED_SALMON, ItemDict.SALMON, ItemDict.LEMON, ItemDict.BUTTER, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.CREAMY_SQUID, ItemDict.MOLTEN_SQUID, ItemDict.CREAM, ItemDict.MILK, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.BAKED_SNAPPER, ItemDict.RED_SNAPPER, ItemDict.BUTTER, ItemDict.SPINACH, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SWORDFISH_POT, ItemDict.SWORDFISH, ItemDict.EGG, ItemDict.CREAM, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.POTATO_AND_BEET_FRIES, ItemDict.POTATO, ItemDict.BEET, ItemDict.OIL, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SPRING_PIZZA, ItemDict.SPINACH, ItemDict.CHEESE, ItemDict.MOUNTAIN_WHEAT, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.VEGGIE_CHIPS, ItemDict.CARROT, ItemDict.POTATO, ItemDict.SALT_SHARDS, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.FARMERS_STEW, ItemDict.ONION, ItemDict.CABBAGE, ItemDict.TOMATO, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.CLAM_LINGUINI, ItemDict.CLAM, ItemDict.CLAM, ItemDict.MOUNTAIN_WHEAT, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.FRENCH_ONION_SOUP, ItemDict.ONION, ItemDict.ONION, ItemDict.CHEESE, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.ELDERBERRY_TART, ItemDict.ELDERBERRY, ItemDict.ELDERBERRY, ItemDict.MOUNTAIN_WHEAT, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.VEGGIE_SIDE_ROAST, ItemDict.BEET, ItemDict.BELLPEPPER, ItemDict.OIL, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.CREAM_OF_MUSHROOM, ItemDict.SHIITAKE, ItemDict.MOREL, ItemDict.CREAM, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.STEWED_VEGGIES, ItemDict.CUCUMBER, ItemDict.TOMATO, ItemDict.ONION, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.BOAR_STEW, ItemDict.WILD_MEAT, ItemDict.POTATO, ItemDict.CARROT, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.HONEY_STIR_FRY, ItemDict.BELLPEPPER, ItemDict.WILD_MEAT, ItemDict.WILD_HONEY, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.FRIED_EGG, ItemDict.EGG, ItemDict.EGG, ItemDict.OIL, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.CREAMY_SPINACH, ItemDict.SPINACH, ItemDict.CREAM, ItemDict.MILK, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.DWARVEN_STEW, ItemDict.ONION, ItemDict.SHIITAKE, ItemDict.CHANTERELLE, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.EGG_SCRAMBLE, ItemDict.EGG, ItemDict.MILK, ItemDict.BUTTER, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.BAKED_POTATO, ItemDict.POTATO, ItemDict.BUTTER, ItemDict.BUTTER, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.MEATY_PIZZA, ItemDict.MOUNTAIN_WHEAT, ItemDict.WILD_MEAT, ItemDict.TOMATO, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.BUTTERED_ROLLS, ItemDict.MOUNTAIN_WHEAT, ItemDict.BUTTER, ItemDict.EGG, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.WRAPPED_CABBAGE, ItemDict.CABBAGE, ItemDict.WILD_MEAT, ItemDict.WILD_MEAT, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SAVORY_ROAST, ItemDict.WILD_MEAT, ItemDict.APPLE, ItemDict.BUTTER, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SEAFOOD_PAELLA, ItemDict.SHRIMP, ItemDict.BELLPEPPER, ItemDict.WILD_RICE, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.COCONUT_BOAR, ItemDict.WILD_MEAT, ItemDict.COCONUT, ItemDict.CABBAGE, CookingRecipe.LengthEnum.LONG));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.MOUNTAIN_TERIYAKI, ItemDict.WILD_MEAT, ItemDict.CAVE_FUNGI, ItemDict.SHIITAKE, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SWEET_COCO_TREAT, ItemDict.WILD_HONEY, ItemDict.VANILLA_EXTRACT, ItemDict.COCONUT, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.MINTY_MELT, ItemDict.SNOW_CRYSTAL, ItemDict.MINT_EXTRACT, ItemDict.VANILLA_EXTRACT, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.BLUEBERRY_PANCAKES, ItemDict.MOUNTAIN_WHEAT, ItemDict.WILD_HONEY, ItemDict.BLUEBERRY, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.APPLE_MUFFIN, ItemDict.APPLE, ItemDict.MOUNTAIN_WHEAT, ItemDict.VANILLA_EXTRACT, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.SEARED_TUNA, ItemDict.TUNA, ItemDict.SPICY_LEAF, ItemDict.PINEAPPLE, CookingRecipe.LengthEnum.SHORT));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.RATATOUILLE, ItemDict.EGGPLANT, ItemDict.BELLPEPPER, ItemDict.ONION, CookingRecipe.LengthEnum.MEDIUM));
            OVEN_RECIPES.Add(new CookingRecipe(ItemDict.EGGPLANT_PARMESAN, ItemDict.EGGPLANT, ItemDict.TOMATO, ItemDict.CHEESE, CookingRecipe.LengthEnum.LONG));

            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SEARED_TUNA, ItemDict.TUNA, ItemDict.SPICY_LEAF, ItemDict.PINEAPPLE, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SARDINE_SNACK, ItemDict.SARDINE, ItemDict.HERRING, ItemDict.STRIPED_BASS, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.MUSHROOM_STIR_FRY, ItemDict.MOREL, ItemDict.OYSTER_MUSHROOM, ItemDict.WILD_RICE, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SEAFOOD_BASKET, ItemDict.CLAM, ItemDict.OYSTER, ItemDict.SHRIMP, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.CRISPY_GRASSHOPPER, ItemDict.RICE_GRASSHOPPER, ItemDict.SPICY_LEAF, ItemDict.OIL, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.ESCARGOT, ItemDict.SNAIL, ItemDict.SNAIL, ItemDict.BUTTER, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.BREAKFAST_POTATOES, ItemDict.POTATO, ItemDict.SALT_SHARDS, ItemDict.OIL, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SPICY_BACON, ItemDict.WILD_MEAT, ItemDict.SPICY_LEAF, ItemDict.OIL, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.CAMPFIRE_COFFEE, ItemDict.CHICORY_ROOT, ItemDict.CHICORY_ROOT, ItemDict.SNOW_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.COLESLAW, ItemDict.MAYONNAISE, ItemDict.CARROT, ItemDict.CABBAGE, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUPER_JUICE, ItemDict.SPINACH, ItemDict.CARROT, ItemDict.CABBAGE, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.BLUEGILL, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.LAKE_TROUT, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.CARP, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.CATFISH, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.JUNGLE_PIRANHA, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.SARDINE, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.HERRING, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.MACKEREL, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.TUNA, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.STRIPED_BASS, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.INKY_SQUID, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.SHRIMP, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.CAVEFISH, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.CAVERN_TETRA, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.BLACKENED_OCTOPUS, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.MOLTEN_SQUID, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SUSHI_ROLL, ItemDict.CLOUD_FLOUNDER, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.EEL_ROLL, ItemDict.ONYX_EEL, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SEAWEED_SNACK, ItemDict.SEAWEED, ItemDict.SEAWEED, ItemDict.SALT_SHARDS, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.TUNA, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE)); //TODO
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.SALMON, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.PIKE, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.SUNFISH, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.GREAT_WHITE_SHARK, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.QUEEN_AROWANA, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.EMPEROR_SALMON, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.SWORDFISH, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.CRAB, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.RED_SNAPPER, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.DARK_ANGLER, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.BOXER_LOBSTER, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.INFERNAL_SHARK, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.LUNAR_WHALE, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.SKY_PIKE, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DELUXE_SUSHI, ItemDict.STORMBRINGER_KOI, ItemDict.WILD_RICE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.RAW_CALAMARI, ItemDict.BLACKENED_OCTOPUS, ItemDict.SALT_SHARDS, ItemDict.BUTTER, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.CHICKWEED_BLEND, ItemDict.CHICKWEED, ItemDict.CHICKWEED, ItemDict.SPINACH, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.DARK_TEA, ItemDict.CAVE_SOYBEAN, ItemDict.SASSAFRAS, ItemDict.SASSAFRAS, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.STRAWBERRY_SALAD, ItemDict.STRAWBERRY, ItemDict.SPINACH, ItemDict.SPINACH, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.FRESH_SALAD, ItemDict.WATERMELON_SLICE, ItemDict.CUCUMBER, ItemDict.TOMATO, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.VANILLA_ICE_CREAM, ItemDict.VANILLA_EXTRACT, ItemDict.SNOW_CRYSTAL, ItemDict.SNOW_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.PICKLED_BEET_EGG, ItemDict.EGG, ItemDict.BEET, ItemDict.SPICY_LEAF, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.CHERRY_CHEESECAKE, ItemDict.MOUNTAIN_WHEAT, ItemDict.CHEESE, ItemDict.CHERRY, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.SURVIVORS_SURPRISE, ItemDict.EARTHWORM, ItemDict.CAVEWORM, ItemDict.SOLDIER_ANT, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.LEMON_SHORTCAKE, ItemDict.MOUNTAIN_WHEAT, ItemDict.BUTTER, ItemDict.LEMON, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.LUMINOUS_STEW, ItemDict.FIREFLY, ItemDict.LANTERN_MOTH, ItemDict.MILK, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.LICHEN_JUICE, ItemDict.EMERALD_MOSS, ItemDict.EMERALD_MOSS, ItemDict.CAVE_FUNGI, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.REJUVENATION_TEA, ItemDict.NETTLES, ItemDict.NETTLES, ItemDict.SPICY_LEAF, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.WATERMELON_ICE, ItemDict.SNOW_CRYSTAL, ItemDict.SNOW_CRYSTAL, ItemDict.WATERMELON_SLICE, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.BERRY_MILKSHAKE, ItemDict.BLACKBERRY, ItemDict.MILK, ItemDict.SNOW_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.MINT_CHOCO_BAR, ItemDict.MINT_EXTRACT, ItemDict.CACAO_BEAN, ItemDict.SNOW_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.AUTUMN_MASH, ItemDict.ORANGE, ItemDict.PERSIMMON, ItemDict.PERSIMMON, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.LETHAL_SASHIMI, ItemDict.PUFFERFISH, ItemDict.SHIITAKE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.LETHAL_SASHIMI, ItemDict.WHITE_BLOWFISH, ItemDict.SHIITAKE, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.HOMESTYLE_JELLY, ItemDict.STRAWBERRY, ItemDict.RASPBERRY, ItemDict.RASPBERRY, CookingRecipe.LengthEnum.NONE));
            TABLE_RECIPES.Add(new CookingRecipe(ItemDict.BANANA_SUNDAE, ItemDict.BANANA, ItemDict.SNOW_CRYSTAL, ItemDict.CACAO_BEAN, CookingRecipe.LengthEnum.NONE));

            UnlockRecipe(ItemDict.CHEST);
            UnlockRecipe(ItemDict.TORCH);

            PERFUMERY_RECIPE_DICT = new Dictionary<Item, Dictionary<Item, Item>>
            {
                { ItemDict.MARIGOLD, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.FLORAL_PERFUME },
                    { ItemDict.LAVENDER, ItemDict.SUMMERS_GIFT },
                    { ItemDict.WINTERGREEN, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.BLUEBELL, ItemDict.WARM_MEMORIES },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.RED_ANGEL },
                    { ItemDict.WILD_HONEY, ItemDict.SWEET_BREEZE },
                    { ItemDict.SASSAFRAS, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SUNFLOWER, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SNOWDROP, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.RED_GINGER, ItemDict.SUMMERS_GIFT } }
                },
                { ItemDict.LAVENDER, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.SUMMERS_GIFT },
                    { ItemDict.LAVENDER, ItemDict.FLORAL_PERFUME },
                    { ItemDict.WINTERGREEN, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.BLUEBELL, ItemDict.WARM_MEMORIES },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.SUMMERS_GIFT },
                    { ItemDict.WILD_HONEY, ItemDict.SWEET_BREEZE },
                    { ItemDict.SASSAFRAS, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SUNFLOWER, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SNOWDROP, ItemDict.AUTUMNS_KISS },
                    { ItemDict.RED_GINGER, ItemDict.SUMMERS_GIFT } }
                },
                { ItemDict.WINTERGREEN, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.LAVENDER, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.WINTERGREEN, ItemDict.MINT_EXTRACT },
                    { ItemDict.BLUEBELL, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.VANILLA_BEAN, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.SKY_ROSE, ItemDict.BLISSFUL_SKY },
                    { ItemDict.WILD_HONEY, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.SASSAFRAS, ItemDict.BLISSFUL_SKY },
                    { ItemDict.SUNFLOWER, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.SNOWDROP, ItemDict.SNOW_CRYSTAL },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
                { ItemDict.BLUEBELL, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.WARM_MEMORIES },
                    { ItemDict.LAVENDER, ItemDict.WARM_MEMORIES },
                    { ItemDict.WINTERGREEN, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.BLUEBELL, ItemDict.FLORAL_PERFUME },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.WARM_MEMORIES },
                    { ItemDict.WILD_HONEY, ItemDict.WARM_MEMORIES },
                    { ItemDict.SASSAFRAS, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SUNFLOWER, ItemDict.WARM_MEMORIES },
                    { ItemDict.SNOWDROP, ItemDict.BLISSFUL_SKY },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
                { ItemDict.VANILLA_BEAN, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.SWEET_BREEZE },
                    { ItemDict.LAVENDER, ItemDict.SWEET_BREEZE },
                    { ItemDict.WINTERGREEN, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.BLUEBELL, ItemDict.SWEET_BREEZE },
                    { ItemDict.VANILLA_BEAN, ItemDict.VANILLA_EXTRACT },
                    { ItemDict.SKY_ROSE, ItemDict.SWEET_BREEZE },
                    { ItemDict.WILD_HONEY, ItemDict.SWEET_BREEZE },
                    { ItemDict.SASSAFRAS, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SUNFLOWER, ItemDict.SWEET_BREEZE },
                    { ItemDict.SNOWDROP, ItemDict.SWEET_BREEZE },
                    { ItemDict.RED_GINGER, ItemDict.SWEET_BREEZE } }
                },
                { ItemDict.SKY_ROSE, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.RED_ANGEL },
                    { ItemDict.LAVENDER, ItemDict.SUMMERS_GIFT },
                    { ItemDict.WINTERGREEN, ItemDict.BLISSFUL_SKY },
                    { ItemDict.BLUEBELL, ItemDict.WARM_MEMORIES },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.FLORAL_PERFUME },
                    { ItemDict.WILD_HONEY, ItemDict.SWEET_BREEZE },
                    { ItemDict.SASSAFRAS, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SUNFLOWER, ItemDict.BLISSFUL_SKY },
                    { ItemDict.SNOWDROP, ItemDict.BLISSFUL_SKY },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
                { ItemDict.WILD_HONEY, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.SWEET_BREEZE },
                    { ItemDict.LAVENDER, ItemDict.SWEET_BREEZE },
                    { ItemDict.WINTERGREEN, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.BLUEBELL, ItemDict.WARM_MEMORIES },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.SWEET_BREEZE },
                    { ItemDict.WILD_HONEY, ItemDict.WILD_HONEY },
                    { ItemDict.SASSAFRAS, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SUNFLOWER, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SNOWDROP, ItemDict.FLORAL_PERFUME },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
                { ItemDict.SASSAFRAS, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.AUTUMNS_KISS },
                    { ItemDict.LAVENDER, ItemDict.AUTUMNS_KISS },
                    { ItemDict.WINTERGREEN, ItemDict.BLISSFUL_SKY },
                    { ItemDict.BLUEBELL, ItemDict.AUTUMNS_KISS },
                    { ItemDict.VANILLA_BEAN, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SKY_ROSE, ItemDict.AUTUMNS_KISS },
                    { ItemDict.WILD_HONEY, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SASSAFRAS, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SUNFLOWER, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SNOWDROP, ItemDict.AUTUMNS_KISS },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
                { ItemDict.SUNFLOWER, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.FLORAL_PERFUME },
                    { ItemDict.LAVENDER, ItemDict.FLORAL_PERFUME },
                    { ItemDict.WINTERGREEN, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.BLUEBELL, ItemDict.WARM_MEMORIES },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.BLISSFUL_SKY },
                    { ItemDict.WILD_HONEY, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SASSAFRAS, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SUNFLOWER, ItemDict.OIL },
                    { ItemDict.SNOWDROP, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
                { ItemDict.SNOWDROP, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.LAVENDER, ItemDict.AUTUMNS_KISS },
                    { ItemDict.WINTERGREEN, ItemDict.SNOW_CRYSTAL },
                    { ItemDict.BLUEBELL, ItemDict.BLISSFUL_SKY },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.BLISSFUL_SKY },
                    { ItemDict.WILD_HONEY, ItemDict.FLORAL_PERFUME },
                    { ItemDict.SASSAFRAS, ItemDict.AUTUMNS_KISS },
                    { ItemDict.SUNFLOWER, ItemDict.BIZARRE_PERFUME },
                    { ItemDict.SNOWDROP, ItemDict.SNOW_CRYSTAL },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
                { ItemDict.RED_GINGER, new Dictionary<Item, Item> {
                    { ItemDict.MARIGOLD, ItemDict.SUMMERS_GIFT },
                    { ItemDict.LAVENDER, ItemDict.SUMMERS_GIFT },
                    { ItemDict.WINTERGREEN, ItemDict.OCEAN_GUST },
                    { ItemDict.BLUEBELL, ItemDict.OCEAN_GUST },
                    { ItemDict.VANILLA_BEAN, ItemDict.SWEET_BREEZE },
                    { ItemDict.SKY_ROSE, ItemDict.OCEAN_GUST },
                    { ItemDict.WILD_HONEY, ItemDict.OCEAN_GUST },
                    { ItemDict.SASSAFRAS, ItemDict.OCEAN_GUST },
                    { ItemDict.SUNFLOWER, ItemDict.OCEAN_GUST },
                    { ItemDict.SNOWDROP, ItemDict.OCEAN_GUST },
                    { ItemDict.RED_GINGER, ItemDict.OCEAN_GUST } }
                },
            };

            //alchemy recipes
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SKY_ELEMENT, ItemDict.CREAM, ItemDict.SKY_PIKE, ItemDict.WIND_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SEA_ELEMENT, ItemDict.CREAM, ItemDict.STORMBRINGER_KOI, ItemDict.WATER_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.LAND_ELEMENT, ItemDict.CREAM, ItemDict.OYSTER_MUSHROOM, ItemDict.EARTH_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.PRIMEVAL_ELEMENT, ItemDict.CREAM, ItemDict.GOLD_BAR, ItemDict.FIRE_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.BLACK_CANDLE, ItemDict.BEESWAX, ItemDict.BLACK_FEATHER, ItemDict.BAT_WING, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SALTED_CANDLE, ItemDict.BEESWAX, ItemDict.WHITE_FEATHER, ItemDict.SARDINE, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SPICED_CANDLE, ItemDict.BEESWAX, ItemDict.BLUE_FEATHER, ItemDict.SPICY_LEAF, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.MOSS_BOTTLE, ItemDict.RASPBERRY, ItemDict.EMERALD_MOSS, ItemDict.CAVEFISH, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SHIMMERING_SALVE, ItemDict.CREAM, ItemDict.GOLDEN_EGG, ItemDict.GOLDEN_WOOL, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.VOODOO_STEW, ItemDict.MILK, ItemDict.SEAWEED, ItemDict.SPICY_LEAF, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SOOTHE_CANDLE, ItemDict.BEESWAX, ItemDict.EMERALD_MOSS, ItemDict.CAVE_SOYBEAN, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.BURST_STONE, ItemDict.SNOW_CRYSTAL, ItemDict.FIRE_CRYSTAL, ItemDict.EARTH_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.UNSTABLE_LIQUID, ItemDict.QUARTZ, ItemDict.WATER_CRYSTAL, ItemDict.WIND_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.TROPICAL_BOTTLE, ItemDict.BLUEBERRY, ItemDict.COCONUT, ItemDict.PINEAPPLE, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.PHILOSOPHERS_STONE, ItemDict.MYTHRIL_BAR, ItemDict.GOLDEN_WOOL, ItemDict.DIAMOND, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.MYTHRIL_BAR, ItemDict.SCRAP_IRON, ItemDict.IRON_BAR, ItemDict.PHILOSOPHERS_STONE, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.GOLD_BAR, ItemDict.IRON_ORE, ItemDict.IRON_BAR, ItemDict.PHILOSOPHERS_STONE, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.ADAMANTITE_BAR, ItemDict.MYTHRIL_ORE, ItemDict.MYTHRIL_BAR, ItemDict.PHILOSOPHERS_STONE, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.IMPERIAL_INCENSE, ItemDict.BAMBOO, ItemDict.SKY_ROSE, ItemDict.GOLDEN_LEAF, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SWEET_INCENSE, ItemDict.BAMBOO, ItemDict.MARIGOLD, ItemDict.VANILLA_EXTRACT, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.LAVENDER_INCENSE, ItemDict.BAMBOO, ItemDict.LAVENDER, ItemDict.LAVENDER, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.COLD_INCENSE, ItemDict.BAMBOO, ItemDict.WINTERGREEN, ItemDict.MINT_EXTRACT, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.FRESH_INCENSE, ItemDict.BAMBOO, ItemDict.SNOWDROP, ItemDict.PERSIMMON, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SUGAR_CANDLE, ItemDict.BEESWAX, ItemDict.RED_FEATHER, ItemDict.PINEAPPLE, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.SKY_BOTTLE, ItemDict.BLACKBERRY, ItemDict.WHITE_BLOWFISH, ItemDict.CLOUD_FLOUNDER, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.HEART_VESSEL, ItemDict.SKY_ROSE, ItemDict.GOLDEN_EGG, ItemDict.OLIVE, CookingRecipe.LengthEnum.NONE));
            ALCHEMY_RECIPES.Add(new CookingRecipe(ItemDict.INVINCIROID, ItemDict.CREAM, ItemDict.POLLEN_PUFF, ItemDict.ROYAL_JELLY, CookingRecipe.LengthEnum.NONE));

            //accessory recipes
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.ROYAL_CREST, ItemDict.AMETHYST, ItemDict.ADAMANTITE_BAR, ItemDict.QUEENS_STINGER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.MIDIAN_SYMBOL, ItemDict.AQUAMARINE, ItemDict.ADAMANTITE_BAR, ItemDict.QUEEN_AROWANA, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.UNITY_CREST, ItemDict.DIAMOND, ItemDict.ADAMANTITE_BAR, ItemDict.WIND_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.COMPRESSION_CREST, ItemDict.EARTH_CRYSTAL, ItemDict.ADAMANTITE_BAR, ItemDict.STAG_BEETLE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.POLYMORPH_CREST, ItemDict.EMERALD, ItemDict.ADAMANTITE_BAR, ItemDict.EMPRESS_BUTTERFLY, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.DASHING_CREST, ItemDict.OPAL, ItemDict.ADAMANTITE_BAR, ItemDict.PRISMATIC_FEATHER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.FROZEN_CREST, ItemDict.QUARTZ, ItemDict.ADAMANTITE_BAR, ItemDict.ICE_NINE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.MUTATING_CREST, ItemDict.RUBY, ItemDict.ADAMANTITE_BAR, ItemDict.FIRE_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.MYTHICAL_CREST, ItemDict.SAPPHIRE, ItemDict.ADAMANTITE_BAR, ItemDict.FAIRY_DUST, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.VAMPYRIC_CREST, ItemDict.TOPAZ, ItemDict.ADAMANTITE_BAR, ItemDict.ALBINO_WING, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.BREWERY_CREST, ItemDict.WATER_CRYSTAL, ItemDict.ADAMANTITE_BAR, ItemDict.EMPEROR_SALMON, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.CLOUD_CREST, ItemDict.WIND_CRYSTAL, ItemDict.ADAMANTITE_BAR, ItemDict.CLOUD_FLOUNDER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.PHILOSOPHERS_CREST, ItemDict.FIRE_CRYSTAL, ItemDict.ADAMANTITE_BAR, ItemDict.SHIMMERING_SALVE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.BUTTERFLY_CHARM, ItemDict.AMETHYST, ItemDict.CLAY, ItemDict.YELLOW_BUTTERFLY, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.DROPLET_CHARM, ItemDict.AQUAMARINE, ItemDict.CLAY, ItemDict.LAKE_TROUT, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SNOUT_CHARM, ItemDict.EMERALD, ItemDict.CLAY, ItemDict.TRUFFLE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SUNFLOWER_CHARM, ItemDict.OPAL, ItemDict.CLAY, ItemDict.SUNFLOWER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SALTY_CHARM, ItemDict.QUARTZ, ItemDict.CLAY, ItemDict.SEAWEED, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SPINED_CHARM, ItemDict.SAPPHIRE, ItemDict.CLAY, ItemDict.CACTUS, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.MANTLE_CHARM, ItemDict.TOPAZ, ItemDict.CLAY, ItemDict.QUALITY_COMPOST, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.DANDYLION_CHARM, ItemDict.WIND_CRYSTAL, ItemDict.CLAY, ItemDict.WEEDS, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.CHURN_CHARM, ItemDict.DIAMOND, ItemDict.CLAY, ItemDict.CHEESE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.PRIMAL_CHARM, ItemDict.EARTH_CRYSTAL, ItemDict.CLAY, ItemDict.WILD_MEAT, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SUNRISE_CHARM, ItemDict.FIRE_CRYSTAL, ItemDict.CLAY, ItemDict.RICE_GRASSHOPPER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.VOLCANIC_CHARM, ItemDict.RUBY, ItemDict.CLAY, ItemDict.FIRE_CRYSTAL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.MUSHY_CHARM, ItemDict.WATER_CRYSTAL, ItemDict.CLAY, ItemDict.CLAY_BALL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.OCEANIC_RING, ItemDict.SAPPHIRE, ItemDict.GOLD_BAR, ItemDict.GREAT_WHITE_SHARK, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.BLIND_RING, ItemDict.AQUAMARINE, ItemDict.GOLD_BAR, ItemDict.CAVEFISH, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.FLIGHT_RING, ItemDict.DIAMOND, ItemDict.GOLD_BAR, ItemDict.RED_FEATHER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.GLIMMER_RING, ItemDict.EMERALD, ItemDict.GOLD_BAR, ItemDict.JEWEL_SPIDER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.MONOCULTURE_RING, ItemDict.FIRE_CRYSTAL, ItemDict.GOLD_BAR, ItemDict.GOLDEN_SPINACH, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.LUMBER_RING, ItemDict.OPAL, ItemDict.GOLD_BAR, ItemDict.HARDWOOD, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.BAKERY_RING, ItemDict.QUARTZ, ItemDict.GOLD_BAR, ItemDict.SALT_SHARDS, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.ROSE_RING, ItemDict.RUBY, ItemDict.GOLD_BAR, ItemDict.SKY_ROSE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SHELL_RING, ItemDict.WATER_CRYSTAL, ItemDict.GOLD_BAR, ItemDict.SEA_TURTLE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.FURNACE_RING, ItemDict.WIND_CRYSTAL, ItemDict.GOLD_BAR, ItemDict.INFERNAL_SHARK, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.LUMINOUS_RING, ItemDict.AMETHYST, ItemDict.GOLD_BAR, ItemDict.FIREFLY, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.FLORAL_RING, ItemDict.EARTH_CRYSTAL, ItemDict.GOLD_BAR, ItemDict.SKY_ROSE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.MUSICBOX_RING, ItemDict.TOPAZ, ItemDict.GOLD_BAR, ItemDict.BROWN_CICADA, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.ACID_BRACER, ItemDict.AMETHYST, ItemDict.IRON_BAR, ItemDict.LEMON, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.URCHIN_BRACER, ItemDict.AQUAMARINE, ItemDict.IRON_BAR, ItemDict.SEA_URCHIN, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.FLUFFY_BRACER, ItemDict.DIAMOND, ItemDict.IRON_BAR, ItemDict.WOOL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.DRUID_BRACER, ItemDict.EARTH_CRYSTAL, ItemDict.IRON_BAR, ItemDict.OLIVE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.TRADITION_BRACER, ItemDict.EMERALD, ItemDict.IRON_BAR, ItemDict.MAIZE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SANDSTORM_BRACER, ItemDict.FIRE_CRYSTAL, ItemDict.IRON_BAR, ItemDict.CLAY_DOLL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.DWARVEN_CHILDS_BRACER, ItemDict.OPAL, ItemDict.IRON_BAR, ItemDict.SCRAP_IRON, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.CARNIVORE_BRACER, ItemDict.RUBY, ItemDict.IRON_BAR, ItemDict.WILD_MEAT, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.PURIFICATION_BRACER, ItemDict.SAPPHIRE, ItemDict.IRON_BAR, ItemDict.CLAM, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SCRAP_BRACER, ItemDict.TOPAZ, ItemDict.IRON_BAR, ItemDict.SCRAP_IRON, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.ESSENCE_BRACER, ItemDict.WIND_CRYSTAL, ItemDict.IRON_BAR, ItemDict.BLACK_FEATHER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.STRIPED_BRACER, ItemDict.QUARTZ, ItemDict.IRON_BAR, ItemDict.STINGER_HORNET, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.PIN_BRACER, ItemDict.WATER_CRYSTAL, ItemDict.IRON_BAR, ItemDict.PUFFERFISH, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.CYCLE_PENDANT, ItemDict.EARTH_CRYSTAL, ItemDict.MYTHRIL_BAR, ItemDict.LUNAR_WHALE, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.SOUND_PENDANT, ItemDict.AQUAMARINE, ItemDict.MYTHRIL_BAR, ItemDict.FLAWLESS_CONCH, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.EROSION_PENDANT, ItemDict.EMERALD, ItemDict.MYTHRIL_BAR, ItemDict.PEARL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.POLYCULTURE_PENDANT, ItemDict.FIRE_CRYSTAL, ItemDict.MYTHRIL_BAR, ItemDict.GOLDEN_PUMPKIN, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.LADYBUG_PENDANT, ItemDict.QUARTZ, ItemDict.MYTHRIL_BAR, ItemDict.PINK_LADYBUG, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.STREAMLINE_PENDANT, ItemDict.WATER_CRYSTAL, ItemDict.MYTHRIL_BAR, ItemDict.RED_SNAPPER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.TORNADO_PENDANT, ItemDict.WIND_CRYSTAL, ItemDict.MYTHRIL_BAR, ItemDict.STORMBRINGER_KOI, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.DISSECTION_PENDANT, ItemDict.AMETHYST, ItemDict.MYTHRIL_BAR, ItemDict.EARTHWORM, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.GAIA_PENDANT, ItemDict.DIAMOND, ItemDict.MYTHRIL_BAR, ItemDict.DARK_ANGLER, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.CONTRACT_PENDANT, ItemDict.OPAL, ItemDict.MYTHRIL_BAR, ItemDict.SOLDIER_ANT, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.DYNAMITE_PENDANT, ItemDict.RUBY, ItemDict.MYTHRIL_BAR, ItemDict.COAL, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.OILY_PENDANT, ItemDict.SAPPHIRE, ItemDict.MYTHRIL_BAR, ItemDict.CAVE_FUNGI, CookingRecipe.LengthEnum.NONE));
            ACCESSORY_RECIPES.Add(new CookingRecipe(ItemDict.NEUTRALIZED_PENDANT, ItemDict.TOPAZ, ItemDict.MYTHRIL_BAR, ItemDict.CAVEWORM, CookingRecipe.LengthEnum.NONE));

        }

        public static void UnlockRecipe(Item[] items)
        {
            foreach(Item item in items)
            {
                UnlockRecipe(item);
            }
        }

        public static void UnlockRecipe(Item item)
        {
            bool success = false;
            foreach(CraftingRecipe r in CLOTHING_RECIPES)
            {
                if(r.result.GetItem() == item)
                {
                    r.haveBlueprint = true;
                    success =  true;
                }
            }
            foreach (CraftingRecipe r in MACHINE_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    r.haveBlueprint = true;
                    success = true;
                }
            }
            foreach (CraftingRecipe r in SCAFFOLDING_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    r.haveBlueprint = true;
                    success = true;
                }
            }
            foreach (CraftingRecipe r in WALL_FLOOR_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    r.haveBlueprint = true;
                    success = true;
                }
            }
            foreach (CraftingRecipe r in FURNITURE_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    r.haveBlueprint = true;
                    success = true;
                }
            }
            if(!success)
            {
                throw new Exception("Couldn't unlock recipe?");
            }
        }

        public CraftingRecipe GetRecipeFor(Item item)
        {
            foreach (CraftingRecipe r in CLOTHING_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    return r;
                }
            }
            foreach (CraftingRecipe r in MACHINE_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    return r;
                }
            }
            foreach (CraftingRecipe r in SCAFFOLDING_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    return r;
                }
            }
            foreach (CraftingRecipe r in WALL_FLOOR_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    return r;
                }
            }
            foreach (CraftingRecipe r in FURNITURE_RECIPES)
            {
                if (r.result.GetItem() == item)
                {
                    return r;
                }
            }
            return null;
        }

        public static void UnlockAllRecipes()
        {
            foreach (CraftingRecipe r in CLOTHING_RECIPES)
            {
                r.haveBlueprint = true;
            }
            foreach (CraftingRecipe r in MACHINE_RECIPES)
            {
                r.haveBlueprint = true;
            }
            foreach (CraftingRecipe r in SCAFFOLDING_RECIPES)
            {
                r.haveBlueprint = true;
            }
            foreach (CraftingRecipe r in WALL_FLOOR_RECIPES)
            {
                r.haveBlueprint = true;
            }
            foreach (CraftingRecipe r in FURNITURE_RECIPES)
            {
                r.haveBlueprint = true;
            }
        }

        public static CraftingRecipe GetMachineRecipe(int i)
        {
            if(i >= 0 && i < MACHINE_RECIPES.Count)
            {
                return MACHINE_RECIPES[i];
            }
            return null;
        }

        public static int NumMachineRecipes()
        {
            return MACHINE_RECIPES.Count;
        }

        public static CraftingRecipe GetScaffoldingRecipe(int i)
        {
            if (i >= 0 && i < SCAFFOLDING_RECIPES.Count)
            {
                return SCAFFOLDING_RECIPES[i];
            }
            return null;
        }

        public static int NumScaffoldingRecipes()
        {
            return SCAFFOLDING_RECIPES.Count;
        }

        public static CraftingRecipe GetFurnitureRecipe(int i)
        {
            if (i >= 0 && i < FURNITURE_RECIPES.Count)
            {
                return FURNITURE_RECIPES[i];
            }
            return null;
        }

        public static int NumFurnitureRecipes()
        {
            return FURNITURE_RECIPES.Count;
        }

        public static CraftingRecipe GetWallFloorRecipe(int i)
        {
            if (i >= 0 && i < WALL_FLOOR_RECIPES.Count)
            {
                return WALL_FLOOR_RECIPES[i];
            }
            return null;
        }

        public static int NumWallFloorRecipes()
        {
            return WALL_FLOOR_RECIPES.Count;
        }

        public static CraftingRecipe GetClothingRecipe(int i)
        {
            if (i >= 0 && i < CLOTHING_RECIPES.Count)
            {
                return CLOTHING_RECIPES[i];
            }
            return null;
        }

        public static int NumClothingRecipes()
        {
            return CLOTHING_RECIPES.Count;
        }

        public static bool CheckFlag(string flag)
        {
            return FLAGS[flag] != 0;
        }

        public static bool ContainsFlag(string flag)
        {
            return FLAGS.ContainsKey(flag);
        }

        public static int GetFlagValue(string flag)
        {
            return FLAGS[flag];
        }

        public static void IncrementFlag(string flag, int amount = 1)
        {
            FLAGS[flag] += amount;
        }

        public static void SetFlag(string flag, bool value)
        {
            FLAGS[flag] = value ? 1 : 0;
        }

        public static void SetFlag(string flag, int value)
        {
            FLAGS[flag] = value;
        }

        public static void FlipFlag(string flag)
        {
            if(FLAGS[flag] == 0)
            {
                FLAGS[flag] = 1;
            } else
            {
                FLAGS[flag] = 0;
            }
        }

        public static SaveState GenerateSave()
        {
            SaveState save = new SaveState(SaveState.Identifier.GAMESTATE);

            foreach(string flag in FLAGS.Keys)
            {
                save.AddData(flag, FLAGS[flag].ToString());
            }

            foreach(string stat in STATISTICS.Keys)
            {
                save.AddData(stat, STATISTICS[stat].ToString());
            }

            for(int i = 0; i < MACHINE_RECIPES.Count; i++)
            {
                save.AddData(MACHINE_RECIPES[i].result.GetItem().GetName(), MACHINE_RECIPES[i].haveBlueprint.ToString());
            }
            for(int i = 0; i < SCAFFOLDING_RECIPES.Count; i++)
            {
                save.AddData(SCAFFOLDING_RECIPES[i].result.GetItem().GetName(), SCAFFOLDING_RECIPES[i].haveBlueprint.ToString());
            }
            for (int i = 0; i < FURNITURE_RECIPES.Count; i++)
            {
                save.AddData(FURNITURE_RECIPES[i].result.GetItem().GetName(), FURNITURE_RECIPES[i].haveBlueprint.ToString());
            }
            for (int i = 0; i < WALL_FLOOR_RECIPES.Count; i++)
            {
                save.AddData(WALL_FLOOR_RECIPES[i].result.GetItem().GetName(), WALL_FLOOR_RECIPES[i].haveBlueprint.ToString());
            }
            for (int i = 0; i < CLOTHING_RECIPES.Count; i++)
            {
                save.AddData(CLOTHING_RECIPES[i].result.GetItem().GetName(), CLOTHING_RECIPES[i].haveBlueprint.ToString());
            }

            foreach (ShrineStatus ss in shrineList)
            {
                foreach (ShrineStatus.RequiredItem ri in ss.GetRequiredItems()) {
                    save.AddData(ss.GetID() + ri.item.GetName(), ri.amountHave.ToString());
                }
            }

            for(int i = 0; i < LOST_ITEMS.Count; i++)
            {
                save.AddData("LOSTITEM_" + i, LOST_ITEMS[i].GetName());
            }

            save.AddData("LandElementX", LAND_ELEMENT_X.ToString());
            save.AddData("LandElementY", LAND_ELEMENT_Y.ToString());
            save.AddData("LandElementArea", LAND_ELEMENT_AREA.ToString());
            save.AddData("LandElementPrimed", LAND_ELEMENT_PRIMED.ToString());
            
            return save;
        }

        private static CookingRecipe GetCookingRecipeFromList(Item ing1, Item ing2, Item ing3, List<CookingRecipe> recipeList)
        {
            foreach (CookingRecipe recipe in recipeList)
            {
                List<Item> inputList = new List<Item> { ing1, ing2, ing3 };
                if (inputList.Contains(recipe.ingredient1))
                {
                    inputList.Remove(recipe.ingredient1);
                }
                if (inputList.Contains(recipe.ingredient2))
                {
                    inputList.Remove(recipe.ingredient2);
                }
                if (inputList.Contains(recipe.ingredient3))
                {
                    inputList.Remove(recipe.ingredient3);
                }

                if (inputList.Count == 0)
                {
                    return recipe;
                }

            }
            return null;
        }

        public static CraftingRecipe GetCraftingRecipeForResult(Item result)
        {
            foreach(CraftingRecipe recipe in MACHINE_RECIPES)
            {
                if(recipe.result.GetItem() == result)
                {
                    return recipe;
                }
            }
            foreach (CraftingRecipe recipe in FURNITURE_RECIPES)
            {
                if (recipe.result.GetItem() == result)
                {
                    return recipe;
                }
            }
            foreach (CraftingRecipe recipe in CLOTHING_RECIPES)
            {
                if (recipe.result.GetItem() == result)
                {
                    return recipe;
                }
            }
            foreach (CraftingRecipe recipe in SCAFFOLDING_RECIPES)
            {
                if (recipe.result.GetItem() == result)
                {
                    return recipe;
                }
            }
            foreach (CraftingRecipe recipe in WALL_FLOOR_RECIPES)
            {
                if (recipe.result.GetItem() == result)
                {
                    return recipe;
                }
            }
            return null;
        }

        public static CookingRecipe GetCookingRecipeForResult(Item result)
        {
            foreach(CookingRecipe ovenRecipe in OVEN_RECIPES)
            {
                if(ovenRecipe.result == result)
                {
                    return ovenRecipe;
                }
            }
            foreach(CookingRecipe tableRecipe in TABLE_RECIPES)
            {
                if(tableRecipe.result == result)
                {
                    return tableRecipe;
                }
            }

            return null;
        }

        public static CookingRecipe GetAlchemyRecipeForResult(Item result)
        {
            foreach (CookingRecipe recipe in ALCHEMY_RECIPES)
            {
                if (recipe.result == result)
                {
                    return recipe;
                }
            }
            return null;
        }

        public static CookingRecipe GetAccessoryRecipeForResult(Item result)
        {
            foreach(CookingRecipe recipe in ACCESSORY_RECIPES)
            {
                if(recipe.result == result)
                {
                    return recipe;
                }
            }
            return null;
        }

        public static CookingRecipe GetTableCookingRecipe(Item ing1, Item ing2, Item ing3)
        {
            return GetCookingRecipeFromList(ing1, ing2, ing3, TABLE_RECIPES);
        }

        public static CookingRecipe GetOvenCookingRecipe(Item ing1, Item ing2, Item ing3)
        {
            return GetCookingRecipeFromList(ing1, ing2, ing3, OVEN_RECIPES);
        }

        public static CookingRecipe GetAlchemyRecipe(Item ing1, Item ing2, Item ing3)
        {
            return GetCookingRecipeFromList(ing1, ing2, ing3, ALCHEMY_RECIPES);
        }

        public static CookingRecipe GetAccessoryRecipe(Item ing1, Item ing2, Item ing3)
        {
            return GetCookingRecipeFromList(ing1, ing2, ing3, ACCESSORY_RECIPES);
        }

        public static void LoadSave(SaveState save)
        {
            List<string> flagString = new List<string>();
            foreach(string flag in FLAGS.Keys)
            {
                flagString.Add(flag);
            }
            foreach (string flag in flagString)
            {
                FLAGS[flag] = Int32.Parse(save.TryGetData(flag, "0"));
            }

            List<string> statString = new List<string>();
            foreach(string stat in STATISTICS.Keys)
            {
                statString.Add(stat);
            }
            foreach(string stat in statString)
            {
                STATISTICS[stat] = Double.Parse(save.TryGetData(stat, "0"));
            }

            for(int i = 0; i < MACHINE_RECIPES.Count; i++)
            {
                MACHINE_RECIPES[i].haveBlueprint = save.TryGetData(MACHINE_RECIPES[i].result.GetItem().GetName(), false.ToString()) == true.ToString();
            }
            for (int i = 0; i < SCAFFOLDING_RECIPES.Count; i++)
            {
                SCAFFOLDING_RECIPES[i].haveBlueprint = save.TryGetData(SCAFFOLDING_RECIPES[i].result.GetItem().GetName(), false.ToString()) == true.ToString();
            }
            for (int i = 0; i < FURNITURE_RECIPES.Count; i++)
            {
                FURNITURE_RECIPES[i].haveBlueprint = save.TryGetData(FURNITURE_RECIPES[i].result.GetItem().GetName(), false.ToString()) == true.ToString();
            }
            for (int i = 0; i < WALL_FLOOR_RECIPES.Count; i++)
            {
                WALL_FLOOR_RECIPES[i].haveBlueprint = save.TryGetData(WALL_FLOOR_RECIPES[i].result.GetItem().GetName(), false.ToString()) == true.ToString();
            }
            for (int i = 0; i < CLOTHING_RECIPES.Count; i++)
            {
                CLOTHING_RECIPES[i].haveBlueprint = save.TryGetData(CLOTHING_RECIPES[i].result.GetItem().GetName(), false.ToString()) == true.ToString();
            }

            foreach(ShrineStatus ss in shrineList)
            {
                foreach(ShrineStatus.RequiredItem ri in ss.GetRequiredItems())
                {
                    ri.amountHave = Int32.Parse(save.TryGetData(ss.GetID() + ri.item.GetName(), "0"));
                }
            }

            int j = 0;
            while(!save.TryGetData("LOSTITEM_" + j, "DONE").Equals("DONE"))
            {
                LOST_ITEMS.Add(ItemDict.GetItemByName(save.TryGetData("LOSTITEM_" + j, "ERROR")));
                j++;
            }

            LAND_ELEMENT_X = float.Parse(save.TryGetData("LandElementX", "0"));
            LAND_ELEMENT_Y = float.Parse(save.TryGetData("LandElementY", "0"));
            LAND_ELEMENT_AREA = save.TryGetData("LandElementArea", "0");
            LAND_ELEMENT_PRIMED = save.TryGetData("LandElementPrimed", "false") == true.ToString();
        }
    }
}
