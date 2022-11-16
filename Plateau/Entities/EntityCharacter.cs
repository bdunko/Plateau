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
    public class EntityCharacter : EntityCollidable, ITickDaily, ITick, IInteract, IPersist
    {
        private static int HEIGHT = 24; //height of character's hitbox
        private static int WIDTH = 8; //width of character's hitbox
        private static int OFFSET_X = 29; //offset x from where the image is to where the hitbox begins
        private static int OFFSET_Y = 8; //offset y from where the image border is to where the hitbox begins

        private static float SPEED = 38;
        //private static float SPEED = 67;
        private static float SPEED_WHILE_JUMPING = 30;
        private static float GRAVITY = 8;
        private static float JUMP_SPEED = -2.55F;
        private static int COLLISION_STEPS = 3;

        private AnimatedSprite emotionPanel;

        private enum FadeState
        {
            FADE_OUT, FADE_IN, NONE
        }
        private static float FADE_SPEED = 2.5f;
        private FadeState fadeState;
        private float opacity;

        public enum CharacterEnum
        {
            AIDEN, CADE, CAMUS, CECILY, CHARLOTTE, CLAUDE, ELLE, FINLEY, HIMEKO, MEREDITH, OTIS, PAIGE, PIPER, RAUL, ROCKWELL, SKYE, TROY
        }

        public class ClothingSet
        {
            public ClothingItem hat, shirt, outerwear, pants, socks, shoes, gloves, earrings, scarf, glasses, back, sailcloth, facialhair;
            public ClothingItem skin, hair, eyes;
            public Func<World, EntityCharacter, bool>[] conditionFunctions;
            public int priority;

            public ClothingSet(ClothingItem skin, ClothingItem hair, ClothingItem eyes, ClothingItem hat, ClothingItem shirt, ClothingItem outerwear, ClothingItem pants,
                ClothingItem shoes, ClothingItem back, ClothingItem glasses, ClothingItem socks, ClothingItem gloves, ClothingItem earrings, ClothingItem scarf, ClothingItem sailcloth, ClothingItem facialhair,
                Func<World, EntityCharacter, bool>[] conditionFunctions, int priority)
            {
                this.hat = hat;
                this.shirt = shirt;
                this.outerwear = outerwear;
                this.pants = pants;
                this.socks = socks;
                this.shoes = shoes;
                this.gloves = gloves;
                this.earrings = earrings;
                this.scarf = scarf;
                this.glasses = glasses;
                this.back = back;
                this.sailcloth = sailcloth;
                this.skin = skin;
                this.hair = hair;
                this.eyes = eyes;
                this.facialhair = facialhair;
                this.conditionFunctions = conditionFunctions;
                this.priority = priority;
            }

            public bool CheckCondition(World world, EntityCharacter character)
            {
                foreach (Func<World, EntityCharacter, bool> condition in conditionFunctions)
                {
                    if (!condition(world, character))
                    {
                        return false;
                    }
                }
                return true;
            }

            public void UpdateClothedSprite(float deltaTime, ClothedSprite sprite)
            {
                bool drawPantsOverShoes = pants.HasTag(Item.Tag.DRAW_OVER_SHOES);
                bool hideHair = false;
                bool hideFacialHair = false;
                if (hat != ItemDict.CLOTHING_NONE && hair.HasTag(Item.Tag.HIDE_WHEN_HAT))
                {
                    hideHair = true;
                }
                if (hat.HasTag(Item.Tag.ALWAYS_HIDE_HAIR))
                {
                    hideHair = true;
                    hideFacialHair = true;
                }
                else if (hat.HasTag(Item.Tag.ALWAYS_SHOW_HAIR))
                {
                    hideHair = false;
                }
                sprite.Update(deltaTime, hat.GetSpritesheet(), shirt.GetSpritesheet(), outerwear.GetSpritesheet(),
                pants.GetSpritesheet(), socks.GetSpritesheet(), shoes.GetSpritesheet(),
                gloves.GetSpritesheet(), earrings.GetSpritesheet(), scarf.GetSpritesheet(), glasses.GetSpritesheet(),
                back.GetSpritesheet(), sailcloth.GetSpritesheet(), hair.GetSpritesheet(), skin.GetSpritesheet(), eyes.GetSpritesheet(),
                facialhair.GetSpritesheet(), drawPantsOverShoes, hideHair, hideFacialHair);
            }
        }

        private class ClothingManager
        {
            private List<ClothingSet> clothingSets;
            private ClothingSet currentSet;

            public ClothingManager(List<ClothingSet> sets)
            {
                this.clothingSets = sets;
                this.currentSet = clothingSets[0];
            }

            public void ChooseClothingSet(World world, EntityCharacter character)
            {
                currentSet = clothingSets[0];
                foreach (ClothingSet set in clothingSets)
                {
                    if (set.CheckCondition(world, character) && set.priority > currentSet.priority)
                    {
                        currentSet = set;
                    }
                }
            }

            public void Update(float deltaTime, ClothedSprite sprite)
            {
                currentSet.UpdateClothedSprite(deltaTime, sprite);
            }

            public void TickDaily(World world, EntityCharacter character)
            {
                ChooseClothingSet(world, character);
            }
        }

        public class Schedule
        {
            private class SubzoneMap
            {
                private class Node
                {
                    public class ConnectedNode
                    {
                        public MovementTypeWaypoint waypoint;
                        public Node nodeTo;

                        public ConnectedNode(Node nodeTo, MovementTypeWaypoint waypoint)
                        {
                            this.nodeTo = nodeTo;
                            this.waypoint = waypoint;
                        }
                    }

                    public List<ConnectedNode> neighbors;
                    public Area.Subarea.NameEnum subzone;
                    public Node(Area.Subarea.NameEnum subzone)
                    {
                        this.subzone = subzone;
                        this.neighbors = new List<ConnectedNode>();
                    }

                    public void AddNeighbor(Node neighbor, MovementTypeWaypoint connection)
                    {
                        this.neighbors.Add(new ConnectedNode(neighbor, connection));
                    }
                }

                private static List<Node> allNodesInMap;
                public SubzoneMap(World world)
                {
                    Node apex, beach, farm, s0walk, s0warp, store, cafe, bookstoreLower, bookstoreUpper, s1walk, s1warp,
                        farmhouseCabin, farmhouseHouse, farmhouseMansionUpper, farmhouseMansionLower, rockwellHouse, beachHouse, piperLower, piperUpper, townhallLower, townhallUpper,
                        workshop, forge, town, s2, s3, s4, inn, innBath, innTroy, innCharlotte, innSpare1, innSpare2, innSpare3;

                    allNodesInMap = new List<Node>();
                    allNodesInMap.Add(apex = new Node(Area.Subarea.NameEnum.APEX));
                    allNodesInMap.Add(beach = new Node(Area.Subarea.NameEnum.BEACH));
                    allNodesInMap.Add(farm = new Node(Area.Subarea.NameEnum.FARM));
                    allNodesInMap.Add(s0walk = new Node(Area.Subarea.NameEnum.S0WALK));
                    allNodesInMap.Add(s0warp = new Node(Area.Subarea.NameEnum.S0WARP));
                    allNodesInMap.Add(store = new Node(Area.Subarea.NameEnum.STORE));
                    allNodesInMap.Add(cafe = new Node(Area.Subarea.NameEnum.CAFE));
                    allNodesInMap.Add(bookstoreLower = new Node(Area.Subarea.NameEnum.BOOKSTORELOWER));
                    allNodesInMap.Add(bookstoreUpper = new Node(Area.Subarea.NameEnum.BOOKSTOREUPPER));
                    allNodesInMap.Add(s1walk = new Node(Area.Subarea.NameEnum.S1WALK));
                    allNodesInMap.Add(s1warp = new Node(Area.Subarea.NameEnum.S1WARP));
                    allNodesInMap.Add(farmhouseCabin = new Node(Area.Subarea.NameEnum.FARMHOUSECABIN));
                    allNodesInMap.Add(farmhouseHouse = new Node(Area.Subarea.NameEnum.FARMHOUSEHOUSE));
                    allNodesInMap.Add(farmhouseMansionLower = new Node(Area.Subarea.NameEnum.FARMHOUSEMANSIONLOWER));
                    allNodesInMap.Add(farmhouseMansionUpper = new Node(Area.Subarea.NameEnum.FARMHOUSEMANSIONUPPER));
                    allNodesInMap.Add(rockwellHouse = new Node(Area.Subarea.NameEnum.ROCKWELLHOUSE));
                    allNodesInMap.Add(beachHouse = new Node(Area.Subarea.NameEnum.BEACHHOUSE));
                    allNodesInMap.Add(piperLower = new Node(Area.Subarea.NameEnum.PIPERLOWER));
                    allNodesInMap.Add(piperUpper = new Node(Area.Subarea.NameEnum.PIPERUPPER));
                    allNodesInMap.Add(townhallLower = new Node(Area.Subarea.NameEnum.TOWNHALLLOWER));
                    allNodesInMap.Add(workshop = new Node(Area.Subarea.NameEnum.WORKSHOP));
                    allNodesInMap.Add(forge = new Node(Area.Subarea.NameEnum.FORGE));
                    allNodesInMap.Add(town = new Node(Area.Subarea.NameEnum.TOWN));
                    allNodesInMap.Add(s2 = new Node(Area.Subarea.NameEnum.S2));
                    allNodesInMap.Add(s3 = new Node(Area.Subarea.NameEnum.S3));
                    allNodesInMap.Add(s4 = new Node(Area.Subarea.NameEnum.S4));
                    allNodesInMap.Add(inn = new Node(Area.Subarea.NameEnum.INN));
                    allNodesInMap.Add(innBath = new Node(Area.Subarea.NameEnum.INNBATH));
                    allNodesInMap.Add(innTroy = new Node(Area.Subarea.NameEnum.INNTROY));
                    allNodesInMap.Add(innCharlotte = new Node(Area.Subarea.NameEnum.INNCHARLOTTE));
                    allNodesInMap.Add(innSpare1 = new Node(Area.Subarea.NameEnum.INNSPARE1));
                    allNodesInMap.Add(innSpare2 = new Node(Area.Subarea.NameEnum.INNSPARE2));
                    allNodesInMap.Add(innSpare3 = new Node(Area.Subarea.NameEnum.INNSPARE3));
                    allNodesInMap.Add(townhallUpper = new Node(Area.Subarea.NameEnum.TOWNHALLUPPER));

                    farm.AddNeighbor(s0walk, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.FARM).GetWaypoint("TRfarmRight"), MovementTypeWaypoint.MovementEnum.WALK));

                    farm.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.FARM).GetWaypoint("TRfarmLeft"), MovementTypeWaypoint.MovementEnum.WALK));
                    farm.AddNeighbor(farmhouseCabin, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.FARM).GetWaypoint("TRtoFarmhouseCabin"), MovementTypeWaypoint.MovementEnum.WALK));
                    farm.AddNeighbor(farmhouseHouse, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.FARM).GetWaypoint("TRtoFarmhouseHouse"), MovementTypeWaypoint.MovementEnum.WALK));
                    farm.AddNeighbor(farmhouseMansionLower, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.FARM).GetWaypoint("TRtoFarmhouseMansionLower"), MovementTypeWaypoint.MovementEnum.WALK));
                    farmhouseCabin.AddNeighbor(farm, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPfarmhouseCabin"), MovementTypeWaypoint.MovementEnum.WALK));
                    farmhouseHouse.AddNeighbor(farm, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPfarmhouseHouse"), MovementTypeWaypoint.MovementEnum.WALK));
                    farmhouseMansionLower.AddNeighbor(farm, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPfarmhouseMansion"), MovementTypeWaypoint.MovementEnum.WALK));
                    farmhouseMansionLower.AddNeighbor(farmhouseMansionUpper, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPfarmhouseMansionDownstairs"), MovementTypeWaypoint.MovementEnum.WALK));
                    farmhouseMansionUpper.AddNeighbor(farmhouseMansionLower, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPfarmhouseMansionUpstairs"), MovementTypeWaypoint.MovementEnum.WALK));

                    town.AddNeighbor(farm, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("TRtownRight"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(cafe, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPcafe"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(bookstoreUpper, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPbookstoreTop"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(bookstoreLower, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPbookstoreBottom"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(townhallLower, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPtownhall"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(store, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPstore"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(workshop, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPworkshop"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(forge, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPforge"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(piperLower, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPpipers"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(beach, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPbeach"), MovementTypeWaypoint.MovementEnum.WALK));
                    town.AddNeighbor(s1walk, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.TOWN).GetWaypoint("SPcablecar"), MovementTypeWaypoint.MovementEnum.WALK));

                    cafe.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPcafe"), MovementTypeWaypoint.MovementEnum.WALK));
                    bookstoreUpper.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPbookstoreTop"), MovementTypeWaypoint.MovementEnum.WALK));
                    bookstoreLower.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPbookstoreBottom"), MovementTypeWaypoint.MovementEnum.WALK));
                    townhallLower.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPtownhall"), MovementTypeWaypoint.MovementEnum.WALK));
                    townhallLower.AddNeighbor(townhallUpper, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPtownhallDownstairs"), MovementTypeWaypoint.MovementEnum.WALK));
                    townhallUpper.AddNeighbor(townhallLower, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPtownhallUpstairs"), MovementTypeWaypoint.MovementEnum.WALK));
                    store.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPstore"), MovementTypeWaypoint.MovementEnum.WALK));
                    workshop.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPworkshop"), MovementTypeWaypoint.MovementEnum.WALK));
                    forge.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPforgeTop"), MovementTypeWaypoint.MovementEnum.WALK));
                    piperLower.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPpipers"), MovementTypeWaypoint.MovementEnum.WALK));
                    piperLower.AddNeighbor(piperUpper, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPpipersDownstairs"), MovementTypeWaypoint.MovementEnum.WALK));
                    piperUpper.AddNeighbor(piperLower, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPpipersUpstairs"), MovementTypeWaypoint.MovementEnum.WALK));

                    beach.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.BEACH).GetWaypoint("SPbeachElevator"), MovementTypeWaypoint.MovementEnum.WALK));
                    beach.AddNeighbor(rockwellHouse, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.BEACH).GetWaypoint("SProckwell"), MovementTypeWaypoint.MovementEnum.WALK));
                    beach.AddNeighbor(beachHouse, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.BEACH).GetWaypoint("SPclaude"), MovementTypeWaypoint.MovementEnum.WALK));

                    rockwellHouse.AddNeighbor(beach, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SProckwell"), MovementTypeWaypoint.MovementEnum.WALK));
                    beachHouse.AddNeighbor(beach, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPclaude"), MovementTypeWaypoint.MovementEnum.WALK));

                    s0warp.AddNeighbor(s0walk, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S0).GetWaypoint("SPtop"), MovementTypeWaypoint.MovementEnum.WARP));
                    s0walk.AddNeighbor(farm, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S0).GetWaypoint("TRs1top"), MovementTypeWaypoint.MovementEnum.WALK));
                    s0walk.AddNeighbor(s0warp, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S0).GetWaypoint("SPtop"), MovementTypeWaypoint.MovementEnum.WARP));

                    s1walk.AddNeighbor(town, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S1).GetWaypoint("SPs1entrance"), MovementTypeWaypoint.MovementEnum.WALK));
                    s1walk.AddNeighbor(inn, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S1).GetWaypoint("SPinn"), MovementTypeWaypoint.MovementEnum.WALK));
                    s1walk.AddNeighbor(s1warp, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S1).GetWaypoint("SPs1entrance"), MovementTypeWaypoint.MovementEnum.WARP));

                    inn.AddNeighbor(s1walk, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinn"), MovementTypeWaypoint.MovementEnum.WALK));

                    inn.AddNeighbor(innBath, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnBathHall"), MovementTypeWaypoint.MovementEnum.WALK));
                    inn.AddNeighbor(innTroy, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnTroyHall"), MovementTypeWaypoint.MovementEnum.WALK));
                    inn.AddNeighbor(innCharlotte, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnCharlotteHall"), MovementTypeWaypoint.MovementEnum.WALK));
                    inn.AddNeighbor(innSpare1, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnSpare1Hall"), MovementTypeWaypoint.MovementEnum.WALK));
                    inn.AddNeighbor(innSpare2, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnSpare2Hall"), MovementTypeWaypoint.MovementEnum.WALK));
                    inn.AddNeighbor(innSpare3, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnSpare3Hall"), MovementTypeWaypoint.MovementEnum.WALK));

                    innBath.AddNeighbor(inn, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnBath"), MovementTypeWaypoint.MovementEnum.WALK));
                    innTroy.AddNeighbor(inn, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnTroy"), MovementTypeWaypoint.MovementEnum.WALK));
                    innCharlotte.AddNeighbor(inn, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnCharlotte"), MovementTypeWaypoint.MovementEnum.WALK));
                    innSpare1.AddNeighbor(inn, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnSpare1"), MovementTypeWaypoint.MovementEnum.WALK));
                    innSpare2.AddNeighbor(inn, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnSpare2"), MovementTypeWaypoint.MovementEnum.WALK));
                    innSpare3.AddNeighbor(inn, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.INTERIOR).GetWaypoint("SPinnSpare3"), MovementTypeWaypoint.MovementEnum.WALK));

                    s1warp.AddNeighbor(s1walk, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S1).GetWaypoint("SPs1entrance"), MovementTypeWaypoint.MovementEnum.WARP));
                    s1warp.AddNeighbor(s2, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S1).GetWaypoint("SPs1exit"), MovementTypeWaypoint.MovementEnum.WARP));

                    s2.AddNeighbor(s1warp, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S2).GetWaypoint("SPs2entrance"), MovementTypeWaypoint.MovementEnum.WARP));
                    s2.AddNeighbor(s3, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S2).GetWaypoint("SPs2exit"), MovementTypeWaypoint.MovementEnum.WARP));

                    s3.AddNeighbor(s2, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S3).GetWaypoint("SPs3entrance"), MovementTypeWaypoint.MovementEnum.WARP));
                    s3.AddNeighbor(s4, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S3).GetWaypoint("SPs3exit"), MovementTypeWaypoint.MovementEnum.WARP));

                    s4.AddNeighbor(s3, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S4).GetWaypoint("SPs4entrance"), MovementTypeWaypoint.MovementEnum.WARP));
                    s4.AddNeighbor(apex, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.S4).GetWaypoint("SPs4exit"), MovementTypeWaypoint.MovementEnum.WARP));

                    apex.AddNeighbor(s4, new MovementTypeWaypoint(world.GetAreaByEnum(Area.AreaEnum.APEX).GetWaypoint("SPapexEntrance"), MovementTypeWaypoint.MovementEnum.WARP));
                }

                private MovementTypeWaypoint.MovementEnum GetMovementTypeForSubzone(Area.Subarea.NameEnum subzone)
                {
                    switch (subzone)
                    {
                        case Area.Subarea.NameEnum.S0WARP:
                        case Area.Subarea.NameEnum.APEX:
                        case Area.Subarea.NameEnum.S1WARP:
                        case Area.Subarea.NameEnum.S2:
                        case Area.Subarea.NameEnum.S3:
                        case Area.Subarea.NameEnum.S4:
                            return MovementTypeWaypoint.MovementEnum.WARP;
                    }
                    return MovementTypeWaypoint.MovementEnum.WALK;
                }

                private Node GetNodeForSubzone(Area.Subarea.NameEnum subzone)
                {
                    foreach (Node node in allNodesInMap)
                    {
                        if (node.subzone == subzone)
                        {
                            return node;
                        }
                    }
                    throw new Exception("Error in GetNodeForSubzone");
                }

                public Queue<MovementTypeWaypoint> FindPath(Area.Subarea.NameEnum source, Area.Waypoint sourceWaypoint, Area.Subarea.NameEnum destinationSubzone, Area.Waypoint destinationWaypoint)
                {
                    List<Node.ConnectedNode> startingPath = new List<Node.ConnectedNode>();
                    startingPath.Add(new Node.ConnectedNode(GetNodeForSubzone(source), new MovementTypeWaypoint(sourceWaypoint, MovementTypeWaypoint.MovementEnum.WALK)));

                    Queue<MovementTypeWaypoint> path = BFS(startingPath, destinationSubzone);
                    path.Enqueue(new MovementTypeWaypoint(destinationWaypoint, GetMovementTypeForSubzone(destinationSubzone))); //add the ending node
                    path.Dequeue(); //trim off the start?
                    return path;
                }

                private bool DoesCurrentPathIncludeSubzone(List<Node.ConnectedNode> path, Area.Subarea.NameEnum subzone)
                {
                    foreach (Node.ConnectedNode node in path)
                    {
                        if (node.nodeTo.subzone == subzone)
                        {
                            return true;
                        }
                    }
                    return false;
                }

                private Queue<MovementTypeWaypoint> BFS(List<Node.ConnectedNode> currentPath, Area.Subarea.NameEnum dest)
                {
                    Node.ConnectedNode currentNode = currentPath[currentPath.Count - 1];
                    if (currentNode.nodeTo.subzone == dest)
                    {
                        //System.Diagnostics.Debug.WriteLine("SUCCESSPATH");
                        //foreach (Node.ConnectedNode cn in currentPath)
                        //{
                        //    System.Diagnostics.Debug.WriteLine(cn.nodeTo.subzone + "  ");
                        //}
                        //SUCCESS - found path
                        Queue<MovementTypeWaypoint> returnQueue = new Queue<MovementTypeWaypoint>();
                        foreach (Node.ConnectedNode node in currentPath)
                        {
                            returnQueue.Enqueue(node.waypoint);
                        }
                        return returnQueue;
                    }
                    else
                    {
                        foreach (Node.ConnectedNode neighbor in currentNode.nodeTo.neighbors)
                        {
                            if (!DoesCurrentPathIncludeSubzone(currentPath, neighbor.nodeTo.subzone))
                            {
                                List<Node.ConnectedNode> currentPathCopy = new List<Node.ConnectedNode>();
                                foreach (Node.ConnectedNode cn in currentPath)
                                {
                                    currentPathCopy.Add(cn);
                                }
                                currentPathCopy.Add(neighbor);
                                Queue<MovementTypeWaypoint> result = BFS(currentPathCopy, dest);
                                if (result != null)
                                {
                                    return result;
                                }
                            }
                        }
                    }
                    /*Console.WriteLine("FAILEDPATH");
                    foreach(Node.ConnectedNode cn in currentPath)
                    {
                        Console.WriteLine(cn.nodeTo.subzone + "  ");
                    }*/
                    return null; //dead end case
                }
            }

            public class MovementTypeWaypoint
            {
                public enum MovementEnum
                {
                    WALK, WARP
                }

                public MovementEnum movementType;
                public Area.Waypoint waypoint;

                public MovementTypeWaypoint(Area.Waypoint waypoint, MovementEnum movementType)
                {
                    this.movementType = movementType;
                    this.waypoint = waypoint;
                }
            }

            public abstract class Event
            {
                private Area area;
                private Area.Waypoint waypoint;
                private int startHour, startMinute, endHour, endMinute;
                private Func<World, EntityCharacter, bool> conditionFunction;
                private int priority;

                public Event(Area area, Area.Waypoint waypoint, int startHour, int startMinute, int endHour, int endMinute, Func<World, EntityCharacter, bool> conditionFunction, int priority)
                {
                    this.area = area;
                    this.waypoint = waypoint;
                    this.startHour = startHour;
                    this.startMinute = startMinute;
                    this.endHour = endHour;
                    this.endMinute = endMinute;
                    this.conditionFunction = conditionFunction;
                    this.priority = priority;
                }

                public Area GetArea()
                {
                    return area;
                }

                public Area.Waypoint GetWaypoint()
                {
                    return waypoint;
                }

                public int GetPriority()
                {
                    return this.priority;
                }

                public bool CheckActivation(World world, EntityCharacter character)
                {
                    int currHour = world.GetHour();
                    int currMin = world.GetMinute();
                    if (currHour < startHour || currHour > endHour)
                    {
                        return false;
                    }
                    if (currHour == startHour && currMin < startMinute)
                    {
                        return false;
                    }
                    if (currHour == endHour && currMin > endMinute)
                    {
                        return false;
                    }
                    return conditionFunction(world, character);
                }

                public abstract void Update(float deltaTime, EntityCharacter character, Area area, Queue<MovementTypeWaypoint> waypoints);
            }

            public class StandAtEvent : Event
            {
                public enum DirectionBehavior
                {
                    LEFT, RIGHT, RANDOM
                }

                private DirectionBehavior directionBehavior;
                private static int CHANCE_TO_RANDOMIZE_DIRECTION = 100;
                private static float MIN_TIME_BETWEEN_CHANGE = 1.0f;
                private float timeSinceLastChange;

                public StandAtEvent(Area area, Area.Waypoint waypoint, int startHour, int startMinute, int endHour, int endMinute, Func<World, EntityCharacter, bool> conditionFunction, int priority, DirectionBehavior directionBehavior) : base(area, waypoint, startHour, startMinute, endHour, endMinute, conditionFunction, priority)
                {
                    this.directionBehavior = directionBehavior;
                    this.timeSinceLastChange = 0;
                }

                public override void Update(float deltaTime, EntityCharacter character, Area area, Queue<MovementTypeWaypoint> waypoints)
                {
                    if (directionBehavior == DirectionBehavior.LEFT)
                    {
                        character.direction = DirectionEnum.LEFT;
                    }
                    else if (directionBehavior == DirectionBehavior.RIGHT)
                    {
                        character.direction = DirectionEnum.RIGHT;
                    }
                    else
                    {
                        timeSinceLastChange += deltaTime;

                        if (timeSinceLastChange > MIN_TIME_BETWEEN_CHANGE && Util.RandInt(0, CHANCE_TO_RANDOMIZE_DIRECTION) == 0)
                        {
                            timeSinceLastChange = 0;
                            switch (Util.RandInt(0, 1))
                            {
                                case 0:
                                    character.direction = DirectionEnum.RIGHT;
                                    break;
                                case 1:
                                default:
                                    character.direction = DirectionEnum.LEFT;
                                    break;
                            }
                        }
                    }
                }
            }

            public class WanderNearEvent : Event
            {
                public enum WanderRange
                {
                    VERY_SMALL, SMALL, MEDIUM, LARGE, INFINITE
                }

                private static int VERY_SMALL_RANGE = 24;
                private static int SMALL_RANGE = 60;
                private static int MEDIUM_RANGE = 120;
                private static int LARGE_RANGE = 320;
                private static int INFINITE_RANGE = 99999;

                private static int CHANCE_TO_START_WANDERING = 100; //odds to start wandering each frame
                private static int CHANCE_TO_STOP_WANDERING = 100; //odds to stop wandering each frame
                private static float MIN_TIME_BETWEEN_CHANGE = 1.0f;

                private float timeSinceChange;
                private bool wandering;
                private RectangleF wanderBox;

                private int rangeForEnum(WanderRange range)
                {
                    switch (range)
                    {
                        case WanderRange.VERY_SMALL:
                            return VERY_SMALL_RANGE;
                        case WanderRange.SMALL:
                            return SMALL_RANGE;
                        case WanderRange.MEDIUM:
                            return MEDIUM_RANGE;
                        case WanderRange.LARGE:
                            return LARGE_RANGE;
                        default:
                            return INFINITE_RANGE;
                    }
                }

                public WanderNearEvent(Area area, Area.Waypoint waypoint, int startHour, int startMinute, int endHour, int endMinute, Func<World, EntityCharacter, bool> conditionFunction, int priority, WanderRange range) : base(area, waypoint, startHour, startMinute, endHour, endMinute, conditionFunction, priority)
                {
                    this.wandering = false;
                    int rangeSize = rangeForEnum(range);
                    wanderBox = new RectangleF(GetWaypoint().position.X - (rangeSize / 2), GetWaypoint().position.Y - 5000, rangeSize, 10000);
                    this.timeSinceChange = MIN_TIME_BETWEEN_CHANGE + 1;
                }

                public override void Update(float deltaTime, EntityCharacter character, Area area, Queue<MovementTypeWaypoint> waypoints)
                {
                    if (wanderBox.Width != rangeForEnum(WanderRange.INFINITE))
                        Util.DrawDebugRect(wanderBox, Color.Green * 0.2f);
                    timeSinceChange += deltaTime;

                    //chance to wander left or right
                    if (!wandering && Util.RandInt(0, CHANCE_TO_START_WANDERING) == 0 && timeSinceChange > MIN_TIME_BETWEEN_CHANGE)
                    {
                        wandering = true;
                        timeSinceChange = 0;

                        //if within valid wandering parameters, randomly choose a direction
                        if (character.GetCollisionHitbox().Intersects(wanderBox))
                        {
                            if (Util.RandInt(0, 10) == 0)
                                character.direction = DirectionEnum.LEFT;
                            else
                                character.direction = DirectionEnum.RIGHT;
                        }
                        else //otherwise, must go back towards
                        {
                            if (wanderBox.Center.X > character.GetCollisionHitbox().Center.X)
                                character.direction = DirectionEnum.RIGHT;
                            else
                                character.direction = DirectionEnum.LEFT;
                        }
                    }
                    else if (wandering && Util.RandInt(0, CHANCE_TO_STOP_WANDERING) == 0 && character.grounded && timeSinceChange > MIN_TIME_BETWEEN_CHANGE)  //chance stop wandering
                    {
                        wandering = false;
                        timeSinceChange = 0;
                    }

                    //if wandering away and outside of range, stop movement
                    if (wandering && !wanderBox.Contains(character.GetCollisionHitbox().Center))
                    {
                        if (character.direction == DirectionEnum.LEFT && wanderBox.Center.X > character.GetCollisionHitbox().Center.X ||
                            character.direction == DirectionEnum.RIGHT && wanderBox.Center.X < character.GetCollisionHitbox().Center.X)
                        {
                            wandering = false;
                            timeSinceChange = 0;
                        }
                    }

                    //if still wandering at this point, start walking in direction
                    if (wandering)
                        character.Walk(character.direction, deltaTime);
                    else
                        character.velocityX = 0;
                }
            }

            private static SubzoneMap subzoneMap;
            private Queue<MovementTypeWaypoint> waypoints;
            private List<Event> events;
            private Event currentEvent;

            public Schedule(List<Event> events, World world)
            {
                waypoints = new Queue<MovementTypeWaypoint>();
                this.events = events;
                currentEvent = null;
                if (subzoneMap == null)
                {
                    subzoneMap = new SubzoneMap(world);
                }
            }

            public void Update(float deltaTime, Area area, EntityCharacter character, World world)
            {
                //if talking, don't move or do anything else
                if (character.isTalking)
                    return;
                else
                    character.timeSinceTalking += deltaTime;

                if (character.timeSinceTalking < TIME_AFTER_TALK_BEFORE_MOVEMENT)
                {
                    character.FaceTowardsPlayer(character.talkingPlayer);
                    return;
                }

                if (character.fadeState == FadeState.FADE_OUT) //handle fade out before transitioning to new area
                {
                    character.velocityX = 0;
                    character.velocityY = 0;
                    character.opacity -= FADE_SPEED * deltaTime;
                    if (character.opacity < 0)
                    {
                        if (waypoints.Count != 0)
                        {
                            Area.TransitionZone transitionZone = area.CheckTransition(character.GetCollisionHitbox().Center, true);
                            if (transitionZone != null)
                            {
                                Area transitionTo = world.GetAreaByName(transitionZone.to);
                                world.MoveCharacter(character, area, transitionTo);
                                character.SetPosition(transitionTo.GetWaypoint(transitionZone.spawn).position - new Vector2(0, 32f));
                            }
                            else
                            {
                                throw new Exception("Failed to transition!");
                            }
                        }
                        character.fadeState = FadeState.FADE_IN;
                    }
                }
                else if (character.fadeState == FadeState.FADE_IN) //handle fade in when arriving at new area
                {
                    character.velocityX = 0;
                    character.velocityY = 0;
                    character.opacity += FADE_SPEED * deltaTime;
                    if (character.opacity > 1)
                    {
                        character.opacity = 1;
                        character.fadeState = FadeState.NONE;
                    }
                }
                else if (waypoints.Count == 0) //if at destination, let event handle character
                {
                    if (currentEvent != null)
                    {
                        currentEvent.Update(deltaTime, character, area, waypoints);
                    }
                }
                else //otherwise, move towards destination
                {
                    if (waypoints.Peek().movementType == MovementTypeWaypoint.MovementEnum.WALK) //WALK
                    {
                        if (waypoints.Peek().waypoint.position.X > character.GetCollisionHitbox().Center.X)
                        {
                            character.Walk(DirectionEnum.RIGHT, deltaTime);
                        }
                        else if (waypoints.Peek().waypoint.position.X < character.GetCollisionHitbox().Center.X)
                        {
                            character.Walk(DirectionEnum.LEFT, deltaTime);
                        }

                        if (area.IsCollideWithPathingHelperType(character.GetCollisionHitbox(), Area.PathingHelper.Type.BEACHHELPER) &&
                            waypoints.Peek().waypoint.position.Y < character.GetCollisionHitbox().Bottom - 5)
                        {
                            //force character to walk RIGHT when leaving from beach elevator, when heading to cable car
                            character.Walk(DirectionEnum.RIGHT, deltaTime);
                        }
                        else if ((waypoints.Peek().waypoint.position.Y < character.GetCollisionHitbox().Bottom - 5 &&
                            area.IsCollideWithPathingHelperType(character.GetCollisionHitbox(), Area.PathingHelper.Type.CONDITIONALJUMP)))
                        {
                            //indicates that a character should jump, if their destination is above them
                            character.TryJump();
                        }
                    }
                    else if (waypoints.Peek().movementType == MovementTypeWaypoint.MovementEnum.WARP) //WARP
                    {
                        world.MoveCharacter(character, area, waypoints.Peek().waypoint.area);
                        character.SetPosition(waypoints.Peek().waypoint.position - new Vector2(0, 32f));
                    }

                    RectangleF destinationCheck = character.GetCollisionHitbox();
                    destinationCheck.Height += 6;
                    destinationCheck.X += 2;
                    destinationCheck.Width -= 4;
                    if (destinationCheck.Contains(waypoints.Peek().waypoint.position))
                    {
                        waypoints.Dequeue();
                        if (waypoints.Count > 0 && area.CheckTransition(character.GetCollisionHitbox().Center, true) != null)
                        {
                            character.fadeState = FadeState.FADE_OUT;
                        }
                        if (waypoints.Count == 0) //arrived, stop moving
                        {
                            character.velocityX = 0;
                            character.velocityY = 0;
                        }
                    }
                }
            }

            public void Update(World world, EntityCharacter character, Area currentArea)
            {
                if (currentEvent != null && !currentEvent.CheckActivation(world, character))
                {
                    currentEvent = null;
                }
                foreach (Schedule.Event scEvent in events)
                {
                    if (scEvent.CheckActivation(world, character) && (currentEvent == null || scEvent.GetPriority() > currentEvent.GetPriority()))
                    {
                        //System.Diagnostics.Debug.WriteLine(character.name + " is doing event for waypoint " + scEvent.GetWaypoint().name);
                        //System.Diagnostics.Debug.Write("EntityCharacter.cs: " + character.name + " is going to " + scEvent.GetArea().GetName());

                        waypoints.Clear();
                        currentEvent = scEvent;

                        waypoints = subzoneMap.FindPath(currentArea.GetSubareaAt(character.GetCollisionHitbox()),
                            new Area.Waypoint(character.GetCollisionHitbox().Center, "CHAR", currentArea),
                            scEvent.GetArea().GetSubareaAt(new RectangleF(scEvent.GetWaypoint().position, new Size2(2, 2))),
                            scEvent.GetWaypoint());

                    }
                }
            }
        }

        public class DialogueOption
        {
            private Func<World, EntityCharacter, bool>[] conditionFunctions;
            private DialogueNode root;
            private int weight;

            public DialogueOption(DialogueNode root, Func<World, EntityCharacter, bool>[] conditions, int weight = 1)
            {
                this.root = root;
                this.conditionFunctions = conditions;
                this.weight = weight;
            }

            public int GetWeight()
            {
                return weight;
            }

            public DialogueNode GetDialogue()
            {
                return root;
            }

            public bool CanActivate(World world, EntityCharacter character)
            {
                foreach (Func<World, EntityCharacter, bool> conditionFunc in conditionFunctions)
                {
                    if (!conditionFunc(world, character))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public class DialogueManager
        {
            private List<DialogueOption> dialogueOptions;

            public DialogueManager(List<DialogueOption> options)
            {
                this.dialogueOptions = options;
            }

            public DialogueNode GetDialogue(World world, EntityCharacter character)
            {
                List<DialogueNode> choices = new List<DialogueNode>();

                foreach (DialogueOption option in dialogueOptions)
                {
                    if (option.CanActivate(world, character))
                    {
                        for (int i = 0; i < option.GetWeight(); i++)
                        {
                            choices.Add(option.GetDialogue());
                        }
                    }
                }

                return choices[Util.RandInt(0, choices.Count - 1)];
            }

        }

        private ClothedSprite sprite;
        private ClothingManager clothingManager;
        private string name;
        private Schedule schedule;
        private DialogueManager dialogueManager;
        private CharacterEnum characterEnum;
        private int heartPoints;
        private float velocityX, velocityY;
        private bool grounded;
        private DirectionEnum direction;
        private Area currentArea;
        private World world;
        private Area.Waypoint spawn;
        private EntityPlayer talkingPlayer;

        private bool isTalking;
        private float timeSinceTalking;
        private static float TIME_AFTER_TALK_BEFORE_MOVEMENT = 3.0f;

        private static string EMOTION_NONE = "emo_none";
        private static string EMOTION_LOVE = "emo_love";
        private static string EMOTION_JOY = "emo_joy";
        private static string EMOTION_HAPPY = "emo_happy";
        private static string EMOTION_NEUTRAL = "emo_neutral";
        private static string EMOTION_SAD = "emo_sad";
        private static string EMOTION_HEARTBREAK = "emo_hbreak";
        private static string EMOTION_TALK_L = "emo_talkL";
        private static string EMOTION_TALK_R = "emo_talkR";
        private static string EMOTION_MUSIC = "emo_music";
        private static string EMOTION_SLEEP = "emo_sleep";
        private static string EMOTION_EAT = "emo_eat";
        private static string EMOTION_FISH = "emo_fish";
        private static string EMOTION_SCHOOL = "emo_school";
        private static string EMOTION_GAME = "emo_game";
        private static string EMOTION_CARDS = "emo_cards";
        private static string EMOTION_FORGE = "emo_forge";
        private static string EMOTION_COOK = "emo_cook";
        private static string EMOTION_JOG = "emo_jog";
        private static string EMOTION_BOOK = "emo_book";
        private static string EMOTION_PRAY = "emo_pray";
        private static string EMOTION_PAPERWORK = "emo_papwork";
        private static string EMOTION_PAINT = "emo_paint";

        private static int[] HEART_LEVEL_BREAKPOINTS = { 0, 100, 250, 450, 700, 1000, 1350, 1750, 2200, 2700, 3500 };

        private string giftFlag;

        public EntityCharacter(String name, World world, CharacterEnum characterEnum, List<ClothingSet> clothingSets, List<Schedule.Event> scheduleEvents, List<DialogueOption> dialogueOptions, Texture2D emotionPanelTex, Area.Waypoint spawn,
            string giftFlag) : base(new Vector2(0, 0), DrawLayer.PRIORITY)
        {
            this.name = name;
            sprite = new ClothedSprite(true);
            this.world = world;
            this.clothingManager = new ClothingManager(clothingSets);
            this.schedule = new Schedule(scheduleEvents, world);
            this.dialogueManager = new DialogueManager(dialogueOptions);
            this.direction = DirectionEnum.LEFT;
            this.characterEnum = characterEnum;
            this.heartPoints = 0;
            this.velocityX = 0;
            this.velocityY = 0;
            this.grounded = false;
            this.fadeState = FadeState.NONE;
            this.opacity = 1.0f;
            this.giftFlag = giftFlag;
            this.isTalking = false;
            this.timeSinceTalking = TIME_AFTER_TALK_BEFORE_MOVEMENT + 1;

            this.emotionPanel = new AnimatedSprite(emotionPanelTex, 23, 1, 23, Util.CreateAndFillArray(23, 0.1f));
            emotionPanel.AddLoop(EMOTION_NONE, 0, 0, true, false);
            emotionPanel.AddLoop(EMOTION_LOVE, 1, 1, true, false);
            emotionPanel.AddLoop(EMOTION_JOY, 2, 2, true, false);
            emotionPanel.AddLoop(EMOTION_HAPPY, 3, 3, true, false);
            emotionPanel.AddLoop(EMOTION_NEUTRAL, 4, 4, true, false);
            emotionPanel.AddLoop(EMOTION_SAD, 5, 5, true, false);
            emotionPanel.AddLoop(EMOTION_HEARTBREAK, 6, 6, true, false);
            emotionPanel.AddLoop(EMOTION_TALK_L, 7, 7, true, false);
            emotionPanel.AddLoop(EMOTION_TALK_R, 8, 8, true, false);
            emotionPanel.AddLoop(EMOTION_MUSIC, 9, 9, true, false);
            emotionPanel.AddLoop(EMOTION_SLEEP, 10, 10, true, false);
            emotionPanel.AddLoop(EMOTION_EAT, 11, 11, true, false);
            emotionPanel.AddLoop(EMOTION_FISH, 12, 12, true, false);
            emotionPanel.AddLoop(EMOTION_SCHOOL, 13, 13, true, false);
            emotionPanel.AddLoop(EMOTION_GAME, 14, 14, true, false);
            emotionPanel.AddLoop(EMOTION_CARDS, 15, 15, true, false);
            emotionPanel.AddLoop(EMOTION_FORGE, 16, 16, true, false);
            emotionPanel.AddLoop(EMOTION_COOK, 17, 17, true, false);
            emotionPanel.AddLoop(EMOTION_JOG, 18, 18, true, false);
            emotionPanel.AddLoop(EMOTION_BOOK, 19, 19, true, false);
            emotionPanel.AddLoop(EMOTION_PRAY, 20, 20, true, false);
            emotionPanel.AddLoop(EMOTION_PAPERWORK, 21, 21, true, false);
            emotionPanel.AddLoop(EMOTION_PAINT, 22, 22, true, false);
            emotionPanel.SetLoop(EMOTION_NONE);

            this.spawn = spawn;
            world.GetAreaByEnum(spawn.area.GetAreaEnum()).AddEntity(this);
            currentArea = world.GetAreaByEnum(spawn.area.GetAreaEnum());
            MoveToSpawn(world);
        }

        public string GetName()
        {
            return this.name;
        }

        private void MoveToSpawn(World world)
        {
            world.MoveCharacter(this, currentArea, spawn.area);
            SetPosition(spawn.position - new Vector2(0, 32f));
            velocityX = 0;
            velocityY = 0;
        }

        public void RefreshClothing(World world)
        {
            clothingManager.ChooseClothingSet(world, this);
        }

        public Area GetCurrentArea()
        {
            return this.currentArea;
        }

        public void SetCurrentArea(Area newArea)
        {
            this.currentArea = newArea;
        }

        public int GetHeartLevel()
        {
            int level = 0;
            while (heartPoints > HEART_LEVEL_BREAKPOINTS[level + 1] && level < 10)
            {
                level++;
            }
            return level;
        }

        public string GetGiftFlag()
        {
            return this.giftFlag;
        }

        public void Walk(DirectionEnum direction, float deltaTime)
        {
            switch (direction)
            {
                case DirectionEnum.LEFT:
                    this.direction = DirectionEnum.LEFT;
                    velocityX = (grounded ? -SPEED : -SPEED_WHILE_JUMPING) * deltaTime;
                    break;
                case DirectionEnum.RIGHT:
                    this.direction = DirectionEnum.RIGHT;
                    velocityX = (grounded ? SPEED : SPEED_WHILE_JUMPING) * deltaTime;
                    break;
            }
        }

        public void TryJump()
        {
            if (grounded)
            {
                velocityY = JUMP_SPEED;
            }
        }

        public void GainHeartPoints(int amount)
        {
            heartPoints += amount;
            if (heartPoints < 0)
            {
                heartPoints = 0;
            }
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            Vector2 modifiedPosition = new Vector2(position.X, position.Y);
            if (direction == DirectionEnum.LEFT)
            {
                modifiedPosition.X++;
            }
            sprite.Draw(sb, modifiedPosition, layerDepth, SpriteEffects.None, 1.0f, opacity);

            Vector2 emotionPanelPos = modifiedPosition + new Vector2(26.5f, -6);
            emotionPanel.Draw(sb, emotionPanelPos, Color.White, layerDepth);
        }

        //used for collision calculations internally. Not as wide as GetCollisionRectangle, which is wider to allow player to interact from a range 
        private RectangleF GetCollisionHitbox()
        {
            //Util.DrawDebugRect(new RectangleF((position.X + OFFSET_X), position.Y + OFFSET_Y + 1, WIDTH, HEIGHT - 1), Color.Red * 0.1f);
            return new RectangleF((position.X + OFFSET_X), position.Y + OFFSET_Y + 1, WIDTH, HEIGHT - 1);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF((position.X + OFFSET_X) - (WIDTH / 2), position.Y + OFFSET_Y + 1, WIDTH * 2, HEIGHT - 1);
        }

        public override void Update(float deltaTime, Area area)
        {
            if (isTalking && talkingPlayer.GetCurrentDialogue() == null)
            {
                isTalking = false;
                timeSinceTalking = 0;
            }

            schedule.Update(world, this, area);
            clothingManager.Update(deltaTime, sprite);
            schedule.Update(deltaTime, area, this, world);

            velocityY += GRAVITY * deltaTime;

            //calculate collisions
            RectangleF collisionBox = GetCollisionHitbox();
            float stepX = velocityX / COLLISION_STEPS;
            for (int step = 0; step < COLLISION_STEPS; step++)
            {
                if (stepX != 0) //move X
                {
                    collisionBox = GetCollisionHitbox();
                    RectangleF stepXCollisionBox = new RectangleF(collisionBox.X + stepX, collisionBox.Y, collisionBox.Width, collisionBox.Height);
                    bool xCollision = CollisionHelper.CheckCollision(stepXCollisionBox, area, false);
                    RectangleF stepXCollisionBoxForesight = new RectangleF(collisionBox.X + (stepX * 15), collisionBox.Y, collisionBox.Width, collisionBox.Height);
                    bool xCollisionSoon = CollisionHelper.CheckCollision(stepXCollisionBoxForesight, area, false);
                    RectangleF foresightPlusHeight = new RectangleF(stepXCollisionBoxForesight.X, stepXCollisionBoxForesight.Y - 6, 1, 1);
                    if (direction == DirectionEnum.RIGHT)
                        foresightPlusHeight.X += stepXCollisionBoxForesight.Width - 1;
                    bool jumpWouldHelp = !CollisionHelper.CheckCollision(foresightPlusHeight, area, false);

                    bool solidFound = false;
                    Vector2 tenativePos = new Vector2(collisionBox.X + stepX, collisionBox.Y);
                    if (direction == DirectionEnum.LEFT)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Area.CollisionTypeEnum collType = area.GetCollisionTypeAt((int)(tenativePos.X) / 8, ((int)(tenativePos.Y + collisionBox.Height) / 8) + i);
                            if (collType == Area.CollisionTypeEnum.SOLID ||
                                collType == Area.CollisionTypeEnum.BRIDGE ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BRIDGE)
                            {
                                //make sure ground above target isn't water or deep water (topwater is allowed)
                                Area.CollisionTypeEnum collTypeAbove = area.GetCollisionTypeAt((int)(tenativePos.X) / 8, ((int)(tenativePos.Y + collisionBox.Height) / 8) + (i - 1));
                                if (collTypeAbove != Area.CollisionTypeEnum.WATER &&
                                    collTypeAbove != Area.CollisionTypeEnum.DEEP_WATER)
                                {
                                    solidFound = true;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Area.CollisionTypeEnum collType = area.GetCollisionTypeAt((int)(tenativePos.X + collisionBox.Width) / 8, ((int)(tenativePos.Y + collisionBox.Height) / 8) + i);
                            if (collType == Area.CollisionTypeEnum.SOLID ||
                                collType == Area.CollisionTypeEnum.BRIDGE ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BRIDGE)
                            {
                                //make sure ground above target isn't water or deep water (topwater is allowed)
                                Area.CollisionTypeEnum collTypeAbove = area.GetCollisionTypeAt((int)(tenativePos.X + collisionBox.Width) / 8, ((int)(tenativePos.Y + collisionBox.Height) / 8) + (i - 1));
                                if (collTypeAbove != Area.CollisionTypeEnum.WATER &&
                                    collTypeAbove != Area.CollisionTypeEnum.DEEP_WATER)
                                {
                                    solidFound = true;
                                }
                                break;
                            }
                        }
                    }

                    //Util.DrawDebugRect(stepXCollisionBox, Color.Red * 0.2f);
                    //Util.DrawDebugRect(stepXCollisionBoxForesight, Color.Green * 0.2f);
                    //Util.DrawDebugRect(foresightPlusHeight, Color.Blue * 0.5f);

                    if (xCollisionSoon && jumpWouldHelp)
                    {
                        TryJump();
                    }

                    if (xCollision || !solidFound) //if next movement = collision
                    {
                        if (jumpWouldHelp && xCollision)
                            TryJump();
                        stepX = 0; //stop moving if collision
                        if (grounded)
                        {
                            velocityX = 0;
                        }
                    }
                    else
                    {
                        this.position.X += stepX;
                    }
                }
            }


            float stepY = velocityY / COLLISION_STEPS;
            for (int step = 0; step < COLLISION_STEPS; step++)
            {
                if (stepY != 0) //move Y
                {
                    collisionBox = GetCollisionHitbox();
                    RectangleF stepYCollisionBox = new RectangleF(collisionBox.X, collisionBox.Y + stepY, collisionBox.Width, collisionBox.Height);
                    bool yCollision = CollisionHelper.CheckCollision(stepYCollisionBox, area, stepY > 0);

                    if (yCollision)
                    {
                        if (velocityY > 0)
                        {
                            grounded = true;
                        }
                        stepY = 0;
                        velocityY = 0;

                    }
                    else
                    {
                        this.position.Y += stepY;
                        grounded = false;
                    }
                }
            }

            UpdateAnimation(deltaTime);
        }



        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            clothingManager.TickDaily(world, this); //update clothes
            MoveToSpawn(world); //reset location
            fadeState = FadeState.NONE; //required in the case that the day ends during a transition
            opacity = 1.0f;
        }

        public override void SetPosition(Vector2 position)
        {
            this.position = new Vector2(position.X - OFFSET_X, position.Y);
        }

        public void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {

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
            if (!player.GetHeldItem().GetItem().HasTag(Item.Tag.NO_TRASH))
            {
                return "Give";
            }
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (!IsJumping() && fadeState == FadeState.NONE)
                return "Talk";
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {

            if (!IsJumping() && fadeState == FadeState.NONE)
            {
                velocityX = 0;
                velocityY = 0;
                player.SetCurrentDialogue(dialogueManager.GetDialogue(world, this));
                this.talkingPlayer = player;
                isTalking = true;
                FaceTowardsPlayer(talkingPlayer);
            }
        }

        public void FaceTowardsPlayer(EntityPlayer player)
        {
            if (player.GetPosition().X > position.X)
                direction = DirectionEnum.RIGHT;
            else
                direction = DirectionEnum.LEFT;
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //gift item
            //Wish for Love AppliedEffect (from Wishboat) - should give more points
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //ask for something
        }

        public CharacterEnum GetCharacterEnum()
        {
            return this.characterEnum;
        }

        public DirectionEnum GetDirection()
        {
            return this.direction;
        }

        public SaveState GenerateSave()
        {
            SaveState state = new SaveState(SaveState.Identifier.CHARACTER);

            state.AddData("character", characterEnum.ToString());
            state.AddData("heartpoints", heartPoints.ToString());

            return state;
        }

        public void LoadSave(SaveState state)
        {
            heartPoints = Int32.Parse(state.TryGetData("heartpoints", "0"));
        }

        public bool IsJumping()
        {
            return velocityY < 0;
        }

        private void UpdateAnimation(float deltaTime)
        {
            /*else if (swimming)
            {
                if (inputVelocityX != 0)
                {
                    sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.WALK_CYCLE_L : ClothedSprite.WALK_CYCLE_R);
                }
                else
                {
                    sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.JUMP_ANIM_L : ClothedSprite.JUMP_ANIM_R);
                }
            }*/
            if (IsJumping())
            {
                sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.JUMP_ANIM_L : ClothedSprite.JUMP_ANIM_R);
            }
            else if (grounded && velocityX != 0)
            {
                sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.WALK_CYCLE_L : ClothedSprite.WALK_CYCLE_R);
            }
            else if (!grounded)
            {
                sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.FALLING_ANIM_L : ClothedSprite.FALLING_ANIM_R);
            }
            else
            {
                if (sprite.IsCurrentLoop(ClothedSprite.FALLING_ANIM_L) || sprite.IsCurrentLoop(ClothedSprite.FALLING_ANIM_R) || sprite.IsCurrentLoop(ClothedSprite.ROLLING_CYCLE_L) ||
                    sprite.IsCurrentLoop(ClothedSprite.ROLLING_CYCLE_R) || sprite.IsCurrentLoop(ClothedSprite.LANDING_ANIM_R) || sprite.IsCurrentLoop(ClothedSprite.LANDING_ANIM_L))
                {
                    if (!sprite.IsCurrentLoop(ClothedSprite.IDLE_CYCLE_L) && !sprite.IsCurrentLoop(ClothedSprite.LANDING_ANIM_L) && !sprite.IsCurrentLoop(ClothedSprite.IDLE_CYCLE_R) && !sprite.IsCurrentLoop(ClothedSprite.LANDING_ANIM_R))
                    {
                        sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.LANDING_ANIM_L : ClothedSprite.LANDING_ANIM_R);
                    }
                    else if (sprite.IsCurrentLoopFinished())
                    {
                        sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.IDLE_CYCLE_L : ClothedSprite.IDLE_CYCLE_R);
                    }
                }
                else
                {
                    sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? ClothedSprite.IDLE_CYCLE_L : ClothedSprite.IDLE_CYCLE_R);
                }
            }
        }

        protected override void OnXCollision()
        {
            velocityX = 0;
        }

        protected override void OnYCollision()
        {
            velocityY = 0;
        }

        public bool ShouldBeSaved()
        {
            return true;
        }
    }
}