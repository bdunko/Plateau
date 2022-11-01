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
    class DriftUpwardParticle : Particle
    {
        public DriftUpwardParticle(Vector2 source, Texture2D particleShape, Color particleColor, float duration, bool strong, bool reversed) : base(source, particleShape, particleColor, duration)
        {
            this.velocityY = strong ? Util.RandInt(-120, -95) / 10.0f : Util.RandInt(-80, -65) / 10.0f;
            if(reversed)
            {
                velocityY = -velocityY;
            }
            this.velocityX = Util.RandInt(-5, 5) / 10.0f;
            this.rotationSpeed = 0;

        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);

            this.position.Y += velocityY * deltaTime;

            this.position.X += velocityX * deltaTime;
            if (CollisionHelper.CheckCollision(new RectangleF(this.position - new Vector2(0, particleShape.Height), new Size2(particleShape.Width, particleShape.Height)), area, velocityY >= 0))
            {
                velocityX = 0;
            }
        }
    }
}
