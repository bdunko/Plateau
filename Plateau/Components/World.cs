using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using Plateau.Entities;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class World
    {
        public enum Weather
        {
            SUNNY, CLOUDY, RAINY, SNOWY
        }

        public enum TimeOfDay
        {
            MORNING, DAY, EVENING, NIGHT, ALL
        }

        public class TimeData
        {
            public Season season;
            public int day;
            public int hour, minute;
            public TimeOfDay timeOfDay;

            public TimeData(Season season, int day, int hour, int minute, TimeOfDay timeOfDay)
            {
                this.season = season;
                this.day = day;
                this.hour = hour;
                this.minute = minute;
                this.timeOfDay = timeOfDay;
            }

            public int CalculateGameTime()
            {
                return (60 * hour) + minute;
            }
        }

        public static int MinutesUntilTransition(int currentHour, int currentMinute)
        {
            int hours = 0;

            if (currentHour < MORNING_START_HOUR)
            {
                hours = MORNING_START_HOUR - currentHour;
            }
            else if (currentHour < MORNING_END_HOUR)
            {
                hours = MORNING_END_HOUR - currentHour;
            }
            else if (currentHour < DAY_END_HOUR)
            {
                hours = DAY_END_HOUR - currentHour;
            }
            else if (currentHour < EVENING_END_HOUR)
            {
                hours = EVENING_END_HOUR - currentHour;
            } else
            {
                hours = 24 - currentHour + 7;
            }
            return (hours * 60) - currentMinute;
        }

        public static TimeOfDay NextTimeOfDay(TimeOfDay current)
        {
            switch (current)
            {
                case TimeOfDay.MORNING:
                    return TimeOfDay.DAY;
                case TimeOfDay.DAY:
                    return TimeOfDay.EVENING;
                case TimeOfDay.EVENING:
                    return TimeOfDay.NIGHT;
                default:
                    return TimeOfDay.MORNING;
            }
        }

        public enum Season
        {
            SPRING, SUMMER, AUTUMN, WINTER, DEFER, NONE
        }

        private Season StringToSeason(string str)
        {
            foreach (Season season in Enum.GetValues(typeof(Season)))
            {
                if (season.ToString() == str)
                {
                    return season;
                }
            }

            throw new Exception("No season found for string.");
        }

        private Dictionary<Area.AreaEnum, Area> areas;
        private Area currentArea;
        private Weather currentWeather;
        private float currentTime;
        private int currentDay, currentCycle;
        private List<EntityCharacter> characters;
        private Season currentSeason;
        //1 2 3 4 5 6 7 8 9 10 11 12 1 2 3 4 5 6 7 8 9 10 11 12
        private static float[] hourlyLight = { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
        private static float[] hourlyDark = { 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 1f, 1f, 1f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.95f, 0.90f, 0.85f, 0.75f, 0.55f, 0.45f, 0.4f, 0.35f, 0.25f };
        private static float DAY_STARTING_TIME = 420;
        private static float DAY_ENDING_TIME = 1440;
        private static int DAYS_IN_SEASON = 7;
        public static int MORNING_START_HOUR = 5;
        public static int MORNING_END_HOUR = 9;
        public static int DAY_END_HOUR = 17;
        public static int EVENING_END_HOUR = 20;

        private static int TICK_LENGTH = 5; //5 minutes ingame = 5 seconds
        private float tickTimer = 0.0f;

        private Area transitionToArea;
        private Vector2 transitionToPosition;
        private Area.Waypoint transitionToSpawn;
        private bool paused;

        public static float GRAVITY = 8;

        private GameplayInterface ui;
        private CutsceneManager.Cutscene cutsceneToPlay;

        public World(GameplayInterface ui)
        {
            this.cutsceneToPlay = null;
            this.ui = ui;
            this.currentWeather = Weather.SUNNY;
            paused = false;
            transitionToArea = null;
            transitionToPosition = new Vector2(-100, -100);
            transitionToSpawn = null;
            currentTime = DAY_STARTING_TIME;
            currentDay = 0;
            currentSeason = Season.SPRING;
        }

        public void PlayCutscene(CutsceneManager.Cutscene cutscene)
        {
            this.cutsceneToPlay = cutscene;
        }

        public CutsceneManager.Cutscene GetCutsceneIfPossible(EntityPlayer player)
        {
            if(cutsceneToPlay != null)
            {
                CutsceneManager.Cutscene temp = cutsceneToPlay;
                cutsceneToPlay = null;
                return temp;
            }

            List<CutsceneManager.Cutscene> possibleCutscenes = currentArea.GetPossibleCutscenes(player.GetAdjustedPosition());
            foreach(CutsceneManager.Cutscene possibleCutscene in possibleCutscenes)
            {
                if(possibleCutscene.CheckActivationCondition(player, this))
                {
                    return possibleCutscene;
                }
            }
            return null;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphics, EntityPlayer player, RectangleF cameraBoundingBox)
        {
            characters = new List<EntityCharacter>();

            areas = new Dictionary<Area.AreaEnum, Area>();
            areas[Area.AreaEnum.FARM] = new Area(Area.AreaEnum.FARM, Content.Load<TiledMap>(Paths.MAP_FARM), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, true, true),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, true, true),
                null, null, Paths.MAP_FARM_BASE, null, Paths.MAP_FARM_WALLS, null, null);
            areas[Area.AreaEnum.TOWN] = new Area(Area.AreaEnum.TOWN, Content.Load<TiledMap>(Paths.MAP_TOWN), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, true, true),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, true, true),
                Paths.MAP_TOWN_WATER, Paths.MAP_TOWN_DECORATION_FG, Paths.MAP_TOWN_BASE, Paths.MAP_TOWN_DECORATION, Paths.MAP_TOWN_WALLS, Paths.MAP_TOWN_WATER_BG, Paths.MAP_TOWN_FG_CAVE);
            areas[Area.AreaEnum.INTERIOR] = new Area(Area.AreaEnum.INTERIOR, Content.Load<TiledMap>(Paths.MAP_INTERIOR), false, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, false, true),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, false, true),
                Paths.MAP_INTERIOR_WATER, Paths.MAP_INTERIOR_DECORATION_FG, Paths.MAP_INTERIOR_BASE, Paths.MAP_INTERIOR_DECORATION, Paths.MAP_INTERIOR_WALLS, null, Paths.MAP_INTERIOR_FG_CAVE);

            areas[Area.AreaEnum.BEACH] = new Area(Area.AreaEnum.BEACH, Content.Load<TiledMap>(Paths.MAP_BEACH), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, false, false),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, false, false),
                Paths.MAP_BEACH_WATER, Paths.MAP_BEACH_DECORATION_FG, Paths.MAP_BEACH_BASE, Paths.MAP_BEACH_DECORATION, Paths.MAP_BEACH_WALLS, Paths.MAP_BEACH_WATER_BG, Paths.MAP_BEACH_FG_CAVE);
            areas[Area.AreaEnum.S0] = new Area(Area.AreaEnum.S0, Content.Load<TiledMap>(Paths.MAP_S0), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, true, true),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, true, true),
                Paths.MAP_S0_WATER, Paths.MAP_S0_DECORATION_FG, Paths.MAP_S0_BASE, Paths.MAP_S0_DECORATION, Paths.MAP_S0_WALLS, Paths.MAP_S0_WATER_BG, Paths.MAP_S0_FG_CAVE);
            areas[Area.AreaEnum.S1] = new Area(Area.AreaEnum.S1, Content.Load<TiledMap>(Paths.MAP_S1), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, true, true),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, true, true),
                Paths.MAP_S1_WATER, Paths.MAP_S1_DECORATION_FG, Paths.MAP_S1_BASE, Paths.MAP_S1_DECORATION, Paths.MAP_S1_WALLS, Paths.MAP_S1_WATER_BG, Paths.MAP_S1_FG_CAVE);
            areas[Area.AreaEnum.S2] = new Area(Area.AreaEnum.S2, Content.Load<TiledMap>(Paths.MAP_S2), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, true, true),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, false, false),
                Paths.MAP_S2_WATER, Paths.MAP_S2_DECORATION_FG, Paths.MAP_S2_BASE, Paths.MAP_S2_DECORATION, Paths.MAP_S2_WALLS, Paths.MAP_S2_WATER_BG, Paths.MAP_S2_FG_CAVE);
            areas[Area.AreaEnum.S3] = new Area(Area.AreaEnum.S3, Content.Load<TiledMap>(Paths.MAP_S3), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, true, true),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, true, true),
                Paths.MAP_S3_WATER, Paths.MAP_S3_DECORATION_FG, Paths.MAP_S3_BASE, Paths.MAP_S3_DECORATION, Paths.MAP_S3_WALLS, Paths.MAP_S3_WATER_BG, Paths.MAP_S3_FG_CAVE);

            areas[Area.AreaEnum.S4] = new Area(Area.AreaEnum.S4, Content.Load<TiledMap>(Paths.MAP_S4), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, false, false),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SKY, false, false),
                null, null, null, null, null, null, null);
            areas[Area.AreaEnum.APEX] = new Area(Area.AreaEnum.APEX, Content.Load<TiledMap>(Paths.MAP_APEX), true, graphics, Content, PlateauMain.createContentManager(), player, cameraBoundingBox,
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SPACE, false, false),
                new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.FOREGROUND_SPACE, false, false),
                null, null, null, null, null, null, null);
            //Console.WriteLine("FARM xy: " + areas[0].MapPixelWidth() + "  " + areas[0].MapPixelHeight());

            currentArea = areas[Area.AreaEnum.FARM]; //set starting area...
            currentArea.LoadLayers();

            //load characters!
            //schedule
            //clothing sets

            //List<EntityCharacter.Schedule.Event> rockwellSchedule = Util.GenerateSchedule(
            //    new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.BEACH], areas[Area.AreaEnum.BEACH].GetWaypoint("rockwellSpawn"), 7, 0, 7, 4, trueCondition, 0),
            //    new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("houseWaypoint"), 7, 5, 7, 19, trueCondition, 0),
            //    new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("cabinWaypoint"), 7, 20, 7, 39, trueCondition, 0),
            //    new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("mansionWaypoint"), 7, 40, 7, 59, trueCondition, 0));

            Func<World, EntityCharacter, bool> trueCondition = (world, chara) => { return true; };
            Func<World, EntityCharacter, bool> springCondition = (world, chara) => { return world.GetSeason() == Season.SPRING; };
            Func<World, EntityCharacter, bool> summerCondition = (world, chara) => { return world.GetSeason() == Season.SUMMER; };
            Func<World, EntityCharacter, bool> fallCondition = (world, chara) => { return world.GetSeason() == Season.AUTUMN; };
            Func<World, EntityCharacter, bool> winterCondition = (world, chara) => { return world.GetSeason() == Season.WINTER; };

            //ROCKWELL
            List<EntityCharacter.ClothingSet> rockwellClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> rockwellSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("rockwellHome"), 7, 0, 7, 30, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL),
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.BEACH], areas[Area.AreaEnum.BEACH].GetWaypoint("rockwellDogwalk"), 7, 30, 8, 30, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.MEDIUM),
                new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("rockwellWork"), 8, 30, 19, 0, trueCondition, 0, EntityCharacter.Schedule.StandAtEvent.DirectionBehavior.RIGHT),
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.BEACH], areas[Area.AreaEnum.BEACH].GetWaypoint("rockwellDogwalk"), 19, 0, 20, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.MEDIUM),
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("rockwellHome"), 20, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL));

            List<EntityCharacter.DialogueOption> rockwellDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Rockwell.", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)),
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Rockwell. (In Spring)", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(springCondition), 3));

            characters.Add(new EntityCharacter("Rockwell", this, EntityCharacter.CharacterEnum.ROCKWELL, rockwellClothing, rockwellSchedule, rockwellDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.INTERIOR].GetWaypoint("rockwellHome"),
                GameState.FLAG_LETTER_GIFT_ROCKWELL));


            //CAMUS
            List<EntityCharacter.ClothingSet> camusClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> camusSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("camusHome"), 7, 0, 7, 30, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL),
                new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.TOWN], areas[Area.AreaEnum.TOWN].GetWaypoint("camusShop"), 7, 30, 17, 0, trueCondition, 0, EntityCharacter.Schedule.StandAtEvent.DirectionBehavior.RANDOM),
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("camusHome"), 17, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL));

            List<EntityCharacter.DialogueOption> camusDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Camus. abcdefghijklmnopqrstuvwxyz\nABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\nanother sentence here!.?", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));
            
            characters.Add(new EntityCharacter("Camus", this, EntityCharacter.CharacterEnum.CAMUS, camusClothing, camusSchedule, camusDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.INTERIOR].GetWaypoint("camusHome"),
                GameState.FLAG_LETTER_GIFT_CAMUS));

            //AIDEN
            List<EntityCharacter.ClothingSet> aidenClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> aidenSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("aidenHome"), 7, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.SMALL));

            List<EntityCharacter.DialogueOption> aidenDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Aiden... :(", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));
            
            characters.Add(new EntityCharacter("Aiden", this, EntityCharacter.CharacterEnum.AIDEN, aidenClothing, aidenSchedule, aidenDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.INTERIOR].GetWaypoint("aidenHome"),
                GameState.FLAG_LETTER_GIFT_AIDEN));

            //RAUL
            List<EntityCharacter.ClothingSet> raulClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> raulSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.BEACH], areas[Area.AreaEnum.BEACH].GetWaypoint("raulHome"), 7, 0, 22, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.INFINITE),
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.BEACH], areas[Area.AreaEnum.BEACH].GetWaypoint("raulHome"), 22, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.SMALL));

            List<EntityCharacter.DialogueOption> raulDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Raul! :)", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));
            
            characters.Add(new EntityCharacter("Raul", this, EntityCharacter.CharacterEnum.RAUL, raulClothing, raulSchedule, raulDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.BEACH].GetWaypoint("raulHome"),
                GameState.FLAG_LETTER_GIFT_RAUL));

            //FINLEY
            List<EntityCharacter.ClothingSet> finleyClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            //test moving from beach -> inn
            //List<EntityCharacter.Schedule.Event> finleySchedule = Util.GenerateSchedule(
            //    new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("SPinnSpare1Hall"), 7, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.INFINITE));
            //List<EntityCharacter.DialogueOption> finleyDialogue = Util.GenerateDialogueList(
            //    new EntityCharacter.DialogueOption(new DialogueNode("I'm Finley.", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));
            //characters.Add(new EntityCharacter("Finley", this, EntityCharacter.CharacterEnum.FINLEY, finleyClothing, finleySchedule, finleyDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.TOWN].GetWaypoint("SPbeach"),
            //    GameState.FLAG_LETTER_GIFT_FINLEY));


            List<EntityCharacter.Schedule.Event> finleySchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.S0], areas[Area.AreaEnum.S0].GetWaypoint("finleyHome"), 7, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.INFINITE));

            List<EntityCharacter.DialogueOption> finleyDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Finley.", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));
            
            characters.Add(new EntityCharacter("Finley", this, EntityCharacter.CharacterEnum.FINLEY, finleyClothing, finleySchedule, finleyDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.S0].GetWaypoint("finleyHome"),
                GameState.FLAG_LETTER_GIFT_FINLEY));

            //THEO
            List<EntityCharacter.ClothingSet> theoClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> theoSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("theoHome"), 7, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL));

            List<EntityCharacter.DialogueOption> theoDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm ThEo.", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));
            
            characters.Add(new EntityCharacter("Theo", this, EntityCharacter.CharacterEnum.THEO, theoClothing, theoSchedule, theoDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.INTERIOR].GetWaypoint("theoHome"),
                GameState.FLAG_LETTER_GIFT_THEO));

            //ELLE
            List<EntityCharacter.ClothingSet> elleClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> elleSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("elleHome"), 7, 0, 8, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL),
                new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("elleWork"), 8, 0, 21, 0, trueCondition, 0, EntityCharacter.Schedule.StandAtEvent.DirectionBehavior.RIGHT),
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("elleHome"), 21, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL));

            List<EntityCharacter.DialogueOption> elleDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Elle!", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));

            characters.Add(new EntityCharacter("Elle", this, EntityCharacter.CharacterEnum.ELLE, elleClothing, elleSchedule, elleDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.INTERIOR].GetWaypoint("elleHome"),
                GameState.FLAG_LETTER_GIFT_ELLE));

            //SKYE
            List<EntityCharacter.ClothingSet> skyeClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> skyeSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("skyeHome"), 7, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL));

            List<EntityCharacter.DialogueOption> skyeDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Skye...", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));

            characters.Add(new EntityCharacter("Skye", this, EntityCharacter.CharacterEnum.SKYE, skyeClothing, skyeSchedule, skyeDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.INTERIOR].GetWaypoint("skyeHome"),
                GameState.FLAG_LETTER_GIFT_SKYE));

            //CHARLOTTE
            List<EntityCharacter.ClothingSet> charlotteClothing = Util.GenerateClothingSetList(
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(trueCondition), 0),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.JEANS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(springCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.CLOTHING_NONE, ItemDict.BUTTON_DOWN, ItemDict.CLOTHING_NONE, ItemDict.GetColoredItem(ItemDict.CHINO_SHORTS, Util.NAVY), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(summerCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.BUTTON_DOWN, Util.WHITE), ItemDict.GetColoredItem(ItemDict.ALL_SEASON_JACKET, Util.BLACK), ItemDict.GetColoredItem(ItemDict.CHINOS, Util.LIGHT_BROWN), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(fallCondition), 1),
                new EntityCharacter.ClothingSet(ItemDict.SKIN_PEACH, ItemDict.GetColoredItem(ItemDict.HAIR_LUCKY_LUKE, Util.HAIR_CHARCOAL_BLACK), ItemDict.EYES_BROWN,
                ItemDict.GetColoredItem(ItemDict.BASEBALL_CAP, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SHORT_SLEEVE_TEE, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.HOODED_SWEATSHIRT, Util.LIGHT_GREY), ItemDict.GetColoredItem(ItemDict.JEANS, Util.BLACK), ItemDict.GetColoredItem(ItemDict.SNEAKERS, Util.WHITE), ItemDict.CLOTHING_NONE,
                ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE, ItemDict.CLOTHING_NONE,
                Util.QuickArray(winterCondition), 1));

            List<EntityCharacter.Schedule.Event> charlotteSchedule = Util.GenerateSchedule(
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("charlotteHome"), 6, 0, 7, 30, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL),
                new EntityCharacter.Schedule.StandAtEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("charlotteWork"), 7, 30, 20, 0, trueCondition, 0, EntityCharacter.Schedule.StandAtEvent.DirectionBehavior.RIGHT),
                new EntityCharacter.Schedule.WanderNearEvent(areas[Area.AreaEnum.INTERIOR], areas[Area.AreaEnum.INTERIOR].GetWaypoint("charlotteHome"), 20, 0, 24, 0, trueCondition, 0, EntityCharacter.Schedule.WanderNearEvent.WanderRange.VERY_SMALL));

            List<EntityCharacter.DialogueOption> charlotteDialogue = Util.GenerateDialogueList(
                new EntityCharacter.DialogueOption(new DialogueNode("I'm Charlotte. :3", DialogueNode.PORTRAIT_SYSTEM), Util.QuickArray(trueCondition)));

            characters.Add(new EntityCharacter("Charlotte", this, EntityCharacter.CharacterEnum.CHARLOTTE, charlotteClothing, charlotteSchedule, charlotteDialogue, Content.Load<Texture2D>(Paths.EMOTION_PANEL), areas[Area.AreaEnum.INTERIOR].GetWaypoint("charlotteHome"),
                GameState.FLAG_LETTER_GIFT_CHARLOTTE));
        }
        

        public void MoveCharacter(EntityCharacter character, Area areaFrom, Area areaTo)
        {
            areaFrom.RemoveEntity(character);
            areaTo.AddEntity(character);
            character.SetCurrentArea(areaTo);
        }

        public EntityCharacter GetCharacter(EntityCharacter.CharacterEnum cEnum)
        {
            foreach(EntityCharacter character in characters)
            {
                if (character.GetCharacterEnum() == cEnum)
                    return character;
            }
            return null;
        }

        private int GetDayAdjustment()
        {
            switch(currentSeason)
            {
                case Season.SPRING:
                    return 0;
                case Season.SUMMER:
                    return 1;
                case Season.AUTUMN:
                    return 0;
                case Season.WINTER:
                    return 1;
            }
            return 10000;
        }

        public void DrawForeground(SpriteBatch sb, RectangleF cameraBoundingBox, float layerDepth)
        {
            currentArea.DrawForeground(sb, cameraBoundingBox, layerDepth);
        }

        public float GetDarkLevel()
        {
            float hourStart = 0.0f;
            if (GetHour() == 0)
            {
                hourStart = hourlyDark[23];
            }
            else
            {
                hourStart = hourlyDark[GetHour() - 1];
            }
            float hourEnd = hourlyDark[GetHour()];
            float hourChange = hourEnd - hourStart;
            hourChange *= GetMinute() / 60.0f;

            float darkLevel = hourStart + hourChange;
            if (currentWeather == World.Weather.RAINY)
            {
                darkLevel -= 0.2f;
            }

            return darkLevel;
        }

        public float GetLightLevel()
        {
            float hourStart = 0.0f;
            if (GetHour() == 0)
            {
                hourStart = hourlyLight[23];
            }
            else
            {
                hourStart = hourlyLight[GetHour() - 1];
            }
            float hourEnd = hourlyLight[GetHour()];
            float hourChange = hourEnd - hourStart;
            hourChange *= GetMinute() / 60.0f;

            float lightLevel = hourStart + hourChange;
            return lightLevel;
        }

        public Dictionary<Area.AreaEnum, Area> GetAreaDict()
        {
            return areas;
        }

        public void ChangeArea(Area newArea)
        {
            currentArea.UnloadLayers();
            this.currentArea = newArea;
            currentArea.LoadLayers();
        }

        public Area GetCurrentArea()
        {
            return currentArea;
        }

        public Season GetSeason()
        {
            return currentSeason;
        }

        public int GetDay()
        {
            return currentDay;
        }

        public int GetHour()
        {
            return (int)(currentTime / 60);
        }

        public int GetMinute()
        {
            return ((int)currentTime) % 60;
        }

        public static World.TimeOfDay GetTimeOfDay(int hour)
        {
            if (hour < MORNING_START_HOUR)
            {
                return TimeOfDay.NIGHT;
            }
            else if (hour < MORNING_END_HOUR)
            {
                return TimeOfDay.MORNING;
            }
            else if (hour < DAY_END_HOUR)
            {
                return TimeOfDay.DAY;
            }
            else if (hour < EVENING_END_HOUR)
            {
                return TimeOfDay.EVENING;
            }
            else
            {
                return TimeOfDay.NIGHT;
            }
        }

        public TimeOfDay GetTimeOfDay()
        {
            return World.GetTimeOfDay(GetHour());
        }

        public TimeData GetTimeData()
        {
            return new TimeData(currentSeason, GetDay(), GetHour(), GetMinute(), GetTimeOfDay());
        }

        public void Update(float deltaTime, GameTime gameTime, EntityPlayer player, GameplayInterface ui, Camera camera, bool cutscene)
        {
            if (!paused)
            {
                if (transitionToArea != null && ui.IsTransitionReady())
                {
                    ChangeArea(transitionToArea);
                    if (transitionToSpawn != null)
                    {
                        currentArea.MoveToWaypoint(player, transitionToSpawn.name);
                        if (transitionToSpawn.IsCameraLocked())
                        {
                            camera.Update(deltaTime, transitionToSpawn.cameraLockPosition, currentArea.MapPixelWidth(), currentArea.MapPixelHeight());
                            camera.Lock();
                        }
                        else
                        {
                            camera.Update(deltaTime, player.GetAdjustedPosition(), currentArea.MapPixelWidth(), currentArea.MapPixelHeight());
                            camera.Unlock();
                        }
                    } else
                    {
                        player.SetPosition(transitionToPosition);
                    }
                    
                    currentArea.RandomizeBackground(camera.GetBoundingBox());
                    transitionToArea = null;
                    transitionToSpawn = null;
                    transitionToPosition = new Vector2(-100, -100);
                    player.Unpause();
                }

                if (!cutscene && player.GetCurrentDialogue() == null)
                {
                    currentTime += deltaTime * 1.0f;

                    tickTimer += deltaTime;
                    if (tickTimer > TICK_LENGTH)
                    {
                        tickTimer -= TICK_LENGTH;
                        foreach (Area.AreaEnum areaEnum in areas.Keys)
                        {
                            areas[areaEnum].Tick(TICK_LENGTH, player, this);
                        }
                    }
                }
                currentArea.Update(deltaTime, gameTime, GetTimeData(), currentWeather, camera.GetBoundingBox(), player);
                foreach(EntityCharacter chara in characters)
                {
                    if (chara.GetCurrentArea() != currentArea)
                    {
                        chara.Update(deltaTime, chara.GetCurrentArea());
                    }
                }
            }
        }

        public Area GetAreaByEnum(Area.AreaEnum areaEnum)
        {
            return areas[areaEnum];
        }

        public void DrawBackground(SpriteBatch sb, RectangleF cameraBoundingBox, float layerDepth)
        {
            currentArea.DrawBackground(sb, cameraBoundingBox, layerDepth);
        }

        public void DrawWaterBackground(SpriteBatch sb, float layerDepth)
        {
            currentArea.DrawWaterBackground(sb, layerDepth);
        }

        public void DrawBuildingBlocks(SpriteBatch sb, float layerDepth)
        {
            currentArea.DrawBuildingBlocks(sb, layerDepth);
        }

        public void DrawItemEntities(SpriteBatch sb, float layerDepth)
        {
            currentArea.DrawItemEntities(sb, layerDepth);
        }

        public void DrawEntities(SpriteBatch sb, DrawLayer layer, RectangleF cameraBoundingBox, float layerDepth)
        {
            currentArea.DrawEntities(sb, layer, cameraBoundingBox, layerDepth);
        }

        public void DrawBaseTerrain(SpriteBatch sb, float layerDepth)
        {
            currentArea.DrawBaseTerrain(sb, layerDepth); //draw the tiledmap
        }

        public void DrawWater(SpriteBatch sb, float layerDepth)
        {
            currentArea.DrawWater(sb, layerDepth);
        }

        public bool IsDayOver()
        {
            return currentTime >= DAY_ENDING_TIME;
        }

        public void AdvanceDay(EntityPlayer player)
        {
            tickTimer = 0;
            int numTicksSkipped = (int)(DAY_STARTING_TIME / TICK_LENGTH);
            float timeSkipped = DAY_ENDING_TIME - currentTime;
            numTicksSkipped += (int)(timeSkipped / TICK_LENGTH);
            for(int i = 0; i < numTicksSkipped; i++)
            {
                foreach (Area.AreaEnum areaEnum in areas.Keys)
                {
                    areas[areaEnum].Tick(TICK_LENGTH, player, this);
                }
            }

            currentTime = DAY_STARTING_TIME;
            //currentTime = 0;
            currentDay++;
            if (currentDay >= DAYS_IN_SEASON)
            {
                currentDay = 0;
                switch (currentSeason)
                {
                    case Season.SPRING:
                        currentSeason = Season.SUMMER;
                        break;
                    case Season.SUMMER:
                        currentSeason = Season.AUTUMN;
                        break;
                    case Season.AUTUMN:
                        currentSeason = Season.WINTER;
                        break;
                    case Season.WINTER:
                        currentSeason = Season.SPRING;
                        currentCycle++;
                        break;
                }
            }

            int weatherSeed = Util.RandInt(1, 10);
            switch(currentSeason)
            {
                case Season.SPRING:
                    if(weatherSeed <= 3)
                    {
                        currentWeather = Weather.SUNNY;
                    }  else if (weatherSeed <= 7)
                    {
                        currentWeather = Weather.CLOUDY;
                    } else
                    {
                        currentWeather = Weather.RAINY;
                    }
                    break;
                case Season.SUMMER:
                    if(weatherSeed <= 5)
                    {
                        currentWeather = Weather.SUNNY;
                    } else if (weatherSeed <= 7)
                    {
                        currentWeather = Weather.RAINY;
                    } else
                    {
                        currentWeather = Weather.CLOUDY;
                    }
                    break;
                case Season.AUTUMN:
                    if(weatherSeed <= 6)
                    {
                        currentWeather = Weather.CLOUDY;
                    } else if (weatherSeed <= 8)
                    {
                        currentWeather = Weather.SUNNY;
                    } else
                    {
                        currentWeather = Weather.RAINY;
                    }
                    break;
                case Season.WINTER:
                    if (weatherSeed <= 2)
                    {
                        currentWeather = Weather.CLOUDY;
                    }
                    else
                    {
                        currentWeather = Weather.SNOWY;
                    }
                    break;
            }

            foreach (Area.AreaEnum areaEnum in areas.Keys)
            {
                areas[areaEnum].TickDay(this, player);
            }

            PlateauMain.SAVE_MANAGER.SaveFile(player, this);
        }

        public SaveState GenerateSave()
        {
            SaveState state = new SaveState(SaveState.Identifier.WORLD);
            state.AddData("season", currentSeason.ToString());
            state.AddData("day", currentDay.ToString());
            state.AddData("weather", currentWeather.ToString());
            state.AddData("cycle", currentCycle.ToString());
            return state;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public Weather GetWeather()
        {
            return currentWeather;
        }

        //load
        public void LoadSave(SaveState save)
        {
            currentSeason = StringToSeason(save.TryGetData("season", "SPRING"));
            currentDay = Int32.Parse(save.TryGetData("day", "0"));
            currentCycle = Int32.Parse(save.TryGetData("cycle", "1"));
            string weatherName = save.TryGetData("weather", Weather.CLOUDY.ToString());
            World.Weather weather;
            Enum.TryParse(weatherName, out weather);
            currentWeather = weather;
            foreach(EntityCharacter character in characters)
            {
                character.RefreshClothing(this);
            }
        }

        public int GetCycle()
        {
            return currentCycle;
        }

        public void SetWeather(Weather newWeather)
        {
            this.currentWeather = newWeather;
        }

        public void DrawWalls(SpriteBatch sb, float layerDepth)
        {
            currentArea.DrawWalls(sb, layerDepth);
        }

        public void DrawParticles(SpriteBatch sb, float layerDepth)
        {
            currentArea.DrawParticles(sb, layerDepth);
        }

        public void SetTime(int hour, int minute, EntityPlayer player)
        {
            int timeToAdvanceTo = (hour * 60) + minute;

            if(timeToAdvanceTo > currentTime)
            {
                tickTimer = 0;
                int numTicksSkipped = 0;
                float timeSkipped = timeToAdvanceTo - currentTime;
                numTicksSkipped += (int)(timeSkipped / TICK_LENGTH);
                System.Diagnostics.Debug.WriteLine("skipped ticks: " + numTicksSkipped);
                for (int i = 0; i < numTicksSkipped; i++)
                {
                    foreach (Area.AreaEnum areaEnum in areas.Keys)
                    {
                        areas[areaEnum].Tick(TICK_LENGTH, player, this);
                    }
                }
            }

            currentTime = (hour * 60) + minute;
        }

        public Area GetAreaByName(string areaName)
        {
            foreach (Area.AreaEnum areaEnum in areas.Keys)
            {
                if (areaEnum.ToString().Equals(areaName))
                {
                    return GetAreaByEnum(areaEnum);
                }
            }
            throw new Exception("Exception in GetAreaByName...");
        }

        public void PlayTransitionAnimation(EntityPlayer player, Area.TransitionZone.Animation animationType)
        {
            switch (animationType)
            {
                case Area.TransitionZone.Animation.TO_DOWN:
                    ui.TransitionDown();
                    player.Pause();
                    break;
                case Area.TransitionZone.Animation.TO_UP:
                    ui.TransitionUp();
                    player.Pause();
                    break;
                case Area.TransitionZone.Animation.TO_LEFT:
                    ui.TransitionLeft();
                    player.Pause();
                    break;
                case Area.TransitionZone.Animation.TO_RIGHT:
                    ui.TransitionRight();
                    player.Pause();
                    break;
            }
        }

        public void HandleTransitions(EntityPlayer player)
        {
            Area.TransitionZone tz;
            if(player.GetTransitionAreaTo().Equals("") && player.GetTransitionSpawnTo().Equals(""))
            {
                tz = GetCurrentArea().CheckTransition(player.GetAdjustedPosition(), player.AttemptingTransition());
            } else
            {
                tz = new Area.TransitionZone(new RectangleF(0, 0, 0, 0), player.GetTransitionAreaTo(), player.GetTransitionSpawnTo(), false, player.GetTransitionAnimation());
            }
            if (tz != null)
            {
                if(player.AttemptingTransition())
                {
                    player.ToggleAttemptTransition();
                }
                foreach (Area.AreaEnum areaEnum in areas.Keys)
                {
                    if (areaEnum.ToString().Equals(tz.to))
                    {
                        PlayTransitionAnimation(player, tz.animation);
                        transitionToArea = areas[areaEnum];

                        if (tz.spawn.Equals(""))
                        {
                            transitionToPosition = player.GetTransitionPositionTo();
                        }
                        else
                        {
                            transitionToSpawn = transitionToArea.GetWaypoint(tz.spawn);
                        }
                        player.ClearTransitionData();
                    }
                }
            }
        }
    }
}
