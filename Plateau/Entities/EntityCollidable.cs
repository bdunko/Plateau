using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public abstract class EntityCollidable : Entity
    {
        public EntityCollidable(Vector2 position, DrawLayer drawLayer) : base(position, drawLayer)
        {
            //nothing
        }

        protected abstract void OnXCollision();
        protected abstract void OnYCollision();

        public virtual bool IsColliding(RectangleF solidRect)
        {
            return solidRect.Intersects(GetCollisionRectangle());
        }

        public virtual bool IsRiding(RectangleF solidRect)
        {
            RectangleF ridingHitbox = GetCollisionRectangle();
            ridingHitbox.Y -= 0.5f;
            return solidRect.Intersects(ridingHitbox) && GetCollisionRectangle().Bottom - 0.2f < solidRect.Top;
        }

        public virtual bool PushX(float pushX, Area area)
        {
            RectangleF collisionBox = GetCollisionRectangle();
            collisionBox.X += pushX;
            if (CollisionHelper.CheckCollision(collisionBox, area, true)) //if next movement = collision
            {
                OnXCollision();
                return false;
            }
            else
            {
                this.position.X += pushX;
                return true;
            }
        }

        public virtual bool PushY(float pushY, Area area)
        {
            RectangleF collisionBox = GetCollisionRectangle();
            collisionBox.Y += pushY;
            if (CollisionHelper.CheckCollision(collisionBox, area, true)) //if next movement = collision
            {
                OnYCollision();
                return false;
            }
            else
            {
                this.position.Y += pushY;
                return true;
            }
        }
    }
}
