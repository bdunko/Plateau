using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class CollisionHelper
    {
        public static bool CheckForCollisionType(RectangleF hitbox, Area area, Area.CollisionTypeEnum type)
        {
            int indexXLeft = (int)(hitbox.X / 8);
            int indexYTop = (int)(hitbox.Y / 8);
            int indexXRight = (int)((hitbox.X + hitbox.Width) / 8);
            int indexYBottom = (int)((hitbox.Y - 1 + hitbox.Height) / 8);

            //Console.WriteLine(indexXLeft + " " + indexXRight + " " + indexYTop + " " + indexYBottom + " ");

            for (int indexXCurrent = indexXLeft; indexXCurrent <= indexXRight; indexXCurrent++)
            {
                for (int indexYCurrent = indexYTop; indexYCurrent <= indexYBottom; indexYCurrent++)
                {
                    Area.CollisionTypeEnum cType = area.GetCollisionTypeAt(indexXCurrent, indexYCurrent);
                    if (cType == type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool CheckCollision(RectangleF hitbox, Area area, bool falling)
        {
            return CheckCollision(hitbox, area, falling, null);
        }

        public static bool CheckCollision(RectangleF hitbox, Area area, bool falling, EntitySolid self)
        {
            int indexXLeft = (int)(hitbox.X / 8);
            int indexYTop = (int)(hitbox.Y / 8);
            int indexXRight = (int)((hitbox.X + hitbox.Width) / 8);
            int indexYBottom = (int)((hitbox.Y-1 + hitbox.Height) / 8);

            //Console.WriteLine(indexXLeft + " " + indexXRight + " " + indexYTop + " " + indexYBottom + " ");

            for (int indexXCurrent = indexXLeft; indexXCurrent <= indexXRight; indexXCurrent++)
            {
                for (int indexYCurrent = indexYTop; indexYCurrent <= indexYBottom; indexYCurrent++)
                {
                    Area.CollisionTypeEnum cType = area.GetCollisionTypeAt(indexXCurrent, indexYCurrent);
                    if (cType == Area.CollisionTypeEnum.SOLID || cType == Area.CollisionTypeEnum.SCAFFOLDING_BLOCK)
                    {
                        return true;
                    }
                    else if ((cType == Area.CollisionTypeEnum.BRIDGE || cType == Area.CollisionTypeEnum.SCAFFOLDING_BRIDGE) 
                        && falling 
                        && area.GetPositionOfTile(indexXCurrent, indexYCurrent).Y > hitbox.Bottom-3) //-3 seems to be a magic number - changing this may cause creatures, characters, particles to clip through bridges
                    {
                        Vector2 tilePos = area.GetPositionOfTile(indexXCurrent, indexYCurrent);
                        return true;
                    }
                }
            }

            //EntitySolids
            List<EntitySolid> solids = area.GetSolidEntities();
            foreach (EntitySolid solid in solids)
            {
                if (solid != self) //prevent collision on itself
                {
                    RectangleF solidHitbox = solid.GetCollisionRectangle();
                    if (solidHitbox.Intersects(hitbox) && solid.GetPlatformType() != EntitySolid.PlatformType.AIR)
                    {
                        if (solid.GetPlatformType() == EntitySolid.PlatformType.BRIDGE && falling && hitbox.Y < solidHitbox.Top)
                        {
                            return true;
                        }
                        else if (solid.GetPlatformType() == EntitySolid.PlatformType.SOLID)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }
}
