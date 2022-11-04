using Microsoft.Xna.Framework;
using Plateau.Entities;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class SaveManager
    {
        public static string FILE_NAME_1 = "save1.txt";
        public static string FILE_NAME_2 = "save2.txt";
        public static string FILE_NAME_3 = "save3.txt";

        private List<SaveState> saveStates;
        private string fileName;

        public SaveManager(string fileName)
        {
            this.fileName = fileName;
            saveStates = new List<SaveState>();
        }

        public void LoadFile(EntityPlayer player, World world)
        {
            List<SaveState> loadedStates = new List<SaveState>();
            string line = "";
            using(StreamReader sr = new StreamReader(fileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    loadedStates.Add(new SaveState(line));
                }
            }

            if(loadedStates.Count == 0) //new game init
            {
                //place player in tutorial cave cave
                world.ChangeArea(world.GetAreaDict()[Area.AreaEnum.S0]);
                world.GetCurrentArea().MoveToWaypoint(player, "SPtutorial");
                for (int i = 0; i < 28; i++)
                {
                    world.AdvanceDay(player);
                }
            } else
            {
                //place player in front of house
                world.ChangeArea(world.GetAreaDict()[Area.AreaEnum.FARM]);
                world.GetCurrentArea().MoveToWaypoint(player, "SPfarmhouseOutside");

                foreach (SaveState saveState in loadedStates)
                {
                    if (saveState.GetIdentifier() == SaveState.Identifier.PLAYER)
                    {
                        player.LoadSave(saveState);
                    }
                    else if (saveState.GetIdentifier() == SaveState.Identifier.PLACEABLE)
                    {
                        string areaName = saveState.TryGetData("area", "NONE");
                        foreach (Area.AreaEnum areaEnum in world.GetAreaDict().Keys)
                        {
                            if (areaEnum.ToString().Equals(areaName))
                            {
                                Area area = world.GetAreaByEnum(areaEnum);
                                Vector2 location = new Vector2(Int32.Parse(saveState.TryGetData("positionX", "NONE")), Int32.Parse(saveState.TryGetData("positionY", "NONE")));
                                Vector2 tile = new Vector2(Int32.Parse(saveState.TryGetData("tileX", "NONE")), Int32.Parse(saveState.TryGetData("tileY", "NONE")));
                                Item item = ItemDict.GetItemByName(saveState.TryGetData("sourceitem", ItemDict.NONE.GetName()));
                                EntityType type = EntityFactory.StringToEntityType(saveState.TryGetData("entitytype", "NONE"));
                                TileEntity restoredEntity = (TileEntity)EntityFactory.GetEntity(type, item, tile, world.GetAreaByEnum(areaEnum));
                                if (item == ItemDict.NONE)
                                {
                                    //remove duplicates - for tile entities placed from map editor
                                    if (area.GetTileEntity((int)tile.X, (int)tile.Y) != null)
                                    {
                                        area.RemoveTileEntity(player, (int)tile.X, (int)tile.Y, world);
                                    }
                                    world.GetAreaByEnum(areaEnum).AddTileEntity(restoredEntity);
                                }
                                else if (((PlaceableItem)item).GetPlacementType() == PlaceableItem.PlacementType.WALL)
                                {
                                    world.GetAreaByEnum(areaEnum).AddWallEntity(restoredEntity);
                                }
                                else if (((PlaceableItem)item).GetPlacementType() == PlaceableItem.PlacementType.WALLPAPER)
                                {
                                    world.GetAreaByEnum(areaEnum).AddWallpaperEntity((PEntityWallpaper)restoredEntity);
                                }
                                else //Normal & Floor
                                {
                                    world.GetAreaByEnum(areaEnum).AddTileEntity(restoredEntity);
                                }
                                restoredEntity.LoadSave(saveState);
                                for (int i = 0; i < 10; i++)
                                {
                                    restoredEntity.Update(1.0f, area); //skip animation
                                }
                            }
                        }
                    }
                    else if (saveState.GetIdentifier() == SaveState.Identifier.WORLD)
                    {
                        world.LoadSave(saveState);
                    }
                    else if (saveState.GetIdentifier() == SaveState.Identifier.GAMESTATE)
                    {
                        GameState.LoadSave(saveState);
                    }
                    else if (saveState.GetIdentifier() == SaveState.Identifier.BUILDING_BLOCK)
                    {
                        string areaName = saveState.TryGetData("area", "NONE");
                        foreach (Area.AreaEnum areaEnum in world.GetAreaDict().Keys)
                        {
                            if (areaEnum.ToString().Equals(areaName))
                            {
                                BuildingBlockItem item = (BuildingBlockItem)ItemDict.GetItemByName(saveState.TryGetData("sourceitem", "ERROR"));
                                Vector2 tile = new Vector2(Int32.Parse(saveState.TryGetData("tileX", "NONE")), Int32.Parse(saveState.TryGetData("tileY", "NONE")));
                                BuildingBlock restoredBB = new BuildingBlock(item, tile, item.GetPlacedTexture(), item.GetBlockType());
                                world.GetAreaByEnum(areaEnum).AddBuildingBlock(restoredBB);
                                restoredBB.LoadSave(saveState);
                            }
                        }
                    }
                    /*(else if (saveState.GetIdentifier() == SaveState.Identifier.VENDORENTITY)
                    {
                        string areaName = saveState.TryGetData("area", "NONE");
                        string id = saveState.TryGetData("id", "NONE");
                        foreach (Area.AreaEnum areaEnum in world.GetAreaDict().Keys)
                        {
                            if (areaEnum.ToString().Equals(areaName))
                            {
                                MEntityVendor vendor = (MEntityVendor)world.GetAreaByEnum(areaEnum).GetPlacedFromMapEntityById(id);
                                vendor.LoadSave(saveState);
                            }
                        }
                    }*/
                    else if (saveState.GetIdentifier() == SaveState.Identifier.SHIPPING_BIN)
                    {
                        string areaName = saveState.TryGetData("area", "NONE");
                        string id = saveState.TryGetData("id", "NONE");
                        foreach (Area.AreaEnum areaEnum in world.GetAreaDict().Keys)
                        {
                            if (areaEnum.ToString().Equals(areaName))
                            {
                                TEntityShippingBin bin = (TEntityShippingBin)world.GetAreaByEnum(areaEnum).GetPlacedFromMapEntityById(id);
                                bin.LoadSave(saveState);
                            }
                        }
                    } else if (saveState.GetIdentifier() == SaveState.Identifier.CUTSCENES)
                    {
                        CutsceneManager.LoadSave(saveState);
                    } else if (saveState.GetIdentifier() == SaveState.Identifier.CHARACTER)
                    {
                        Enum.TryParse(saveState.TryGetData("character", "ERROR"), out EntityCharacter.CharacterEnum characterEnum);
                        world.GetCharacter(characterEnum).LoadSave(saveState);
                    } else if (saveState.GetIdentifier() == SaveState.Identifier.SOULCHEST)
                    {
                        PEntitySoulchest.LoadSave(saveState);
                    }
                }

                //now that flags are loaded, move player to their house
                int houseUpgradeLevel = GameState.GetFlagValue(GameState.FLAG_HOUSE_UPGRADE_LEVEL);
                switch (houseUpgradeLevel) //move player to their bed/house
                {
                    case 0:
                        world.ChangeArea(world.GetAreaDict()[Area.AreaEnum.FARM]);
                        world.GetCurrentArea().MoveToWaypoint(player, "SPfarmhouseOutside");
                        break;
                    case 1:
                        world.ChangeArea(world.GetAreaDict()[Area.AreaEnum.INTERIOR]);
                        world.GetCurrentArea().MoveToWaypoint(player, "SPfarmhouseCabinBed");
                        break;
                    case 2:
                        world.ChangeArea(world.GetAreaDict()[Area.AreaEnum.INTERIOR]);
                        world.GetCurrentArea().MoveToWaypoint(player, "SPfarmhouseHouseBed");
                        break;
                    case 3:
                    default:
                        world.ChangeArea(world.GetAreaDict()[Area.AreaEnum.INTERIOR]);
                        world.GetCurrentArea().MoveToWaypoint(player, "SPfarmhouseMansionBed");
                        break;
                }
            }
        }

        public static bool DoesSaveExist(int i)
        {
            FileInfo fi = null;

            switch (i)
            {
                case 1:
                    fi = new FileInfo(FILE_NAME_1);
                    break;
                case 2:
                    fi = new FileInfo(FILE_NAME_2);
                    break;
                case 3:
                    fi = new FileInfo(FILE_NAME_3);
                    break;
            }
            

            return !(fi.Length == 0) ;
        }

        public void SaveFile(EntityPlayer player, World world)
        {
            saveStates.Add(player.GenerateSave());
            foreach(Area.AreaEnum areaEnum in world.GetAreaDict().Keys)
            {
                Area area = world.GetAreaByEnum(areaEnum);
                area.AddEntitySaveStates(saveStates);
                area.AddBuildingBlockSaveStates(saveStates);
            }
            saveStates.Add(world.GenerateSave());
            saveStates.Add(GameState.GenerateSave());
            saveStates.Add(CutsceneManager.GenerateSave());
            saveStates.Add(PEntitySoulchest.GenerateSave());

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                foreach (SaveState state in saveStates)
                {
                    sw.WriteLine(state.ToString());
                }
            }

            saveStates.Clear();
        }
    }
}
