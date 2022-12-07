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
using Plateau.Particles;

namespace Plateau.Entities
{
    class TEntityToolable : TileEntity, IInteractTool, IHaveHealthBar, IDetonate
    {
        protected Texture2D texture;
        private EntityType type;
        private HealthBar healthBar;
        private LootTables.LootTable lootTable;
        private Item.Tag toolUsed;
        private Color particlePrimary, particleSecondary;
        private bool ceiling;

        private static float SHAKE_DURATION = 0.19f;
        private static float SHAKE_INTERVAL = 0.06f;
        private const float SHAKE_AMOUNT = 0.75f;
        private float shakeTimeLeft;
        private float shakeTimer;
        private float shakeModX;

        public TEntityToolable(Texture2D texture, Vector2 tilePosition, EntityType type, Item.Tag toolUsed, HealthBar hb, LootTables.LootTable lootTable, Color particlePrimary, Color particleSecondary, bool ceiling = false) : base(tilePosition, texture.Width / 8, texture.Height / 8, DrawLayer.NORMAL)
        {
            this.toolUsed = toolUsed;
            this.healthBar = hb;
            this.texture = texture;
            this.type = type;
            this.ceiling = ceiling;
            this.particlePrimary = particlePrimary;
            this.particleSecondary = particleSecondary;
            this.lootTable = lootTable;
            this.shakeTimeLeft = 0;
            this.shakeTimer = 0;
            this.shakeModX = 0;
            this.healthBar.SetPosition(this.position + new Vector2((texture.Width / 2) - (healthBar.GetWidth() / 2), 0) + (ceiling ? new Vector2(0, texture.Height - 4) : new Vector2(0, -8)));
        }

        public HealthBar GetHealthBar()
        {
            return this.healthBar;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sb.Draw(texture, position + new Vector2(shakeModX, ceiling ? 0 : 1), Color.White);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(texture.Width, texture.Height));
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
            Item tool = player.GetHeldItem().GetItem();
            bool rightTool = false;
            if(toolUsed == Item.Tag.PICKAXE_OR_AXE)
            {
                if(tool.HasTag(Item.Tag.PICKAXE) || tool.HasTag(Item.Tag.AXE))
                {
                    rightTool = true;
                }
            } else
            {
                rightTool = tool.HasTag(toolUsed);
            }

            if (rightTool)
            {
                shakeTimeLeft = SHAKE_DURATION;
                shakeTimer = 0;
                healthBar.Damage(((DamageDealingItem)tool).GetDamage(player, world.GetTimeData()));
                for (int i = 0; i < 6; i++)
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width/2, texture.Height/2), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                        particlePrimary, ParticleFactory.DURATION_LONG));
                }

                CheckDestroyed(player, area, world);
            }
        }

        private void CheckDestroyed(EntityPlayer player, Area area, World world)
        {
            if (healthBar.IsDepleted())
            {
                area.RemoveTileEntity(player, (int)tilePosition.X, (int)tilePosition.Y, world);
                List<Item> drops = lootTable.RollLoot(player, area, world.GetTimeData());
                foreach (Item drop in drops)
                {
                    if (player.GetGravityState() == EntityPlayer.GravityState.REVERSED)
                    {
                        area.AddEntity(new EntityItem(drop, new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2), new Vector2(Util.RandInt(-50, 50) / 100.0f, -1.4f)));
                    }
                    else
                    {
                        area.AddEntity(new EntityItem(drop, new Vector2(position.X + texture.Width/2 - 8, position.Y - 8 + texture.Height / 2)));
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width / 2, texture.Height / 2), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        particlePrimary, ParticleFactory.DURATION_LONG));
                }
                for (int i = 0; i < 4; i++)
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width / 2, texture.Height / 2), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        particleSecondary, ParticleFactory.DURATION_LONG));
                }

                if (toolUsed == Item.Tag.PICKAXE)
                {
                    GameState.STATISTICS[GameState.STAT_ROCKS_MINED] += 1;
                }
                else if (toolUsed == Item.Tag.AXE)
                {
                    GameState.STATISTICS[GameState.STAT_WOOD_CHOPPED] += 1;
                }
            }
        }

        public void Detonate(EntityPlayer player, Area area, World world)
        {
            healthBar.Damage(Int32.MaxValue);
            CheckDestroyed(player, area, world);
        }

        public override SaveState GenerateSave()
        {
            SaveState save =  base.GenerateSave();
            save.AddData("entitytype", type.ToString());
            return save;
        }

        public override bool ShouldBeSaved()
        {
            return true;
        }

        public override void Update(float deltaTime, Area area)
        {
            healthBar.Update(deltaTime);

            if (shakeTimeLeft > 0)
            {
                shakeTimer += deltaTime;
                if (shakeTimer >= SHAKE_INTERVAL)
                {
                    switch (shakeModX)
                    {
                        case 0:
                            shakeModX = SHAKE_AMOUNT;
                            break;
                        case SHAKE_AMOUNT:
                            shakeModX = -SHAKE_AMOUNT;
                            break;
                        case -SHAKE_AMOUNT:
                            shakeModX = 0;
                            break;
                    }
                    shakeTimer = 0;
                }
                shakeTimeLeft -= deltaTime;
            }
        }
    }
}
