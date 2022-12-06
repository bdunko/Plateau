using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Plateau.Particles;

namespace Plateau.Items
{
    public class UsableItem : Item
    {
        private DialogueNode onUse;
        private string mouseText;

        private static int ELEMENT_PARTICLES_X_RADIUS = 30;
        private static int ELEMENT_PARTICLES_Y_RADIUS = 15;
        private static int PRIMEVIL_X_RADIUS = 14;
        private static int PRIMEVIL_Y_RADIUS = 5;
        private static int BURST_STONE_X_RADIUS = 20;
        private static int BURST_STONE_Y_RADIUS = 12;
        private static int UNSTABLE_LIQUID_X_RADIUS = 24;
        private static int UNSTABLE_LIQUID_Y_RADIUS = 12;
        private static int BOTTLE_X_RADIUS = 20;
        private static int BOTTLE_Y_RAIDUS = 10;
        private static float INVINCIROID_TIME = 300.0f;
        public static DialogueNode BURST_STONE_DIALOGUE;
        public static DialogueNode UNSTABLE_LIQUID_DIALOGUE;
        public static DialogueNode MILK_CREAM_DIALOGUE;
        public static DialogueNode CREAM_DIALOGUE;
        public static DialogueNode INVINCIROID_DIALOGUE;
        public static DialogueNode MOSS_BOTTLE_DIALOGUE;
        public static DialogueNode TROPICAL_BOTTLE_DIALOGUE;
        public static DialogueNode SKY_BOTTLE_DIALOGUE;
        public static DialogueNode BLACK_CANDLE_DIALOGUE, SPICED_CANDLE_DIALOGUE, SALTED_CANDLE_DIALOGUE, SOOTHE_CANDLE_DIALOGUE, SUGAR_CANDLE_DIALOGUE;
        public static DialogueNode SEA_ELEMENT_DIALOGUE, PRIMEVAL_ELEMENT_DIALOGUE, LAND_ELEMENT_DIALOGUE, SKY_ELEMENT_DIALOGUE;
        public static DialogueNode AUTUMNS_KISS_DIALOGUE, BIZARRE_PERFUME_DIALOGUE, BLISSFUL_SKY_DIALOGUE, FLORAL_PERFUME_DIALOGUE, OCEAN_GUST_DIALOGUE, 
            RED_ANGEL_DIALOGUE, SUMMERS_GIFT_DIALOGUE, SWEET_BREEZE_DIALOGUE, WARM_MEMORIES_DIALOGUE;

        private static DialogueNode GeneratePerfumeDialogue(AppliedEffects.Effect perfumeEffect, string afterUse)
        {
            DialogueNode perfumeDialogue = new DialogueNode("Apply the perfume?", DialogueNode.PORTRAIT_SYSTEM);
            perfumeDialogue.decisionLeftText = "Yes";
            perfumeDialogue.decisionRightText = "No";
            perfumeDialogue.decisionLeftNode = new DialogueNode(afterUse, DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
            {
                player.ClearPerfumeEffects();
                player.ApplyEffect(perfumeEffect, AppliedEffects.LENGTH_VERY_LONG);
                player.GetHeldItem().Subtract(1);
            });
            perfumeDialogue.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);

            return perfumeDialogue;
        }

