using Microsoft.Xna.Framework;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateau.Entities
{
    public class TEntityMarketStall : TEntityVendor, ITickDaily
    {
        private static string EMPTY_LOOP = "empty";
        private static string FULL_LOOP = "full";

        private static List<Item> rareItems;
        private bool costsTrilobites;

        public enum StallType
        {
            SPIRIT_FISH, SPIRIT_INSECT, SPIRIT_FARM, SPIRIT_MINERAL, 
            FARMERS_FARM_SPRING, FARMERS_FARM_SUMMER, FARMERS_FARM_AUTUMN, FARMERS_JAM, FARMERS_BUTCHER, FARMERS_FISH, FARMERS_MEDIUMS, FARMERS_FORAGE_SPRING, FARMERS_FORAGE_SUMMER, FARMERS_FORAGE_AUTUMN, FARMERS_FORAGE_WINTER, FARMERS_CLOTHING, FARMERS_PAINTINGS, FARMERS_DYES
        }

        public TEntityMarketStall(Vector2 tilePosition, AnimatedSprite sprite, StallType stallType) : base(tilePosition, sprite)
        {
            costsTrilobites = false;

            if(rareItems == null)
            {
                rareItems = new List<Item>
                {
                    ItemDict.DARK_ANGLER, ItemDict.EMPEROR_SALMON, ItemDict.GREAT_WHITE_SHARK, ItemDict.INFERNAL_SHARK, ItemDict.LUNAR_WHALE, ItemDict.PEARL,
                    ItemDict.JEWEL_SPIDER, ItemDict.EMPRESS_BUTTERFLY, ItemDict.STAG_BEETLE,
                    ItemDict.ADAMANTITE_BAR, ItemDict.DIAMOND, ItemDict.EMERALD, ItemDict.RUBY, ItemDict.SAPPHIRE, ItemDict.ADAMANTITE_ORE
                };
            }

            switch(stallType)
            {
                case StallType.FARMERS_FARM_SPRING:
                    saleItems.Add(new SaleItem(ItemDict.CARROT, 40, Util.GetSaleValue(ItemDict.CARROT), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Carrots are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SPINACH, 40, Util.GetSaleValue(ItemDict.SPINACH), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Spinach is for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.POTATO, 40, Util.GetSaleValue(ItemDict.POTATO), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Potatoes are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_FARM_SUMMER:
                    saleItems.Add(new SaleItem(ItemDict.ONION, 40, Util.GetSaleValue(ItemDict.ONION), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Onions are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.TOMATO, 40, Util.GetSaleValue(ItemDict.TOMATO), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Tomatoes are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CUCUMBER, 40, Util.GetSaleValue(ItemDict.CUCUMBER), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Cucumbers are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_FARM_AUTUMN:
                    saleItems.Add(new SaleItem(ItemDict.BEET, 40, Util.GetSaleValue(ItemDict.BEET), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Beets are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BELLPEPPER, 40, Util.GetSaleValue(ItemDict.BELLPEPPER), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Bellpeppers are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BROCCOLI, 40, Util.GetSaleValue(ItemDict.BROCCOLI), new DialogueNode("The stall says \"Piper's Vegetables\". Looks like fresh Broccoli is for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_JAM:
                    saleItems.Add(new SaleItem(ItemDict.APPLE_PRESERVES, 10, Util.GetSaleValue(ItemDict.APPLE_PRESERVES), new DialogueNode("The stall says \"Piper's Jams\". Looks like apple preserves are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BANANA_JAM, 10, Util.GetSaleValue(ItemDict.BANANA_JAM), new DialogueNode("The stall says \"Piper's Jams\". Looks like banana...? jam...? is for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.MARMALADE, 10, Util.GetSaleValue(ItemDict.MARMALADE), new DialogueNode("The stall says \"Piper's Jams\". Looks like marmalade for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CHERRY_JELLY, 10, Util.GetSaleValue(ItemDict.CHERRY_JELLY), new DialogueNode("The stall says \"Piper's Jams\". Looks like cherry jelly is for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLACKBERRY_PRESERVES, 10, Util.GetSaleValue(ItemDict.BLACKBERRY_PRESERVES), new DialogueNode("The stall says \"Piper's Jams\". Looks like blackberry preserves are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLUEBERRY_JELLY, 10, Util.GetSaleValue(ItemDict.BLUEBERRY_JELLY), new DialogueNode("The stall says \"Piper's Jams\". Looks like blueberry jelly is for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.STRAWBERRY_BLAST_JAM, 10, Util.GetSaleValue(ItemDict.STRAWBERRY_BLAST_JAM), new DialogueNode("The stall says \"Piper's Jams\". Looks like strawberry jam is for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_BUTCHER:
                    saleItems.Add(new SaleItem(ItemDict.WILD_MEAT, 30, Util.GetSaleValue(ItemDict.WILD_MEAT), new DialogueNode("The stall says \"Nimbus Butcher\". Looks like Troy has wild boar meat for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_FISH:
                    saleItems.Add(new SaleItem(ItemDict.LAKE_TROUT, 10, Util.GetSaleValue(ItemDict.LAKE_TROUT), new DialogueNode("The stall says \"Troy's Catch!\" Looks like Trout are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SALMON, 10, Util.GetSaleValue(ItemDict.SALMON), new DialogueNode("The stall says \"Troy's Catch!\" Looks like Salmon are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.STRIPED_BASS, 10, Util.GetSaleValue(ItemDict.STRIPED_BASS), new DialogueNode("The stall says \"Troy's Catch!\" Looks like Striped Bass are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CARP, 10, Util.GetSaleValue(ItemDict.CARP), new DialogueNode("The stall says \"Troy's Catch!\" Looks like Carp are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.HERRING, 10, Util.GetSaleValue(ItemDict.HERRING), new DialogueNode("The stall says \"Troy's Catch!\" Looks like non-red vHerrings are for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_MEDIUMS:
                    saleItems.Add(new SaleItem(ItemDict.AMETHYST, 1, Util.GetSaleValue(ItemDict.AMETHYST), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like an Amethyst is for sale today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.QUARTZ, 1, Util.GetSaleValue(ItemDict.QUARTZ), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like Quartz are for sale today!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.AQUAMARINE, 1, Util.GetSaleValue(ItemDict.AQUAMARINE), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like Aquarmarine is for sale today!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.TOPAZ, 1, Util.GetSaleValue(ItemDict.TOPAZ), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like Topaz is for sale today!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.WIND_CRYSTAL, 1, Util.GetSaleValue(ItemDict.WIND_CRYSTAL), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like a rare Wind Crystal is for sale today!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.FIRE_CRYSTAL, 1, Util.GetSaleValue(ItemDict.FIRE_CRYSTAL), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like a rare Fire Crystal is for sale today!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.WATER_CRYSTAL, 1, Util.GetSaleValue(ItemDict.WATER_CRYSTAL), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like a rare Water Crystal is for sale today!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.EARTH_CRYSTAL, 1, Util.GetSaleValue(ItemDict.EARTH_CRYSTAL), new DialogueNode("The stall says \"Charlotte's Mediums\". Looks like a rare Earth Crystal is for sale today!", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_FORAGE_SPRING:
                    saleItems.Add(new SaleItem(ItemDict.WEEDS, 25, Util.GetSaleValue(ItemDict.WEEDS), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling... weeds?", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CHICKWEED, 10, Util.GetSaleValue(ItemDict.CHICKWEED), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling foraged chickweed today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.RASPBERRY, 10, Util.GetSaleValue(ItemDict.RASPBERRY), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling raspberries today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.NETTLES, 10, Util.GetSaleValue(ItemDict.NETTLES), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling foraged nettles today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLUEBELL, 10, Util.GetSaleValue(ItemDict.BLUEBELL), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling bluebell flowers today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_FORAGE_SUMMER:
                    saleItems.Add(new SaleItem(ItemDict.MARIGOLD, 10, Util.GetSaleValue(ItemDict.MARIGOLD), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling marigolds today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.LAVENDER, 10, Util.GetSaleValue(ItemDict.LAVENDER), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling lavender today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SUNFLOWER, 10, Util.GetSaleValue(ItemDict.SUNFLOWER), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling sunflowers today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLUEBERRY, 10, Util.GetSaleValue(ItemDict.BLUEBERRY), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling fresh blueberries today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_FORAGE_AUTUMN:
                    saleItems.Add(new SaleItem(ItemDict.WILD_RICE, 10, Util.GetSaleValue(ItemDict.WILD_RICE), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling wild rice today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PERSIMMON, 10, Util.GetSaleValue(ItemDict.PERSIMMON), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling persimmons today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SASSAFRAS, 10, Util.GetSaleValue(ItemDict.SASSAFRAS), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling sassafras today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLACKBERRY, 10, Util.GetSaleValue(ItemDict.BLACKBERRY), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling fresh blackberries today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.ELDERBERRY, 10, Util.GetSaleValue(ItemDict.ELDERBERRY), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling fresh elderberries today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_FORAGE_WINTER:
                    saleItems.Add(new SaleItem(ItemDict.WINTERGREEN, 10, Util.GetSaleValue(ItemDict.WINTERGREEN), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling foraged wintergreen today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CHICORY_ROOT, 10, Util.GetSaleValue(ItemDict.CHICORY_ROOT), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling chicory root today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CHANTERELLE, 10, Util.GetSaleValue(ItemDict.CHANTERELLE), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling foraged chanterelles today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SNOWDROP, 10, Util.GetSaleValue(ItemDict.SNOWDROP), new DialogueNode("The stall says \"Finley's Forage\". Looks like Finley is selling snowdrop flowers today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_CLOTHING:
                    saleItems.Add(new SaleItem(ItemDict.LINEN_BUTTON, 1, Util.GetSaleValue(ItemDict.LINEN_BUTTON), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling a Linen Button today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BUTTON_DOWN, 1, Util.GetSaleValue(ItemDict.BUTTON_DOWN), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling a Button Down today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SWEATER, 1, Util.GetSaleValue(ItemDict.SWEATER), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling a Sweater today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PUFF_SKIRT, 1, Util.GetSaleValue(ItemDict.PUFF_SKIRT), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling a Puff Skirt today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.TURTLENECK, 1, Util.GetSaleValue(ItemDict.TURTLENECK), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling a Turtleneck today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BATHROBE, 1, Util.GetSaleValue(ItemDict.BATHROBE), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling a Bathrobe today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.ASCOT, 1, Util.GetSaleValue(ItemDict.ASCOT), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling an Ascot today.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BUTTERFLY_CLIP, 1, Util.GetSaleValue(ItemDict.BUTTERFLY_CLIP), new DialogueNode("The stall says \"Skye's Threads\". Looks like she's selling a Butterfly Clip today.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_PAINTINGS:
                    String paintingDesc = "The stall says \"Beauty by Theo\"... he's selling something that could generously be described as a painting.";
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_OASIS, 1, 3200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_FOUR, 1, 3200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_FUTURE, 1, 3200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_ARCTIC, 1, 2800, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_FATE, 1, 2800, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_DITHER, 1, 5200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_BEACHDAY, 1, 4200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_RIVER, 1, 4200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_BEDTIME, 1, 4800, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_ILOVEYOU, 1, 4800, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_CHANGES, 1, 4800, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_LIBERTY, 1, 4800, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_INTERLUDE, 1, 4800, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_SKYROSE, 1, 3000, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_EARTH, 1, 3000, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_CALCULATOR, 1, 3000, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_CORAL, 1, 2400, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_SEASONS, 1, 2400, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_WHATEVER, 1, 2400, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_SOLEMN, 1, 3300, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_MOONSET, 1, 3300, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_MONA, 1, 3300, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_OVERHANG, 1, 3300, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_SUNSET, 1, 2200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_SPICE, 1, 2200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_WINDOW, 1, 2200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_ET, 1, 2200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_LAVENDER, 1, 2200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_THREADS, 1, 2200, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_LAUNCH, 1, 2500, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_FABLE, 1, 2500, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_GROWTH, 1, 2500, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_SHADES, 1, 2500, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_GENIUS, 1, 2500, new DialogueNode(paintingDesc, DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PAINTING_RESONANT, 1, 800000000, new DialogueNode("The stall says \"Beauty by Theo\"... he's selling something... at an OUTRAGEOUS price!", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.FARMERS_DYES:
                    saleItems.Add(new SaleItem(ItemDict.RED_DYE, 25, Util.GetSaleValue(ItemDict.RED_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Red.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLUE_DYE, 25, Util.GetSaleValue(ItemDict.BLUE_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Blue.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.NAVY_DYE, 25, Util.GetSaleValue(ItemDict.NAVY_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Navy.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLACK_DYE, 25, Util.GetSaleValue(ItemDict.BLACK_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Black.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.DARK_GREY_DYE, 25, Util.GetSaleValue(ItemDict.DARK_GREY_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Dark Grey.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.LIGHT_BROWN_DYE, 25, Util.GetSaleValue(ItemDict.LIGHT_BROWN_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Light Brown.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.DARK_BROWN_DYE, 25, Util.GetSaleValue(ItemDict.DARK_BROWN_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Dark Brown.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.ORANGE_DYE, 25, Util.GetSaleValue(ItemDict.ORANGE_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Orange.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.YELLOW_DYE, 25, Util.GetSaleValue(ItemDict.YELLOW_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Yellow.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PURPLE_DYE, 25, Util.GetSaleValue(ItemDict.PURPLE_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Purple.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.GREEN_DYE, 25, Util.GetSaleValue(ItemDict.GREEN_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Green.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.OLIVE_DYE, 25, Util.GetSaleValue(ItemDict.OLIVE_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Olive.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.WHITE_DYE, 25, Util.GetSaleValue(ItemDict.WHITE_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is White.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.LIGHT_GREY_DYE, 25, Util.GetSaleValue(ItemDict.LIGHT_GREY_DYE), new DialogueNode("The stall says \"Piper's Inks\". Looks like the color of the day is Light Grey.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.SPIRIT_FISH:
                    costsTrilobites = true;
                    saleItems.Add(new SaleItem(ItemDict.BLACKENED_OCTOPUS, 15, 3, new DialogueNode("You can't find these blackened octopus outside of this region, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BLUEGILL, 15, 1, new DialogueNode("Fresh bluegills are the catch of the day today, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BOXER_LOBSTER, 15, 2, new DialogueNode("Fresh lobster for sale, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CARP, 15, 1, new DialogueNode("Cheap carp available here, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CATFISH, 15, 2, new DialogueNode("I have catfish today, haiku! Good as a pet or deep-fried!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CAVEFISH, 15, 2, new DialogueNode("I have rare cavefish from the caverns below, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CAVERN_TETRA, 15, 2, new DialogueNode("Today's exclusive deal is on cavern tetra, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CLOUD_FLOUNDER, 15, 3, new DialogueNode("This special fish that can fly through the clouds is available now, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CRAB, 15, 1, new DialogueNode("Please take these off my hands. They keep pinching me, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.HERRING, 15, 1, new DialogueNode("Special non-red herrings available now, haiku!!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.INKY_SQUID, 15, 2, new DialogueNode("Squishy squids for sale, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.JUNGLE_PIRANHA, 15, 2, new DialogueNode("Vicious jungle piranhas available for your moat now, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.LAKE_TROUT, 15, 2, new DialogueNode("Freshly caught trout imported from the lower strata, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.MACKEREL, 15, 1, new DialogueNode("Mackerel for sale! Get them while they're fresh, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.MOLTEN_SQUID, 15, 3, new DialogueNode("Deliciously warm squids at deep discounts, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.ONYX_EEL, 15, 3, new DialogueNode("Rare lava eels for sale, you won't find these anywhere else! Get yours now, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PIKE, 15, 3, new DialogueNode("Pikes for sale, haiku! Buy 2 get 2 for the price of 2 flash sale!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PUFFERFISH, 15, 3, new DialogueNode("Spiritplace marketplace cannot be held accountable for any sickness from purchased products, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.RED_SNAPPER, 15, 2, new DialogueNode("You won't find snappers at a better price, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SALMON, 15, 2, new DialogueNode("Flash sale on salmon today only, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SARDINE, 15, 1, new DialogueNode("Sardines for sale, haiku! What they lack in size they make up for in flavor!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SEA_TURTLE, 15, 3, new DialogueNode("Legally dubious endangered turtles available today, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SHRIMP, 15, 1, new DialogueNode("Shrimp for sale! One shrimp per transaction, please!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SKY_PIKE, 15, 3, new DialogueNode("Cloud piercing sky pikes available now!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.STORMBRINGER_KOI, 15, 3, new DialogueNode("Please handle these with gloves today, haiku! The flavor is also electrifying!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.STRIPED_BASS, 15, 1, new DialogueNode("Bass available now for a low, low price! Daily special! Haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SUNFISH, 15, 2, new DialogueNode("One day sale on sunfish, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SWORDFISH, 15, 3, new DialogueNode("Swordfish for sale! Fun for the whole family! I also recommend them for the inlaws, haiku.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.TUNA, 15, 2, new DialogueNode("Fatty tuna available now, haiku! Sushi grade!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.WHITE_BLOWFISH, 15, 3, new DialogueNode("Exclusive blowfish imported from the upper strata, haiku! One bite is one year of good luck!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.DARK_ANGLER, 1, 25, new DialogueNode("Special deal today, haiku! This fish was long thought to be extinct, but here it is in the flesh haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.EMPEROR_SALMON, 1, 25, new DialogueNode("The finest salmon in the world, available right here, right now! Get yours while you can, haiku! They're going fast!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.QUEEN_AROWANA, 1, 20, new DialogueNode("Human! This is a secret deal on the queen of the pond, only for you and only today! Buy buy buy haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.GREAT_WHITE_SHARK, 1, 25, new DialogueNode("Supremely rare shark, imported from lands far below! The finest fish in the world, and at a bargain price haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.INFERNAL_SHARK, 1, 20, new DialogueNode("Ultra exclusive infernal shark available now, to card-holders and humans only! Haiku! Haiku! I'm excited to be selling this one, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.LUNAR_WHALE, 1, 35, new DialogueNode("Extremely rare lunar whale available now! This is the deal of a lifetime, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PEARL, 1, 20, new DialogueNode("Human! I don't usually carry this sort of thing, but today I have a rare pearl! Take a look haiku, the price is a steal!", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.SPIRIT_INSECT:
                    costsTrilobites = true;
                    saleItems.Add(new SaleItem(ItemDict.BANDED_DRAGONFLY, 20, 1, new DialogueNode("Awesome dragonflies for sale! They're like insect dragons, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BROWN_CICADA, 30, 1, new DialogueNode("Look human, today I have musical cicadas! Isn't their song incredible?", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CAVEWORM, 15, 2, new DialogueNode("Special worms from the caves below! Mom grounded me for sneaking off to gather them, but they're available now to you, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.EARTHWORM, 30, 1, new DialogueNode("Worms for sale! Great for making fishing bait, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.FIREFLY, 20, 2, new DialogueNode("Human, look at the way these ones light up! Great for lanterns haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.HONEY_BEE, 30, 1, new DialogueNode("Sweet honey bees for sale! Great price, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.LANTERN_MOTH, 20, 1, new DialogueNode("Special moths available now! You can only find these beauties during the night, but here they are now today!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PINK_LADYBUG, 15, 2, new DialogueNode("Imported pink ladybugs available now! They're an invasive species, and if you don't buy them I'll release them all into the wild, haiku.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.RICE_GRASSHOPPER, 30, 1, new DialogueNode("Grasshoppers for sale! Look at them jump, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SNAIL, 20, 2, new DialogueNode("Shelled snails for sale! Freshly picked from the ground, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SOLDIER_ANT, 30, 1, new DialogueNode("Today I have army ants, haiku. Please handle them with care.", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.YELLOW_BUTTERFLY, 30, 1, new DialogueNode("Butterflies for sale! Perfect for weddings or gifts!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.STINGER_HORNET, 25, 1, new DialogueNode("Hornets for sale! Great for dares or masochists!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.STAG_BEETLE, 1, 25, new DialogueNode("Human, today I brought a rare one! Before you lies the stag beetle, king of the insects! Buy it now, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.JEWEL_SPIDER, 1, 25, new DialogueNode("Behold the jewel of my collection! The legendary jewel spider, for sale today only!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.EMPRESS_BUTTERFLY, 1, 25, new DialogueNode("From the skies above appears the empress butterfly! Buy it while you can!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.WEEDS, 99, 1, new DialogueNode("Sorry, these were all I could find today, haiku...", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.PINECONE, 99, 1, new DialogueNode("Pinecones for sale... sorry, not even I can get excited over these... sigh-ku.", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.SPIRIT_FARM:
                    costsTrilobites = true;
                    saleItems.Add(new SaleItem(ItemDict.BELLPEPPER, 10, 2, new DialogueNode("Ring in this great deal on bellpeppers, haiku! Healthy and fresh!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.BROCCOLI, 10, 2, new DialogueNode("Today we have broccoli for sale, haiku. Buy some, it's good for you!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CACTUS, 10, 2, new DialogueNode("Prickly cactus for sale, haiku! Hard to find elsewhere! You could even make seeds out of it!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CARROT, 10, 2, new DialogueNode("Fresh carrots for sale, haiku! Healthy and pointy!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CUCUMBER, 10, 2, new DialogueNode("The deal of the day is on fine cucumbers, haiku! Only the highest quality for our valued customers!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.ONION, 10, 2, new DialogueNode("Sweet onions available today, haiku! Delicious in stews, soups, and other dishes!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.POTATO, 10, 2, new DialogueNode("Starchy potatoes for sale! You won't find larger ones elsewhere, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.STRAWBERRY, 10, 2, new DialogueNode("Rare strawberries available here only, haiku! Imported from the other side of the mountain! You could make seeds out of these and grow some yourself too, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.CABBAGE, 10, 2, new DialogueNode("Special cabbages for sale, haiku! You could make seeds from them to farm yourself!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.TOMATO, 10, 2, new DialogueNode("Freshly picked tomatoes for sale, haiku! Satisfaction guaranteed, or your trilobites back!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.EGGPLANT, 10, 2, new DialogueNode("Exclusive flash deal! Rare eggplants on sale, haiku! You can turn them into seeds for your own farm, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
                case StallType.SPIRIT_MINERAL:
                    costsTrilobites = true;
                    saleItems.Add(new SaleItem(ItemDict.STONE, 99, 1, new DialogueNode("All we have today is stones, human. No luck in the mines, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SCRAP_IRON, 99, 1, new DialogueNode("Scrap iron for sale! It's only a little bit worse than the normal stuff, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.IRON_ORE, 24, 2, new DialogueNode("Quality iron ores available for purchase! Freshly mined a few cycles ago!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.IRON_BAR, 10, 5, new DialogueNode("Refined iron bars for sale, haiku! Forged in magma by the finest of our blacksmiths!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.GOLD_ORE, 15, 3, new DialogueNode("Today I'm selling nuggets of pure gold! At these prices, it's like I'm putting myself out of business, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.GOLD_BAR, 5, 8, new DialogueNode("Solid 25-carat gold bars for sale! Get your bling while you can, kaching! (haiku)", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.MYTHRIL_ORE, 15, 3, new DialogueNode("Mythril ores for sale, haiku! Mined from the upper strata!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.MYTHRIL_BAR, 5, 8, new DialogueNode("Rare mythril bars for sale, haiku! Have you even seen a material like this before, human?", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.ADAMANTITE_ORE, 6, 10, new DialogueNode("Extremely rare adamantite ores, haiku! Mined from comets, brought directly to you!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.ADAMANTITE_BAR, 2, 25, new DialogueNode("Ultra exclusive adamantite bars for sale, haiku! It's called the strongest material in the world, and you can see for yourself why!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.COAL, 25, 2, new DialogueNode("Find yourself with extra ores and no way to smelt them, haiku? Well struggle no longer, for today I am selling lumps of coal!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.AMETHYST, 3, 5, new DialogueNode("Healing crystals for sale, haiku. Today's amethyst!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.AQUAMARINE, 3, 10, new DialogueNode("Check out these specially blessed gemstones, available today only, haiku!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.QUARTZ, 3, 5, new DialogueNode("Quartz for sale, haiku. You've probably seen some around, but none of this luster!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.TOPAZ, 3, 5, new DialogueNode("Topaz available here only, haiku! Valuable gemstones, but at bargain prices!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.DIAMOND, 1, 50, new DialogueNode("Pure diamond for sale, haiku! Get it quickly before it's gone!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.EMERALD, 1, 40, new DialogueNode("Today I have a true gem, haiku. Behold, this flawless emerald! Get it now or never!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.RUBY, 1, 40, new DialogueNode("Ruby for sale! Red like the sun! Maybe you have a loved one who needs a gift, haiku?", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.SAPPHIRE, 1, 40, new DialogueNode("Look at this lovely blue sapphire, haiku. You won't find a nicer one on this side of the mountain!", DialogueNode.PORTRAIT_SYSTEM)));
                    saleItems.Add(new SaleItem(ItemDict.OPAL, 3, 5, new DialogueNode("Shining opal for sale, haiku. Makes for a great souvenir!", DialogueNode.PORTRAIT_SYSTEM)));
                    break;
            }

            sprite.AddLoop(EMPTY_LOOP, 0, 0, true);
            sprite.AddLoop(FULL_LOOP, 1, 1, true);
            RandomizeStock();
        }

        public void RandomizeStock()
        {
            bool done = false;
            while (!done)
            {
                currentSaleItem = saleItems[Util.RandInt(0, saleItems.Count - 1)];
                if(!rareItems.Contains(currentSaleItem.item))
                {
                    done = true;
                } else if(Util.RandInt(1, 3) == 1)
                {
                    done = true;
                }
            }
            quantityRemaining = currentSaleItem.quantity;
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            if (currentSaleItem.quantity == 0)
            {
                sprite.SetLoopIfNot(EMPTY_LOOP);
            } else
            {
                sprite.SetLoop(FULL_LOOP);
            }
        }
        public override void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            if (!costsTrilobites)
            {
                base.InteractLeftShift(player, area, world);
            }
            else
            {
                if (currentSaleItem != SaleItem.NONE && quantityRemaining > 0)
                {
                    ItemStack cost = new ItemStack(ItemDict.TRILOBITE, currentSaleItem.cost);
                    if (player.HasItemStack(cost))
                    {
                        player.RemoveItemStackFromInventory(cost);

                        area.AddEntity(new EntityItem(currentSaleItem.item, new Vector2(position.X, position.Y - 10)));
                        quantityRemaining--;
                        if (quantityRemaining == 0)
                        {
                            currentSaleItem = SaleItem.NONE;
                        }
                    }
                    else
                    {
                        player.AddNotification(new EntityPlayer.Notification("Not enough trilobites!", Color.Red, EntityPlayer.Notification.Length.SHORT));
                    }
                }
            }
        }

        public override HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (costsTrilobites)
            {
                if (currentSaleItem != SaleItem.NONE)
                {
                    return new HoveringInterface(
                        new HoveringInterface.Row(
                            new HoveringInterface.TextElement(currentSaleItem.item.GetName())),
                        new HoveringInterface.Row(
                            new HoveringInterface.ItemStackElement(currentSaleItem.item)),
                        new HoveringInterface.Row(
                            new HoveringInterface.TextElement("Price: " + currentSaleItem.cost.ToString() + (costsTrilobites ? " Trilobites" : "Gold"))),
                        new HoveringInterface.Row(
                            new HoveringInterface.TextElement("Stock: " + quantityRemaining)));
                }
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.TextElement("Sold Out!")));
            } else
            {
                return base.GetHoveringInterface(player);
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);
            ChangeAnimation();
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            RandomizeStock();
        }
    }
}
