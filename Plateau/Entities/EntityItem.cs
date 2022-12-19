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
    class EntityItem : Entity, ITickDaily
    {
        private Item itemForm;

        private float velocityY;
        private float velocityX;
        private static float GRAVITY = 8;
        private static float FRICTION_X_AIRBORNE = 0.6f;
        private static float FRICTION_X_GROUNDED = 1.5f;
        private static float MAXIMUM_BOUNCE = -2.8f;

        private static float WATER_FLOAT_VELOCITY_Y = -10.0f; //speed at which objects float up in water
        private static float MAXIMUM_WATER_UPWARD_VELOCITY = -2.0f;

        private float timeElapsed;

        private static float TIME_BEFORE_COLLECTION = 0.55f;
        private static int COLLISION_STEPS = 3;
        private static float MINIMUM_BOUNCE = 1.0f;
        private static float BOUNCE_MULTIPLIER = 0.6f;

        private bool firstUpdate;
        private bool grounded;

        public EntityItem(Item itemForm, Vector2 centerPosition, Vector2 velocityReplacement) : this(itemForm, centerPosition)
        {
            //adjust such that the placeable spawns centered on the given position
            position.X -= 8;
            position.Y -= 8;
            if (velocityReplacement.X != 0)
            {
                velocityX = velocityReplacement.X;
            }
            if (velocityReplacement.Y != 0)
            {
                velocityY = velocityReplacement.Y;
            }
        }

        public EntityItem(Item itemForm, Vector2 position) : base(position, DrawLayer.PRIORITY)
        {
            this.itemForm = itemForm;
            // this.grounded = false;
            velocityX = Util.RandInt(-40, 40) / 100.0f;
            velocityY = Util.RandInt(-31, -24) / 10.0f;
            this.timeElapsed = 0;
            this.firstUpdate = true;
            this.grounded = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            itemForm.Draw(sb, new Vector2(position.X, position.Y + 2), Color.White);
        }

        public bool CanBeCollected()
        {
            return timeElapsed >= TIME_BEFORE_COLLECTION;
        }

        public Item GetItemForm()
        {
            return itemForm;
        }

        //adjusts position to attempt to not be in solid terrain; called once during the very first update
        private void PerformInitialAdjustment(Area area)
        {
            RectangleF hitbox = new RectangleF(position.X + 6, position.Y + 11, 6, 5);

            if (CollisionHelper.CheckCollision(hitbox, area, false))
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        if (!CollisionHelper.CheckCollision(new RectangleF(hitbox.X + x, hitbox.Y + y, hitbox.Width, hitbox.Height), area, false))
                        {
                            position.X += x;
                            position.Y += y;
                            return;
                        }
                        else if (!CollisionHelper.CheckCollision(new RectangleF(hitbox.X + x, hitbox.Y - y, hitbox.Width, hitbox.Height), area, false))
                        {
                            position.X += x;
                            position.Y -= y;
                            return;
                        }
                        else if (!CollisionHelper.CheckCollision(new RectangleF(hitbox.X - x, hitbox.Y + y, hitbox.Width, hitbox.Height), area, false))
                        {
                            position.X -= x;
                            position.Y += y;
                            return;
                        }
                        else if (!CollisionHelper.CheckCollision(new RectangleF(hitbox.X - x, hitbox.Y - y, hitbox.Width, hitbox.Height), area, false))
                        {
                            position.X -= x;
                            position.Y -= y;
                            return;
                        }
                    }
                }
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            //try to move to prevent being stuck in ground
            if (firstUpdate)
            {
                PerformInitialAdjustment(area);
                firstUpdate = false;
            }

            timeElapsed += deltaTime;

            velocityX = Util.AdjustTowards(velocityX, 0, (grounded ? FRICTION_X_GROUNDED : FRICTION_X_AIRBORNE) * deltaTime);

            RectangleF waterCollisionHitbox = new RectangleF(position.X + 6, position.Y + 6, 6, 5);
            if (CollisionHelper.CheckSwimmingCollision(waterCollisionHitbox, area)) //if in water, apply gravity for water and buoyancy
            {
                velocityY += GRAVITY / 4 * deltaTime;
                velocityY += WATER_FLOAT_VELOCITY_Y * deltaTime;
            }
            else //apply gravity normally
            {
                velocityY += GRAVITY * deltaTime;
            }

            //prevents flying high out of water on way up
            if (CollisionHelper.CheckTopWaterCollision(waterCollisionHitbox, area) && velocityY < MAXIMUM_WATER_UPWARD_VELOCITY)
                velocityY = MAXIMUM_WATER_UPWARD_VELOCITY;

            //converges back towards no/little movement
            if (CollisionHelper.CheckTopWaterCollision(waterCollisionHitbox, area) && Math.Abs(velocityY) <= 1 && Math.Abs(velocityY) >= 0.5f)
                velocityY *= 0.9f;

            //calculate collisions
            float stepX = velocityX / COLLISION_STEPS;
            float stepY = velocityY / COLLISION_STEPS;

            for (int step = 0; step < COLLISION_STEPS; step++)
            {
                if (stepX != 0) //move X
                {
                    bool xCollision = CollisionHelper.CheckCollision(new RectangleF(position.X + 6 + stepX, position.Y + 11, 6, 5), area, stepY >= 0);

                    if (xCollision) //if next movement = collision
                    {
                        stepX = 0; //stop moving if collision
                    }
                    else
                    {
                        this.position.X += stepX;
                    }
                }
                if (stepY != 0) //move Y
                {
                    bool yCollision = CollisionHelper.CheckCollision(new RectangleF(position.X + 6, position.Y + 11 + stepY, 6, 5), area, stepY >= 0);
                    if (yCollision)
                    {
                        stepY = 0;
                        grounded = true;
                        if (velocityY > MINIMUM_BOUNCE)
                        {
                            velocityY = -velocityY * BOUNCE_MULTIPLIER;
                            if (velocityY < MAXIMUM_BOUNCE)
                                velocityY = MAXIMUM_BOUNCE;
                            //velocityX -= velocityX / 3;
                        }
                        else
                        {
                            velocityY = 0;
                        }
                    }
                    else
                    {
                        grounded = false;
                        this.position.Y += stepY;
                    }
                }
            }
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position.X - 1, position.Y - 1, 14, 14);
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            if (itemForm.HasTag(Item.Tag.NO_TRASH))
            {
                if (!GameState.LOST_ITEMS.Contains(itemForm)) //allows removal of duplicate nodrop items; which is possible through shrine of creation giving extra tools
                {
                    GameState.LOST_ITEMS.Add(itemForm); //prevent losing key items
                    GameState.SetFlag(GameState.FLAG_LETTER_LOST_ITEMS, 1);
                }
            }
            //despawn
            area.RemoveEntity(this);
        }
    }
}