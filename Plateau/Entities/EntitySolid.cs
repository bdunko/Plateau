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
    public class EntitySolid : Entity
    {
        public enum PlatformType
        {
            SOLID, BRIDGE, AIR
        }

        private static int STEPS = 6;

        private bool isOverlapping;
        protected Vector2 velocity;
        private EntityPlayer player;
        private PlatformType type;
        private bool collideWithTerrain;
        private AnimatedSprite sprite;

        public EntitySolid(Vector2 position, AnimatedSprite sprite, EntityPlayer player, PlatformType type, bool collideWithTerrain) : base(position, DrawLayer.NORMAL)
        {
            this.sprite = sprite;
            this.type = type;
            this.player = player;
            this.velocity = new Vector2(0, 0);
            this.isOverlapping = false;
            this.collideWithTerrain = collideWithTerrain;
        }

        public PlatformType GetPlatformType()
        {
            if(isOverlapping)
            {
                return PlatformType.AIR;
            }
            return type;
        }

        public bool IsGrabbable()
        {
            if(isOverlapping)
            {
                return false;
            }
            return type == PlatformType.SOLID;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, position, isOverlapping ? Color.LightGray * 0.8f : Color.White);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position.X, position.Y, sprite.GetFrameWidth(), sprite.GetFrameHeight());
        }

        protected virtual void OnPlayerContact()
        {

        }

        protected virtual void OnPlayerRide()
        {

        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);

            float stepX = (velocity.X * deltaTime) / STEPS;
            float stepY = (velocity.Y * deltaTime) / STEPS;

            for (int i = 0; i < STEPS; i++)
            {
                if (stepX != 0)
                {
                    RectangleF collisionBox = GetCollisionRectangle();
                    collisionBox.X += stepX;
                    if (!(collideWithTerrain && CollisionHelper.CheckCollision(collisionBox, area, stepY > 0, this)))
                    {
                        position.X += stepX;

                        foreach (EntityCollidable collideEn in area.GetCollideableEntities())
                        {
                            if (collideEn.IsRiding(GetCollisionRectangle()))
                            {
                                if (!isOverlapping)
                                {
                                    collideEn.PushX(stepX, area);
                                }
                            }
                            else if (collideEn.IsColliding(GetCollisionRectangle()) && type == PlatformType.SOLID)
                            {
                                if (!isOverlapping)
                                {
                                    bool pushSuccess = collideEn.PushX(stepX, area);
                                    if (!pushSuccess)
                                    {
                                        isOverlapping = true;
                                    }
                                }
                            }
                        }

                        if (player.IsRiding(GetCollisionRectangle()))
                        {
                            if (!isOverlapping)
                            {
                                player.PushX(stepX, area);
                                OnPlayerRide();
                            }
                        }
                        else if (player.IsColliding(GetCollisionRectangle()) && type == PlatformType.SOLID)
                        {
                            if (!isOverlapping)
                            {
                                bool pushSuccess = player.PushX(stepX, area);
                                OnPlayerContact();

                                if (!pushSuccess)
                                {
                                    isOverlapping = true;
                                }
                            }
                        }
                    } else
                    {
                        velocity.X = 0;
                    }
                }
                if (stepY != 0)
                {
                    RectangleF collisionBox = GetCollisionRectangle();
                    RectangleF stepYCollisionBox = new RectangleF(collisionBox.X, collisionBox.Y + stepY, collisionBox.Width, collisionBox.Height);
                    if (!(collideWithTerrain && CollisionHelper.CheckCollision(collisionBox, area, stepY > 0, this)))
                    {
                        position.Y += stepY;
                        foreach (EntityCollidable collideEn in area.GetCollideableEntities())
                        {
                            if (collideEn.IsRiding(GetCollisionRectangle()))
                            {
                                if (!isOverlapping)
                                {
                                    bool pushSuccess = collideEn.PushY(stepY, area);
                                    if (!pushSuccess && stepY < 0)
                                    {
                                        isOverlapping = true;
                                    }
                                }
                            }
                            else if (collideEn.IsColliding(GetCollisionRectangle()) && type == PlatformType.SOLID)
                            {
                                bool pushSuccess = collideEn.PushY(stepY, area);
                                if (!pushSuccess)
                                {
                                    isOverlapping = true;
                                }
                            }
                        }

                        if (player.IsRiding(GetCollisionRectangle()))
                        {
                            if (!isOverlapping)
                            {
                                bool pushSuccess = player.PushY(stepY, area);
                                OnPlayerRide();
                                if (!pushSuccess && stepY < 0)
                                {
                                    isOverlapping = true;
                                }
                            }
                        }
                        else if (player.IsColliding(GetCollisionRectangle()) && type == PlatformType.SOLID)
                        {
                            if (!isOverlapping)
                            {
                                bool pushSuccess = player.PushY(stepY, area);
                                OnPlayerContact();
                                if (!pushSuccess)
                                {
                                    isOverlapping = true;
                                }
                            }
                        }
                    } else
                    {
                        //velocity.Y = 0;
                    }
                }
            }

            if(isOverlapping) {
                bool collidingNothingThisFrame = true;
                foreach (EntityCollidable collideEn in area.GetCollideableEntities())
                {
                    if(collideEn.IsColliding(this.GetCollisionRectangle()))
                    {
                        collidingNothingThisFrame = false;
                        break;
                    } 
                }
                if(player.IsColliding(GetCollisionRectangle()))
                {
                    collidingNothingThisFrame = false;
                } 
                

                if (collidingNothingThisFrame)
                {
                    isOverlapping = false;
                }
            }
        }
    }
}
