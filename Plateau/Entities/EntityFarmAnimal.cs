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
    public class EntityFarmAnimal : EntityCreature
    {
        public enum Type
        {
            COW, CHICKEN, SHEEP, PIG
        }

        private static float SPEED = 0.3f;
        private static float JUMP_SPEED = 1.7f;
        private static float HERD_SPEED = 0.3f;
        private static float HERD_TIME = 4.5f;

        private LootTables.LootTable lootTable;
        private Item harvestedWith;
        private bool harvestable;
        private float herdTimer;
        private Type creatureType;

        public EntityFarmAnimal(AnimatedSprite sprite, Vector2 position, LootTables.LootTable lootTable, Item harvestedWith, Type type) : base(sprite, position, SPEED, JUMP_SPEED)
        {
            this.herdTimer = 0;
            this.lootTable = lootTable;
            this.harvestedWith = harvestedWith;
            this.creatureType = type;
            if (creatureType == Type.PIG) {
                this.harvestable = false;
            } else {
                this.harvestable = true;
            }
        }

        public override RectangleF GetCollisionRectangle()
        {
            if(creatureType == Type.CHICKEN)
            {
                return new RectangleF(position.X+2, position.Y+6, 10, 10);
            } else if (creatureType == Type.PIG || creatureType == Type.COW)
            {
                return new RectangleF(position.X + 3, position.Y, 16, 16);
            }
            return new RectangleF(position.X, position.Y, sprite.GetFrameWidth(), sprite.GetFrameHeight());
        }

        public override void Update(float deltaTime, Area area)
        {
            if(herdTimer >= 0)
            {
                herdTimer -= deltaTime;
                if(herdTimer < 0)
                {
                    velocityX = 0;
                }
            }

            if(herdTimer < 0)
            {
                if(velocityX == 0 && Util.RandInt(0, 50) == 0)
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
                } else
                {
                    if (Util.RandInt(0, 100) == 0 && grounded)
                    {
                        velocityX = 0;
                    }
                }
            }

            base.Update(deltaTime, area);
        }

        public void SetHarvestable(bool harvestable)
        {
            this.harvestable = harvestable;
        }

        public bool IsHarvestable()
        {
            return harvestable;
        }

        public override string GetLeftClickAction(EntityPlayer player)
        {
            if (this.harvestable)
            {
                return "Gather";
            }
            return "";
        }

        public override string GetRightClickAction(EntityPlayer player)
        {
            return "Herd";
        }

        protected override void UpdateAnimation()
        {
            if (velocityX == 0)
            {
                if (creatureType == Type.SHEEP && !harvestable) {
                    sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? "idle2L": "idle2R");
                }
                else
                {
                    sprite.SetLoopIfNot(direction == DirectionEnum.LEFT? "idleL" : "idleR");
                }
            } else
            {
                if(creatureType == Type.SHEEP && !harvestable) {
                    sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? "walk2L" : "walk2R");
                } else
                {
                    sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? "walkL" : "walkR");
                }
             }
        }

        public override void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (player.GetDirection() == DirectionEnum.LEFT)
            {
                velocityX = -HERD_SPEED;
                herdTimer = HERD_TIME;
                direction = DirectionEnum.LEFT;
            }
            else
            {
                velocityX = HERD_SPEED;
                herdTimer = HERD_TIME;
                direction = DirectionEnum.RIGHT;
            }
        }

        public override void InteractLeft(EntityPlayer player, Area area, World world)
        {
            Item item = player.GetHeldItem().GetItem();
            if (harvestedWith != ItemDict.NONE && item == harvestedWith)
            {
                if (harvestable)
                {
                    List<Item> drops = lootTable.RollLoot(player, area, world.GetTimeData());
                    foreach (Item drop in drops)
                    {
                        area.AddEntity(new EntityItem(drop, new Vector2(position.X, position.Y - 10)));
                    }
                    harvestable = false;
                }
                else
                {
                    player.AddNotification(new EntityPlayer.Notification("I've already gathered from this one recently.", Color.Red));
                }
            }
            else
            {
                player.AddNotification(new EntityPlayer.Notification("I need the proper tool to do this.", Color.Red));
            }
            TurnToFace(player);
        }

        public override void TickDaily(World world, Area area, EntityPlayer player)
        {
            if(this.creatureType != Type.PIG)
            {
                harvestable = true;
            }
        }

        public override void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            if(creatureType == Type.PIG)
            {
                if (world.GetTimeData().timeOfDay != World.TimeOfDay.NIGHT)
                {
                    if (Util.RandInt(1, 55) == 1)
                    {
                        List<Item> drops = lootTable.RollLoot(player, area, world.GetTimeData());
                        foreach (Item drop in drops)
                        {
                            area.AddEntity(new EntityItem(drop, new Vector2(position.X, position.Y - 10)));
                        }
                    }
                }
            }
        }
    }
}
