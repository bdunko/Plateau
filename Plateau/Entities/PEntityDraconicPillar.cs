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
    public class PEntityDraconicPillar : PlacedEntity, IInteract
    {
        private PartialRecolorSprite sprite;
        private static List<PEntityDraconicPillar> pillars;
        private Area.AreaEnum area;

        public PEntityDraconicPillar(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer, Area.AreaEnum area) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            sprite.AddLoop("placement", 0, 3, false);
            sprite.AddLoop("anim", 4, 7, false);
            sprite.SetLoop("placement");
            if (pillars == null)
            {
                pillars = new List<PEntityDraconicPillar>();
            }
            pillars.Add(this);
            this.area = area;
        }

        private Area.AreaEnum GetArea()
        {
            return this.area;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("anim");
            }
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            pillars.Remove(this);
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
            if (pillars.Count == 1)
            {
                player.AddNotification(new EntityPlayer.Notification("Place another Draconic Pillar to teleport to.", Color.Red));
            }
            else
            {
                PEntityDraconicPillar target = pillars[Util.RandInt(0, pillars.Count - 1)];
                while(target == this)
                {
                    target = pillars[Util.RandInt(0, pillars.Count - 1)];
                }
                Vector2 targetPosition = target.GetPosition();
                targetPosition.X += 4;
                targetPosition.Y -= 1;

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
