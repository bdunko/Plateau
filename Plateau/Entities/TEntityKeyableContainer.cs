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
    class TEntityKeyableContainer : TileEntity, IInteract, ITickDaily
    {
        protected Texture2D texture;
        private Texture2D harvestedTexture;
        protected LootTables.LootTable lootTable;
        private Color particleColor1;
        private Color particleColor2;
        private EntityType type;
        private bool harvested;
        private float refreshChance;
        private Item key;

        private static float SHAKE_DURATION = 0.19f;
        private static float SHAKE_INTERVAL = 0.06f;
        private const float SHAKE_AMOUNT = 0.75f;
        private float shakeTimeLeft;
        private float shakeTimer;
        private float shakeModX;

        public TEntityKeyableContainer(Texture2D texture, Texture2D harvestedTexture, Vector2 tilePosition, EntityType type, Color particle1, Color particle2, Item key, LootTables.LootTable lootTable, float refreshChance) : base(tilePosition, texture.Width / 8, texture.Height / 8, DrawLayer.NORMAL)
        {
            this.texture = texture;
            this.harvestedTexture = harvestedTexture;
            this.position.Y += 1;
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
            this.key = key;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(harvested ? harvestedTexture : texture, position + new Vector2(shakeModX, 0), texture.Bounds, Color.White);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(texture.Width, texture.Height));
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (harvested)
            {
                return "";
            }
            return "Unlock";
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
            if (!harvested)
            {
                Item heldItem = player.GetHeldItem().GetItem();
                if (heldItem == key || heldItem == ItemDict.SKELETON_KEY)
                {
                    player.GetHeldItem().Subtract(1);
                    shakeTimeLeft = SHAKE_DURATION;
                    List<Item> drops = lootTable.RollLoot(player, area, world.GetTimeData());
                    foreach (Item drop in drops)
                    {
                        area.AddEntity(new EntityItem(drop, new Vector2(position.X, position.Y - 10)));
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(0, 3), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                            particleColor1, ParticleFactory.DURATION_LONG));
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(0, 3), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                            particleColor2, ParticleFactory.DURATION_LONG));
                    }
                    harvested = true;
                    GameState.STATISTICS[GameState.STAT_CHESTS_OPENED] += 1;
                }
                else
                {
                    if (heldItem == ItemDict.NONE)
                    {
                        player.AddNotification(new EntityPlayer.Notification("The chest is locked. I'll need to find a key...", Color.Black, EntityPlayer.Notification.Length.SHORT));
                    }
                    player.AddNotification(new EntityPlayer.Notification("This doesn't seem to fit the lock...", Color.Black, EntityPlayer.Notification.Length.SHORT));
                }
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            //do nothing
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
            }
        }
    }
}
