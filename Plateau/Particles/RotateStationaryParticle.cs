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
    class RotateStationaryParticle : Particle
    {
        public RotateStationaryParticle(Vector2 source, Texture2D particleShape, Color particleColor, float duration) : base(source, particleShape, particleColor, duration)
        {
            this.velocityY = Util.RandInt(5, 5) / 10.0f;
            this.velocityX = Util.RandInt(-5, 5) / 10.0f;
            this.rotationSpeed = Util.RandInt(-12, 12) / 100.0f;
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);

            this.position.Y += velocityY * deltaTime;
            if (CollisionHelper.CheckCollision(new RectangleF(this.position - new Vector2(0, particleShape.Height), new Size2(particleShape.Width, particleShape.Height)), area, velocityY >= 0))
            {
                velocityY = 0;
            }

            this.position.X += velocityX * deltaTime;
            if (CollisionHelper.CheckCollision(new RectangleF(this.position - new Vector2(0, particleShape.Height), new Size2(particleShape.Width, particleShape.Height)), area, velocityY >= 0))
            {
                velocityX = 0;
            }
        }
    }
}