        public static void Initialize()
        {
            if (AUTUMNS_KISS_DIALOGUE == null)
            {
                AUTUMNS_KISS_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_AUTUMNS_KISS, "Smelling good!");
            }
            if (BIZARRE_PERFUME_DIALOGUE == null)
            {
                BIZARRE_PERFUME_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_BIZARRE_PERFUME, "Smelling... odd!");
            }
            if (BLISSFUL_SKY_DIALOGUE == null)
            {
                BLISSFUL_SKY_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_BLISSFUL_SKY, "Smelling good!");
            }
            if (FLORAL_PERFUME_DIALOGUE == null)
            {
                FLORAL_PERFUME_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_FLORAL_PERFUME, "Smelling good!");
            }
            if (OCEAN_GUST_DIALOGUE == null)
            {
                OCEAN_GUST_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_OCEAN_GUST, "Smelling good!");
            }
            if (RED_ANGEL_DIALOGUE == null)
            {
                RED_ANGEL_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_RED_ANGEL, "Smelling good!");
            }
            if (SUMMERS_GIFT_DIALOGUE == null)
            {
                SUMMERS_GIFT_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_SUMMERS_GIFT, "Smelling good!");
            }
            if (SWEET_BREEZE_DIALOGUE == null)
            {
                SWEET_BREEZE_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_SWEET_BREEZE, "Smelling good!");
            }
            if (WARM_MEMORIES_DIALOGUE == null)
            {
                WARM_MEMORIES_DIALOGUE = GeneratePerfumeDialogue(AppliedEffects.PERFUME_WARM_MEMORIES, "Smelling good!");
            }

            if (SEA_ELEMENT_DIALOGUE == null)
            {
                DialogueNode success = new DialogueNode("Its purpose complete, the stone shatters.", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {
                    Vector2 playerLocation = player.GetAdjustedPosition();
                    for (int i = 0; i < 500; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * ELEMENT_PARTICLES_X_RADIUS, 8 * ELEMENT_PARTICLES_X_RADIUS), Util.RandInt(-8 * ELEMENT_PARTICLES_Y_RADIUS, 8 * ELEMENT_PARTICLES_Y_RADIUS)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_WATER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * ELEMENT_PARTICLES_X_RADIUS, 8 * ELEMENT_PARTICLES_X_RADIUS), Util.RandInt(-8 * ELEMENT_PARTICLES_Y_RADIUS, 8 * ELEMENT_PARTICLES_Y_RADIUS)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_WATER_SECONDARY.color, ParticleFactory.DURATION_LONG));
                    }
                    world.SetWeather(World.Weather.RAINY);
                    player.GetHeldItem().Subtract(1);
                });
                DialogueNode failure = new DialogueNode("Using the stone achieves nothing, as the weather is already rainy.", DialogueNode.PORTRAIT_SYSTEM);

                SEA_ELEMENT_DIALOGUE = new DialogueNode("Activate the Sea Element?", DialogueNode.PORTRAIT_SYSTEM);
                SEA_ELEMENT_DIALOGUE.decisionLeftText = "Yes";
                SEA_ELEMENT_DIALOGUE.decisionRightText = "No";
                SEA_ELEMENT_DIALOGUE.decisionLeftNode = new DialogueNode((player, area, world) => { return world.GetWeather() != World.Weather.RAINY; }, success, failure);
                SEA_ELEMENT_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (SKY_ELEMENT_DIALOGUE == null)
            {
                DialogueNode success = new DialogueNode("Its purpose complete, the stone shatters.", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {
                    Vector2 playerLocation = player.GetAdjustedPosition();
                    for (int i = 0; i < 500; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * ELEMENT_PARTICLES_X_RADIUS, 8 * ELEMENT_PARTICLES_X_RADIUS), Util.RandInt(-8 * ELEMENT_PARTICLES_Y_RADIUS, 8 * ELEMENT_PARTICLES_Y_RADIUS)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GOLDEN_WISP.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * ELEMENT_PARTICLES_X_RADIUS, 8 * ELEMENT_PARTICLES_X_RADIUS), Util.RandInt(-8 * ELEMENT_PARTICLES_Y_RADIUS, 8 * ELEMENT_PARTICLES_Y_RADIUS)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_YELLOW_WISP.color, ParticleFactory.DURATION_LONG));
                    }
                    world.SetWeather(World.Weather.SUNNY);
                    player.GetHeldItem().Subtract(1);
                });
                DialogueNode failure = new DialogueNode("Using the stone achieves nothing, as the weather is already sunny.", DialogueNode.PORTRAIT_SYSTEM);

                SKY_ELEMENT_DIALOGUE = new DialogueNode("Activate the Sky Element?", DialogueNode.PORTRAIT_SYSTEM);
                SKY_ELEMENT_DIALOGUE.decisionLeftText = "Yes";
                SKY_ELEMENT_DIALOGUE.decisionRightText = "No";
                SKY_ELEMENT_DIALOGUE.decisionLeftNode = new DialogueNode((player, area, world) => { return world.GetWeather() != World.Weather.SUNNY; }, success, failure);
                SKY_ELEMENT_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (PRIMEVAL_ELEMENT_DIALOGUE == null)
            {
                PRIMEVAL_ELEMENT_DIALOGUE = new DialogueNode("Activate the Primeval Element?", DialogueNode.PORTRAIT_SYSTEM);
                PRIMEVAL_ELEMENT_DIALOGUE.decisionLeftText = "Yes";
                PRIMEVAL_ELEMENT_DIALOGUE.decisionRightText = "No";
                PRIMEVAL_ELEMENT_DIALOGUE.decisionLeftNode = new DialogueNode("Its purpose complete, the stone shatters.", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {
                    Vector2 playerLocation = player.GetAdjustedPosition();
                    Vector2 centerLocation = new Vector2(playerLocation.X, playerLocation.Y + 16);
                    Vector2 tileLocation = new Vector2(((int)(centerLocation.X/ 8.0f)), ((int)(centerLocation.Y/ 8.0f)));

                    Rectangle legalTiles = new Rectangle((int)tileLocation.X - PRIMEVIL_X_RADIUS, (int)tileLocation.Y - PRIMEVIL_Y_RADIUS, 2* PRIMEVIL_X_RADIUS, 2* PRIMEVIL_Y_RADIUS);

                    for (int i = 0; i < 10; i++)
                    {
                        area.TickSpawnZones(legalTiles);
                    }

                    for (int i = 0; i < 175; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(centerLocation + new Vector2(Util.RandInt(-8 * PRIMEVIL_X_RADIUS, 8 * PRIMEVIL_X_RADIUS), Util.RandInt(-8 * PRIMEVIL_Y_RADIUS, 8 * PRIMEVIL_Y_RADIUS)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.RED_SAND_PRIMARY.color, ParticleFactory.DURATION_MEDIUM));
                        area.AddParticle(ParticleFactory.GenerateParticle(centerLocation + new Vector2(Util.RandInt(-8 * PRIMEVIL_X_RADIUS, 8 * PRIMEVIL_X_RADIUS), Util.RandInt(-8 * PRIMEVIL_Y_RADIUS, 8 * PRIMEVIL_Y_RADIUS)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.RED_SAND_SECONDARY.color, ParticleFactory.DURATION_MEDIUM));
                        area.AddParticle(ParticleFactory.GenerateParticle(centerLocation + new Vector2(Util.RandInt(-8 * PRIMEVIL_X_RADIUS, 8 * PRIMEVIL_X_RADIUS), Util.RandInt(-8 * PRIMEVIL_Y_RADIUS, 8 * PRIMEVIL_Y_RADIUS)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SPRING_PRIMARY.color, ParticleFactory.DURATION_MEDIUM));
                    }

                    player.GetHeldItem().Subtract(1);
                });
                PRIMEVAL_ELEMENT_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (LAND_ELEMENT_DIALOGUE == null)
            {
                DialogueNode firstUse = new DialogueNode("The crystal undulates vigorously.\nYour current position has been remembered.", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {
                    for (int i = 0; i < 80; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(player.GetCenteredPosition() + new Vector2(0, Util.RandInt(-200, 200)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GOLDEN_WISP.color, ParticleFactory.DURATION_VERY_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(player.GetCenteredPosition() + new Vector2(0, Util.RandInt(-200, 200)),
                            ParticleBehavior.ROTATE_STATIONARY, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_YELLOW_WISP.color, ParticleFactory.DURATION_VERY_LONG));
                    }
                    GameState.LAND_ELEMENT_X = player.GetAdjustedPosition().X;
                    GameState.LAND_ELEMENT_Y = player.GetAdjustedPosition().Y - EntityPlayer.HEIGHT + 16;
                    GameState.LAND_ELEMENT_AREA = area.GetAreaEnum().ToString();
                    GameState.LAND_ELEMENT_PRIMED = true;
                });

                DialogueNode secondUse = new DialogueNode("", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {
                    player.TransitionTo(GameState.LAND_ELEMENT_AREA, new Vector2(GameState.LAND_ELEMENT_X, GameState.LAND_ELEMENT_Y), Area.TransitionZone.Animation.TO_UP);
                    GameState.LAND_ELEMENT_PRIMED = false;
                    player.GetHeldItem().Subtract(1);
                });

                DialogueNode cancel = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);

                DialogueNode promptSecondUse = new DialogueNode("Activate the Land Element?\nThis will teleport you to the stored location.", DialogueNode.PORTRAIT_SYSTEM);
                promptSecondUse.decisionLeftText = "Yes";
                promptSecondUse.decisionLeftNode = secondUse;
                promptSecondUse.decisionRightText = "No";
                promptSecondUse.decisionRightNode = cancel;

                DialogueNode promptFirstUse = new DialogueNode("Activate the Land Element?\nThis cause your current location to be stored for later recall.", DialogueNode.PORTRAIT_SYSTEM);
                promptFirstUse.decisionLeftText = "Yes";
                promptFirstUse.decisionLeftNode = firstUse;
                promptFirstUse.decisionRightText = "No";
                promptFirstUse.decisionRightNode = cancel;

                LAND_ELEMENT_DIALOGUE = new DialogueNode((player, area, world) => { return GameState.LAND_ELEMENT_PRIMED; }, promptSecondUse, promptFirstUse);
                LAND_ELEMENT_DIALOGUE.decisionLeftText = "neednothing";

            }

            if (BURST_STONE_DIALOGUE == null)
            {
                BURST_STONE_DIALOGUE = new DialogueNode("Use the Burst Stone?", DialogueNode.PORTRAIT_SYSTEM);
                BURST_STONE_DIALOGUE.decisionLeftText = "Yes";
                BURST_STONE_DIALOGUE.decisionRightText = "No";
                BURST_STONE_DIALOGUE.decisionLeftNode = new DialogueNode("Kaboom!", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {

                    Vector2 playerLocation = player.GetAdjustedPosition();
                    Vector2 tileLocation = new Vector2(((int)(playerLocation.X / 8.0f)), ((int)(playerLocation.Y / 8.0f)));

                    for (int i = 0; i < 200; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BURST_STONE_X_RADIUS, 8 * BURST_STONE_X_RADIUS), Util.RandInt(-8 * BURST_STONE_Y_RADIUS, 8 * BURST_STONE_Y_RADIUS)),
                            ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.RED_SAND_PRIMARY.color, ParticleFactory.DURATION_MEDIUM));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BURST_STONE_X_RADIUS, 8 * BURST_STONE_X_RADIUS), Util.RandInt(-8 * BURST_STONE_Y_RADIUS, 8 * BURST_STONE_Y_RADIUS)),
                            ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.BLACK_GROUND_PRIMARY.color, ParticleFactory.DURATION_MEDIUM));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BURST_STONE_X_RADIUS, 8 * BURST_STONE_X_RADIUS), Util.RandInt(-8 * BURST_STONE_Y_RADIUS, 8 * BURST_STONE_Y_RADIUS)),
                            ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.RED_SAND_SECONDARY.color, ParticleFactory.DURATION_MEDIUM));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BURST_STONE_X_RADIUS, 8 * BURST_STONE_X_RADIUS), Util.RandInt(-8 * BURST_STONE_Y_RADIUS, 8 * BURST_STONE_Y_RADIUS)),
                            ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.ORANGE_EARTH_PRIMARY.color, ParticleFactory.DURATION_MEDIUM));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BURST_STONE_X_RADIUS, 8 * BURST_STONE_X_RADIUS), Util.RandInt(-8 * BURST_STONE_Y_RADIUS, 8 * BURST_STONE_Y_RADIUS)),
                            ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.ORANGE_EARTH_SECONDARY.color, ParticleFactory.DURATION_MEDIUM));
                    }

                    for (int x = -BURST_STONE_X_RADIUS; x <= BURST_STONE_X_RADIUS; x++)
                    {
                        for (int y = -BURST_STONE_Y_RADIUS; y <= BURST_STONE_Y_RADIUS; y++)
                        {
                            Vector2 targetTile = new Vector2(tileLocation.X + x, tileLocation.Y + y);
                            TileEntity targetEntity = area.GetTileEntity((int)targetTile.X, (int)targetTile.Y);
                            if (targetEntity == null)
                            {
                                continue;
                            }
                            if (targetEntity is IDetonate)
                            {
                                ((IDetonate)targetEntity).Detonate(player, area, world);
                            }
                        }
                    }

                    player.GetHeldItem().Subtract(1);
                });
                BURST_STONE_DIALOGUE.decisionRightNode = new DialogueNode("Probably a wise decision.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (UNSTABLE_LIQUID_DIALOGUE == null)
            {
                UNSTABLE_LIQUID_DIALOGUE = new DialogueNode("Use the Unstable Liquid?", DialogueNode.PORTRAIT_SYSTEM);
                UNSTABLE_LIQUID_DIALOGUE.decisionLeftText = "Yes";
                UNSTABLE_LIQUID_DIALOGUE.decisionRightText = "No";
                UNSTABLE_LIQUID_DIALOGUE.decisionLeftNode = new DialogueNode("Kaboom!", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {

                    Vector2 playerLocation = player.GetAdjustedPosition();
                    for (int i = 0; i < 500; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * UNSTABLE_LIQUID_X_RADIUS, 8 * UNSTABLE_LIQUID_X_RADIUS), Util.RandInt(-8 * UNSTABLE_LIQUID_Y_RADIUS, 8 * UNSTABLE_LIQUID_Y_RADIUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_WATER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * UNSTABLE_LIQUID_X_RADIUS, 8 * UNSTABLE_LIQUID_X_RADIUS), Util.RandInt(-8 * UNSTABLE_LIQUID_Y_RADIUS, 8 * UNSTABLE_LIQUID_Y_RADIUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_WATER_SECONDARY.color, ParticleFactory.DURATION_LONG));
                    }

                    Vector2 tileLocation = new Vector2(((int)(playerLocation.X / 8.0f)), ((int)(playerLocation.Y / 8.0f)));
                    for (int x = -UNSTABLE_LIQUID_X_RADIUS; x <= UNSTABLE_LIQUID_X_RADIUS; x++)
                    {
                        for (int y = -UNSTABLE_LIQUID_Y_RADIUS; y <= UNSTABLE_LIQUID_Y_RADIUS; y++)
                        {
                            Vector2 targetTile = new Vector2(tileLocation.X + x, tileLocation.Y + y);
                            TileEntity targetEntity = area.GetTileEntity((int)targetTile.X, (int)targetTile.Y);
                            if (targetEntity == null)
                            {
                                continue;
                            }
                            else if (targetEntity is TEntityFarmable)
                            {
                                ((TEntityFarmable)targetEntity).Water(area);
                            }
                        }
                    }
                    player.GetHeldItem().Subtract(1);
                });
                UNSTABLE_LIQUID_DIALOGUE.decisionRightNode = new DialogueNode("Probably a wise decision.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (MOSS_BOTTLE_DIALOGUE == null)
            {
                MOSS_BOTTLE_DIALOGUE = new DialogueNode("Use the Moss Bottle?", DialogueNode.PORTRAIT_SYSTEM);
                MOSS_BOTTLE_DIALOGUE.decisionLeftText = "Yes";
                MOSS_BOTTLE_DIALOGUE.decisionRightText = "No";
                MOSS_BOTTLE_DIALOGUE.decisionLeftNode = new DialogueNode("Kaboom!", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {

                    Vector2 playerLocation = player.GetAdjustedPosition();
                    for (int i = 0; i < 333; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SUMMER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SUMMER_SECONDARY.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SPRING_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    }

                    Vector2 tileLocation = new Vector2(((int)(playerLocation.X / 8.0f)), ((int)(playerLocation.Y / 8.0f)));
                    for (int x = -BOTTLE_X_RADIUS; x <= BOTTLE_X_RADIUS; x++)
                    {
                        for (int y = -BOTTLE_Y_RAIDUS; y <= BOTTLE_Y_RAIDUS; y++)
                        {
                            Vector2 targetTile = new Vector2(tileLocation.X + x, tileLocation.Y + y);
                            TileEntity targetEntity = area.GetTileEntity((int)targetTile.X, (int)targetTile.Y);
                            if (targetEntity == null)
                            {
                                continue;
                            }
                            else if (targetEntity is TEntityFarmable)
                            {
                                ((TEntityFarmable)targetEntity).Accelerate(1, area);
                            }
                        }
                    }
                    player.GetHeldItem().Subtract(1);
                });
                MOSS_BOTTLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (TROPICAL_BOTTLE_DIALOGUE == null)
            {
                TROPICAL_BOTTLE_DIALOGUE = new DialogueNode("Use the Tropical Bottle?", DialogueNode.PORTRAIT_SYSTEM);
                TROPICAL_BOTTLE_DIALOGUE.decisionLeftText = "Yes";
                TROPICAL_BOTTLE_DIALOGUE.decisionRightText = "No";
                TROPICAL_BOTTLE_DIALOGUE.decisionLeftNode = new DialogueNode("Kaboom!", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {

                    Vector2 playerLocation = player.GetAdjustedPosition();
                    for (int i = 0; i < 333; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SUMMER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SUMMER_SECONDARY.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_YELLOW_WISP.color, ParticleFactory.DURATION_LONG));
                    }

                    Vector2 tileLocation = new Vector2(((int)(playerLocation.X / 8.0f)), ((int)(playerLocation.Y / 8.0f)));
                    for (int x = -BOTTLE_X_RADIUS; x <= BOTTLE_X_RADIUS; x++)
                    {
                        for (int y = -BOTTLE_Y_RAIDUS; y <= BOTTLE_Y_RAIDUS; y++)
                        {
                            Vector2 targetTile = new Vector2(tileLocation.X + x, tileLocation.Y + y);
                            TileEntity targetEntity = area.GetTileEntity((int)targetTile.X, (int)targetTile.Y);
                            if (targetEntity == null)
                            {
                                continue;
                            }
                            else if (targetEntity is TEntityFarmable)
                            {
                                ((TEntityFarmable)targetEntity).Accelerate(3, area);
                            }
                        }
                    }
                    player.GetHeldItem().Subtract(1);
                });
                TROPICAL_BOTTLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
            }
            if (SKY_BOTTLE_DIALOGUE == null)
            {
                SKY_BOTTLE_DIALOGUE = new DialogueNode("Use the Sky Bottle?", DialogueNode.PORTRAIT_SYSTEM);
                SKY_BOTTLE_DIALOGUE.decisionLeftText = "Yes";
                SKY_BOTTLE_DIALOGUE.decisionRightText = "No";
                SKY_BOTTLE_DIALOGUE.decisionLeftNode = new DialogueNode("Kaboom!", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {

                    Vector2 playerLocation = player.GetAdjustedPosition();
                    for (int i = 0; i < 333; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_BLUE_RISER.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_WHITE_WISP.color, ParticleFactory.DURATION_LONG));
                        area.AddParticle(ParticleFactory.GenerateParticle(playerLocation + new Vector2(Util.RandInt(-8 * BOTTLE_X_RADIUS, 8 * BOTTLE_X_RADIUS), Util.RandInt(-8 * BOTTLE_Y_RAIDUS, 8 * BOTTLE_Y_RAIDUS)),
                            ParticleBehavior.RUSH_UPWARD, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_CHERRY_SPRING_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    }

                    Vector2 tileLocation = new Vector2(((int)(playerLocation.X / 8.0f)), ((int)(playerLocation.Y / 8.0f)));
                    for (int x = -BOTTLE_X_RADIUS; x <= BOTTLE_X_RADIUS; x++)
                    {
                        for (int y = -BOTTLE_Y_RAIDUS; y <= BOTTLE_Y_RAIDUS; y++)
                        {
                            Vector2 targetTile = new Vector2(tileLocation.X + x, tileLocation.Y + y);
                            TileEntity targetEntity = area.GetTileEntity((int)targetTile.X, (int)targetTile.Y);
                            if (targetEntity == null)
                            {
                                continue;
                            }
                            else if (targetEntity is TEntityFarmable)
                            {
                                ((TEntityFarmable)targetEntity).Accelerate(10, area);
                            }
                        }
                    }
                    player.GetHeldItem().Subtract(1);
                });
                SKY_BOTTLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (MILK_CREAM_DIALOGUE == null)
            {
                MILK_CREAM_DIALOGUE = new DialogueNode("Drink it?", DialogueNode.PORTRAIT_SYSTEM);
                MILK_CREAM_DIALOGUE.decisionLeftText = "Yes";
                MILK_CREAM_DIALOGUE.decisionRightText = "No";
                MILK_CREAM_DIALOGUE.decisionLeftNode = new DialogueNode("You got milk. It's very refreshing!", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {
                    player.ClearEffects();
                    player.GetHeldItem().Subtract(1);
                });
                MILK_CREAM_DIALOGUE.decisionRightNode = new DialogueNode("Don't got milk?", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (INVINCIROID_DIALOGUE == null)
            {
                INVINCIROID_DIALOGUE = new DialogueNode("Drink the Invinciroid?", DialogueNode.PORTRAIT_SYSTEM);
                INVINCIROID_DIALOGUE.decisionLeftText = "Yes";
                INVINCIROID_DIALOGUE.decisionRightText = "No";
                INVINCIROID_DIALOGUE.decisionLeftNode = new DialogueNode("The taste is empowering. You feel strong, indomitable. Time to get back to work!", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
                {
                    player.ExtendEffects(INVINCIROID_TIME);
                    player.GetHeldItem().Subtract(1);
                });
                INVINCIROID_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
            }

            if (BLACK_CANDLE_DIALOGUE == null)
            {
                DialogueNode teleport = new DialogueNode("", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    player.TransitionTo("FARM", "SPfarmhouseOutside", Area.TransitionZone.Animation.TO_UP);
                    player.GetHeldItem().Subtract(1);
                });
                DialogueNode failure = new DialogueNode("The wick refuses to light. It seems the candle will not work at this time.", DialogueNode.PORTRAIT_SYSTEM);
                BLACK_CANDLE_DIALOGUE = new DialogueNode("Light the candle?", DialogueNode.PORTRAIT_SYSTEM);
                BLACK_CANDLE_DIALOGUE.decisionLeftText = "Yes";
                BLACK_CANDLE_DIALOGUE.decisionRightText = "No";
                BLACK_CANDLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
                BLACK_CANDLE_DIALOGUE.decisionLeftNode = new DialogueNode((player, area, world) => { return true; }, teleport, failure);
            }

            if (SOOTHE_CANDLE_DIALOGUE == null)
            {
                DialogueNode teleport = new DialogueNode("", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    player.TransitionTo("S2", "SPs2Candle", Area.TransitionZone.Animation.TO_UP);
                    player.GetHeldItem().Subtract(1);
                });
                DialogueNode failure = new DialogueNode("The wick refuses to light. It seems the candle will not work at this time.", DialogueNode.PORTRAIT_SYSTEM);
                SOOTHE_CANDLE_DIALOGUE = new DialogueNode("Light the candle?", DialogueNode.PORTRAIT_SYSTEM);
                SOOTHE_CANDLE_DIALOGUE.decisionLeftText = "Yes";
                SOOTHE_CANDLE_DIALOGUE.decisionRightText = "No";
                SOOTHE_CANDLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
                SOOTHE_CANDLE_DIALOGUE.decisionLeftNode = new DialogueNode((player, area, world) => { return GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 2; }, teleport, failure);
            }

            if (SALTED_CANDLE_DIALOGUE == null)
            {
                DialogueNode teleport = new DialogueNode("", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    player.TransitionTo("BEACH", "SPbeachCandle", Area.TransitionZone.Animation.TO_UP);
                    player.GetHeldItem().Subtract(1);
                });
                DialogueNode failure = new DialogueNode("The wick refuses to light. It seems the candle will not work at this time.", DialogueNode.PORTRAIT_SYSTEM);
                SALTED_CANDLE_DIALOGUE = new DialogueNode("Light the candle?", DialogueNode.PORTRAIT_SYSTEM);
                SALTED_CANDLE_DIALOGUE.decisionLeftText = "Yes";
                SALTED_CANDLE_DIALOGUE.decisionRightText = "No";
                SALTED_CANDLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
                SALTED_CANDLE_DIALOGUE.decisionLeftNode = new DialogueNode((player, area, world) => { return true; }, teleport, failure);
            }

            if (SPICED_CANDLE_DIALOGUE == null)
            {
                DialogueNode teleport = new DialogueNode("", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    player.TransitionTo("S1", "SPs1Candle", Area.TransitionZone.Animation.TO_UP);
                    player.GetHeldItem().Subtract(1);
                });
                DialogueNode failure = new DialogueNode("The wick refuses to light. It seems the candle will not work at this time.", DialogueNode.PORTRAIT_SYSTEM);
                SPICED_CANDLE_DIALOGUE = new DialogueNode("Light the candle?", DialogueNode.PORTRAIT_SYSTEM);
                SPICED_CANDLE_DIALOGUE.decisionLeftText = "Yes";
                SPICED_CANDLE_DIALOGUE.decisionRightText = "No";
                SPICED_CANDLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
                SPICED_CANDLE_DIALOGUE.decisionLeftNode = new DialogueNode((player, area, world) => { return GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 1; }, teleport, failure);
            }

            if (SUGAR_CANDLE_DIALOGUE == null)
            {
                DialogueNode teleport = new DialogueNode("", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) => {
                    player.TransitionTo("S3", "SPs3Candle", Area.TransitionZone.Animation.TO_UP);
                    player.GetHeldItem().Subtract(1);
                });
                DialogueNode failure = new DialogueNode("The wick refuses to light. It seems the candle will not work at this time.", DialogueNode.PORTRAIT_SYSTEM);
                SUGAR_CANDLE_DIALOGUE = new DialogueNode("Light the candle?", DialogueNode.PORTRAIT_SYSTEM);
                SUGAR_CANDLE_DIALOGUE.decisionLeftText = "Yes";
                SUGAR_CANDLE_DIALOGUE.decisionRightText = "No";
                SUGAR_CANDLE_DIALOGUE.decisionRightNode = new DialogueNode("Maybe later.", DialogueNode.PORTRAIT_SYSTEM);
                SUGAR_CANDLE_DIALOGUE.decisionLeftNode = new DialogueNode((player, area, world) => { return GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 3; }, teleport, failure);
            }
        }

        private static void GenerateCandleDialogue()
        {

        }

        public UsableItem(string name, string texturePath, int stackCapacity, string mouseText, DialogueNode onUse, string description, int value, params Tag[] tags) : base(name, texturePath, stackCapacity, description, value, tags)
        {
            this.onUse = onUse;
            this.mouseText = mouseText;
        }

        public string GetMouseText()
        {
            return mouseText;
        }

        public void Use(EntityPlayer player)
        {
            player.SetCurrentDialogue(onUse);
        }
    }
}
