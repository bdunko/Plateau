using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Particles
{
    public enum ParticleBehavior
    {
        BOUNCE_DOWN, RUSH_OUTWARD, RUSH_OUTWARD_STRONG, RUSH_UPWARD, RUSH_UPWARD_STRONG, RUSH_UPWARD_REVERSED, RUSH_UPWARD_STRONG_REVERSED, ROTATE_STATIONARY, ROTATE_FALLING, DRIFT_UPWARD
    }

    public enum ParticleTextureStyle
    {
        ONEXONE, TWOXTWO, SMALL, ZIGGY, CHUNK, ONEXTWO
    }

    public class ParticleFactory
    {
        public static Texture2D particle1x1, particle2x2, particle1x2, particleL, particleD;
        public static Texture2D particleChunk1, particleChunk2, particleChunk3, particleChunk4, particleChunk5;
        public static Texture2D particleZig1, particleZig2, particleZig3, particleZig4, particleZig5;
        private static List<Texture2D> smallStyleTextures = new List<Texture2D>();
        private static List<Texture2D> ziggyStyleTextures = new List<Texture2D>();
        private static List<Texture2D> chunkStyleTextures = new List<Texture2D>();
        public static float DURATION_VERY_SHORT = 0.15f;
        public static float DURATION_SHORT = 0.25f;
        public static float DURATION_SHORT_FOOD = 0.35f; //used exclusively for effect when eating food/milk and applying boost
        public static float DURATION_MEDIUM = 0.5f;
        public static float DURATION_LONG = 1.5f;
        public static float DURATION_PRETTY_LONG = 3.0f;
        public static float DURATION_VERY_LONG = 10.0f;
        public static float DURATION_INFINITE = 1000.0f;

        public static void LoadContent(ContentManager content)
        {
            particle1x1 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_1x1);
            particle2x2 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_2x2);
            particle1x2 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_1x2);
            particleL = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_L);
            particleD = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_D);

            particleChunk1 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_CHUNK1);
            particleChunk2 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_CHUNK2);
            particleChunk3 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_CHUNK3);
            particleChunk4 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_CHUNK4);
            particleChunk5 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_CHUNK5);

            particleZig1 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_ZIG1);
            particleZig2 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_ZIG2);
            particleZig3 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_ZIG3);
            particleZig4 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_ZIG4);
            particleZig5 = content.Load<Texture2D>(Paths.SPRITE_PARTICLE_ZIG5);

            smallStyleTextures = new List<Texture2D>();
            smallStyleTextures.Add(particle1x1);
            smallStyleTextures.Add(particle1x2);
            smallStyleTextures.Add(particle2x2);
            smallStyleTextures.Add(particleL);
            smallStyleTextures.Add(particleD);

            chunkStyleTextures = new List<Texture2D>();
            chunkStyleTextures.Add(particleChunk1);
            chunkStyleTextures.Add(particleChunk2);
            chunkStyleTextures.Add(particleChunk3);
            chunkStyleTextures.Add(particleChunk4);
            chunkStyleTextures.Add(particleChunk5);

            ziggyStyleTextures = new List<Texture2D>();
            ziggyStyleTextures.Add(particleZig1);
            ziggyStyleTextures.Add(particleZig2);
            ziggyStyleTextures.Add(particleZig3);
            ziggyStyleTextures.Add(particleZig4);
            ziggyStyleTextures.Add(particleZig5);
        }

        private static Texture2D GetTextureForStyle(ParticleTextureStyle style)
        {
            switch(style)
            {
                case ParticleTextureStyle.ONEXONE:
                    return particle1x1;
                case ParticleTextureStyle.ONEXTWO:
                    return particle1x2;
                case ParticleTextureStyle.TWOXTWO:
                    return particle2x2;
                case ParticleTextureStyle.SMALL:
                    return smallStyleTextures[Util.RandInt(0, smallStyleTextures.Count-1)];
                case ParticleTextureStyle.CHUNK:
                    return chunkStyleTextures[Util.RandInt(0, chunkStyleTextures.Count - 1)];
                case ParticleTextureStyle.ZIGGY:
                    return ziggyStyleTextures[Util.RandInt(0, ziggyStyleTextures.Count - 1)];
            }
            return null;
        }

        public static Particle GenerateStaticVelocityParticle(Vector2 source, ParticleTextureStyle style, Color color, float duration, Vector2 velocity, bool rotates, bool checkCollisions)
        {
            return new StaticVelocityParticle(source, GetTextureForStyle(style), color, duration, velocity, rotates, checkCollisions);
        }

        public static Particle GenerateParticle(Vector2 source, ParticleBehavior behavior, ParticleTextureStyle style, Color color, float duration)
        {
            switch(behavior)
            {
                case ParticleBehavior.BOUNCE_DOWN:
                    return new BounceDownParticle(source, GetTextureForStyle(style), color, duration);
                case ParticleBehavior.RUSH_OUTWARD:
                    return new RushOutwardParticle(source, GetTextureForStyle(style), color, duration, false);
                case ParticleBehavior.RUSH_OUTWARD_STRONG:
                    return new RushOutwardParticle(source, GetTextureForStyle(style), color, duration, true);
                case ParticleBehavior.RUSH_UPWARD:
                    return new RushUpwardParticle(source, GetTextureForStyle(style), color, duration, false, false);
                case ParticleBehavior.DRIFT_UPWARD:
                    return new DriftUpwardParticle(source, GetTextureForStyle(style), color, duration, false, false);
                case ParticleBehavior.RUSH_UPWARD_STRONG:
                    return new RushUpwardParticle(source, GetTextureForStyle(style), color, duration, true, false);
                case ParticleBehavior.RUSH_UPWARD_REVERSED:
                    return new RushUpwardParticle(source, GetTextureForStyle(style), color, duration, false, true);
                case ParticleBehavior.RUSH_UPWARD_STRONG_REVERSED:
                    return new RushUpwardParticle(source, GetTextureForStyle(style), color, duration, true, true);
                case ParticleBehavior.ROTATE_STATIONARY:
                    return new RotateStationaryParticle(source, GetTextureForStyle(style), color, duration);
                case ParticleBehavior.ROTATE_FALLING:
                    return new RotateFallingParticle(source, GetTextureForStyle(style), color, duration);
            }
            return null;
        }

    }
}
