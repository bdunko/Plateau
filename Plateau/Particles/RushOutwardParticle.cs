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
    class RushOutwardParticle : Particle
    {
        public RushOutwardParticle(Vector2 source, Texture2D particleShape, Color particleColor, float duration, bool strong) : base(source, particleShape, particleColor, duration)
        {
            this.velocityY = Util.RandInt(10, 30) / 10.0f;
            this.velocityX = Util.RandInt(-30, 30) / 10.0f;
            if(velocityX >= 0)
            {
                velocityX += strong ? 220 : 20;
            } else
            {
                velocityX -= strong ? 220 : 20;
            }
            this.rotationSpeed = 0;

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
            if (CollisionHelper.CheckCollision(new RectangleF(this.position-new Vector2(0, particleShape.Height), new Size2(particleShape.Width, particleShape.Height)), area, velocityY >= 0))
            {
                velocityX = 0;
            }
        }
    }
}
