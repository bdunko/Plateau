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
    class TEntityForage : TileEntity, IInteract, IDetonate
    {
        protected Texture2D texture;
        private LootTables.LootTable lootTable;
        private Color particleColor1;
        private Color particleColor2;
        private EntityType type;

        public TEntityForage(Texture2D texture, Vector2 tilePosition, EntityType type, Color particle1, Color particle2, LootTables.LootTable lootTable) : base(tilePosition, texture.Width / 8, texture.Height / 8, DrawLayer.NORMAL)
        {
            this.texture = texture;
            this.type = type;
            this.lootTable = lootTable;
            this.particleColor1 = particle1;
            this.particleColor2 = particle2;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position + new Vector2(0, 1), texture.Bounds, Color.White);
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
            return "Gather";
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

        bool harvested = false;

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            area.RemoveTileEntity(player, (int)tilePosition.X, (int)tilePosition.Y, world);
            if (!harvested)
            {
                List<Item> drops = lootTable.RollLoot(player, area, world.GetTimeData());
                foreach (Item drop in drops)
                {
                    area.AddEntity(new EntityItem(drop, new Vector2(position.X, position.Y - 10)));
                }
                //player.SetAnimLock
                for (int i = 0; i < 3; i++)
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width/2, texture.Height-0.5f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        particleColor1, ParticleFactory.DURATION_LONG));
                }
                for (int i = 0; i < 2; i++)
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width/2, texture.Height-0.5f), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.ONEXONE,
                        particleColor2, ParticleFactory.DURATION_LONG));
                }
                for (int i = 0; i < 2; i++)
                {
                    area.AddParticle(ParticleFactory.GenerateParticle(this.position + new Vector2(texture.Width/2, texture.Height-1), ParticleBehavior.BOUNCE_DOWN, ParticleTextureStyle.SMALL,
                        area.GetSecondaryColorForTile((int)tilePosition.X, (int)tilePosition.Y + tileHeight), ParticleFactory.DURATION_LONG));
                }
                harvested = true;
                GameState.STATISTICS[GameState.STAT_FORAGE_COLLECTED] += 1;
                player.PlayHarvestAnimation();
            }
        }

        public void Detonate(EntityPlayer player, Area area, World world)
        {
            InteractRight(player, area, world);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("entitytype", type.ToString());
            return save;
        }

        public override bool ShouldBeSaved()
        {
            return true;
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public override void Update(float deltaTime, Area area)
        {
            //do nothing...
        }
    }
}
