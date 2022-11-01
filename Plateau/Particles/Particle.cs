using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Plateau.Particles
{
    public abstract class Particle
    {
        public Vector2 position;
        protected Texture2D particleShape;
        protected Color particleColor;
        public float velocityX, velocityY;
        private float duration;
        protected float timeSinceCreation;
        protected float rotationSpeed;
        private float angle;

        public Particle(Vector2 source, Texture2D particleShape, Color particleColor, float duration)
        {
            this.position = source;
            this.particleShape = particleShape;
            this.particleColor = particleColor;
            this.duration = duration;
            this.rotationSpeed = 0;
            this.angle = 3.14f/2;
        }

        public void Draw(SpriteBatch sb, float layerDepth)
        {
            if(timeSinceCreation < duration/2)
            {
                sb.Draw(particleShape, position, new Rectangle(0, 0, particleShape.Height, particleShape.Width), particleColor, angle, new Vector2(particleShape.Width/2, particleShape.Height/2), 1.0f, SpriteEffects.None, layerDepth);
            } else
            {
                sb.Draw(particleShape, position, new Rectangle(0, 0, particleShape.Height, particleShape.Width), particleColor * (1f - ((timeSinceCreation - duration / 2) / (duration / 2))), 
                    angle, new Vector2(particleShape.Width / 2, particleShape.Height / 2), 1.0f, SpriteEffects.None, layerDepth);
            }
            
        }

        public Vector2 GetPosition()
        {
            return this.position;
        }

        public void NotifyDispose()
        {
            this.duration = 1.5f;
            this.timeSinceCreation = 0.75f;
        }

        public bool IsDisposable()
        {
            return timeSinceCreation >= duration;
        }

        public virtual void Update(float deltaTime, Area area)
        {
            timeSinceCreation += deltaTime;
            angle += rotationSpeed;
        }
    }
}
