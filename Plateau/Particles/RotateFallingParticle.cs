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
    class RotateFallingParticle : Particle
    {
        private float fallingY;

        public RotateFallingParticle(Vector2 source, Texture2D particleShape, Color particleColor, float duration) : base(source, particleShape, particleColor, duration)
        {
            this.fallingY = Util.RandInt(480, 540) / 10.0f;
            this.velocityY = Util.RandInt(-170, -155) / 10.0f;
            this.velocityX = Util.RandInt(-15, 15) / 10.0f;
            this.rotationSpeed = Util.RandInt(-10, 10) / 100.0f;
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);

            this.velocityY += fallingY * deltaTime;
            if(this.velocityY > fallingY)
            {
                velocityY = fallingY;
            }
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

            if (CollisionHelper.CheckCollision(new RectangleF(this.position, new Size2(1, 1)), area, velocityY >= 0))
            {
                velocityX = 0;
                velocityY = 0;
                rotationSpeed = 0;
            }
        }
    }
}
