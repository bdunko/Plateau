using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;
using Plateau.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class TEntityGrass : TileEntity, IInteractTool, ITickDaily, IDetonate
    {
        private AnimatedSprite sprite;
        private EntityType type;

        public TEntityGrass(AnimatedSprite sprite, Vector2 tilePosition, EntityType type) : base(tilePosition, 1, 1, DrawLayer.FOREGROUND)
        {
            this.sprite = sprite;
            this.type = type;
            sprite.AddLoop("spring", 0, 0, false);
            sprite.AddLoop("springC", 1, 1, false);
            sprite.AddLoop("springL", 2, 2, false);
            sprite.AddLoop("springR", 3, 3, false);
            sprite.AddLoop("summer", 4, 4, false);
            sprite.AddLoop("summerC", 5, 5, false);
            sprite.AddLoop("summerL", 6, 6, false);
            sprite.AddLoop("summerR", 7, 7, false);
            sprite.AddLoop("fall", 8, 8, false);
            sprite.AddLoop("fallC", 9, 9, false);
            sprite.AddLoop("fallL", 10, 10, false);
            sprite.AddLoop("fallR", 11, 11, false);
            sprite.AddLoop("winter", 12, 12, false);
            sprite.AddLoop("winterC", 13, 13, false);
            sprite.AddLoop("winterL", 14, 14, false);
            sprite.AddLoop("winterR", 15, 15, false);
            sprite.SetLoop("spring");
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, position - new Vector2(0, 8), Color.White);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(8, 8));
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public void InteractTool(EntityPlayer player, Area area, World world)
        {
            if (player.GetHeldItem().GetItem().HasTag(Item.Tag.HOE))
            {
                DestroyGrass(player, area, world);
            }
        }

        private void DestroyGrass(EntityPlayer player, Area area, World world)
        {
            for (int i = 0; i < 6; i++)
            {
                string season = sprite.GetCurrentLoop();
                if (season.StartsWith("spring"))
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        Util.PARTICLE_GRASS_SPRING_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SPRING_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SPRING_SECONDARY.color, ParticleFactory.DURATION_LONG));
                }
                else if (season.StartsWith("summer"))
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        Util.PARTICLE_GRASS_SUMMER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SUMMER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_SUMMER_SECONDARY.color, ParticleFactory.DURATION_LONG));
                }
                else if (season.StartsWith("fall"))
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        Util.PARTICLE_GRASS_FALL_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_FALL_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_FALL_SECONDARY.color, ParticleFactory.DURATION_LONG));
                }
                else
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        Util.PARTICLE_GRASS_WINTER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_WINTER_PRIMARY.color, ParticleFactory.DURATION_LONG));
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -0.1f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            Util.PARTICLE_GRASS_WINTER_SECONDARY.color, ParticleFactory.DURATION_LONG));
                }
            }
            area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(4, -3), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                area.GetSecondaryColorForTile((int)tilePosition.X, (int)tilePosition.Y), ParticleFactory.DURATION_LONG));
            area.RemoveTileEntity(player, (int)this.tilePosition.X, (int)this.tilePosition.Y, world);

            if (area.GetSeason() == World.Season.WINTER)
            {
                LootTables.LootTable snow = LootTables.SNOW;
                List<Item> snowLoot = snow.RollLoot(player, area, world.GetTimeData());
                foreach (Item ins in snowLoot)
                {
                    area.AddEntity(new EntityItem(ins, new Vector2(position.X, position.Y - 4)));
                }
            }
            else if (Util.RandInt(1, 6) == 1)
            {
                LootTables.LootTable insects = LootTables.INSECT_GRASS;
                List<Item> loot = insects.RollLoot(player, area, world.GetTimeData());
                foreach (Item ins in loot)
                {
                    area.AddEntity(new EntityItem(ins, new Vector2(position.X, position.Y - 16)));
                }
            }
        }

        public void Detonate(EntityPlayer player, Area area, World world)
        {
            //decided not to detonate grass after all, leaving this in case I change my mind
            //DestroyGrass(player, area, world);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("entitytype", type.ToString());
            save.AddData("sprite", sprite.GetCurrentLoop());
            return save;
        }

        public override bool ShouldBeSaved()
        {
            return true;
        }

        public override void LoadSave(SaveState state)
        {
            base.LoadSave(state);
            sprite.SetLoop(state.TryGetData("sprite", "spring"));
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
        }

        private void UpdateSprite(World.Season season, Area area)
        {
            string loopNameBase = "";
            switch (season)
            {
                case World.Season.SPRING:
                    loopNameBase = "spring";
                    break;
                case World.Season.SUMMER:
                    loopNameBase = "summer";
                    break;
                case World.Season.AUTUMN:
                    loopNameBase = "fall";
                    break;
                case World.Season.WINTER:
                    loopNameBase = "winter";
                    break;
            }

            Vector2 left = this.tilePosition + new Vector2(-1, 0);
            Vector2 right = this.tilePosition + new Vector2(1, 0);
            bool grassOnLeft = area.GetTileEntity((int)left.X, (int)left.Y) is TEntityGrass;
            bool grassOnRight = area.GetTileEntity((int)right.X, (int)right.Y) is TEntityGrass;
            if(grassOnLeft && grassOnRight)
            {
                sprite.SetLoop(loopNameBase + "C");
            } else if (grassOnLeft && !grassOnRight)
            {
                sprite.SetLoop(loopNameBase + "R");
            } else if (grassOnRight && !grassOnLeft)
            {
                sprite.SetLoop(loopNameBase + "L");
            } else
            {
                sprite.SetLoop(loopNameBase);
            }
        }

        public void TickDaily(World world, Area area, EntityPlayer player)
        {
            UpdateSprite(area.GetSeason(), area);
            /*for (int i = 0; i < 2; i++)
            {
                Vector2 newGrassPos = new Vector2(this.tilePosition.X, this.tilePosition.Y) + new Vector2(Util.RandInt(-1, 1), Util.RandInt(-1, 1));
                if (area.IsFloorEntityPlacementValid((int)newGrassPos.X, (int)newGrassPos.Y, 1))
                {
                    area.AddTileEntity(TileEntityFactory.GetEntity(EntityType.GRASS, null, newGrassPos, area));
                    UpdateSprite(timeData.season, area);
                }
            }*/
        }
    }
}
