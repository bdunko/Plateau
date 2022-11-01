using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;

namespace Plateau.Entities
{
    public class EntityMovingPlatform : EntitySolid
    {
        public EntityMovingPlatform(Vector2 position, AnimatedSprite sprite, EntityPlayer player, PlatformType type, Vector2 initialVelocity, bool collideWithTerrain) : base(position, sprite, player, type, collideWithTerrain)
        {
            this.velocity = initialVelocity;
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);

            Area.MovingPlatformDirectorZone directorZone = area.GetDirectorZoneAt(GetCollisionRectangle());
            if(directorZone != null)
            {
                this.velocity = directorZone.newVelocity;
            }
        }
    }
}
