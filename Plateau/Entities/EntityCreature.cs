using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;

namespace Plateau.Entities
{
    public class EntityCreature : EntityCollidable, IInteract, ITickDaily, ITick
    {
        protected float speed = 0.3f;
        protected float jump_speed = 1.7f;
        protected static int COLLISION_STEPS = 3;
        protected bool triedJump;
        protected bool stationary;

        protected AnimatedSprite sprite;
        protected float velocityX, velocityY;
        protected DirectionEnum direction;
        protected bool grounded;

        public EntityCreature(AnimatedSprite sprite, Vector2 position, float speed, float jump_speed) : base(position, DrawLayer.PRIORITY)
        {
            this.speed = speed;
            this.jump_speed = jump_speed;
            this.sprite = sprite;
            this.grounded = false;
            this.velocityX = 0;
            this.velocityY = 0;
            this.triedJump = false;
            this.direction = DirectionEnum.LEFT;
            UpdateAnimation();
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, position, Color.White, layerDepth);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position.X, position.Y, sprite.GetFrameWidth(), sprite.GetFrameHeight());
        }

        protected void TurnToFace(EntityPlayer player)
        {
            if (player.GetCenteredPosition().X > position.X)
            {
                direction = DirectionEnum.RIGHT;
            }
            else
            {
                direction = DirectionEnum.LEFT;
            }
        }
        private void Jump()
        {
            if (grounded)
            {
                velocityY = -jump_speed;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            velocityY += World.GRAVITY * deltaTime;
            if (stationary)
            {
                velocityX = 0;
            } else
            {
                if (velocityX == 0 && Util.RandInt(0, 100) == 0)
                {
                    if (Util.RandInt(1, 2) == 1)
                    {
                        velocityX = speed;
                        direction = DirectionEnum.RIGHT;
                        velocityY = -0.6f;
                        triedJump = false;
                    }
                    else
                    {
                        velocityX = -speed;
                        direction = DirectionEnum.LEFT;
                        velocityY = -0.6f;
                        triedJump = false;
                    }
                }
                else
                {
                    if (Util.RandInt(0, 100) == 0 && grounded)
                    {
                        velocityX = 0;
                    }
                }
            }
            

            //calculate collisions
            RectangleF collisionBox = GetCollisionRectangle();
            float stepX = velocityX / COLLISION_STEPS;
            for (int step = 0; step < COLLISION_STEPS; step++)
            {
                if (stepX != 0) //move X
                {
                    collisionBox = GetCollisionRectangle();
                    RectangleF stepXCollisionBox = new RectangleF(collisionBox.X + stepX, collisionBox.Y, collisionBox.Width, collisionBox.Height);
                    bool xCollision = CollisionHelper.CheckCollision(stepXCollisionBox, area, true);
                    RectangleF stepXCollisionBoxForesight = new RectangleF(collisionBox.X + (stepX*18), collisionBox.Y, collisionBox.Width, collisionBox.Height);
                    bool xCollisionSoon = CollisionHelper.CheckCollision(stepXCollisionBoxForesight, area, true);
                    RectangleF stepXCollisionBoxBoundary = new RectangleF(collisionBox.X + (stepX*55), collisionBox.Y, collisionBox.Width, collisionBox.Height);
                    bool xCollisionBoundary = CollisionHelper.CheckForCollisionType(stepXCollisionBoxBoundary, area, Area.CollisionTypeEnum.BOUNDARY);


                    bool solidFound = false;
                    Vector2 tenativePos = this.position + new Vector2(stepX, 0);
                    if (direction == DirectionEnum.LEFT)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Area.CollisionTypeEnum collType = area.GetCollisionTypeAt((int)(tenativePos.X) / 8, ((int)(tenativePos.Y + sprite.GetFrameHeight()) / 8) + i);
                            if (collType == Area.CollisionTypeEnum.SOLID ||
                                collType == Area.CollisionTypeEnum.BRIDGE ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BRIDGE)
                            {
                                //make sure ground above target isn't water or deep water (topwater is allowed)
                                Area.CollisionTypeEnum collTypeAbove = area.GetCollisionTypeAt((int)(tenativePos.X) / 8, ((int)(tenativePos.Y + sprite.GetFrameHeight()) / 8) + (i-1));
                                if (collTypeAbove != Area.CollisionTypeEnum.WATER &&
                                    collTypeAbove != Area.CollisionTypeEnum.DEEP_WATER)
                                {
                                    solidFound = true;
                                } 
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Area.CollisionTypeEnum collType = area.GetCollisionTypeAt((int)(tenativePos.X + sprite.GetFrameWidth()) / 8, ((int)(tenativePos.Y + sprite.GetFrameHeight()) / 8) + i);
                            if (collType == Area.CollisionTypeEnum.SOLID ||
                                collType == Area.CollisionTypeEnum.BRIDGE ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BLOCK ||
                                collType == Area.CollisionTypeEnum.SCAFFOLDING_BRIDGE)
                            {
                                //make sure ground above target isn't water or deep water (topwater is allowed)
                                Area.CollisionTypeEnum collTypeAbove = area.GetCollisionTypeAt((int)(tenativePos.X + sprite.GetFrameWidth()) / 8, ((int)(tenativePos.Y + sprite.GetFrameHeight()) / 8) + (i-1));
                                if (collTypeAbove != Area.CollisionTypeEnum.WATER &&
                                    collTypeAbove != Area.CollisionTypeEnum.DEEP_WATER)
                                {
                                    solidFound = true;
                                }
                                break;
                            }
                        }
                    }

                    if (xCollisionSoon && !triedJump)
                    {
                        Jump();
                        triedJump = true;
                    }

                    if (xCollision || xCollisionBoundary || !solidFound) //if next movement = collision
                    {
                        if(!triedJump)
                        {
                            Jump();
                            triedJump = true;
                        }
                        stepX = 0; //stop moving if collision
                        if (grounded)
                        {
                            velocityX = 0;
                        }
                    } 
                    else
                    {
                        this.position.X += stepX;
                        triedJump = false;
                    }
                }
            }

            
            float stepY = velocityY / COLLISION_STEPS;
            for (int step = 0; step < COLLISION_STEPS; step++)
            {
                if (stepY != 0) //move Y
                {
                    collisionBox = GetCollisionRectangle();
                    RectangleF stepYCollisionBox = new RectangleF(collisionBox.X, collisionBox.Y + stepY, collisionBox.Width, collisionBox.Height);
                    bool yCollision = CollisionHelper.CheckCollision(stepYCollisionBox, area, stepY >= 0);

                    if (yCollision)
                    {
                        if(velocityY > 0)
                        {
                            grounded = true;
                        } 
                        stepY = 0;
                        velocityY = 0;
                        
                    }
                    else
                    {
                        this.position.Y += stepY;
                        grounded = false;
                    }
                }
            }

            UpdateAnimation();
        }

        public virtual string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public virtual string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public virtual string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public virtual string GetRightClickAction(EntityPlayer player)
        {
            return "";
        }

        protected virtual void UpdateAnimation()
        {
            if (velocityX == 0)
            {
                sprite.SetLoopIfNot(direction == DirectionEnum.LEFT? "idleL" : "idleR");
            } else
            {
                sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? "walkL" : "walkR");
            }
        }

        public virtual void InteractRight(EntityPlayer player, Area area, World world)
        {
            //talk TODO:
        }

        public virtual void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //nothing? use item?
        }

        public virtual void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            
        }

        public virtual void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            
        }

        public virtual void TickDaily(World world, Area area, EntityPlayer player)
        {
            //nothing yet
        }

        public virtual void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            //nothing yet
        }

        protected override void OnXCollision()
        {
            velocityX = 0;
        }

        protected override void OnYCollision()
        {
            velocityY = 0;
        }
    }
}
