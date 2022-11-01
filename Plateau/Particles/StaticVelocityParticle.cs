using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Particles
{
    class StaticVelocityParticle : Particle
    {
        private bool checkCollisions;
        public StaticVelocityParticle(Vector2 source, Texture2D particleShape, Color particleColor, float duration, Vector2 velocity, bool rotates, bool checkCollisions) : base(source, particleShape, particleColor, duration)
        {
            this.velocityX = velocity.X;
            this.velocityY = velocity.Y;
            this.rotationSpeed = rotates ? Util.RandInt(-12, 12) / 100.0f : 0;
            this.checkCollisions = checkCollisions;
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);

            if (checkCollisions)
            {
                this.position.Y += velocityY * deltaTime;
                if (CollisionHelper.CheckCollision(new RectangleF(this.position - new Vector2(0, particleShape.Height), new Size2(particleShape.Width, particleShape.Height)), area, velocityY >= 0))
                {
                    velocityY = 0;
                    timeSinceCreation = 100000;
                }

                this.position.X += velocityX * deltaTime;
                if (CollisionHelper.CheckCollision(new RectangleF(this.position - new Vector2(0, particleShape.Height), new Size2(particleShape.Width, particleShape.Height)), area, velocityY >= 0))
                {
                    velocityX = 0;
                    timeSinceCreation = 100000;
                }
            } else
            {
                this.position.Y += velocityY * deltaTime;
                this.position.X += velocityX * deltaTime;
            }
        }
    }
}
