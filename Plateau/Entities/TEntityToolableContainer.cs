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
    class TEntityToolableContainer : TileEntity, IInteract, ITickDaily, IInteractTool, IHaveHealthBar
    {
        protected Texture2D texture;
        private Texture2D harvestedTexture;
        protected LootTables.LootTable lootTable;
        private Color particleColor1;
        private Color particleColor2;
        private EntityType type;
        private bool harvested;
        private float refreshChance;
        private Item.Tag toolUsed;
        private HealthBar healthBar;

        private static float SHAKE_DURATION = 0.19f;
        private static float SHAKE_INTERVAL = 0.06f;
        private const float SHAKE_AMOUNT = 0.75f;
        private float shakeTimeLeft;
        private float shakeTimer;
        private float shakeModX;

        public TEntityToolableContainer(Texture2D texture, Texture2D harvestedTexture, Vector2 tilePosition, EntityType type, Item.Tag toolUsed, HealthBar hb, Color particle1, Color particle2, LootTables.LootTable lootTable, float refreshChance) : base(tilePosition, texture.Width / 8, texture.Height / 8, DrawLayer.NORMAL)
        {
            this.texture = texture;
            this.harvestedTexture = harvestedTexture;
            this.tilePosition = tilePosition;
            this.type = type;
            this.lootTable = lootTable;
            this.particleColor1 = particle1;
            this.particleColor2 = particle2;
            this.harvested = false;
            this.refreshChance = refreshChance;
            this.shakeTimer = 0;
            this.shakeModX = 0;
            this.shakeTimeLeft = 0;
            this.healthBar = hb;
            this.toolUsed = toolUsed;
            this.healthBar.SetPosition(this.position + new Vector2((texture.Width / 2) - (healthBar.GetWidth() / 2), -8));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(harvested ? harvestedTexture : texture, position + new Vector2(shakeModX, 1), texture.Bounds, Color.White);
        }

        public HealthBar GetHealthBar()
        {
            return this.healthBar;
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

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractTool(EntityPlayer player, Area area, World world)
        {
            if (!harvested)
            {
                Item tool = player.GetHeldItem().GetItem();
                bool rightTool = false;
                if (toolUsed == Item.Tag.PICKAXE_OR_AXE)
                {
                    if (tool.HasTag(Item.Tag.PICKAXE) || tool.HasTag(Item.Tag.AXE))
                    {
                        rightTool = true;
                    }
                }
                else
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
                        area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width / 2, texture.Height / 2), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            particleColor1, ParticleFactory.DURATION_LONG));
                    }

                    if (healthBar.IsDepleted())
                    {
                        List<Item> drops = lootTable.RollLoot(player, area, world.GetTimeData());
                        foreach (Item drop in drops)
                        {
                            area.AddEntity(new EntityItem(drop, new Vector2(position.X, position.Y - 10)));
                        }

                        for (int i = 0; i < 7; i++)
                        {
                            area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width / 2, texture.Height / 2), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                                particleColor1, ParticleFactory.DURATION_LONG));
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width / 2, texture.Height / 2), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                                particleColor2, ParticleFactory.DURATION_LONG));
                        }

                        if (toolUsed == Item.Tag.PICKAXE)
                        {
                            GameState.STATISTICS[GameState.STAT_ROCKS_MINED] += 1;
                        }
                        else if (toolUsed == Item.Tag.AXE)
                        {
                            GameState.STATISTICS[GameState.STAT_WOOD_CHOPPED] += 1;
                        }

                        harvested = true;
                    }
                }
                else
                {
                    //player.AddNotification(new EntityPlayer.Notification("Maybe with a different tool...", Color.Black, EntityPlayer.Notification.Length.SHORT));
                }
            }
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("entitytype", type.ToString());
            save.AddData("harvested", harvested.ToString());
            return save;
        }

        public override bool ShouldBeSaved()
        {
            return true;
        }
        public override void LoadSave(SaveState state)
        {
            harvested = state.TryGetData("harvested", false.ToString()) == true.ToString();
            base.LoadSave(state);
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
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

        public virtual void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            if (Util.RandInt(1, 100) / 100.0f <= refreshChance)
            {
                this.harvested = false;
                this.healthBar.Reset();
            }
        }
    }
}
