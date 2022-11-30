using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Plateau.Components;
using Plateau.Entities;
using Plateau.Items;
using Plateau.Particles;
using Plateau.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau
{
    public class Area
    {
        public class XYTile
        {
            public int tileX, tileY;

            public XYTile(int tileX, int tileY)
            {
                this.tileX = tileX;
                this.tileY = tileY;
            }
        }

        public class SoundZone
        {
            public World.TimeOfDay time;
            public RectangleF rect;
            public SoundSystem.Sound sound;
            public World.Season season;

            public SoundZone(RectangleF zone, SoundSystem.Sound sound, World.TimeOfDay time, World.Season season)
            {
                this.time = time;
                this.rect = zone;
                this.sound = sound;
                this.season = season;
            }
        }

        public class LightingZone
        {
            public RectangleF rect;
            public float darkLevel;

            public LightingZone(RectangleF zone, float darkLevel)
            {
                this.rect = zone;
                this.darkLevel = darkLevel;
            }
        }

        public class Subarea
        {
            public enum NameEnum
            {
                APEX, BEACH, FARM, STORE, CAFE, BOOKSTORELOWER, BOOKSTOREUPPER, ROCKWELLHOUSE, BEACHHOUSE,
                PIPERHOUSE, TOWNHALLLOWER, TOWNHALLUPPER, WORKSHOP, FORGE, S0WALK, S0WARP, S1WALK, S1WARP, S2, S3, S4, TOWN, INN, PIPERUPPER, PIPERLOWER,
                INNBATH, INNTROY, INNCHARLOTTE, INNSPARE1, INNSPARE2, INNHIMEKO,
                FARMHOUSECABIN, FARMHOUSEHOUSE, FARMHOUSEMANSIONLOWER, FARMHOUSEMANSIONUPPER
            }

            public RectangleF rect;
            public NameEnum subareaName;

            public Subarea(RectangleF rect, string subareaName)
            {
                this.rect = rect;
                if (!Enum.TryParse(subareaName.ToUpper(), out this.subareaName))
                {
                    throw new Exception("Couldn't parse " + subareaName + " to enum...");
                }
            }
        }

        public class MovingPlatformDirectorZone
        {
            public RectangleF rectangle;
            public Vector2 newVelocity;

            public MovingPlatformDirectorZone(RectangleF rectangle, Vector2 velocity)
            {
                this.rectangle = rectangle;
                this.newVelocity = velocity;
            }
        }

        public class FishingZone
        {
            public RectangleF rectangle;
            public LootTables.LootTable lootTable;
            public int difficulty;

            public FishingZone(int difficulty, LootTables.LootTable lootTable, RectangleF rectangle) {
                this.difficulty = difficulty;
                this.lootTable = lootTable;
                this.rectangle = rectangle;
            }
        }

        public class CutsceneTriggerZone
        {
            public string cutsceneID;
            public RectangleF rectangle;

            public CutsceneTriggerZone(string id, RectangleF rectangle)
            {
                this.cutsceneID = id;
                this.rectangle = rectangle;
            }
        }

        public class NamedZone
        {
            public string name;
            public RectangleF rectangle;

            public NamedZone(string name, RectangleF rectangle)
            {
                this.name = name;
                this.rectangle = rectangle;
            }
        }

        public class SpawnZone
        {
            protected class Entry
            {
                public EntityType type;
                public float spawnFrequency;
                public World.Season season;

                public Entry(EntityType type, float spawnFrequency, World.Season season)
                {
                    this.spawnFrequency = spawnFrequency;
                    this.type = type;
                    this.season = season;
                }
            }

            protected List<Entry> possibleSpawns;
            protected List<XYTile> validTiles;
            protected bool ceiling;
            protected bool bridgesAllowed;

            public SpawnZone(List<XYTile> validTiles, bool ceil, bool bridgesAllowed)
            {
                this.validTiles = validTiles;
                this.possibleSpawns = new List<Entry>();
                this.ceiling = ceil;
                this.bridgesAllowed = bridgesAllowed;
            }

            public void AddEntry(EntityType type, float spawnFrequency, World.Season season)
            {
                this.possibleSpawns.Add(new Entry(type, spawnFrequency, season));
            }

            public virtual void TickDaily(Area area)
            {
                TickDaily(area, new Rectangle(0, 0, Int32.MaxValue, Int32.MaxValue));
            }

            public virtual void TickDaily(Area area, Rectangle legalTiles) {
                foreach (Entry entry in possibleSpawns)
                {
                    if (entry.season != World.Season.NONE && entry.season != area.GetSeason())
                    {
                        continue;
                    }

                    float spawnsLeft = entry.spawnFrequency;
                    while (spawnsLeft > 0)
                    {
                        if (spawnsLeft >= 1)
                        {
                            TryCreateSpawn(area, entry.type, legalTiles);
                            spawnsLeft--;
                        }
                        else
                        {
                            float roll = Util.RandInt(1, 100) / 100.0f;
                            if (roll >= spawnsLeft)
                            {
                                TryCreateSpawn(area, entry.type, legalTiles);
                            }
                            spawnsLeft = 0;
                        }
                    }
                }
            }

            private void TryCreateSpawn(Area area, EntityType toSpawn, Rectangle legalTiles)
            {
                //choose a random tile
                XYTile chosenXYTile = validTiles[Util.RandInt(0, validTiles.Count - 1)];
                Vector2 chosenTile = new Vector2(chosenXYTile.tileX, chosenXYTile.tileY);

                if (legalTiles.Contains(chosenTile))
                {
                    TileEntity measurement = (TileEntity)EntityFactory.GetEntity(toSpawn, ItemDict.NONE, chosenTile, area);
                    if (!ceiling)
                    {
                        chosenTile.Y -= (measurement.GetTileHeight() - 1);
                    }
                    TileEntity toPlace = (TileEntity)EntityFactory.GetEntity(toSpawn, ItemDict.NONE, chosenTile, area);

                    if (IsGivenSpawnLegal(area, chosenTile, toPlace))
                    {
                        area.AddTileEntity(toPlace);
                    }
                }
            }

            public virtual bool IsGivenSpawnLegal(Area area, Vector2 chosenTile, TileEntity toPlace)
            {
                if(ceiling)
                {
                    return area.IsCeilingTileEntityPlacementValid((int)chosenTile.X, (int)chosenTile.Y, toPlace.GetTileWidth(), toPlace.GetTileHeight());
                }
                return area.IsTileEntityPlacementValid((int)chosenTile.X, (int)chosenTile.Y, toPlace.GetTileWidth(), toPlace.GetTileHeight(), bridgesAllowed);
            }
        }

        public class TransitionZone
        {
            public enum Animation
            {
                TO_UP, TO_DOWN, TO_LEFT, TO_RIGHT
            }

            public string to;
            public string spawn;
            public RectangleF rectangle;
            public bool automatic;
            public Animation animation;

            public TransitionZone(RectangleF rectangle, string to, string spawn, bool auto, Animation animation)
            {
                this.to = to;
                this.spawn = spawn;
                this.rectangle = rectangle;
                this.automatic = auto;
                this.animation = animation;
            }
        }

        public class PathingHelper
        {
            public enum Type
            {
                CONDITIONALJUMP, BEACHHELPER
            }

            public RectangleF rect;
            public Type type;

            public PathingHelper(RectangleF rect, Type type)
            {
                this.rect = rect;
                this.type = type;
            }
        }

        public class Waypoint
        {
            public Vector2 position;
            public Vector2 cameraLockPosition;
            public string name;
            public Area area;

            public Waypoint(Vector2 position, string name, Area area)
            {
                this.position = position;
                this.name = name;
                this.cameraLockPosition = new Vector2(-10000, -10000);
                this.area = area;
            }

            public Waypoint(Vector2 position, string name, Vector2 cameraLockPosition, Area area)
            {
                this.name = name;
                this.position = position;
                this.cameraLockPosition = cameraLockPosition;
                this.area = area;
            }

            public bool IsCameraLocked()
            {
                return cameraLockPosition.X != -10000 & cameraLockPosition.Y != -10000;
            }
        }

        public class LightSource
        {
            public enum Strength
            {
                SMALL, MEDIUM, LARGE
            }

            public Strength lightStrength;
            public Vector2 position;
            public Entity source;
            public Color color;

            public LightSource(Strength lightStrength, Vector2 position, Color color, Entity source = null)
            {
                this.lightStrength = lightStrength;
                this.position = position;
                this.source = source;
                this.color = color;
            }
        }

        private class EntityListManager
        {
            private List<Entity> entityList;
            private List<IInteract> interactableEntityList;
            private List<IInteractContact> contactInteractableEntityList;
            private List<IInteractTool> toolInteractableEntityList;
            private List<IHaveHealthBar> healthBarEntityList;
            private List<EntityCollidable> collideableEntityList;
            private List<EntitySolid> solidEntityList;
            private Dictionary<DrawLayer, List<Entity>> entityListByLayer;

            public EntityListManager()
            {
                entityList = new List<Entity>();
                entityListByLayer = new Dictionary<DrawLayer, List<Entity>>();
                collideableEntityList = new List<EntityCollidable>();
                interactableEntityList = new List<IInteract>();
                healthBarEntityList = new List<IHaveHealthBar>();
                toolInteractableEntityList = new List<IInteractTool>();
                contactInteractableEntityList = new List<IInteractContact>();
                solidEntityList = new List<EntitySolid>();
            }

            public void Remove(Entity entity)
            {
                entityList.Remove(entity);
                entityListByLayer[entity.GetDrawLayer()].Remove(entity);
                if (entity is EntityCollidable)
                {
                    collideableEntityList.Remove((EntityCollidable)entity);
                }
                if (entity is EntitySolid)
                {
                    solidEntityList.Remove((EntitySolid)entity);
                }
                if (entity is IInteract)
                {
                    interactableEntityList.Remove((IInteract)entity);
                }
                if (entity is IInteractContact)
                {
                    contactInteractableEntityList.Remove((IInteractContact)entity);
                }
                if (entity is IInteractTool)
                {
                    toolInteractableEntityList.Remove((IInteractTool)entity);
                }
                if(entity is IHaveHealthBar)
                {
                    healthBarEntityList.Remove((IHaveHealthBar)entity);
                }
            }

            public void Add(Entity entity)
            {
                entityList.Add(entity);
                if (!entityListByLayer.ContainsKey(entity.GetDrawLayer()))
                {
                    entityListByLayer[entity.GetDrawLayer()] = new List<Entity>();
                }
                entityListByLayer[entity.GetDrawLayer()].Add(entity);
                if (entity is EntityCollidable)
                {
                    collideableEntityList.Add(((EntityCollidable)entity));
                }
                if (entity is EntitySolid)
                {
                    solidEntityList.Add((EntitySolid)entity);
                }
                if (entity is IInteract)
                {
                    interactableEntityList.Add((IInteract)entity);
                }
                if (entity is IInteractContact)
                {
                    contactInteractableEntityList.Add((IInteractContact)entity);
                }
                if (entity is IInteractTool)
                {
                    toolInteractableEntityList.Add((IInteractTool)entity);
                }
                if(entity is IHaveHealthBar)
                {
                    healthBarEntityList.Add((IHaveHealthBar)entity);
                }
            }

            public List<Entity> GetEntitiesByDrawLayer(DrawLayer layer)
            {
                if (entityListByLayer.ContainsKey(layer))
                {
                    return entityListByLayer[layer];
                }
                return new List<Entity>();
            }

            public List<Entity> GetEntityList()
            {
                return entityList;
            }

            public List<EntityCollidable> GetCollideableEntityList()
            {
                return collideableEntityList;
            }

            public List<EntitySolid> GetSolidEntityList()
            {
                return solidEntityList;
            }

            public List<IInteract> GetInteractableEntityList()
            {
                return interactableEntityList;
            }

            public List<IInteractContact> GetContactInteractableEntityList()
            {
                return contactInteractableEntityList;
            }

            public List<IInteractTool> GetToolInteractableEntityList()
            {
                return toolInteractableEntityList;
            }

            public List<IHaveHealthBar> GetHealthBarEntityList()
            {
                return healthBarEntityList;
            }
        }

        public List<IHaveHealthBar> GetHealthBarEntities()
        {
            return entityListManager.GetHealthBarEntityList();
        }

        //Earth - farmable with hoe
        //Sand - for palm trees
        //Solid - not farmable, not extractable
        //Extractable - not farmable, but mineable with extractor
        public enum GroundTileType
        {
            EARTH, SAND, BRIDGE, SOLID, EXTRACTABLE
        }

        public enum AreaEnum {
            FARM, TOWN, BEACH, S0, S1, S2, S3, S4, APEX, INTERIOR, NONE
        }

        public enum CollisionTypeEnum
        {
            BOUNDARY, AIR, SOLID, BRIDGE, WATER, DEEP_WATER, TOP_WATER,
            SCAFFOLDING, SCAFFOLDING_BRIDGE, SCAFFOLDING_BLOCK
        }

        private int tileHeight, tileWidth;
        private int widthInTiles, heightInTiles;

        private TiledMap tiledMap;
        private string mapWaterPath, mapDecorationFGPath, mapBasePath, mapDecorationPath, mapWallsPath, mapWaterBGPath, mapFGCavePath;
        private Texture2D mapWater, mapDecorationFG, mapBase, mapDecoration, mapWalls, mapWaterBG, mapFGCave;
        private ContentManager layerContentManager;
        private CollisionTypeEnum[,] collisionMap;
        private bool[,] wallMap;
        private bool[,] grassMap;
        private bool[,] blockerMap;
        private bool[,] caveMap;
        private List<PathingHelper> pathingHelpers;
        private TileEntity[,] tileEntityGrid;
        private BuildingBlock[,] buildingBlockGrid;
        private List<BuildingBlock> buildingBlockList;
        private TileEntity[,] wallEntityGrid;
        private PEntityWallpaper[,] wallpaperEntityGrid;
        private AreaEnum areaEnum;
        private string name;
        private List<TransitionZone> transitions = new List<TransitionZone>();
        private List<Waypoint> waypoints = new List<Waypoint>();
        private EntityListManager entityListManager = new EntityListManager();
        private List<LightSource> lights = new List<LightSource>();
        private List<EntityItem> itemEntities = new List<EntityItem>();
        private List<Particle> particleList = new List<Particle>();
        private List<SpawnZone> spawnZones = new List<SpawnZone>();
        private List<FishingZone> fishingZones = new List<FishingZone>();
        private List<NamedZone> nameZones = new List<NamedZone>();
        private List<Subarea> subareas = new List<Subarea>();
        private List<SoundZone> soundZones = new List<SoundZone>();
        private List<LightingZone> lightingZones = new List<LightingZone>();
        private List<CutsceneTriggerZone> cutsceneTriggerZones = new List<CutsceneTriggerZone>();
        private List<MovingPlatformDirectorZone> directorZones = new List<MovingPlatformDirectorZone>();
        private LayeredBackground background, foreground;
        private bool cameraMoves;
        private World.Weather areaWeather;
        private World.Season areaSeason;
        private World.Season worldSeason;
        private World.TimeData timeData;
        private float foregroundCaveTransparency;
        private static float FOREGROUND_CAVE_TRANSPARENCY_DELTA = 3f;
        private static float LIGHTING_CHANGE_SPEED = 1f;
        private float baseDarkLevel, positionalDarkLevel;

        //ALL NUMBERS ARE 1 HIGHER THAN TILED SAYS! TILED USES LACK OF A TILE AS ID=0; so IDS REALLY START AT 1
        //add 1 to all ids from tiled!
        //used for map collision
        private static int WATER_TOPPER_ID = 208;
        private static int WATER_PURE_TOPPER_ID = 217;
        private static int WATER_SWAMP_TOPPER_ID = 270;
        private static int WATER_LAVA_TOPPER_ID = 290;
        private static int WATER_CLOUD_TOPPER_ID = 300;
        private static int WATER_CLOUD_MID_TOPPER_ID = 298;
        private static int[] WATER_TOPPER_IDs = { 208, 217, 270, 290, 300, 298};
        private static int[] WATER_TILE_IDS = { 205, 216, 267, 287, 297 };
        private static int[] DEEP_WATER_TILE_IDS = { 206, 207, 268, 269, 288, 289, 298, 299 };

        //used for particles & farmable placement
        private static int[] ORANGE_EARTH_TILE_IDS = { 151, 152, 153, 154, 156 }; //farm, town, stratum 0
        private static int[] WHITE_EARTH_TILE_IDS = { 141, 142, 143, 144, 146 }; //stratum 1
        private static int[] BROWN_RUINS_TILE_IDS = { 71, 72, 73, 74, 76 }; //stratum 1
        private static int[] CAVE_STONE_TILE_IDS = { 161, 162, 163, 164, 166 }; //stratum 2
        private static int[] METAL_TILE_IDS = { 221, 222, 223, 224, 226 }; //stratum 2
        private static int[] BROWN_BARK_TILE_IDS = { 181, 182, 183, 184, 186 }; //stratum 3
        private static int[] RED_SAND_TILE_IDS = { 241, 242, 243, 244, 246 }; //stratum 3
        private static int[] ORANGE_VOLCANO_TILE_IDS = { 91, 92, 93, 94, 95, 96}; //stratum 3
        private static int[] BLACK_GROUND_TILE_IDS = { 171, 172, 173, 174, 176 }; // stratum 4
        private static int[] LAB_METAL_TILE_IDS = {221, 222, 223, 224, 225, 226 }; //stratum 2
        private static int[] WOODEN_BRIDGE_IDS = { 31, 32, 33, 34, 35, 36, 38, 39, 40 }; //bridges
        private static int[] METAL_BRIDGE_IDS = { 291, 292, 293 }; //stratum 4 bridges
        private static int[] BEACH_SAND_TILE_IDS = { 191, 192, 193, 194, 195, 202, 203, 211, 212, 213, 214, 215 }; //sand at the beach
        private static int[] WOOD_INTERIOR_TILE_IDS = { 68, 77, 78, 79 };
        private static int[] METAL_INTERIOR_TILE_IDS = { 119, 120, 110 };
        private static int[] WHITE_INTERIOR_TILE_IDS = { 160, 169, 170 };
        private static int[] BROWN_MUD_TILE_IDS = { 271, 272, 273, 274, 275 };

        public Area(AreaEnum name, TiledMap map, bool cameraMoves, GraphicsDevice graphics, ContentManager content, ContentManager layerContentManager, EntityPlayer player, RectangleF cameraBoundingBox, LayeredBackground.BackgroundParams backgroundParameters, LayeredBackground.BackgroundParams foregroundParameters,
            string mapWaterPath, string mapDecorationFGPath, string mapBasePath, string mapDecorationPath, string mapWallsPath, string mapWaterBGPath, string mapFGCavePath)
        {
            this.areaWeather = World.Weather.SUNNY;
            this.areaEnum = name;
            this.cameraMoves = cameraMoves;

            this.layerContentManager = layerContentManager;
            this.mapWaterPath = mapWaterPath;
            this.mapDecorationFGPath = mapDecorationFGPath;
            this.mapBasePath = mapBasePath;
            this.mapDecorationPath = mapDecorationPath;
            this.mapWallsPath = mapWallsPath;
            this.mapWaterBGPath = mapWaterBGPath;
            this.mapFGCavePath = mapFGCavePath;
            this.foregroundCaveTransparency = 1.0f;
            this.baseDarkLevel = 1.0f;
            this.positionalDarkLevel = 1.0f;

            //Set the tiled map, create the collision map based off of the given map
            this.tiledMap = map;

            TiledMapTileLayer baseLayer = (TiledMapTileLayer)tiledMap.GetLayer("base");
            TiledMapTileLayer wallLayer = (TiledMapTileLayer)tiledMap.GetLayer("walls");
            TiledMapTileLayer grassLayer = (TiledMapTileLayer)tiledMap.GetLayer("grass");
            TiledMapTileLayer blockerLayer = (TiledMapTileLayer)tiledMap.GetLayer("blocker");
            TiledMapTileLayer caveLayer = (TiledMapTileLayer)tiledMap.GetLayer("fg_cave");
            tileHeight = baseLayer.TileHeight;
            tileWidth = baseLayer.TileWidth;
            widthInTiles = baseLayer.Width;
            heightInTiles = baseLayer.Height;
            collisionMap = new CollisionTypeEnum[baseLayer.Width, baseLayer.Height];
            wallMap = new bool[baseLayer.Width, baseLayer.Height];
            blockerMap = new bool[baseLayer.Width, baseLayer.Height];
            caveMap = new bool[baseLayer.Width, baseLayer.Height];
            pathingHelpers = new List<PathingHelper>();
            tileEntityGrid = new TileEntity[baseLayer.Width, baseLayer.Height];
            wallEntityGrid = new TileEntity[baseLayer.Width, baseLayer.Height];
            grassMap = new bool[baseLayer.Width, baseLayer.Height];
            wallpaperEntityGrid = new PEntityWallpaper[baseLayer.Width, baseLayer.Height];
            buildingBlockGrid = new BuildingBlock[baseLayer.Width, baseLayer.Height];
            buildingBlockList = new List<BuildingBlock>();
            baseDarkLevel = float.Parse(map.Properties["darkLevel"]);
            for (int x = 0; x < baseLayer.Width; x++)
            {
                for (int y = 0; y < baseLayer.Height; y++)
                {
                    tileEntityGrid[x, y] = null;
                    buildingBlockGrid[x, y] = null;
                    wallMap[x, y] = false;
                    grassMap[x, y] = false;
                    wallEntityGrid[x, y] = null;
                    blockerMap[x, y] = false;
                }
            }
            for (int x = 0; x < baseLayer.Width; x++)
            {
                for (int y = 0; y < baseLayer.Height; y++)
                {
                    TiledMapTile? t;
                    baseLayer.TryGetTile((ushort)x, (ushort)y, out t); //COULD BE BAD

                    //process base layer
                    int tileGlobalId = t.Value.GlobalIdentifier;
                    if (WOODEN_BRIDGE_IDS.Contains(tileGlobalId) || METAL_BRIDGE_IDS.Contains(tileGlobalId))
                    {
                        collisionMap[x, y] = CollisionTypeEnum.BRIDGE;
                    }
                    else if (tileGlobalId != 0)
                    {
                        collisionMap[x, y] = CollisionTypeEnum.SOLID;
                    }
                    else
                    {
                        collisionMap[x, y] = CollisionTypeEnum.AIR;
                    }

                    if (wallLayer != null)
                    {
                        //process wall layer
                        TiledMapTile? t2;
                        wallLayer.TryGetTile((ushort)x, (ushort)y, out t2);
                        tileGlobalId = t2.Value.GlobalIdentifier;
                        if (tileGlobalId != 0)
                        {
                            wallMap[x, y] = true;
                        }
                        else
                        {
                            wallMap[x, y] = false;
                        }
                    }

                    if (grassLayer != null)
                    {
                        //process grass layer
                        TiledMapTile? t3;
                        grassLayer.TryGetTile((ushort)x, (ushort)y, out t3);
                        tileGlobalId = t3.Value.GlobalIdentifier;
                        if (tileGlobalId != 0)
                        {
                            grassMap[x, y + 1] = true;
                        }
                    }

                    if (blockerLayer != null)
                    {
                        //process blocker layer
                        TiledMapTile? t4;
                        blockerLayer.TryGetTile((ushort)x, (ushort)y, out t4);
                        tileGlobalId = t4.Value.GlobalIdentifier;
                        if (tileGlobalId != 0)
                        {
                            blockerMap[x, y] = true;
                        }
                    }

                    if (caveLayer != null)
                    {
                        //process cave layer
                        TiledMapTile? t5;
                        caveLayer.TryGetTile((ushort)x, (ushort)y, out t5);
                        tileGlobalId = t5.Value.GlobalIdentifier;
                        if (tileGlobalId != 0)
                        {
                            caveMap[x, y] = true;
                        }
                    }
                }
            }

            this.name = tiledMap.Properties["name"];
            string seasonProperty = tiledMap.Properties["season"];
            if (seasonProperty == "spring")
            {
                areaSeason = World.Season.SPRING;
            } else if (seasonProperty == "summer")
            {
                areaSeason = World.Season.SUMMER;
            } else if (seasonProperty == "fall" || seasonProperty == "autumn")
            {
                areaSeason = World.Season.AUTUMN;
            } else if (seasonProperty == "winter")
            {
                areaSeason = World.Season.WINTER;
            } else if (seasonProperty == "world" || seasonProperty == "defer")
            {
                areaSeason = World.Season.DEFER;
            } else
            {
                throw new Exception("Map file doesn't have a proper season property!");
            }

            TiledMapObjectLayer transitionLayer = (TiledMapObjectLayer)tiledMap.GetLayer("transitions");
            foreach (TiledMapObject tiledObject in transitionLayer.Objects) {
                RectangleF rectangle = new RectangleF(tiledObject.Position, tiledObject.Size);
                string to = tiledObject.Properties["areaTo"];
                string spawn = tiledObject.Properties["areaSpawn"];
                string automatic = tiledObject.Properties["automatic"];
                string animation = tiledObject.Properties["animation"];
                TransitionZone.Animation anim = TransitionZone.Animation.TO_LEFT;
                if (animation.Equals("up"))
                {
                    anim = TransitionZone.Animation.TO_UP;
                }
                else if (animation.Equals("down"))
                {
                    anim = TransitionZone.Animation.TO_DOWN;
                }
                else if (animation.Equals("right"))
                {
                    anim = TransitionZone.Animation.TO_RIGHT;
                }
                transitions.Add(new TransitionZone(rectangle, to, spawn, automatic.Equals("yes"), anim));
            }

            //ENTITY LAYER
            TiledMapObjectLayer entityLayer = (TiledMapObjectLayer)tiledMap.GetLayer("entity");
            foreach (TiledMapObject tiledObject in entityLayer.Objects)
            {
                string entityType = tiledObject.Properties["entity"];
                if (entityType.Equals("farmhouse"))
                {
                    Vector2 tilePosition = tiledObject.Position / 8 - new Vector2(0, TEntityFarmhouse.HEIGHT);
                    TEntityFarmhouse home = (TEntityFarmhouse)EntityFactory.GetEntity(EntityType.FARMHOUSE, ItemDict.NONE, tilePosition, this);
                    AddTileEntity(home);
                } else if (entityType.Equals("door"))
                {
                    int tileWidth = tiledObject.Properties.ContainsKey("width") ? int.Parse(tiledObject.Properties["width"]) : 2;
                    int tileHeight = 3;
                    Vector2 tilePosition = (tiledObject.Position / 8) - new Vector2(0, tileHeight);
                    TEntityDoor door;
                    if (tiledObject.Properties.ContainsKey("type"))
                    {
                        switch(tiledObject.Properties["type"])
                        {
                            case "elevator":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.ELEVATOR);
                                break;
                            case "cellar":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CELLAR);
                                break;
                            case "cablecar":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CABLECAR);
                                break;
                            case "cellarb1":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CELLARB1);
                                break;
                            case "cellarb2":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CELLARB2);
                                break;
                            case "cellarb3":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CELLARB3);
                                break;
                            case "cellarb4":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CELLARB4);
                                break;
                            case "cellarb5":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CELLARB5);
                                break;
                            case "cellarb6":
                                door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.CELLARB6);
                                break;
                            default:
                                throw new Exception("Illegal door type: " + tiledObject.Properties["type"]);
                        }
                    }
                    else
                    {
                        door = new TEntityDoor(tilePosition, tileWidth, tileHeight, TEntityDoor.DoorType.NORMAL);
                    }
                    AddTileEntity(door);
                } else if (entityType.Equals("storeShelf"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_STORE_SHELF), 18, 2, 10, Util.CreateAndFillArray(18, 3000f));
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    TEntityStoreShelf shelf = new TEntityStoreShelf(tilePosition, sprite, this);
                    AddTileEntity(shelf);
                } else if (entityType.Equals("storeCompost"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_STORE_COMPOST_BIN), 8, 1, 8, Util.CreateAndFillArray(8, 3000f));
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    TEntityStoreCompost compost = new TEntityStoreCompost(tilePosition, sprite);
                    AddTileEntity(compost);
                } else if (entityType.Equals("storeManikin"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_STORE_MANIKIN), 1, 1, 1, Util.CreateAndFillArray(1, 3000f));
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    TEntityStoreManikin manikin = new TEntityStoreManikin(tilePosition, sprite, this);
                    AddTileEntity(manikin);
                } else if (entityType.Equals("shipping_bin"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SHIPPING_BIN_SPRITESHEET), 3, 1, 3, Util.CreateAndFillArray(3, 0.10f));
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    TEntityShippingBin bin = new TEntityShippingBin(tiledObject.Properties["id"], tilePosition, sprite);
                    AddTileEntity(bin);
                } else if (entityType.Equals("storeFurniture"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_STORE_PHANTOM_FURNITURE), 1, 1, 1, Util.CreateAndFillArray(1, 3000f));
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    TEntityStoreFurniture storeFurniture = new TEntityStoreFurniture(tilePosition, sprite);
                    AddTileEntity(storeFurniture);
                } else if (entityType.Equals("sleepable"))
                {
                    string type = tiledObject.Properties["type"];
                    Vector2 tilePosition = tiledObject.Position / 8;
                    Vector2 drawAdjustment;
                    Texture2D texture;
                    switch (type)
                    {
                        case "greentent":
                            drawAdjustment = new Vector2(-8, 0);
                            texture = content.Load<Texture2D>(Paths.SPRITE_GREEN_TENT);
                            break;
                        case "bunkbeds":
                            drawAdjustment = new Vector2(0, 0);
                            texture = content.Load<Texture2D>(Paths.SPRITE_BUNK_BEDS);
                            break;
                        case "redtent":
                            drawAdjustment = new Vector2(-8, 0);
                            texture = content.Load<Texture2D>(Paths.SPRITE_RED_TENT);
                            break;
                        case "bluetent":
                        default:
                            drawAdjustment = new Vector2(-8, 0);
                            texture = content.Load<Texture2D>(Paths.SPRITE_BLUE_TENT);
                            break;
                    }
                    tilePosition.Y -= texture.Height / 8.0f;
                    TEntitySleepable sleepable = new TEntitySleepable(texture, tilePosition, 3, 4, drawAdjustment);
                    AddTileEntity(sleepable);
                } else if (entityType.Equals("bed"))
                {
                    Vector2 tilePosition = tiledObject.Position / 8 - new Vector2(0, TEntityBed.HEIGHT);
                    TEntityBed home = (TEntityBed)EntityFactory.GetEntity(EntityType.BED, ItemDict.NONE, tilePosition, this);
                    AddTileEntity(home);
                } else if (entityType.Equals("mailbox"))
                {
                    Vector2 tilePosition = tiledObject.Position / 8 - new Vector2(0, TEntityMailbox.HEIGHT);
                    TEntityMailbox mailbox = (TEntityMailbox)EntityFactory.GetEntity(EntityType.MAILBOX, ItemDict.NONE, tilePosition, this);
                    AddTileEntity(mailbox);
                }
                else if (entityType.Equals("farmersStall"))
                {
                    string type = tiledObject.Properties["type"];
                    Vector2 tilePosition = tiledObject.Position / 8 - new Vector2(0, 4);
                    TEntityFarmersMarketStall.MarketBehavior behavior;
                    Dictionary<TEntityMarketStall.StallType, TEntityMarketStall> stallDict = new Dictionary<TEntityMarketStall.StallType, TEntityMarketStall>();
                    switch (type)
                    {
                        case "piper":
                            behavior = TEntityFarmersMarketStall.MarketBehavior.PIPER;
                            stallDict[TEntityMarketStall.StallType.FARMERS_FARM_SPRING] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FARM_SPRING), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FARM_SPRING);
                            stallDict[TEntityMarketStall.StallType.FARMERS_FARM_SUMMER] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FARM_SUMMER), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FARM_SUMMER);
                            stallDict[TEntityMarketStall.StallType.FARMERS_FARM_AUTUMN] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FARM_AUTUMN), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FARM_AUTUMN);
                            stallDict[TEntityMarketStall.StallType.FARMERS_JAM] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_JAM), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_JAM);
                            stallDict[TEntityMarketStall.StallType.FARMERS_CLOTHING] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_CLOTHING), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_CLOTHING);
                            stallDict[TEntityMarketStall.StallType.FARMERS_DYES] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_DYES), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_DYES);
                            break;
                        case "troy":
                            behavior = TEntityFarmersMarketStall.MarketBehavior.TROY;
                            stallDict[TEntityMarketStall.StallType.FARMERS_BUTCHER] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_BUTCHER), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_BUTCHER);
                            stallDict[TEntityMarketStall.StallType.FARMERS_FISH] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FISH), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FISH);
                            stallDict[TEntityMarketStall.StallType.FARMERS_MEDIUMS] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_MEDIUMS), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_MEDIUMS);
                            break;
                        case "art":
                            behavior = TEntityFarmersMarketStall.MarketBehavior.ART;
                            stallDict[TEntityMarketStall.StallType.FARMERS_PAINTINGS] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_PAINTINGS), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_PAINTINGS);
                            break;
                        case "forage":
                        default:
                            behavior = TEntityFarmersMarketStall.MarketBehavior.FINLEY;
                            stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_SPRING] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FORAGE_SPRING), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FORAGE_SPRING);
                            stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_SUMMER] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FORAGE_SUMMER), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FORAGE_SUMMER);
                            stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_AUTUMN] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FORAGE_AUTUMN), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FORAGE_AUTUMN);
                            stallDict[TEntityMarketStall.StallType.FARMERS_FORAGE_WINTER] = new TEntityMarketStall(tilePosition,
                                new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_FORAGE_WINTER), 2, 1, 2, Util.CreateAndFillArray(2, 1000f)),
                                TEntityMarketStall.StallType.FARMERS_FORAGE_WINTER);
                            break;

                    }
                    TEntityFarmersMarketStall stall = new TEntityFarmersMarketStall(tilePosition, 4, 4, stallDict, this, content.Load<Texture2D>(Paths.SPRITE_FARMERS_STALL_EMPTY), behavior);
                    AddTileEntity(stall);
                }
                else if (entityType.Equals("marketStall"))
                {
                    string type = tiledObject.Properties["type"];
                    AnimatedSprite sprite;
                    TEntityMarketStall.StallType stallType = TEntityMarketStall.StallType.SPIRIT_INSECT;
                    switch (type)
                    {
                        case "spiritMineral":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_STALL_MINERAL), 2, 1, 2, Util.CreateAndFillArray(2, 1000f));
                            stallType = TEntityMarketStall.StallType.SPIRIT_MINERAL;
                            break;
                        case "spiritInsect":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_STALL_INSECT), 2, 1, 2, Util.CreateAndFillArray(2, 1000f));
                            stallType = TEntityMarketStall.StallType.SPIRIT_INSECT;
                            break;
                        case "spiritFish":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_STALL_FISH), 2, 1, 2, Util.CreateAndFillArray(2, 1000f));
                            stallType = TEntityMarketStall.StallType.SPIRIT_FISH;
                            break;
                        case "spiritFarm":
                        default:
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_STALL_FARM), 2, 1, 2, Util.CreateAndFillArray(2, 1000f));
                            stallType = TEntityMarketStall.StallType.SPIRIT_FARM;
                            break;
                    }
                    Vector2 tilePosition = tiledObject.Position / 8 - new Vector2(0, sprite.GetFrameHeight() / 8);
                    TEntityMarketStall stall = new TEntityMarketStall(tilePosition, sprite, stallType);
                    AddTileEntity(stall);
                } else if (entityType.Equals("spiritWheel"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_WHEEL), 8, 1, 8, Util.CreateAndFillArray(8, 1.0f));
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    TEntitySpiritWheel wheel = new TEntitySpiritWheel(tilePosition, sprite);
                    AddTileEntity(wheel);
                }
                else if (entityType.Equals("geyser"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_GEYSER), 6, 1, 6, Util.CreateAndFillArray(6, 0.10f));
                    sprite.AddLoop("anim", 0, 5, true);
                    sprite.SetLoop("anim");
                    Vector2 tilePosition = tiledObject.Position / 8 - new Vector2(0, sprite.GetFrameHeight() / 8);
                    TEntityGeyser geyser = new TEntityGeyser(tilePosition, sprite);
                    AddTileEntity(geyser);
                }
                else if (entityType.Equals("shrine"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SHRINE_SPRITESHEET), 3, 1, 3, Util.CreateAndFillArray(3, 2000f));
                    AnimatedSprite incenseSprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SHRINE_INCENSE), 1, 1, 1, Util.CreateAndFillArray(1, 1000f));
                    TileEntity shrine = null;
                    string type = tiledObject.Properties["type"].ToLower();
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    if (type.Equals("season"))
                    {
                        TEntityShrine springShrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Spring Shrine", GameState.SHRINE_SEASON_SPRING_1, GameState.SHRINE_SEASON_SPRING_2, GameState.STS_SEASON_SPRING);
                        TEntityShrine summerShrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Summer Shrine", GameState.SHRINE_SEASON_SUMMER_1, GameState.SHRINE_SEASON_SUMMER_2, GameState.STS_SEASON_SUMMER);
                        TEntityShrine autumnShrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Autumn Shrine", GameState.SHRINE_SEASON_AUTUMN_1, GameState.SHRINE_SEASON_AUTUMN_2, GameState.STS_SEASON_AUTUMN);
                        TEntityShrine wintershrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Winter Shrine", GameState.SHRINE_SEASON_WINTER_1, GameState.SHRINE_SEASON_WINTER_2, GameState.STS_SEASON_WINTER);
                        shrine = new TEntitySeasonShrine(tilePosition, sprite, springShrine, summerShrine, autumnShrine, wintershrine, this);
                    } else if (type.Equals("chicken"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Chicken Shrine", GameState.SHRINE_CHICKEN_1, GameState.SHRINE_CHICKEN_2, GameState.STS_CHICKEN);
                    } else if (type.Equals("sheep"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Sheep Shrine", GameState.SHRINE_SHEEP_1, GameState.SHRINE_SHEEP_2, GameState.STS_SHEEP);
                    } else if (type.Equals("cow"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Cow Shrine", GameState.SHRINE_COW_1, GameState.SHRINE_COW_2, GameState.STS_COW);
                    } else if (type.Equals("librarian"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Librarian's Shrine", GameState.SHRINE_LIBRARIAN_1, GameState.SHRINE_LIBRARIAN_2, GameState.STS_LIBRARIAN);
                    } else if (type.Equals("painter"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Painter's Shrine", GameState.SHRINE_PAINTER_1, GameState.SHRINE_PAINTER_2, GameState.STS_PAINTER);
                    } else if (type.Equals("chef"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Chef's Shrine", GameState.SHRINE_CHEF_1, GameState.SHRINE_CHEF_2, GameState.STS_CHEF);
                    } else if (type.Equals("celebration"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Celebration Shrine", GameState.SHRINE_CELEBRATION_1, GameState.SHRINE_CELEBRATION_2, GameState.STS_CELEBRATION);
                    } else if (type.Equals("iron"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Iron Shrine", GameState.SHRINE_IRON_1, GameState.SHRINE_IRON_2, GameState.STS_IRON);
                    }
                    else if (type.Equals("builder"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Builder's Shrine", GameState.SHRINE_BUILDER_1, GameState.SHRINE_BUILDER_2, GameState.STS_BUILDER);
                    }
                    else if (type.Equals("friendship"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Friendship Shrine", GameState.SHRINE_FRIENDSHIP_1, GameState.SHRINE_FRIENDSHIP_2, GameState.STS_FRIENDSHIP);
                    }
                    else if (type.Equals("oldman"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Old Man of the Sea's Shrine", GameState.SHRINE_OLD_MAN_OF_THE_SEA_1, GameState.SHRINE_OLD_MAN_OF_THE_SEA_2, GameState.STS_OLD_MAN_OF_THE_SEA);
                    }
                    else if (type.Equals("seabird"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Seabird Shrine", GameState.SHRINE_SEABIRD_1, GameState.SHRINE_SEABIRD_2, GameState.STS_SEABIRD);
                    }
                    else if (type.Equals("merchant"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Merchant's Shrine", GameState.SHRINE_MERCHANT_1, GameState.SHRINE_MERCHANT_2, GameState.STS_MERCHANT);
                    }
                    else if (type.Equals("alchemist"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Alchemist's Shrine", GameState.SHRINE_ALCHEMIST_1, GameState.SHRINE_ALCHEMIST_2, GameState.STS_ALCHEMIST);
                    }
                    else if (type.Equals("blacksmith"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Blacksmith's Shrine", GameState.SHRINE_BLACKSMITH_1, GameState.SHRINE_BLACKSMITH_2, GameState.STS_BLACKSMITH);
                    }
                    else if (type.Equals("arborist"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Arborist's Shrine", GameState.SHRINE_ARBORIST_1, GameState.SHRINE_ARBORIST_2, GameState.STS_ARBORIST);
                    }
                    else if (type.Equals("fragrant"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Fragrant Shrine", GameState.SHRINE_FRAGRANT_1, GameState.SHRINE_FRAGRANT_2, GameState.STS_FRAGRANT);
                    }
                    else if (type.Equals("insect"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Insect Shrine", GameState.SHRINE_INSECT_1, GameState.SHRINE_INSECT_2, GameState.STS_INSECT);
                    }
                    else if (type.Equals("mountain"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Mountain Shrine", GameState.SHRINE_MOUNTAIN_1, GameState.SHRINE_MOUNTAIN_2, GameState.STS_MOUNTAIN);
                    }
                    else if (type.Equals("boar"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Boar Shrine", GameState.SHRINE_BOAR_1, GameState.SHRINE_BOAR_2, GameState.STS_BOAR);
                    }
                    else if (type.Equals("bee"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Bee Shrine", GameState.SHRINE_BEE_1, GameState.SHRINE_BEE_2, GameState.STS_BEE);
                    }
                    else if (type.Equals("bird"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Bird Shrine", GameState.SHRINE_BIRD_1, GameState.SHRINE_BIRD_2, GameState.STS_BIRD);
                    }
                    else if (type.Equals("labourer"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Labourer's Shrine", GameState.SHRINE_LABOURER_1, GameState.SHRINE_LABOURER_2, GameState.STS_LABOURER);
                    }
                    else if (type.Equals("pond"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Pond Shrine", GameState.SHRINE_POND_1, GameState.SHRINE_POND_1, GameState.STS_POND);
                    } else if (type.Equals("potters"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Potter's Shrine", GameState.SHRINE_POTTER_1, GameState.SHRINE_POTTER_2, GameState.STS_POTTER);
                    }
                    else if (type.Equals("bat"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Bat Shrine", GameState.SHRINE_BAT_1, GameState.SHRINE_BAT_2, GameState.STS_BAT);
                    }
                    else if (type.Equals("groundwater"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Groundwater Shrine", GameState.SHRINE_GROUNDWATER_1, GameState.SHRINE_GROUNDWATER_2, GameState.STS_GROUNDWATER);
                    }
                    else if (type.Equals("shining"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Shining Shrine", GameState.SHRINE_SHINING_1, GameState.SHRINE_SHINING_2, GameState.STS_SHINING);
                    }
                    else if (type.Equals("mushroom"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Mushroom Shrine", GameState.SHRINE_MUSHROOM_1, GameState.SHRINE_MUSHROOM_2, GameState.STS_MUSHROOM);
                    }
                    else if (type.Equals("cavers"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Caver's Shrine", GameState.SHRINE_CAVER_1, GameState.SHRINE_CAVER_2, GameState.STS_CAVER);
                    }
                    else if (type.Equals("concrete"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Concrete Shrine", GameState.SHRINE_CONCRETE_1, GameState.SHRINE_CONCRETE_2, GameState.STS_CONCRETE);
                    }
                    else if (type.Equals("cavers"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Caver's Shrine", GameState.SHRINE_CAVER_1, GameState.SHRINE_CAVER_2, GameState.STS_CAVER);
                    }
                    else if (type.Equals("jungle"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Jungle Shrine", GameState.SHRINE_JUNGLE_1, GameState.SHRINE_JUNGLE_2, GameState.STS_JUNGLE);
                    }
                    else if (type.Equals("volcano"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Volcano Shrine", GameState.SHRINE_VOLCANO_1, GameState.SHRINE_VOLCANO_2, GameState.STS_VOLCANO);
                    }
                    else if (type.Equals("weavers"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Weaver's Shrine", GameState.SHRINE_WEAVER_1, GameState.SHRINE_WEAVER_2, GameState.STS_WEAVER);
                    }
                    else if (type.Equals("golden"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Golden Shrine", GameState.SHRINE_GOLDEN_1, GameState.SHRINE_GOLDEN_2, GameState.STS_GOLDEN);
                    }
                    else if (type.Equals("legendary"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Legendary Shrine", GameState.SHRINE_LEGENDARY_1, GameState.SHRINE_LEGENDARY_2, GameState.STS_LEGENDARY);
                    }
                    else if (type.Equals("archaeologist"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Archaeologist's Shrine", GameState.SHRINE_ARCHAEOLOGIST_1, GameState.SHRINE_ARCHAEOLOGIST_2, GameState.STS_ARCHAEOLOGIST);
                    }
                    else if (type.Equals("myth"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Myth Shrine", GameState.SHRINE_MYTH_1, GameState.SHRINE_MYTH_2, GameState.STS_MYTH);
                    }
                    else if (type.Equals("shrubbery"))
                    {
                        shrine = new TEntityShrine(tilePosition, sprite, incenseSprite, "Shrubbery Shrine", GameState.SHRINE_SHRUBBERY_1, GameState.SHRINE_SHRUBBERY_2, GameState.STS_SHRUBBERY);
                    }
                    else
                    {
                        int x = 0;
                        x = x / x;
                    }

                    AddTileEntity(shrine);
                } else if (entityType.Equals("directorZone"))
                {
                    float veloX = Int32.Parse(tiledObject.Properties["velocityX"]);
                    float veloY = Int32.Parse(tiledObject.Properties["velocityY"]);
                    MovingPlatformDirectorZone zone = new MovingPlatformDirectorZone(new RectangleF(tiledObject.Position, tiledObject.Size), new Vector2(veloX, veloY));
                    directorZones.Add(zone);
                } else if (entityType.Equals("platform"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_MOVING_PLATFORM), 1, 1, 1, Util.CreateAndFillArray(1, 2000f));
                    float veloX = Int32.Parse(tiledObject.Properties["velocityX"]);
                    float veloY = Int32.Parse(tiledObject.Properties["velocityY"]);
                    EntitySolid.PlatformType platType = EntitySolid.PlatformType.SOLID;
                    string platTypeStr = tiledObject.Properties["type"].ToLower();
                    switch (platTypeStr)
                    {
                        case "solid":
                            platType = EntitySolid.PlatformType.SOLID;
                            break;
                        case "bridge":
                            platType = EntitySolid.PlatformType.BRIDGE;
                            break;
                    }
                    bool collideWithTerrain = tiledObject.Properties["collideTerrain"].ToLower().Equals("true");
                    EntityMovingPlatform platform = new EntityMovingPlatform(tiledObject.Position, sprite, player, platType, new Vector2(veloX, veloY), collideWithTerrain);
                    entityListManager.Add(platform);
                }
                else if (entityType.Equals("solid"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_MOVING_PLATFORM), 1, 1, 1, Util.CreateAndFillArray(1, 2000f));
                    EntitySolid.PlatformType platType = EntitySolid.PlatformType.SOLID;
                    string platTypeStr = tiledObject.Properties["type"].ToLower();
                    switch (platTypeStr)
                    {
                        case "solid":
                            platType = EntitySolid.PlatformType.SOLID;
                            break;
                        case "bridge":
                            platType = EntitySolid.PlatformType.BRIDGE;
                            break;
                    }
                    bool collideWithTerrain = tiledObject.Properties["collideTerrain"].ToLower().Equals("true");
                    EntitySolid platform = new EntitySolid(tiledObject.Position, sprite, player, platType, collideWithTerrain);
                    entityListManager.Add(platform);
                }
                else if (entityType.Equals("trampoline") || entityType.Equals("trompolineUp"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_TRAMPOLINE), 6, 1, 6, Util.CreateAndFillArray(6, 0.075f));
                    float veloX = float.Parse(tiledObject.Properties["velocityX"]);
                    float veloY = float.Parse(tiledObject.Properties["velocityY"]);
                    sprite.AddLoop("idle", 0, 0, true, false);
                    sprite.AddLoop("anim", 1, 5, false, false);
                    sprite.SetLoop("idle");
                    bool forceRoll = false;
                    if (tiledObject.Properties.ContainsKey("forceRoll"))
                    {
                        forceRoll = bool.Parse(tiledObject.Properties["forceRoll"]);
                    }
                    EntityTrampoline trampoline = new EntityTrampoline(sprite, tiledObject.Position + new Vector2(-2, -sprite.GetFrameHeight() + 1), new Vector2(veloX, veloY), EntityTrampoline.TrampolineType.UP, forceRoll);
                    entityListManager.Add(trampoline);
                } else if (entityType.Equals("trampolineLeft"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_TRAMPOLINE_SIDE), 6, 1, 6, Util.CreateAndFillArray(6, 0.075f));
                    float veloX = float.Parse(tiledObject.Properties["velocityX"]);
                    float veloY = float.Parse(tiledObject.Properties["velocityY"]);
                    sprite.AddLoop("idle", 0, 0, true, false);
                    sprite.AddLoop("anim", 1, 5, false, false);
                    sprite.SetLoop("idle");
                    bool forceRoll = false;
                    if (tiledObject.Properties.ContainsKey("forceRoll"))
                    {
                        forceRoll = bool.Parse(tiledObject.Properties["forceRoll"]);
                    }
                    EntityTrampoline trampoline = new EntityTrampoline(sprite, tiledObject.Position + new Vector2(-sprite.GetFrameWidth() + 1, -2), new Vector2(veloX, veloY), EntityTrampoline.TrampolineType.LEFT, forceRoll);
                    entityListManager.Add(trampoline);
                } else if (entityType.Equals("trampolineRight"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_TRAMPOLINE_SIDE), 6, 1, 6, Util.CreateAndFillArray(6, 0.075f));
                    float veloX = float.Parse(tiledObject.Properties["velocityX"]);
                    float veloY = float.Parse(tiledObject.Properties["velocityY"]);
                    sprite.AddLoop("idle", 0, 0, true, false);
                    sprite.AddLoop("anim", 1, 5, false, false);
                    sprite.SetLoop("idle");
                    bool forceRoll = false;
                    if (tiledObject.Properties.ContainsKey("forceRoll"))
                    {
                        forceRoll = bool.Parse(tiledObject.Properties["forceRoll"]);
                    }
                    EntityTrampoline trampoline = new EntityTrampoline(sprite, tiledObject.Position + new Vector2(-1, -2), new Vector2(veloX, veloY), EntityTrampoline.TrampolineType.RIGHT, forceRoll);
                    entityListManager.Add(trampoline);
                } else if (entityType.Equals("trampolineDown"))
                {
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_TRAMPOLINE), 6, 1, 6, Util.CreateAndFillArray(6, 0.075f));
                    float veloX = float.Parse(tiledObject.Properties["velocityX"]);
                    float veloY = float.Parse(tiledObject.Properties["velocityY"]);
                    sprite.AddLoop("idle", 0, 0, true, false);
                    sprite.AddLoop("anim", 1, 5, false, false);
                    sprite.SetLoop("idle");
                    bool forceRoll = false;
                    if (tiledObject.Properties.ContainsKey("forceRoll"))
                    {
                        forceRoll = bool.Parse(tiledObject.Properties["forceRoll"]);
                    }
                    EntityTrampoline trampoline = new EntityTrampoline(sprite, tiledObject.Position + new Vector2(-2, -1), new Vector2(veloX, veloY), EntityTrampoline.TrampolineType.DOWN, forceRoll);
                    entityListManager.Add(trampoline);
                } else if (entityType.Equals("windCurrent"))
                {
                    World.Season season = World.Season.NONE;
                    string seasonStr = tiledObject.Properties["season"].ToLower();
                    switch (seasonStr)
                    {
                        case "spring":
                            season = World.Season.SPRING;
                            break;
                        case "summer":
                            season = World.Season.SUMMER;
                            break;
                        case "fall":
                        case "autumn":
                            season = World.Season.AUTUMN;
                            break;
                        case "winter":
                            season = World.Season.WINTER;
                            break;
                        case "defer":
                            season = World.Season.DEFER;
                            break;
                    }
                    EntityWindCurrent windCurrent = new EntityWindCurrent(new RectangleF(tiledObject.Position, tiledObject.Size), this, season);
                    entityListManager.Add(windCurrent);
                } else if (entityType.Equals("reverseCrystal") || entityType.Equals("antigravity_machine"))
                {
                    float[] frameLengths = Util.CreateAndFillArray(2, 10000f);
                    AnimatedSprite sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_ANTIGRAVITY_MACHINE), 2, 1, 2, frameLengths);
                    sprite.AddLoop("normal", 0, 0, true, false);
                    sprite.AddLoop("reversed", 1, 1, true, false);
                    sprite.SetLoop("normal");
                    bool upsideDown = tiledObject.Properties["type"].Equals("down");
                    Vector2 tilePosition = (tiledObject.Position / 8.0f) - new Vector2(0, sprite.GetFrameHeight() / 8.0f);
                    if (upsideDown)
                    {
                        //todo
                        tilePosition += new Vector2(0, sprite.GetFrameHeight() / 8);
                    }
                    //TEntityAntigravityMachine antigrav_mach = new TEntityAntigravityMachine(tiledObject.Position - (upsideDown ? new Vector2(0, 1) : new Vector2(0, sprite.GetFrameHeight() - 1)), sprite, upsideDown);
                    TEntityAntigravityMachine antigrav_mach = new TEntityAntigravityMachine(tilePosition, sprite, upsideDown);
                    AddTileEntity(antigrav_mach);
                    //entityListManager.Add(antigrav_mach);
                } else if (entityType.Equals("placeable"))
                {
                    string baseItem = tiledObject.Properties["item"];
                    PlaceableItem itemForm = (PlaceableItem)ItemDict.GetItemByName(baseItem);
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, itemForm.GetPlaceableHeight());
                    PlacedEntity toAdd = (PlacedEntity)EntityFactory.GetEntity(EntityType.USE_ITEM, itemForm, position, this);
                    for (int i = 0; i < 10; i++)
                    {
                        toAdd.Update(1.0f, this); //skip animation
                    }
                    toAdd.MarkAsUnremovable();
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("lightsource"))
                {
                    Vector2 position = tiledObject.Position / 8.0f;
                    PlacedEntity toAdd;
                    string strengthStr = tiledObject.Properties["strength"].ToLower();
                    if (strengthStr.Equals("big") || strengthStr.Equals("large"))
                    {
                        toAdd = (PlacedEntity)EntityFactory.GetEntity(EntityType.LIGHTSOURCE_LARGE, ItemDict.NONE, position, this);
                    } else if (strengthStr.Equals("medium"))
                    {
                        toAdd = (PlacedEntity)EntityFactory.GetEntity(EntityType.LIGHTSOURCE_MEDIUM, ItemDict.NONE, position, this);
                    } else
                    {
                        toAdd = (PlacedEntity)EntityFactory.GetEntity(EntityType.LIGHTSOURCE_SMALL, ItemDict.NONE, position, this);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        toAdd.Update(1.0f, this); //skip animation
                    }
                    toAdd.MarkAsUnremovable();
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("minecart"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.MINECART, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("vending_machine"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 3);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.VENDING_MACHINE, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("filing_cabinet"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 3);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.FILING_CABINET, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                } else if (entityType.Equals("sci_table1"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.SCI_TABLE1, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                } else if (entityType.Equals("sci_table2"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.SCI_TABLE2, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("sandcastle"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.SANDCASTLE, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("red_sandcastle"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.RED_SANDCASTLE, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("stump"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.STUMP, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("boar_trap"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.BOAR_TRAP, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("trashcan"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.TRASHCAN, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("bamboo_pot"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 3);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.BAMBOO_POT, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("ancient_chest"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.ANCIENT_CHEST, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("crystal_chest"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.CRYSTAL_CHEST, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("sedimentary_chest"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.SEDIMENTARY_CHEST, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("igneous_chest"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.IGNEOUS_CHEST, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("metamorphic_chest"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.METAMORPHIC_CHEST, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("gem_rock"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 3);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.GEM_ROCK, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                }
                else if (entityType.Equals("fossil_rock"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(EntityType.FOSSIL_ROCK, ItemDict.NONE, position, this);
                    AddTileEntity(toAdd);
                } else if (entityType.Equals("signpost") || entityType.Equals("signpost_tech") || entityType.Equals("signpost_street"))
                {
                    Vector2 position = (tiledObject.Position / 8.0f) - new Vector2(0, 2);
                    EntityType enType = EntityType.SIGNPOST;
                    if (entityType.Equals("signpost_tech"))
                    {
                        enType = EntityType.SIGNPOST_TECH;
                    } else if (entityType.Equals("signpost_street"))
                    {
                        enType = EntityType.SIGNPOST_STREET;
                        position = position - new Vector2(0, 1);
                    }
                    TileEntity toAdd = (TileEntity)EntityFactory.GetEntity(enType, ItemDict.NONE, position, this);
                    DialogueNode signText = new DialogueNode(tiledObject.Properties["text"], DialogueNode.PORTRAIT_SYSTEM);
                    ((TEntitySignpost)toAdd).SetDialogueNode(signText);
                    AddTileEntity(toAdd);
                } else if (entityType.Equals("spirit"))
                {
                    string spriteType = tiledObject.Properties["sprite"];
                    AnimatedSprite sprite;
                    EntitySpirit.Element element = EntitySpirit.Element.SUN;
                    switch (spriteType)
                    {
                        case "sun_sm":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_SUN_SM), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.SUN;
                            break;
                        case "sun_lg":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_SUN_LG), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.SUN;
                            break;
                        case "sun_sm_towel":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_SUN_SM_TOWEL), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.SUN;
                            break;
                        case "sun_lg_towel":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_SUN_LG_TOWEL), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.SUN;
                            break;
                        case "leaf_sm":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_LEAF_SM), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.LEAF;
                            break;
                        case "leaf_lg":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_LEAF_LG), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.LEAF;
                            break;
                        case "wood_sm":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_WOOD_SM), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.WOOD;
                            break;
                        case "wood_lg":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_WOOD_LG), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.WOOD;
                            break;
                        case "water_sm":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_WATER_SM), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.WATER;
                            break;
                        case "water_lg":
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_WATER_LG), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            element = EntitySpirit.Element.WATER;
                            break;
                        default:
                            sprite = new AnimatedSprite(content.Load<Texture2D>(Paths.SPRITE_SPIRIT_LEAF_LG), 8, 1, 8, Util.CreateAndFillArray(8, 0.1f));
                            break;
                    }
                    sprite.AddLoop("idleL", 4, 4, true);
                    sprite.AddLoop("idleR", 0, 0, true);
                    sprite.AddLoop("walkL", 5, 7, true);
                    sprite.AddLoop("walkR", 1, 3, true);
                    sprite.SetLoop("idleR");

                    string spiritType = tiledObject.Properties["type"];

                    DialogueNode[] dialogues;
                    string[] dialogueStrings = tiledObject.Properties["dialogue"].Split('|');
                    dialogues = new DialogueNode[dialogueStrings.Length];

                    bool moves = bool.Parse(tiledObject.Properties["moves"]);
                    for (int i = 0; i < dialogueStrings.Length; i++)
                    {
                        dialogues[i] = new DialogueNode(dialogueStrings[i], DialogueNode.PORTRAIT_SYSTEM);
                    }

                    Entity toAdd;
                    switch (spiritType)
                    {
                        case "foodie":
                            toAdd = new EntitySpiritFoodie(sprite, tiledObject.Position - new Vector2(0, sprite.GetFrameHeight()), element, moves);
                            break;
                        case "normal":
                            toAdd = new EntitySpirit(sprite, tiledObject.Position - new Vector2(0, sprite.GetFrameHeight()), element, dialogues, moves);
                            break;
                        case "archaeologist":
                        default:
                            toAdd = new EntitySpiritArchaeologist(sprite, tiledObject.Position - new Vector2(0, sprite.GetFrameHeight()), element, moves);
                            break;
                    }
                    entityListManager.Add(toAdd);
                }
            }

            TiledMapObjectLayer waypointLayer = (TiledMapObjectLayer)tiledMap.GetLayer("waypoints");
            foreach (TiledMapObject tiledObject in waypointLayer.Objects)
            {
                Vector2 position = tiledObject.Position;
                string waypointName = tiledObject.Properties["name"];
                Vector2 camLock = new Vector2(-10000, -10000);
                if (tiledObject.Properties.ContainsKey("cameraX") && tiledObject.Properties.ContainsKey("cameraY"))
                {
                    camLock.X = Int32.Parse(tiledObject.Properties["cameraX"]);
                    camLock.Y = Int32.Parse(tiledObject.Properties["cameraY"]);
                }
                waypoints.Add(new Waypoint(position, waypointName, camLock, this));
            }

            //read pathinghelper layer
            //read fishingzone layer
            TiledMapObjectLayer pathingLayer = (TiledMapObjectLayer)tiledMap.GetLayer("pathing");
            foreach (TiledMapObject tiledObject in pathingLayer.Objects)
            {
                string type = tiledObject.Properties["type"];
                PathingHelper.Type pht;
                if (!Enum.TryParse(type.ToUpper(), out pht))
                {
                    throw new Exception("Couldn't parse " + type.ToUpper() + " to enum...");
                }
                pathingHelpers.Add(new PathingHelper(new RectangleF(tiledObject.Position, tiledObject.Size), pht));
            }

            //read water layer
            TiledMapTileLayer waterLayer = (TiledMapTileLayer)tiledMap.GetLayer("water");
            for (int x = 0; x < baseLayer.Width; x++)
            {
                for (int y = 0; y < baseLayer.Height; y++)
                {
                    TiledMapTile? t;
                    waterLayer.TryGetTile((ushort)x, (ushort)y, out t);

                    int tileGlobalId = t.Value.GlobalIdentifier;
                    if (collisionMap[x, y] == CollisionTypeEnum.AIR)
                    {
                        if (Util.ArrayContains(WATER_TILE_IDS, tileGlobalId))
                        {
                            collisionMap[x, y] = CollisionTypeEnum.WATER;
                        } else if (Util.ArrayContains(DEEP_WATER_TILE_IDS, tileGlobalId))
                        {
                            collisionMap[x, y] = CollisionTypeEnum.DEEP_WATER;
                        } else if (Util.ArrayContains(WATER_TOPPER_IDs, tileGlobalId))
                        {
                            collisionMap[x, y] = CollisionTypeEnum.TOP_WATER;

                            Texture2D topperTex;
                            int frames = 3;
                            float frameSpeed = 0.25f;

                            if(tileGlobalId == WATER_PURE_TOPPER_ID)
                            {
                                topperTex = content.Load<Texture2D>(Paths.SPRITE_WATER_TOPPER_PURE);
                            } else if (tileGlobalId == WATER_SWAMP_TOPPER_ID)
                            {
                                topperTex = content.Load<Texture2D>(Paths.SPRITE_WATER_TOPPER_SWAMP);
                            } else if (tileGlobalId == WATER_LAVA_TOPPER_ID)
                            {
                                topperTex = content.Load<Texture2D>(Paths.SPRITE_WATER_TOPPER_LAVA);
                            } else if (tileGlobalId == WATER_CLOUD_TOPPER_ID)
                            {
                                topperTex = content.Load<Texture2D>(Paths.SPRITE_WATER_TOPPER_CLOUD);
                                frames = 8;
                                frameSpeed = 0.5f;
                            } else if (tileGlobalId == WATER_CLOUD_MID_TOPPER_ID)
                            {
                                topperTex = content.Load<Texture2D>(Paths.SPRITE_WATER_TOPPER);
                                frames = 4;
                                frameSpeed = 0.35f;
                            } else if (tileGlobalId == WATER_TOPPER_ID)
                            {
                                topperTex = content.Load<Texture2D>(Paths.SPRITE_WATER_TOPPER);
                            } else
                            {
                                topperTex = content.Load<Texture2D>(Paths.SPRITE_FRUIT_TREE);
                                int wyz = 0;
                                wyz = wyz / wyz;
                            }

                            AnimatedSprite sprite = new AnimatedSprite(topperTex, frames, 1, frames, Util.CreateAndFillArray(frames, frameSpeed));
                            sprite.AddLoop("anim", 0, frames-1, true, false);
                            sprite.SetLoop("anim");
                            TEntityWaterTopper waterTop = new TEntityWaterTopper(new Vector2(x, y), sprite, 0.70f, DrawLayer.FOREGROUND);
                            AddTileEntity(waterTop);

                            AnimatedSprite sprite2 = new AnimatedSprite(topperTex, frames, 1, frames, Util.CreateAndFillArray(frames, frameSpeed));
                            sprite2.AddLoop("anim", 0, frames-1, true, false);
                            sprite2.SetLoop("anim");
                            TEntityWaterTopper waterTopBack = new TEntityWaterTopper(new Vector2(x, y), sprite2, 1.0f, DrawLayer.BACKGROUND_BEHIND_WALL);
                            AddTileEntity(waterTopBack);
                        }
                    }
                }
            }

            //read fishingzone layer
            TiledMapObjectLayer fishingzoneLayer = (TiledMapObjectLayer)tiledMap.GetLayer("fishingzones");
            foreach (TiledMapObject tiledObject in fishingzoneLayer.Objects)
            {
                string pool = tiledObject.Properties["pool"];
                int difficulty = Int32.Parse(tiledObject.Properties["difficulty"]);
                LootTables.LootTable table = LootTables.WEEDS;
                if (pool.Equals("ocean"))
                {
                    table = LootTables.FISH_OCEAN;
                }

                fishingZones.Add(new FishingZone(difficulty, table, new RectangleF(tiledObject.Position, tiledObject.Size)));
            }

            //read namedzone layer
            nameZones = new List<NamedZone>();
            TiledMapObjectLayer namezoneLayer = (TiledMapObjectLayer)tiledMap.GetLayer("zones");
            foreach (TiledMapObject tiledObject in namezoneLayer.Objects)
            {
                string zoneName = tiledObject.Properties["name"];
                nameZones.Add(new NamedZone(zoneName, new RectangleF(tiledObject.Position, tiledObject.Size)));
            }

            //read namedzone layer
            lightingZones = new List<LightingZone>();
            TiledMapObjectLayer lightingzoneLayer = (TiledMapObjectLayer)tiledMap.GetLayer("lighting");
            foreach (TiledMapObject tiledObject in lightingzoneLayer.Objects)
            {
                float darkLevel = float.Parse(tiledObject.Properties["darkLevel"]);
                lightingZones.Add(new LightingZone(new RectangleF(tiledObject.Position, tiledObject.Size), darkLevel));
            }

            //read soundzones
            soundZones = new List<SoundZone>();
            TiledMapObjectLayer soundzoneLayer = (TiledMapObjectLayer)tiledMap.GetLayer("sounds");
            foreach (TiledMapObject tiledObject in soundzoneLayer.Objects)
            {
                SoundSystem.Sound sound;
                World.TimeOfDay time;
                World.Season season;
                if (!Enum.TryParse(tiledObject.Properties["sound"], out sound))
                {
                    throw new Exception("Couldn't parse " + tiledObject.Properties["sound"] + " to enum...");
                }
                if (!Enum.TryParse(tiledObject.Properties["time"], out time))
                {
                    throw new Exception("Couldn't parse " + tiledObject.Properties["time"] + " to enum...");
                }
                if (!Enum.TryParse(tiledObject.Properties["season"], out season))
                {
                    throw new Exception("Couldn't parse " + tiledObject.Properties["season"] + " to enum...");
                }
                soundZones.Add(new SoundZone(new RectangleF(tiledObject.Position, tiledObject.Size), sound, time, season));
            }

            //read subarea layer
            subareas = new List<Subarea>();
            TiledMapObjectLayer subareaLayer = (TiledMapObjectLayer)tiledMap.GetLayer("subareas");
            foreach (TiledMapObject tiledObject in subareaLayer.Objects)
            {
                string subareaName = tiledObject.Properties["name"];
                subareas.Add(new Subarea(new RectangleF(tiledObject.Position, tiledObject.Size), subareaName));
            }

            //read cutscene trigger layer
            cutsceneTriggerZones = new List<CutsceneTriggerZone>();
            TiledMapObjectLayer cutsceneTriggerLayer = (TiledMapObjectLayer)tiledMap.GetLayer("cutscenes");
            foreach (TiledMapObject tiledObject in cutsceneTriggerLayer.Objects)
            {
                string cutsceneID = tiledObject.Properties["id"];
                cutsceneTriggerZones.Add(new CutsceneTriggerZone(cutsceneID, new RectangleF(tiledObject.Position, tiledObject.Size)));
            }

            //read spawnzone layer
            TiledMapObjectLayer spawnzoneLayer = (TiledMapObjectLayer)tiledMap.GetLayer("spawnzones");
            foreach (TiledMapObject tiledObject in spawnzoneLayer.Objects)
            {
                //Get Spawnzone Type: Floor, Ceiling, FloorBridge
                string type = tiledObject.Properties["type"];
                List<XYTile> validTiles = new List<XYTile>();
                bool ceil = false;
                bool bridgesAllowed = false;

                if (type.Equals("ground") || type.Equals("groundbridge"))
                {
                    for (int x = (int)tiledObject.Position.X; x <= tiledObject.Size.Width + tiledObject.Position.X; x += 8)
                    {
                        for (int y = (int)tiledObject.Position.Y; y <= tiledObject.Size.Height + tiledObject.Position.Y; y += 8)
                        {
                            XYTile toCheck = new XYTile(x / 8, y / 8);
                            if (IsTileEntityPlacementValid(toCheck.tileX, toCheck.tileY, 1, 1, type.Equals("groundbridge") ? true : false))
                            {
                                validTiles.Add(toCheck);
                            }
                        }
                    }

                    if (type.Equals("groundbridge"))
                    {
                        bridgesAllowed = true;
                    }
                } else if (type.Equals("ceiling"))
                {
                    for (int x = (int)tiledObject.Position.X; x <= tiledObject.Size.Width + tiledObject.Position.X; x += 8)
                    {
                        for (int y = (int)tiledObject.Position.Y; y <= tiledObject.Size.Height + tiledObject.Position.Y; y += 8)
                        {
                            XYTile toCheck = new XYTile(x / 8, y / 8);
                            if (IsCeilingTileEntityPlacementValid(toCheck.tileX, toCheck.tileY, 1, 1))
                            {
                                
                                validTiles.Add(toCheck);
                                //System.Diagnostics.Debug.WriteLine("FOUND A VALID CEIL TILE: " + toCheck.tileX + "   " + toCheck.tileY);
                            }
                        }
                    }
                    ceil = true;
                } else
                {
                    System.Diagnostics.Debug.WriteLine("TYPE IS INCORRECTLY SET TO: " + type);
                }

                if (validTiles.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("NO VALID TYPES" + type);
                    int exep = 0;
                    int x = 10 / exep;
                }

                SpawnZone spawnZone = new SpawnZone(validTiles, ceil, bridgesAllowed);
                foreach (string key in tiledObject.Properties.Keys)
                {
                    if (key.Equals("type"))
                    {
                        continue;
                    }

                    string entity = key;
                    float frequency = float.Parse(tiledObject.Properties[entity]);

                    if (entity.Equals("branch"))
                    {
                        spawnZone.AddEntry(EntityType.BRANCH, frequency / 2, World.Season.NONE);
                        spawnZone.AddEntry(EntityType.BRANCH_LARGE, frequency / 2, World.Season.NONE);
                    } else if (entity.Equals("rock") || entity.Equals("stone"))
                    {
                        spawnZone.AddEntry(EntityType.ROCK, frequency / 2, World.Season.NONE);
                        spawnZone.AddEntry(EntityType.ROCK_LARGE, frequency / 2, World.Season.NONE);
                    } else if (entity.Equals("weed") || entity.Equals("weeds"))
                    {
                        spawnZone.AddEntry(EntityType.WEEDS, frequency, World.Season.NONE);
                    } else if (entity.Equals("bush"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_BUSH, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("pine_tree"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_PINE_TREE, frequency, World.Season.NONE);
                    } else if (entity.Equals("seasonal_forage"))
                    {
                        spawnZone.AddEntry(EntityType.CHICKWEED, frequency / 3.0f, World.Season.SPRING);
                        spawnZone.AddEntry(EntityType.BLUEBELL, frequency / 3.0f, World.Season.SPRING);
                        spawnZone.AddEntry(EntityType.NETTLES, frequency / 3.0f, World.Season.SPRING);

                        spawnZone.AddEntry(EntityType.SUNFLOWER, frequency / 3.0f, World.Season.SUMMER);
                        spawnZone.AddEntry(EntityType.MARIGOLD, frequency / 3.0f, World.Season.SUMMER);
                        spawnZone.AddEntry(EntityType.LAVENDER, frequency / 3.0f, World.Season.SUMMER);

                        spawnZone.AddEntry(EntityType.FALL_LEAF_PILE, frequency, World.Season.AUTUMN);

                        spawnZone.AddEntry(EntityType.WINTER_SNOW_PILE, frequency, World.Season.WINTER);
                    } else if (entity.Equals("crate"))
                    {
                        spawnZone.AddEntry(EntityType.CRATE, frequency * 0.75f, World.Season.NONE);
                        spawnZone.AddEntry(EntityType.CRATE_PILE, frequency * 0.25f, World.Season.NONE);
                    } else if (entity.Equals("beach"))
                    {
                        spawnZone.AddEntry(EntityType.BEACH_FORAGE, frequency, World.Season.NONE);
                    } else if (entity.Equals("beach_shell"))
                    {
                        spawnZone.AddEntry(EntityType.SHELL, frequency, World.Season.NONE);
                    } else if (entity.Equals("palm_tree_coconut"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_COCONUT_PALM, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("palm_tree_banana"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_BANANA_PALM, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("red_ginger"))
                    {
                        spawnZone.AddEntry(EntityType.RED_GINGER, frequency, World.Season.SUMMER);
                    } else if (entity.Equals("salt_rock"))
                    {
                        spawnZone.AddEntry(EntityType.SALT_ROCK, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("lava_rock"))
                    {
                        spawnZone.AddEntry(EntityType.LAVA_ROCK, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("lava_gold_rock"))
                    {
                        spawnZone.AddEntry(EntityType.LAVA_GOLD_ROCK, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("iron_rock"))
                    {
                        spawnZone.AddEntry(EntityType.IRON_ROCK, frequency, World.Season.NONE);
                    } else if (entity.Equals("coal_rock"))
                    {
                        spawnZone.AddEntry(EntityType.COAL_ROCK, frequency, World.Season.NONE);
                    } else if (entity.Equals("paintcan") || entity.Equals("paintcans"))
                    {
                        spawnZone.AddEntry(EntityType.PAINTCANS, frequency, World.Season.NONE);
                    } else if (entity.Equals("dynamite"))
                    {
                        spawnZone.AddEntry(EntityType.DYNAMITE, frequency, World.Season.NONE);
                    } else if (entity.Equals("apple_tree"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_APPLE_TREE, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("cherry_tree"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_CHERRY_TREE, frequency, World.Season.NONE);
                    } else if (entity.Equals("bamboo"))
                    {
                        spawnZone.AddEntry(EntityType.BAMBOO, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("mountain_wheat"))
                    {
                        spawnZone.AddEntry(EntityType.MOUNTAIN_WHEAT, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("morel"))
                    {
                        spawnZone.AddEntry(EntityType.MOREL, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("bluebell"))
                    {
                        spawnZone.AddEntry(EntityType.BLUEBELL, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("nettles"))
                    {
                        spawnZone.AddEntry(EntityType.NETTLES, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("sunflower"))
                    {
                        spawnZone.AddEntry(EntityType.SUNFLOWER, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("chickweed"))
                    {
                        spawnZone.AddEntry(EntityType.CHICKWEED, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("marigold"))
                    {
                        spawnZone.AddEntry(EntityType.MARIGOLD, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("lavender"))
                    {
                        spawnZone.AddEntry(EntityType.LAVENDER, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("sunflower"))
                    {
                        spawnZone.AddEntry(EntityType.LAVENDER, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("spicy_leaf"))
                    {
                        spawnZone.AddEntry(EntityType.SPICY_LEAF, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("wind_crystal"))
                    {
                        spawnZone.AddEntry(EntityType.WIND_CRYSTAL, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("pottery"))
                    {
                        spawnZone.AddEntry(EntityType.POTTERY, frequency, World.Season.NONE);
                    } else if (entity.Equals("emerald_moss"))
                    {
                        spawnZone.AddEntry(EntityType.EMERALD_MOSS, frequency, World.Season.NONE);
                    } else if (entity.Equals("cave_soybean"))
                    {
                        spawnZone.AddEntry(EntityType.CAVE_SOYBEAN, frequency, World.Season.NONE);
                    } else if (entity.Equals("cave_fungi"))
                    {
                        spawnZone.AddEntry(EntityType.CAVE_FUNGI, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("lemon_tree"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_LEMON_TREE, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("water_crystal"))
                    {
                        spawnZone.AddEntry(EntityType.WATER_CRYSTAL, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("stalagmite"))
                    {
                        spawnZone.AddEntry(EntityType.STALAGMITE, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("stalactite"))
                    {
                        spawnZone.AddEntry(EntityType.STALACTITE, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("mythril_machine"))
                    {
                        spawnZone.AddEntry(EntityType.MYTHRIL_MACHINE, frequency, World.Season.NONE);
                    } else if (entity.Equals("mythril_rock"))
                    {
                        spawnZone.AddEntry(EntityType.MYTHRIL_ROCK, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("cacao_bean") || entity.Equals("cacao"))
                    {
                        spawnZone.AddEntry(EntityType.CACAO, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("vanilla_bean") || entity.Equals("vanilla"))
                    {
                        spawnZone.AddEntry(EntityType.VANILLA_BEAN, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("maize"))
                    {
                        spawnZone.AddEntry(EntityType.MAIZE, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("pineapple"))
                    {
                        spawnZone.AddEntry(EntityType.PINEAPPLE, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("gold_rock"))
                    {
                        spawnZone.AddEntry(EntityType.GOLD_ROCK, frequency, World.Season.NONE);
                    } else if (entity.Equals("orange_tree"))
                    {
                        spawnZone.AddEntry(EntityType.WILD_ORANGE_TREE, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("earth_crystal"))
                    {
                        spawnZone.AddEntry(EntityType.EARTH_CRYSTAL, frequency, World.Season.NONE);
                    }
                    else if (entity.Equals("fire_crystal"))
                    {
                        spawnZone.AddEntry(EntityType.FIRE_CRYSTAL, frequency, World.Season.NONE);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("NO ENTITY FOUND FOR: " + entity);
                    }
                }
                spawnZones.Add(spawnZone);
            }

            //create foreground/background
            foreground = new LayeredBackground(content, cameraBoundingBox, foregroundParameters);
            background = new LayeredBackground(content, cameraBoundingBox, backgroundParameters);


            //debug output of collision layout
            /*for (int y = 0; y < baseLayer.Height; y++)
            {
                for(int x = 0; x < baseLayer.Width; x++)
                {
                    if(collisionMap[x,y] == CollisionTypeEnum.BRIDGE)
                    {
                        Console.Write("B");
                    }
                    else if(collisionMap[x,y] == CollisionTypeEnum.SOLID)
                    {
                        Console.Write("X");
                    }
                    else if (collisionMap[x, y] == CollisionTypeEnum.WATER)
                    {
                        Console.Write("W");
                    }
                    else
                    {
                        Console.Write(" ");
                    } 
                }
                Console.Write("\n");
            }*/
            /*Console.WriteLine("\n\n\n" + GetAreaName());
            for(int y = 0; y < baseLayer.Height; y++)
            {
                for(int x = 0; x < baseLayer.Width; x++)
                {
                    if(wallMap[x, y] == true)
                    {
                        Console.Write("W");
                    } else
                    {
                        Console.Write(".");
                    }
                }
                Console.Write("\n");
            }*/
        }


        /**
         * 
         *         private string mapWaterPath, mapDecorationFGPath, mapBasePath, mapDecorationPath, mapWallsPath, mapWaterBGPath, mapFGCavePath;
        private Texture2D mapWater, mapDecorationFG, mapBase, mapDecoration, mapWalls, mapWaterBG, mapFGCave;
         */
        public void LoadLayers()
        {
            if (mapWaterPath != null) {
                mapWater = layerContentManager.Load<Texture2D>(mapWaterPath);
            }
            if (mapDecorationFGPath != null)
            {
                mapDecorationFG = layerContentManager.Load<Texture2D>(mapDecorationFGPath);
            }
            if (mapBasePath != null)
            {
                mapBase = layerContentManager.Load<Texture2D>(mapBasePath);
            }
            if (mapDecorationPath != null)
            {
                mapDecoration = layerContentManager.Load<Texture2D>(mapDecorationPath);
            }
            if (mapWallsPath != null)
            {
                mapWalls = layerContentManager.Load<Texture2D>(mapWallsPath);
            }
            if (mapWaterBGPath != null)
            {
                mapWaterBG = layerContentManager.Load<Texture2D>(mapWaterBGPath);
            }
            if (mapFGCavePath != null)
            {
                mapFGCave = layerContentManager.Load<Texture2D>(mapFGCavePath);
            }
        }

        public void UnloadLayers()
        {
            layerContentManager.Unload();
        }

        public string GetName()
        {
            return this.name;
        }

        public Subarea.NameEnum GetSubareaAt(RectangleF rect)
        {
            foreach (Subarea sa in subareas)
            {
                if (sa.rect.Intersects(rect))
                {
                    return sa.subareaName;
                }
            }
            throw new Exception("No subarea found at " + rect);
        }

        public string GetZoneName(Vector2 playerPosition)
        {
            foreach (NamedZone nz in nameZones)
            {
                if (nz.rectangle.Contains(playerPosition))
                {
                    return nz.name;
                }
            }
            return "";
        }

        public List<CutsceneManager.Cutscene> GetPossibleCutscenes(Vector2 playerPosition)
        {
            List<CutsceneManager.Cutscene> possible = new List<CutsceneManager.Cutscene>();
            foreach (CutsceneTriggerZone ctZone in cutsceneTriggerZones)
            {
                if (ctZone.rectangle.Contains(playerPosition)) {
                    possible.Add(CutsceneManager.GetCutsceneById(ctZone.cutsceneID));
                }
            }
            return possible;
        }

        public List<PathingHelper> GetPathingHelpers(RectangleF hitbox)
        {
            List<PathingHelper> collidingHelpers = new List<PathingHelper>();
            foreach (PathingHelper ph in pathingHelpers)
            {
                if (ph.rect.Intersects(hitbox))
                {
                    collidingHelpers.Add(ph);
                }
            }
            return collidingHelpers;
        }

        public List<SoundZone> GetSoundZonesAtPosAndTimeAndSeason(Vector2 pos, World.TimeOfDay time, World.Season season)
        {
            List<SoundZone> validSoundZones = new List<SoundZone>();
            foreach (SoundZone sz in soundZones)
            {
                if (sz.rect.Contains(pos) && (sz.time == World.TimeOfDay.ALL || sz.time == time) && (sz.season == World.Season.NONE || sz.season == season))
                {
                    validSoundZones.Add(sz);
                }
            }
            return validSoundZones;
        }

        public bool IsCollideWithPathingHelperType(RectangleF hitbox, PathingHelper.Type type)
        {
            foreach (PathingHelper ph in pathingHelpers)
            {
                if (ph.rect.Intersects(hitbox) && ph.type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public AreaEnum GetAreaEnum()
        {
            return this.areaEnum;
        }

        public bool DoesCameraMove()
        {
            return cameraMoves;
        }

        public World.Season GetWorldSeason()
        {
            return worldSeason;
        }

        public int GetDay()
        {
            if(timeData == null)
            {
                return 0;
            }
            return timeData.day;
        }

        public World.Season GetSeason()
        {
            if (areaSeason == World.Season.DEFER)
            {
                return worldSeason;
            }
            return areaSeason;
        }

        public CollisionTypeEnum GetCollisionTypeAt(int x, int y)
        {
            if (x < 0 || x >= collisionMap.GetLength(0) || y < 0 || y >= collisionMap.GetLength(1))
            {
                return CollisionTypeEnum.BOUNDARY;
            }

            if (collisionMap[x, y] == CollisionTypeEnum.AIR && buildingBlockGrid[x, y] != null)
            {
                if (buildingBlockGrid[x, y].GetBlockType() == BlockType.PLATFORM || buildingBlockGrid[x, y].GetBlockType() == BlockType.PLATFORM_FARM)
                {
                    return CollisionTypeEnum.SCAFFOLDING_BRIDGE;
                } else if (buildingBlockGrid[x, y].GetBlockType() == BlockType.BLOCK)
                {
                    return CollisionTypeEnum.SCAFFOLDING_BLOCK;
                } else
                {
                    return CollisionTypeEnum.SCAFFOLDING;
                }
            }

            return collisionMap[x, y];
        }

        public FishingZone GetFishingZoneAt(int x, int y)
        {
            foreach (FishingZone fz in fishingZones)
            {
                if (fz.rectangle.Contains(new Point2(x, y))) {
                    return fz;
                }
            }
            throw new Exception("Water not covered by fishingzone?");
        }

        public Vector2 GetPositionOfTile(int x, int y)
        {
            return new Vector2(x * tileWidth, y * tileHeight);
        }

        public int MapPixelWidth()
        {
            return tiledMap.WidthInPixels;
        }

        public int MapPixelHeight()
        {
            return tiledMap.HeightInPixels;
        }

        private void CheckItemCollision(EntityPlayer player)
        {
            for (int i = 0; i < itemEntities.Count; i++)
            {
                EntityItem ei = itemEntities[i];
                if (ei.CanBeCollected() && player.GetCollisionRectangle().Intersects(ei.GetCollisionRectangle()))
                {
                    if (player.AddItemToInventory(ei.GetItemForm()))
                    {
                        itemEntities.Remove(ei);

                    }
                }
            }
        }

        private void CheckTileCollision(EntityPlayer player)
        {
            RectangleF baseHitbox = player.GetCollisionRectangle();
            baseHitbox.X += 2;
            baseHitbox.Width -= 4;
            Rectangle tileHitbox = new Rectangle((int)(baseHitbox.Left / 8), (int)(baseHitbox.Top / 8), (int)(baseHitbox.Width / 8), (int)(baseHitbox.Height / 8));
            for (int x = tileHitbox.Left; x <= tileHitbox.Left + tileHitbox.Width; x++)
            {
                for (int y = tileHitbox.Top; y <= tileHitbox.Top + tileHitbox.Height; y++)
                {
                    TileEntity en = GetTileEntity(x, y);
                    if (en != null && en is IInteractContact)
                    {
                        ((IInteractContact)en).OnContact(player, this);
                    }
                }
            }
        }

        public void CheckEntityCollisions(EntityPlayer player)
        {
            RectangleF playerHitbox = player.GetCollisionRectangle();
            CheckItemCollision(player);
            CheckTileCollision(player);
            foreach (Entity en in entityListManager.GetContactInteractableEntityList())
            {
                if (en is IInteractContact && playerHitbox.Intersects(en.GetCollisionRectangle()))
                {
                    ((IInteractContact)en).OnContact(player, this);
                }
            }
        }

        public List<EntityCollidable> GetCollideableEntities()
        {
            return entityListManager.GetCollideableEntityList();
        }

        public void DrawBackground(SpriteBatch sb, RectangleF cameraBoundingBox, float layerDepth)
        {
            background.Draw(sb, cameraBoundingBox, layerDepth, 1.0f);
        }

        public World.Weather GetWeather()
        {
            return areaWeather;
        }

        public List<EntitySolid> GetSolidEntities()
        {
            return entityListManager.GetSolidEntityList();
        }

        public bool IsTilePositionInsideCave(Vector2 tilePosition)
        {
            return caveMap[(int)tilePosition.X, (int)tilePosition.Y];
        }

        public bool IsPositionInsideCave(Vector2 position)
        {
            return caveMap[(int)position.X/8, (int)position.Y/8];
        }

        public void Update(float deltaTime, GameTime gameTime, World.TimeData timeData, World.Weather worldWeather, RectangleF cameraBoundingBox, EntityPlayer player)
        {
            this.areaWeather = worldWeather; //update this eventually?
            this.worldSeason = timeData.season;
            this.timeData = timeData;
            if (areaWeather == World.Weather.SNOWY)
            {
                if (areaSeason != World.Season.DEFER && areaSeason != World.Season.WINTER)
                {
                    areaWeather = World.Weather.RAINY;
                }
            }

            background.Update(deltaTime, cameraBoundingBox, timeData, areaWeather, (areaSeason == World.Season.DEFER ? worldSeason : areaSeason));
            foreground.Update(deltaTime, cameraBoundingBox, timeData, areaWeather, (areaSeason == World.Season.DEFER ? worldSeason : areaSeason));

            for (int i = 0; i < entityListManager.GetEntityList().Count; i++)
            {
                entityListManager.GetEntityList()[i].Update(deltaTime, this);
            }

            foreach (EntityItem ei in itemEntities)
            {
                ei.Update(deltaTime, this);
            }

            List<Particle> removeList = new List<Particle>();
            foreach (Particle pa in particleList)
            {
                pa.Update(deltaTime, this);

                if (pa.IsDisposable())
                {
                    removeList.Add(pa);
                }
            }
            foreach (Particle paR in removeList)
            {
                particleList.Remove(paR);
            }

            //cave transparency
            if (IsPlayerInCave(player))
            {
                foregroundCaveTransparency = Util.AdjustTowards(foregroundCaveTransparency, 0, FOREGROUND_CAVE_TRANSPARENCY_DELTA * deltaTime);
                //foregroundCaveTransparency = 0;
            }
            else
            {
                foregroundCaveTransparency = Util.AdjustTowards(foregroundCaveTransparency, 1, FOREGROUND_CAVE_TRANSPARENCY_DELTA * deltaTime);
                //foregroundCaveTransparency = 1;
            }
            

            positionalDarkLevel = Util.AdjustTowards(positionalDarkLevel, GetDarkLevelForPosition(player.GetAdjustedPosition()), LIGHTING_CHANGE_SPEED * deltaTime);
        }

        public bool IsPlayerInCave(EntityPlayer player)
        {
            Vector2 playerTile = player.GetAdjustedPosition();
            playerTile.X /= 8;
            playerTile.Y /= 8;
            playerTile.Y += 2;
            if(playerTile.X > caveMap.GetLongLength(0) || playerTile.Y > caveMap.GetLongLength(1) || playerTile.X < 0 || playerTile.Y < 0)
            {
                return false;
            }

            return caveMap[(int)playerTile.X, (int)playerTile.Y];
        }

        public Entity GetInteractableEntityAt(Vector2 atPoint)
        {
            foreach (Entity en in entityListManager.GetInteractableEntityList())
            {
                if (!(en is TileEntity) && en.GetCollisionRectangle().Contains(atPoint)) {
                    return en;
                }
            }
            foreach (Entity en in entityListManager.GetToolInteractableEntityList())
            {
                if (!(en is TileEntity) && en.GetCollisionRectangle().Contains(atPoint))
                {
                    return en;
                }
            }
            return null;
        }

        public Entity GetInteractableEntityAt(Rectangle inRect)
        {
            foreach (Entity en in entityListManager.GetInteractableEntityList())
            {
                if (!(en is TileEntity) && en.GetCollisionRectangle().Intersects(inRect))
                {
                    return en;
                }
            }
            foreach (Entity en in entityListManager.GetToolInteractableEntityList())
            {
                if (!(en is TileEntity) && en.GetCollisionRectangle().Intersects(inRect))
                {
                    return en;
                }
            }
            return null;
        }

        public void DrawBaseTerrain(SpriteBatch sb, float layerDepth)
        {
            if (mapBase != null) {
                sb.Draw(mapBase, Vector2.Zero, Color.White);
            }
            if(mapDecorationFG != null)
            {
                sb.Draw(mapDecorationFG, Vector2.Zero, Color.White);
            }
        }
        public void DrawParticles(SpriteBatch sb, float layerDepth)
        {
            foreach (Particle pa in particleList)
            {
                pa.Draw(sb, layerDepth);
            }
        }

        public void DrawWater(SpriteBatch sb, float layerDepth)
        {
            if (mapWater != null)
            {
                sb.Draw(mapWater, Vector2.Zero, Color.White);
            }
        }

        public void DrawWaterBackground(SpriteBatch sb, float layerDepth)
        {
            if (mapWaterBG != null)
            {
                sb.Draw(mapWaterBG, Vector2.Zero, Color.White);
            }
        }

        public List<LightSource> GetAreaLights()
        {
            return lights;
        }

        public void DrawBuildingBlocks(SpriteBatch sb, float layerDepth)
        {
            foreach (BuildingBlock block in buildingBlockList)
            {
                block.Draw(sb, layerDepth);
            }
        }

        public bool IsWallEntityPlacementValid(int tileX, int tileY, int tileWidth, int tileHeight)
        {
            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    if (wallEntityGrid[tileX + x, tileY + y] != null)
                    {
                        return false;
                    }
                    if (wallMap[tileX + x, tileY + y] == false)
                    {
                        if (buildingBlockGrid[tileX + x, tileY + y] == null ||
                            (buildingBlockGrid[tileX + x, tileY + y].GetBlockType() != BlockType.SCAFFOLDING &&
                            buildingBlockGrid[tileX + x, tileY + y].GetBlockType() != BlockType.PLATFORM &&
                            buildingBlockGrid[tileX + x, tileY + y].GetBlockType() != BlockType.PLATFORM_FARM))
                            return false;
                    }
                    if (GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.AIR &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.SCAFFOLDING &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.SCAFFOLDING_BRIDGE)
                    {
                        return false;
                    }

                    if(CheckBlockerMap(tileX + x, tileY + y) == true)
                    {
                        return false;
                    }

                }
            }

            return true;
        }

        public bool IsWallpaperPlacementValid(int tileX, int tileY, int tileWidth, int tileHeight)
        {
            if (tileX > MapPixelWidth() / 8 || tileX < 0)
            {
                return false;
            }
            if (tileY > MapPixelHeight() / 8 || tileY < 0)
            {
                return false;
            }

            //can only place in house
            Area.Subarea.NameEnum subarea = this.GetSubareaAt(new RectangleF(tileX * 8, tileY * 8, tileWidth * 8, tileHeight * 8));
            if (subarea != Area.Subarea.NameEnum.FARMHOUSECABIN && subarea != Area.Subarea.NameEnum.FARMHOUSEHOUSE &&
                subarea != Area.Subarea.NameEnum.FARMHOUSEMANSIONLOWER && subarea != Area.Subarea.NameEnum.FARMHOUSEMANSIONUPPER)
                return false;

            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    if (areaEnum != AreaEnum.INTERIOR)
                    {
                        return false;
                    }
                    if (wallpaperEntityGrid[tileX + x, tileY + y] != null)
                    {
                        return false;
                    }
                    if (wallMap[tileX + x, tileY + y] == false || buildingBlockGrid[tileX + x, tileY + y] != null)
                    {
                        return false;
                    }
                    if (GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.AIR &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.SCAFFOLDING &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.SCAFFOLDING_BRIDGE)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public MovingPlatformDirectorZone GetDirectorZoneAt(RectangleF collisionRect)
        {
            foreach (MovingPlatformDirectorZone director in directorZones)
            {
                if (director.rectangle.Intersects(collisionRect))
                {
                    return director;
                }
            }
            return null;
        }

        public bool IsTileEntityPlacementValid(int tileX, int tileY, int tileWidth, int tileHeight, bool bridgesAllowed = false)
        {
            //check if the object itself intersects anything solid
            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    if ((GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.AIR &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.WATER &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.TOP_WATER &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.DEEP_WATER &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.SCAFFOLDING &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.BRIDGE) 
                        || tileEntityGrid[tileX + x, tileY + y] != null
                        || CheckBlockerMap(tileX + x, tileY + y) == true)
                    {
                        return false;
                    }
                }
            }

            //check the tile directly BELOW the object
            for (int x = 0; x < tileWidth; x++)
            {
                if (GetCollisionTypeAt(tileX + x, tileY + tileHeight) == CollisionTypeEnum.AIR || 
                    GetCollisionTypeAt(tileX + x, tileY + tileHeight) == CollisionTypeEnum.SCAFFOLDING || 
                    GetCollisionTypeAt(tileX + x, tileY + tileHeight) == CollisionTypeEnum.WATER ||
                    GetCollisionTypeAt(tileX + x, tileY + tileHeight) == CollisionTypeEnum.TOP_WATER ||
                    GetCollisionTypeAt(tileX + x, tileY + tileHeight) == CollisionTypeEnum.DEEP_WATER ||
                    (!bridgesAllowed && GetCollisionTypeAt(tileX + x, tileY + tileHeight) == CollisionTypeEnum.BRIDGE))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsCeilingTileEntityPlacementValid(int tileX, int tileY, int tileWidth, int tileHeight)
        {
            //check if the object itself intersects anything solid
            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    if ((GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.AIR &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.WATER &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.TOP_WATER &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.DEEP_WATER &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.SCAFFOLDING &&
                        GetCollisionTypeAt(tileX + x, tileY + y) != CollisionTypeEnum.BRIDGE)
                        || tileEntityGrid[tileX + x, tileY + y] != null
                        || CheckBlockerMap(tileX + x, tileY + y) == true)
                    {
                        return false;
                    }
                }
            }

            //check the tile directly ABOVE the object
            for (int x = 0; x < tileWidth; x++)
            {
                if (GetCollisionTypeAt(tileX + x, tileY - 1) == CollisionTypeEnum.AIR ||
                    GetCollisionTypeAt(tileX + x, tileY - 1) == CollisionTypeEnum.SCAFFOLDING ||
                    GetCollisionTypeAt(tileX + x, tileY - 1) == CollisionTypeEnum.WATER ||
                    GetCollisionTypeAt(tileX + x, tileY - 1) == CollisionTypeEnum.TOP_WATER ||
                    GetCollisionTypeAt(tileX + x, tileY - 1) == CollisionTypeEnum.DEEP_WATER ||
                    GetCollisionTypeAt(tileX + x, tileY - 1) == CollisionTypeEnum.BRIDGE)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsFloorEntityPlacementValid(int tileX, int tileY, int tileWidth)
        {
            if (tileX < 0 || tileX >= tileEntityGrid.GetLength(0))
            {
                return false;
            }
            if (tileY < 0 || tileY >= tileEntityGrid.GetLength(1))
            {
                return false;
            }

            for (int x = 0; x < tileWidth; x++)
            {
                if (tileEntityGrid[tileX + x, tileY] != null)
                {
                    return false;
                }
                if (GetCollisionTypeAt(tileX + x, tileY) != CollisionTypeEnum.SOLID 
                    || CheckBlockerMap(tileX + x, tileY) == true)
                {
                    return false;
                }
                if (GetCollisionTypeAt(tileX + x, tileY - 1) != CollisionTypeEnum.AIR &&
                    GetCollisionTypeAt(tileX + x, tileY - 1) != CollisionTypeEnum.SCAFFOLDING &&
                    GetCollisionTypeAt(tileX + x, tileY - 1) != CollisionTypeEnum.SCAFFOLDING_BRIDGE &&
                    GetCollisionTypeAt(tileX + x, tileY - 1) != CollisionTypeEnum.BRIDGE)
                {
                    return false;
                }
            }

            return true;
        }

        public Entity GetPlacedFromMapEntityById(string id)
        {
            foreach (Entity en in entityListManager.GetEntityList())
            {
                if (en is IHaveID && ((IHaveID)en).GetID() == id)
                {
                    return en;
                }
            }
            return null;
        }

        public bool IsBuildingBlockPlacementValid(int tileX, int tileY, bool isBlock, bool ignoreScaffolding = false, bool checkingExistingBlock = false)
        {
            if (!checkingExistingBlock && GetCollisionTypeAt(tileX, tileY) != CollisionTypeEnum.AIR)
            {
                return false;
            }

            bool validAdjacentTile = false;
            if (GetCollisionTypeAt(tileX, tileY + 1) != CollisionTypeEnum.AIR && GetCollisionTypeAt(tileX, tileY + 1) != CollisionTypeEnum.WATER)
            {
                validAdjacentTile = true;
                if (ignoreScaffolding &&
                    (GetCollisionTypeAt(tileX, tileY + 1) == CollisionTypeEnum.SCAFFOLDING ||
                    GetCollisionTypeAt(tileX, tileY + 1) == CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                    GetCollisionTypeAt(tileX, tileY + 1) == CollisionTypeEnum.SCAFFOLDING_BRIDGE))
                {
                    validAdjacentTile = false;
                }
            }
            if (!validAdjacentTile && GetCollisionTypeAt(tileX, tileY - 1) != CollisionTypeEnum.AIR && GetCollisionTypeAt(tileX, tileY - 1) != CollisionTypeEnum.WATER && GetCollisionTypeAt(tileX, tileY - 1) != CollisionTypeEnum.BRIDGE)
            {
                validAdjacentTile = true;
                if (ignoreScaffolding &&
                    (GetCollisionTypeAt(tileX, tileY - 1) == CollisionTypeEnum.SCAFFOLDING ||
                    GetCollisionTypeAt(tileX, tileY - 1) == CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                    GetCollisionTypeAt(tileX, tileY - 1) == CollisionTypeEnum.SCAFFOLDING_BRIDGE))
                {
                    validAdjacentTile = false;
                }
            }
            if (!validAdjacentTile && GetCollisionTypeAt(tileX + 1, tileY) != CollisionTypeEnum.AIR && GetCollisionTypeAt(tileX + 1, tileY) != CollisionTypeEnum.WATER)
            {
                validAdjacentTile = true;
                if (ignoreScaffolding &&
                    (GetCollisionTypeAt(tileX + 1, tileY) == CollisionTypeEnum.SCAFFOLDING ||
                    GetCollisionTypeAt(tileX + 1, tileY) == CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                    GetCollisionTypeAt(tileX + 1, tileY) == CollisionTypeEnum.SCAFFOLDING_BRIDGE))
                {
                    validAdjacentTile = false;
                }
            }
            if (!validAdjacentTile && GetCollisionTypeAt(tileX - 1, tileY) != CollisionTypeEnum.AIR && GetCollisionTypeAt(tileX - 1, tileY) != CollisionTypeEnum.WATER)
            {
                validAdjacentTile = true;
                if (ignoreScaffolding &&
                    (GetCollisionTypeAt(tileX - 1, tileY) == CollisionTypeEnum.SCAFFOLDING ||
                    GetCollisionTypeAt(tileX - 1, tileY) == CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                    GetCollisionTypeAt(tileX - 1, tileY) == CollisionTypeEnum.SCAFFOLDING_BRIDGE))
                {
                    validAdjacentTile = false;
                }
            }
            if (!validAdjacentTile)
            {
                return false;
            }

            if (isBlock && GetTileEntity(tileX, tileY) != null)
            {
                return false;
            }

            if(CheckBlockerMap(tileX, tileY) == true)
            {
                return false;
            }

            return true;
        }

        public bool CheckBlockerMap(int tileX, int tileY)
        {
            if (tileX > blockerMap.GetLength(0) - 1 || tileX < 0 || tileY > blockerMap.GetLength(1) - 1 || tileY < 0)
            {
                return true;
            }
            else
            {
                return blockerMap[tileX, tileY];
            }
        }

        public bool IsFarmablePlacementValid(int tileX, int tileY)
        {
            if(CheckBlockerMap(tileX, tileY+2) == true)
            {
                return false; //enables blocking specific tiles - ie for spots where flooring is placed
            }
            if (GetGroundTileType(tileX, tileY + 2) != GroundTileType.EARTH && GetGroundTileType(tileX, tileY + 2) != GroundTileType.SAND)
            {
                return false;
            }
            tileY++;
            if ((GetCollisionTypeAt(tileX, tileY) != CollisionTypeEnum.AIR && GetCollisionTypeAt(tileX, tileY) != CollisionTypeEnum.BRIDGE &&
                GetCollisionTypeAt(tileX, tileY) != CollisionTypeEnum.SCAFFOLDING) && GetCollisionTypeAt(tileX, tileY) != CollisionTypeEnum.SCAFFOLDING_BRIDGE)
            {
                return false;
            }

            //check current tile
            if(GetTileEntity(tileX, tileY) != null)
            {
                return false;
            }

            //check tile directly below (grass, flooring...)
            TileEntity below = GetTileEntity(tileX, tileY + 1);
            if(below != null) //&& !(below is TEntityGrass) 
            {
                //also comment out the interact with grass lines in TEntityPlayer to disable grass destruction
                return false;
            }

            if (GetCollisionTypeAt(tileX, tileY + 1) != CollisionTypeEnum.SOLID)
            {
                if (GetBuildingBlockAt(tileX, tileY + 1) != null &&
                    GetBuildingBlockAt(tileX, tileY + 1).GetBlockType() == BlockType.PLATFORM_FARM)
                {
                    return true;
                }
                return false;
            }
            if(CheckBlockerMap(tileX, tileY) == true)
            {
                return false;
            }

            return true;
        }

        public void AddWallEntity(TileEntity entity)
        {
            Vector2 tilePosition = entity.GetTilePosition();
            entityListManager.Add(entity);
            for (int x = 0; x < entity.GetTileWidth(); x++)
            {
                for (int y = 0; y < entity.GetTileHeight(); y++)
                {
                    wallEntityGrid[(int)tilePosition.X + x, (int)tilePosition.Y + y] = entity;
                }
            }
        }


        public void AddWallpaperEntity(PEntityWallpaper entity)
        {
            Vector2 tilePosition = entity.GetTilePosition();
            entityListManager.Add(entity);
            for (int x = 0; x < entity.GetTileWidth(); x++)
            {
                for (int y = 0; y < entity.GetTileHeight(); y++)
                {
                    wallpaperEntityGrid[(int)tilePosition.X + x, (int)tilePosition.Y + y] = entity;
                }
            }
        }

        public void RandomizeBackground(RectangleF cameraBoundingBox)
        {
            background.Randomize(cameraBoundingBox);
            foreground.Randomize(cameraBoundingBox);
        }

        public void AddTileEntity(TileEntity entity)
        {
            Vector2 tilePosition = entity.GetTilePosition();
            entityListManager.Add(entity);
            for (int x = 0; x < entity.GetTileWidth(); x++)
            {
                for (int y = 0; y < entity.GetTileHeight(); y++)
                {
                    tileEntityGrid[(int)tilePosition.X + x, (int)tilePosition.Y + y] = entity;
                }
            }

            if (entity is ILightSource)
            {
                ILightSource els = (ILightSource)entity;
                Vector2 lightPosition = els.GetLightPosition();
                lights.Add(new LightSource(els.GetLightStrength(), lightPosition, els.GetLightColor(), entity));
            }
        }

        public TileEntity GetTileEntity(int tileX, int tileY)
        {
            if (tileX > tileEntityGrid.GetLength(0) - 1 || tileX < 0 || tileY > tileEntityGrid.GetLength(1) - 1 || tileY < 0)
            {
                return null;
            } else {
                return tileEntityGrid[tileX, tileY];
            }
        }

        public Item GetTileEntityItemForm(int tileX, int tileY)
        {
            TileEntity en = GetTileEntity(tileX, tileY);

            if (en == null)
            {
                return ItemDict.NONE;
            }

            if (en is PlacedEntity)
            {
                return ((PlacedEntity)en).GetItemForm();
            }

            return ItemDict.NONE;
        }

        public Item GetWallEntityItemForm(int tileX, int tileY)
        {
            TileEntity en = wallEntityGrid[tileX, tileY];
            if (en == null)
            {
                return ItemDict.NONE;
            }

            if (en is PlacedEntity)
            {
                return ((PlacedEntity)en).GetItemForm();
            }
            return ItemDict.NONE;
        }

        public Item GetWallpaperItemForm(int tileX, int tileY)
        {
            PEntityWallpaper en = wallpaperEntityGrid[tileX, tileY];
            if (en == null)
            {
                return ItemDict.NONE;
            }
            return en.GetItemForm();
        }

        public TileEntity RemoveWallEntity(EntityPlayer player, int tileX, int tileY, World world)
        {
            TileEntity toRemove = wallEntityGrid[tileX, tileY];

            if (toRemove != null)
            {
                tileX = (int)toRemove.GetTilePosition().X;
                tileY = (int)toRemove.GetTilePosition().Y;
                for (int x = 0; x < toRemove.GetTileWidth(); x++)
                {
                    for (int y = 0; y < toRemove.GetTileHeight(); y++)
                    {
                        wallEntityGrid[tileX + x, tileY + y] = null;
                    }
                }
                entityListManager.Remove(toRemove);
            }

            if (toRemove is PlacedEntity)
            {
                ((PlacedEntity)toRemove).OnRemove(player, this, world);
            }

            return toRemove;
        }

        public PEntityWallpaper RemoveWallpaperEntity(EntityPlayer player, int tileX, int tileY, World world)
        {
            PEntityWallpaper toRemove = wallpaperEntityGrid[tileX, tileY];

            if (toRemove != null)
            {
                tileX = (int)toRemove.GetTilePosition().X;
                tileY = (int)toRemove.GetTilePosition().Y;
                for (int x = 0; x < toRemove.GetTileWidth(); x++)
                {
                    for (int y = 0; y < toRemove.GetTileHeight(); y++)
                    {
                        wallpaperEntityGrid[tileX + x, tileY + y] = null;
                    }
                }
                entityListManager.Remove(toRemove);
            }

            if (toRemove is PlacedEntity)
            {
                ((PlacedEntity)toRemove).OnRemove(player, this, world);
            }

            return toRemove;
        }

        public void RemoveTileEntity(EntityPlayer player, int tileX, int tileY, World world)
        {
            TileEntity toRemove = (TileEntity)tileEntityGrid[tileX, tileY];

            if (toRemove is PlacedEntity && !((PlacedEntity)toRemove).IsRemovable()) {
                player.AddNotification(new EntityPlayer.Notification("This isn't mine, I shouldn't remove this.", Color.Red));
            } else {
                if (toRemove != null)
                {
                    tileX = (int)toRemove.GetTilePosition().X;
                    tileY = (int)toRemove.GetTilePosition().Y;
                    for (int x = 0; x < toRemove.GetTileWidth(); x++)
                    {
                        for (int y = 0; y < toRemove.GetTileHeight(); y++)
                        {
                            tileEntityGrid[tileX + x, tileY + y] = null;
                        }
                    }
                    entityListManager.Remove(toRemove);
                    if (toRemove is ILightSource)
                    {
                        foreach (LightSource ls in lights)
                        {
                            if (toRemove == ls.source)
                            {
                                lights.Remove(ls);
                                break; //PREVENTS SINGLE ENTITY FROM HAVING MULTIPLE LS>>>>
                            }
                        }
                    }
                }

                if (toRemove is PlacedEntity)
                {
                    ((PlacedEntity)toRemove).OnRemove(player, this, world);
                }
            }
        }

        public void AddBuildingBlock(BuildingBlock block)
        {
            buildingBlockGrid[(int)block.GetTilePosition().X, (int)block.GetTilePosition().Y] = block;
            buildingBlockList.Add(block);
        }

        public Item GetBuildingBlockItemForm(int tileX, int tileY)
        {
            BuildingBlock bb = buildingBlockGrid[tileX, tileY];

            if (bb == null)
            {
                return ItemDict.NONE;
            }

            return bb.GetItemForm();
        }

        public void AddEntity(Entity en)
        {
            if (en == null)
            {
                throw new Exception("Added a null entity");
            }

            if (en is EntityItem)
            {
                itemEntities.Add((EntityItem)en);
            }
            else
            {
                entityListManager.Add(en);
                if (en is ILightSource)
                {
                    ILightSource els = (ILightSource)en;
                    Vector2 lightPosition = els.GetLightPosition();
                    lights.Add(new LightSource(els.GetLightStrength(), lightPosition, els.GetLightColor(), en));
                }
            }
        }

        public GroundTileType GetGroundTileType(int tileX, int tileY)
        {
            int baseTileId = GetTileIdFor(tileX, tileY);
            if (BEACH_SAND_TILE_IDS.Contains(baseTileId) || 
                RED_SAND_TILE_IDS.Contains(baseTileId))
            {
                return GroundTileType.SAND;
            } else if (WOODEN_BRIDGE_IDS.Contains(baseTileId) || 
                METAL_BRIDGE_IDS.Contains(baseTileId))
            {
                return GroundTileType.BRIDGE;
            } else if (WOOD_INTERIOR_TILE_IDS.Contains(baseTileId) || 
                METAL_TILE_IDS.Contains(baseTileId) || 
                BROWN_RUINS_TILE_IDS.Contains(baseTileId) || 
                LAB_METAL_TILE_IDS.Contains(baseTileId) || 
                WHITE_EARTH_TILE_IDS.Contains(baseTileId) || 
                BROWN_BARK_TILE_IDS.Contains(baseTileId))
            {
                return GroundTileType.SOLID;
            } else if (ORANGE_VOLCANO_TILE_IDS.Contains(baseTileId) ||
                CAVE_STONE_TILE_IDS.Contains(baseTileId))
            {
                return GroundTileType.EXTRACTABLE;
            }

            //Orange Earth
            //White Earth
            return GroundTileType.EARTH;
        }

        private bool CheckRemoveBuildingBlock(int tileX, int tileY, EntityPlayer player, List<XYTile> alreadyCovered)
        {
            foreach(XYTile pair in alreadyCovered)
            {
                if(tileX == pair.tileX && tileY == pair.tileY)
                {
                    return false;
                }
            }
            alreadyCovered.Add(new XYTile(tileX, tileY));

            if (IsBuildingBlockPlacementValid(tileX, tileY, false, true, true))
            {
                return true;
            }

            bool legal = false;
            if(GetBuildingBlockAt(tileX + 1, tileY) != null)
            {
                legal = CheckRemoveBuildingBlock(tileX + 1, tileY, player, alreadyCovered);
            }
            if(!legal && GetBuildingBlockAt(tileX - 1, tileY) != null)
            {
                legal = CheckRemoveBuildingBlock(tileX - 1, tileY, player, alreadyCovered);
            }
            if(!legal && GetBuildingBlockAt(tileX, tileY + 1) != null)
            {
                legal = CheckRemoveBuildingBlock(tileX, tileY + 1, player, alreadyCovered);
            }
            if (!legal && GetBuildingBlockAt(tileX, tileY - 1) != null)
            {
                legal = CheckRemoveBuildingBlock(tileX, tileY - 1, player, alreadyCovered);
            }

            return legal;
        }

        public void RemoveBuildingBlock(int tileX, int tileY, EntityPlayer player, World world)
        {
            BuildingBlock toRemove = buildingBlockGrid[tileX, tileY];
            buildingBlockList.Remove(toRemove);
            buildingBlockGrid[tileX, tileY] = null;

            toRemove.OnRemove(player, this, world);

            List<XYTile> alreadyCovered = new List<XYTile>();
            alreadyCovered.Add(new XYTile(tileX, tileY));

            //check all 4 adjacent..
            if(GetBuildingBlockAt(tileX + 1, tileY) != null)
            {
                if(!CheckRemoveBuildingBlock(tileX + 1, tileY, player, alreadyCovered))
                {
                    foreach(XYTile pair in alreadyCovered)
                    {
                        toRemove = buildingBlockGrid[pair.tileX, pair.tileY];
                        buildingBlockList.Remove(toRemove);
                        buildingBlockGrid[pair.tileX, pair.tileY] = null;
                        if (toRemove != null)
                        {
                            toRemove.OnRemove(player, this, world);
                        }
                        if (wallEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallEntity(player, pair.tileX, pair.tileY, world);
                        }
                        if (wallpaperEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallpaperEntity(player, pair.tileX, pair.tileY, world);
                        }
                    }
                }
            }
            alreadyCovered.Clear();
            if(GetBuildingBlockAt(tileX - 1, tileY) != null)
            {
                if(!CheckRemoveBuildingBlock(tileX - 1, tileY, player, alreadyCovered))
                {
                    foreach (XYTile pair in alreadyCovered)
                    {
                        toRemove = buildingBlockGrid[pair.tileX, pair.tileY];
                        buildingBlockList.Remove(toRemove);
                        buildingBlockGrid[pair.tileX, pair.tileY] = null;
                        if (toRemove != null)
                        {
                            toRemove.OnRemove(player, this, world);
                        }
                        if (wallEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallEntity(player, pair.tileX, pair.tileY, world);
                        }
                        if (wallpaperEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallpaperEntity(player, pair.tileX, pair.tileY, world);
                        }
                    }
                }
            }
            alreadyCovered.Clear();
            if(GetBuildingBlockAt(tileX, tileY + 1) != null)
            {
                if(!CheckRemoveBuildingBlock(tileX, tileY + 1, player, alreadyCovered))
                {
                    foreach (XYTile pair in alreadyCovered)
                    {
                        toRemove = buildingBlockGrid[pair.tileX, pair.tileY];
                        buildingBlockList.Remove(toRemove);
                        buildingBlockGrid[pair.tileX, pair.tileY] = null;
                        if (toRemove != null)
                        {
                            toRemove.OnRemove(player, this, world);
                        }
                        if (wallEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallEntity(player, pair.tileX, pair.tileY, world);
                        }
                        if (wallpaperEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallpaperEntity(player, pair.tileX, pair.tileY, world);
                        }
                    }
                }
            }
            alreadyCovered.Clear();
            if(GetBuildingBlockAt(tileX, tileY - 1) != null)
            {
                if(!CheckRemoveBuildingBlock(tileX, tileY - 1, player, alreadyCovered))
                {
                    foreach (XYTile pair in alreadyCovered)
                    {
                        toRemove = buildingBlockGrid[pair.tileX, pair.tileY];
                        buildingBlockList.Remove(toRemove);
                        buildingBlockGrid[pair.tileX, pair.tileY] = null;
                        if (toRemove != null)
                        {
                            toRemove.OnRemove(player, this, world);
                        }
                        if (wallEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallEntity(player, pair.tileX, pair.tileY, world);
                        }
                        if (wallpaperEntityGrid[pair.tileX, pair.tileY] != null)
                        {
                            RemoveWallpaperEntity(player, pair.tileX, pair.tileY, world);
                        }
                    }
                }
            }
        }

        public BuildingBlock GetBuildingBlockAt(int tileX, int tileY)
        {
            if(tileX < 0 || tileX > widthInTiles)
            {
                return null;
            }
            if(tileY < 0 || tileY > heightInTiles)
            {
                return null;
            }
            return buildingBlockGrid[tileX, tileY];
        }

        public void RemoveEntity(Entity en)
        {
            if (en is EntityItem)
            {
                itemEntities.Remove((EntityItem)en);
            }
            else
            {
                entityListManager.Remove(en);
            }
        }

        public bool IsEntityInEntityList(Entity en)
        {
            if(entityListManager.GetEntityList().Contains(en))
            {
                return true;
            }
            return false;
        }

        public void AddEntitySaveStates(List<SaveState> saveStates)
        {
            foreach(Entity entity in entityListManager.GetEntityList())
            {
                if (entity is IPersist && ((IPersist)entity).ShouldBeSaved())
                {
                    SaveState entitySaveState = ((IPersist)entity).GenerateSave();
                    entitySaveState.AddData("area", this.areaEnum.ToString());
                    saveStates.Add(entitySaveState);
                }
            }
        }

        public void AddBuildingBlockSaveStates(List<SaveState> saveStates)
        {
            foreach(BuildingBlock block in buildingBlockList)
            {
                SaveState bbSaveState = block.GenerateSave();
                bbSaveState.AddData("area", this.areaEnum.ToString());
                saveStates.Add(bbSaveState);
            }
        }

        public void MoveToWaypoint(EntityPlayer player, string waypointName)
        {
            foreach(Waypoint waypoint in waypoints)
            {
                if(waypoint.name.Equals(waypointName))
                {
                    player.SetPosition(new Vector2(waypoint.position.X, waypoint.position.Y - 32.1f));
                    return;
                }
            }
        }

        public void TickSpawnZones()
        {
            foreach (SpawnZone sz in spawnZones)
            {
                sz.TickDaily(this);
            }
        }

        public void TickSpawnZones(Rectangle legalTiles)
        {
            foreach(SpawnZone sz in spawnZones)
            {
                sz.TickDaily(this, legalTiles);
            }
        }

        public void TickDay(World world, EntityPlayer player)
        {
            this.worldSeason = world.GetSeason();
            this.timeData = world.GetTimeData();
            List<Entity> removedEn = new List<Entity>();
            
            //try spawn new grass
            for (int x = 0; x < grassMap.GetLength(0); x++)
            {
                for (int y = 0; y < grassMap.GetLength(1); y++)
                {
                    if (grassMap[x, y] && GetTileEntity(x, y) == null && Util.RandInt(0, 3) == 0)
                    {
                        bool validPlacement = IsFloorEntityPlacementValid(x, y, 1);
                        bool farmableAbove = GetTileEntity(x, y - 1) is TEntityFarmable;
                        if (validPlacement && !farmableAbove)
                        {
                            TileEntity newGrass = (TileEntity)EntityFactory.GetEntity(EntityType.GRASS, ItemDict.NONE, new Vector2(x, y), this);
                            AddTileEntity(newGrass);
                        }
                    }
                }
            }

            //TODO DESPAWN LOGIC
            foreach (Entity en in entityListManager.GetEntityList())
            {
                if (en is TEntityFarmable)
                {
                    if (world.GetDay() == 0 && ((TEntityFarmable)en).RemovedAtSeasonShift(GetSeason()))
                    {
                        removedEn.Add(en);
                    }
                }
                else if (en is TEntityForage)
                {
                    if (en is TEntitySeasonalForage && ((TEntitySeasonalForage)en).GetSeason() != this.areaSeason)
                    {
                        removedEn.Add(en);
                    }
                    if (Util.RandInt(1, 15) == 1)
                    {
                        removedEn.Add(en);
                    }
                } else if (en is TEntityToolable)
                {
                    if(Util.RandInt(1, 10) == 1)
                    {
                        removedEn.Add(en);
                    }
                } else if (en is TEntityBush)
                {
                    if(((TEntityBush)en).IsWild() && Util.RandInt(1, 20) == 1)
                    {
                        removedEn.Add(en);
                    }
                }
                else if (en is TEntityTree)
                {
                    if (((TEntityTree)en).IsWild() && ((TEntityTree)en).isFullyGrown() && Util.RandInt(1, 30) == 1)
                    {
                        removedEn.Add(en);
                    }
                }
            }
            foreach (Entity en in removedEn)
            {
                if (en is TileEntity)
                {
                    RemoveTileEntity(player, (int)((TileEntity)en).GetTilePosition().X, (int)((TileEntity)en).GetTilePosition().Y, world);
                } else
                {
                    RemoveEntity(en);
                }
            }

            TickSpawnZones();

            //POTENTAILLY BAD! MAKE SURE ENTITYLIST SIZE DOESN"T DECREASE...
            int initialSize = entityListManager.GetEntityList().Count;
            Queue<ITickDaily> toTick = new Queue<ITickDaily>();

            for(int i = 0; i < entityListManager.GetEntityList().Count; i++)
            {
                Entity en = entityListManager.GetEntityList()[i];
                if (en is ITickDaily)
                {
                    toTick.Enqueue((ITickDaily)en);
                    //((ITickDaily)en).TickDaily(world, this, player);
                }
            }

            for(int i = 0; i < itemEntities.Count; i++)
            {
                if(itemEntities[i] is ITickDaily)
                {
                    toTick.Enqueue((ITickDaily)itemEntities[i]);
                }
            }

            foreach(ITickDaily tickable in toTick) {
                tickable.TickDaily(world, this, player);
            }
        }

        public void DrawEntities(SpriteBatch sb, DrawLayer layer, RectangleF cameraBoundingBox, float layerDepth)
        {
            foreach (Entity en in entityListManager.GetEntitiesByDrawLayer(layer))
            {
                RectangleF colRect = en.GetCollisionRectangle();
                colRect.Inflate(48, 48);
                if (colRect.Intersects(cameraBoundingBox))
                {
                    en.Draw(sb, layerDepth);
                }
            }
        }

        public void DrawItemEntities(SpriteBatch sb, float layerDepth)
        {
            foreach(EntityItem ien in itemEntities)
            {
                ien.Draw(sb, layerDepth);
            }
        }

        public void DrawWalls(SpriteBatch sb, float layerDepth)
        {
            if (mapWalls != null)
            {
                sb.Draw(mapWalls, Vector2.Zero, Color.White);
            }
        }

        public void DrawDecorations(SpriteBatch sb, float layerDepth)
        {
            if (mapDecoration != null)
            {
                sb.Draw(mapDecoration, Vector2.Zero, Color.White);
            }
        }

        public void DrawForeground(SpriteBatch sb, RectangleF cameraBoundingBox, float layerDepth)
        {
            if (mapFGCave != null)
            {
                sb.Draw(mapFGCave, Vector2.Zero, Color.White * foregroundCaveTransparency);
            }
            foreground.Draw(sb, cameraBoundingBox, layerDepth, foregroundCaveTransparency);
        }

        private int GetTileIdFor(int tileX, int tileY)
        {
            int tileGlobalId = 0;
            TiledMapTileLayer layer = (TiledMapTileLayer)tiledMap.GetLayer("base");
            TiledMapTile? t;
            layer.TryGetTile((ushort)tileX, (ushort)tileY, out t);
            if (t != null)
            {
                tileGlobalId = t.Value.GlobalIdentifier;
            }
            return tileGlobalId;
        }

        public Color GetPrimaryColorForTile(int tileX, int tileY)
        {
            int id = GetTileIdFor(tileX, tileY);
            if(WOODEN_BRIDGE_IDS.Contains(id) )
            {
                return Util.BRIDGE_PRIMARY.color;
            } else if(METAL_BRIDGE_IDS.Contains(id))
            {
                return Util.BRIGHT_METAL_PRIMARY.color;
            }
            else if (ORANGE_EARTH_TILE_IDS.Contains(id))
            {
                return Util.ORANGE_EARTH_PRIMARY.color;
            } 
            else if (BEACH_SAND_TILE_IDS.Contains(id))
            {
                return Util.BEACH_SAND_PRIMARY.color;
            } 
            else if (WOOD_INTERIOR_TILE_IDS.Contains(id))
            {
                return Util.WOOD_PRIMARY.color;
            }
            else if (WHITE_EARTH_TILE_IDS.Contains(id))
            {
                return Util.WHITE_EARTH_PRIMARY.color;
            }
            else if (BROWN_RUINS_TILE_IDS.Contains(id))
            {
                return Util.BROWN_RUINS_PRIMARY.color;
            }
            else if (CAVE_STONE_TILE_IDS.Contains(id))
            {
                return Util.CAVE_STONE_PRIMARY.color;
            }
            else if (METAL_TILE_IDS.Contains(id))
            {
                return Util.METAL_TILE_PRIMARY.color;
            }
            else if (BROWN_BARK_TILE_IDS.Contains(id))
            {
                return Util.BROWN_BARK_PRIMARY.color;
            }
            else if (RED_SAND_TILE_IDS.Contains(id))
            {
                return Util.RED_SAND_PRIMARY.color;
            }
            else if (BLACK_GROUND_TILE_IDS.Contains(id))
            {
                return Util.BLACK_GROUND_PRIMARY.color;
            }
            else if (METAL_INTERIOR_TILE_IDS.Contains(id))
            {
                return Util.BRIGHT_METAL_PRIMARY.color;
            }
            else if (BROWN_MUD_TILE_IDS.Contains(id))
            {
                return Util.BROWN_MUD_PRIMARY.color;
            }
            else if (WHITE_INTERIOR_TILE_IDS.Contains(id))
            {
                return Util.WHITE_EARTH_PRIMARY.color;
            }
            else if (LAB_METAL_TILE_IDS.Contains(id))
            {
                return Util.BRIGHT_METAL_PRIMARY.color;
            }
            else if (ORANGE_VOLCANO_TILE_IDS.Contains(id))
            {
                return Util.ORANGE_VOLCANO_PRIMARY.color;
            }
            return Util.TRANSPARENT.color;
        }

        public Color GetSecondaryColorForTile(int tileX, int tileY)
        {
            int id = GetTileIdFor(tileX, tileY);
            if (WOODEN_BRIDGE_IDS.Contains(id))
            {
                return Util.BRIDGE_SECONDARY.color;
            }
            else if (METAL_BRIDGE_IDS.Contains(id))
            {
                return Util.BRIGHT_METAL_PRIMARY.color;
            }
            else if (ORANGE_EARTH_TILE_IDS.Contains(id))
            {
                return Util.ORANGE_EARTH_SECONDARY.color;
            }
            else if (BEACH_SAND_TILE_IDS.Contains(id))
            {
                return Util.BEACH_SAND_SECONDARY.color;
            }
            else if (WOOD_INTERIOR_TILE_IDS.Contains(id))
            {
                return Util.WOOD_SECONDARY.color;
            } 
            else if (WHITE_EARTH_TILE_IDS.Contains(id)) {
                return Util.WHITE_EARTH_SECONDARY.color;
            }
            else if (BROWN_RUINS_TILE_IDS.Contains(id)) {
                return Util.BROWN_RUINS_SECONDARY.color;
            }
            else if (CAVE_STONE_TILE_IDS.Contains(id)) {
                return Util.CAVE_STONE_SECONDARY.color;
            }
            else if (METAL_TILE_IDS.Contains(id)) {
                return Util.METAL_TILE_SECONDARY.color;
            }
            else if (BROWN_BARK_TILE_IDS.Contains(id)) {
                return Util.BROWN_BARK_SECONDARY.color;
            }
            else if (RED_SAND_TILE_IDS.Contains(id)) {
                return Util.RED_SAND_SECONDARY.color;
            }
            else if (BLACK_GROUND_TILE_IDS.Contains(id)) {
                return Util.BLACK_GROUND_SECONDARY.color;
            }
            else if (METAL_INTERIOR_TILE_IDS.Contains(id)) {
                return Util.BRIGHT_METAL_SECONDARY.color;
            }
            else if (WHITE_INTERIOR_TILE_IDS.Contains(id))
            {
                return Util.WHITE_EARTH_SECONDARY.color;
            }
            else if (BROWN_MUD_TILE_IDS.Contains(id))
            {
                return Util.BROWN_MUD_SECONDARY.color;
            }
            else if (LAB_METAL_TILE_IDS.Contains(id))
            {
                return Util.BRIGHT_METAL_SECONDARY.color;
            }
            else if (ORANGE_VOLCANO_TILE_IDS.Contains(id))
            {
                return Util.ORANGE_VOLCANO_SECONDARY.color;
            }
            return Util.TRANSPARENT.color;
        }

        public void Tick(int length, EntityPlayer player, World world)
        {
            for(int i = 0; i < entityListManager.GetEntityList().Count; i++)
            {
                if(entityListManager.GetEntityList()[i] is ITick)
                {
                    ((ITick)entityListManager.GetEntityList()[i]).Tick(length, player, this, world);
                }
            }
        }

        public Waypoint GetWaypoint(string name)
        {
            foreach(Waypoint sp in waypoints)
            {
                if(sp.name.Equals(name))
                {
                    return sp;
                }
            }
            throw new Exception("Waypoint " + name + " not found!");
        }

        public TransitionZone CheckTransition(Vector2 position, bool interacting)
        {
            foreach(TransitionZone tz in transitions)
            {
                if (tz.automatic || interacting)
                {
                    if (tz.rectangle.Contains(position))
                    {
                        return tz;
                    }
                }
            }
            return null;
        }

        public void AddParticle(Particle particle)
        {
            this.particleList.Add(particle);
        }

        private float GetDarkLevelForPosition(Vector2 position)
        {
            foreach(LightingZone lz in lightingZones)
            {
                if(lz.rect.Contains(position))
                {
                    return lz.darkLevel;
                }
            }
            return 1.0f;
        }

        public float GetDarkLevel(Vector2 position)
        {
            return baseDarkLevel * positionalDarkLevel;
        }
    }
}
