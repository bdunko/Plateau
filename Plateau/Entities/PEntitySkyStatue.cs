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
    public class PEntitySkyStatue : PlacedEntity, IInteract
    {
        private PartialRecolorSprite sprite;
        private float timeSinceAnimation;
        private static float TIME_BETWEEN_ANIMATION = 2.0f;
        private static List<PEntitySkyStatue> skyStatues;
        private Area.AreaEnum area;

        public PEntitySkyStatue(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer, Area.AreaEnum area) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.AddLoop("anim", 4, 6, false);
            sprite.SetLoop("placement");
            this.timeSinceAnimation = 0;
            if (skyStatues == null)
            {
                skyStatues = new List<PEntitySkyStatue>();
            }
            skyStatues.Add(this);
            this.area = area;
        }

        private Area.AreaEnum GetArea()
        {
            return this.area;
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White);
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                timeSinceAnimation += deltaTime;
                if (timeSinceAnimation >= TIME_BETWEEN_ANIMATION)
                {
                    sprite.SetLoop("anim");
                    timeSinceAnimation = 0;
                }
            }
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            skyStatues.Remove(this);
            base.OnRemove(player, area, world);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Teleport";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            List<PEntitySkyStatue> statuesInThisArea = new List<PEntitySkyStatue>();
            foreach(PEntitySkyStatue statue in skyStatues)
            {
                if(statue == this || statue.GetArea() != this.GetArea())
                {
                    continue;
                }
                statuesInThisArea.Add(statue);
            }

            if(statuesInThisArea.Count == 0)
            {
                player.AddNotification(new EntityPlayer.Notification("Place another Sky Statue in this area to teleport to.", Color.Red));
            } else
            {
                PEntitySkyStatue target = statuesInThisArea[Util.RandInt(0, statuesInThisArea.Count-1)];
                Vector2 targetPosition = target.GetPosition();
                targetPosition.X += 4;
                targetPosition.Y -= 9;
                player.TransitionTo(world.GetAreaByEnum(target.GetArea()).GetAreaEnum().ToString(), targetPosition, Area.TransitionZone.Animation.TO_UP);
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {

        }
    }
}
