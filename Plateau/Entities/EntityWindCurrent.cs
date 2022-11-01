using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Particles;
using Plateau.Items;

namespace Plateau.Entities
{
    public class EntityWindCurrent : Entity, IInteractContact, ITick
    {
        private static float PARTICLES_PER_PX = 0.01f;

        private List<Particle> particles;
        private RectangleF windRectangle;
        private RectangleF topRectangle;
        private Color particle1, particle2;
        private World.Season season;

        public EntityWindCurrent(RectangleF fullRectangle, Area area, World.Season season) : base(fullRectangle.TopLeft, DrawLayer.NORMAL)
        {
            this.windRectangle = fullRectangle;
            this.particles = new List<Particle>();
            this.season = season;
            this.topRectangle = new RectangleF(new Point2(fullRectangle.X-1, fullRectangle.Y-16), new Size2(fullRectangle.Width+2, 16));
            SetColors();
        }

        private void SetColors()
        {
            switch(season)
            {
                case World.Season.SPRING:
                case World.Season.NONE:
                case World.Season.DEFER:
                    particle1 = Util.PARTICLE_SPRING_PETAL_FOREGROUND.color;
                    particle2 = Util.PARTICLE_SPRING_PETAL_BACKGROUND.color;
                    break;
                case World.Season.SUMMER:
                    particle1 = Util.PARTICLE_SUMMER_LEAF_FOREGROUND.color;
                    particle2 = Util.PARTICLE_SUMMER_LEAF_BACKGROUND.color;
                    break;
                case World.Season.AUTUMN:
                    particle1 = Util.PARTICLE_FALL_LEAF_FOREGROUND.color;
                    particle2 = Util.PARTICLE_FALL_LEAF_BACKGROUND.color;
                    break;
                case World.Season.WINTER:
                    particle1 = Util.PARTICLE_WINTER_SNOW_FOREGROUND.color;
                    particle2 = Util.PARTICLE_WINTER_SNOW_BACKGROUND.color;
                    break;
            }
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {

        }

        public override RectangleF GetCollisionRectangle()
        {
            return windRectangle;
        }

        private void PopulateParticles(Area area)
        {
            for (int i = 0; i < ((int)(PARTICLES_PER_PX * windRectangle.Width * windRectangle.Height)); i++)
            {
                Vector2 particlePosition = new Vector2(Util.RandInt((int)windRectangle.Left, (int)windRectangle.Right), Util.RandInt((int)windRectangle.Top, (int)windRectangle.Bottom));
                Particle particle = ParticleFactory.GenerateStaticVelocityParticle(particlePosition, ParticleTextureStyle.ONEXONE, Util.RandInt(1, 2) == 1 ? particle1 : particle2, ParticleFactory.DURATION_INFINITE, new Vector2(0, Util.RandInt(-360, -320) / 10.0f), true, false);
                particles.Add(particle);
                area.AddParticle(particle);
            }
        }

        private void ClearParticles()
        {
            foreach(Particle particle in particles)
            {
                particle.NotifyDispose();
            }
            particles.Clear();
        }

        public void OnContact(EntityPlayer player, Area area)
        {
            if((season == World.Season.NONE || season == World.Season.DEFER || area.GetWorldSeason() == season) && player.IsGliding() && player.GetSailcloth().GetItem() != ItemDict.CLOTHING_NONE)
            {
                if (player.GetCollisionRectangle().Intersects(topRectangle))
                {
                    player.SetVelocityY(0);
                    player.SkipGravityThisFrame();
                }
                else
                {
                    player.SetVelocityY(-0.9f);
                    player.OverwriteGlideThisFrame();
                }
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            if (!(season == World.Season.NONE || season == World.Season.DEFER) && area.GetWorldSeason() != season)
            {
                ClearParticles();
            }
            else
            {
                if (particles.Count == 0)
                {
                    PopulateParticles(area);
                }

                List<Particle> toRemove = new List<Particle>();
                foreach (Particle part in particles)
                {
                    if (topRectangle.Contains(part.GetPosition()))
                    {
                        part.NotifyDispose();
                        toRemove.Add(part);
                    }
                }

                foreach (Particle part in toRemove)
                {
                    particles.Remove(part);
                }

                for (int i = 0; i < toRemove.Count; i++)
                {
                    Vector2 particlePosition = new Vector2(Util.RandInt((int)windRectangle.Left, (int)windRectangle.Right), (int)windRectangle.Bottom - 1);
                    Particle particle = ParticleFactory.GenerateStaticVelocityParticle(particlePosition, ParticleTextureStyle.ONEXONE, Util.RandInt(1, 2) == 1 ? particle1 : particle2, ParticleFactory.DURATION_INFINITE, new Vector2(0, Util.RandInt(-360, -320) / 10.0f), true, false);
                    particles.Add(particle);
                    area.AddParticle(particle);
                }
            }
        }

        public void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            if(world.GetCurrentArea() != area)
            {
                ClearParticles();
            }
        }
    }
}
