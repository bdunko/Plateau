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
    public class DebugConsole
    {
        private World world;
        private SaveManager saveManager;
        private EntityPlayer player;
        private bool didLastSucceed;

        public static string[] COMMAND_LIST = {"help", "save", "give <item>", "givedyes", "givetops", "givebottoms", "givehats", "giveaccessories", "clearinv", "resetinv", "gold <x>", "bankrupt", "hair <item", "haircolor <color>", "skin <item>", "eyes <item>", "timeset <hour> <minute>", "nextday <days>", "warp <area>", "move <x> <y>", "launch" };


        public DebugConsole(World world, SaveManager saveManager, EntityPlayer player)
        {
            this.world = world;
            this.saveManager = saveManager;
            this.player = player;
            this.didLastSucceed = true;
        }

        public bool DidLastSucceed()
        {
            return didLastSucceed;
        }

        public void RunCommand(string command)
        {
            didLastSucceed = false;
            command = command.ToLower();
            string[] pieces = command.Split(' ');
            Console.WriteLine("Running command: " + command);

            if (pieces[0].Equals("info"))
            {
                Console.WriteLine("Current Area:         " + world.GetCurrentArea().GetAreaEnum().ToString()); 
                Console.WriteLine("Player Position:      " + player.GetAdjustedPosition());
                Console.WriteLine("Weather:              " + world.GetWeather().ToString());
                didLastSucceed = true;
            }
            else if (pieces[0].Equals("save"))
            {
                saveManager.SaveFile(player, world);
                didLastSucceed = true;
            } else if (pieces[0].Equals("fup"))
            {
                PlateauMain.FONT_SCALE += 0.0025f;
                didLastSucceed = true;
                System.Diagnostics.Debug.WriteLine(PlateauMain.FONT_SCALE);
            } else if (pieces[0].Equals("fdown"))
            {
                PlateauMain.FONT_SCALE -= 0.0025f;
                didLastSucceed = true;
                System.Diagnostics.Debug.WriteLine(PlateauMain.FONT_SCALE);
            } else if (pieces[0].Equals("fsup"))
            {
                PlateauMain.FONT.Spacing += 0.5f;
                didLastSucceed = true;
                System.Diagnostics.Debug.WriteLine(PlateauMain.FONT.Spacing);
            } else if (pieces[0].Equals("fsdown"))
            {
                PlateauMain.FONT.Spacing -= 0.5f;
                didLastSucceed = true;
                System.Diagnostics.Debug.WriteLine(PlateauMain.FONT.Spacing);
            }
            else if (pieces[0].Equals("flsup"))
            {
                PlateauMain.FONT.LineSpacing += 1;
                didLastSucceed = true;
                System.Diagnostics.Debug.WriteLine(PlateauMain.FONT.LineSpacing);
            }
            else if (pieces[0].Equals("flsdown"))
            {
                PlateauMain.FONT.LineSpacing -= 1;
                didLastSucceed = true;
                System.Diagnostics.Debug.WriteLine(PlateauMain.FONT.LineSpacing);
            } else if (pieces[0].Equals("fontdata"))
            {
                System.Diagnostics.Debug.WriteLine("SCALE: " + PlateauMain.FONT_SCALE);
                System.Diagnostics.Debug.WriteLine("SPACING: " + PlateauMain.FONT.Spacing);
                System.Diagnostics.Debug.WriteLine("LINE SPACING: " + PlateauMain.FONT.LineSpacing);
                didLastSucceed = true;
            } else if (pieces[0].Equals("flag"))
            {
                string flag = pieces[1];
                int flagValue = 0;
                if (GameState.ContainsFlag(flag))
                {
                    if(Int32.TryParse(pieces[2], out flagValue))
                    {
                        GameState.SetFlag(flag, flagValue);
                    }

                    didLastSucceed = true;
                } else
                {
                    didLastSucceed = false;
                }
            }
            else if (pieces[0].Equals("timeset"))
            {
                if(pieces.Count() == 2)
                {
                    int hour = 0;
                    if (Int32.TryParse(pieces[1], out hour))
                    {
                        world.SetTime(hour, 0, player);
                        didLastSucceed = true;
                    }
                } else if (pieces.Count() == 3)
                {
                    int hour = 0, minute;
                    if (Int32.TryParse(pieces[1], out hour))
                    {
                        if (Int32.TryParse(pieces[2], out minute))
                        {
                            world.SetTime(hour, minute, player);
                            didLastSucceed = true;
                        }
                    }
                }
            }
            else if (pieces[0].Equals("warp"))
            {
                if(pieces.Count() != 1)
                {
                    string areaName = pieces[1].ToUpper();
                    foreach (Area.AreaEnum areaEnum in world.GetAreaDict().Keys)
                    {
                        if (areaEnum.ToString().Equals(areaName))
                        {
                            world.ChangeArea(world.GetAreaDict()[areaEnum]);
                            if(areaEnum == Area.AreaEnum.FARM)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPfarmhouseOutside");
                            }
                            if (areaEnum == Area.AreaEnum.TOWN)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPstore");
                            }
                            if (areaEnum == Area.AreaEnum.BEACH)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPbeachElevator");
                            }
                            if (areaEnum == Area.AreaEnum.S0)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPtop");
                            }
                            if (areaEnum == Area.AreaEnum.S1)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPs1entrance");
                            }
                            if (areaEnum == Area.AreaEnum.S2)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPs2entrance");
                            }
                            if (areaEnum == Area.AreaEnum.S3)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPs3entrance");
                            }
                            if (areaEnum == Area.AreaEnum.S4)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPs4entrance");
                            }
                            if (areaEnum == Area.AreaEnum.APEX)
                            {
                                world.GetCurrentArea().MoveToWaypoint(player, "SPapexEntrance");
                            }
                            didLastSucceed = true;
                            break;
                        }
                    }
                }
            }
            else if (pieces[0].Equals("move"))
            {
                int x, y;
                if (pieces[1].Equals("x"))
                {
                    if (Int32.TryParse(pieces[2], out y))
                    {
                        player.SetPosition(new Vector2(player.GetAdjustedPosition().X, y));
                        didLastSucceed = true;
                    }
                } else if (pieces[2].Equals("y"))
                {
                    if (Int32.TryParse(pieces[1], out x))
                    {
                        player.SetPosition(new Vector2(x, player.GetAdjustedPosition().Y - 10));
                        didLastSucceed = true;
                    }
                }
                else if (Int32.TryParse(pieces[1], out x))
                {
                    if (Int32.TryParse(pieces[2], out y))
                    {
                        player.SetPosition(new Vector2(x, y));
                        didLastSucceed = true;
                    }
                }
            }
            else if (pieces[0].Equals("give"))
            {
                string itemName = "";
                for(int i = 1; i < pieces.Length; i++)
                {
                    itemName += pieces[i];
                    itemName += " ";
                }
                Item toGive = ItemDict.GetItemByName(itemName.Substring(0, itemName.Length-1));
                if (toGive != ItemDict.NONE)
                {
                    for (int i = 0; i < toGive.GetStackCapacity(); i++)
                    {
                        player.AddItemToInventory(toGive, false, true);
                    }
                    didLastSucceed = true;
                }
            }
            else if (pieces[0].Equals("nextday"))
            {
                if (pieces.Count() == 1)
                {
                    world.AdvanceDay(player);
                    saveManager.SaveFile(player, world);
                    didLastSucceed = true;
                } else if (pieces.Count() == 2)
                {
                    int num;
                    if (Int32.TryParse(pieces[1], out num))
                    {
                        for(int i = 0; i < num; i++)
                        {
                            world.AdvanceDay(player);
                        }
                        saveManager.SaveFile(player, world);
                        didLastSucceed = true;
                    }
                }
            }
            else if (pieces[0].Equals("launch"))
            {
                player.SetPosition(new Vector2(player.GetAdjustedPosition().X, 0));
                didLastSucceed = true;
            }
            else if (pieces[0].Equals("commands") || pieces[0].Equals("help"))
            {
                Console.WriteLine("info - outputs information");
                Console.WriteLine("save - saves data");
                Console.WriteLine("give <item> - gives player item");
                Console.WriteLine("givedyes - gives player a stack of all dyes");
                Console.WriteLine("givetops - gives all shirts and outerwear items");
                Console.WriteLine("givebottoms - gives all pants, shoes, and socks");
                Console.WriteLine("givehats - gives all hats and glasses");
                Console.WriteLine("giveaccessories - gives all back items, neckwear, earrings, gloves, sailcloths");
                Console.WriteLine("clearinv - removes all items from inventory");
                Console.WriteLine("resetinv - clears inventory; then gives tools");
                Console.WriteLine("gold <x> - gives x gold");
                Console.WriteLine("bankrupt - sets gold to 0");
                Console.WriteLine("hair <item> - sets the player hair");
                Console.WriteLine("haircolor <color> - sets the player hair color - (for a haircolor, leave out the Hair part: the color Hair Magma Red should be called as haircolor magma red");
                Console.WriteLine("skin <item> - sets the player skin");
                Console.WriteLine("eyes <item> - sets the player eyes");
                Console.WriteLine("timeset <hour> <minute> - sets current time; minute parameter is optional");
                Console.WriteLine("nextday - advances day");
                Console.WriteLine("nextday <x> - advances x days");
                Console.WriteLine("warp <area> - changes current area");
                Console.WriteLine("move <x> <y> - sets player xy position");
                Console.WriteLine("move x <y> - sets player y position, x stays same");
                Console.WriteLine("move <x> y - sets player x position, y stays same");
                Console.WriteLine("launch - sets player y to top of screen");


                didLastSucceed = true;
            }
            else if (pieces[0].Equals("gold"))
            {
                if(pieces.Count() == 2)
                {
                    int goldAmount = 0;
                    if (Int32.TryParse(pieces[1], out goldAmount))
                    {
                        player.GainGold(goldAmount);
                        didLastSucceed = true;
                    }
                }
            }
            else if (pieces[0].Equals("bankrupt"))
            {
                player.SpendGold(player.GetGold());
                didLastSucceed = true;
            }
            else if (pieces[0].Equals("hair"))
            {
                string currentHairColor = player.GetHair().GetItem().GetName().Split('(')[1];
                currentHairColor = "(" + currentHairColor;
                string itemName = "";
                for (int i = 1; i < pieces.Length; i++)
                {
                    itemName += pieces[i];
                    itemName += " ";
                }
                Item toGive = ItemDict.GetItemByName(itemName.Substring(0, itemName.Length - 1) + " " + currentHairColor);
                if (toGive != ItemDict.NONE && toGive.HasTag(Item.Tag.HAIR))
                {
                    player.SetHair(new ItemStack(toGive, 1));
                    didLastSucceed = true;
                }
            }
            else if (pieces[0].Equals("skin"))
            {
                string itemName = "";
                for (int i = 1; i < pieces.Length; i++)
                {
                    itemName += pieces[i];
                    itemName += " ";
                }
                Item toGive = ItemDict.GetItemByName(itemName.Substring(0, itemName.Length - 1));
                if (toGive != ItemDict.NONE && toGive.HasTag(Item.Tag.SKIN))
                {
                    player.SetSkin(new ItemStack(toGive, 1));
                    didLastSucceed = true;
                }
            }
            else if (pieces[0].Equals("eyes"))
            {
                string itemName = "";
                for (int i = 1; i < pieces.Length; i++)
                {
                    itemName += pieces[i];
                    itemName += " ";
                }
                Item toGive = ItemDict.GetItemByName(itemName.Substring(0, itemName.Length - 1));
                if (toGive != ItemDict.NONE && toGive.HasTag(Item.Tag.EYES))
                {
                    player.SetEyes(new ItemStack(toGive, 1));
                    didLastSucceed = true;
                }
            }
            else if (pieces[0].Equals("clearinv"))
            {
                for(int i = 0; i < EntityPlayer.INVENTORY_SIZE; i++)
                {
                    player.RemoveItemStackAt(i);
                }
                didLastSucceed = true;
            }
            else if (pieces[0].Equals("givedyes"))
            {
                RunCommand("give un-dye");
                RunCommand("give white dye");
                RunCommand("give light grey dye");
                RunCommand("give dark grey dye");
                RunCommand("give black dye");
                RunCommand("give navy dye");
                RunCommand("give blue dye");
                RunCommand("give red dye");
                RunCommand("give pink dye");
                RunCommand("give orange dye");
                RunCommand("give yellow dye");
                RunCommand("give dark brown dye");
                RunCommand("give light brown dye");
                RunCommand("give green dye");
                RunCommand("give olive dye");
                RunCommand("give purple dye");
                didLastSucceed = true;
            }
            else if (pieces[0].Equals("resetinv"))
            {
                RunCommand("clearinv");
                RunCommand("give hoe");
                RunCommand("give watering can");
                RunCommand("give pickaxe");
                RunCommand("give axe");
                RunCommand("give fishing rod");
                didLastSucceed = true;
            }
            else if (pieces[0].Equals("resetinv2"))
            {
                RunCommand("clearinv");
                RunCommand("give adamantite hoe");
                RunCommand("give adamantite can");
                RunCommand("give adamantite pickaxe");
                RunCommand("give adamantite axe");
                RunCommand("give adamantite rod");
                didLastSucceed = true;
            }
            else if (pieces[0].Equals("haircolor"))
            {
                string hairName = ItemDict.GetColoredItemBaseForm(player.GetHair().GetItem());
                if (pieces.Length != 1)
                {
                    hairName += " (Hair ";
                    for (int i = 1; i < pieces.Length; i++)
                    {
                        hairName += pieces[i];
                        hairName += " ";
                    }
                    hairName = hairName.Substring(0, hairName.Length - 1);
                    hairName += ")";
                }
                Item toGive = ItemDict.GetItemByName(hairName);
                if (toGive != ItemDict.NONE && toGive.HasTag(Item.Tag.HAIR))
                {
                    player.SetHair(new ItemStack(toGive, 1));
                    didLastSucceed = true;
                }
            } else if (pieces[0].Equals("weather"))
            {
                string weatherName = pieces[1].ToUpper();
                World.Weather weather;
                bool success = Enum.TryParse(weatherName, out weather);
                if(success)
                    world.SetWeather(weather);
                didLastSucceed = success;
            } else if (pieces[0].Equals("unlockallrecipes"))
            {
                GameState.UnlockAllRecipes();
                didLastSucceed = true;
            } else if (pieces[0].Equals("givetops"))
            {
                List<Item> toGive = ItemDict.GetAllOfTag(Item.Tag.SHIRT);
                foreach(Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.OUTERWEAR);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }
            }
            else if (pieces[0].Equals("givebottoms"))
            {
                List<Item> toGive = ItemDict.GetAllOfTag(Item.Tag.PANTS);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.SOCKS);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.SHOES);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }
            } else if (pieces[0].Equals("givehats"))
            {
                List<Item> toGive = ItemDict.GetAllOfTag(Item.Tag.HAT);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.GLASSES);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }
            } else if (pieces[0].Equals("giveaccessories"))
            {
                List<Item> toGive = ItemDict.GetAllOfTag(Item.Tag.BACK);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.EARRINGS);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.GLOVES);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.SCARF);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }

                toGive = ItemDict.GetAllOfTag(Item.Tag.SAILCLOTH);
                foreach (Item item in toGive)
                {
                    RunCommand("give " + item.GetName());
                }
            } 
            else if (pieces[0].Equals("unlockrecipe"))
            {
                string itemName = "";
                if(pieces.Length != 1) {
                    for (int i = 1; i < pieces.Length; i++)
                    {
                        itemName += pieces[i];
                        itemName += " ";
                    }
                    Item toUnlock = ItemDict.GetItemByName(itemName.Substring(0, itemName.Length - 1));
                    didLastSucceed = true;
                } else
                {
                    didLastSucceed = false;
                }
            }
            else
            {
                Console.WriteLine("Illegal command - see \"commands\"");
            }
        }
    }
}
