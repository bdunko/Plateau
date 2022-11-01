using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;

namespace Plateau.Particles
{
    public class BounceDownParticle : Particle
    {
        private static float GRAVITY_Y = 240.0f;
        private static float FRICTION_X =2f;
        private static float INITIAL_VELO_Y = -40.0f;
        private static int INITIAL_VELO_X_RANGE = 25;

        public BounceDownParticle(Vector2 source, Texture2D particleShape, Color particleColor, float duration) : base(source, particleShape, particleColor, duration)
        {
            this.velocityY = INITIAL_VELO_Y - Util.RandInt(0, 650)/10.0f;
            this.velocityX = Util.RandInt(-INITIAL_VELO_X_RANGE * 10, INITIAL_VELO_X_RANGE * 10) / 10.0f;
            this.rotationSpeed = Util.RandInt(-8, 8) / 100.0f;
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);
            if (CollisionHelper.CheckCollision(new RectangleF(this.position, new Size2(1, 1)), area, velocityY >= 0)) {
                velocityX = Util.RandInt(-1, 1) * 0.7f * velocityX;
                velocityY = -0.5f * velocityY;
                rotationSpeed = rotationSpeed / 2;
            }
            this.position.Y += velocityY * deltaTime;
            this.position.X += velocityX * deltaTime;
            velocityX = Util.AdjustTowards(velocityX, 0, FRICTION_X * deltaTime);
            velocityY += GRAVITY_Y * deltaTime;
        }
    }
}
